using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.CourseManager
{
    public class CourseStandardForCourseViewModel : CommonFields
    {
        public CourseStandardForCourseViewModel()
        {
            courseStandards = new List<CourseStandard>();
        }
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int CourseId { get; set; }
        public List<CourseStandard> courseStandards { get; set; }
    }
}
