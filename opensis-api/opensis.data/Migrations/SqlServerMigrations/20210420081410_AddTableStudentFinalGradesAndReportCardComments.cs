using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.SqlServerMigrations
{
    public partial class AddTableStudentFinalGradesAndReportCardComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "report_card_comments",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(nullable: false),
                    school_id = table.Column<int>(nullable: false),
                    course_comment_id = table.Column<string>(unicode: false, maxLength: 200, nullable: false, comment: "Separated by | character"),
                    applicable_all_courses = table.Column<bool>(nullable: true),
                    course_id = table.Column<int>(nullable: false),
                    course_section_id = table.Column<int>(nullable: false),
                    comments = table.Column<string>(unicode: false, maxLength: 500, nullable: true),
                    created_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_report_card_comments", x => new { x.tenant_id, x.school_id, x.course_comment_id });
                });

            migrationBuilder.CreateTable(
                name: "student_final_grade",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(nullable: false),
                    school_id = table.Column<int>(nullable: false),
                    student_id = table.Column<int>(nullable: false),
                    student_guid = table.Column<Guid>(nullable: false),
                    alternate_id = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    student_internal_id = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    course_id = table.Column<int>(nullable: false),
                    course_section_id = table.Column<int>(nullable: false),
                    grade_id = table.Column<int>(nullable: true),
                    grade_scale_id = table.Column<int>(nullable: true),
                    academic_year = table.Column<decimal>(type: "decimal(4, 0)", nullable: false),
                    calendar_id = table.Column<int>(nullable: true),
                    standard_grade_scale_id = table.Column<int>(nullable: true),
                    yr_marking_period_id = table.Column<int>(nullable: true),
                    smstr_marking_period_id = table.Column<int>(nullable: true),
                    qtr_marking_period_id = table.Column<int>(nullable: true),
                    is_percent = table.Column<bool>(nullable: true),
                    percent_marks = table.Column<int>(nullable: true),
                    grade_obtained = table.Column<string>(unicode: false, fixedLength: true, maxLength: 5, nullable: true, comment: "A,A++,A+++,NONAC"),
                    course_comment_id = table.Column<string>(unicode: false, maxLength: 200, nullable: true, comment: "Separated by | character"),
                    created_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_final_grade", x => new { x.tenant_id, x.school_id, x.student_id });
                    table.ForeignKey(
                        name: "FK_student_final_grade_report_card_comments",
                        columns: x => new { x.tenant_id, x.school_id, x.course_comment_id },
                        principalTable: "report_card_comments",
                        principalColumns: new[] { "tenant_id", "school_id", "course_comment_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_student_final_grade_quarters",
                        columns: x => new { x.tenant_id, x.school_id, x.qtr_marking_period_id },
                        principalTable: "quarters",
                        principalColumns: new[] { "tenant_id", "school_id", "marking_period_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_student_final_grade_semesters",
                        columns: x => new { x.tenant_id, x.school_id, x.smstr_marking_period_id },
                        principalTable: "semesters",
                        principalColumns: new[] { "tenant_id", "school_id", "marking_period_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_student_final_grade_student_final_grade",
                        columns: x => new { x.tenant_id, x.school_id, x.student_id },
                        principalTable: "student_master",
                        principalColumns: new[] { "tenant_id", "school_id", "student_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_student_final_grade_school_years",
                        columns: x => new { x.tenant_id, x.school_id, x.yr_marking_period_id },
                        principalTable: "school_years",
                        principalColumns: new[] { "tenant_id", "school_id", "marking_period_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_student_final_grade_course_section",
                        columns: x => new { x.tenant_id, x.school_id, x.course_id, x.course_section_id },
                        principalTable: "course_section",
                        principalColumns: new[] { "tenant_id", "school_id", "course_id", "course_section_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_student_final_grade_tenant_id_school_id_course_comment_id",
                table: "student_final_grade",
                columns: new[] { "tenant_id", "school_id", "course_comment_id" });

            migrationBuilder.CreateIndex(
                name: "IX_student_final_grade_tenant_id_school_id_qtr_marking_period_id",
                table: "student_final_grade",
                columns: new[] { "tenant_id", "school_id", "qtr_marking_period_id" });

            migrationBuilder.CreateIndex(
                name: "IX_student_final_grade_tenant_id_school_id_smstr_marking_period_id",
                table: "student_final_grade",
                columns: new[] { "tenant_id", "school_id", "smstr_marking_period_id" });

            migrationBuilder.CreateIndex(
                name: "IX_student_final_grade_tenant_id_school_id_yr_marking_period_id",
                table: "student_final_grade",
                columns: new[] { "tenant_id", "school_id", "yr_marking_period_id" });

            migrationBuilder.CreateIndex(
                name: "IX_student_final_grade_tenant_id_school_id_course_id_course_section_id",
                table: "student_final_grade",
                columns: new[] { "tenant_id", "school_id", "course_id", "course_section_id" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "student_final_grade");

            migrationBuilder.DropTable(
                name: "report_card_comments");
        }
    }
}
