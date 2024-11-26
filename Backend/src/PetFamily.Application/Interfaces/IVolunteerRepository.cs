using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Volunteer;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Interfaces;

public interface IVolunteerRepository
{
    public Task<Guid> AddAsync(Volunteer volunteer, CancellationToken cancellationToken = default);
    public Task<Guid> Save(Volunteer volunteer, CancellationToken cancellationToken = default);
    public Task<Guid> Delete(Volunteer volunteer, CancellationToken cancellationToken = default);
    public Task<Result<Volunteer, CustomError>> GetByFullname(FullName fullname, CancellationToken cancellationToken = default);
    public Task<Result<Volunteer, CustomError>> GetById(VolunteerId volunteerId, CancellationToken cancellationToken = default);
    public Task<Result<Volunteer, CustomError>> GetByEmail(Email email, CancellationToken cancellationToken = default);
    
    
}