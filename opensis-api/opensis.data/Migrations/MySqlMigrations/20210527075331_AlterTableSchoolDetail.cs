using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.MySqlMigrations
{
    public partial class AlterTableSchoolDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "male_toilet_type",
                table: "school_detail",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(50) CHARACTER SET utf8mb4",
                oldFixedLength: true,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "male_toilet_accessibility",
                table: "school_detail",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(50) CHARACTER SET utf8mb4",
                oldFixedLength: true,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "female_toilet_type",
                table: "school_detail",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(50) CHARACTER SET utf8mb4",
                oldFixedLength: true,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "female_toilet_accessibility",
                table: "school_detail",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(50) CHARACTER SET utf8mb4",
                oldFixedLength: true,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "comon_toilet_type",
                table: "school_detail",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(50) CHARACTER SET utf8mb4",
                oldFixedLength: true,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "common_toilet_accessibility",
                table: "school_detail",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(50) CHARACTER SET utf8mb4",
                oldFixedLength: true,
                oldMaxLength: 50,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "male_toilet_type",
                table: "school_detail",
                type: "char(50) CHARACTER SET utf8mb4",
                fixedLength: true,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "male_toilet_accessibility",
                table: "school_detail",
                type: "char(50) CHARACTER SET utf8mb4",
                fixedLength: true,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "female_toilet_type",
                table: "school_detail",
                type: "char(50) CHARACTER SET utf8mb4",
                fixedLength: true,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "female_toilet_accessibility",
                table: "school_detail",
                type: "char(50) CHARACTER SET utf8mb4",
                fixedLength: true,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "comon_toilet_type",
                table: "school_detail",
                type: "char(50) CHARACTER SET utf8mb4",
                fixedLength: true,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "common_toilet_accessibility",
                table: "school_detail",
                type: "char(50) CHARACTER SET utf8mb4",
                fixedLength: true,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldNullable: true);
        }
    }
}
