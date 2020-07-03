using Microsoft.EntityFrameworkCore.Migrations;

namespace PE.API.Data.Migrations
{
    public partial class ChangedRaiderDtoRaiderNoteDtoIdsInteraction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RaiderNotesDto_AspNetUsers_CreatorId",
                table: "RaiderNotesDto");

            migrationBuilder.DropForeignKey(
                name: "FK_RaiderNotesDto_RaidersDto_RaiderId",
                table: "RaiderNotesDto");

            migrationBuilder.DropForeignKey(
                name: "FK_RaidersDto_RostersDto_RosterId",
                table: "RaidersDto");

            migrationBuilder.DropIndex(
                name: "IX_RaidersDto_RosterId",
                table: "RaidersDto");

            migrationBuilder.DropIndex(
                name: "IX_RaiderNotesDto_CreatorId",
                table: "RaiderNotesDto");

            migrationBuilder.DropIndex(
                name: "IX_RaiderNotesDto_RaiderId",
                table: "RaiderNotesDto");

            migrationBuilder.DropColumn(
                name: "RosterId",
                table: "RaidersDto");

            migrationBuilder.DropColumn(
                name: "RaiderId",
                table: "RaiderNotesDto");

            migrationBuilder.AddColumn<string>(
                name: "RaiderId",
                table: "RaidersDto",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "RaiderNotesDto",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "RaiderNotesDto",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RaiderNoteId",
                table: "RaiderNotesDto",
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

            migrationBuilder.AddForeignKey(
                name: "FK_RaiderNotesDto_RaidersDto_Id",
                table: "RaiderNotesDto",
                column: "Id",
                principalTable: "RaidersDto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RaidersDto_RostersDto_Id",
                table: "RaidersDto",
                column: "Id",
                principalTable: "RostersDto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RaiderNotesDto_AspNetUsers_CreatedById",
                table: "RaiderNotesDto");

            migrationBuilder.DropForeignKey(
                name: "FK_RaiderNotesDto_RaidersDto_Id",
                table: "RaiderNotesDto");

            migrationBuilder.DropForeignKey(
                name: "FK_RaidersDto_RostersDto_Id",
                table: "RaidersDto");

            migrationBuilder.DropIndex(
                name: "IX_RaiderNotesDto_CreatedById",
                table: "RaiderNotesDto");

            migrationBuilder.DropColumn(
                name: "RaiderId",
                table: "RaidersDto");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "RaiderNotesDto");

            migrationBuilder.DropColumn(
                name: "RaiderNoteId",
                table: "RaiderNotesDto");

            migrationBuilder.AddColumn<string>(
                name: "RosterId",
                table: "RaidersDto",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "RaiderNotesDto",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RaiderId",
                table: "RaiderNotesDto",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RaidersDto_RosterId",
                table: "RaidersDto",
                column: "RosterId");

            migrationBuilder.CreateIndex(
                name: "IX_RaiderNotesDto_CreatorId",
                table: "RaiderNotesDto",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_RaiderNotesDto_RaiderId",
                table: "RaiderNotesDto",
                column: "RaiderId");

            migrationBuilder.AddForeignKey(
                name: "FK_RaiderNotesDto_AspNetUsers_CreatorId",
                table: "RaiderNotesDto",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RaiderNotesDto_RaidersDto_RaiderId",
                table: "RaiderNotesDto",
                column: "RaiderId",
                principalTable: "RaidersDto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RaidersDto_RostersDto_RosterId",
                table: "RaidersDto",
                column: "RosterId",
                principalTable: "RostersDto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
