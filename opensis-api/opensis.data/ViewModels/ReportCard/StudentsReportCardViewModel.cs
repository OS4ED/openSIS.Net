using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.ReportCard
{
    public class StudentsReportCardViewModel
    {
        public StudentsReportCardViewModel()
        {
            reportCardMarkingPeriodsDetails = new List<ReportCardMarkingPeriodsDetails>();
        }
        public List<ReportCardMarkingPeriodsDetails> reportCardMarkingPeriodsDetails { get; set; }
        public string FirstGivenName { get; set; }
        public string MiddleName { get; set; }
        public string LastFamilyName { get; set; }
        public int? StudentId { get; set; }
        public Guid? StudentGuid { get; set; }
        public string AlternateId { get; set; }
        public string StudentInternalId { get; set; }
        public string GradeTitle { get; set; }
        public string YearToDateAttendencePercent { get; set; }
        public int? YearToDateAbsencesInDays { get; set; }

    }
}
