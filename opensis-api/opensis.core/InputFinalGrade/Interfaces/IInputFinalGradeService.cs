using opensis.data.ViewModels.InputFinalGrade;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.InputFinalGrade.Interfaces
{
    public interface IInputFinalGradeService
    {
        public StudentFinalGradeListModel AddUpdateStudentFinalGrade(StudentFinalGradeListModel studentFinalGradeListModel);
        public StudentFinalGradeListModel GetAllStudentFinalGradeList(StudentFinalGradeListModel studentFinalGradeListModel);
        //public ReportCardCommentListViewModel GetReportCardCommentsForInputFinalGrade(ReportCardCommentListViewModel reportCardCommentListViewModel);
    }
}
