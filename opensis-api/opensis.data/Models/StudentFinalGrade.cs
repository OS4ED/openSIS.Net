using System;
using System.Collections.Generic;

namespace opensis.data.Models
{
    public partial class StudentFinalGrade
    {
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int StudentId { get; set; }
        public Guid StudentGuid { get; set; }
        public string AlternateId { get; set; }
        public string StudentInternalId { get; set; }
        public int CourseId { get; set; }
        public int CourseSectionId { get; set; }
        public int? GradeId { get; set; }
        public int? GradeScaleId { get; set; }
        public decimal AcademicYear { get; set; }
        public int? CalendarId { get; set; }
        public int? StandardGradeScaleId { get; set; }
        public int? YrMarkingPeriodId { get; set; }
        public int? SmstrMarkingPeriodId { get; set; }
        public int? QtrMarkingPeriodId { get; set; }
        public bool? IsPercent { get; set; }
        public int? PercentMarks { get; set; }
        public string GradeObtained { get; set; }
        public string CourseCommentId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual CourseSection CourseSection { get; set; }
        public virtual Quarters Quarters { get; set; }
        public virtual ReportCardComments ReportCardComments { get; set; }
        public virtual SchoolYears SchoolYears { get; set; }
        public virtual Semesters Semesters { get; set; }
        public virtual StudentMaster StudentMaster { get; set; }
    }
}
