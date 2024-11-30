﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PetFamily.Infrastructure;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20241125164653_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PetFamily.Domain.Entities.Pet.Pet", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("color");

                    b.Property<int>("CurrentStatus")
                        .HasColumnType("integer")
                        .HasColumnName("current_status");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_of_birth");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("description");

                    b.Property<int>("Gender")
                        .HasColumnType("integer")
                        .HasColumnName("gender");

                    b.Property<string>("HealthInformation")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("health_information");

                    b.Property<double>("Height")
                        .HasColumnType("double precision")
                        .HasColumnName("height");

                    b.Property<bool>("IsSterilized")
                        .HasColumnType("boolean")
                        .HasColumnName("is_sterilized");

                    b.Property<bool>("IsVaccinated")
                        .HasColumnType("boolean")
                        .HasColumnName("is_vaccinated");

                    b.Property<string>("OwnersPhoneNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("owners_phone_number");

                    b.Property<string>("PetsName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("pets_name");

                    b.Property<DateTime>("PetsPageCreationDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("pets_page_creation_date");

                    b.Property<double>("Weight")
                        .HasColumnType("double precision")
                        .HasColumnName("weight");

                    b.Property<Guid?>("volunteer_id")
                        .HasColumnType("uuid")
                        .HasColumnName("volunteer_id");

                    b.ComplexProperty<Dictionary<string, object>>("SpecieDetails", "PetFamily.Domain.Entities.Pet.Pet.SpecieDetails#SpicieDetails", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<Guid>("BreedId")
                                .HasColumnType("uuid")
                                .HasColumnName("specie_details_breed_id");
                        });

                    b.HasKey("Id")
                        .HasName("pk_pets");

                    b.HasIndex("volunteer_id")
                        .HasDatabaseName("ix_pets_volunteer_id");

                    b.ToTable("pets", (string)null);
                });

            modelBuilder.Entity("PetFamily.Domain.Entities.Pet.ValueObjects.Breed", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<Guid>("specie_Id")
                        .HasColumnType("uuid")
                        .HasColumnName("specie_id");

                    b.HasKey("Id")
                        .HasName("pk_breed");

                    b.HasIndex("specie_Id")
                        .HasDatabaseName("ix_breed_specie_id");

                    b.ToTable("breed", (string)null);
                });

            modelBuilder.Entity("PetFamily.Domain.Entities.Pet.ValueObjects.Specie", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_species");

                    b.ToTable("species", (string)null);
                });

            modelBuilder.Entity("PetFamily.Domain.Entities.Volunteer.Volunteer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("Gender")
                        .HasColumnType("integer")
                        .HasColumnName("gender");

                    b.ComplexProperty<Dictionary<string, object>>("Description", "PetFamily.Domain.Entities.Volunteer.Volunteer.Description#Description", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(255)
                                .HasColumnType("character varying(255)")
                                .HasColumnName("description");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Email", "PetFamily.Domain.Entities.Volunteer.Volunteer.Email#Email", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("email");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Fullname", "PetFamily.Domain.Entities.Volunteer.Volunteer.Fullname#FullName", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("fullname_last_name");

                            b1.Property<string>("MiddleName")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("fullname_middle_name");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("fullname_name");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("PhoneNumber", "PetFamily.Domain.Entities.Volunteer.Volunteer.PhoneNumber#PhoneNumber", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("character varying(20)")
                                .HasColumnName("phone_number");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("WorkingExperience", "PetFamily.Domain.Entities.Volunteer.Volunteer.WorkingExperience#WorkingExperience", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("Value")
                                .HasColumnType("integer")
                                .HasColumnName("workingExperience");
                        });

                    b.HasKey("Id")
                        .HasName("pk_volunteers");

                    b.ToTable("volunteers", (string)null);
                });

            modelBuilder.Entity("PetFamily.Domain.Entities.Pet.Pet", b =>
                {
                    b.HasOne("PetFamily.Domain.Entities.Volunteer.Volunteer", null)
                        .WithMany("CurrentPets")
                        .HasForeignKey("volunteer_id")
                        .HasConstraintName("fk_pets_volunteers_volunteer_id");

                    b.OwnsOne("PetFamily.Domain.Entities.Others.DonationInfoList", "DonateForHelpInfos", b1 =>
                        {
                            b1.Property<Guid>("PetId")
                                .HasColumnType("uuid");

                            b1.HasKey("PetId")
                                .HasName("pk_pets");

                            b1.ToTable("pets");

                            b1.ToJson("DonateForHelpInfos");

                            b1.WithOwner()
                                .HasForeignKey("PetId")
                                .HasConstraintName("fk_pets_pets_pet_id");

                            b1.OwnsMany("PetFamily.Domain.Entities.Volunteer.ValueObjects.DonationInfo", "DonationInfos", b2 =>
                                {
                                    b2.Property<Guid>("DonationInfoListPetId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<string>("Description")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.Property<string>("Name")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.HasKey("DonationInfoListPetId", "Id")
                                        .HasName("pk_pets");

                                    b2.ToTable("pets");

                                    b2.WithOwner()
                                        .HasForeignKey("DonationInfoListPetId")
                                        .HasConstraintName("fk_pets_pets_donation_info_list_pet_id");
                                });

                            b1.Navigation("DonationInfos");
                        });

                    b.OwnsOne("PetFamily.Domain.Entities.Pet.ValueObjects.Photos", "Photos", b1 =>
                        {
                            b1.Property<Guid>("PetId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.HasKey("PetId");

                            b1.ToTable("pets");

                            b1.ToJson("Photos");

                            b1.WithOwner()
                                .HasForeignKey("PetId")
                                .HasConstraintName("fk_pets_pets_id");

                            b1.OwnsMany("PetFamily.Domain.Entities.Pet.ValueObjects.PetPhoto", "PetPhotos", b2 =>
                                {
                                    b2.Property<Guid>("PhotosPetId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<string>("FilePath")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.Property<bool>("IsMain")
                                        .HasColumnType("boolean");

                                    b2.HasKey("PhotosPetId", "Id")
                                        .HasName("pk_pets");

                                    b2.ToTable("pets");

                                    b2.WithOwner()
                                        .HasForeignKey("PhotosPetId")
                                        .HasConstraintName("fk_pets_pets_photos_pet_id");
                                });

                            b1.Navigation("PetPhotos");
                        });

                    b.OwnsOne("PetFamily.Domain.Entities.Volunteer.ValueObjects.LocationAddress", "LocationAddress", b1 =>
                        {
                            b1.Property<Guid>("PetId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<string>("Apartment")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");

                            b1.Property<string>("Floor")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");

                            b1.Property<string>("HouseNumber")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");

                            b1.Property<string>("Region")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");

                            b1.HasKey("PetId");

                            b1.ToTable("pets");

                            b1.ToJson("LocationAddress");

                            b1.WithOwner()
                                .HasForeignKey("PetId")
                                .HasConstraintName("fk_pets_pets_id");
                        });

                    b.Navigation("DonateForHelpInfos")
                        .IsRequired();

                    b.Navigation("LocationAddress")
                        .IsRequired();

                    b.Navigation("Photos")
                        .IsRequired();
                });

            modelBuilder.Entity("PetFamily.Domain.Entities.Pet.ValueObjects.Breed", b =>
                {
                    b.HasOne("PetFamily.Domain.Entities.Pet.ValueObjects.Specie", null)
                        .WithMany("Breeds")
                        .HasForeignKey("specie_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_breed_species_specie_id");
                });

            modelBuilder.Entity("PetFamily.Domain.Entities.Volunteer.Volunteer", b =>
                {
                    b.OwnsOne("PetFamily.Domain.Entities.Others.SocialMediaDetails", "SocialMediaDetails", b1 =>
                        {
                            b1.Property<Guid>("VolunteerId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.HasKey("VolunteerId");

                            b1.ToTable("volunteers");

                            b1.ToJson("SocialMediaDetails");

                            b1.WithOwner()
                                .HasForeignKey("VolunteerId")
                                .HasConstraintName("fk_volunteers_volunteers_id");

                            b1.OwnsMany("PetFamily.Domain.Entities.Volunteer.ValueObjects.SocialMedia", "SocialMedias", b2 =>
                                {
                                    b2.Property<Guid>("SocialMediaDetailsVolunteerId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<string>("Name")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.Property<string>("Url")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.HasKey("SocialMediaDetailsVolunteerId", "Id")
                                        .HasName("pk_volunteers");

                                    b2.ToTable("volunteers");

                                    b2.WithOwner()
                                        .HasForeignKey("SocialMediaDetailsVolunteerId")
                                        .HasConstraintName("fk_volunteers_volunteers_social_media_details_volunteer_id");
                                });

                            b1.Navigation("SocialMedias");
                        });

                    b.OwnsOne("PetFamily.Domain.Entities.Others.DonationInfoList", "DonateForHelpInfos", b1 =>
                        {
                            b1.Property<Guid>("VolunteerId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.HasKey("VolunteerId");

                            b1.ToTable("volunteers");

                            b1.ToJson("DonateForHelpInfos");

                            b1.WithOwner()
                                .HasForeignKey("VolunteerId")
                                .HasConstraintName("fk_volunteers_volunteers_id");

                            b1.OwnsMany("PetFamily.Domain.Entities.Volunteer.ValueObjects.DonationInfo", "DonationInfos", b2 =>
                                {
                                    b2.Property<Guid>("DonationInfoListVolunteerId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<string>("Description")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.Property<string>("Name")
                                        .IsRequired()
                                        .HasColumnType("text");

                                    b2.HasKey("DonationInfoListVolunteerId", "Id")
                                        .HasName("pk_volunteers");

                                    b2.ToTable("volunteers");

                                    b2.WithOwner()
                                        .HasForeignKey("DonationInfoListVolunteerId")
                                        .HasConstraintName("fk_volunteers_volunteers_donation_info_list_volunteer_id");
                                });

                            b1.Navigation("DonationInfos");
                        });

                    b.Navigation("DonateForHelpInfos")
                        .IsRequired();

                    b.Navigation("SocialMediaDetails")
                        .IsRequired();
                });

            modelBuilder.Entity("PetFamily.Domain.Entities.Pet.ValueObjects.Specie", b =>
                {
                    b.Navigation("Breeds");
                });

            modelBuilder.Entity("PetFamily.Domain.Entities.Volunteer.Volunteer", b =>
                {
                    b.Navigation("CurrentPets");
                });
#pragma warning restore 612, 618
        }
    }
}
