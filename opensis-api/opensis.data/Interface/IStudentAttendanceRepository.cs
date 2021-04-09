using opensis.data.ViewModels.StudentAttendances;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.Interface
{
    public interface IStudentAttendanceRepository
    {
        public StudentAttendanceAddViewModel AddUpdateStudentAttendance(StudentAttendanceAddViewModel studentAttendanceAddViewModel);
    }
}
