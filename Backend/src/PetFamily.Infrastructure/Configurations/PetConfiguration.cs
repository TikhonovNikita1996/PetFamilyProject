using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Entities.Ids;
using PetFamily.Domain.Entities.Pet;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => PetId.Create(value));
        
        builder.ComplexProperty(v => v.PetsDescription, pm =>
        {
            pm.Property(p => p.Value)
                .HasColumnName("description")
                .HasMaxLength(ProjectConstants.MAX_HIGHT_TEXT_LENGTH)
                .IsRequired();
        });
        
        builder.ComplexProperty(v => v.HealthInformation, pm =>
        {
            pm.Property(p => p.Value)
                .HasColumnName("healthInformation")
                .HasMaxLength(ProjectConstants.MAX_HIGHT_TEXT_LENGTH)
                .IsRequired();
        });
        
        builder.ComplexProperty(v => v.OwnersPhoneNumber, pm =>
        {
            pm.Property(p => p.Value)
                .HasColumnName("owners_phone_number")
                .HasMaxLength(ProjectConstants.MAX_HIGHT_PHONENUMBER_LENGTH)
                .IsRequired();
        });
        
        builder.ComplexProperty(v => v.Color, pm =>
        {
            pm.Property(p => p.Value)
                .HasColumnName("color")
                .HasMaxLength(ProjectConstants.MAX_HIGHT_PHONENUMBER_LENGTH)
                .IsRequired();
        });
        
        builder.ComplexProperty(v => v.PetsName, pm =>
        {
            pm.Property(p => p.Value)
                .HasColumnName("pets_name")
                .HasMaxLength(ProjectConstants.MAX_LOW_TEXT_LENGTH)
                .IsRequired();
        });
        
        builder.ComplexProperty(v => v.PositionNumber, pm =>
        {
            pm.Property(p => p.Value)
                .HasColumnName("pets_position_number")
                .IsRequired();
        });
        
        builder.Property(p => p.Height)
            .IsRequired();
        
        builder.Property(p => p.Weight)
            .IsRequired();

        builder.Property(p => p.IsSterilized)
            .IsRequired();
        
        builder.Property(p => p.IsVaccinated)
            .IsRequired();
        
        builder.Property(p => p.DateOfBirth)
            .IsRequired();
        
        builder.Property(p => p.PetsPageCreationDate)
            .IsRequired();
        
        builder.Property(p => p.Gender)
            .IsRequired();

        builder.ComplexProperty(p => p.SpecieDetails, psd =>
        {
            psd.Property(p => p.BreedId)
                .IsRequired();
        });
        
        builder.OwnsOne(p => p.LocationAddress, la =>
        {
            la.ToJson();
            
            la.Property(p => p.Region)
                .IsRequired()
                .HasMaxLength(ProjectConstants.MAX_LOW_TEXT_LENGTH);
            
            la.Property(p => p.City)
                .IsRequired()
                .HasMaxLength(ProjectConstants.MAX_LOW_TEXT_LENGTH);
            
            la.Property(p => p.Street)
                .IsRequired()
                .HasMaxLength(ProjectConstants.MAX_LOW_TEXT_LENGTH);
            
            la.Property(p => p.HouseNumber)
                .IsRequired()
                .HasMaxLength(ProjectConstants.MAX_LOW_TEXT_LENGTH);
            
            la.Property(p => p.Floor)
                .IsRequired()
                .HasMaxLength(ProjectConstants.MAX_LOW_TEXT_LENGTH);
            
            la.Property(p => p.Apartment)
                .IsRequired()
                .HasMaxLength(ProjectConstants.MAX_LOW_TEXT_LENGTH);
                
        });

        builder.OwnsOne(p => p.PhotosList, ppb =>
        {
            ppb.ToJson();

            ppb.OwnsMany(pp => pp.PetPhotos, pb =>
            {
                pb.Property(p => p.IsMain)
                    .IsRequired();

                pb.Property(p => p.FilePath)
                    .IsRequired();
            });
        });
        
        builder.OwnsOne(v => v.DonateForHelpInfos, db =>
        {
            db.ToJson();

            db.OwnsMany(s => s.DonationInfos, m =>
            {
                m.Property(sm => sm.Name)
                    .IsRequired();

                m.Property(sm => sm.Description)
                    .IsRequired();
            });
        });
        
        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}