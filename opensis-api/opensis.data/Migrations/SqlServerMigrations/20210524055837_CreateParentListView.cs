using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.SqlServerMigrations
{
    public partial class CreateParentListView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string script =
                   @"
                    DROP VIEW IF EXISTS [dbo].[parent_list_view] ;  
                    GO

                    CREATE VIEW [dbo].[parent_list_view]
                    AS
                    SELECT        dbo.parent_info.*, dbo.parent_associationship.student_id, dbo.student_master.first_given_name, dbo.student_master.middle_name, dbo.student_master.last_family_name, dbo.parent_address.student_address_same, 
                      dbo.parent_address.address_line_one, dbo.parent_address.address_line_two, dbo.parent_address.country, dbo.parent_address.city, dbo.parent_address.state, dbo.parent_address.zip
                    FROM dbo.parent_associationship INNER JOIN
                        dbo.parent_info ON dbo.parent_associationship.tenant_id = dbo.parent_info.tenant_id AND dbo.parent_associationship.school_id = dbo.parent_info.school_id AND 
                        dbo.parent_associationship.parent_id = dbo.parent_info.parent_id INNER JOIN
                        dbo.student_master ON dbo.parent_associationship.tenant_id = dbo.student_master.tenant_id AND dbo.parent_associationship.school_id = dbo.student_master.school_id AND 
                        dbo.parent_associationship.student_id = dbo.student_master.student_id LEFT OUTER JOIN
                        dbo.parent_address ON dbo.parent_info.tenant_id = dbo.parent_address.tenant_id AND dbo.parent_info.school_id = dbo.parent_address.school_id AND dbo.parent_info.parent_id = dbo.parent_address.parent_id
                    GO";

            migrationBuilder.Sql(script);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string script = @"DROP VIEW dbo.parent_list_view";
            migrationBuilder.Sql(script);
        }
    }
}
