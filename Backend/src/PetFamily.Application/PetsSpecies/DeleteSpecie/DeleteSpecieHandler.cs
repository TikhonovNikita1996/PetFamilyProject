using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Extensions;
using PetFamily.Application.Interfaces;
using PetFamily.Application.PetsSpecies.Create;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Pet.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.PetsSpecies.DeleteSpecie;

public class DeleteSpecieHandler : ICommandHandler<Guid,DeleteSpecieCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<DeleteSpecieHandler> _logger;
    private readonly IValidator<DeleteSpecieCommand> _validator;
    private readonly IReadDbContext _readDbContext;

    public DeleteSpecieHandler(
        IUnitOfWork unitOfWork,
        ISpeciesRepository speciesRepository,
        ILogger<DeleteSpecieHandler> logger,
        IValidator<DeleteSpecieCommand> validator,
        IReadDbContext readDbContext)
    {
        _unitOfWork = unitOfWork;
        _speciesRepository = speciesRepository;
        _logger = logger;
        _validator = validator;
        _readDbContext = readDbContext;
    }
    
    public async Task<Result<Guid, CustomErrorsList>> Handle(
        DeleteSpecieCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var petsQuery = _readDbContext.Pets.AsQueryable();

        var firstPetWithSpecie = await petsQuery
            .SingleOrDefaultAsync(p => p.SpecieId == command.SpecieId, cancellationToken);
        
        if (firstPetWithSpecie != null)
            return Errors.General.DeleteFailure().ToErrorList();
        
        var specieResult = await _speciesRepository.GetById(command.SpecieId,cancellationToken);   
            
        await _speciesRepository.Delete(specieResult.Value, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Specie with id {specieId} was deleted.", specieResult.Value.Name);

        return command.SpecieId;
    }
}