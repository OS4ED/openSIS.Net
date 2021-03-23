using Microsoft.EntityFrameworkCore.Migrations;

namespace opensis.data.Migrations.MySqlMigrations
{
    public partial class AddCalendarIdInAllCourseSectionView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string script =
                   @"
                    DROP VIEW IF EXISTS all_course_section_view;

                    CREATE VIEW all_course_section_view
                    AS
                    SELECT course.tenant_id, course.school_id, course.course_id, course.course_title, course.course_subject, course.course_program,         course_section.academic_year, course_section.course_section_id, 
		                    course_section.course_section_name, course_section.seats, course_section.duration_start_date, course_section.duration_end_date, course_section.yr_marking_period_id, 
		                    course_section.qtr_marking_period_id, course_section.smstr_marking_period_id, course_section.schedule_type, course_section.meeting_days AS fixed_days, 
		                    course_fixed_schedule.room_id AS fixed_room_id, course_fixed_schedule.period_id AS fixed_period_id, course_variable_schedule.day AS var_day, course_variable_schedule.period_id AS var_period_id, 
		                    course_variable_schedule.room_id AS var_room_id, course_calendar_schedule.date AS cal_date, course_calendar_schedule.period_id AS cal_period_id, course_calendar_schedule.room_id AS cal_room_id, 
		                    course_block_schedule.period_id AS block_period_id, course_block_schedule.room_id AS block_room_id, course_section.is_active, course.course_grade_level, course_section.grade_scale_id, 
		                    course_section.allow_teacher_conflict, course_section.allow_student_conflict, course_section.calendar_id
                    FROM course INNER JOIN
		                    course_section ON course.tenant_id = course_section.tenant_id AND course.school_id = course_section.school_id AND course.course_id = course_section.course_id LEFT OUTER JOIN
		                    course_fixed_schedule ON course_section.tenant_id = course_fixed_schedule.tenant_id AND course_section.school_id = course_fixed_schedule.school_id AND 
		                    course_section.course_id = course_fixed_schedule.course_id AND course_section.course_section_id = course_fixed_schedule.course_section_id LEFT OUTER JOIN
		                    course_variable_schedule ON course_section.tenant_id = course_variable_schedule.tenant_id AND course_section.school_id = course_variable_schedule.school_id AND 
		                    course_section.course_id = course_variable_schedule.course_id AND course_section.course_section_id = course_variable_schedule.course_section_id LEFT OUTER JOIN
		                    course_calendar_schedule ON course_section.tenant_id = course_calendar_schedule.tenant_id AND course_section.school_id = course_calendar_schedule.school_id AND 
		                    course_section.course_id = course_calendar_schedule.course_id AND course_section.course_section_id = course_calendar_schedule.course_section_id LEFT OUTER JOIN
		                    course_block_schedule ON course_section.tenant_id = course_block_schedule.tenant_id AND course_section.school_id = course_block_schedule.school_id AND 
		                    course_section.course_id = course_block_schedule.course_id AND course_section.course_section_id = course_block_schedule.course_section_id";

            migrationBuilder.Sql(script);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string script = @"DROP VIEW all_course_section_view";
            migrationBuilder.Sql(script);
        }
    }
}
