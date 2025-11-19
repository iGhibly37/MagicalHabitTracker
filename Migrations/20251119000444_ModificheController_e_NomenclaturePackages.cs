using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicalHabitTracker.Migrations
{
    /// <inheritdoc />
    public partial class ModificheController_e_NomenclaturePackages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HabitTrackers_HabitId_Date",
                table: "HabitTrackers");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "HabitTrackers");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "HabitTrackers");

            migrationBuilder.AddColumn<int>(
                name: "MonthlyDaysMask",
                table: "Schedules",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletedAtUtc",
                table: "HabitTrackers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HabitTrackers_HabitId_CompletedAtUtc",
                table: "HabitTrackers",
                columns: new[] { "HabitId", "CompletedAtUtc" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HabitTrackers_HabitId_CompletedAtUtc",
                table: "HabitTrackers");

            migrationBuilder.DropColumn(
                name: "MonthlyDaysMask",
                table: "Schedules");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletedAtUtc",
                table: "HabitTrackers",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "HabitTrackers",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "HabitTrackers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_HabitTrackers_HabitId_Date",
                table: "HabitTrackers",
                columns: new[] { "HabitId", "Date" },
                unique: true);
        }
    }
}
