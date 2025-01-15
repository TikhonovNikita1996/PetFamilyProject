namespace PetFamily.Discussions.Domain;

public class Relation
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    
    public IReadOnlyList<Discussion> Discussions { get; private set; } = [];

    private Relation() { }
    private Relation(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    public static Relation Create(string name) => new Relation(name);
}