using Microsoft.EntityFrameworkCore;
using Pet.Family.SharedKernel;
using PetFamily.Volunteers.Infrastructure.DataContexts;

namespace PetFamily.Volunteers.Infrastructure.Services;

public class DeleteExpiredVolunteersService (VolunteersWriteDbContext dbContext)
{  
    public async Task Process(CancellationToken cancellationToken)
    {
        var volunteersToDelete = await dbContext.Volunteers
            .Where(v => v.DeletionDate < DateTime.UtcNow - TimeSpan
                .FromDays(ProjectConstants.SOFT_DELETED_ENTITIES_LIFE_TIME_IN_HOURS))
            .ToListAsync(cancellationToken);

        dbContext.Volunteers.RemoveRange(volunteersToDelete);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}