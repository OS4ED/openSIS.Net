using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.MySqlMigrations
{
    public partial class AddTablePermissionSubCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "permission_subcategory",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(nullable: false),
                    school_id = table.Column<int>(nullable: false),
                    permission_category_id = table.Column<int>(nullable: false),
                    permission_subcategory_id = table.Column<int>(nullable: false),
                    permission_group_id = table.Column<int>(nullable: false),
                    permission_subcategory_name = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    short_code = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    path = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    title = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    type = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    enable_view = table.Column<bool>(nullable: true),
                    enable_add = table.Column<bool>(nullable: true),
                    enable_edit = table.Column<bool>(nullable: true),
                    enable_delete = table.Column<bool>(nullable: true),
                    created_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permission_subcategory", x => new { x.tenant_id, x.school_id, x.permission_category_id, x.permission_subcategory_id });
                    table.ForeignKey(
                        name: "FK_permission_subcategory_permission_category",
                        columns: x => new { x.tenant_id, x.school_id, x.permission_category_id },
                        principalTable: "permission_category",
                        principalColumns: new[] { "tenant_id", "school_id", "permission_category_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "search_filter",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(nullable: false),
                    school_id = table.Column<int>(nullable: false),
                    module = table.Column<string>(unicode: false, fixedLength: true, maxLength: 15, nullable: false),
                    filter_id = table.Column<int>(nullable: false),
                    filter_name = table.Column<string>(unicode: false, fixedLength: true, maxLength: 50, nullable: true),
                    emailaddress = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    json_list = table.Column<string>(unicode: false, nullable: true),
                    created_by = table.Column<string>(unicode: false, fixedLength: true, maxLength: 50, nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime", nullable: true),
                    modified_by = table.Column<string>(unicode: false, fixedLength: true, maxLength: 50, nullable: true),
                    date_modifed = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_search_filter", x => new { x.tenant_id, x.school_id, x.module, x.filter_id });
                    table.ForeignKey(
                        name: "FK_search_filter_school_master",
                        columns: x => new { x.tenant_id, x.school_id },
                        principalTable: "school_master",
                        principalColumns: new[] { "tenant_id", "school_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_search_filter_user_master",
                        columns: x => new { x.tenant_id, x.school_id, x.emailaddress },
                        principalTable: "user_master",
                        principalColumns: new[] { "tenant_id", "school_id", "emailaddress" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_search_filter_tenant_id_school_id_emailaddress",
                table: "search_filter",
                columns: new[] { "tenant_id", "school_id", "emailaddress" });

            migrationBuilder.CreateIndex(
                name: "IX_search_filter",
                table: "search_filter",
                columns: new[] { "tenant_id", "school_id", "filter_name" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "permission_subcategory");

            migrationBuilder.DropTable(
                name: "search_filter");
        }
    }
}
