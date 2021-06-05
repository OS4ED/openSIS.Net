using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.MySqlMigrations
{
    public partial class CreateParentListView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string script =
                   @"
                    DROP VIEW IF EXISTS parent_list_view;

                    CREATE VIEW parent_list_view
                    AS
                    SELECT parent_info.*, parent_associationship.student_id, student_master.first_given_name, student_master.middle_name, student_master.last_family_name, parent_address.student_address_same, 
                    parent_address.address_line_one, parent_address.address_line_two, parent_address.country, parent_address.city, parent_address.state, parent_address.zip
                    FROM parent_associationship INNER JOIN
                    parent_info ON parent_associationship.tenant_id = parent_info.tenant_id AND parent_associationship.school_id = parent_info.school_id AND 
                    parent_associationship.parent_id = parent_info.parent_id INNER JOIN
                    student_master ON parent_associationship.tenant_id = student_master.tenant_id AND parent_associationship.school_id = student_master.school_id AND 
                    parent_associationship.student_id = student_master.student_id LEFT OUTER JOIN
                    parent_address ON parent_info.tenant_id = parent_address.tenant_id AND parent_info.school_id = parent_address.school_id AND parent_info.parent_id = parent_address.parent_id";

            migrationBuilder.Sql(script);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string script = @"DROP VIEW parent_list_view";
            migrationBuilder.Sql(script);
        }
    }
}
