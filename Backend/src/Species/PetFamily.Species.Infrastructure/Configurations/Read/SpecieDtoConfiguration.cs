using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.Dtos.Specie;

namespace PetFamily.Species.Infrastructure.Configurations.Read;

public class SpecieDtoConfiguration : IEntityTypeConfiguration<SpecieDto>
{
    public void Configure(EntityTypeBuilder<SpecieDto> builder)
    {
        builder.ToTable("species");
        builder.HasKey(v => v.SpecieId);

        builder.Property(v => v.SpecieId)
            .HasColumnName("specie_id");
    }
}