using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.MySqlMigrations
{
    public partial class AddColumnIsAssignedInStaffCoursectionSchedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "meeting_days",
                table: "staff_coursesection_schedule",
                unicode: false,
                maxLength: 100,
                nullable: true,
                comment: "Starting Sunday as 0, 0|1|2|3|4|5|6",
                oldClrType: typeof(string),
                oldType: "varchar(13) CHARACTER SET utf8mb4",
                oldUnicode: false,
                oldMaxLength: 13,
                oldNullable: true,
                oldComment: "Starting Sunday as 0, 0|1|2|3|4|5|6");

            migrationBuilder.AddColumn<bool>(
                name: "is_assigned",
                table: "staff_coursesection_schedule",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "associations",
                table: "school_detail",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(255) CHARACTER SET utf8mb4",
                oldFixedLength: true,
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "affiliation",
                table: "school_detail",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(255) CHARACTER SET utf8mb4",
                oldFixedLength: true,
                oldMaxLength: 255,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_assigned",
                table: "staff_coursesection_schedule");

            migrationBuilder.AlterColumn<string>(
                name: "meeting_days",
                table: "staff_coursesection_schedule",
                type: "varchar(13) CHARACTER SET utf8mb4",
                unicode: false,
                maxLength: 13,
                nullable: true,
                comment: "Starting Sunday as 0, 0|1|2|3|4|5|6",
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "Starting Sunday as 0, 0|1|2|3|4|5|6");

            migrationBuilder.AlterColumn<string>(
                name: "associations",
                table: "school_detail",
                type: "char(255) CHARACTER SET utf8mb4",
                fixedLength: true,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "affiliation",
                table: "school_detail",
                type: "char(255) CHARACTER SET utf8mb4",
                fixedLength: true,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldNullable: true);
        }
    }
}
