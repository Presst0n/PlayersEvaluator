using Microsoft.EntityFrameworkCore.Migrations;

namespace PE.DataManager.Migrations
{
    public partial class AddedAuthUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthUserId",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthUserId",
                table: "Users");
        }
    }
}
