using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pet.Family.SharedKernel;
using PetFamily.Volunteers.Domain.Ids;
using PetFamily.Volunteers.Domain.Volunteer;

namespace PetFamily.Volunteers.Infrastructure.Configurations.Write;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteers");
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .HasConversion(
                id => id.Value,
                value => VolunteerId.Create(value));

        builder.HasMany(v => v.CurrentPets)
            .WithOne()
            .HasForeignKey("volunteer_id")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        builder.ComplexProperty(v => v.PhoneNumber, pm =>
        {
            pm.Property(p => p.Value)
                .HasColumnName("phone_number")
                .HasMaxLength(ProjectConstants.MAX_HIGHT_PHONENUMBER_LENGTH)
                .IsRequired();
        });
        
        builder.ComplexProperty(v => v.Description, pm =>
        {
            pm.Property(p => p.Value)
                .HasColumnName("description")
                .HasMaxLength(ProjectConstants.MAX_HIGHT_TEXT_LENGTH)
                .HasColumnName("description")
                .IsRequired();
        });
    }
}