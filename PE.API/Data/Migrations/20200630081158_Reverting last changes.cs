using Microsoft.EntityFrameworkCore.Migrations;

namespace PE.API.Data.Migrations
{
    public partial class Revertinglastchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RaidersDto_RostersDto_RaidRosterId",
                table: "RaidersDto");

            migrationBuilder.DropIndex(
                name: "IX_RaidersDto_RaidRosterId",
                table: "RaidersDto");

            migrationBuilder.DropColumn(
                name: "RaidRosterId",
                table: "RaidersDto");

            migrationBuilder.AlterColumn<string>(
                name: "RosterId",
                table: "RaidersDto",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RaidersDto_RosterId",
                table: "RaidersDto",
                column: "RosterId");

            migrationBuilder.AddForeignKey(
                name: "FK_RaidersDto_RostersDto_RosterId",
                table: "RaidersDto",
                column: "RosterId",
                principalTable: "RostersDto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RaidersDto_RostersDto_RosterId",
                table: "RaidersDto");

            migrationBuilder.DropIndex(
                name: "IX_RaidersDto_RosterId",
                table: "RaidersDto");

            migrationBuilder.AlterColumn<string>(
                name: "RosterId",
                table: "RaidersDto",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RaidRosterId",
                table: "RaidersDto",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RaidersDto_RaidRosterId",
                table: "RaidersDto",
                column: "RaidRosterId");

            migrationBuilder.AddForeignKey(
                name: "FK_RaidersDto_RostersDto_RaidRosterId",
                table: "RaidersDto",
                column: "RaidRosterId",
                principalTable: "RostersDto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
