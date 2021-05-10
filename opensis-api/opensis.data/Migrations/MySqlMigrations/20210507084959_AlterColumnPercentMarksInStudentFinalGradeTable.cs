using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.MySqlMigrations
{
    public partial class AlterColumnPercentMarksInStudentFinalGradeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "percent_marks",
                table: "student_final_grade",
                type: "decimal(5, 2)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "percent_marks",
                table: "student_final_grade",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 2)",
                oldNullable: true);
        }
    }
}
