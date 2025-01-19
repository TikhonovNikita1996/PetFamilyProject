using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.Dtos.VolunteersRequest;
using PetFamily.VolunteersRequests.Domain;
using PetFamily.VolunteersRequests.Domain.ValueObjects;

namespace PetFamily.VolunteersRequests.Infrastructure.Configurations.Read;

public class VolunteerRequestDtoConfiguration : IEntityTypeConfiguration<VolunteersRequestDto>
{
    public void Configure(EntityTypeBuilder<VolunteersRequestDto> builder)
    {
        builder.ToTable("volunteer_requests");
        
        builder.HasKey(b => b.RequestId);
        
        builder.Property(s => s.RequestId)
            .IsRequired()
            .HasColumnName("request_Id");
        
        builder.Property(s => s.UserId)
            .HasColumnName("user_Id");
        
        builder.Property(s => s.AdminId)
            .IsRequired(false)
            .HasColumnName("admin_Id");
        
        builder.Property(s => s.DiscussionId)
            .IsRequired(false)
            .HasColumnName("discussion_id");
        
        builder.Property(s => s.VolunteerInfo)
            .IsRequired(false)
            .HasColumnName("volunteer_info");
        
        builder.Property(v=> v.Status)
            .HasConversion<string>()
            .HasColumnName("status")
            .IsRequired();
        
    }
}