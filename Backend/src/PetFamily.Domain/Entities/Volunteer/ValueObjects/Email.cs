﻿using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Entities.Volunteer.ValueObjects;

public record Email
{
    public Email()
    {
        
    }
    private static readonly Regex ValidationRegex = new Regex(
        @"^[\w-\.]{1,40}@([\w-]+\.)+[\w-]{2,4}$",
        RegexOptions.Singleline | RegexOptions.Compiled);
    private Email(string value)
    {
        Value = value;
    }
    public string Value { get; } = default!;
    
    public static Result<Email, CustomError> Create(string value)
    {
        if (!ValidationRegex.IsMatch(value))
            return Errors.General.ValueIsInvalid(nameof(Email));
        
        var newEmail = new Email(value);

        return newEmail;
    }
}