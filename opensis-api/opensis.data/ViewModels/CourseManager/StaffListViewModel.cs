using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.CourseManager
{
    public class StaffListViewModel:CommonFields
    {
        public StaffListViewModel()
        {
            staffMasterList = new List<StaffMaster>();
        }
        public List<StaffMaster> staffMasterList { get; set; }
        public Guid? TenantId { get; set; }
        public int? SchoolId { get; set; }
        public int? CourseId { get; set; }
        public int? CourseSectionId { get; set; }
    }
}
