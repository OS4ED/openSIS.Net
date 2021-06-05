using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.MySqlMigrations
{
    public partial class AlterPKFKInStudentTranscriptMasterTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_student_transcript_detail_student_transcript_master",
                table: "student_transcript_detail");

            migrationBuilder.DropPrimaryKey(
                name: "PRIMARY",
                table: "student_transcript_master");

            migrationBuilder.DropIndex(
                name: "IX_student_transcript_detail_tenant_id_school_id_student_id",
                table: "student_transcript_detail");

            migrationBuilder.AlterColumn<string>(
                name: "grade_title",
                table: "student_transcript_master",
                unicode: false,
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldUnicode: false);

            migrationBuilder.AddColumn<string>(
                name: "grade_title",
                table: "student_transcript_detail",
                unicode: false,
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_student_transcript",
                table: "student_transcript_master",
                columns: new[] { "tenant_id", "school_id", "student_id", "grade_title" });

            migrationBuilder.CreateIndex(
                name: "IX_student_transcript_detail_tenant_id_school_id_student_id_gra~",
                table: "student_transcript_detail",
                columns: new[] { "tenant_id", "school_id", "student_id", "grade_title" });

            migrationBuilder.AddForeignKey(
                name: "FK_student_transcript_detail_student_transcript_master1",
                table: "student_transcript_detail",
                columns: new[] { "tenant_id", "school_id", "student_id", "grade_title" },
                principalTable: "student_transcript_master",
                principalColumns: new[] { "tenant_id", "school_id", "student_id", "grade_title" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_student_transcript_detail_student_transcript_master1",
                table: "student_transcript_detail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_student_transcript",
                table: "student_transcript_master");

            migrationBuilder.DropIndex(
                name: "IX_student_transcript_detail_tenant_id_school_id_student_id_gra~",
                table: "student_transcript_detail");

            migrationBuilder.DropColumn(
                name: "grade_title",
                table: "student_transcript_detail");

            migrationBuilder.AlterColumn<string>(
                name: "grade_title",
                table: "student_transcript_master",
                type: "longtext CHARACTER SET utf8mb4",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 200);

            migrationBuilder.AddPrimaryKey(
                name: "PK_student_transcript",
                table: "student_transcript_master",
                columns: new[] { "tenant_id", "school_id", "student_id" });

            migrationBuilder.CreateIndex(
                name: "IX_student_transcript_detail_tenant_id_school_id_student_id",
                table: "student_transcript_detail",
                columns: new[] { "tenant_id", "school_id", "student_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_student_transcript_detail_student_transcript_master",
                table: "student_transcript_detail",
                columns: new[] { "tenant_id", "school_id", "student_id" },
                principalTable: "student_transcript_master",
                principalColumns: new[] { "tenant_id", "school_id", "student_id" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
