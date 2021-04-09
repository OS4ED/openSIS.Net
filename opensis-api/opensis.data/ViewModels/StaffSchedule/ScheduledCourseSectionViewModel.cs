using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.StaffSchedule
{
    public class ScheduledCourseSectionViewModel : CommonFields
    {
        public ScheduledCourseSectionViewModel()
        {
            courseSectionViewList = new List<CourseSectionViewList>();
        }
        public List<CourseSectionViewList> courseSectionViewList { get; set; }
        public Guid? TenantId { get; set; }
        public int? SchoolId { get; set; }
        public int? StaffId { get; set; }
    }
}
