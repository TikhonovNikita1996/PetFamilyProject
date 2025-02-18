using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Pet.Family.SharedKernel;
using PetFamily.VolunteersRequests.Application.Interfaces;
using PetFamily.VolunteersRequests.Domain;
using PetFamily.VolunteersRequests.Domain.ValueObjects;
using PetFamily.VolunteersRequests.Infrastructure.DataContexts;

namespace PetFamily.VolunteersRequests.Infrastructure;

public class VolunteersRequestsRepository(VolunteersRequestWriteDbContext dbContext)
    : IVolunteersRequestRepository
{
    public async Task Add(VolunteerRequest volunteerRequest,
        CancellationToken cancellationToken = default)
    {
        await dbContext.VolunteersRequests.AddAsync(volunteerRequest, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(VolunteerRequest volunteerRequest,
        CancellationToken cancellationToken = default)
    {
        dbContext.VolunteersRequests.Remove(volunteerRequest);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<Result<VolunteerRequest, CustomError>> GetById(VolunteerRequestId requestId,
        CancellationToken cancellationToken = default)
    {
        var request = await dbContext.VolunteersRequests.
            FirstOrDefaultAsync(r => r.Id == requestId, cancellationToken);

        if (request is null)
            return Errors.General.NotFound(requestId);

        return request;
    }
}