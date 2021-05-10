using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using opensis.core.StudentEffortGrade.Interfaces;
using opensis.data.ViewModels.StudentEffortGrade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace opensisAPI.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("{tenant}/StudentEffortGrade")]
    [ApiController]
    public class StudentEffortGradeController : ControllerBase
    {
        private IStudentEffortGradeService _studentEffortGradeService;
        public StudentEffortGradeController(IStudentEffortGradeService studentEffortGradeService)
        {
            _studentEffortGradeService = studentEffortGradeService;
        }

        [HttpPost("addUpdateStudentEffortGrade")]
        public ActionResult<StudentEffortGradeListModel> AddUpdateStudentEffortGrade(StudentEffortGradeListModel studentEffortGradeListModel)
        {
            StudentEffortGradeListModel studentEffortGradeAdd = new StudentEffortGradeListModel();
            try
            {
                studentEffortGradeAdd = _studentEffortGradeService.AddUpdateStudentEffortGrade(studentEffortGradeListModel);
            }
            catch (Exception ex)
            {
                studentEffortGradeAdd._message = ex.Message;
                studentEffortGradeAdd._failure = true;
            }
            return studentEffortGradeAdd;
        }

        [HttpPost("getAllStudentEffortGradeList")]
        public ActionResult<StudentEffortGradeListModel> GetAllStudentEffortGradeList(StudentEffortGradeListModel studentEffortGradeListModel)
        {
            StudentEffortGradeListModel studentEffortGradeList = new StudentEffortGradeListModel();
            try
            {
                studentEffortGradeList = _studentEffortGradeService.GetAllStudentEffortGradeList(studentEffortGradeListModel);
            }
            catch (Exception ex)
            {

                studentEffortGradeList._message = ex.Message;
                studentEffortGradeList._failure = true;
            }
            return studentEffortGradeList;
        }
    }
}
