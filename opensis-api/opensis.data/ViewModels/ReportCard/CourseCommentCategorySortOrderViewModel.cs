using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.ReportCard
{
    public class CourseCommentCategorySortOrderViewModel : CommonFields
    {
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int? CourseId { get; set; }
        public int? PreviousSortOrder { get; set; }
        public int? CurrentSortOrder { get; set; }
    }
}
