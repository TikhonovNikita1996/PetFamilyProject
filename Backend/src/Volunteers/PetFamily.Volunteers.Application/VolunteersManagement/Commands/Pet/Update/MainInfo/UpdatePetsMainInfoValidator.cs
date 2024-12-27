using FluentValidation;
using Pet.Family.SharedKernel;
using PetFamily.Core.Dtos.Pet;
using PetFamily.Core.Validation;
using PetFamily.Volunteers.Domain.Pet.ValueObjects;
using PetFamily.Volunteers.Domain.Volunteer.ValueObjects;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Pet.Update.MainInfo;

public class UpdatePetsMainInfoValidator : AbstractValidator<UpdatePetsMainInfoCommand>
{
    public UpdatePetsMainInfoValidator()
    {
        RuleFor(r => r.PetId).NotNull().WithError(Errors.General.ValueIsRequired());
    }
}

public class UpdatePetsMainInfoDtoValidator : AbstractValidator<UpdatePetsMainInfoDto>
{
    public UpdatePetsMainInfoDtoValidator()
    {
        RuleFor(c => new { c.Name })
            .MustBeValueObject(x => PetsName.Create(x.Name));
        
        RuleFor(c => c.Description)
            .MustBeValueObject(Description.Create);
        
        RuleFor(c => c.OwnersPhoneNumber)
            .MustBeValueObject(PhoneNumber.Create);
        
        RuleFor(c => c.Gender).NotNull().Must(g => g is "Male" or "Female")
            .WithError(Errors.General.ValueIsInvalid("Gender"));
        
        RuleFor(c => c.SpeciesId).NotNull().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.BreedId).NotNull().WithError(Errors.General.ValueIsRequired());
        
    }
}

public class LocationAddressValidator : AbstractValidator<LocationAddressDto>
{
    public LocationAddressValidator()
    {
        RuleFor(c => new { c.Region, c.City, c.Street, c.HouseNumber, c.Floor, c.Apartment })
            .MustBeValueObject(x => LocationAddress.Create(x.Region, x.City,
                x.Street, x.HouseNumber, x.Floor, x.Apartment));
    }
}