using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaTicketingSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class j : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCurrentlyShowing",
                schema: "cinema_mgmt",
                table: "Movies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ShowingEndDate",
                schema: "cinema_mgmt",
                table: "Movies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ShowingStartDate",
                schema: "cinema_mgmt",
                table: "Movies",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCurrentlyShowing",
                schema: "cinema_mgmt",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ShowingEndDate",
                schema: "cinema_mgmt",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ShowingStartDate",
                schema: "cinema_mgmt",
                table: "Movies");
        }
    }
}
