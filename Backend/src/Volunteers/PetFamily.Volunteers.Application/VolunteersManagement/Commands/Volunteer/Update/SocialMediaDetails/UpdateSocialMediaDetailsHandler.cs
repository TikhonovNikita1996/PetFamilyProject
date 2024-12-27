using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Pet.Family.SharedKernel;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.Volunteers.Application.Interfaces;
using PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.Update.MainInfo;
using PetFamily.Volunteers.Domain.Volunteer.ValueObjects;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Volunteer.Update.SocialMediaDetails;

public class UpdateSocialMediaDetailsHandler : ICommandHandler<Guid,UpdateSocialMediaDetailsCommand>
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<UpdateMainInfoHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateSocialMediaDetailsCommand> _validator;

    public UpdateSocialMediaDetailsHandler(IVolunteerRepository volunteerRepository,
        ILogger<UpdateMainInfoHandler> logger, IUnitOfWork unitOfWork
        ,IValidator<UpdateSocialMediaDetailsCommand> validator)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<Guid, CustomErrorsList>> Handle(UpdateSocialMediaDetailsCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var volunteerResult = await _volunteerRepository.GetById(command.VolonteerId, cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var newSocialNetworks = command.UpdateSocialNetworksDto.SocialNetworks.Select(sn =>
            SocialMedia.Create(sn.Name, sn.Url).Value).ToList();
        
        volunteerResult.Value.UpdateSocialMediaDetails(new Domain.Volunteer.ValueObjects.SocialMediaDetails(newSocialNetworks));
        
        await _volunteerRepository.Save(volunteerResult.Value, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation( "For volunteer with ID: {id} was updated social networks", volunteerResult.Value.Id);
        
        return volunteerResult.Value.Id.Value;
    }
}