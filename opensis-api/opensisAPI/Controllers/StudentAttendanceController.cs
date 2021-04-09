using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using opensis.core.StudentAttendances.Interfaces;
using opensis.data.ViewModels.StudentAttendances;

namespace opensisAPI.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("{tenant}/StudentAttendance")]
    [ApiController]
    public class StudentAttendanceController : ControllerBase
    {
        private IStudentAttendanceService _studentAttendanceService;
        public StudentAttendanceController(IStudentAttendanceService studentAttendanceService)
        {
            _studentAttendanceService = studentAttendanceService;
        }

        [HttpPost("addUpdateStudentAttendance")]
        public ActionResult<StudentAttendanceAddViewModel> AddUpdateStudentAttendance(StudentAttendanceAddViewModel studentAttendanceAddViewModel)
        { 
            StudentAttendanceAddViewModel studentAttendanceAdd = new StudentAttendanceAddViewModel();
            try
            {
                studentAttendanceAdd = _studentAttendanceService.SaveStudentAttendance(studentAttendanceAddViewModel);
            }
            catch (Exception ex)
            {

                studentAttendanceAdd._message = ex.Message;
                studentAttendanceAdd._failure = true;
            }

            return studentAttendanceAdd;
        }
    }
}
