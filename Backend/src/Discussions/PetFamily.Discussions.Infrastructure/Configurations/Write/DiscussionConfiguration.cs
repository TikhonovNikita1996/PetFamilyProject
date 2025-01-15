using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Discussions.Domain;

namespace PetFamily.Discussions.Infrastructure.Configurations.Write;

public class DiscussionConfiguration : IEntityTypeConfiguration<Discussion>
{
    public void Configure(EntityTypeBuilder<Discussion> builder)
    {
        builder.ToTable("discussions");
        
        builder.HasKey(b => b.DiscussionId);
        
        builder.Property(s => s.DiscussionId)
            .IsRequired()
            .HasColumnName("discussion_id");
        
        builder.Property(s => s.RelationId)
            .IsRequired()
            .HasColumnName("relation_id");
        
        builder.ComplexProperty(p => p.DiscussionUsers, du =>
        {
            du.Property(p => p.FirstUserId)
                .IsRequired()
                .HasColumnName("first_user_id");
            
            du.Property(p => p.SecondUserId)
                .IsRequired()
                .HasColumnName("second_user_id");
        });
        
        builder.HasOne(d => d.Relation)
            .WithMany(r => r.Discussions)
            .HasForeignKey(d => d.RelationId);
        
        builder.HasMany(d => d.Messages)
            .WithOne()
            .HasForeignKey(m => m.DiscussionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(v=> v.Status)
            .HasConversion<string>()
            .HasColumnName("status")
            .IsRequired();
        
    }
}