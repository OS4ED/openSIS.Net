using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.SqlServerMigrations
{
    public partial class AlterTableStudentFinalGrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comment_category_school_master",
                table: "course_comment_category");

            migrationBuilder.DropForeignKey(
                name: "FK_student_final_grade_course_section",
                table: "student_final_grade");

            migrationBuilder.DropForeignKey(
                name: "FK_student_final_grade_comments_report_card_comments",
                table: "student_final_grade_comments");

            migrationBuilder.DropForeignKey(
                name: "FK_student_final_grade_comments_student_final_grade",
                table: "student_final_grade_comments");

            migrationBuilder.DropTable(
                name: "report_card_comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_student_final_grade_comments_1",
                table: "student_final_grade_comments");

            migrationBuilder.DropIndex(
                name: "IX_student_final_grade_comments_tenant_id_school_id_id",
                table: "student_final_grade_comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_student_final_grade",
                table: "student_final_grade");

            migrationBuilder.DropIndex(
                name: "IX_student_final_grade_tenant_id_school_id_course_id_course_section_id",
                table: "student_final_grade");

            migrationBuilder.DropColumn(
                name: "calendar_id",
                table: "student_final_grade_comments");

            migrationBuilder.DropColumn(
                name: "yr_marking_period_id",
                table: "student_final_grade_comments");

            migrationBuilder.DropColumn(
                name: "smstr_marking_period_id",
                table: "student_final_grade_comments");

            migrationBuilder.DropColumn(
                name: "qtr_marking_period_id",
                table: "student_final_grade_comments");

            migrationBuilder.DropColumn(
                name: "id",
                table: "student_final_grade_comments");

            migrationBuilder.DropColumn(
                name: "alternate_id",
                table: "student_final_grade");

            migrationBuilder.DropColumn(
                name: "course_section_id",
                table: "student_final_grade");

            migrationBuilder.DropColumn(
                name: "standard_grade_scale_id",
                table: "student_final_grade");

            migrationBuilder.DropColumn(
                name: "student_guid",
                table: "student_final_grade");

            migrationBuilder.DropColumn(
                name: "student_internal_id",
                table: "student_final_grade");

            migrationBuilder.AddColumn<long>(
                name: "student_final_grade_srlno",
                table: "student_final_grade_comments",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "course_comment_id",
                table: "student_final_grade_comments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "academic_year",
                table: "student_final_grade",
                type: "decimal(4, 0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(4, 0)");

            migrationBuilder.AlterColumn<int>(
                name: "qtr_marking_period_id",
                table: "student_final_grade",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "smstr_marking_period_id",
                table: "student_final_grade",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "yr_marking_period_id",
                table: "student_final_grade",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "calendar_id",
                table: "student_final_grade",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<long>(
                name: "student_final_grade_srlno",
                table: "student_final_grade",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "based_on_standard_grade",
                table: "student_final_grade",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "teacher_comment",
                table: "student_final_grade",
                unicode: false,
                maxLength: 500,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "course_name",
                table: "course_comment_category",
                unicode: false,
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldUnicode: false,
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "applicable_all_courses",
                table: "course_comment_category",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "comments",
                table: "course_comment_category",
                unicode: false,
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "course_id",
                table: "course_comment_category",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "sort_order",
                table: "course_comment_category",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_student_final_grade_comments",
                table: "student_final_grade_comments",
                columns: new[] { "tenant_id", "school_id", "student_id", "student_final_grade_srlno", "course_comment_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_student_final_grade",
                table: "student_final_grade",
                columns: new[] { "tenant_id", "school_id", "student_id", "student_final_grade_srlno" });

            migrationBuilder.CreateTable(
                name: "student_final_grade_standard",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(nullable: false),
                    school_id = table.Column<int>(nullable: false),
                    student_id = table.Column<int>(nullable: false),
                    student_final_grade_srlno = table.Column<long>(nullable: false),
                    academic_year = table.Column<decimal>(type: "decimal(4, 0)", nullable: true),
                    calendar_id = table.Column<int>(nullable: true),
                    standard_grade_scale_id = table.Column<int>(nullable: true),
                    yr_marking_period_id = table.Column<int>(nullable: true),
                    smstr_marking_period_id = table.Column<int>(nullable: true),
                    qtr_marking_period_id = table.Column<int>(nullable: true),
                    grade_obtained = table.Column<int>(nullable: true),
                    teacher_comment = table.Column<string>(unicode: false, maxLength: 500, nullable: true),
                    created_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_final_grade_standard", x => new { x.tenant_id, x.school_id, x.student_id, x.student_final_grade_srlno });
                });

            migrationBuilder.CreateIndex(
                name: "IX_student_final_grade_comments_tenant_id_school_id_course_comment_id",
                table: "student_final_grade_comments",
                columns: new[] { "tenant_id", "school_id", "course_comment_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_student_final_grade_comments_course_comment_category",
                table: "student_final_grade_comments",
                columns: new[] { "tenant_id", "school_id", "course_comment_id" },
                principalTable: "course_comment_category",
                principalColumns: new[] { "tenant_id", "school_id", "course_comment_id" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_student_final_grade_comments_student_final_grade1",
                table: "student_final_grade_comments",
                columns: new[] { "tenant_id", "school_id", "student_id", "student_final_grade_srlno" },
                principalTable: "student_final_grade",
                principalColumns: new[] { "tenant_id", "school_id", "student_id", "student_final_grade_srlno" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_student_final_grade_comments_student_final_grade_standard",
                table: "student_final_grade_comments",
                columns: new[] { "tenant_id", "school_id", "student_id", "student_final_grade_srlno" },
                principalTable: "student_final_grade_standard",
                principalColumns: new[] { "tenant_id", "school_id", "student_id", "student_final_grade_srlno" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_student_final_grade_comments_course_comment_category",
                table: "student_final_grade_comments");

            migrationBuilder.DropForeignKey(
                name: "FK_student_final_grade_comments_student_final_grade1",
                table: "student_final_grade_comments");

            migrationBuilder.DropForeignKey(
                name: "FK_student_final_grade_comments_student_final_grade_standard",
                table: "student_final_grade_comments");

            migrationBuilder.DropTable(
                name: "student_final_grade_standard");

            migrationBuilder.DropPrimaryKey(
                name: "PK_student_final_grade_comments",
                table: "student_final_grade_comments");

            migrationBuilder.DropIndex(
                name: "IX_student_final_grade_comments_tenant_id_school_id_course_comment_id",
                table: "student_final_grade_comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_student_final_grade",
                table: "student_final_grade");

            migrationBuilder.DropColumn(
                name: "student_final_grade_srlno",
                table: "student_final_grade_comments");

            migrationBuilder.DropColumn(
                name: "course_comment_id",
                table: "student_final_grade_comments");

            migrationBuilder.DropColumn(
                name: "student_final_grade_srlno",
                table: "student_final_grade");

            migrationBuilder.DropColumn(
                name: "based_on_standard_grade",
                table: "student_final_grade");

            migrationBuilder.DropColumn(
                name: "teacher_comment",
                table: "student_final_grade");

            migrationBuilder.DropColumn(
                name: "applicable_all_courses",
                table: "course_comment_category");

            migrationBuilder.DropColumn(
                name: "comments",
                table: "course_comment_category");

            migrationBuilder.DropColumn(
                name: "course_id",
                table: "course_comment_category");

            migrationBuilder.DropColumn(
                name: "sort_order",
                table: "course_comment_category");

            migrationBuilder.AddColumn<int>(
                name: "calendar_id",
                table: "student_final_grade_comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "yr_marking_period_id",
                table: "student_final_grade_comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "smstr_marking_period_id",
                table: "student_final_grade_comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "qtr_marking_period_id",
                table: "student_final_grade_comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "student_final_grade_comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "yr_marking_period_id",
                table: "student_final_grade",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "smstr_marking_period_id",
                table: "student_final_grade",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "qtr_marking_period_id",
                table: "student_final_grade",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "calendar_id",
                table: "student_final_grade",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "academic_year",
                table: "student_final_grade",
                type: "decimal(4, 0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4, 0)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "alternate_id",
                table: "student_final_grade",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "course_section_id",
                table: "student_final_grade",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "standard_grade_scale_id",
                table: "student_final_grade",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "student_guid",
                table: "student_final_grade",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "student_internal_id",
                table: "student_final_grade",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "course_name",
                table: "course_comment_category",
                type: "varchar(250)",
                unicode: false,
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 250);

            migrationBuilder.AddPrimaryKey(
                name: "PK_student_final_grade_comments_1",
                table: "student_final_grade_comments",
                columns: new[] { "tenant_id", "school_id", "student_id", "calendar_id", "yr_marking_period_id", "smstr_marking_period_id", "qtr_marking_period_id", "id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_student_final_grade",
                table: "student_final_grade",
                columns: new[] { "tenant_id", "school_id", "student_id", "calendar_id", "yr_marking_period_id", "smstr_marking_period_id", "qtr_marking_period_id" });

            migrationBuilder.CreateTable(
                name: "report_card_comments",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    school_id = table.Column<int>(type: "int", nullable: false),
                    course_comment_id = table.Column<int>(type: "int", nullable: false, comment: ""),
                    applicable_all_courses = table.Column<bool>(type: "bit", nullable: true),
                    comments = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    course_id = table.Column<int>(type: "int", nullable: false),
                    course_section_id = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    id = table.Column<int>(type: "int", nullable: false),
                    sort_order = table.Column<int>(type: "int", nullable: true),
                    updated_by = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_report_card_comments", x => new { x.tenant_id, x.school_id, x.course_comment_id });
                    table.UniqueConstraint("AK_report_card_comments_tenant_id_school_id_id", x => new { x.tenant_id, x.school_id, x.id });
                    table.ForeignKey(
                        name: "FK_report_card_comments_course_comment_category",
                        columns: x => new { x.tenant_id, x.school_id, x.course_comment_id },
                        principalTable: "course_comment_category",
                        principalColumns: new[] { "tenant_id", "school_id", "course_comment_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_student_final_grade_comments_tenant_id_school_id_id",
                table: "student_final_grade_comments",
                columns: new[] { "tenant_id", "school_id", "id" });

            migrationBuilder.CreateIndex(
                name: "IX_student_final_grade_tenant_id_school_id_course_id_course_section_id",
                table: "student_final_grade",
                columns: new[] { "tenant_id", "school_id", "course_id", "course_section_id" });

            migrationBuilder.CreateIndex(
                name: "IX_report_card_comments",
                table: "report_card_comments",
                columns: new[] { "tenant_id", "school_id", "id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_comment_category_school_master",
                table: "course_comment_category",
                columns: new[] { "tenant_id", "school_id" },
                principalTable: "school_master",
                principalColumns: new[] { "tenant_id", "school_id" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_student_final_grade_course_section",
                table: "student_final_grade",
                columns: new[] { "tenant_id", "school_id", "course_id", "course_section_id" },
                principalTable: "course_section",
                principalColumns: new[] { "tenant_id", "school_id", "course_id", "course_section_id" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_student_final_grade_comments_report_card_comments",
                table: "student_final_grade_comments",
                columns: new[] { "tenant_id", "school_id", "id" },
                principalTable: "report_card_comments",
                principalColumns: new[] { "tenant_id", "school_id", "id" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_student_final_grade_comments_student_final_grade",
                table: "student_final_grade_comments",
                columns: new[] { "tenant_id", "school_id", "student_id", "calendar_id", "yr_marking_period_id", "smstr_marking_period_id", "qtr_marking_period_id" },
                principalTable: "student_final_grade",
                principalColumns: new[] { "tenant_id", "school_id", "student_id", "calendar_id", "yr_marking_period_id", "smstr_marking_period_id", "qtr_marking_period_id" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
