using System;
using System.Collections.Generic;

namespace opensis.data.Models
{
    public partial class BlockPeriod
    {
        public BlockPeriod()
        {
            CourseBlockSchedule = new HashSet<CourseBlockSchedule>();
            CourseCalendarSchedule = new HashSet<CourseCalendarSchedule>();
            CourseFixedSchedule = new HashSet<CourseFixedSchedule>();
            CourseVariableSchedule = new HashSet<CourseVariableSchedule>();
            StudentAttendance = new HashSet<StudentAttendance>();
        }
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int BlockId { get; set; }
        public int PeriodId { get; set; }
        public string PeriodTitle { get; set; }
        public string PeriodShortName { get; set; }
        public string PeriodStartTime { get; set; }
        public string PeriodEndTime { get; set; }
        public int? PeriodSortOrder { get; set; }
        public bool? CalculateAttendance { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual SchoolMaster SchoolMaster { get; set; }
        public virtual ICollection<CourseBlockSchedule> CourseBlockSchedule { get; set; }
        public virtual ICollection<CourseCalendarSchedule> CourseCalendarSchedule { get; set; }
        public virtual ICollection<CourseFixedSchedule> CourseFixedSchedule { get; set; }
        public virtual ICollection<CourseVariableSchedule> CourseVariableSchedule { get; set; }
        public virtual ICollection<StudentAttendance> StudentAttendance { get; set; }
    }
}
