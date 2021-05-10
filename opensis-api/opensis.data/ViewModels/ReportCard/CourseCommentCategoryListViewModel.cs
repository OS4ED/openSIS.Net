using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.ReportCard
{
    public class CourseCommentCategoryListViewModel : CommonFields
    {
        public CourseCommentCategoryListViewModel()
        {
            courseCommentCategories = new List<CourseCommentCategory>();
        }

        public List<CourseCommentCategory> courseCommentCategories { get; set; }
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
    }
}
