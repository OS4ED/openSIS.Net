using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.Models
{
    public partial class AttendanceCode
    {
        public AttendanceCode()
        {
            StudentAttendance = new HashSet<StudentAttendance>();
        }

        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int AttendanceCategoryId { get; set; }
        public int AttendanceCode1 { get; set; }
        public decimal? AcademicYear { get; set; }
        public string Title { get; set; }
        public string ShortName { get; set; }
        public string Type { get; set; }
        public string StateCode { get; set; }
        public bool? DefaultCode { get; set; }
        public string AllowEntryBy { get; set; }
        public int? SortOrder { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string UpdatedBy { get; set; }

        public virtual AttendanceCodeCategories AttendanceCodeCategories { get; set; }
        public virtual ICollection<StudentAttendance> StudentAttendance { get; set; }
    }
}
