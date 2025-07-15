using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaTicketingSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Ticketing");

            migrationBuilder.CreateTable(
                name: "MovieTickets",
                schema: "Ticketing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MovieSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDiscountApplied = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieTickets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SeatReservations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MovieSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReservationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatReservations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketSales",
                schema: "Ticketing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MovieTicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Row = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    TicketCode = table.Column<string>(type: "nchar(6)", fixedLength: true, maxLength: 6, nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketSales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketSales_MovieTickets_MovieTicketId",
                        column: x => x.MovieTicketId,
                        principalSchema: "Ticketing",
                        principalTable: "MovieTickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReservedSeat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Row = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    SeatReservationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservedSeat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservedSeat_SeatReservations_SeatReservationId",
                        column: x => x.SeatReservationId,
                        principalTable: "SeatReservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservedSeat_SeatReservationId",
                table: "ReservedSeat",
                column: "SeatReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketSales_MovieTicketId",
                schema: "Ticketing",
                table: "TicketSales",
                column: "MovieTicketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservedSeat");

            migrationBuilder.DropTable(
                name: "TicketSales",
                schema: "Ticketing");

            migrationBuilder.DropTable(
                name: "SeatReservations");

            migrationBuilder.DropTable(
                name: "MovieTickets",
                schema: "Ticketing");
        }
    }
}
