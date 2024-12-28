using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;

namespace PetFamily.Volunteers.Domain.Volunteer.ValueObjects;

public class PhoneNumber
{
    private static readonly Regex ValidationRegex = new Regex(
        @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$",
        RegexOptions.Singleline | RegexOptions.Compiled);
    public string Value { get; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static Result<PhoneNumber, CustomError> Create(string value)
    {
        if (!ValidationRegex.IsMatch(value))
            return Errors.General.ValueIsInvalid(nameof(PhoneNumber));
        
        return new PhoneNumber(value);
    }
}