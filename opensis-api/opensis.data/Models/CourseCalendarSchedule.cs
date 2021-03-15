using System;
using System.Collections.Generic;

namespace opensis.data.Models
{
    public partial class CourseCalendarSchedule
    {
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int CourseId { get; set; }
        public int CourseSectionId { get; set; }
        public int? GradeScaleId { get; set; }
        public int Serial { get; set; }
        public DateTime? Date { get; set; }
        public int? BlockId { get; set; }
        public int? PeriodId { get; set; }
        public int? RoomId { get; set; }
        public bool? TakeAttendance { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public virtual BlockPeriod BlockPeriod { get; set; }
        public virtual Rooms Rooms { get; set; }
        
    }
}
