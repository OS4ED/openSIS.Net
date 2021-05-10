using System;
using System.Collections.Generic;

namespace opensis.data.Models
{
    public partial class StudentEffortGradeDetail
    {
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int StudentId { get; set; }
        public long StudentEffortGradeSrlno { get; set; }
        public long Id { get; set; }
        public int? EffortCategoryId { get; set; }
        public int? EffortItemId { get; set; }
        public int? EffortGradeScaleId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual StudentEffortGradeMaster StudentEffortGradeMaster { get; set; }
    }
}
