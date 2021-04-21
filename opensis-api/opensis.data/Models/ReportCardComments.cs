using System;
using System.Collections.Generic;

namespace opensis.data.Models
{
    public partial class ReportCardComments
    {
        public ReportCardComments()
        {
            StudentFinalGrade = new HashSet<StudentFinalGrade>();
        }

        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public string CourseCommentId { get; set; }
        public bool? ApplicableAllCourses { get; set; }
        public int CourseId { get; set; }
        public int CourseSectionId { get; set; }
        public string Comments { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<StudentFinalGrade> StudentFinalGrade { get; set; }
    }
}
