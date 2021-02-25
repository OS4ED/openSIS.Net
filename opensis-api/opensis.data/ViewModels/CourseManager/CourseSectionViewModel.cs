using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.CourseManager
{
    public class CourseSectionViewModel : CommonFields
    {
        public CourseSectionViewModel()
        {
            getCourseSectionForView = new List<GetCourseSectionForView>();
        }
        public List<GetCourseSectionForView> getCourseSectionForView { get; set; }
        public Guid? TenantId { get; set; }
        public int? SchoolId { get; set; }
        public int? CourseId { get; set; }
        public decimal? AcademicYear { get; set; }
    }
}
