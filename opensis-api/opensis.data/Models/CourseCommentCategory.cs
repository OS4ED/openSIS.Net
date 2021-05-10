using System;
using System.Collections.Generic;

namespace opensis.data.Models
{
    public partial class CourseCommentCategory
    {
        public CourseCommentCategory()
        {
            StudentFinalGradeComments = new HashSet<StudentFinalGradeComments>();
        }

        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int CourseCommentId { get; set; }
        public int? CourseId { get; set; }
        public string CourseName { get; set; }
        public bool ApplicableAllCourses { get; set; }
        public string Comments { get; set; }
        public int? SortOrder { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<StudentFinalGradeComments> StudentFinalGradeComments { get; set; }
    }
}
