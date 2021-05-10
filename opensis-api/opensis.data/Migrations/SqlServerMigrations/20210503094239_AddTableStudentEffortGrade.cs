using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.SqlServerMigrations
{
    public partial class AddTableStudentEffortGrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "student_effort_grade_master",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(nullable: false),
                    school_id = table.Column<int>(nullable: false),
                    student_id = table.Column<int>(nullable: false),
                    student_effort_grade_srlno = table.Column<long>(nullable: false),
                    academic_year = table.Column<decimal>(type: "decimal(4, 0)", nullable: true),
                    calendar_id = table.Column<int>(nullable: true),
                    yr_marking_period_id = table.Column<int>(nullable: true),
                    smstr_marking_period_id = table.Column<int>(nullable: true),
                    qtr_marking_period_id = table.Column<int>(nullable: true),
                    teacher_comment = table.Column<string>(unicode: false, maxLength: 500, nullable: true),
                    created_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_effort_grade_master", x => new { x.tenant_id, x.school_id, x.student_id, x.student_effort_grade_srlno });
                    table.ForeignKey(
                        name: "FK_student_effort_grade_master_school_calendars",
                        columns: x => new { x.tenant_id, x.school_id, x.calendar_id },
                        principalTable: "school_calendars",
                        principalColumns: new[] { "tenant_id", "school_id", "calender_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_student_effort_grade_master_quarters",
                        columns: x => new { x.tenant_id, x.school_id, x.qtr_marking_period_id },
                        principalTable: "quarters",
                        principalColumns: new[] { "tenant_id", "school_id", "marking_period_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_student_effort_grade_master_semesters",
                        columns: x => new { x.tenant_id, x.school_id, x.smstr_marking_period_id },
                        principalTable: "semesters",
                        principalColumns: new[] { "tenant_id", "school_id", "marking_period_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_student_effort_grade_master_school_years",
                        columns: x => new { x.tenant_id, x.school_id, x.yr_marking_period_id },
                        principalTable: "school_years",
                        principalColumns: new[] { "tenant_id", "school_id", "marking_period_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "student_effort_grade_detail",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(nullable: false),
                    school_id = table.Column<int>(nullable: false),
                    student_id = table.Column<int>(nullable: false),
                    student_effort_grade_srlno = table.Column<long>(nullable: false),
                    id = table.Column<long>(nullable: false),
                    effort_category_id = table.Column<int>(nullable: true, comment: "To get category_name"),
                    effort_item_id = table.Column<int>(nullable: true, comment: "To get effort_item_title"),
                    effort_grade_scale_id = table.Column<int>(nullable: true, comment: "To get grade_scale_value"),
                    created_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_effort_grade_detail", x => new { x.tenant_id, x.school_id, x.student_id, x.student_effort_grade_srlno, x.id });
                    table.ForeignKey(
                        name: "FK_student_effort_grade_detail_student_effort_grade_master",
                        columns: x => new { x.tenant_id, x.school_id, x.student_id, x.student_effort_grade_srlno },
                        principalTable: "student_effort_grade_master",
                        principalColumns: new[] { "tenant_id", "school_id", "student_id", "student_effort_grade_srlno" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_student_effort_grade_master_tenant_id_school_id_calendar_id",
                table: "student_effort_grade_master",
                columns: new[] { "tenant_id", "school_id", "calendar_id" });

            migrationBuilder.CreateIndex(
                name: "IX_student_effort_grade_master_tenant_id_school_id_qtr_marking_period_id",
                table: "student_effort_grade_master",
                columns: new[] { "tenant_id", "school_id", "qtr_marking_period_id" });

            migrationBuilder.CreateIndex(
                name: "IX_student_effort_grade_master_tenant_id_school_id_smstr_marking_period_id",
                table: "student_effort_grade_master",
                columns: new[] { "tenant_id", "school_id", "smstr_marking_period_id" });

            migrationBuilder.CreateIndex(
                name: "IX_student_effort_grade_master_tenant_id_school_id_yr_marking_period_id",
                table: "student_effort_grade_master",
                columns: new[] { "tenant_id", "school_id", "yr_marking_period_id" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "student_effort_grade_detail");

            migrationBuilder.DropTable(
                name: "student_effort_grade_master");
        }
    }
}
