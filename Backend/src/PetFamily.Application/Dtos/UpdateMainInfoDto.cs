namespace PetFamily.Application.Dtos;

public record UpdateMainInfoDto(
    string Description, 
    string PhoneNumber, 
    int WorkingExperience, 
    string FirstName, 
    string LastName, 
    string MiddleName );