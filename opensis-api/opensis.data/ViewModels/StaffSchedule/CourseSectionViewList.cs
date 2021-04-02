using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.StaffSchedule
{
    public class CourseSectionViewList
    {
        public CourseSectionViewList()
        {
            courseVariableSchedule = new List<CourseVariableSchedule>();
            courseCalendarSchedule = new List<CourseCalendarSchedule>();
            courseBlockSchedule = new List<CourseBlockSchedule>();
        }
        public int? CourseSectionId { get; set; }
        public int? CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string CourseSectionName { get; set; }
        public DateTime? DurationStartDate { get; set; }
        public DateTime? DurationEndDate { get; set; }
        public int? YrMarkingPeriodId { get; set; }
        public int? QtrMarkingPeriodId { get; set; }
        public int? SmstrMarkingPeriodId { get; set; }
        public string ScheduleType { get; set; }
        public string MeetingDays { get; set; }
        public string ScheduledStaff { get; set; }
        public string WeekDays { get; set; }
        public bool? TakeAttendanceForFixedSchedule { get; set; }
        public bool? ConflictCourseSection { get; set; }

        public CourseFixedSchedule courseFixedSchedule { get; set; }
        public List<CourseVariableSchedule> courseVariableSchedule { get; set; }
        public List<CourseCalendarSchedule> courseCalendarSchedule { get; set; }
        public List<CourseBlockSchedule> courseBlockSchedule { get; set; }

    }
}
