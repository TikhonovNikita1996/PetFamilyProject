using PetFamily.Domain.Entities.Volunteer;

namespace PetFamily.Application.Interfaces;

public interface IVolunteerRepository
{
    public Task<Guid> AddAsync(Volunteer volunteer, CancellationToken cancellationToken = default);
}