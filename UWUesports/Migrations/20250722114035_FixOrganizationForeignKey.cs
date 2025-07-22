using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UWUesports.Web.Migrations
{
    /// <inheritdoc />
    public partial class FixOrganizationForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Dodaj domyślną organizację i pobierz jej Id
            migrationBuilder.Sql(@"
        INSERT INTO ""Organization"" (""Name"") VALUES ('Brak organizacji');
    ");

            // 2. Pobierz Id nowej organizacji (w PostgreSQL można to zrobić tak)
            migrationBuilder.Sql(@"
        UPDATE ""Teams"" SET ""OrganizationId"" = (SELECT ""Id"" FROM ""Organization"" WHERE ""Name"" = 'Brak organizacji') WHERE ""OrganizationId"" IS NULL;
    ");

            // 3. Ustaw kolumnę OrganizationId na NOT NULL
            migrationBuilder.AlterColumn<int>(
                name: "OrganizationId",
                table: "Teams",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            // 4. Dodaj klucz obcy z ON DELETE CASCADE (lub innym trybem, który chcesz)
            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Organization_OrganizationId",
                table: "Teams",
                column: "OrganizationId",
                principalTable: "Organization",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
       name: "FK_Teams_Organization_OrganizationId",
       table: "Teams");

            migrationBuilder.AlterColumn<int>(
                name: "OrganizationId",
                table: "Teams",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            // Opcjonalnie usuń organizację "Brak organizacji" jeśli chcesz (uwaga: mogą być zależności!)
            migrationBuilder.Sql(@"
        DELETE FROM ""Organization"" WHERE ""Name"" = 'Brak organizacji';
    ");
        }
    }
}
