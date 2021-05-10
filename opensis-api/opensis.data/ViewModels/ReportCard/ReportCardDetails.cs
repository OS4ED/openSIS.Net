using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.ReportCard
{
    public class ReportCardDetails
    {
        public string CourseName { get; set; }
        public string CourseSectionName { get; set; }
        public string StaffName { get; set; }
        public decimal? PercentMarks { get; set; }
        public string GradeObtained { get; set; }
        public string GPA { get; set; }
        public string Comments { get; set; }
        public string TeacherComments { get; set; }
       
    }
}
