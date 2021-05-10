using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.Student
{
    public class GradeDetailsForTranscriptViewModel
    {
        public GradeDetailsForTranscriptViewModel()
        {
            reportCardDetailsForTranscriptViewModel = new List<ReportCardDetailsForTranscriptViewModel>();
        }
        public List<ReportCardDetailsForTranscriptViewModel> reportCardDetailsForTranscriptViewModel { get; set; }
        public int? GradeId { get; set; }
        public string GradeTitle { get; set; }
        public string SchoolName { get; set; }
        public string SchoolYear { get; set; }
        public decimal? CreditAttemped { get; set; }
        public decimal? CreditEarned { get; set; }
        public decimal? GPA { get; set; }
    }
}
