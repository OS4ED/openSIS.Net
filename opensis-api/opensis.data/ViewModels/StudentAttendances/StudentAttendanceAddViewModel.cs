using System;
using System.Collections.Generic;
using System.Text;
using opensis.data.Models;


namespace opensis.data.ViewModels.StudentAttendances
{

    public class StudentAttendanceAddViewModel : CommonFields
    {
        public List<StudentAttendance> studentAttendance { get; set; }
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int CourseSectionId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public int PeriodId { get; set; }
        public int StaffId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public int CourseId { get; set; }
    }
}
