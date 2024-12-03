using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Logging;
using PetFamily.Application.DataBase;
using PetFamily.Application.Extensions;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Pet.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.PetsSpecies.Create;

public class CreateSpecieHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<CreateSpecieHandler> _logger;
    private readonly IValidator<CreateSpecieCommand> _validator;

    public CreateSpecieHandler(
        IUnitOfWork unitOfWork,
        ISpeciesRepository speciesRepository,
        ILogger<CreateSpecieHandler> logger,
        IValidator<CreateSpecieCommand> validator)
    {
        _unitOfWork = unitOfWork;
        _speciesRepository = speciesRepository;
        _logger = logger;
        _validator = validator;
    }
    
    public async Task<Result<Guid, CustomErrorsList>> Handle(
        CreateSpecieCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var specieId = SpecieId.NewId();
        var name = command.Name;

        var newBreeds = new List<Breed>();
        
        var specie = Specie.Create(specieId, name, newBreeds).Value;

        await _speciesRepository.Add(specie, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Species added with id {specieId}.", specieId.Value);

        return specie.Id.Value;
    }
}