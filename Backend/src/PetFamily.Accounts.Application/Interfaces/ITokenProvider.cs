using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Application.Interfaces;

public interface ITokenProvider
{
    string GenerateAccessToken(User user);
}