namespace PetFamily.Accounts.Presentation.Requests;

public record RefreshTokenRequest(string AccessToken, Guid RefreshToken);