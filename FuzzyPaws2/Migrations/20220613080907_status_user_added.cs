using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FuzzyPaws2.Migrations
{
    public partial class status_user_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "Appointments");
        }
    }
}
