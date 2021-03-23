using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.StaffSchedule
{
    public class StaffScheduleViewList
    {
        public StaffScheduleViewList()
        {
            courseSectionViewList = new List<CourseSectionViewList>();
        }
        public List<CourseSectionViewList> courseSectionViewList { get; set; }
        public int StaffId { get; set; }
        public string StaffFullName { get; set; }
        public string StaffEmail { get; set; }
        public bool? HomeroomTeacher { get; set; }
        public Guid? StaffGuid { get; set; }
    }
}
