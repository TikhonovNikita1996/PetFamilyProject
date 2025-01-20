namespace PetFamily.Discussions.Presentation.Requests;

public record DeleteMessageRequest(Guid SenderId, Guid MessageId);
