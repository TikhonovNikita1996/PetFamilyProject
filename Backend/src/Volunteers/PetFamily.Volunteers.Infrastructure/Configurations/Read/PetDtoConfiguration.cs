using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pet.Family.SharedKernel.ValueObjects.Pet;
using PetFamily.Core.Dtos.Discussion;
using PetFamily.Core.Dtos.Pet;
using PetFamily.Core.Extensions;

namespace PetFamily.Volunteers.Infrastructure.Configurations.Read;

public class PetDtoConfiguration : IEntityTypeConfiguration<PetDto>
{
    public void Configure(EntityTypeBuilder<PetDto> builder)
    {
        builder.ToTable("pets");
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id);

        builder.Property(v => v.Photos)
            .HasConversion(
                photos => JsonSerializer.Serialize(photos, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<PhotoDto[]>(json, JsonSerializerOptions.Default)!);

    }
}
