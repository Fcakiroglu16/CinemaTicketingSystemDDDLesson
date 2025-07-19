using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaTicketingSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ddddxd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                schema: "scheduling",
                table: "MovieSchedules");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                schema: "scheduling",
                table: "MovieSchedules");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                schema: "scheduling",
                table: "MovieSchedules");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                schema: "scheduling",
                table: "MovieSchedules");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "scheduling",
                table: "MovieSchedules");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                schema: "scheduling",
                table: "MovieSchedules");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                schema: "scheduling",
                table: "MovieSchedules");

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                schema: "scheduling",
                table: "MovieSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SupportedTechnology",
                schema: "scheduling",
                table: "MovieSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CinemaHallSchedules",
                schema: "scheduling",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CinemaHallId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupportedTechnologies = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CinemaHallSchedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CinemaHallScheduleMovieSchedule",
                schema: "scheduling",
                columns: table => new
                {
                    CinemaHallSchedulesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MovieSchedulesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CinemaHallScheduleMovieSchedule", x => new { x.CinemaHallSchedulesId, x.MovieSchedulesId });
                    table.ForeignKey(
                        name: "FK_CinemaHallScheduleMovieSchedule_CinemaHallSchedules_CinemaHallSchedulesId",
                        column: x => x.CinemaHallSchedulesId,
                        principalSchema: "scheduling",
                        principalTable: "CinemaHallSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CinemaHallScheduleMovieSchedule_MovieSchedules_MovieSchedulesId",
                        column: x => x.MovieSchedulesId,
                        principalSchema: "scheduling",
                        principalTable: "MovieSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CinemaHallScheduleMovieSchedule_MovieSchedulesId",
                schema: "scheduling",
                table: "CinemaHallScheduleMovieSchedule",
                column: "MovieSchedulesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CinemaHallScheduleMovieSchedule",
                schema: "scheduling");

            migrationBuilder.DropTable(
                name: "CinemaHallSchedules",
                schema: "scheduling");

            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                schema: "scheduling",
                table: "MovieSchedules");

            migrationBuilder.DropColumn(
                name: "SupportedTechnology",
                schema: "scheduling",
                table: "MovieSchedules");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                schema: "scheduling",
                table: "MovieSchedules",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                schema: "scheduling",
                table: "MovieSchedules",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                schema: "scheduling",
                table: "MovieSchedules",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                schema: "scheduling",
                table: "MovieSchedules",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "scheduling",
                table: "MovieSchedules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                schema: "scheduling",
                table: "MovieSchedules",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                schema: "scheduling",
                table: "MovieSchedules",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
