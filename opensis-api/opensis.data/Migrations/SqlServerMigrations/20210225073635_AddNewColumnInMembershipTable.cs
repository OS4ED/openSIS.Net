using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.SqlServerMigrations
{
    public partial class AddNewColumnInMembershipTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_roles",
                table: "role_permission");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropIndex(
                name: "IX_role_permission_tenant_id_school_id_role_id",
                table: "role_permission");

            migrationBuilder.DropColumn(
                name: "role_id",
                table: "role_permission");

            migrationBuilder.DropColumn(
                name: "access",
                table: "membership");

            migrationBuilder.DropColumn(
                name: "last_updated",
                table: "membership");

            migrationBuilder.DropColumn(
                name: "title",
                table: "membership");

            migrationBuilder.DropColumn(
                name: "weekly_update",
                table: "membership");

            migrationBuilder.AlterColumn<string>(
                name: "office_phone",
                table: "staff_master",
                unicode: false,
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(15)",
                oldUnicode: false,
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "mobile_phone",
                table: "staff_master",
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
                table: "staff_master",
                unicode: false,
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(15)",
                oldUnicode: false,
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "emergency_work_phone",
                table: "staff_master",
                unicode: false,
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(15)",
                oldUnicode: false,
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "emergency_mobile_phone",
                table: "staff_master",
                unicode: false,
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(15)",
                oldUnicode: false,
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "emergency_home_phone",
                table: "staff_master",
                unicode: false,
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(15)",
                oldUnicode: false,
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "telephone",
                table: "school_detail",
                fixedLength: true,
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nchar(20)",
                oldFixedLength: true,
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "membership_id",
                table: "role_permission",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "updated_by",
                table: "membership",
                unicode: false,
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "membership",
                unicode: false,
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_on",
                table: "membership",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "membership",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_superadmin",
                table: "membership",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_system",
                table: "membership",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_on",
                table: "membership",
                type: "datetime",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_role_permission_tenant_id_school_id_membership_id",
                table: "role_permission",
                columns: new[] { "tenant_id", "school_id", "membership_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_permission_group_school_master",
                table: "permission_group",
                columns: new[] { "tenant_id", "school_id" },
                principalTable: "school_master",
                principalColumns: new[] { "tenant_id", "school_id" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_membership",
                table: "role_permission",
                columns: new[] { "tenant_id", "school_id", "membership_id" },
                principalTable: "membership",
                principalColumns: new[] { "tenant_id", "school_id", "membership_id" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_permission_group_school_master",
                table: "permission_group");

            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_membership",
                table: "role_permission");

            migrationBuilder.DropIndex(
                name: "IX_role_permission_tenant_id_school_id_membership_id",
                table: "role_permission");

            migrationBuilder.DropColumn(
                name: "membership_id",
                table: "role_permission");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "membership");

            migrationBuilder.DropColumn(
                name: "created_on",
                table: "membership");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "membership");

            migrationBuilder.DropColumn(
                name: "is_superadmin",
                table: "membership");

            migrationBuilder.DropColumn(
                name: "is_system",
                table: "membership");

            migrationBuilder.DropColumn(
                name: "updated_on",
                table: "membership");

            migrationBuilder.AlterColumn<string>(
                name: "office_phone",
                table: "staff_master",
                type: "varchar(15)",
                unicode: false,
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "mobile_phone",
                table: "staff_master",
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
                table: "staff_master",
                type: "varchar(15)",
                unicode: false,
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "emergency_work_phone",
                table: "staff_master",
                type: "varchar(15)",
                unicode: false,
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "emergency_mobile_phone",
                table: "staff_master",
                type: "varchar(15)",
                unicode: false,
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "emergency_home_phone",
                table: "staff_master",
                type: "varchar(15)",
                unicode: false,
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "telephone",
                table: "school_detail",
                type: "nchar(20)",
                fixedLength: true,
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldFixedLength: true,
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "role_id",
                table: "role_permission",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "updated_by",
                table: "membership",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "access",
                table: "membership",
                type: "varchar(max)",
                unicode: false,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_updated",
                table: "membership",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "title",
                table: "membership",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                comment: "e.g. Administrator,Student,Teacher,Dept. head");

            migrationBuilder.AddColumn<bool>(
                name: "weekly_update",
                table: "membership",
                type: "bit",
                nullable: true,
                defaultValueSql: "((0))");

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    school_id = table.Column<int>(type: "int", nullable: false),
                    role_id = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    is_superadmin = table.Column<bool>(type: "bit", nullable: true),
                    is_system = table.Column<bool>(type: "bit", nullable: true),
                    role_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    updated_by = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles_1", x => new { x.tenant_id, x.school_id, x.role_id });
                });

            migrationBuilder.CreateIndex(
                name: "IX_role_permission_tenant_id_school_id_role_id",
                table: "role_permission",
                columns: new[] { "tenant_id", "school_id", "role_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_roles",
                table: "role_permission",
                columns: new[] { "tenant_id", "school_id", "role_id" },
                principalTable: "roles",
                principalColumns: new[] { "tenant_id", "school_id", "role_id" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
