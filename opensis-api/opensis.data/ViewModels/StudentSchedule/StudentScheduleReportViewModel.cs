using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace opensis.data.ViewModels.StudentSchedule
{
    public class StudentScheduleReportViewModel : CommonFields
    {
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public DataTable ScheduleReport { get; set; }
    }
}
