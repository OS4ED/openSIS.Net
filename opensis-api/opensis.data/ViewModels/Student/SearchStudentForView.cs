using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.Student
{
    public class SearchStudentForView
    {
        public Guid? TenantId { get; set; }
        public int? SchoolId { get; set; }
        public string StudentName { get; set; }
        public int? StudentId { get; set; }
        public string AlternateId { get; set; }
        public int? GradeId { get; set; }
        public int? SectionId { get; set; }
        public int? FirstLanguageId { get; set; }
    }
}
