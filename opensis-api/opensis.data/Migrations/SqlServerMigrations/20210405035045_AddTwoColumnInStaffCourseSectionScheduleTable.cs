using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.SqlServerMigrations
{
    public partial class AddTwoColumnInStaffCourseSectionScheduleTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "effective_drop_date",
                table: "staff_coursesection_schedule",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_dropped",
                table: "staff_coursesection_schedule",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "effective_drop_date",
                table: "staff_coursesection_schedule");

            migrationBuilder.DropColumn(
                name: "is_dropped",
                table: "staff_coursesection_schedule");
        }
    }
}
