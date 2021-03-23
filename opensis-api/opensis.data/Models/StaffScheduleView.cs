using System;
using System.Collections.Generic;

namespace opensis.data.Models
{
    public partial class StaffScheduleView
    {
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int StaffId { get; set; }
        public int CourseId { get; set; }
        public string CourseShortName { get; set; }
        public int CourseSectionId { get; set; }
        public string CourseSectionName { get; set; }
        public string StaffInternalId { get; set; }
        public string StaffName { get; set; }
        public bool Scheduled { get; set; }
        public string ConflictComment { get; set; }
    }
}
