using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.MySqlMigrations
{
    public partial class AlterTableStudentTranscriptMasterAndStudentTranscriptDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "credit_attempted",
                table: "student_transcript_detail");

            migrationBuilder.DropColumn(
                name: "gpa",
                table: "student_transcript_detail");

            migrationBuilder.DropColumn(
                name: "grade_title",
                table: "student_transcript_detail");

            migrationBuilder.DropColumn(
                name: "school_name",
                table: "student_transcript_detail");

            migrationBuilder.DropColumn(
                name: "school_year",
                table: "student_transcript_detail");

            migrationBuilder.DropColumn(
                name: "total_credit_earned",
                table: "student_transcript_detail");

            migrationBuilder.AddColumn<decimal>(
                name: "credit_attempted",
                table: "student_transcript_master",
                type: "decimal(5, 2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "gpa",
                table: "student_transcript_master",
                type: "decimal(5, 2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "grade_title",
                table: "student_transcript_master",
                unicode: false,
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "school_name",
                table: "student_transcript_master",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "school_year",
                table: "student_transcript_master",
                unicode: false,
                maxLength: 9,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "total_grade_credit_earned",
                table: "student_transcript_master",
                type: "decimal(5, 2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "credit_attempted",
                table: "student_transcript_master");

            migrationBuilder.DropColumn(
                name: "gpa",
                table: "student_transcript_master");

            migrationBuilder.DropColumn(
                name: "grade_title",
                table: "student_transcript_master");

            migrationBuilder.DropColumn(
                name: "school_name",
                table: "student_transcript_master");

            migrationBuilder.DropColumn(
                name: "school_year",
                table: "student_transcript_master");

            migrationBuilder.DropColumn(
                name: "total_grade_credit_earned",
                table: "student_transcript_master");

            migrationBuilder.AddColumn<decimal>(
                name: "credit_attempted",
                table: "student_transcript_detail",
                type: "decimal(5, 2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "gpa",
                table: "student_transcript_detail",
                type: "decimal(5, 2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "grade_title",
                table: "student_transcript_detail",
                type: "longtext CHARACTER SET utf8mb4",
                unicode: false,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "school_name",
                table: "student_transcript_detail",
                type: "varchar(100) CHARACTER SET utf8mb4",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "school_year",
                table: "student_transcript_detail",
                type: "varchar(9) CHARACTER SET utf8mb4",
                unicode: false,
                maxLength: 9,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "total_credit_earned",
                table: "student_transcript_detail",
                type: "decimal(5, 2)",
                nullable: true);
        }
    }
}
