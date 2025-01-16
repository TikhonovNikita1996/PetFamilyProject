using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Discussions.Domain;

namespace PetFamily.Discussions.Infrastructure.Configurations.Write;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("messages");
        
        builder.HasKey(b => b.MessageId);
        
        builder.Property(s => s.MessageId)
            .IsRequired()
            .HasColumnName("message_id");
        
        builder.Property(s => s.SenderId)
            .IsRequired()
            .HasColumnName("sender_id");
        
        builder.Property(s => s.IsEdited)
            .IsRequired()
            .HasColumnName("is_edited");
        
        builder.Property(s => s.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");
        
        builder.Property(v => v.MessageText)
            .HasConversion(
                id => id.Value,
                value => MessageText.Create(value).Value)
            .IsRequired()
            .HasColumnName("message_text");
        
    }
}