using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.CourseManager
{
    public class CourseStandardForCourseViewModel : CommonFields
    {
        public CourseStandardForCourseViewModel()
        {
            getCourseStandardForCourses = new List<GetCourseStandardForCourse>();
        }
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int CourseId { get; set; }
        public List<GetCourseStandardForCourse> getCourseStandardForCourses { get; set; }
    }
}
