using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuzzyPaws2.Migrations
{
    public partial class rename_row : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_MyPets_PetId",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "PetId",
                table: "Appointments",
                newName: "MyPetId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_PetId",
                table: "Appointments",
                newName: "IX_Appointments_MyPetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_MyPets_MyPetId",
                table: "Appointments",
                column: "MyPetId",
                principalTable: "MyPets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_MyPets_MyPetId",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "MyPetId",
                table: "Appointments",
                newName: "PetId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_MyPetId",
                table: "Appointments",
                newName: "IX_Appointments_PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_MyPets_PetId",
                table: "Appointments",
                column: "PetId",
                principalTable: "MyPets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
