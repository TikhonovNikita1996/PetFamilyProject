namespace PetFamily.Discussions.Presentation.Requests;

public record WriteMessageRequest(Guid SenderId, string MessageText);
