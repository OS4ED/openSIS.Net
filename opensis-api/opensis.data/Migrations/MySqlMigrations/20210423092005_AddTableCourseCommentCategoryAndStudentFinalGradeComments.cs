using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.MySqlMigrations
{
    public partial class AddTableCourseCommentCategoryAndStudentFinalGradeComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_student_final_grade_report_card_comments",
                table: "student_final_grade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_student_final_grade",
                table: "student_final_grade");

            migrationBuilder.DropIndex(
                name: "IX_student_final_grade_tenant_id_school_id_course_comment_id",
                table: "student_final_grade");

            migrationBuilder.DropColumn(
                name: "course_comment_id",
                table: "student_final_grade");

            migrationBuilder.AlterColumn<int>(
                name: "yr_marking_period_id",
                table: "student_final_grade",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "smstr_marking_period_id",
                table: "student_final_grade",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "qtr_marking_period_id",
                table: "student_final_grade",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "calendar_id",
                table: "student_final_grade",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "course_comment_id",
                table: "report_card_comments",
                nullable: false,
                comment: "",
                oldClrType: typeof(string),
                oldType: "varchar(200) CHARACTER SET utf8mb4",
                oldUnicode: false,
                oldMaxLength: 200,
                oldComment: "Separated by | character");

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "report_card_comments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "sort_order",
                table: "report_card_comments",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_student_final_grade",
                table: "student_final_grade",
                columns: new[] { "tenant_id", "school_id", "student_id", "calendar_id", "yr_marking_period_id", "smstr_marking_period_id", "qtr_marking_period_id" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_report_card_comments_tenant_id_school_id_id",
                table: "report_card_comments",
                columns: new[] { "tenant_id", "school_id", "id" });

            migrationBuilder.CreateTable(
                name: "course_comment_category",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(nullable: false),
                    school_id = table.Column<int>(nullable: false),
                    course_comment_id = table.Column<int>(nullable: false),
                    course_name = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    created_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comment_category", x => new { x.tenant_id, x.school_id, x.course_comment_id });
                    table.ForeignKey(
                        name: "FK_comment_category_school_master",
                        columns: x => new { x.tenant_id, x.school_id },
                        principalTable: "school_master",
                        principalColumns: new[] { "tenant_id", "school_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "student_final_grade_comments",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(nullable: false),
                    school_id = table.Column<int>(nullable: false),
                    student_id = table.Column<int>(nullable: false),
                    calendar_id = table.Column<int>(nullable: false),
                    yr_marking_period_id = table.Column<int>(nullable: false),
                    smstr_marking_period_id = table.Column<int>(nullable: false),
                    qtr_marking_period_id = table.Column<int>(nullable: false),
                    id = table.Column<int>(nullable: false),
                    created_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_final_grade_comments_1", x => new { x.tenant_id, x.school_id, x.student_id, x.calendar_id, x.yr_marking_period_id, x.smstr_marking_period_id, x.qtr_marking_period_id, x.id });
                    table.ForeignKey(
                        name: "FK_student_final_grade_comments_report_card_comments",
                        columns: x => new { x.tenant_id, x.school_id, x.id },
                        principalTable: "report_card_comments",
                        principalColumns: new[] { "tenant_id", "school_id", "id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_student_final_grade_comments_student_final_grade",
                        columns: x => new { x.tenant_id, x.school_id, x.student_id, x.calendar_id, x.yr_marking_period_id, x.smstr_marking_period_id, x.qtr_marking_period_id },
                        principalTable: "student_final_grade",
                        principalColumns: new[] { "tenant_id", "school_id", "student_id", "calendar_id", "yr_marking_period_id", "smstr_marking_period_id", "qtr_marking_period_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_report_card_comments",
                table: "report_card_comments",
                columns: new[] { "tenant_id", "school_id", "id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_student_final_grade_comments_tenant_id_school_id_id",
                table: "student_final_grade_comments",
                columns: new[] { "tenant_id", "school_id", "id" });

            migrationBuilder.AddForeignKey(
                name: "FK_report_card_comments_course_comment_category",
                table: "report_card_comments",
                columns: new[] { "tenant_id", "school_id", "course_comment_id" },
                principalTable: "course_comment_category",
                principalColumns: new[] { "tenant_id", "school_id", "course_comment_id" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_report_card_comments_course_comment_category",
                table: "report_card_comments");

            migrationBuilder.DropTable(
                name: "course_comment_category");

            migrationBuilder.DropTable(
                name: "student_final_grade_comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_student_final_grade",
                table: "student_final_grade");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_report_card_comments_tenant_id_school_id_id",
                table: "report_card_comments");

            migrationBuilder.DropIndex(
                name: "IX_report_card_comments",
                table: "report_card_comments");

            migrationBuilder.DropColumn(
                name: "id",
                table: "report_card_comments");

            migrationBuilder.DropColumn(
                name: "sort_order",
                table: "report_card_comments");

            migrationBuilder.AlterColumn<int>(
                name: "qtr_marking_period_id",
                table: "student_final_grade",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "smstr_marking_period_id",
                table: "student_final_grade",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "yr_marking_period_id",
                table: "student_final_grade",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "calendar_id",
                table: "student_final_grade",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "course_comment_id",
                table: "student_final_grade",
                type: "varchar(200) CHARACTER SET utf8mb4",
                unicode: false,
                maxLength: 200,
                nullable: true,
                comment: "Separated by | character");

            migrationBuilder.AlterColumn<string>(
                name: "course_comment_id",
                table: "report_card_comments",
                type: "varchar(200) CHARACTER SET utf8mb4",
                unicode: false,
                maxLength: 200,
                nullable: false,
                comment: "Separated by | character",
                oldClrType: typeof(int),
                oldComment: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_student_final_grade",
                table: "student_final_grade",
                columns: new[] { "tenant_id", "school_id", "student_id" });

            migrationBuilder.CreateIndex(
                name: "IX_student_final_grade_tenant_id_school_id_course_comment_id",
                table: "student_final_grade",
                columns: new[] { "tenant_id", "school_id", "course_comment_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_student_final_grade_report_card_comments",
                table: "student_final_grade",
                columns: new[] { "tenant_id", "school_id", "course_comment_id" },
                principalTable: "report_card_comments",
                principalColumns: new[] { "tenant_id", "school_id", "course_comment_id" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
