using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.ReportCard
{
    public class ReportCardViewModel : CommonFields
    {
        public ReportCardViewModel()
        {
            studentsReportCardViewModelList = new List<StudentsReportCardViewModel>();
            courseCommentCategories = new List<CourseCommentCategory>();
            teacherCommentList = new List<string>();
        }
        public Guid? TenantId { get; set; }
        public int? SchoolId { get; set; }
        public decimal? AcademicYear { get; set; }
        public string MarkingPeriods { get; set; }
        public bool? TeacherName { get; set; }
        public bool? TeacherComments { get; set; }
        public bool? Parcentage { get; set; }
        public bool? GPA { get; set; }
        public bool? YearToDateDailyAbsences { get; set; }
        public bool? DailyAbsencesThisMarkingPeriod { get; set; }
        public bool? OtherAttendanceCodeYearToDate { get; set; }

        public List<StudentsReportCardViewModel> studentsReportCardViewModelList { get; set; }
        public string SchoolName { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public List<CourseCommentCategory> courseCommentCategories { get; set; }
        public List<String> teacherCommentList { get; set; }

    }
}
