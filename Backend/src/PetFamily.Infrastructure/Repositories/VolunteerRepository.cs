using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Entities.Volunteer;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;
using PetFamily.Domain.Shared;

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
    
    public async Task<Result<Volunteer, CustomError>> GetByFullname(FullName fullname)
    {
        var volunteer = await _dbContext.Volunteers
            .Include(p => p.CurrentPets)
            .FirstOrDefaultAsync(v => v.Fullname == fullname);
        if (volunteer is null)
            return Errors.General.NotFound();
        return volunteer;
    }
    
    public async Task<Result<Volunteer, CustomError>> GetByEmail(Email email)
    {
        var volunteer = await _dbContext.Volunteers.Where(x => x.Email == email).FirstOrDefaultAsync();
        if (volunteer is null)
            return Errors.General.NotFound();
        return volunteer;
    }
}