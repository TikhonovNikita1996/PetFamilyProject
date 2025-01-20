using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.Dtos.Discussion;

namespace PetFamily.Discussions.Infrastructure.Configurations.Read;

public class DiscussionDtoConfiguration : IEntityTypeConfiguration<DiscussionDto>
{
    public void Configure(EntityTypeBuilder<DiscussionDto> builder)
    {
        builder.ToTable("discussions");
        builder.HasKey(b => b.DiscussionId);
        
        builder.Property(s => s.DiscussionId)
            .IsRequired()
            .HasColumnName("discussion_id");
        
        builder.Property(s => s.DiscussionId)
                .IsRequired()
                .HasColumnName("discussion_id");
        
        builder.Property(u => u.ReviewingUserId)
            .HasColumnName("reviewing_user_id");
        
        builder.Property(u => u.ApplicantUserId)
            .HasColumnName("applicant_user_id");
    }
}