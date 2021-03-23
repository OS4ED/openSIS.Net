using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.MySqlMigrations
{
    public partial class AddTableStudentScheduleView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "grade_scale_id",
                table: "student_coursesection_schedule",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "grade_id",
                table: "student_coursesection_schedule",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "effective_drop_date",
                table: "student_coursesection_schedule",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_dropped",
                table: "student_coursesection_schedule",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "student_schedule_view",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(nullable: false),
                    school_id = table.Column<int>(nullable: false),
                    student_id = table.Column<int>(nullable: false),
                    course_id = table.Column<int>(nullable: false),
                    course_section_id = table.Column<int>(nullable: false),
                    course_section_name = table.Column<string>(unicode: false, maxLength: 200, nullable: true),
                    student_internal_id = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    student_name = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    scheduled = table.Column<bool>(nullable: false),
                    conflict_comment = table.Column<string>(unicode: false, maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stutent_schedule_view", x => new { x.tenant_id, x.school_id, x.student_id, x.course_id, x.course_section_id });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "student_schedule_view");

            migrationBuilder.DropColumn(
                name: "effective_drop_date",
                table: "student_coursesection_schedule");

            migrationBuilder.DropColumn(
                name: "is_dropped",
                table: "student_coursesection_schedule");

            migrationBuilder.AlterColumn<int>(
                name: "grade_scale_id",
                table: "student_coursesection_schedule",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "grade_id",
                table: "student_coursesection_schedule",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
