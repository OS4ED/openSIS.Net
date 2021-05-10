using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.SqlServerMigrations
{
    public partial class AlterTableStudentFinalGradeStandard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_student_final_grade_comments_student_final_grade_standard",
                table: "student_final_grade_comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_student_final_grade_standard",
                table: "student_final_grade_standard");

            migrationBuilder.AlterColumn<int>(
                name: "standard_grade_scale_id",
                table: "student_final_grade_standard",
                unicode: false,
                maxLength: 50,
                nullable: true,
                comment: "table course_standard->course_us_standard->standard_grade_scale_id",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "student_final_grade_standard",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_student_final_grade_standard_1",
                table: "student_final_grade_standard",
                columns: new[] { "tenant_id", "school_id", "student_id", "id" });

            migrationBuilder.CreateIndex(
                name: "IX_student_final_grade_standard_tenant_id_school_id_student_id_student_final_grade_srlno",
                table: "student_final_grade_standard",
                columns: new[] { "tenant_id", "school_id", "student_id", "student_final_grade_srlno" });

            migrationBuilder.AddForeignKey(
                name: "FK_student_final_grade_standard_student_final_grade",
                table: "student_final_grade_standard",
                columns: new[] { "tenant_id", "school_id", "student_id", "student_final_grade_srlno" },
                principalTable: "student_final_grade",
                principalColumns: new[] { "tenant_id", "school_id", "student_id", "student_final_grade_srlno" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_student_final_grade_standard_student_final_grade",
                table: "student_final_grade_standard");

            migrationBuilder.DropPrimaryKey(
                name: "PK_student_final_grade_standard_1",
                table: "student_final_grade_standard");

            migrationBuilder.DropIndex(
                name: "IX_student_final_grade_standard_tenant_id_school_id_student_id_student_final_grade_srlno",
                table: "student_final_grade_standard");

            migrationBuilder.DropColumn(
                name: "id",
                table: "student_final_grade_standard");

            migrationBuilder.AlterColumn<int>(
                name: "standard_grade_scale_id",
                table: "student_final_grade_standard",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "table course_standard->course_us_standard->standard_grade_scale_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_student_final_grade_standard",
                table: "student_final_grade_standard",
                columns: new[] { "tenant_id", "school_id", "student_id", "student_final_grade_srlno" });

            migrationBuilder.AddForeignKey(
                name: "FK_student_final_grade_comments_student_final_grade_standard",
                table: "student_final_grade_comments",
                columns: new[] { "tenant_id", "school_id", "student_id", "student_final_grade_srlno" },
                principalTable: "student_final_grade_standard",
                principalColumns: new[] { "tenant_id", "school_id", "student_id", "student_final_grade_srlno" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
