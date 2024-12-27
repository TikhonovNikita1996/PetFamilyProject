using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pet.Family.SharedKernel;
using Pet.Family.SharedKernel.ValueObjects.Specie;
using PetFamily.Species.Domain.ValueObjects;

namespace PetFamily.Species.Infrastructure.Configurations.Write;

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.HasKey(b => b.Id);
        
        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,
                value => BreedId.Create(value))
            .HasColumnName("breed_id");
        
        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(ProjectConstants.MAX_LOW_TEXT_LENGTH);
    }
}