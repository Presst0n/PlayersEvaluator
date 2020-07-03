using Microsoft.EntityFrameworkCore.Migrations;

namespace PE.API.Data.Migrations
{
    public partial class ChangedRaiderDtoRaiderNoteDtoIdsInteraction20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RaiderNotesDto_AspNetUsers_CreatedById",
                table: "RaiderNotesDto");

            migrationBuilder.DropIndex(
                name: "IX_RaiderNotesDto_CreatedById",
                table: "RaiderNotesDto");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "RaiderNotesDto");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "RaiderNotesDto",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RaiderNotesDto_CreatedById",
                table: "RaiderNotesDto",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_RaiderNotesDto_AspNetUsers_CreatedById",
                table: "RaiderNotesDto",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
