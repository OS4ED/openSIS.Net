using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.StudentEffortGrade
{
    public class StudentEffortGradeListModel : CommonFields
    {
        public List<StudentEffortGradeMaster> studentEffortGradeList { get; set; }
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int CourseId { get; set; }
        public int CourseSectionId { get; set; }
        public int? CalendarId { get; set; }
        public string MarkingPeriodId { get; set; }
        public decimal? AcademicYear { get; set; }
        public string CreatedOrUpdatedBy { get; set; }
    }
}
