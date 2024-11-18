using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Pet;
using PetFamily.Domain.Entities.Pet.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations;

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.HasKey(b => b.Id);
        
        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,
                value => BreedId.Create(value));
        
        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(ProjectConstants.MAX_LOW_TEXT_LENGTH);
    }
}