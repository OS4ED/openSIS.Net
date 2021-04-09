using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.MySqlMigrations
{
    public partial class AddPeriodIdInStudentAttendanceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "block_id",
                table: "student_attendance",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "period_id",
                table: "student_attendance",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_student_attendance_tenant_id_school_id_block_id_period_id",
                table: "student_attendance",
                columns: new[] { "tenant_id", "school_id", "block_id", "period_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_student_attendance_block_period",
                table: "student_attendance",
                columns: new[] { "tenant_id", "school_id", "block_id", "period_id" },
                principalTable: "block_period",
                principalColumns: new[] { "tenant_id", "school_id", "block_id", "period_id" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_student_attendance_block_period",
                table: "student_attendance");

            migrationBuilder.DropIndex(
                name: "IX_student_attendance_tenant_id_school_id_block_id_period_id",
                table: "student_attendance");

            migrationBuilder.DropColumn(
                name: "block_id",
                table: "student_attendance");

            migrationBuilder.DropColumn(
                name: "period_id",
                table: "student_attendance");
        }
    }
}
