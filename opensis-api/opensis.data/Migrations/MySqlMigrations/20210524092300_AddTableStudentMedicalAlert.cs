using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.MySqlMigrations
{
    public partial class AddTableStudentMedicalAlert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_student_report_card_student_master",
                table: "student_report_card_master");

            migrationBuilder.DropForeignKey(
                name: "FK_student_report_card_detail_student_report_card_detail",
                table: "student_report_card_detail");

            migrationBuilder.DropIndex(
                name: "PRIMARY",
                table: "student_report_card_master");

            migrationBuilder.DropIndex(
                name: "PRIMARY",
                table: "student_report_card_detail");

            migrationBuilder.DropIndex(
                name: "IX_student_report_card_detail_tenant_id_school_id_student_id_sc~",
                table: "student_report_card_detail");

            migrationBuilder.DropColumn(
                name: "absences",
                table: "student_report_card_detail");

            migrationBuilder.DropColumn(
                name: "excused_absences",
                table: "student_report_card_detail");

            migrationBuilder.AddColumn<string>(
                name: "marking_period_title",
                table: "student_report_card_master",
                fixedLength: true,
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "absences",
                table: "student_report_card_master",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "excused_absences",
                table: "student_report_card_master",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "overall_teacher_comments",
                table: "student_report_card_detail",
                unicode: false,
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_student_report_card_2",
                table: "student_report_card_master",
                columns: new[] { "tenant_id", "school_id", "student_id", "school_year", "marking_period_title" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_student_report_card_detail",
                table: "student_report_card_detail",
                column: "id");

            migrationBuilder.CreateTable(
                name: "student_medical_alert",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(nullable: false),
                    school_id = table.Column<int>(nullable: false),
                    student_id = table.Column<int>(nullable: false),
                    id = table.Column<int>(nullable: false),
                    alert_type = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    alert_description = table.Column<string>(unicode: false, nullable: true),
                    created_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_medical_alert", x => new { x.tenant_id, x.school_id, x.student_id, x.id });
                    table.ForeignKey(
                        name: "FK_student_medical_alert_student_master",
                        columns: x => new { x.tenant_id, x.school_id, x.student_id },
                        principalTable: "student_master",
                        principalColumns: new[] { "tenant_id", "school_id", "student_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "student_medical_immunization",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(nullable: false),
                    school_id = table.Column<int>(nullable: false),
                    student_id = table.Column<int>(nullable: false),
                    id = table.Column<int>(nullable: false),
                    immunization_type = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    immunization_date = table.Column<DateTime>(type: "date", nullable: true),
                    comment = table.Column<string>(unicode: false, nullable: true),
                    created_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_medical_immunization", x => new { x.tenant_id, x.school_id, x.student_id, x.id });
                    table.ForeignKey(
                        name: "FK_student_medical_immunization_student_master",
                        columns: x => new { x.tenant_id, x.school_id, x.student_id },
                        principalTable: "student_master",
                        principalColumns: new[] { "tenant_id", "school_id", "student_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "student_medical_note",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(nullable: false),
                    school_id = table.Column<int>(nullable: false),
                    student_id = table.Column<int>(nullable: false),
                    id = table.Column<int>(nullable: false),
                    note_date = table.Column<DateTime>(type: "date", nullable: true),
                    medical_note = table.Column<string>(unicode: false, nullable: true),
                    created_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_medical_note", x => new { x.tenant_id, x.school_id, x.student_id, x.id });
                    table.ForeignKey(
                        name: "FK_student_medical_note_student_master",
                        columns: x => new { x.tenant_id, x.school_id, x.student_id },
                        principalTable: "student_master",
                        principalColumns: new[] { "tenant_id", "school_id", "student_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "student_medical_nurse_visit",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(nullable: false),
                    school_id = table.Column<int>(nullable: false),
                    student_id = table.Column<int>(nullable: false),
                    id = table.Column<int>(nullable: false),
                    nurse_visit_date = table.Column<DateTime>(type: "date", nullable: true),
                    time_in = table.Column<DateTime>(type: "datetime", nullable: true),
                    time_out = table.Column<DateTime>(type: "datetime", nullable: true),
                    reason = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    result = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    comment = table.Column<string>(unicode: false, nullable: true),
                    created_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_medical_nurse_visit", x => new { x.tenant_id, x.school_id, x.student_id, x.id });
                    table.ForeignKey(
                        name: "FK_student_medical_nurse_visit_student_master",
                        columns: x => new { x.tenant_id, x.school_id, x.student_id },
                        principalTable: "student_master",
                        principalColumns: new[] { "tenant_id", "school_id", "student_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "student_medical_provider",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(nullable: false),
                    school_id = table.Column<int>(nullable: false),
                    student_id = table.Column<int>(nullable: false),
                    id = table.Column<int>(nullable: false),
                    primary_care_physician = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    primary_care_physician_phone = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    preferred_medical_facility = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    preferred_medical_facility_phone = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    insurance_company = table.Column<string>(unicode: false, maxLength: 200, nullable: true),
                    insurance_company_phone = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    policy_number = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    policy_holder_name = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    dentist_name = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    dentist_phone = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    vision_name = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    vision_provider_phone = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    created_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_medical_provider", x => new { x.tenant_id, x.school_id, x.student_id, x.id });
                    table.ForeignKey(
                        name: "FK_student_medical_provider_student_master",
                        columns: x => new { x.tenant_id, x.school_id, x.student_id },
                        principalTable: "student_master",
                        principalColumns: new[] { "tenant_id", "school_id", "student_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_student_report_card_student_master",
                table: "student_report_card_master",
                columns: new[] { "tenant_id", "school_id", "student_id" },
                principalTable: "student_master",
                principalColumns: new[] { "tenant_id", "school_id", "student_id" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.CreateIndex(
                name: "IX_student_report_card_detail_tenant_id_school_id_student_id_sc~",
                table: "student_report_card_detail",
                columns: new[] { "tenant_id", "school_id", "student_id", "school_year", "marking_period_title" });

            migrationBuilder.AddForeignKey(
                name: "FK_student_report_card_detail_student_report_card_master",
                table: "student_report_card_detail",
                columns: new[] { "tenant_id", "school_id", "student_id", "school_year", "marking_period_title" },
                principalTable: "student_report_card_master",
                principalColumns: new[] { "tenant_id", "school_id", "student_id", "school_year", "marking_period_title" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_student_report_card_detail_student_report_card_master",
                table: "student_report_card_detail");

            migrationBuilder.DropTable(
                name: "student_medical_alert");

            migrationBuilder.DropTable(
                name: "student_medical_immunization");

            migrationBuilder.DropTable(
                name: "student_medical_note");

            migrationBuilder.DropTable(
                name: "student_medical_nurse_visit");

            migrationBuilder.DropTable(
                name: "student_medical_provider");

            migrationBuilder.DropPrimaryKey(
                name: "PK_student_report_card_2",
                table: "student_report_card_master");

            migrationBuilder.DropPrimaryKey(
                name: "PK_student_report_card_detail",
                table: "student_report_card_detail");

            migrationBuilder.DropIndex(
                name: "IX_student_report_card_detail_tenant_id_school_id_student_id_sc~",
                table: "student_report_card_detail");

            migrationBuilder.DropColumn(
                name: "marking_period_title",
                table: "student_report_card_master");

            migrationBuilder.DropColumn(
                name: "absences",
                table: "student_report_card_master");

            migrationBuilder.DropColumn(
                name: "excused_absences",
                table: "student_report_card_master");

            migrationBuilder.DropColumn(
                name: "overall_teacher_comments",
                table: "student_report_card_detail");

            migrationBuilder.AddColumn<int>(
                name: "absences",
                table: "student_report_card_detail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "excused_absences",
                table: "student_report_card_detail",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_student_report_card_2",
                table: "student_report_card_master",
                columns: new[] { "tenant_id", "school_id", "student_id", "school_year" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_student_report_card_detail",
                table: "student_report_card_detail",
                columns: new[] { "id", "tenant_id", "school_id", "student_id", "school_year" });

            migrationBuilder.CreateIndex(
                name: "IX_student_report_card_detail_tenant_id_school_id_student_id_sc~",
                table: "student_report_card_detail",
                columns: new[] { "tenant_id", "school_id", "student_id", "school_year" });

            migrationBuilder.AddForeignKey(
                name: "FK_student_report_card_detail_student_report_card_detail",
                table: "student_report_card_detail",
                columns: new[] { "tenant_id", "school_id", "student_id", "school_year" },
                principalTable: "student_report_card_master",
                principalColumns: new[] { "tenant_id", "school_id", "student_id", "school_year" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
