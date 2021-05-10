using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.StudentSchedule
{
    public class Student360ScheduleCourseSectionListViewModel : CommonFields
    {

        public Student360ScheduleCourseSectionListViewModel()
        {
            scheduleCourseSectionForView = new List<Student360ScheduleCourseSectionForView>();
        }
        public List<Student360ScheduleCourseSectionForView> scheduleCourseSectionForView { get; set; }
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int? StudentId { get; set; }
        public bool? IsDropped { get; set; }
    }
}
