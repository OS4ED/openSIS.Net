using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.CourseManager
{
    public class DeleteScheduleViewModel : CommonFields
    {
        public Guid? TenantId { get; set; }
        public int? SchoolId { get; set; }
        public int? CourseId { get; set; }
        public int? CourseSectionId { get; set; }
        public string ScheduleType { get; set; }
        public int? Serial { get; set; }
    }
}
