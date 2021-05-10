using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.MySqlMigrations
{
    public partial class AddCourseSectionIdInStudentEffortGradeMasterTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "course_id",
                table: "student_effort_grade_master",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "course_section_id",
                table: "student_effort_grade_master",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "course_id",
                table: "student_effort_grade_master");

            migrationBuilder.DropColumn(
                name: "course_section_id",
                table: "student_effort_grade_master");
        }
    }
}
