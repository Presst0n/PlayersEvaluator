using Microsoft.EntityFrameworkCore.Migrations;

namespace PE.API.Data.Migrations
{
    public partial class AddedonepropertytoRaiderNote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorName",
                table: "RaiderNotesDto",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorName",
                table: "RaiderNotesDto");
        }
    }
}
