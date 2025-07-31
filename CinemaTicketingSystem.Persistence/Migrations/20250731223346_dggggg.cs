using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaTicketingSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class dggggg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketSales_MovieTickets_MovieTicketId",
                schema: "Ticketing",
                table: "TicketSales");

            migrationBuilder.DropTable(
                name: "MovieTickets",
                schema: "Ticketing");

            migrationBuilder.RenameColumn(
                name: "MovieTicketId",
                schema: "Ticketing",
                table: "TicketSales",
                newName: "TicketPurchaseId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketSales_MovieTicketId",
                schema: "Ticketing",
                table: "TicketSales",
                newName: "IX_TicketSales_TicketPurchaseId");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                schema: "scheduling",
                table: "Schedules",
                type: "decimal(9,2)",
                precision: 9,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                schema: "scheduling",
                table: "Schedules",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TicketPurchases",
                schema: "Ticketing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDiscountApplied = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketPurchases", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_TicketSales_TicketPurchases_TicketPurchaseId",
                schema: "Ticketing",
                table: "TicketSales",
                column: "TicketPurchaseId",
                principalSchema: "Ticketing",
                principalTable: "TicketPurchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketSales_TicketPurchases_TicketPurchaseId",
                schema: "Ticketing",
                table: "TicketSales");

            migrationBuilder.DropTable(
                name: "TicketPurchases",
                schema: "Ticketing");

            migrationBuilder.DropColumn(
                name: "Amount",
                schema: "scheduling",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "Currency",
                schema: "scheduling",
                table: "Schedules");

            migrationBuilder.RenameColumn(
                name: "TicketPurchaseId",
                schema: "Ticketing",
                table: "TicketSales",
                newName: "MovieTicketId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketSales_TicketPurchaseId",
                schema: "Ticketing",
                table: "TicketSales",
                newName: "IX_TicketSales_MovieTicketId");

            migrationBuilder.CreateTable(
                name: "MovieTickets",
                schema: "Ticketing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDiscountApplied = table.Column<bool>(type: "bit", nullable: false),
                    MovieHallIdId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieTickets", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_TicketSales_MovieTickets_MovieTicketId",
                schema: "Ticketing",
                table: "TicketSales",
                column: "MovieTicketId",
                principalSchema: "Ticketing",
                principalTable: "MovieTickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
