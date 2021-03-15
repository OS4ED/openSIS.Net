using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.GradeLevel
{
    public class GradeLevelView
    {
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int GradeId { get; set; }
        public string ShortName { get; set; }
        public string Title { get; set; }
        public string NextGrade { get; set; }
        //public string GradeDescription { get; set; }
        public int? EquivalencyId { get; set; }
        public int? AgeRangeId { get; set; }
        public int? IscedCode { get; set; }
        public string GradeLevelEquivalency { get; set; }
        public int? NextGradeId { get; set; }
        public int? SortOrder { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string UpdatedBy { get; set; }
    }
}
