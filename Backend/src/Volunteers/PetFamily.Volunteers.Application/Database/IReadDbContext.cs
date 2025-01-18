using PetFamily.Core.Dtos.Volunteer;

namespace PetFamily.Volunteers.Application.Database;

public interface IReadDbContext
{
    IQueryable<VolunteerDto> Volunteers { get;}
    IQueryable<PetDto> Pets { get;}
}