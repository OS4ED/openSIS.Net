using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.CourseManager
{
    public class SearchCourseSectionViewModel : CommonFields
    {
        public List<AllCourseSectionView> allCourseSectionViewList { get; set; }
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int? CourseId { get; set; }
        public string CourseSubject { get; set; }
        public string CourseProgram { get; set; }
        public string MarkingPeriodId { get; set; }
        public bool? ForStaff { get; set; }
        public bool? ForStudent { get; set; }
    }
}
