using System;
using System.Collections.Generic;

namespace opensis.data.Models
{
    public partial class Gradelevels
    {
        public Gradelevels()
        {
            StudentEnrollment = new HashSet<StudentEnrollment>();
        }

        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int GradeId { get; set; }
        public string ShortName { get; set; }
        public string Title { get; set; }
        public int? EquivalencyId { get; set; }
        public int? AgeRangeId { get; set; }
        public int? IscedCode { get; set; }
        public int? NextGradeId { get; set; }
        public int? SortOrder { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string UpdatedBy { get; set; }

        public virtual GradeAgeRange AgeRange { get; set; }
        public virtual GradeEquivalency Equivalency { get; set; }
        public virtual GradeEducationalStage IscedCodeNavigation { get; set; }
        public virtual SchoolMaster SchoolMaster { get; set; }
        public virtual ICollection<StudentEnrollment> StudentEnrollment { get; set; }

    }
}
