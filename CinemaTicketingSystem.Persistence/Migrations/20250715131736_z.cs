using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaTicketingSystem.Persistence.Migrations
{
    /// <inheritdoc />
#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
    public partial class z : Migration
#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservedSeat_SeatReservations_SeatReservationId",
                table: "ReservedSeat");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReservedSeat",
                table: "ReservedSeat");

            migrationBuilder.RenameTable(
                name: "SeatReservations",
                newName: "SeatReservations",
                newSchema: "Ticketing");

            migrationBuilder.RenameTable(
                name: "ReservedSeat",
                newName: "ReservedSeats",
                newSchema: "Ticketing");

            migrationBuilder.RenameIndex(
                name: "IX_ReservedSeat_SeatReservationId",
                schema: "Ticketing",
                table: "ReservedSeats",
                newName: "IX_ReservedSeats_SeatReservationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReservedSeats",
                schema: "Ticketing",
                table: "ReservedSeats",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservedSeats_SeatReservations_SeatReservationId",
                schema: "Ticketing",
                table: "ReservedSeats",
                column: "SeatReservationId",
                principalSchema: "Ticketing",
                principalTable: "SeatReservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservedSeats_SeatReservations_SeatReservationId",
                schema: "Ticketing",
                table: "ReservedSeats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReservedSeats",
                schema: "Ticketing",
                table: "ReservedSeats");

            migrationBuilder.RenameTable(
                name: "SeatReservations",
                schema: "Ticketing",
                newName: "SeatReservations");

            migrationBuilder.RenameTable(
                name: "ReservedSeats",
                schema: "Ticketing",
                newName: "ReservedSeat");

            migrationBuilder.RenameIndex(
                name: "IX_ReservedSeats_SeatReservationId",
                table: "ReservedSeat",
                newName: "IX_ReservedSeat_SeatReservationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReservedSeat",
                table: "ReservedSeat",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservedSeat_SeatReservations_SeatReservationId",
                table: "ReservedSeat",
                column: "SeatReservationId",
                principalTable: "SeatReservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
