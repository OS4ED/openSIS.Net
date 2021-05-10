using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.InputFinalGrade
{
    public class StudentFinalGradeListModel : CommonFields
    {
        public List<StudentFinalGrade> studentFinalGradeList { get; set; }
        //public List<CourseStandard> courseStandardList { get; set; }
        //public List<StudentFinalGradeStandard> studentFinalGradeStandardList { get; set; }
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int? CalendarId { get; set; }
        public int CourseSectionId { get; set; }
        public int? StandardGradeScaleId { get; set; }
        public int CourseId { get; set; }
        public bool? IsPercent { get; set; }
        public string MarkingPeriodId { get; set; }
        public decimal? AcademicYear { get; set; }
        public string CreatedOrUpdatedBy { get; set; }
    }
}
