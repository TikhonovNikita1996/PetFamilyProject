using PetFamily.Domain.Shared;

namespace PetFamily.Domain.ValueObjects;

public record FullName
{
    private FullName(string lastName,
        string name,
        string middleName)
    {
        LastName = lastName;
        Name = name;
        MiddleName = middleName;
    }
    public string LastName { get; } = default!;
    public string Name { get; } = default!;
    public string MiddleName { get; } = default!;
    
    public static CustomResult<FullName> Create(string lastName,
        string name,
        string middleName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
            return "Last name can not be empty";
        if (string.IsNullOrWhiteSpace(name))
            return "Name can not be empty";
        if (string.IsNullOrWhiteSpace(middleName))
            return "Middle name can not be empty";

        var newFullName = new FullName(lastName, name, middleName);

        return newFullName;
    }
}