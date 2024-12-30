using Microsoft.AspNetCore.Identity;
using Pet.Family.SharedKernel.ValueObjects.Pet;
using Pet.Family.SharedKernel.ValueObjects.Volunteer;

namespace PetFamily.Accounts.Domain;

public class User : IdentityUser<Guid>
{
    private List<SocialMedia> _socialNetworks = [];
    private List<Photo> _photos = [];
    public FullName FullName { get; set; } = null!;
    
    public Photo Photo { get; set; } = null!;
    
    public Guid RoleId { get; set; }
    public IReadOnlyList<Photo> Photos => _photos;
    public IReadOnlyList<SocialMedia> SocialNetworks => _socialNetworks;
    
}