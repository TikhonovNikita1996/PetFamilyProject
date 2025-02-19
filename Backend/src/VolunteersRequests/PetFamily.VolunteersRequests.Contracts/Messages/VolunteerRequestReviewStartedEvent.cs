namespace PetFamily.VolunteersRequests.Contracts.Messages;

public record VolunteerRequestReviewStartedEvent(Guid ReviewingUserId, Guid ApplicantUserId);