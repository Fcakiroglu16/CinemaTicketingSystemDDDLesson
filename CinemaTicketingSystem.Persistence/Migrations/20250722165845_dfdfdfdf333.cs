using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaTicketingSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class dfdfdfdf333 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShowTimes_MovieSchedules_MovieScheduleId",
                schema: "scheduling",
                table: "ShowTimes");

            migrationBuilder.DropTable(
                name: "MovieSchedules",
                schema: "scheduling");

            migrationBuilder.DropTable(
                name: "CinemaHallSchedules",
                schema: "scheduling");

            migrationBuilder.DropIndex(
                name: "IX_ShowTimes_MovieScheduleId",
                schema: "scheduling",
                table: "ShowTimes");

            migrationBuilder.DropColumn(
                name: "MovieScheduleId",
                schema: "scheduling",
                table: "ShowTimes");

            migrationBuilder.CreateTable(
                name: "CinemaHallSnapshots",
                schema: "scheduling",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CinemaHallId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupportedTechnologies = table.Column<int>(type: "int", nullable: false),
                    SeatCount = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CinemaHallSnapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MovieSnapshot",
                schema: "scheduling",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MovieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    SupportedTechnology = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieSnapshot", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MovieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HallId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShowTimeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedules_ShowTimes_ShowTimeId",
                        column: x => x.ShowTimeId,
                        principalSchema: "scheduling",
                        principalTable: "ShowTimes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_ShowTimeId",
                table: "Schedules",
                column: "ShowTimeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CinemaHallSnapshots",
                schema: "scheduling");

            migrationBuilder.DropTable(
                name: "MovieSnapshot",
                schema: "scheduling");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.AddColumn<Guid>(
                name: "MovieScheduleId",
                schema: "scheduling",
                table: "ShowTimes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "CinemaHallSchedules",
                schema: "scheduling",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CinemaHallId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SeatCount = table.Column<short>(type: "smallint", nullable: false),
                    SupportedTechnologies = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CinemaHallSchedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MovieSchedules",
                schema: "scheduling",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CinemaHallScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MovieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupportedTechnology = table.Column<int>(type: "int", nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieSchedules_CinemaHallSchedules_CinemaHallScheduleId",
                        column: x => x.CinemaHallScheduleId,
                        principalSchema: "scheduling",
                        principalTable: "CinemaHallSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShowTimes_MovieScheduleId",
                schema: "scheduling",
                table: "ShowTimes",
                column: "MovieScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieSchedules_CinemaHallScheduleId",
                schema: "scheduling",
                table: "MovieSchedules",
                column: "CinemaHallScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShowTimes_MovieSchedules_MovieScheduleId",
                schema: "scheduling",
                table: "ShowTimes",
                column: "MovieScheduleId",
                principalSchema: "scheduling",
                principalTable: "MovieSchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
