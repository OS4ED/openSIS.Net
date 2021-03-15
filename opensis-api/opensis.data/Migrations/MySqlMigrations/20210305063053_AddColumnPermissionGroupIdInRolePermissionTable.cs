using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.MySqlMigrations
{
    public partial class AddColumnPermissionGroupIdInRolePermissionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "user_master",
                unicode: false,
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "permission_group_id",
                table: "role_permission",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "membership",
                unicode: false,
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "user_master");

            migrationBuilder.DropColumn(
                name: "permission_group_id",
                table: "role_permission");

            migrationBuilder.DropColumn(
                name: "description",
                table: "membership");
        }
    }
}
