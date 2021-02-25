using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.CommonModel
{
    public class DashboardViewModel : CommonFields
    {
        public DashboardViewModel()
        {
            calendarEventList = new List<opensis.data.Models.CalendarEvents>();
        }

        public Guid? TenantId { get; set; }
        public int? SchoolId { get; set; }
        public string SuperAdministratorName { get; set; }
        public string SchoolName { get; set; }
        public decimal? AcademicYear { get; set; }
        public int? TotalStudent { get; set; }
        public int? TotalStaff { get; set; }
        public int? TotalParent { get; set; }
        public string NoticeTitle { get; set; }
        public string NoticeBody { get; set; }
        public SchoolCalendars schoolCalendar { get; set; }
        public List<opensis.data.Models.CalendarEvents> calendarEventList { get; set; }
    }
}
