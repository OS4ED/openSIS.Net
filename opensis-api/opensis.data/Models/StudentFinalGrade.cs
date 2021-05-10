using System;
using System.Collections.Generic;

namespace opensis.data.Models
{
    public partial class StudentFinalGrade
    {
       
        public StudentFinalGrade()
        {
            StudentFinalGradeComments = new HashSet<StudentFinalGradeComments>();
            StudentFinalGradeStandard = new HashSet<StudentFinalGradeStandard>();
        }

        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int StudentId { get; set; }
        public long StudentFinalGradeSrlno { get; set; }
        public bool? BasedOnStandardGrade { get; set; }
        public int CourseId { get; set; }
        public int CourseSectionId { get; set; }
        public int? GradeId { get; set; }
        public int? GradeScaleId { get; set; }
        public decimal? AcademicYear { get; set; }
        public int? CalendarId { get; set; }
        public int? YrMarkingPeriodId { get; set; }
        public int? SmstrMarkingPeriodId { get; set; }
        public int? QtrMarkingPeriodId { get; set; }
        public bool? IsPercent { get; set; }
        public decimal? PercentMarks { get; set; }
        public string GradeObtained { get; set; }
        public string TeacherComment { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual Quarters Quarters { get; set; }
        public virtual SchoolYears SchoolYears { get; set; }
        public virtual Semesters Semesters { get; set; }
        public virtual StudentMaster StudentMaster { get; set; }
        public virtual ICollection<StudentFinalGradeComments> StudentFinalGradeComments { get; set; }
        public virtual ICollection<StudentFinalGradeStandard> StudentFinalGradeStandard { get; set; }
    
}
}
