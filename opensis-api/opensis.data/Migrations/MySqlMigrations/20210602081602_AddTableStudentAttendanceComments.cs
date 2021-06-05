using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.MySqlMigrations
{
    public partial class AddTableStudentAttendanceComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PRIMARY",
                table: "student_attendance");

            migrationBuilder.AddColumn<int>(
                name: "student_attendance_id",
                table: "student_attendance",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_student_attendance_tenant_id_school_id_student_id_student_at~",
                table: "student_attendance",
                columns: new[] { "tenant_id", "school_id", "student_id", "student_attendance_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_student_attendance",
                table: "student_attendance",
                columns: new[] { "tenant_id", "school_id", "student_id", "staff_id", "course_id", "course_section_id", "attendance_category_id", "attendance_code", "attendance_date", "block_id", "period_id" });

            migrationBuilder.CreateTable(
                name: "student_attendance_comments",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(nullable: false),
                    school_id = table.Column<int>(nullable: false),
                    student_id = table.Column<int>(nullable: false),
                    student_attendance_id = table.Column<int>(nullable: false),
                    comment_id = table.Column<long>(nullable: false),
                    comment = table.Column<string>(unicode: false, maxLength: 500, nullable: true),
                    comment_timestamp = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_attendance_comments", x => new { x.tenant_id, x.school_id, x.student_id, x.student_attendance_id, x.comment_id });
                    table.ForeignKey(
                        name: "FK_student_attendance_comments_student_attendance",
                        columns: x => new { x.tenant_id, x.school_id, x.student_id, x.student_attendance_id },
                        principalTable: "student_attendance",
                        principalColumns: new[] { "tenant_id", "school_id", "student_id", "student_attendance_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "student_attendance_id_idx",
                table: "student_attendance",
                columns: new[] { "tenant_id", "school_id", "student_id", "student_attendance_id" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "student_attendance_comments");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_student_attendance_tenant_id_school_id_student_id_student_at~",
                table: "student_attendance");

            migrationBuilder.DropPrimaryKey(
                name: "PK_student_attendance",
                table: "student_attendance");

            migrationBuilder.DropIndex(
                name: "student_attendance_id_idx",
                table: "student_attendance");

            migrationBuilder.DropColumn(
                name: "student_attendance_id",
                table: "student_attendance");

            migrationBuilder.AddPrimaryKey(
                name: "PK_student_attendance",
                table: "student_attendance",
                columns: new[] { "tenant_id", "school_id", "student_id", "staff_id", "course_id", "course_section_id", "attendance_category_id", "attendance_code", "attendance_date" });
        }
    }
}
