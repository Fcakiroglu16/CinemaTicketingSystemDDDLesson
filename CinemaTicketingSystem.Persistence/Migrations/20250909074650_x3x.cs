using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaTicketingSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class x3x : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price_Amount",
                schema: "Ticketing",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Price_Currency",
                schema: "Ticketing",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "Purchases",
                table: "Purchases",
                newName: "PayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PayerId",
                schema: "Purchases",
                table: "Purchases",
                newName: "UserId");

            migrationBuilder.AddColumn<decimal>(
                name: "Price_Amount",
                schema: "Ticketing",
                table: "Tickets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Price_Currency",
                schema: "Ticketing",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
