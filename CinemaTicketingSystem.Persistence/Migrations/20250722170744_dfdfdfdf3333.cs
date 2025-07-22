using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaTicketingSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class dfdfdfdf3333 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_ShowTimes_ShowTimeId",
                table: "Schedules");

            migrationBuilder.DropTable(
                name: "ShowTimes",
                schema: "scheduling");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_ShowTimeId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "ShowTimeId",
                table: "Schedules");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "ShowTime_EndTime",
                table: "Schedules",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "ShowTime_StartTime",
                table: "Schedules",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowTime_EndTime",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "ShowTime_StartTime",
                table: "Schedules");

            migrationBuilder.AddColumn<Guid>(
                name: "ShowTimeId",
                table: "Schedules",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ShowTimes",
                schema: "scheduling",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowTimes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_ShowTimeId",
                table: "Schedules",
                column: "ShowTimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_ShowTimes_ShowTimeId",
                table: "Schedules",
                column: "ShowTimeId",
                principalSchema: "scheduling",
                principalTable: "ShowTimes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
