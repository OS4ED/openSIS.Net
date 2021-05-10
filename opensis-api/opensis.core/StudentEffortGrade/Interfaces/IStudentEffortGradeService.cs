using opensis.data.ViewModels.StudentEffortGrade;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.StudentEffortGrade.Interfaces
{
    public interface IStudentEffortGradeService
    {
        public StudentEffortGradeListModel AddUpdateStudentEffortGrade(StudentEffortGradeListModel studentEffortGradeListModel);
        public StudentEffortGradeListModel GetAllStudentEffortGradeList(StudentEffortGradeListModel studentEffortGradeListModel);
    }
}
