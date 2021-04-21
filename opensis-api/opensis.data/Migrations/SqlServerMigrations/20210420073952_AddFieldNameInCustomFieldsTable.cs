using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.SqlServerMigrations
{
    public partial class AddFieldNameInCustomFieldsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "custom_fields",
                unicode: false,
                maxLength: 50,
                nullable: true,
                comment: "Field Title",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "Field Name");

            migrationBuilder.AddColumn<string>(
                name: "field_name",
                table: "custom_fields",
                unicode: false,
                maxLength: 50,
                nullable: true,
                comment: "Field Name");

            migrationBuilder.CreateIndex(
                name: "IX_custom_fields",
                table: "custom_fields",
                columns: new[] { "tenant_id", "school_id", "title" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_custom_fields",
                table: "custom_fields");

            migrationBuilder.DropColumn(
                name: "field_name",
                table: "custom_fields");

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "custom_fields",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true,
                comment: "Field Name",
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "Field Title");
        }
    }
}
