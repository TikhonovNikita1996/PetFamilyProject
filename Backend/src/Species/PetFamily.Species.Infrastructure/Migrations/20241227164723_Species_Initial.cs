using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Species.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Species_Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "PetFamily_Species");

            migrationBuilder.CreateTable(
                name: "species",
                schema: "PetFamily_Species",
                columns: table => new
                {
                    specie_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_species", x => x.specie_id);
                });

            migrationBuilder.CreateTable(
                name: "breed",
                schema: "PetFamily_Species",
                columns: table => new
                {
                    breed_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    specie_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_breed", x => x.breed_id);
                    table.ForeignKey(
                        name: "fk_breed_species_specie_id",
                        column: x => x.specie_id,
                        principalSchema: "PetFamily_Species",
                        principalTable: "species",
                        principalColumn: "specie_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_breed_specie_id",
                schema: "PetFamily_Species",
                table: "breed",
                column: "specie_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "breed",
                schema: "PetFamily_Species");

            migrationBuilder.DropTable(
                name: "species",
                schema: "PetFamily_Species");
        }
    }
}
