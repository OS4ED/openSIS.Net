using System;
using System.Collections.Generic;

namespace opensis.data.Models
{
    public partial class SchoolCalendars
    {
        public SchoolCalendars()
        {
            CourseSection = new HashSet<CourseSection>();
            StudentEffortGradeMaster = new HashSet<StudentEffortGradeMaster>();
        }
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int CalenderId { get; set; }
        public string Title { get; set; }
        public decimal? AcademicYear { get; set; }
        public bool? DefaultCalender { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string VisibleToMembershipId { get; set; }
        public string Days { get; set; }
        public int? RolloverId { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string UpdatedBy { get; set; }

        public virtual SchoolMaster SchoolMaster { get; set; }
        public virtual ICollection<CourseSection> CourseSection { get; set; }
        public virtual ICollection<StudentEffortGradeMaster> StudentEffortGradeMaster { get; set; }
    }
}
