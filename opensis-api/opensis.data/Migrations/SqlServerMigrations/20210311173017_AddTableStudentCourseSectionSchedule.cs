using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.SqlServerMigrations
{
    public partial class AddTableStudentCourseSectionSchedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Grade_Level_Equivalency",
                table: "grade_equivalency",
                newName: "grade_level_equivalency");

            migrationBuilder.AddColumn<int>(
                name: "grade_id",
                table: "student_enrollment",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "student_coursesection_schedule",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(nullable: false),
                    school_id = table.Column<int>(nullable: false),
                    student_id = table.Column<int>(nullable: false),
                    course_id = table.Column<int>(nullable: false),
                    course_section_id = table.Column<int>(nullable: false),
                    student_guid = table.Column<Guid>(nullable: false),
                    alternate_id = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    student_internal_id = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    first_given_name = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    middle_name = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    last_family_name = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    first_language_id = table.Column<int>(nullable: false),
                    grade_id = table.Column<int>(nullable: false),
                    academic_year = table.Column<decimal>(type: "decimal(4, 0)", nullable: false),
                    grade_scale_id = table.Column<int>(nullable: false),
                    course_section_name = table.Column<string>(unicode: false, maxLength: 200, nullable: true),
                    calendar_id = table.Column<int>(nullable: true),
                    created_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_coursesection_schedule", x => new { x.tenant_id, x.school_id, x.student_id, x.course_id, x.course_section_id });
                    table.ForeignKey(
                        name: "FK_student_coursesection_schedule_school_master",
                        columns: x => new { x.tenant_id, x.school_id },
                        principalTable: "school_master",
                        principalColumns: new[] { "tenant_id", "school_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_student_coursesection_schedule_student_master",
                        columns: x => new { x.tenant_id, x.school_id, x.student_id },
                        principalTable: "student_master",
                        principalColumns: new[] { "tenant_id", "school_id", "student_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_student_coursesection_schedule_course_section",
                        columns: x => new { x.tenant_id, x.school_id, x.course_id, x.course_section_id },
                        principalTable: "course_section",
                        principalColumns: new[] { "tenant_id", "school_id", "course_id", "course_section_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_student_enrollment_tenant_id_school_id_grade_id",
                table: "student_enrollment",
                columns: new[] { "tenant_id", "school_id", "grade_id" });

            migrationBuilder.CreateIndex(
                name: "IX_student_coursesection_schedule",
                table: "student_coursesection_schedule",
                columns: new[] { "tenant_id", "school_id", "student_id" });

            migrationBuilder.CreateIndex(
                name: "IX_student_coursesection_schedule_1",
                table: "student_coursesection_schedule",
                columns: new[] { "tenant_id", "school_id", "course_id", "course_section_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_student_enrollment_gradelevels",
                table: "student_enrollment",
                columns: new[] { "tenant_id", "school_id", "grade_id" },
                principalTable: "gradelevels",
                principalColumns: new[] { "tenant_id", "school_id", "grade_id" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_student_enrollment_gradelevels",
                table: "student_enrollment");

            migrationBuilder.DropTable(
                name: "student_coursesection_schedule");

            migrationBuilder.DropIndex(
                name: "IX_student_enrollment_tenant_id_school_id_grade_id",
                table: "student_enrollment");

            migrationBuilder.DropColumn(
                name: "grade_id",
                table: "student_enrollment");

            migrationBuilder.RenameColumn(
                name: "grade_level_equivalency",
                table: "grade_equivalency",
                newName: "Grade_Level_Equivalency");
        }
    }
}
