using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Species.Application.Database;
using PetFamily.Species.Application.Interfaces;
using PetFamily.Volunteers.Contracts;

namespace PetFamily.Species.Application.Commands.DeleteSpecie;

public class DeleteSpecieHandler : ICommandHandler<Guid,DeleteSpecieCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<DeleteSpecieHandler> _logger;
    private readonly IValidator<DeleteSpecieCommand> _validator;
    private readonly IVolunteerContracts _volunteerContracts;

    public DeleteSpecieHandler(
        [FromKeyedServices(ProjectConstants.Context.SpeciesManagement)] IUnitOfWork unitOfWork,
        ISpeciesRepository speciesRepository,
        ILogger<DeleteSpecieHandler> logger,
        IValidator<DeleteSpecieCommand> validator,
        IVolunteerContracts volunteerContracts)
    {
        _unitOfWork = unitOfWork;
        _speciesRepository = speciesRepository;
        _logger = logger;
        _validator = validator;
        _volunteerContracts = volunteerContracts;
    }
    
    public async Task<Result<Guid, CustomErrorsList>> Handle(
        DeleteSpecieCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(
            command, cancellationToken);
        
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        bool deleteFlag = await _volunteerContracts.IsAnyPetWithNeededSpecieExists(command.SpecieId,
            cancellationToken);

        if (deleteFlag)
            return Errors.General.DeleteFailure().ToErrorList();
        
        var specieResult = await _speciesRepository.GetById(command.SpecieId,cancellationToken);   
            
        await _speciesRepository.Delete(specieResult.Value, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Specie with id {specieId} was deleted.", specieResult.Value.Name);

        return command.SpecieId;
    }
}