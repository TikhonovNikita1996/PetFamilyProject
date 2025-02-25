using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.Dtos.Account;

namespace PetFamily.Accounts.Infrastructure.Configurations.Read;

public class ParticipantDtoConfiguration : IEntityTypeConfiguration<ParticipantAccountDto>
{
    public void Configure(EntityTypeBuilder<ParticipantAccountDto> builder)
    {
        builder.ToTable("participant_accounts");
        builder.HasKey(v => v.ParticipantAccountId);
       
        builder.Property(x => x.ParticipantAccountId)
            .HasColumnName("id");
        
        builder.Property(v => v.UserId)
            .HasColumnName("user_id");
        
        builder.Property(n => n.BannedForRequestsUntil)
            .IsRequired(false)
            .HasColumnName("banned_for_requests_until");
    }
}