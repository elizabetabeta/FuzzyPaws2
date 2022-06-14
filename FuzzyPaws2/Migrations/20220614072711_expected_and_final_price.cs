using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuzzyPaws2.Migrations
{
    public partial class expected_and_final_price : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Appointments",
                newName: "FinalPrice");

            migrationBuilder.AddColumn<int>(
                name: "ExpectedPrice",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpectedPrice",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "FinalPrice",
                table: "Appointments",
                newName: "Price");

        }
    }
}
