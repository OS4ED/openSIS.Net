using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.ReportCard
{
    public class ReportCardMarkingPeriodsDetails
    {
        public ReportCardMarkingPeriodsDetails()
        {
            reportCardDetails = new List<ReportCardDetails>();
        }
        public List<ReportCardDetails> reportCardDetails { get; set; }
        public int? YrMarkingPeriodId { get; set; }
        public int? SmstrMarkingPeriodId { get; set; }
        public int? QtrMarkingPeriodId { get; set; }
        public string MarkingPeriod { get; set; }
        public int? Absences { get; set; }
        public int? ExcusedAbsences { get; set; }
    }
}
