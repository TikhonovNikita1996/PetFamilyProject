using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteers");
        builder.HasKey(x => x.Id);

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

        /*builder.OwnsOne(v => v.DonateForHelpInfos, db =>
        {
            db.ToJson();

            db.OwnsMany(s => s.DonationInfos, m =>
            {
                m.Property(sm => sm.Name)
                    .IsRequired();

                m.Property(sm => sm.Description)
                    .IsRequired();
            });
        });*/

    }
}