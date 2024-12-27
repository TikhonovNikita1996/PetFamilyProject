using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using Pet.Family.SharedKernel.ValueObjects.Specie;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Species.Application.Interfaces;
using PetFamily.Species.Domain.ValueObjects;

namespace PetFamily.Species.Application.Commands.Create;

public class CreateSpecieHandler : ICommandHandler<Guid,CreateSpecieCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<CreateSpecieHandler> _logger;
    private readonly IValidator<CreateSpecieCommand> _validator;

    public CreateSpecieHandler(
        [FromKeyedServices(ProjectConstants.Context.SpeciesManagement)] IUnitOfWork unitOfWork,
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

        var newBreeds = command.Breeds.Select(breed => 
            new Breed(BreedId.NewId(),breed.Name)).ToList();
        
        var specie = Specie.Create(specieId, name, newBreeds).Value;

        await _speciesRepository.Add(specie, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Species added with id {specieId}.", specieId.Value);

        return specie.Id.Value;
    }
}