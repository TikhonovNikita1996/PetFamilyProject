using PetFamily.Core.Abstractions;

namespace PetFamily.Accounts.Application.AccountsManagement.Queries.GetUsersInfo;

public record GetUsersInfoQuery(Guid UserId) : IQuery;