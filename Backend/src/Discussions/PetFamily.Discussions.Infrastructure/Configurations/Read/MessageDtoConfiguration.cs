using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.Dtos.Discussion;

namespace PetFamily.Discussions.Infrastructure.Configurations.Read;

public class MessageDtoConfiguration : IEntityTypeConfiguration<MessageDto>
{
    public void Configure(EntityTypeBuilder<MessageDto> builder)
    {
        builder.ToTable("messages");
        
        builder.HasKey(b => b.MessageId);
        
        builder.Property(s => s.MessageId)
            .IsRequired()
            .HasColumnName("message_id");
    }
}