using System;
using System.Collections.Generic;

namespace opensis.data.Models
{
    public partial class StaffCoursesectionSchedule
    {
        public StaffCoursesectionSchedule()
        {
            StudentAttendance = new HashSet<StudentAttendance>();
        }
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int StaffId { get; set; }
        public Guid StaffGuid { get; set; }
        public int CourseId { get; set; }
        public int CourseSectionId { get; set; }
        public string CourseSectionName { get; set; }
        public bool? IsDropped { get; set; }
        public DateTime? EffectiveDropDate { get; set; }
        public bool? IsAssigned { get; set; }
        public int? YrMarkingPeriodId { get; set; }
        public int? SmstrMarkingPeriodId { get; set; }
        public int? QtrMarkingPeriodId { get; set; }
        public DateTime? DurationStartDate { get; set; }
        public DateTime? DurationEndDate { get; set; }
        public string MeetingDays { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual CourseSection CourseSection { get; set; }
        public virtual Quarters Quarters { get; set; }
        public virtual SchoolYears SchoolYears { get; set; }
        public virtual Semesters Semesters { get; set; }
        public virtual StaffMaster StaffMaster { get; set; }
        public virtual ICollection<StudentAttendance> StudentAttendance { get; set; }
    }
}
