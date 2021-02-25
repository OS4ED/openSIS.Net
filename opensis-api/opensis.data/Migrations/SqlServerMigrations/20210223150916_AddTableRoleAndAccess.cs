using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.SqlServerMigrations
{
    public partial class AddTableRoleAndAccess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "mobile_phone",
                table: "student_master",
                unicode: false,
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(15)",
                oldUnicode: false,
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "home_phone",
                table: "student_master",
                unicode: false,
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(15)",
                oldUnicode: false,
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "work_phone",
                table: "parent_info",
                unicode: false,
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(15)",
                oldUnicode: false,
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "mobile",
                table: "parent_info",
                unicode: false,
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(15)",
                oldUnicode: false,
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "home_phone",
                table: "parent_info",
                unicode: false,
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(15)",
                oldUnicode: false,
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "state_code",
                table: "attendance_code",
                unicode: false,
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1)",
                oldUnicode: false,
                oldMaxLength: 1,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "permission_group",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(nullable: false),
                    school_id = table.Column<int>(nullable: false),
                    permission_group_id = table.Column<int>(nullable: false),
                    permission_group_name = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    short_name = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    is_active = table.Column<bool>(nullable: true),
                    is_system = table.Column<bool>(nullable: false),
                    title = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    icon = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    icon_type = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    sort_order = table.Column<int>(nullable: true),
                    type = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    path = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    badgeType = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    badgeValue = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    active = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permission_group", x => new { x.tenant_id, x.school_id, x.permission_group_id });
                });

            migrationBuilder.CreateTable(
                name: "release_number",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(nullable: false),
                    school_id = table.Column<int>(nullable: false),
                    release_number = table.Column<string>(unicode: false, maxLength: 9, nullable: false, comment: "999.99.99"),
                    release_date = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_release_number", x => new { x.tenant_id, x.school_id, x.release_number });
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(nullable: false),
                    school_id = table.Column<int>(nullable: false),
                    role_id = table.Column<int>(nullable: false),
                    role_name = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    is_active = table.Column<bool>(nullable: false),
                    is_system = table.Column<bool>(nullable: true),
                    is_superadmin = table.Column<bool>(nullable: true),
                    created_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles_1", x => new { x.tenant_id, x.school_id, x.role_id });
                });

            migrationBuilder.CreateTable(
                name: "permission_category",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(nullable: false),
                    school_id = table.Column<int>(nullable: false),
                    permission_category_id = table.Column<int>(nullable: false),
                    permission_group_id = table.Column<int>(nullable: false),
                    permission_category_name = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_permission_category_1", x => new { x.tenant_id, x.school_id, x.permission_category_id });
                    table.ForeignKey(
                        name: "FK_permission_category_permission_group",
                        columns: x => new { x.tenant_id, x.school_id, x.permission_group_id },
                        principalTable: "permission_group",
                        principalColumns: new[] { "tenant_id", "school_id", "permission_group_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "role_permission",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(nullable: false),
                    school_id = table.Column<int>(nullable: false),
                    role_permission_id = table.Column<int>(nullable: false),
                    role_id = table.Column<int>(nullable: true),
                    permission_category_id = table.Column<int>(nullable: true),
                    can_view = table.Column<bool>(nullable: true),
                    can_add = table.Column<bool>(nullable: true),
                    can_edit = table.Column<bool>(nullable: true),
                    can_delete = table.Column<bool>(nullable: true),
                    created_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_permission", x => new { x.tenant_id, x.school_id, x.role_permission_id });
                    table.ForeignKey(
                        name: "FK_role_permission_permission_category",
                        columns: x => new { x.tenant_id, x.school_id, x.permission_category_id },
                        principalTable: "permission_category",
                        principalColumns: new[] { "tenant_id", "school_id", "permission_category_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_role_permission_roles",
                        columns: x => new { x.tenant_id, x.school_id, x.role_id },
                        principalTable: "roles",
                        principalColumns: new[] { "tenant_id", "school_id", "role_id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_permission_category_tenant_id_school_id_permission_group_id",
                table: "permission_category",
                columns: new[] { "tenant_id", "school_id", "permission_group_id" });

            migrationBuilder.CreateIndex(
                name: "IX_role_permission_tenant_id_school_id_permission_category_id",
                table: "role_permission",
                columns: new[] { "tenant_id", "school_id", "permission_category_id" });

            migrationBuilder.CreateIndex(
                name: "IX_role_permission_tenant_id_school_id_role_id",
                table: "role_permission",
                columns: new[] { "tenant_id", "school_id", "role_id" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "release_number");

            migrationBuilder.DropTable(
                name: "role_permission");

            migrationBuilder.DropTable(
                name: "permission_category");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "permission_group");

            migrationBuilder.AlterColumn<string>(
                name: "mobile_phone",
                table: "student_master",
                type: "varchar(15)",
                unicode: false,
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "home_phone",
                table: "student_master",
                type: "varchar(15)",
                unicode: false,
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "work_phone",
                table: "parent_info",
                type: "varchar(15)",
                unicode: false,
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "mobile",
                table: "parent_info",
                type: "varchar(15)",
                unicode: false,
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "home_phone",
                table: "parent_info",
                type: "varchar(15)",
                unicode: false,
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "state_code",
                table: "attendance_code",
                type: "varchar(1)",
                unicode: false,
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 8,
                oldNullable: true);
        }
    }
}
