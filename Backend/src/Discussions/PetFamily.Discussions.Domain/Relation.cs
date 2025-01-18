namespace PetFamily.Discussions.Domain;

public class Relation
{
    private readonly List<Discussion> _discussions = [];
    public Guid RelationId { get; private set; }
    public Guid PetId { get; private set; }
    public IReadOnlyList<Discussion> Discussions => _discussions;
    private Relation() { }
    private Relation(Guid petId)
    {
        RelationId = Guid.NewGuid();
    }
    public static Relation Create(Guid petId) => new Relation(petId);
}