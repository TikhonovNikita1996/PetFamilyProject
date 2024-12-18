namespace PetFamily.Application.Dtos;

public record UpdateVolunteerMainInfoDto(
    string Description, 
    string PhoneNumber, 
    int WorkingExperience, 
    string FirstName, 
    string LastName, 
    string MiddleName );