using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Pets.SetMainPhoto;

public class SetPetsMainPhotoValidator : AbstractValidator<SetPetsMainPhotoCommand>
{
    public SetPetsMainPhotoValidator()
    {
        RuleFor(r => r.PetId).NotNull().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.VolunteerId).NotNull().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.FilePath).MustBeValueObject(fp => FilePath.Create(fp.Path));
    }
}