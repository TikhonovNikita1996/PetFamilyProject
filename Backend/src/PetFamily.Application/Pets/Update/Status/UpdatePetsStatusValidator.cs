using FluentValidation;
using PetFamily.Application.Dtos;
using PetFamily.Application.Pets.Update.MainInfo;
using PetFamily.Application.Validation;
using PetFamily.Domain.Entities.Pet.ValueObjects;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Pets.Update.Status;

public class UpdatePetsStatusValidator : AbstractValidator<UpdatePetsStatusCommand>
{
    public UpdatePetsStatusValidator()
    {
        RuleFor(r => r.PetId).NotNull().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.VolunteerId).NotNull().WithError(Errors.General.ValueIsRequired());
        RuleFor(r => r.NewStatus).NotNull().WithError(Errors.General.ValueIsRequired());
    }
}