using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.StudentSchedule
{
    public class ScheduledStudentDropModel : CommonFields
    {
        public List<StudentCoursesectionSchedule> studentCoursesectionScheduleList { get; set; }        
        public int CourseSectionId { get; set; }
        public DateTime? EffectiveDropDate { get; set; }
    }
}
