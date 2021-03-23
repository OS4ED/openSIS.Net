using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.MySqlMigrations
{
    public partial class AddTwoColumnInCourseSectionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "allow_student_conflict",
                table: "course_section",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "allow_teacher_conflict",
                table: "course_section",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "allow_student_conflict",
                table: "course_section");

            migrationBuilder.DropColumn(
                name: "allow_teacher_conflict",
                table: "course_section");
        }
    }
}
