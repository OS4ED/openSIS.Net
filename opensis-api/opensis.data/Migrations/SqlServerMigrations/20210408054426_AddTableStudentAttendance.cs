using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.SqlServerMigrations
{
    public partial class AddTableStudentAttendance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "student_attendance",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(nullable: false),
                    school_id = table.Column<int>(nullable: false),
                    student_id = table.Column<int>(nullable: false),
                    staff_id = table.Column<int>(nullable: false),
                    course_id = table.Column<int>(nullable: false),
                    course_section_id = table.Column<int>(nullable: false),
                    attendance_category_id = table.Column<int>(nullable: false),
                    attendance_code = table.Column<int>(nullable: false),
                    attendance_date = table.Column<DateTime>(type: "date", nullable: false),
                    comments = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    created_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_attendance", x => new { x.tenant_id, x.school_id, x.student_id, x.staff_id, x.course_id, x.course_section_id, x.attendance_category_id, x.attendance_code, x.attendance_date });
                    table.ForeignKey(
                        name: "FK_student_attendance_attendance_code",
                        columns: x => new { x.tenant_id, x.school_id, x.attendance_category_id, x.attendance_code },
                        principalTable: "attendance_code",
                        principalColumns: new[] { "tenant_id", "school_id", "attendance_category_id", "attendance_code" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_student_attendance_staff_coursesection_schedule",
                        columns: x => new { x.tenant_id, x.school_id, x.staff_id, x.course_id, x.course_section_id },
                        principalTable: "staff_coursesection_schedule",
                        principalColumns: new[] { "tenant_id", "school_id", "staff_id", "course_id", "course_section_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_student_attendance_student_coursesection_schedule",
                        columns: x => new { x.tenant_id, x.school_id, x.student_id, x.course_id, x.course_section_id },
                        principalTable: "student_coursesection_schedule",
                        principalColumns: new[] { "tenant_id", "school_id", "student_id", "course_id", "course_section_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_student_attendance_1",
                table: "student_attendance",
                columns: new[] { "tenant_id", "school_id", "course_section_id" });

            migrationBuilder.CreateIndex(
                name: "IX_student_attendance_2",
                table: "student_attendance",
                columns: new[] { "tenant_id", "school_id", "staff_id" });

            migrationBuilder.CreateIndex(
                name: "IX_student_attendance",
                table: "student_attendance",
                columns: new[] { "tenant_id", "school_id", "student_id" });

            migrationBuilder.CreateIndex(
                name: "IX_student_attendance_tenant_id_school_id_attendance_category_id_attendance_code",
                table: "student_attendance",
                columns: new[] { "tenant_id", "school_id", "attendance_category_id", "attendance_code" });

            migrationBuilder.CreateIndex(
                name: "IX_student_attendance_3",
                table: "student_attendance",
                columns: new[] { "tenant_id", "school_id", "student_id", "attendance_date" });

            migrationBuilder.CreateIndex(
                name: "IX_student_attendance_tenant_id_school_id_staff_id_course_id_course_section_id",
                table: "student_attendance",
                columns: new[] { "tenant_id", "school_id", "staff_id", "course_id", "course_section_id" });

            migrationBuilder.CreateIndex(
                name: "IX_student_attendance_tenant_id_school_id_student_id_course_id_course_section_id",
                table: "student_attendance",
                columns: new[] { "tenant_id", "school_id", "student_id", "course_id", "course_section_id" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "student_attendance");
        }
    }
}
