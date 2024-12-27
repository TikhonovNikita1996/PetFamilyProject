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
        
        builder.Property(v=> v.Gender)
            .HasConversion<string>()
            .HasColumnName("gender")
            .IsRequired();
        
        builder.OwnsOne(v => v.SocialMediaDetails, sb =>
        {
            sb.ToJson();

            sb.OwnsMany(s => s.SocialMedias, m =>
            {
                m.Property(sm => sm.Name)
                    .IsRequired();

                m.Property(sm => sm.Url)
                    .IsRequired();
            });
        });

        builder.OwnsOne(v => v.DonateForHelpInfos, db =>
        {
            db.ToJson();

            db.OwnsMany(s => s.DonationInfos, m =>
            {
                m.Property(sm => sm.Name)
                    .IsRequired();

                m.Property(sm => sm.Description)
                    .IsRequired();
            });
        });

        builder.ComplexProperty(v => v.Fullname, fn =>
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
        
        builder.ComplexProperty(v => v.Email, pm =>
        {
            pm.Property(p => p.Value)
                .HasColumnName("email")
                .HasMaxLength(ProjectConstants.MAX_LOW_TEXT_LENGTH)
                .IsRequired();
        });
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
        
        builder.ComplexProperty(v => v.WorkingExperience, pm =>
        {
            pm.Property(p => p.Value)
                .HasColumnName("workingExperience")
                .HasColumnName("working_experience")
                .IsRequired();
        });
        
        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}