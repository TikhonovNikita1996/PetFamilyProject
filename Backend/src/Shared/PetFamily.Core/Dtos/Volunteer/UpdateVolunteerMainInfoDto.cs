namespace PetFamily.Core.Dtos.Volunteer;

public record UpdateVolunteerMainInfoDto(
    string Description, 
    string PhoneNumber, 
    int WorkingExperience, 
    string FirstName, 
    string LastName, 
    string MiddleName);