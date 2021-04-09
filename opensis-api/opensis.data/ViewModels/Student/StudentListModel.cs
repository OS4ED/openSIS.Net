using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.Student
{
    public class StudentListModel : CommonFields
    {
        public StudentListModel()
        {
            getStudentListForViews = new List<GetStudentListForView>();
            studentMaster = new List<StudentMaster>();
        }
        public List<GetStudentListForView> getStudentListForViews { get; set; }
        public List<StudentMaster> studentMaster { get; set; }
        public Guid? TenantId { get; set; }
        public int? SchoolId { get; set; }
        public int? StudentId { get; set; }
        public int? EnrollmentCode { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public int? GradeId { get; set; }
        public string GradeLevelTitle { get; set; }
        public string AcademicYear { get; set; }
        public string UpdatedBy { get; set; }
        public int? TotalCount { get; set; }
        public int? PageNumber { get; set; }
        public int? _pageSize { get; set; }

    }
}
