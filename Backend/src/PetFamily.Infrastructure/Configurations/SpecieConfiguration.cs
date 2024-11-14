using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Pet;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations;

public class SpecieConfiguration: IEntityTypeConfiguration<Specie>
{
    public void Configure(EntityTypeBuilder<Specie> builder)
    {
        builder.ToTable("species");
        builder.HasKey(s => s.Id);
        
        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,
                value => SpecieId.Create(value));

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(ProjectConstants.MAX_LOW_TEXT_LENGTH);
        
        builder.HasMany(s => s.Breeds)
            .WithOne()
            .HasForeignKey("specie_Id")
            .IsRequired();
    }
}