namespace PetFamily.Discussions.Domain;

public class Relation
{
    private readonly List<Discussion> _discussions = [];
    public Guid RelationId { get; private set; }
    public Guid RelationEntityId { get; private set; }
    public IReadOnlyList<Discussion> Discussions => _discussions;
    private Relation() { }
    private Relation(Guid relationEntityId)
    {
        RelationId = Guid.NewGuid();
        RelationEntityId = relationEntityId;
    }
    public static Relation Create(Guid petId) => new Relation(petId);
}