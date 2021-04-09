using opensis.data.ViewModels.StudentAttendances;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.StudentAttendances.Interfaces
{
    public interface IStudentAttendanceService
    {
        public StudentAttendanceAddViewModel SaveStudentAttendance(StudentAttendanceAddViewModel studentAttendanceAddViewModel);
    }
}
