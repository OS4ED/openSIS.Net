using System;
using System.Collections.Generic;

namespace opensis.data.Models
{
    public partial class StudentCoursesectionSchedule
    {
        public StudentCoursesectionSchedule()
        {
            StudentAttendance = new HashSet<StudentAttendance>();
        }
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int StudentId { get; set; }
        public Guid StudentGuid { get; set; }
        public string AlternateId { get; set; }
        public string StudentInternalId { get; set; }
        public string FirstGivenName { get; set; }
        public string MiddleName { get; set; }
        public string LastFamilyName { get; set; }
        public int? FirstLanguageId { get; set; }
        public int? GradeId { get; set; }
        public int CourseId { get; set; }
        public int CourseSectionId { get; set; }
        public decimal AcademicYear { get; set; }
        public int? GradeScaleId { get; set; }
        public string CourseSectionName { get; set; }
        public int? CalendarId { get; set; }
        public bool? IsDropped { get; set; }
        public DateTime? EffectiveDropDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual CourseSection CourseSection { get; set; }
        public virtual SchoolMaster SchoolMaster { get; set; }
        public virtual StudentMaster StudentMaster { get; set; }

        public virtual ICollection<StudentAttendance> StudentAttendance { get; set; }
    }
}
