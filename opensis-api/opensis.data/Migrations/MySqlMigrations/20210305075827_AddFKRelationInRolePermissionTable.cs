using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.MySqlMigrations
{
    public partial class AddFKRelationInRolePermissionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_role_permission_tenant_id_school_id_permission_group_id",
                table: "role_permission",
                columns: new[] { "tenant_id", "school_id", "permission_group_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_permission_groupId",
                table: "role_permission",
                columns: new[] { "tenant_id", "school_id", "permission_group_id" },
                principalTable: "permission_group",
                principalColumns: new[] { "tenant_id", "school_id", "permission_group_id" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_permission_groupId",
                table: "role_permission");

            migrationBuilder.DropIndex(
                name: "IX_role_permission_tenant_id_school_id_permission_group_id",
                table: "role_permission");
        }
    }
}
