namespace PetFamily.Discussions.Domain;

public class Relation
{
    private readonly List<Discussion> _discussions = [];
    public Guid RelationId { get; private set; }
    public string Name { get; private set; }
    
    public IReadOnlyList<Discussion> Discussions => _discussions;

    private Relation() { }
    private Relation(string name)
    {
        RelationId = Guid.NewGuid();
        Name = name;
    }

    public static Relation Create(string name) => new Relation(name);
}