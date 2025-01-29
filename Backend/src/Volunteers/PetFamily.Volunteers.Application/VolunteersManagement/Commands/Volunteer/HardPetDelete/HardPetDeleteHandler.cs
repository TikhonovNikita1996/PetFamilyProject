using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using PetFamily.Core;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Core.Providers;
using PetFamily.Volunteers.Application.Interfaces;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.HardPetDelete;

public class HardPetDeleteHandler : ICommandHandler<Guid,HardPetDeleteCommand>
{
    private readonly IVolunteerRepository _volunteersRepository;
    private readonly IValidator<HardPetDeleteCommand> _validator;
    private readonly ILogger<HardPetDeleteHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;

    public HardPetDeleteHandler(
        ILogger<HardPetDeleteHandler> logger,
        IVolunteerRepository volunteersRepository,
        IValidator<HardPetDeleteCommand> validator,
        [FromKeyedServices(ProjectConstants.Context.VolunteerManagement)] IUnitOfWork unitOfWork,
        IFileService fileService)
    {
        _logger = logger;
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }

    public async Task<Result<Guid, CustomErrorsList>> Handle(HardPetDeleteCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var petToDelete = volunteerResult.Value.CurrentPets.FirstOrDefault(p => p.Id == command.PetId);
        if (petToDelete == null)
            return Errors.General.NotFound("pet").ToErrorList();
        
        volunteerResult.Value.DeletePet(petToDelete);

        var orderedPetsList = volunteerResult.Value.CurrentPets.OrderBy(p => p.PositionNumber.Value).ToList();
        volunteerResult.Value.UpdatePetsPositions(orderedPetsList);

        // Delete photos from minio
        if (petToDelete.Photos != null)
        {
            var photosMetaDataToDelete = petToDelete.Photos
                .Select(p => new FileMetaData("photos", FilePath.Create(p.FileId.ToString()).Value));
        
            foreach (var photoMetaData in photosMetaDataToDelete)
            {
                await _fileService.DeleteFileAsync(photoMetaData, cancellationToken);
            }
        }
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Pet with Id: {id} was deleted", command.PetId);
        
        return command.PetId;
    }
}