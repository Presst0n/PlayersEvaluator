using Microsoft.EntityFrameworkCore.Migrations;

namespace PE.API.Data.Migrations
{
    public partial class ChangingPrimaryKeysinRaiderDtoRaiderNoteDto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RaiderNotesDto_RaidersDto_Id",
                table: "RaiderNotesDto");

            migrationBuilder.DropForeignKey(
                name: "FK_RaidersDto_RostersDto_Id",
                table: "RaidersDto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RaidersDto",
                table: "RaidersDto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RaiderNotesDto",
                table: "RaiderNotesDto");

            migrationBuilder.AlterColumn<string>(
                name: "RaiderId",
                table: "RaidersDto",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "RaidersDto",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "RaiderNoteId",
                table: "RaiderNotesDto",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "RaiderNotesDto",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RaidersDto",
                table: "RaidersDto",
                column: "RaiderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RaiderNotesDto",
                table: "RaiderNotesDto",
                column: "RaiderNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_RaidersDto_Id",
                table: "RaidersDto",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RaiderNotesDto_Id",
                table: "RaiderNotesDto",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RaiderNotesDto_RaidersDto_Id",
                table: "RaiderNotesDto",
                column: "Id",
                principalTable: "RaidersDto",
                principalColumn: "RaiderId",
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
                name: "FK_RaiderNotesDto_RaidersDto_Id",
                table: "RaiderNotesDto");

            migrationBuilder.DropForeignKey(
                name: "FK_RaidersDto_RostersDto_Id",
                table: "RaidersDto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RaidersDto",
                table: "RaidersDto");

            migrationBuilder.DropIndex(
                name: "IX_RaidersDto_Id",
                table: "RaidersDto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RaiderNotesDto",
                table: "RaiderNotesDto");

            migrationBuilder.DropIndex(
                name: "IX_RaiderNotesDto_Id",
                table: "RaiderNotesDto");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "RaidersDto",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RaiderId",
                table: "RaidersDto",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "RaiderNotesDto",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RaiderNoteId",
                table: "RaiderNotesDto",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_RaidersDto",
                table: "RaidersDto",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RaiderNotesDto",
                table: "RaiderNotesDto",
                column: "Id");

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
    }
}
