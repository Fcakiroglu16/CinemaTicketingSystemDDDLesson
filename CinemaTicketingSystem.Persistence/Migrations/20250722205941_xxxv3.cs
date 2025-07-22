using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaTicketingSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class xxxv3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "catalogs");

            migrationBuilder.RenameTable(
                name: "Seats",
                schema: "cinema_mgmt",
                newName: "Seats",
                newSchema: "catalogs");

            migrationBuilder.RenameTable(
                name: "Movies",
                schema: "cinema_mgmt",
                newName: "Movies",
                newSchema: "catalogs");

            migrationBuilder.RenameTable(
                name: "Cinemas",
                schema: "cinema_mgmt",
                newName: "Cinemas",
                newSchema: "catalogs");

            migrationBuilder.RenameTable(
                name: "CinemaHalls",
                schema: "cinema_mgmt",
                newName: "CinemaHalls",
                newSchema: "catalogs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cinema_mgmt");

            migrationBuilder.RenameTable(
                name: "Seats",
                schema: "catalogs",
                newName: "Seats",
                newSchema: "cinema_mgmt");

            migrationBuilder.RenameTable(
                name: "Movies",
                schema: "catalogs",
                newName: "Movies",
                newSchema: "cinema_mgmt");

            migrationBuilder.RenameTable(
                name: "Cinemas",
                schema: "catalogs",
                newName: "Cinemas",
                newSchema: "cinema_mgmt");

            migrationBuilder.RenameTable(
                name: "CinemaHalls",
                schema: "catalogs",
                newName: "CinemaHalls",
                newSchema: "cinema_mgmt");
        }
    }
}
