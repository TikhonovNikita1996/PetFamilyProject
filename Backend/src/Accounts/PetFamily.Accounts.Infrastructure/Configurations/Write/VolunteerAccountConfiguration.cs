using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Domain.AccountModels;

namespace PetFamily.Accounts.Infrastructure.Configurations.Write;

public class VolunteerAccountConfiguration : IEntityTypeConfiguration<VolunteerAccount>
{
    public void Configure(EntityTypeBuilder<VolunteerAccount> builder)
    {
        builder.ToTable("volunteer_accounts");
        builder.HasKey(x => x.Id);
        
        builder.ComplexProperty(v => v.WorkingExperience, pm =>
        {
            pm.Property(p => p.Value)
                .HasColumnName("workingExperience")
                .HasColumnName("working_experience")
                .IsRequired();
        });
    }
}