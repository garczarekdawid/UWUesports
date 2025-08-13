using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UWUesports.Web.Migrations
{
    /// <inheritdoc />
    public partial class dodanieRolWlasnychDoUserow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoleAssignments_AspNetRoles_RoleId",
                table: "UserRoleAssignments");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoleAssignments_OrganizationRoles_RoleId",
                table: "UserRoleAssignments",
                column: "RoleId",
                principalTable: "OrganizationRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoleAssignments_OrganizationRoles_RoleId",
                table: "UserRoleAssignments");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoleAssignments_AspNetRoles_RoleId",
                table: "UserRoleAssignments",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
