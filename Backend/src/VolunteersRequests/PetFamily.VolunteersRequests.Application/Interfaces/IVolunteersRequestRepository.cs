using CSharpFunctionalExtensions;
using Pet.Family.SharedKernel;
using PetFamily.VolunteersRequests.Domain;

namespace PetFamily.VolunteersRequests.Application.Interfaces;

public interface IVolunteersRequestRepository
{
    public Task Add(VolunteerRequest volunteerRequest,
        CancellationToken cancellationToken = default);

    public Task Delete(VolunteerRequest volunteerRequest,
        CancellationToken cancellationToken = default);

    public Task<Result<VolunteerRequest, CustomError>> GetById(Guid requestId,
        CancellationToken cancellationToken = default);
}