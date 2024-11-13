using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Pet;

namespace PetFamily.Infrastructure.Configurations;

public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(s => s.Id)
            .HasConversion( id => id.Value,
                value => SpeciesId.Create(value));
        
        builder.Property(x => x.Name)
            .IsRequired();
    }
}