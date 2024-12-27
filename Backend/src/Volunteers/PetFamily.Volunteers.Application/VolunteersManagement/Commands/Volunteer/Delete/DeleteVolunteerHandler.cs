using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Volunteers.Application.Interfaces;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.Delete;

public class DeleteVolunteerHandler : ICommandHandler<Guid,DeleteVolunteerCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<DeleteVolunteerHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<DeleteVolunteerCommand> _validator;

    public DeleteVolunteerHandler(IVolunteerRepository volunteerRepository,
        ILogger<DeleteVolunteerHandler> logger,
        IUnitOfWork unitOfWork, IValidator<DeleteVolunteerCommand> validator)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    
    public async Task<Result<Guid, CustomErrorsList>> Handle(DeleteVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerResult = await _volunteerRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var result = await _volunteerRepository.Delete(volunteerResult.Value, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Volunteer with Id: {id} was deleted", volunteerResult.Value.Id);
        
        return result;
    }
}