using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Discussions.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Discussions_Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "PetFamily_Discussions");

            migrationBuilder.CreateTable(
                name: "relations",
                schema: "PetFamily_Discussions",
                columns: table => new
                {
                    relation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_relations", x => x.relation_id);
                });

            migrationBuilder.CreateTable(
                name: "discussions",
                schema: "PetFamily_Discussions",
                columns: table => new
                {
                    discussion_id = table.Column<Guid>(type: "uuid", nullable: false),
                    relation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    first_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    second_user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_discussions", x => x.discussion_id);
                    table.ForeignKey(
                        name: "fk_discussions_relation_relation_id",
                        column: x => x.relation_id,
                        principalSchema: "PetFamily_Discussions",
                        principalTable: "relations",
                        principalColumn: "relation_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "messages",
                schema: "PetFamily_Discussions",
                columns: table => new
                {
                    message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    message_text = table.Column<string>(type: "text", nullable: false),
                    sender_id = table.Column<Guid>(type: "uuid", nullable: false),
                    discussion_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    is_edited = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_messages", x => x.message_id);
                    table.ForeignKey(
                        name: "fk_messages_discussions_discussion_id",
                        column: x => x.discussion_id,
                        principalSchema: "PetFamily_Discussions",
                        principalTable: "discussions",
                        principalColumn: "discussion_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_discussions_relation_id",
                schema: "PetFamily_Discussions",
                table: "discussions",
                column: "relation_id");

            migrationBuilder.CreateIndex(
                name: "ix_messages_discussion_id",
                schema: "PetFamily_Discussions",
                table: "messages",
                column: "discussion_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "messages",
                schema: "PetFamily_Discussions");

            migrationBuilder.DropTable(
                name: "discussions",
                schema: "PetFamily_Discussions");

            migrationBuilder.DropTable(
                name: "relations",
                schema: "PetFamily_Discussions");
        }
    }
}
