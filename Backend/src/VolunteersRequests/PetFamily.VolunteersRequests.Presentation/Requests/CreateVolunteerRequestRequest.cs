namespace PetFamily.VolunteersRequests.Presentation.Requests;

public record CreateVolunteerRequestRequest(Guid UserId, string VolunteerInfo);