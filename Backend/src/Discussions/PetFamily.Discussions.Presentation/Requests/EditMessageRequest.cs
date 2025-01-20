namespace PetFamily.Discussions.Presentation.Requests;

public record EditMessageRequest(Guid DiscussionId, Guid MessageId, Guid SenderId, string MessageText);
