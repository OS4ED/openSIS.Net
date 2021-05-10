using opensis.data.ViewModels.InputFinalGrade;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.Interface
{
    public interface IInputFinalGradeRepository
    {
        public StudentFinalGradeListModel AddUpdateStudentFinalGrade(StudentFinalGradeListModel studentFinalGradeListModel);
        public StudentFinalGradeListModel GetAllStudentFinalGradeList(StudentFinalGradeListModel studentFinalGradeListModel);
        //public ReportCardCommentListViewModel GetReportCardCommentsForInputFinalGrade(ReportCardCommentListViewModel reportCardCommentListViewModel);
    }
}
