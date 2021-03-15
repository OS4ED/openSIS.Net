using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.SqlServerMigrations
{
    public partial class AlterFKPKPermissionCategoryIdInPermissionSubCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_permission_GroupId",
                table: "role_permission");

            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_permission_subcategory",
                table: "role_permission");

            migrationBuilder.DropIndex(
                name: "IX_role_permission_tenant_id_school_id_permission_category_id_permission_subcategory_id",
                table: "role_permission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_permission_subcategory",
                table: "permission_subcategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_permission_subcategory_1",
                table: "permission_subcategory",
                columns: new[] { "tenant_id", "school_id", "permission_subcategory_id" });

            migrationBuilder.CreateIndex(
                name: "IX_role_permission_tenant_id_school_id_permission_category_id",
                table: "role_permission",
                columns: new[] { "tenant_id", "school_id", "permission_category_id" });

            migrationBuilder.CreateIndex(
                name: "IX_role_permission_tenant_id_school_id_permission_subcategory_id",
                table: "role_permission",
                columns: new[] { "tenant_id", "school_id", "permission_subcategory_id" });

            migrationBuilder.CreateIndex(
                name: "IX_permission_subcategory_tenant_id_school_id_permission_category_id",
                table: "permission_subcategory",
                columns: new[] { "tenant_id", "school_id", "permission_category_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_permission_groupId",
                table: "role_permission",
                columns: new[] { "tenant_id", "school_id", "permission_group_id" },
                principalTable: "permission_group",
                principalColumns: new[] { "tenant_id", "school_id", "permission_group_id" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_permission_subcategory",
                table: "role_permission",
                columns: new[] { "tenant_id", "school_id", "permission_subcategory_id" },
                principalTable: "permission_subcategory",
                principalColumns: new[] { "tenant_id", "school_id", "permission_subcategory_id" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_permission_groupId",
                table: "role_permission");

            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_permission_subcategory",
                table: "role_permission");

            migrationBuilder.DropIndex(
                name: "IX_role_permission_tenant_id_school_id_permission_category_id",
                table: "role_permission");

            migrationBuilder.DropIndex(
                name: "IX_role_permission_tenant_id_school_id_permission_subcategory_id",
                table: "role_permission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_permission_subcategory_1",
                table: "permission_subcategory");

            migrationBuilder.DropIndex(
                name: "IX_permission_subcategory_tenant_id_school_id_permission_category_id",
                table: "permission_subcategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_permission_subcategory",
                table: "permission_subcategory",
                columns: new[] { "tenant_id", "school_id", "permission_category_id", "permission_subcategory_id" });

            migrationBuilder.CreateIndex(
                name: "IX_role_permission_tenant_id_school_id_permission_category_id_permission_subcategory_id",
                table: "role_permission",
                columns: new[] { "tenant_id", "school_id", "permission_category_id", "permission_subcategory_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_permission_GroupId",
                table: "role_permission",
                columns: new[] { "tenant_id", "school_id", "permission_group_id" },
                principalTable: "permission_group",
                principalColumns: new[] { "tenant_id", "school_id", "permission_group_id" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_permission_subcategory",
                table: "role_permission",
                columns: new[] { "tenant_id", "school_id", "permission_category_id", "permission_subcategory_id" },
                principalTable: "permission_subcategory",
                principalColumns: new[] { "tenant_id", "school_id", "permission_category_id", "permission_subcategory_id" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
