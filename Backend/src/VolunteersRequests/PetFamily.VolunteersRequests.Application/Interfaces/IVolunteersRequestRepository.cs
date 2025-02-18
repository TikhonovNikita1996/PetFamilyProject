using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;
using PetFamily.VolunteersRequests.Domain;
using PetFamily.VolunteersRequests.Domain.ValueObjects;

namespace PetFamily.VolunteersRequests.Application.Interfaces;

public interface IVolunteersRequestRepository
{
    public Task Add(VolunteerRequest volunteerRequest,
        CancellationToken cancellationToken = default);

    public Task Delete(VolunteerRequest volunteerRequest,
        CancellationToken cancellationToken = default);

    public Task<Result<VolunteerRequest, CustomError>> GetById(VolunteerRequestId requestId,
        CancellationToken cancellationToken = default);
}