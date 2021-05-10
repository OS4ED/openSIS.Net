using System;
using System.Collections.Generic;

namespace opensis.data.Models
{
    public partial class StudentFinalGradeComments
    {
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int StudentId { get; set; }
        public long StudentFinalGradeSrlno { get; set; }
        public int CourseCommentId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual CourseCommentCategory CourseCommentCategory { get; set; }
        public virtual StudentFinalGrade StudentFinalGrade { get; set; }
       
    }
}
