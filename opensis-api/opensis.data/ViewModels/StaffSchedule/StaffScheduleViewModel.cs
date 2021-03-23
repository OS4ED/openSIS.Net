using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.StaffSchedule
{
    public class StaffScheduleViewModel : CommonFields
    {
        public StaffScheduleViewModel()
        {
            staffScheduleViewList = new List<StaffScheduleViewList>();
            courseSectionViewList = new List<CourseSectionViewList>();
        }
        public List<StaffScheduleViewList> staffScheduleViewList{ get; set; }
        public List<CourseSectionViewList> courseSectionViewList { get; set; }
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public string CreatedBy { get; set; }
    }
}
