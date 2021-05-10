using opensis.data.ViewModels.StudentEffortGrade;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.Interface
{
    public interface IStudentEffortGradeRepository
    {
        public StudentEffortGradeListModel AddUpdateStudentEffortGrade(StudentEffortGradeListModel studentEffortGradeListModel);
        public StudentEffortGradeListModel GetAllStudentEffortGradeList(StudentEffortGradeListModel studentEffortGradeListModel);
    }
}
