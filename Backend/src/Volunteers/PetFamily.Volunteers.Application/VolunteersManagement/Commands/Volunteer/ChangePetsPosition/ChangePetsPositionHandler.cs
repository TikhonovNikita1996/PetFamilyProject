using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Volunteers.Application.Interfaces;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.ChangePetsPosition;

public class ChangePetsPositionHandler : ICommandHandler<Guid,ChangePetsPositionCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<ChangePetsPositionHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<ChangePetsPositionCommand> _validator;

    public ChangePetsPositionHandler(IVolunteerRepository volunteerRepository,
        ILogger<ChangePetsPositionHandler> logger,
        [FromKeyedServices(ProjectConstants.Context.VolunteerManagement)]IUnitOfWork unitOfWork,
        IValidator<ChangePetsPositionCommand> validator)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<Guid, CustomErrorsList>> Handle(ChangePetsPositionCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerResult = await _volunteerRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var petResult = volunteerResult.Value.GetPetById(command.PetId);
        if (petResult.IsFailure)
            return petResult.Error.ToErrorList();
        
        volunteerResult.Value.ChangePetsPosition(petResult.Value, command.NewPosition);
        
        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Pet - {petId} moved to position {newPosition}",
            petResult.Value.Id.Value, command.NewPosition);
        
        return petResult.Value.Id.Value;
    }
}