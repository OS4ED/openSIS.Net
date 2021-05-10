using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.ReportCard
{
    public class CourseCommentCategoryDeleteViewModel : CommonFields
    {
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int ? CourseId { get; set; }
        public int? CourseCommentId { get; set; }
    }
}
