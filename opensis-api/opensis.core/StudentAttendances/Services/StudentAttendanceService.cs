using opensis.core.helper;
using opensis.core.StudentAttendances.Interfaces;
using opensis.data.Interface;
using opensis.data.ViewModels.StaffSchedule;
using opensis.data.ViewModels.StudentAttendances;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.StudentAttendances.Services
{
    public class StudentAttendanceService : IStudentAttendanceService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";

        public IStudentAttendanceRepository studentAttendanceRepository;
        public StudentAttendanceService(IStudentAttendanceRepository studentAttendanceRepository)
        {
            this.studentAttendanceRepository = studentAttendanceRepository;
        }
        public StudentAttendanceService() { }

        /// <summary>
        /// Student Attendance Add/Update
        /// </summary>
        /// <param name="studentAttendanceAddViewModel"></param>
        /// <returns></returns>

        public StudentAttendanceAddViewModel SaveStudentAttendance(StudentAttendanceAddViewModel studentAttendanceAddViewModel)
        {
            StudentAttendanceAddViewModel studentAttendanceAdd = new StudentAttendanceAddViewModel();
            if (TokenManager.CheckToken(studentAttendanceAddViewModel._tenantName + studentAttendanceAddViewModel._userName, studentAttendanceAddViewModel._token))
            {
                studentAttendanceAdd = this.studentAttendanceRepository.AddUpdateStudentAttendance(studentAttendanceAddViewModel);
            }
            else
            {
                studentAttendanceAdd._failure = true;
                studentAttendanceAdd._message = TOKENINVALID;
            }
            return studentAttendanceAdd;
        }

        public StudentAttendanceAddViewModel GetAllStudentAttendanceList(StudentAttendanceAddViewModel studentAttendanceAddViewModel)
        {
            StudentAttendanceAddViewModel studentAttendanceView = new StudentAttendanceAddViewModel();

            if (TokenManager.CheckToken(studentAttendanceAddViewModel._tenantName + studentAttendanceAddViewModel._userName, studentAttendanceAddViewModel._token))
            {
                studentAttendanceView = this.studentAttendanceRepository.GetAllStudentAttendanceList(studentAttendanceAddViewModel);
            }
            else
            {
                studentAttendanceView._failure = true;
                studentAttendanceView._message = TOKENINVALID;
            }
            return studentAttendanceView;
        }

        public ScheduledCourseSectionViewModel SearchCourseSectionForStudentAttendance(ScheduledCourseSectionViewModel scheduledCourseSectionViewModel)
        {
            ScheduledCourseSectionViewModel scheduledCourseSectionView = new ScheduledCourseSectionViewModel();

            if (TokenManager.CheckToken(scheduledCourseSectionViewModel._tenantName + scheduledCourseSectionViewModel._userName, scheduledCourseSectionViewModel._token))
            {
                scheduledCourseSectionView = this.studentAttendanceRepository.SearchCourseSectionForStudentAttendance(scheduledCourseSectionViewModel);
            }
            else
            {
                scheduledCourseSectionView._failure = true;
                scheduledCourseSectionView._message = TOKENINVALID;
            }
            return scheduledCourseSectionView;
        }
    }

}
