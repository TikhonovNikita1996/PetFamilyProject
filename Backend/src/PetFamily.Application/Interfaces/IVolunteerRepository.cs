using CSharpFunctionalExtensions;
using PetFamily.Domain.Entities.Volunteer;
using PetFamily.Domain.Entities.Volunteer.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Interfaces;

public interface IVolunteerRepository
{
    public Task<Guid> AddAsync(Volunteer volunteer, CancellationToken cancellationToken = default);
    public Task<Result<Volunteer, CustomError>> GetByFullname(FullName fullname);
}