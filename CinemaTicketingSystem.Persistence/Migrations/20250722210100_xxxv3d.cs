using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaTicketingSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class xxxv3d : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Schedules",
                newName: "Schedules",
                newSchema: "scheduling");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Schedules",
                schema: "scheduling",
                newName: "Schedules");
        }
    }
}
