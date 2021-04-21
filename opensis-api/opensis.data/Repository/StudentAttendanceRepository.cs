using Microsoft.EntityFrameworkCore;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.StaffSchedule;
using opensis.data.ViewModels.StudentAttendances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace opensis.data.Repository
{
    public class StudentAttendanceRepository : IStudentAttendanceRepository
    {
        private CRMContext context;
        private static readonly string NORECORDFOUND = "No Record Found";
        public StudentAttendanceRepository(IDbContextFactory dbContextFactory)
        {
            this.context = dbContextFactory.Create();
        }

        /// <summary>
        /// Student Attendance Add/Update
        /// </summary>
        /// <param name="studentAttendanceAddViewModel"></param>
        /// <returns></returns>

        public StudentAttendanceAddViewModel AddUpdateStudentAttendance(StudentAttendanceAddViewModel studentAttendanceAddViewModel)
        {
            using (var transaction = this.context.Database.BeginTransaction())
            {
                try
                {
                    List<StudentAttendance> studentAttendance = new List<StudentAttendance>();

                    if (studentAttendanceAddViewModel.studentAttendance.Count > 0)
                    {
                        var attendanceDataExist = this.context?.StudentAttendance.Where(x => x.TenantId == studentAttendanceAddViewModel.TenantId && x.SchoolId == studentAttendanceAddViewModel.SchoolId && x.CourseSectionId == studentAttendanceAddViewModel.CourseSectionId && x.AttendanceDate == studentAttendanceAddViewModel.AttendanceDate && x.PeriodId == studentAttendanceAddViewModel.PeriodId).ToList();

                        if (attendanceDataExist.Count > 0)
                        {
                            this.context?.StudentAttendance.RemoveRange(attendanceDataExist);
                            
                            foreach (var studentAttendancedata in studentAttendanceAddViewModel.studentAttendance.ToList())
                            {
                                var studentAttendanceUpdate = new StudentAttendance()
                                {
                                    TenantId = studentAttendanceAddViewModel.TenantId,
                                    SchoolId = studentAttendanceAddViewModel.SchoolId,
                                    StudentId = studentAttendancedata.StudentId,
                                    StaffId = studentAttendanceAddViewModel.StaffId,
                                    CourseId = studentAttendanceAddViewModel.CourseId,
                                    CourseSectionId = studentAttendanceAddViewModel.CourseSectionId,
                                    AttendanceCategoryId = studentAttendancedata.AttendanceCategoryId,
                                    AttendanceCode = studentAttendancedata.AttendanceCode,
                                    AttendanceDate = studentAttendanceAddViewModel.AttendanceDate,
                                    Comments = studentAttendancedata.Comments,
                                    UpdatedBy = studentAttendanceAddViewModel.UpdatedBy,
                                    UpdatedOn = DateTime.UtcNow,
                                    BlockId = studentAttendancedata.BlockId,
                                    PeriodId = studentAttendanceAddViewModel.PeriodId,
                                };
                                studentAttendance.Add(studentAttendanceUpdate);
                            }
                            studentAttendanceAddViewModel._message = "Student Attendance updated succsesfully.";
                        }
                        else
                        {
                            foreach (var studentAttendancedata in studentAttendanceAddViewModel.studentAttendance.ToList())
                            {
                                var studentAttendanceAdd = new StudentAttendance()
                                {
                                    TenantId = studentAttendanceAddViewModel.TenantId,
                                    SchoolId = studentAttendanceAddViewModel.SchoolId,
                                    StudentId = studentAttendancedata.StudentId,
                                    StaffId = studentAttendanceAddViewModel.StaffId,
                                    CourseId = studentAttendanceAddViewModel.CourseId,
                                    CourseSectionId = studentAttendanceAddViewModel.CourseSectionId,
                                    AttendanceCategoryId = studentAttendancedata.AttendanceCategoryId,
                                    AttendanceCode = studentAttendancedata.AttendanceCode,
                                    AttendanceDate = studentAttendanceAddViewModel.AttendanceDate,
                                    Comments = studentAttendancedata.Comments,
                                    CreatedBy = studentAttendanceAddViewModel.UpdatedBy,
                                    CreatedOn = DateTime.UtcNow,
                                    BlockId = studentAttendancedata.BlockId,
                                    PeriodId = studentAttendanceAddViewModel.PeriodId,
                                };
                                studentAttendance.Add(studentAttendanceAdd);
                            }
                            studentAttendanceAddViewModel._message = "Student Attendance added succsesfully.";
                        }
                        this.context?.StudentAttendance.AddRange(studentAttendance);
                        this.context?.SaveChanges();
                        transaction.Commit();
                        studentAttendanceAddViewModel._failure = false;
                    }
                }
                catch (Exception es)
                {
                    transaction.Rollback();
                    studentAttendanceAddViewModel._failure = true;
                    studentAttendanceAddViewModel._message = es.Message;
                }
            }
            return studentAttendanceAddViewModel;
        }

        public StudentAttendanceAddViewModel GetAllStudentAttendanceList(StudentAttendanceAddViewModel studentAttendanceAddViewModel)
        {

            //StudentAttendanceAddViewModel studentAttendanceViewModel = new StudentAttendanceAddViewModel();
            try
            {
                var studentAttendanceData = this.context?.StudentAttendance.Include(c => c.StudentCoursesectionSchedule).Where(x => x.TenantId == studentAttendanceAddViewModel.TenantId && x.SchoolId == studentAttendanceAddViewModel.SchoolId && x.CourseSectionId == studentAttendanceAddViewModel.CourseSectionId && x.AttendanceDate == studentAttendanceAddViewModel.AttendanceDate && x.PeriodId == studentAttendanceAddViewModel.PeriodId).ToList();

                if (studentAttendanceData.Count > 0)
                {
                    studentAttendanceAddViewModel.studentAttendance = studentAttendanceData;
                    //studentAttendanceAddViewModel._tenantName = studentAttendanceListViewModel._tenantName;
                    //studentAttendanceAddViewModel._token = studentAttendanceListViewModel._token;
                    studentAttendanceAddViewModel._failure = false;
                }
                else
                {
                    studentAttendanceAddViewModel._failure = true;
                    studentAttendanceAddViewModel._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {
                studentAttendanceAddViewModel._message = es.Message;
                studentAttendanceAddViewModel._failure = true;
            }
            return studentAttendanceAddViewModel;

        }

        public ScheduledCourseSectionViewModel SearchCourseSectionForStudentAttendance(ScheduledCourseSectionViewModel scheduledCourseSectionViewModel)
        {
            ScheduledCourseSectionViewModel scheduledCourseSectionView = new ScheduledCourseSectionViewModel();
            try
            {
                scheduledCourseSectionView.TenantId = scheduledCourseSectionViewModel.TenantId;
                scheduledCourseSectionView._tenantName = scheduledCourseSectionViewModel._tenantName;
                scheduledCourseSectionView.SchoolId = scheduledCourseSectionViewModel.SchoolId;
                scheduledCourseSectionView.StaffId = scheduledCourseSectionViewModel.StaffId;
                scheduledCourseSectionView._token = scheduledCourseSectionViewModel._token;

                var scheduledCourseSectionData = this.context?.StaffCoursesectionSchedule.Include(s => s.StaffMaster).Include(x => x.CourseSection).Include(x => x.CourseSection.Course).Include(x => x.CourseSection.SchoolCalendars).Where(x => x.TenantId == scheduledCourseSectionViewModel.TenantId && x.SchoolId == scheduledCourseSectionViewModel.SchoolId && x.StaffId == scheduledCourseSectionViewModel.StaffId && x.IsDropped != true).ToList();

                if (scheduledCourseSectionData.Count() > 0)
                {

                    foreach (var scheduledCourseSection in scheduledCourseSectionData)
                    {
                        CourseSectionViewList CourseSections = new CourseSectionViewList();                        

                        if (scheduledCourseSection.CourseSection.ScheduleType == "Fixed Schedule (1)")
                        {
                            CourseSections.ScheduleType = "Fixed Schedule";

                            var courseFixedScheduleData = this.context?.CourseFixedSchedule.Include(c => c.BlockPeriod).FirstOrDefault(x => x.TenantId == scheduledCourseSection.TenantId && x.SchoolId == scheduledCourseSection.SchoolId && x.CourseSectionId == scheduledCourseSection.CourseSectionId);                 
                            if (courseFixedScheduleData != null)
                            {
                                courseFixedScheduleData.BlockPeriod.CourseFixedSchedule = null;
                                courseFixedScheduleData.BlockPeriod.CourseVariableSchedule = null;
                                courseFixedScheduleData.BlockPeriod.CourseCalendarSchedule = null;
                                courseFixedScheduleData.BlockPeriod.CourseBlockSchedule = null;
                                CourseSections.courseFixedSchedule = courseFixedScheduleData;

                            }
                        }
                        if (scheduledCourseSection.CourseSection.ScheduleType == "Variable Schedule (2)")
                        {
                            CourseSections.ScheduleType = "Variable Schedule";

                            var courseVariableScheduleData = this.context?.CourseVariableSchedule.Include(c => c.BlockPeriod).Where(x => x.TenantId == scheduledCourseSection.TenantId && x.SchoolId == scheduledCourseSection.SchoolId && x.CourseSectionId == scheduledCourseSection.CourseSectionId).ToList();

                            if (courseVariableScheduleData.Count > 0)
                            {
                                courseVariableScheduleData.ForEach(x => { x.BlockPeriod.CourseFixedSchedule = null; x.BlockPeriod.CourseVariableSchedule = null; x.BlockPeriod.CourseCalendarSchedule = null; x.BlockPeriod.CourseBlockSchedule = null; });                                
                                
                                CourseSections.courseVariableSchedule = courseVariableScheduleData;
                            }
                        }
                        if (scheduledCourseSection.CourseSection.ScheduleType == "Calendar Schedule (3)")
                        {
                            CourseSections.ScheduleType = "Calendar Schedule";

                            var courseCalenderScheduleData = this.context?.CourseCalendarSchedule.Include(c => c.BlockPeriod).Where(x => x.TenantId == scheduledCourseSection.TenantId && x.SchoolId == scheduledCourseSection.SchoolId && x.CourseSectionId == scheduledCourseSection.CourseSectionId).ToList();

                            if (courseCalenderScheduleData.Count > 0)
                            {
                                courseCalenderScheduleData.ForEach(x => { x.BlockPeriod.CourseFixedSchedule = null; x.BlockPeriod.CourseVariableSchedule = null; x.BlockPeriod.CourseCalendarSchedule = null; x.BlockPeriod.CourseBlockSchedule = null; });
                                
                                CourseSections.courseCalendarSchedule = courseCalenderScheduleData;
                            }
                        }
                        if (scheduledCourseSection.CourseSection.ScheduleType == "Block Schedule (4)")
                        {
                            CourseSections.ScheduleType = "Block Schedule";

                            var courseBlockScheduleData = this.context?.CourseBlockSchedule.Include(c => c.BlockPeriod).Where(x => x.TenantId == scheduledCourseSection.TenantId && x.SchoolId == scheduledCourseSection.SchoolId && x.CourseSectionId == scheduledCourseSection.CourseSectionId).ToList();

                            if (courseBlockScheduleData.Count > 0)
                            {
                                courseBlockScheduleData.ForEach(x => { x.BlockPeriod.CourseFixedSchedule = null; x.BlockPeriod.CourseVariableSchedule = null; x.BlockPeriod.CourseCalendarSchedule = null; x.BlockPeriod.CourseBlockSchedule = null; });
                                
                                CourseSections.courseBlockSchedule = courseBlockScheduleData;
                            }
                        }

                        CourseSections.CourseId = scheduledCourseSection.CourseId;
                        CourseSections.CourseSectionId = scheduledCourseSection.CourseSectionId;
                        CourseSections.CourseSectionName = scheduledCourseSection.CourseSectionName;
                        CourseSections.YrMarkingPeriodId = scheduledCourseSection.YrMarkingPeriodId;
                        CourseSections.SmstrMarkingPeriodId = scheduledCourseSection.SmstrMarkingPeriodId;
                        CourseSections.QtrMarkingPeriodId = scheduledCourseSection.QtrMarkingPeriodId;
                        CourseSections.DurationStartDate = scheduledCourseSection.DurationStartDate;
                        CourseSections.DurationEndDate = scheduledCourseSection.DurationEndDate;
                        CourseSections.MeetingDays = scheduledCourseSection.MeetingDays;
                        CourseSections.AttendanceCategoryId = scheduledCourseSection.CourseSection.AttendanceCategoryId;

                        scheduledCourseSectionView.courseSectionViewList.Add(CourseSections);
                    }
                }
                else
                {
                    scheduledCourseSectionView._failure = true;
                    scheduledCourseSectionView._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {
                scheduledCourseSectionView.courseSectionViewList = null;
                scheduledCourseSectionView._failure = true;
                scheduledCourseSectionView._message = es.Message;
            }
            return scheduledCourseSectionView;
        }
    }
}
