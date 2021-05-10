using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.StudentSchedule
{
    public class ScheduledStudentDropModel : CommonFields
    {
        public List<StudentCoursesectionSchedule> studentCoursesectionScheduleList { get; set; }
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int CourseSectionId { get; set; }
        public int StudentId { get; set; }
        public DateTime? EffectiveDropDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
