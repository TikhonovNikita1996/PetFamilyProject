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
        if (string.IsNullOrEmpty(lastName))
            return "Last name can not be empty";
        if (string.IsNullOrEmpty(name))
            return "Name can not be empty";
        if (string.IsNullOrEmpty(middleName))
            return "Middle name can not be empty";

        var newFullName = new FullName(lastName, name, middleName);

        return newFullName;
    }
}