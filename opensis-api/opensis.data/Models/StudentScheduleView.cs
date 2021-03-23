using System;
using System.Collections.Generic;

namespace opensis.data.Models
{
    public partial class StudentScheduleView
    {
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int CourseSectionId { get; set; }
        public string CourseSectionName { get; set; }
        public string StudentInternalId { get; set; }
        public string StudentName { get; set; }
        public bool Scheduled { get; set; }
        public string ConflictComment { get; set; }
    }
}
