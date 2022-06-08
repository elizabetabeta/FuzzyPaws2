using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuzzyPaws2.Migrations
{
    public partial class userId_to_pet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isSold",
                table: "MyPets");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "MyPets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_MyPets_UserId",
                table: "MyPets",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MyPets_AspNetUsers_UserId",
                table: "MyPets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyPets_AspNetUsers_UserId",
                table: "MyPets");

            migrationBuilder.DropIndex(
                name: "IX_MyPets_UserId",
                table: "MyPets");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MyPets");

            migrationBuilder.AddColumn<bool>(
                name: "isSold",
                table: "MyPets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
