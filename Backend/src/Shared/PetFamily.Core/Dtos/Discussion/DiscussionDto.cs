namespace PetFamily.Core.Dtos.Discussion;

public class DiscussionDto
{
    public Guid DiscussionId { get; private set; }
    public Guid RelationId { get; private set; }
    public Guid FirstUser { get; private set;}
    public Guid SecondUser { get; private set;}
    public string Status { get; private set; } = default!;
    public MessageDto[] Messages { get; private set; } = [];
}
