﻿using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.Interfaces;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Volunteer;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Infrastructure.DataContexts;

namespace PetFamily.Infrastructure.Repositories;

public class VolunteerRepository : IVolunteerRepository
{
    private readonly WriteDbContext _dbContext;

    public VolunteerRepository(WriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> AddAsync(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await _dbContext.Volunteers.AddAsync(volunteer, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return volunteer.Id.Value;
    }

    public async Task<Guid> Save(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        _dbContext.Volunteers.Attach(volunteer);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return volunteer.Id.Value;
    }
    
    public async Task<Guid> Delete(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        _dbContext.Volunteers.Remove(volunteer);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return volunteer.Id.Value;
    }

    public async Task<Result<Volunteer, CustomError>> GetById(
        VolunteerId volunteerId,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _dbContext.Volunteers
            .Include(v => v.CurrentPets)
            .FirstOrDefaultAsync(v =>v.Id == volunteerId, cancellationToken);

        if (volunteer is null)
            return Errors.General.NotFound("");

        return volunteer;
    }
    
    public async Task<Result<Volunteer, CustomError>> GetByFullname(FullName fullname,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _dbContext.Volunteers
            .Include(x => x.CurrentPets)
            .FirstOrDefaultAsync(v => v.Fullname == fullname, cancellationToken);
        if (volunteer is null)
            return Errors.General.NotFound("");
        return volunteer;
    }

    public async Task<Result<Volunteer, CustomError>> GetByEmail(Email email,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _dbContext.Volunteers
            .Where(x => x.Email == email).FirstOrDefaultAsync(cancellationToken);
        if (volunteer is null)
            return Errors.General.NotFound("");
        return volunteer;
    }
}