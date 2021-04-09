using opensis.core.helper;
using opensis.core.StudentSchedule.Interfaces;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.StudentSchedule;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.StudentSchedule.Services
{
    public class StudentScheduleService : IStudentScheduleService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";

        public IStudentScheduleRepository studentScheduleRepository;
        public StudentScheduleService(IStudentScheduleRepository studentScheduleRepository)
        {
            this.studentScheduleRepository = studentScheduleRepository;
        }
        public StudentScheduleService() { }

        /// <summary>
        /// Add Student Course Section Schedule
        /// </summary>
        /// <param name="studentCourseSectionScheduleAddViewModel"></param>
        /// <returns></returns>
        public StudentCourseSectionScheduleAddViewModel AddStudentCourseSectionSchedule(StudentCourseSectionScheduleAddViewModel studentCourseSectionScheduleAddViewModel)
        {
            StudentCourseSectionScheduleAddViewModel studentCourseSectionScheduleAddModel = new StudentCourseSectionScheduleAddViewModel();
            try
            {
                if (TokenManager.CheckToken(studentCourseSectionScheduleAddViewModel._tenantName + studentCourseSectionScheduleAddViewModel._userName, studentCourseSectionScheduleAddViewModel._token))
                {
                    studentCourseSectionScheduleAddModel = this.studentScheduleRepository.AddStudentCourseSectionSchedule(studentCourseSectionScheduleAddViewModel);
                }
                else
                {
                    studentCourseSectionScheduleAddModel._failure = true;
                    studentCourseSectionScheduleAddModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {

                studentCourseSectionScheduleAddModel._failure = true;
                studentCourseSectionScheduleAddModel._message = es.Message;
            }
            return studentCourseSectionScheduleAddModel;
        }

        /// <summary>
        /// Search Scheduled Student For Group Drop
        /// </summary>
        /// <param name="scheduleStudentListViewModel"></param>
        /// <returns></returns>
        public ScheduleStudentListViewModel SearchScheduledStudentForGroupDrop(PageResult pageResult)
        {
            ScheduleStudentListViewModel ScheduledStudentListView = new ScheduleStudentListViewModel();
            try
            {
                if (TokenManager.CheckToken(pageResult._tenantName + pageResult._userName, pageResult._token))
                {
                    ScheduledStudentListView = this.studentScheduleRepository.SearchScheduledStudentForGroupDrop(pageResult);
                }
                else
                {
                    ScheduledStudentListView._failure = true;
                    ScheduledStudentListView._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {

                ScheduledStudentListView._failure = true;
                ScheduledStudentListView._message = es.Message;
            }
            return ScheduledStudentListView;
        }

        public ScheduledStudentDropModel GroupDropForScheduledStudent(ScheduledStudentDropModel scheduledStudentDropModel)
        {
            ScheduledStudentDropModel ScheduledStudentDrop = new ScheduledStudentDropModel();
            try
            {
                if (TokenManager.CheckToken(scheduledStudentDropModel._tenantName + scheduledStudentDropModel._userName, scheduledStudentDropModel._token))
                {
                    ScheduledStudentDrop = this.studentScheduleRepository.GroupDropForScheduledStudent(scheduledStudentDropModel);
                }
                else
                {
                    ScheduledStudentDrop._failure = true;
                    ScheduledStudentDrop._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {

                ScheduledStudentDrop._failure = true;
                ScheduledStudentDrop._message = es.Message;
            }
            return ScheduledStudentDrop;
        }

        public StudentScheduleReportViewModel StudentScheduleReport(StudentScheduleReportViewModel studentScheduleReportViewModel)
        {
            StudentScheduleReportViewModel studentScheduleReportView = new StudentScheduleReportViewModel();
            try
            {
                if (TokenManager.CheckToken(studentScheduleReportViewModel._tenantName + studentScheduleReportViewModel._userName, studentScheduleReportViewModel._token))
                {
                    studentScheduleReportView = this.studentScheduleRepository.StudentScheduleReport(studentScheduleReportViewModel);
                }
                else
                {
                    studentScheduleReportView._failure = true;
                    studentScheduleReportView._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {

                studentScheduleReportView._failure = true;
                studentScheduleReportView._message = es.Message;
            }
            return studentScheduleReportView;
        }

        /// <summary>
        /// Delete Student Schedule Report
        /// </summary>
        /// <param name="studentCourseSectionScheduleAddViewModel"></param>
        /// <returns></returns>
        public StudentCourseSectionScheduleAddViewModel DeleteStudentScheduleReport(StudentCourseSectionScheduleAddViewModel studentCourseSectionScheduleAddViewModel)
        {
            StudentCourseSectionScheduleAddViewModel studentCourseSectionScheduleDeleteModel = new StudentCourseSectionScheduleAddViewModel();
            try
            {
                if (TokenManager.CheckToken(studentCourseSectionScheduleAddViewModel._tenantName + studentCourseSectionScheduleAddViewModel._userName, studentCourseSectionScheduleAddViewModel._token))
                {
                    studentCourseSectionScheduleDeleteModel = this.studentScheduleRepository.DeleteStudentScheduleReport(studentCourseSectionScheduleAddViewModel);
                }
                else
                {
                    studentCourseSectionScheduleDeleteModel._failure = true;
                    studentCourseSectionScheduleDeleteModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {

                studentCourseSectionScheduleDeleteModel._failure = true;
                studentCourseSectionScheduleDeleteModel._message = es.Message;
            }
            return studentCourseSectionScheduleDeleteModel;
        }
    }
}
