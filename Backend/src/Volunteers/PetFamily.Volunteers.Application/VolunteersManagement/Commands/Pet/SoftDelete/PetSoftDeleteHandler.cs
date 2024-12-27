using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Volunteers.Application.Database;
using PetFamily.Volunteers.Application.Interfaces;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Pet.SoftDelete;

public class PetSoftDeleteHandler : ICommandHandler<Guid,PetSoftDeleteCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<PetSoftDeleteHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<PetSoftDeleteCommand> _validator;
    private readonly IReadDbContext _readDbContext;

    public PetSoftDeleteHandler(IVolunteerRepository volunteerRepository,
        ILogger<PetSoftDeleteHandler> logger, 
        IUnitOfWork unitOfWork,
        IValidator<PetSoftDeleteCommand> validator,
        IReadDbContext readDbContext)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _readDbContext = readDbContext;
    }

    public async Task<Result<Guid, CustomErrorsList>> Handle(PetSoftDeleteCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerResult = await _volunteerRepository.GetById(command.VolunteerId, cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var petToUpdate = volunteerResult.Value.CurrentPets.FirstOrDefault(p => p.Id == command.PetId);
        
        if(petToUpdate == null)
           return Errors.General.NotFound("pet not found").ToErrorList();
        
        petToUpdate.Delete();
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Pet with Id: {id} was deleted softly", petToUpdate.Id);
        
        return volunteerResult.Value.Id.Value;
    }
}