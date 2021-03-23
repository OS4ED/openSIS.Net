using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.MySqlMigrations
{
    public partial class AddTableStaffCourseSectionScheduleAndStaffScheduleView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.CreateTable(
                name: "staff_coursesection_schedule",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(nullable: false),
                    school_id = table.Column<int>(nullable: false),
                    staff_id = table.Column<int>(nullable: false),
                    course_id = table.Column<int>(nullable: false),
                    course_section_id = table.Column<int>(nullable: false),
                    staff_guid = table.Column<Guid>(nullable: false),
                    course_section_name = table.Column<string>(unicode: false, maxLength: 200, nullable: true),
                    yr_marking_period_id = table.Column<int>(nullable: true),
                    smstr_marking_period_id = table.Column<int>(nullable: true),
                    qtr_marking_period_id = table.Column<int>(nullable: true),
                    duration_start_date = table.Column<DateTime>(type: "date", nullable: true),
                    duration_end_date = table.Column<DateTime>(type: "date", nullable: true),
                    meeting_days = table.Column<string>(unicode: false, maxLength: 13, nullable: true, comment: "Starting Sunday as 0, 0|1|2|3|4|5|6"),
                    created_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: false),
                    updated_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_staff_coursesection_schedule", x => new { x.tenant_id, x.school_id, x.staff_id, x.course_id, x.course_section_id });
                    table.ForeignKey(
                        name: "FK_staff_coursesection_schedule_staff_master",
                        columns: x => new { x.tenant_id, x.staff_id },
                        principalTable: "staff_master",
                        principalColumns: new[] { "tenant_id", "staff_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_staff_coursesection_schedule_quarters",
                        columns: x => new { x.tenant_id, x.school_id, x.qtr_marking_period_id },
                        principalTable: "quarters",
                        principalColumns: new[] { "tenant_id", "school_id", "marking_period_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_staff_coursesection_schedule_semesters",
                        columns: x => new { x.tenant_id, x.school_id, x.smstr_marking_period_id },
                        principalTable: "semesters",
                        principalColumns: new[] { "tenant_id", "school_id", "marking_period_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_staff_coursesection_schedule_school_years",
                        columns: x => new { x.tenant_id, x.school_id, x.yr_marking_period_id },
                        principalTable: "school_years",
                        principalColumns: new[] { "tenant_id", "school_id", "marking_period_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_staff_coursesection_schedule_course_section",
                        columns: x => new { x.tenant_id, x.school_id, x.course_id, x.course_section_id },
                        principalTable: "course_section",
                        principalColumns: new[] { "tenant_id", "school_id", "course_id", "course_section_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "staff_schedule_view",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(nullable: false),
                    school_id = table.Column<int>(nullable: false),
                    staff_id = table.Column<int>(nullable: false),
                    course_id = table.Column<int>(nullable: false),
                    course_section_id = table.Column<int>(nullable: false),
                    course_short_name = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    course_section_name = table.Column<string>(unicode: false, maxLength: 200, nullable: true),
                    staff_internal_id = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    staff_name = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    scheduled = table.Column<bool>(nullable: false),
                    conflict_comment = table.Column<string>(unicode: false, maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_staff_schedule_view", x => new { x.tenant_id, x.school_id, x.staff_id, x.course_id, x.course_section_id });
                });

            migrationBuilder.CreateIndex(
                name: "IX_staff_coursesection_schedule_tenant_id_staff_id",
                table: "staff_coursesection_schedule",
                columns: new[] { "tenant_id", "staff_id" });

            migrationBuilder.CreateIndex(
                name: "IX_staff_coursesection_schedule_tenant_id_school_id_qtr_marking~",
                table: "staff_coursesection_schedule",
                columns: new[] { "tenant_id", "school_id", "qtr_marking_period_id" });

            migrationBuilder.CreateIndex(
                name: "IX_staff_coursesection_schedule_tenant_id_school_id_smstr_marki~",
                table: "staff_coursesection_schedule",
                columns: new[] { "tenant_id", "school_id", "smstr_marking_period_id" });

            migrationBuilder.CreateIndex(
                name: "IX_staff_coursesection_schedule_tenant_id_school_id_yr_marking_~",
                table: "staff_coursesection_schedule",
                columns: new[] { "tenant_id", "school_id", "yr_marking_period_id" });

            migrationBuilder.CreateIndex(
                name: "IX_staff_coursesection_schedule_tenant_id_school_id_course_id_c~",
                table: "staff_coursesection_schedule",
                columns: new[] { "tenant_id", "school_id", "course_id", "course_section_id" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "staff_coursesection_schedule");

            migrationBuilder.DropTable(
                name: "staff_schedule_view");

           
        }
    }
}
