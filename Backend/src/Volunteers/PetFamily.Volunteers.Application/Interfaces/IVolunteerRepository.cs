using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;
using Pet.Family.SharedKernel.ValueObjects.Volunteer;
using PetFamily.Volunteers.Domain.Ids;
using PetFamily.Volunteers.Domain.Volunteer;

namespace PetFamily.Volunteers.Application.Interfaces;

public interface IVolunteerRepository
{
    public Task<Guid> AddAsync(Volunteer volunteer, CancellationToken cancellationToken = default);
    public Task<Guid> Delete(Volunteer volunteer, CancellationToken cancellationToken = default);
    public Task<Result<Volunteer, CustomError>> GetById(VolunteerId volunteerId,
        CancellationToken cancellationToken = default);
}