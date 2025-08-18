using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UWUesports.Web.Migrations
{
    /// <inheritdoc />
    public partial class zmianaNazwyTeamPlayerNaMembership2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamPlayers_AspNetUsers_UserId",
                table: "TeamPlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamPlayers_Teams_TeamId",
                table: "TeamPlayers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamPlayers",
                table: "TeamPlayers");

            migrationBuilder.RenameTable(
                name: "TeamPlayers",
                newName: "Membership");

            migrationBuilder.RenameIndex(
                name: "IX_TeamPlayers_UserId",
                table: "Membership",
                newName: "IX_Membership_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Membership",
                table: "Membership",
                columns: new[] { "TeamId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Membership_AspNetUsers_UserId",
                table: "Membership",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Membership_Teams_TeamId",
                table: "Membership",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Membership_AspNetUsers_UserId",
                table: "Membership");

            migrationBuilder.DropForeignKey(
                name: "FK_Membership_Teams_TeamId",
                table: "Membership");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Membership",
                table: "Membership");

            migrationBuilder.RenameTable(
                name: "Membership",
                newName: "TeamPlayers");

            migrationBuilder.RenameIndex(
                name: "IX_Membership_UserId",
                table: "TeamPlayers",
                newName: "IX_TeamPlayers_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamPlayers",
                table: "TeamPlayers",
                columns: new[] { "TeamId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TeamPlayers_AspNetUsers_UserId",
                table: "TeamPlayers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamPlayers_Teams_TeamId",
                table: "TeamPlayers",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
