using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.SqlServerMigrations
{
    public partial class AddCalculateAttendanceInBlockPeriod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "academic_year",
                table: "course_section",
                type: "decimal(4, 0)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "calculate_attendance",
                table: "block_period",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "academic_year",
                table: "course_section");

            migrationBuilder.DropColumn(
                name: "calculate_attendance",
                table: "block_period");
        }
    }
}
