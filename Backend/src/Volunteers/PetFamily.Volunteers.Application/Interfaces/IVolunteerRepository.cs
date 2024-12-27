﻿using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;
using PetFamily.Volunteers.Domain.Ids;
using PetFamily.Volunteers.Domain.Volunteer;
using PetFamily.Volunteers.Domain.Volunteer.ValueObjects;

namespace PetFamily.Volunteers.Application.Interfaces;

public interface IVolunteerRepository
{
    public Task<Guid> AddAsync(Volunteer volunteer, CancellationToken cancellationToken = default);
    public Task<Guid> Save(Volunteer volunteer, CancellationToken cancellationToken = default);
    public Task<Guid> Delete(Volunteer volunteer, CancellationToken cancellationToken = default);
    public Task<Result<Volunteer, CustomError>> GetByFullname(FullName fullname,
        CancellationToken cancellationToken = default);
    public Task<Result<Volunteer, CustomError>> GetById(VolunteerId volunteerId,
        CancellationToken cancellationToken = default);
    public Task<Result<Volunteer, CustomError>> GetByEmail(Email email,
        CancellationToken cancellationToken = default);
}