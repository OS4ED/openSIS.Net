using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using opensis.core.StaffSchedule.Interfaces;
using opensis.data.ViewModels.StaffSchedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace opensisAPI.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("{tenant}/StaffSchedule")]
    [ApiController]
    public class StaffScheduleController : ControllerBase
    {
        private IStaffScheduleService _staffScheduleService;
        public StaffScheduleController(IStaffScheduleService staffScheduleService)
        {
            _staffScheduleService = staffScheduleService;
        }

        [HttpPost("staffScheduleViewForCourseSection")]
        public ActionResult<StaffScheduleViewModel> StaffScheduleViewForCourseSection(StaffScheduleViewModel staffScheduleViewModel)
        {
            StaffScheduleViewModel staffSchedule = new StaffScheduleViewModel();
            try
            {
                staffSchedule = _staffScheduleService.StaffScheduleViewForCourseSection(staffScheduleViewModel);
            }
            catch (Exception es)
            {
                staffSchedule._message = es.Message;
                staffSchedule._failure = true;
            }
            return staffSchedule;
        }

        [HttpPost("addStaffCourseSectionSchedule")]
        public ActionResult<StaffScheduleViewModel> AddStaffCourseSectionSchedule(StaffScheduleViewModel staffScheduleViewModel)
        {
            StaffScheduleViewModel staffSchedule = new StaffScheduleViewModel();
            try
            {
                staffSchedule = _staffScheduleService.AddStaffCourseSectionSchedule(staffScheduleViewModel);
            }
            catch (Exception es)
            {
                staffSchedule._message = es.Message;
                staffSchedule._failure = true;
            }
            return staffSchedule;
        }

        [HttpPost("checkAvailabilityStaffCourseSectionSchedule")]
        public ActionResult<StaffScheduleViewModel> CheckAvailabilityStaffCourseSectionSchedule(StaffScheduleViewModel staffScheduleViewModel)
        {
            StaffScheduleViewModel staffSchedule = new StaffScheduleViewModel();
            try
            {
                staffSchedule = _staffScheduleService.CheckAvailabilityStaffCourseSectionSchedule(staffScheduleViewModel);
            }
            catch (Exception es)
            {
                staffSchedule._message = es.Message;
                staffSchedule._failure = true;
            }
            return staffSchedule;
        }
    }
}
