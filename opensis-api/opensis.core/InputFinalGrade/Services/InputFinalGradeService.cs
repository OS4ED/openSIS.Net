using opensis.core.helper;
using opensis.core.InputFinalGrade.Interfaces;
using opensis.data.Interface;
using opensis.data.ViewModels.InputFinalGrade;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.InputFinalGrade.Services
{
    public class InputFinalGradeService : IInputFinalGradeService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";

        public IInputFinalGradeRepository inputFinalGradeRepository;
        public InputFinalGradeService(IInputFinalGradeRepository inputFinalGradeRepository)
        {
            this.inputFinalGradeRepository = inputFinalGradeRepository;
        }

        /// <summary>
        /// Add Update Student Final Grade
        /// </summary>
        /// <param name="studentFinalGradeListModel"></param>
        /// <returns></returns>
        public StudentFinalGradeListModel AddUpdateStudentFinalGrade(StudentFinalGradeListModel studentFinalGradeListModel)
        {
            StudentFinalGradeListModel studentFinalGradeList = new StudentFinalGradeListModel();
            try
            {
                if (TokenManager.CheckToken(studentFinalGradeListModel._tenantName + studentFinalGradeListModel._userName, studentFinalGradeListModel._token))
                {
                    studentFinalGradeList = this.inputFinalGradeRepository.AddUpdateStudentFinalGrade(studentFinalGradeListModel);
                }
                else
                {
                    studentFinalGradeList._failure = true;
                    studentFinalGradeList._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                studentFinalGradeList._failure = true;
                studentFinalGradeList._message = es.Message;
            }
            return studentFinalGradeList;
        }

        /// <summary>
        /// Get All Student Final Grade List
        /// </summary>
        /// <param name="studentFinalGradeListModel"></param>
        /// <returns></returns>
        public StudentFinalGradeListModel GetAllStudentFinalGradeList(StudentFinalGradeListModel studentFinalGradeListModel)
        {
            StudentFinalGradeListModel studentFinalGradeList = new StudentFinalGradeListModel();
            try
            {
                if (TokenManager.CheckToken(studentFinalGradeListModel._tenantName + studentFinalGradeListModel._userName, studentFinalGradeListModel._token))
                {
                    studentFinalGradeList = this.inputFinalGradeRepository.GetAllStudentFinalGradeList(studentFinalGradeListModel);
                }
                else
                {
                    studentFinalGradeList._failure = true;
                    studentFinalGradeList._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                studentFinalGradeList._failure = true;
                studentFinalGradeList._message = es.Message;
            }
            return studentFinalGradeList;
        }

        //public ReportCardCommentListViewModel GetReportCardCommentsForInputFinalGrade(ReportCardCommentListViewModel reportCardCommentListViewModel)
        //{
        //    ReportCardCommentListViewModel reportCardCommentListView = new ReportCardCommentListViewModel();
        //    try
        //    {
        //        if (TokenManager.CheckToken(reportCardCommentListViewModel._tenantName + reportCardCommentListViewModel._userName, reportCardCommentListViewModel._token))
        //        {
        //            reportCardCommentListView = this.inputFinalGradeRepository.GetReportCardCommentsForInputFinalGrade(reportCardCommentListViewModel);
        //        }
        //        else
        //        {
        //            reportCardCommentListView._failure = true;
        //            reportCardCommentListView._message = TOKENINVALID;
        //        }
        //    }
        //    catch (Exception es)
        //    {
        //        reportCardCommentListView._failure = true;
        //        reportCardCommentListView._message = es.Message;
        //    }
        //    return reportCardCommentListView;
        //}
    }
}
