using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Volunteers.Application.Database;
using PetFamily.Volunteers.Application.Interfaces;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Pet.Update.Status;

public class UpdatePetsStatusHandler : ICommandHandler<Guid,UpdatePetsStatusCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<UpdatePetsStatusHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdatePetsStatusCommand> _validator;
    private readonly IReadDbContext _readDbContext;

    public UpdatePetsStatusHandler(IVolunteerRepository volunteerRepository,
        ILogger<UpdatePetsStatusHandler> logger, 
        IUnitOfWork unitOfWork,
        IValidator<UpdatePetsStatusCommand> validator)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<Guid, CustomErrorsList>> Handle(UpdatePetsStatusCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerResult = await _volunteerRepository.GetById(command.VolunteerId, cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var newStatus = Enum.Parse<HelpStatusType>(command.NewStatus);

        var petToUpdate = volunteerResult.Value.CurrentPets.FirstOrDefault(p => p.Id == command.PetId);
        
        if(petToUpdate == null)
           return Errors.General.NotFound("pet not found").ToErrorList();
        
        petToUpdate.UpdateStatus(newStatus);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("For pet with Id: {id} was updated status", petToUpdate.Id);
        
        return volunteerResult.Value.Id.Value;
    }
}