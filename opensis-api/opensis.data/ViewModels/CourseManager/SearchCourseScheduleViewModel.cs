using System;
using System.Collections.Generic;
using System.Text;
using opensis.data.Models;

namespace opensis.data.ViewModels.CourseManager
{
    public class SearchCourseScheduleViewModel : CommonFields
    {
        public List<Course> course { get; set; }
        public Guid ? TenantId { get; set; }
        public int ? SchoolId { get; set; }
        public string CourseSubject { get; set; }
        public string CourseProgram { get; set; }




    }
}
