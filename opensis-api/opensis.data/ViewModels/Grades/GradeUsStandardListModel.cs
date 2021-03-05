using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.Grades
{
    public class GradeUsStandardListModel : CommonFields
    {
        public GradeUsStandardListModel()
        {
            gradeUsStandardList = new List<GradeUsStandard>();
        }
        public List<GradeUsStandard> gradeUsStandardList { get; set; }
        public Guid? TenantId { get; set; }
        public int? SchoolId { get; set; }
        public int? TotalCount { get; set; }
        public int? PageNumber { get; set; }
        public int? _pageSize { get; set; }
    }
}
