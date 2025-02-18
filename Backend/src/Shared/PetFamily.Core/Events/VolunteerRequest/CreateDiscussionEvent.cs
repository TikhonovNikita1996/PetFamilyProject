using Pet.Family.SharedKernel;

namespace PetFamily.Core.Events.VolunteerRequest;

public record CreateDiscussionEvent(Guid ReviewingUserId, Guid ApplicantUserId) : IDomainEvent;