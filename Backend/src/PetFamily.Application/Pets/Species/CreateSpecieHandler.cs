using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Pet.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Pets.Species;

public class CreateSpecieHandler
{
    private readonly IValidator<CreateSpecieCommand> _validator;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<CreateSpecieHandler> _logger;
    
    public CreateSpecieHandler(
        ISpeciesRepository speciesRepository,
        ILogger<CreateSpecieHandler> logger)
    {
        _speciesRepository = speciesRepository;
        _logger = logger;
    }
    
    public async Task<Result<Guid, CustomErrorsList>> Handle(
        CreateSpecieCommand request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var specieId = SpecieId.Create(Guid.NewGuid());
        var name = request.Name;

        var species = Specie.Create(specieId, name).Value;

        await _speciesRepository.Add(species, cancellationToken);
        
        _logger.LogInformation("Species added with id {specieId}.", specieId.Value);

        return species.Id.Value;
    }
}