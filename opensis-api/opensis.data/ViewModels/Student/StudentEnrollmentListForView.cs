using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.Student
{
    public class StudentEnrollmentListForView 
    {
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int StudentId { get; set; }
        public int EnrollmentId { get; set; }
        public int? CalenderId { get; set; }
        public decimal? AcademicYear { get; set; }
        public string RollingOption { get; set; }
        public string SchoolName { get; set; }
        public string GradeLevelTitle { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public string EnrollmentCode { get; set; }
        public DateTime? ExitDate { get; set; }
        public string ExitCode { get; set; }
        public int? TransferredSchoolId { get; set; }
        public string SchoolTransferred { get; set; }
        public string TransferredGrade { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public Guid? StudentGuid { get; set; }
        public string EnrollmentType { get; set; }
        public string ExitType { get; set; }
        public string Type { get; set; }
        public int? GradeId { get; set; }
    }
}
