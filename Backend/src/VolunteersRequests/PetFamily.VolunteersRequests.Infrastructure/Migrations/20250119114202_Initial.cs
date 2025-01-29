using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.VolunteersRequests.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "PetFamily_VolunteersRequests");

            migrationBuilder.CreateTable(
                name: "volunteer_requests",
                schema: "PetFamily_VolunteersRequests",
                columns: table => new
                {
                    request_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    admin_Id = table.Column<Guid>(type: "uuid", nullable: true),
                    user_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    volunteer_info = table.Column<string>(type: "text", nullable: true),
                    discussion_id = table.Column<Guid>(type: "uuid", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    rejection_comment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_volunteer_requests", x => x.request_Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "volunteer_requests",
                schema: "PetFamily_VolunteersRequests");
        }
    }
}
