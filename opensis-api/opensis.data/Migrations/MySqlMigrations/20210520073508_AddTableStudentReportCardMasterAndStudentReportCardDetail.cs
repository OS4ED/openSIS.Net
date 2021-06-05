using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.MySqlMigrations
{
    public partial class AddTableStudentReportCardMasterAndStudentReportCardDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "school_periods_obsolete");

            migrationBuilder.AddColumn<bool>(
                name: "visible_to_all_school",
                table: "notice",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "student_report_card_master",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(nullable: false),
                    school_id = table.Column<int>(nullable: false),
                    student_id = table.Column<int>(nullable: false),
                    school_year = table.Column<string>(unicode: false, maxLength: 9, nullable: false, comment: "year_marking_period_id"),
                    grade_title = table.Column<string>(unicode: false, nullable: false),
                    student_internal_id = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    yod_attendance = table.Column<string>(unicode: false, maxLength: 4, nullable: true, comment: "example:99%,100%"),
                    yod_absence = table.Column<int>(nullable: true),
                    report_generation_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_report_card_2", x => new { x.tenant_id, x.school_id, x.student_id, x.school_year });
                    table.ForeignKey(
                        name: "FK_student_report_card_student_master",
                        columns: x => new { x.tenant_id, x.school_id, x.student_id },
                        principalTable: "student_master",
                        principalColumns: new[] { "tenant_id", "school_id", "student_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "student_transcript_master",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(nullable: false),
                    school_id = table.Column<int>(nullable: false),
                    student_id = table.Column<int>(nullable: false),
                    student_internal_id = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    cumulative_gpa = table.Column<decimal>(type: "decimal(5, 2)", nullable: true),
                    total_credit_attempted = table.Column<decimal>(type: "decimal(5, 2)", nullable: true),
                    total_credit_earned = table.Column<decimal>(type: "decimal(5, 2)", nullable: true),
                    generated_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_transcript", x => new { x.tenant_id, x.school_id, x.student_id });
                    table.ForeignKey(
                        name: "FK_student_transcript_master_student_master",
                        columns: x => new { x.tenant_id, x.school_id, x.student_id },
                        principalTable: "student_master",
                        principalColumns: new[] { "tenant_id", "school_id", "student_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "student_report_card_detail",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false),
                    tenant_id = table.Column<Guid>(nullable: false),
                    school_id = table.Column<int>(nullable: false),
                    student_id = table.Column<int>(nullable: false),
                    school_year = table.Column<string>(unicode: false, maxLength: 9, nullable: false, comment: "year_marking_period_id"),
                    grade_title = table.Column<string>(unicode: false, nullable: false),
                    marking_period_title = table.Column<string>(fixedLength: true, maxLength: 50, nullable: true),
                    course_name = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    teacher = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    grade = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    gpa = table.Column<decimal>(type: "decimal(5, 2)", nullable: true),
                    comments = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    teacher_comments = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    absences = table.Column<int>(nullable: true),
                    excused_absences = table.Column<int>(nullable: true),
                    created_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_report_card_detail", x => new { x.id, x.tenant_id, x.school_id, x.student_id, x.school_year });
                    table.ForeignKey(
                        name: "FK_student_report_card_detail_student_report_card_detail",
                        columns: x => new { x.tenant_id, x.school_id, x.student_id, x.school_year },
                        principalTable: "student_report_card_master",
                        principalColumns: new[] { "tenant_id", "school_id", "student_id", "school_year" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "student_transcript_detail",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false),
                    tenant_id = table.Column<Guid>(nullable: false),
                    school_id = table.Column<int>(nullable: false),
                    student_id = table.Column<int>(nullable: false),
                    school_name = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    school_year = table.Column<string>(unicode: false, maxLength: 9, nullable: true),
                    grade_title = table.Column<string>(unicode: false, nullable: false),
                    course_code = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    course_name = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    credit_hours = table.Column<decimal>(type: "decimal(5, 2)", nullable: true),
                    credit_attempted = table.Column<decimal>(type: "decimal(5, 2)", nullable: true),
                    credit_earned = table.Column<decimal>(type: "decimal(5, 2)", nullable: true),
                    total_credit_earned = table.Column<decimal>(type: "decimal(5, 2)", nullable: true),
                    gp_value = table.Column<decimal>(type: "decimal(5, 2)", nullable: true),
                    gpa = table.Column<decimal>(type: "decimal(5, 2)", nullable: true),
                    grade = table.Column<string>(unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    created_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_transcript_detail", x => new { x.id, x.tenant_id, x.school_id, x.student_id });
                    table.ForeignKey(
                        name: "FK_student_transcript_detail_student_transcript_master",
                        columns: x => new { x.tenant_id, x.school_id, x.student_id },
                        principalTable: "student_transcript_master",
                        principalColumns: new[] { "tenant_id", "school_id", "student_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_student_report_card_detail_tenant_id_school_id_student_id_sc~",
                table: "student_report_card_detail",
                columns: new[] { "tenant_id", "school_id", "student_id", "school_year" });

            migrationBuilder.CreateIndex(
                name: "IX_student_transcript_detail_tenant_id_school_id_student_id",
                table: "student_transcript_detail",
                columns: new[] { "tenant_id", "school_id", "student_id" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "student_report_card_detail");

            migrationBuilder.DropTable(
                name: "student_transcript_detail");

            migrationBuilder.DropTable(
                name: "student_report_card_master");

            migrationBuilder.DropTable(
                name: "student_transcript_master");

            migrationBuilder.DropColumn(
                name: "visible_to_all_school",
                table: "notice");

            migrationBuilder.CreateTable(
                name: "school_periods_obsolete",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    school_id = table.Column<int>(type: "int", nullable: false),
                    period_id = table.Column<int>(type: "int", nullable: false),
                    academic_year = table.Column<decimal>(type: "decimal(4, 0)", nullable: true),
                    attendance = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    block = table.Column<string>(type: "varchar(10) CHARACTER SET utf8mb4", unicode: false, maxLength: 10, nullable: true),
                    end_time = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    ignore_scheduling = table.Column<string>(type: "varchar(10) CHARACTER SET utf8mb4", unicode: false, maxLength: 10, nullable: true),
                    last_updated = table.Column<DateTime>(type: "datetime", nullable: true),
                    length = table.Column<decimal>(type: "decimal(10, 0)", nullable: true),
                    rollover_id = table.Column<int>(type: "int", nullable: true),
                    short_name = table.Column<string>(type: "varchar(10) CHARACTER SET utf8mb4", unicode: false, maxLength: 10, nullable: true),
                    sort_order = table.Column<int>(type: "int", nullable: true),
                    start_time = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    title = table.Column<string>(type: "varchar(100) CHARACTER SET utf8mb4", unicode: false, maxLength: 100, nullable: true),
                    updated_by = table.Column<string>(type: "varchar(100) CHARACTER SET utf8mb4", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_table_school_periods", x => new { x.tenant_id, x.school_id, x.period_id });
                });
        }
    }
}
