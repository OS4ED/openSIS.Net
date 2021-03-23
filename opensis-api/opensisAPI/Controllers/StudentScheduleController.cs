using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using opensis.core.StudentSchedule.Interfaces;
using opensis.data.ViewModels.StudentSchedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace opensisAPI.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("{tenant}/StudentSchedule")]
    [ApiController]
    public class StudentScheduleController : ControllerBase
    {
        private IStudentScheduleService _studentScheduleService;
        public StudentScheduleController(IStudentScheduleService studentScheduleService)
        {
            _studentScheduleService = studentScheduleService;
        }

        [HttpPost("addStudentCourseSectionSchedule")]
        public ActionResult<StudentCourseSectionScheduleAddViewModel> AddStudentCourseSectionSchedule(StudentCourseSectionScheduleAddViewModel studentCourseSectionScheduleAddViewModel)
        {
            StudentCourseSectionScheduleAddViewModel StudentCourseSectionScheduleAddModel = new StudentCourseSectionScheduleAddViewModel();
            try
            {
                StudentCourseSectionScheduleAddModel = _studentScheduleService.AddStudentCourseSectionSchedule(studentCourseSectionScheduleAddViewModel);

            }
            catch (Exception es)
            {
                StudentCourseSectionScheduleAddModel._failure = true;
                StudentCourseSectionScheduleAddModel._message = es.Message;
            }
            return StudentCourseSectionScheduleAddModel;
        }

        [HttpPost("searchScheduledStudentForGroupDrop")]
        public ActionResult<ScheduleStudentListViewModel> SearchScheduledStudentForGroupDrop(ScheduleStudentListViewModel scheduleStudentListViewModel)
        {
            ScheduleStudentListViewModel ScheduledStudentListView = new ScheduleStudentListViewModel();
            try
            {
                ScheduledStudentListView = _studentScheduleService.SearchScheduledStudentForGroupDrop(scheduleStudentListViewModel);
            }
            catch (Exception es)
            {
                ScheduledStudentListView._failure = true;
                ScheduledStudentListView._message = es.Message;
            }
            return ScheduledStudentListView;
        }

        [HttpPut("groupDropForScheduledStudent")]
        public ActionResult<ScheduledStudentDropModel> GroupDropForScheduledStudent(ScheduledStudentDropModel scheduledStudentDropModel)
        {
            ScheduledStudentDropModel ScheduledStudentDrop = new ScheduledStudentDropModel();
            try
            {
                ScheduledStudentDrop = _studentScheduleService.GroupDropForScheduledStudent(scheduledStudentDropModel);
            }
            catch (Exception es)
            {
                ScheduledStudentDrop._failure = true;
                ScheduledStudentDrop._message = es.Message;
            }
            return ScheduledStudentDrop;
        }

        [HttpPost("studentScheduleReport")]
        public ActionResult<StudentScheduleReportViewModel> StudentScheduleReport(StudentScheduleReportViewModel studentScheduleReportViewModel)
        {
            StudentScheduleReportViewModel studentScheduleReportView = new StudentScheduleReportViewModel();
            try
            {
                studentScheduleReportView = _studentScheduleService.StudentScheduleReport(studentScheduleReportViewModel);
            }
            catch (Exception es)
            {
                studentScheduleReportView._failure = true;
                studentScheduleReportView._message = es.Message;
            }
            return studentScheduleReportView;
        }
    }
}
