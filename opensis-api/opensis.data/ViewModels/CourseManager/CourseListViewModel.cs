using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.CourseManager
{
    public class CourseListViewModel : CommonFields
    {
        public CourseListViewModel()
        {
            courseViewModelList = new List<CourseViewModel>(); 
        }
        public List<CourseViewModel> courseViewModelList { get; set; }
        public int? CourseCount { get; set; }
        public Guid? TenantId { get; set; }
        public int? SchoolId { get; set; }
    }
}
