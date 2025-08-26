using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaTicketingSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ggg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_TicketIssuance_TicketIssuanceId",
                schema: "Ticketing",
                table: "Ticket");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ticket",
                schema: "Ticketing",
                table: "Ticket");

            migrationBuilder.RenameTable(
                name: "Ticket",
                schema: "Ticketing",
                newName: "Tickets",
                newSchema: "Ticketing");

            migrationBuilder.RenameColumn(
                name: "TicketPrice_Currency",
                schema: "Ticketing",
                table: "Tickets",
                newName: "Price_Currency");

            migrationBuilder.RenameColumn(
                name: "TicketPrice_Amount",
                schema: "Ticketing",
                table: "Tickets",
                newName: "Price_Amount");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_TicketIssuanceId",
                schema: "Ticketing",
                table: "Tickets",
                newName: "IX_Tickets_TicketIssuanceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tickets",
                schema: "Ticketing",
                table: "Tickets",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_TicketIssuance_TicketIssuanceId",
                schema: "Ticketing",
                table: "Tickets",
                column: "TicketIssuanceId",
                principalSchema: "Ticketing",
                principalTable: "TicketIssuance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_TicketIssuance_TicketIssuanceId",
                schema: "Ticketing",
                table: "Tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tickets",
                schema: "Ticketing",
                table: "Tickets");

            migrationBuilder.RenameTable(
                name: "Tickets",
                schema: "Ticketing",
                newName: "Ticket",
                newSchema: "Ticketing");

            migrationBuilder.RenameColumn(
                name: "Price_Currency",
                schema: "Ticketing",
                table: "Ticket",
                newName: "TicketPrice_Currency");

            migrationBuilder.RenameColumn(
                name: "Price_Amount",
                schema: "Ticketing",
                table: "Ticket",
                newName: "TicketPrice_Amount");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_TicketIssuanceId",
                schema: "Ticketing",
                table: "Ticket",
                newName: "IX_Ticket_TicketIssuanceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ticket",
                schema: "Ticketing",
                table: "Ticket",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_TicketIssuance_TicketIssuanceId",
                schema: "Ticketing",
                table: "Ticket",
                column: "TicketIssuanceId",
                principalSchema: "Ticketing",
                principalTable: "TicketIssuance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
