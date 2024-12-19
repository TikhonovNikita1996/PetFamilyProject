using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.DataBase;
using PetFamily.Application.Extensions;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Pets.SetMainPhoto;

public class SetPetsMainPhotoHandler : ICommandHandler<Guid,SetPetsMainPhotoCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<SetPetsMainPhotoHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<SetPetsMainPhotoCommand> _validator;

    public SetPetsMainPhotoHandler(IVolunteerRepository volunteerRepository,
        ILogger<SetPetsMainPhotoHandler> logger, 
        IUnitOfWork unitOfWork,
        IValidator<SetPetsMainPhotoCommand> validator,
        IReadDbContext readDbContext)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<Guid, CustomErrorsList>> Handle(SetPetsMainPhotoCommand command,
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
        
        petToUpdate.SetMainPhoto(command.FilePath);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Pet with Id: {id} was deleted softly", petToUpdate.Id);
        
        return volunteerResult.Value.Id.Value;
    }
}