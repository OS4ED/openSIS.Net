using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.MySqlMigrations
{
    public partial class AlterColumnTitleInCustomFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "custom_fields",
                unicode: false,
                maxLength: 50,
                nullable: true,
                comment: "Field Name",
                oldClrType: typeof(string),
                oldType: "varchar(30) CHARACTER SET utf8mb4",
                oldUnicode: false,
                oldMaxLength: 30,
                oldNullable: true,
                oldComment: "Field Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "custom_fields",
                type: "varchar(30) CHARACTER SET utf8mb4",
                unicode: false,
                maxLength: 30,
                nullable: true,
                comment: "Field Name",
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "Field Name");
        }
    }
}
