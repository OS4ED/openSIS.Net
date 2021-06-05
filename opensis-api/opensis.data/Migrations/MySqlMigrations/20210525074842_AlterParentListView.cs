using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.MySqlMigrations
{
    public partial class AlterParentListView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string script =
                   @"
                    DROP VIEW IF EXISTS parent_list_view;

                    CREATE VIEW parent_list_view
                    AS
                    SELECT parent_info.tenant_id, parent_info.school_id, parent_info.parent_id, parent_info.firstname, parent_info.lastname, parent_info.home_phone, parent_info.work_phone, parent_info.mobile, 
                        parent_info.is_portal_user, parent_info.bus_pickup, parent_info.bus_dropoff, parent_info.last_updated, parent_info.updated_by, parent_info.bus_No, parent_info.login_email, 
                        parent_info.middlename, parent_info.personal_email, parent_info.salutation, parent_info.suffix, parent_info.user_profile, parent_info.work_email, parent_info.parent_photo, 
                        parent_info.parent_guid, parent_associationship.student_id, student_master.first_given_name, student_master.middle_name AS student_middle_name, student_master.last_family_name, 
                        parent_address.student_address_same, parent_address.address_line_one, parent_address.address_line_two, parent_address.country, parent_address.city, parent_address.state, 
                        parent_address.zip, parent_associationship.associationship
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
