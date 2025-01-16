using PetFamily.Core.Dtos.Volunteer;

namespace PetFamily.Accounts.Presentation.Requests;

public record RegisterUserRequest(FullNameDto FullNameDto, 
    string Email, string UserName ,string Password);