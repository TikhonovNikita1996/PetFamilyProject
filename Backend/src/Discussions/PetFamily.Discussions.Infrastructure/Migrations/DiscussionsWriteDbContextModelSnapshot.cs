﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PetFamily.Discussions.Infrastructure.DataContexts;

#nullable disable

namespace PetFamily.Discussions.Infrastructure.Migrations
{
    [DbContext(typeof(DiscussionsWriteDbContext))]
    partial class DiscussionsWriteDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("PetFamily_Discussions")
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PetFamily.Discussions.Domain.Discussion", b =>
                {
                    b.Property<Guid>("DiscussionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("discussion_id");

                    b.Property<Guid>("RelationId")
                        .HasColumnType("uuid")
                        .HasColumnName("relation_id");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.ComplexProperty<Dictionary<string, object>>("DiscussionUsers", "PetFamily.Discussions.Domain.Discussion.DiscussionUsers#DiscussionUsers", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<Guid>("FirstUserId")
                                .HasColumnType("uuid")
                                .HasColumnName("first_user_id");

                            b1.Property<Guid>("SecondUserId")
                                .HasColumnType("uuid")
                                .HasColumnName("second_user_id");
                        });

                    b.HasKey("DiscussionId")
                        .HasName("pk_discussions");

                    b.HasIndex("RelationId")
                        .HasDatabaseName("ix_discussions_relation_id");

                    b.ToTable("discussions", "PetFamily_Discussions");
                });

            modelBuilder.Entity("PetFamily.Discussions.Domain.Message", b =>
                {
                    b.Property<Guid>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("message_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid>("DiscussionId")
                        .HasColumnType("uuid")
                        .HasColumnName("discussion_id");

                    b.Property<bool>("IsEdited")
                        .HasColumnType("boolean")
                        .HasColumnName("is_edited");

                    b.Property<string>("MessageText")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("message_text");

                    b.Property<Guid>("SenderId")
                        .HasColumnType("uuid")
                        .HasColumnName("sender_id");

                    b.HasKey("MessageId")
                        .HasName("pk_messages");

                    b.HasIndex("DiscussionId")
                        .HasDatabaseName("ix_messages_discussion_id");

                    b.ToTable("messages", "PetFamily_Discussions");
                });

            modelBuilder.Entity("PetFamily.Discussions.Domain.Relation", b =>
                {
                    b.Property<Guid>("RelationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("relation_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("RelationId")
                        .HasName("pk_relations");

                    b.ToTable("relations", "PetFamily_Discussions");
                });

            modelBuilder.Entity("PetFamily.Discussions.Domain.Discussion", b =>
                {
                    b.HasOne("PetFamily.Discussions.Domain.Relation", "Relation")
                        .WithMany("Discussions")
                        .HasForeignKey("RelationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_discussions_relation_relation_id");

                    b.Navigation("Relation");
                });

            modelBuilder.Entity("PetFamily.Discussions.Domain.Message", b =>
                {
                    b.HasOne("PetFamily.Discussions.Domain.Discussion", null)
                        .WithMany("Messages")
                        .HasForeignKey("DiscussionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_messages_discussions_discussion_id");
                });

            modelBuilder.Entity("PetFamily.Discussions.Domain.Discussion", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("PetFamily.Discussions.Domain.Relation", b =>
                {
                    b.Navigation("Discussions");
                });
#pragma warning restore 612, 618
        }
    }
}
