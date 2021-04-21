using System;
using System.Collections.Generic;

namespace opensis.data.Models
{
    public partial class SchoolYears
    {
        public SchoolYears()
        {
            CourseSection = new HashSet<CourseSection>();
            HonorRolls = new HashSet<HonorRolls>();
            Semesters = new HashSet<Semesters>();
            StaffCoursesectionSchedule = new HashSet<StaffCoursesectionSchedule>();
            StudentFinalGrade = new HashSet<StudentFinalGrade>();
        }

        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int MarkingPeriodId { get; set; }
        public decimal? AcademicYear { get; set; }
        public string Title { get; set; }
        public string ShortName { get; set; }
        public decimal? SortOrder { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? PostStartDate { get; set; }
        public DateTime? PostEndDate { get; set; }
        public bool? DoesGrades { get; set; }
        public bool? DoesExam { get; set; }
        public bool? DoesComments { get; set; }
        public int? RolloverId { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string UpdatedBy { get; set; }

        public virtual SchoolMaster SchoolMaster { get; set; }
        public virtual ICollection<CourseSection> CourseSection { get; set; }
        public virtual ICollection<HonorRolls> HonorRolls { get; set; }
        public virtual ICollection<Semesters> Semesters { get; set; }
        public virtual ICollection<StaffCoursesectionSchedule> StaffCoursesectionSchedule { get; set; }
        public virtual ICollection<StudentFinalGrade> StudentFinalGrade { get; set; }
    }
}
