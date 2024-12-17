namespace PetFamily.Application.Dtos;

public class VolunteerDto
{
    public Guid Id { get; init; }
    public string LastName { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string? MiddleName { get; init; } = string.Empty;
    public string Email { get; init; }  = string.Empty;
    public string Gender { get; init; }  = string.Empty;
    public int WorkingExperience { get;  init; }
    public string Description { get; private set; }  = string.Empty;
    public string PhoneNumber { get; private set; }  = string.Empty;
}