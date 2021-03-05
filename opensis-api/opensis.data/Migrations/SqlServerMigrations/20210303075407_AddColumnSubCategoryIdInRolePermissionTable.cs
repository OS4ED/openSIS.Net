using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.SqlServerMigrations
{
    public partial class AddColumnSubCategoryIdInRolePermissionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_role_permission_tenant_id_school_id_permission_category_id",
                table: "role_permission");

            migrationBuilder.AddColumn<int>(
                name: "permission_subcategory_id",
                table: "role_permission",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_role_permission_tenant_id_school_id_permission_category_id_permission_subcategory_id",
                table: "role_permission",
                columns: new[] { "tenant_id", "school_id", "permission_category_id", "permission_subcategory_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_permission_subcategory",
                table: "role_permission",
                columns: new[] { "tenant_id", "school_id", "permission_category_id", "permission_subcategory_id" },
                principalTable: "permission_subcategory",
                principalColumns: new[] { "tenant_id", "school_id", "permission_category_id", "permission_subcategory_id" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_permission_subcategory",
                table: "role_permission");

            migrationBuilder.DropIndex(
                name: "IX_role_permission_tenant_id_school_id_permission_category_id_permission_subcategory_id",
                table: "role_permission");

            migrationBuilder.DropColumn(
                name: "permission_subcategory_id",
                table: "role_permission");

            migrationBuilder.CreateIndex(
                name: "IX_role_permission_tenant_id_school_id_permission_category_id",
                table: "role_permission",
                columns: new[] { "tenant_id", "school_id", "permission_category_id" });
        }
    }
}
