using System;
using System.Collections.Generic;

namespace opensis.data.Models
{
    public partial class StudentFinalGradeStandard
    {
       

        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int StudentId { get; set; }
        public long StudentFinalGradeSrlno { get; set; }
        public int Id { get; set; }
        public decimal? AcademicYear { get; set; }
        public int? CalendarId { get; set; }
        public int? StandardGradeScaleId { get; set; }
        public int? YrMarkingPeriodId { get; set; }
        public int? SmstrMarkingPeriodId { get; set; }
        public int? QtrMarkingPeriodId { get; set; }
        public int? GradeObtained { get; set; }
        public string TeacherComment { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual StudentFinalGrade StudentFinalGrade { get; set; }
    }
}
