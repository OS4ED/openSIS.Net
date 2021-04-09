using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.CourseManager
{
    public class StaffListViewModel : CommonFields
    {
        public StaffListViewModel()
        {
            courseSectionsList = new List<CourseSection>();
        }

        public List<CourseSection> courseSectionsList { get; set; }

        public Guid? TenantId { get; set; }
        public int? SchoolId { get; set; }
        public int? CourseId { get; set; }
        public int? CourseSectionId { get; set; }
        public int? ReScheduleStaffId { get; set; }
        public string CreatedBy { get; set; }
        public string ConflictIndexNo { get; set; }

    }
}
