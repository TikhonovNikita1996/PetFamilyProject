using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;
using PetFamily.Volunteers.Domain.Volunteer.ValueObjects;

namespace PetFamily.Volunteers.Domain.Pet.ValueObjects;

public class OwnersPhoneNumber
{
    private static readonly Regex ValidationRegex = new Regex(
        @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$",
        RegexOptions.Singleline | RegexOptions.Compiled);
    public string Value { get; }

    private OwnersPhoneNumber(string value)
    {
        Value = value;
    }

    public static Result<OwnersPhoneNumber, CustomError> Create(string value)
    {
        if (!ValidationRegex.IsMatch(value))
            return Errors.General.ValueIsInvalid(nameof(PhoneNumber));
        
        return new OwnersPhoneNumber(value);
    }
}