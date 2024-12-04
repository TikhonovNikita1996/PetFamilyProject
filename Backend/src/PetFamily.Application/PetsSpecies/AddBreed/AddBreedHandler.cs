using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.DataBase;
using PetFamily.Application.Extensions;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Pet.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.PetsSpecies.AddBreed;

public class AddBreedHandler
{
    private readonly IValidator<AddBreedCommand> _validator;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<AddBreedHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public AddBreedHandler(
        IValidator<AddBreedCommand> validator,
        ISpeciesRepository speciesRepository,
        ILogger<AddBreedHandler> logger, 
        IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _speciesRepository = speciesRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid, CustomErrorsList>> Handle(
        AddBreedCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var speciesResult = await _speciesRepository.GetById(command.SpecieId, cancellationToken);
        if(speciesResult.IsFailure)
            return speciesResult.Error.ToErrorList();
        
        var breedId = BreedId.NewId();
        
        var name = command.Name;
        
        var breed = new Breed(breedId, name);

        var addBreedResult = speciesResult.Value.AddBreed(breed);
        if (addBreedResult.IsFailure)
            return addBreedResult.Error.ToErrorList();
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Breed added with id {breedId}.", breedId.Value);

        return breed.Id.Value;
    }
}