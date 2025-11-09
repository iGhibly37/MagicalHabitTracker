using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicalHabitTracker.Migrations
{
    /// <inheritdoc />
    public partial class ModificheController_e_aggiunti_servizi_rest_Utenti : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Habits_Name_UserId",
                table: "Habits",
                columns: new[] { "Name", "UserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Habits_Name_UserId",
                table: "Habits");
        }
    }
}
