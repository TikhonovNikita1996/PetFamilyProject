using PetFamily.Application.Interfaces;
using PetFamily.Domain.Entities.Volunteer;

namespace PetFamily.Infrastructure.Repositories;

public class VolunteerRepository : IVolunteerRepository
{
    private readonly DataContext _dbContext;

    public VolunteerRepository(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> AddAsync(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await _dbContext.Volunteers.AddAsync(volunteer, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return volunteer.Id.Value;
    }
}