namespace PetFamily.Domain;
public class DetailForAssistance
{
    public string Name { get; private set; } = default!;
    public string Description { get; private set; } = default!;
}

public class SocialMediaDetail
{
    public string Name { get; private set; } = default!;
    public string Url { get; private set; } = default!;
}
