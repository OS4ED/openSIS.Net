using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using opensis.core.InputFinalGrade.Interfaces;
using opensis.data.ViewModels.InputFinalGrade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace opensisAPI.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("{tenant}/InputFinalGrade")]
    [ApiController]
    public class InputFinalGradeController : ControllerBase
    {
        private IInputFinalGradeService _inputFinalGradeService;
        public InputFinalGradeController(IInputFinalGradeService inputFinalGradeService)
        {
            _inputFinalGradeService = inputFinalGradeService;
        }

        /// <summary>
        /// Add Update Student Final Grade
        /// </summary>
        /// <param name="studentFinalGradeListModel"></param>
        /// <returns></returns>
        [HttpPost("addUpdateStudentFinalGrade")]
        public ActionResult<StudentFinalGradeListModel> AddUpdateStudentFinalGrade(StudentFinalGradeListModel studentFinalGradeListModel)
        {
            StudentFinalGradeListModel studentFinalGradeAdd = new StudentFinalGradeListModel();
            try
            {
                studentFinalGradeAdd = _inputFinalGradeService.AddUpdateStudentFinalGrade(studentFinalGradeListModel);
            }
            catch (Exception ex)
            {

                studentFinalGradeAdd._message = ex.Message;
                studentFinalGradeAdd._failure = true;
            }
            return studentFinalGradeAdd;
        }

        /// <summary>
        /// Get All Student Final Grade List
        /// </summary>
        /// <param name="studentFinalGradeListModel"></param>
        /// <returns></returns>
        [HttpPost("getAllStudentFinalGradeList")]
        public ActionResult<StudentFinalGradeListModel> GetAllStudentFinalGradeList(StudentFinalGradeListModel studentFinalGradeListModel)
        {
            StudentFinalGradeListModel studentFinalGradeList = new StudentFinalGradeListModel();
            try
            {
                studentFinalGradeList = _inputFinalGradeService.GetAllStudentFinalGradeList(studentFinalGradeListModel);
            }
            catch (Exception ex)
            {

                studentFinalGradeList._message = ex.Message;
                studentFinalGradeList._failure = true;
            }
            return studentFinalGradeList;
        }

        //[HttpPost("getReportCardCommentsForInputFinalGrade")]
        //public ActionResult<ReportCardCommentListViewModel> GetReportCardCommentsForInputFinalGrade(ReportCardCommentListViewModel reportCardCommentListViewModel)
        //{
        //    ReportCardCommentListViewModel reportCardCommentList = new ReportCardCommentListViewModel();
        //    try
        //    {
        //        reportCardCommentList = _inputFinalGradeService.GetReportCardCommentsForInputFinalGrade(reportCardCommentListViewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        reportCardCommentList._message = ex.Message;
        //        reportCardCommentList._failure = true;
        //    }
        //    return reportCardCommentList;
        //}
    }
}
