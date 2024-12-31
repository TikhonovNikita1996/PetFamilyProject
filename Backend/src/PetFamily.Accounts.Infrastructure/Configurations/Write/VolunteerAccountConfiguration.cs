using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pet.Family.SharedKernel;
using Pet.Family.SharedKernel.ValueObjects.Pet;
using Pet.Family.SharedKernel.ValueObjects.Volunteer;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Domain.AccountModels;
using PetFamily.Core.Dtos.Pet;
using PetFamily.Core.Dtos.Volunteer;
using PetFamily.Core.Extensions;

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