using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaTicketingSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class xxxddd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                schema: "Ticketing",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "Currency",
                schema: "Ticketing",
                table: "Ticket");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                schema: "Ticketing",
                table: "Ticket",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                schema: "Ticketing",
                table: "Ticket",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");
        }
    }
}
