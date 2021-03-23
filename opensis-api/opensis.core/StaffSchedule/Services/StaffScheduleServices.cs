using opensis.core.helper;
using opensis.core.StaffSchedule.Interfaces;
using opensis.data.Interface;
using opensis.data.ViewModels.StaffSchedule;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.StaffSchedule.Services
{
    public class StaffScheduleServices : IStaffScheduleService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";

        public IStaffScheduleRepository staffScheduleRepository;
        public StaffScheduleServices(IStaffScheduleRepository staffScheduleRepository)
        {
            this.staffScheduleRepository = staffScheduleRepository;
        }

        /// <summary>
        /// Teacher Schedule View For CourseSection
        /// </summary>
        /// <param name="staffScheduleViewModel"></param>
        /// <returns></returns>
        public StaffScheduleViewModel StaffScheduleViewForCourseSection(StaffScheduleViewModel staffScheduleViewModel)
        {
            StaffScheduleViewModel staffSchedule = new StaffScheduleViewModel();
            try
            {
                if (TokenManager.CheckToken(staffScheduleViewModel._tenantName, staffScheduleViewModel._token))
                {
                    staffSchedule = this.staffScheduleRepository.StaffScheduleViewForCourseSection(staffScheduleViewModel);
                }
                else
                {
                    staffSchedule._failure = true;
                    staffSchedule._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                staffSchedule._failure = true;
                staffSchedule._message = es.Message;
            }
            return staffSchedule;
        }

        /// <summary>
        /// Add Staff CourseSection Schedule
        /// </summary>
        /// <param name="staffScheduleViewModel"></param>
        /// <returns></returns>
        public StaffScheduleViewModel AddStaffCourseSectionSchedule(StaffScheduleViewModel staffScheduleViewModel)
        {
            StaffScheduleViewModel staffSchedule = new StaffScheduleViewModel();
            try
            {
                if (TokenManager.CheckToken(staffScheduleViewModel._tenantName, staffScheduleViewModel._token))
                {
                    staffSchedule = this.staffScheduleRepository.AddStaffCourseSectionSchedule(staffScheduleViewModel);
                }
                else
                {
                    staffSchedule._failure = true;
                    staffSchedule._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                staffSchedule._failure = true;
                staffSchedule._message = es.Message;
            }
            return staffSchedule;
        }

        public StaffScheduleViewModel CheckAvailabilityStaffCourseSectionSchedule(StaffScheduleViewModel staffScheduleViewModel)
        {
            StaffScheduleViewModel staffSchedule = new StaffScheduleViewModel();
            try
            {
                if (TokenManager.CheckToken(staffScheduleViewModel._tenantName, staffScheduleViewModel._token))
                {
                    staffSchedule = this.staffScheduleRepository.CheckAvailabilityStaffCourseSectionSchedule(staffScheduleViewModel);
                }
                else
                {
                    staffSchedule._failure = true;
                    staffSchedule._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                staffSchedule._failure = true;
                staffSchedule._message = es.Message;
            }
            return staffSchedule;
        }
    }
}
