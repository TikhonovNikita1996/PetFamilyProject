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

public class AdminAccountConfiguration : IEntityTypeConfiguration<AdminAccount>
{
    public void Configure(EntityTypeBuilder<AdminAccount> builder)
    {
        builder.ToTable("admin_accounts");
        builder.HasKey(x => x.Id);

        builder
            .HasOne(aa => aa.User)
            .WithOne(u => u.AdminAccount)
            .HasForeignKey<AdminAccount>(a => a.UserId);
    }
}