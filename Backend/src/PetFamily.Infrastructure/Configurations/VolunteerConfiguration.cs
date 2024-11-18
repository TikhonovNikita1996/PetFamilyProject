using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Volunteer;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations;

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
        
        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(ProjectConstants.MAX_HIGHT_TEXT_LENGTH);

        builder.HasMany(v => v.CurrentPets)
            .WithOne()
            .HasForeignKey("volunteer_id");
        
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
                .HasMaxLength(ProjectConstants.MAX_LOW_TEXT_LENGTH);
            
            fn.Property(n => n.LastName)
                .IsRequired()
                .HasMaxLength(ProjectConstants.MAX_LOW_TEXT_LENGTH);
            
            fn.Property(n => n.MiddleName)
                .IsRequired()
                .HasMaxLength(ProjectConstants.MAX_LOW_TEXT_LENGTH);
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
                .HasMaxLength(ProjectConstants.MAX_LOW_TEXT_LENGTH)
                .IsRequired();
        });
    }
}