using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.StudentSchedule
{
    public class ScheduleStudentListViewModel : CommonFields
    {
        public List<ScheduleStudentForView> scheduleStudentForView { get; set; }
        public Guid? TenantId { get; set; }
        public int? SchoolId { get; set; }
        public int? CourseSectionId { get; set; }
        public int? TotalCount { get; set; }
        public int? PageNumber { get; set; }
        public int? _pageSize { get; set; }
    }
}
