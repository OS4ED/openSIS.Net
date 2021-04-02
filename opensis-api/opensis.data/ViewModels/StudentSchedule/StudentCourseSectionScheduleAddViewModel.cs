using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.StudentSchedule
{
    public class StudentCourseSectionScheduleAddViewModel : CommonFields
    {
        public List<CourseSection> courseSectionList { get; set; }
        public List<StudentMaster> studentMasterList { get; set; }
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string ConflictMessage { get; set; }
    }
}
