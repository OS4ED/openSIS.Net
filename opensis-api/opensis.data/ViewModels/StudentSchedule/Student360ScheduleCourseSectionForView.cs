using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.StudentSchedule
{
    public class Student360ScheduleCourseSectionForView
    {        
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int CourseId { get; set; }
        public int CourseSectionId { get; set; }
        public string CourseTitle { get; set; }
        public string CourseSectionName { get; set; }
        public DateTime? CourseSectionDurationStartDate { get; set; }
        public DateTime? CourseSectionDurationEndDate { get; set; }
        public int? YrMarkingPeriodId { get; set; }
        public int? SmstrMarkingPeriodId { get; set; }
        public int? QtrMarkingPeriodId { get; set; }
        public DateTime? EnrolledDate { get; set; }
        public DateTime? EffectiveDropDate { get; set; }
        public string DayOfWeek { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsDropped { get; set; }
        public bool? IsAssociationship { get; set; }

        public CourseFixedSchedule courseFixedSchedule { get; set; }
        public List<CourseVariableSchedule> courseVariableScheduleList { get; set; }
        public List<CourseCalendarSchedule> courseCalendarScheduleList { get; set; }
        public List<CourseBlockSchedule> courseBlockScheduleList { get; set; }
        public List<StaffMaster> staffMasterList { get; set; }
        public List<StudentAttendance> studentAttendanceList { get; set; }
        public  SchoolYears SchoolYears { get; set; }
        public  Semesters Semesters { get; set; }
        public  Quarters Quarters { get; set; }
    }
}
