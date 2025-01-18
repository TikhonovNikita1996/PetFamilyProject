using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Discussions.Domain;

namespace PetFamily.Discussions.Infrastructure.Configurations.Write;

public class RelationConfiguration : IEntityTypeConfiguration<Relation>
{
    public void Configure(EntityTypeBuilder<Relation> builder)
    {
        builder.ToTable("relations");
        
        builder.HasKey(b => b.RelationId);
        
        builder.Property(s => s.RelationId)
            .IsRequired()
            .HasColumnName("relation_id");
        
        builder.Property(s => s.RelationEntityId)
            .IsRequired()
            .HasColumnName("relation_entity_id");
    }
}