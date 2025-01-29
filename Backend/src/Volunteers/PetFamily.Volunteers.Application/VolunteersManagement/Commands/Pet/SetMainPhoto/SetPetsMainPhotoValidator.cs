using FluentValidation;
using Pet.Family.SharedKernel;
using PetFamily.Core.Validation;

namespace PetFamily.Volunteers.Application.VolunteersManagement.Commands.Pet.SetMainPhoto;

public class SetPetsMainPhotoValidator : AbstractValidator<SetPetsMainPhotoCommand>
{
    public SetPetsMainPhotoValidator()
    {
        RuleFor(r => r.PetId).NotNull().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.VolunteerId).NotNull().WithError(Errors.General.ValueIsRequired());
        // RuleFor(r => r.FilePath).MustBeValueObject(fp => FilePath.Create(fp.Value));
    }
}