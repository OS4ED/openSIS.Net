using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.MySqlMigrations
{
    public partial class AddTableSchoolPreference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "comments",
                table: "student_attendance");

            migrationBuilder.CreateTable(
                name: "school_preference",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(nullable: false),
                    school_id = table.Column<int>(nullable: false),
                    school_preference_id = table.Column<long>(nullable: false),
                    school_guid = table.Column<Guid>(nullable: false),
                    school_internal_id = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    school_alt_id = table.Column<string>(unicode: false, fixedLength: true, maxLength: 50, nullable: true),
                    full_day_minutes = table.Column<int>(nullable: true),
                    half_day_minutes = table.Column<int>(nullable: true),
                    max_login_failure = table.Column<int>(nullable: true),
                    max_inactivity_days = table.Column<int>(nullable: true),
                    created_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    created_on = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_by = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    updated_on = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_school_preference_1", x => new { x.tenant_id, x.school_id, x.school_preference_id });
                    table.ForeignKey(
                        name: "FK_school_preference_school_master",
                        columns: x => new { x.tenant_id, x.school_id },
                        principalTable: "school_master",
                        principalColumns: new[] { "tenant_id", "school_id" },
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "school_preference");

            migrationBuilder.AddColumn<string>(
                name: "comments",
                table: "student_attendance",
                type: "varchar(250) CHARACTER SET utf8mb4",
                unicode: false,
                maxLength: 250,
                nullable: true);
        }
    }
}
