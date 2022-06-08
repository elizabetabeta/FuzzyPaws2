using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuzzyPaws2.Migrations
{
    public partial class my_pet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MyPets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PetTypeId = table.Column<int>(type: "int", nullable: false),
                    PetBreedId = table.Column<int>(type: "int", nullable: false),
                    isSold = table.Column<bool>(type: "bit", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyPets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyPets_PetBreed_PetBreedId",
                        column: x => x.PetBreedId,
                        principalTable: "PetBreed",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MyPets_PetType_PetTypeId",
                        column: x => x.PetTypeId,
                        principalTable: "PetType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MyPets_PetBreedId",
                table: "MyPets",
                column: "PetBreedId");

            migrationBuilder.CreateIndex(
                name: "IX_MyPets_PetTypeId",
                table: "MyPets",
                column: "PetTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MyPets");
        }
    }
}
