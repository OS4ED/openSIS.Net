using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.Student
{
    public class ReportCardDetailsForTranscriptViewModel
    {
        public string CourseSectionName { get; set; }
        public string CourseCode { get; set; }
        public decimal? CreditHours { get; set; }
        public decimal? CreditEarned { get; set; }
        public string Grade { get; set; }
        public decimal? GPValue { get; set; }
    }
}
