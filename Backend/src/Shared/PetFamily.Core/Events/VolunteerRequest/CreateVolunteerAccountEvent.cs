using Pet.Family.SharedKernel;

namespace PetFamily.Core.Events.VolunteerRequest;

public record CreateVolunteerAccountEvent(Guid UserId) : IDomainEvent;