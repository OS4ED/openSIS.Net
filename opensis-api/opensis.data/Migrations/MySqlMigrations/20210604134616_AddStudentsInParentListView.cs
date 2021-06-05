using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.MySqlMigrations
{
    public partial class AddStudentsInParentListView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string script =
                   @"
                    DROP VIEW IF EXISTS parent_list_view;

                    CREATE VIEW `parent_list_view` AS
                    SELECT 
                        `parent_info`.`tenant_id` AS `tenant_id`,
                        `parent_info`.`school_id` AS `school_id`,
                        `parent_info`.`parent_id` AS `parent_id`,
                        `parent_info`.`firstname` AS `firstname`,
                        `parent_info`.`lastname` AS `lastname`,
                        `parent_info`.`home_phone` AS `home_phone`,
                        `parent_info`.`work_phone` AS `work_phone`,
                        `parent_info`.`mobile` AS `mobile`,
                        `parent_info`.`is_portal_user` AS `is_portal_user`,
                        `parent_info`.`bus_pickup` AS `bus_pickup`,
                        `parent_info`.`bus_dropoff` AS `bus_dropoff`,
                        `parent_info`.`last_updated` AS `last_updated`,
                        `parent_info`.`updated_by` AS `updated_by`,
                        `parent_info`.`bus_No` AS `bus_No`,
                        `parent_info`.`login_email` AS `login_email`,
                        `parent_info`.`middlename` AS `middlename`,
                        `parent_info`.`personal_email` AS `personal_email`,
                        `parent_info`.`salutation` AS `salutation`,
                        `parent_info`.`suffix` AS `suffix`,
                        `parent_info`.`user_profile` AS `user_profile`,
                        `parent_info`.`work_email` AS `work_email`,
                        `parent_info`.`parent_photo` AS `parent_photo`,
                        `parent_info`.`parent_guid` AS `parent_guid`,
                        `parent_associationship`.`student_id` AS `student_id`,
                        `student_master`.`first_given_name` AS `first_given_name`,
                        `student_master`.`middle_name` AS `student_middle_name`,
                        `student_master`.`last_family_name` AS `last_family_name`,
                        `parent_address`.`student_address_same` AS `student_address_same`,
                        `parent_address`.`address_line_one` AS `address_line_one`,
                        `parent_address`.`address_line_two` AS `address_line_two`,
                        `parent_address`.`country` AS `country`,
                        `parent_address`.`city` AS `city`,
                        `parent_address`.`state` AS `state`,
                        `parent_address`.`zip` AS `zip`,
                        `parent_associationship`.`associationship` AS `associationship`,
                        (SELECT 
                                GROUP_CONCAT(DISTINCT CONVERT( CONCAT(IFNULL(`student_master`.`student_id`, ''),
                                                '|') USING UTF8MB4),
                                        CONCAT(IFNULL(`student_master`.`first_given_name`, ''),
                                                '|'),
                                        CONCAT(IFNULL(`student_master`.`middle_name`, ''),
                                                '|'),
                                        IFNULL(`student_master`.`last_family_name`, '')
                                        SEPARATOR ',')
                            FROM
                                (`student_master`
                                JOIN `parent_associationship` `c` ON ((`student_master`.`student_id` = `c`.`student_id`)))
                            WHERE
                                ((`student_master`.`school_id` = `c`.`school_id`)
                                    AND (`student_master`.`tenant_id` = `c`.`tenant_id`)
                                    AND (`c`.`parent_id` = `parent_associationship`.`parent_id`)
                                    AND (`c`.`school_id` = `parent_associationship`.`school_id`))) AS `students`
                    FROM
                        (((`parent_associationship`
                        JOIN `parent_info` ON (((`parent_associationship`.`tenant_id` = `parent_info`.`tenant_id`)
                            AND (`parent_associationship`.`school_id` = `parent_info`.`school_id`)
                            AND (`parent_associationship`.`parent_id` = `parent_info`.`parent_id`))))
                        JOIN `student_master` ON (((`parent_associationship`.`tenant_id` = `student_master`.`tenant_id`)
                            AND (`parent_associationship`.`school_id` = `student_master`.`school_id`)
                            AND (`parent_associationship`.`student_id` = `student_master`.`student_id`))))
                        LEFT JOIN `parent_address` ON (((`parent_info`.`tenant_id` = `parent_address`.`tenant_id`)
                            AND (`parent_info`.`school_id` = `parent_address`.`school_id`)
                            AND (`parent_info`.`parent_id` = `parent_address`.`parent_id`))))";

            migrationBuilder.Sql(script);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string script = @"DROP VIEW parent_list_view";
            migrationBuilder.Sql(script);
        }
    }
}
