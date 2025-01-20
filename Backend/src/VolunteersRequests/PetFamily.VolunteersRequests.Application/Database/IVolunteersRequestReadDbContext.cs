using PetFamily.Core.Dtos.VolunteersRequest;

namespace PetFamily.VolunteersRequests.Application.Database;

public interface IVolunteersRequestReadDbContext
{
    IQueryable<VolunteersRequestDto> VolunteersRequests { get;}
}