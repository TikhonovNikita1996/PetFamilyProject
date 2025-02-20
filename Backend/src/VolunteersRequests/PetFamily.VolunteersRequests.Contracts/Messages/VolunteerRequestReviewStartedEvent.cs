using Pet.Family.SharedKernel;

namespace PetFamily.VolunteersRequests.Contracts.Messages;

public record VolunteerRequestReviewStartedEvent(Guid ReviewingUserId, Guid ApplicantUserId) : IDomainEvent;