using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaTicketingSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class xxxddddfdf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TicketPrice_Amount",
                schema: "Ticketing",
                table: "Ticket",
                type: "decimal(9,2)",
                precision: 9,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "TicketPrice_Currency",
                schema: "Ticketing",
                table: "Ticket",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketPrice_Amount",
                schema: "Ticketing",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "TicketPrice_Currency",
                schema: "Ticketing",
                table: "Ticket");
        }
    }
}
