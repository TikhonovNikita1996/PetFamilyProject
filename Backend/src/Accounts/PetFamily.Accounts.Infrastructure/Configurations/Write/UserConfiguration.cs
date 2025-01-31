using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pet.Family.SharedKernel;
using Pet.Family.SharedKernel.ValueObjects;
using Pet.Family.SharedKernel.ValueObjects.Pet;
using Pet.Family.SharedKernel.ValueObjects.Volunteer;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Dtos.Pet;
using PetFamily.Core.Dtos.Volunteer;
using PetFamily.Core.Extensions;

namespace PetFamily.Accounts.Infrastructure.Configurations.Write;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        
        builder.ComplexProperty(v => v.FullName, fn =>
        {
            fn.Property(n => n.Name)
                .IsRequired()
                .HasMaxLength(ProjectConstants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("name");
            
            fn.Property(n => n.LastName)
                .IsRequired()
                .HasMaxLength(ProjectConstants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("last_name");
            
            fn.Property(n => n.MiddleName)
                .IsRequired()
                .HasMaxLength(ProjectConstants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("middle_name");
        });
        
        builder
            .Property(u => u.SocialNetworks)
            .ValueObjectsJsonConversion(
                input => new SocialMediaDetailsDto( input.Name , input.Url),
                output => SocialMedia.Create(output.Name, output.Url).Value)
            .HasColumnName("social_networks");
        
        builder.Property(a => a.Photo)
            .IsRequired(false)
            .HasConversion(
                photo => photo!.FileId, 
                value => new UserAvatar(value)
            )
            .HasColumnName("photo");

        builder.HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity<IdentityUserRole<Guid>>();
        
    }
}