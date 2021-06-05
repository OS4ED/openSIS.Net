using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.StudentAttendances
{
    public class StudentAttendanceListViewModel : CommonFields
    {
        //public List<StudentAttendance> studentAttendanceList { get; set; }
        //public List<StudentMaster> studentMasters { get; set; }
        public List<StudendAttendanceAdministrationViewModel> studendAttendanceAdministrationList { get; set; }

        public Guid TenantId { get; set; }
        public int? SchoolId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public int AttendanceCode { get; set; }
        public int? TotalCount { get; set; }
        public int? PageNumber { get; set; }
        public int? _pageSize { get; set; }
    }
}
