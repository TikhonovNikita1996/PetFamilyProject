using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.VolunteersRequests.Domain;
using PetFamily.VolunteersRequests.Domain.ValueObjects;

namespace PetFamily.VolunteersRequests.Infrastructure.Configurations.Write;

public class VolunteerRequestConfiguration : IEntityTypeConfiguration<VolunteerRequest>
{
    public void Configure(EntityTypeBuilder<VolunteerRequest> builder)
    {
        builder.ToTable("volunteer_requests");
        
        builder.HasKey(b => b.Id);
        
        builder.Property(v => v.Id)
            .HasConversion(
                id => id.Value,
                value => VolunteerRequestId.Create(value));
        
        builder.Property(s => s.UserId)
            .HasColumnName("user_Id");
        
        builder.Property(s => s.AdminId)
            .IsRequired(false)
            .HasColumnName("admin_Id");
        
        builder.Property(s => s.DiscussionId)
            .IsRequired(false)
            .HasColumnName("discussion_id");
        
        builder.Property(v => v.VolunteerInfo)
            .HasConversion(
                i => i.Value,
                value => VolunteerInfo.Create(value).Value)
            .IsRequired(false)
            .HasColumnName("volunteer_info");
        
        builder.Property(v => v.RejectionComment)
            .HasConversion(
                i => i.Value,
                value => RejectionComment.Create(value).Value)
            .IsRequired(false)
            .HasColumnName("rejection_comment");
        
        builder.Property(v=> v.Status)
            .HasConversion<string>()
            .HasColumnName("status")
            .IsRequired();
        
    }
}