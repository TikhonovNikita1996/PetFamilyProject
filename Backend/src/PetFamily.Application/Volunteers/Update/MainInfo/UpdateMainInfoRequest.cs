using PetFamily.Domain.Entities.Ids;

namespace PetFamily.Application.Volunteers.Update.MainInfo;
public record UpdateMainInfoRequest (Guid VolunteerId, 
    UpdateMainInfoDto Dto );
    
public record UpdateMainInfoDto(
    string Description, 
    string PhoneNumber, 
    int WorkingExperience, 
    string FirstName, 
    string LastName, 
    string MiddleName );