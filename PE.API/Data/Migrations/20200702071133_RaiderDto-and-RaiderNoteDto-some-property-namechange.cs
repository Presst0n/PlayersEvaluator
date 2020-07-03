using Microsoft.EntityFrameworkCore.Migrations;

namespace PE.API.Data.Migrations
{
    public partial class RaiderDtoandRaiderNoteDtosomepropertynamechange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RaiderNotesDto_RaidersDto_Id",
                table: "RaiderNotesDto");

            migrationBuilder.DropForeignKey(
                name: "FK_RaidersDto_RostersDto_Id",
                table: "RaidersDto");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokensDto_AspNetUsers_UserId",
                table: "RefreshTokensDto");

            migrationBuilder.DropForeignKey(
                name: "FK_RostersDto_AspNetUsers_CreatorId",
                table: "RostersDto");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRosterAccessesDto_AspNetUsers_CreatorId",
                table: "UserRosterAccessesDto");

            migrationBuilder.DropIndex(
                name: "IX_RaidersDto_Id",
                table: "RaidersDto");

            migrationBuilder.DropIndex(
                name: "IX_RaiderNotesDto_Id",
                table: "RaiderNotesDto");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RaidersDto");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RaiderNotesDto");

            migrationBuilder.AddColumn<string>(
                name: "RosterId",
                table: "RaidersDto",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RaiderId",
                table: "RaiderNotesDto",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RaidersDto_RosterId",
                table: "RaidersDto",
                column: "RosterId");

            migrationBuilder.CreateIndex(
                name: "IX_RaiderNotesDto_RaiderId",
                table: "RaiderNotesDto",
                column: "RaiderId");

            migrationBuilder.AddForeignKey(
                name: "FK_RaiderNotesDto_RaidersDto_RaiderId",
                table: "RaiderNotesDto",
                column: "RaiderId",
                principalTable: "RaidersDto",
                principalColumn: "RaiderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RaidersDto_RostersDto_RosterId",
                table: "RaidersDto",
                column: "RosterId",
                principalTable: "RostersDto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokensDto_AspNetUsers_UserId",
                table: "RefreshTokensDto",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RostersDto_AspNetUsers_CreatorId",
                table: "RostersDto",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRosterAccessesDto_AspNetUsers_CreatorId",
                table: "UserRosterAccessesDto",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RaiderNotesDto_RaidersDto_RaiderId",
                table: "RaiderNotesDto");

            migrationBuilder.DropForeignKey(
                name: "FK_RaidersDto_RostersDto_RosterId",
                table: "RaidersDto");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokensDto_AspNetUsers_UserId",
                table: "RefreshTokensDto");

            migrationBuilder.DropForeignKey(
                name: "FK_RostersDto_AspNetUsers_CreatorId",
                table: "RostersDto");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRosterAccessesDto_AspNetUsers_CreatorId",
                table: "UserRosterAccessesDto");

            migrationBuilder.DropIndex(
                name: "IX_RaidersDto_RosterId",
                table: "RaidersDto");

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
                name: "Id",
                table: "RaidersDto",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "RaiderNotesDto",
                type: "nvarchar(450)",
                nullable: true);

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
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RaidersDto_RostersDto_Id",
                table: "RaidersDto",
                column: "Id",
                principalTable: "RostersDto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokensDto_AspNetUsers_UserId",
                table: "RefreshTokensDto",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RostersDto_AspNetUsers_CreatorId",
                table: "RostersDto",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRosterAccessesDto_AspNetUsers_CreatorId",
                table: "UserRosterAccessesDto",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
