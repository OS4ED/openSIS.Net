/***********************************************************************************
openSIS is a free student information system for public and non-public
schools from Open Solutions for Education, Inc.Website: www.os4ed.com.

Visit the openSIS product website at https://opensis.com to learn more.
If you have question regarding this software or the license, please contact
via the website.

The software is released under the terms of the GNU Affero General Public License as
published by the Free Software Foundation, version 3 of the License.
See https://www.gnu.org/licenses/agpl-3.0.en.html.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

Copyright (c) Open Solutions for Education, Inc.

All rights reserved.
***********************************************************************************/

using Microsoft.EntityFrameworkCore;
using opensis.data.Helper;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.Staff;
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
                        var attendanceDataExist = this.context?.StudentAttendance.Include(c=>c.StudentAttendanceComments).Where(x => x.TenantId == studentAttendanceAddViewModel.TenantId && x.SchoolId == studentAttendanceAddViewModel.SchoolId && x.CourseSectionId == studentAttendanceAddViewModel.CourseSectionId && x.AttendanceDate == studentAttendanceAddViewModel.AttendanceDate && x.PeriodId == studentAttendanceAddViewModel.PeriodId).ToList();

                        int? StudentAttendanceId = 1;

                        var studentAttendanceData = this.context?.StudentAttendance.Where(x => x.SchoolId == studentAttendanceAddViewModel.SchoolId && x.TenantId == studentAttendanceAddViewModel.TenantId).OrderByDescending(x => x.StudentAttendanceId).FirstOrDefault();

                        if (studentAttendanceData != null)
                        {
                            StudentAttendanceId = studentAttendanceData.StudentAttendanceId + 1;
                        }

                        long? CommentId = Utility.GetMaxLongPK(this.context, new Func<StudentAttendanceComments, long>(x => x.CommentId));

                        foreach (var studentAttendances in studentAttendanceAddViewModel.studentAttendance)
                        {
                            if (studentAttendances.StudentAttendanceComments.Count()>0)
                            {
                                foreach (var StudentAttendanceComment in studentAttendances.StudentAttendanceComments)
                                {
                                    StudentAttendanceComment.CommentId = (long)CommentId;
                                    CommentId++;
                                }
                            }
                        }

                        if (attendanceDataExist.Count > 0)
                        {
                            //this.context?.StudentAttendance.RemoveRange(attendanceDataExist);
                            var studentAttendanceIDs = attendanceDataExist.Select(v => v.StudentAttendanceId).ToList();

                            var studentAttendanceCommentData = this.context?.StudentAttendanceComments.Where(x => x.TenantId == studentAttendanceAddViewModel.TenantId && x.SchoolId == studentAttendanceAddViewModel.SchoolId && (studentAttendanceIDs == null || (studentAttendanceIDs.Contains(x.StudentAttendanceId))));

                            if (studentAttendanceCommentData.Count()>0)
                            {
                                this.context?.StudentAttendanceComments.RemoveRange(studentAttendanceCommentData);
                            }
                            this.context?.StudentAttendance.RemoveRange(attendanceDataExist);
                            this.context?.SaveChanges();

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
                                    //Comments = studentAttendancedata.Comments,
                                    UpdatedBy = studentAttendanceAddViewModel.UpdatedBy,
                                    UpdatedOn = DateTime.UtcNow,
                                    BlockId = studentAttendancedata.BlockId,
                                    PeriodId = studentAttendanceAddViewModel.PeriodId,
                                    StudentAttendanceId = (int)StudentAttendanceId,                                    
                                    StudentAttendanceComments = studentAttendancedata.StudentAttendanceComments.Select(c =>
                                     {
                                         c.UpdatedBy = studentAttendanceAddViewModel.UpdatedBy;
                                         c.UpdatedOn = DateTime.UtcNow;                                         
                                         return c;
                                     }).ToList()                                
                                };
                                studentAttendance.Add(studentAttendanceUpdate);
                                StudentAttendanceId++;
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
                                    //Comments = studentAttendancedata.Comments,
                                    CreatedBy = studentAttendanceAddViewModel.UpdatedBy,
                                    CreatedOn = DateTime.UtcNow,
                                    BlockId = studentAttendancedata.BlockId,
                                    PeriodId = studentAttendanceAddViewModel.PeriodId,
                                    StudentAttendanceId = (int)StudentAttendanceId,
                                    StudentAttendanceComments = studentAttendancedata.StudentAttendanceComments.Select(c =>
                                    {
                                        c.UpdatedBy = studentAttendanceAddViewModel.UpdatedBy;
                                        c.UpdatedOn = DateTime.UtcNow;
                                        return c;
                                    }).ToList()
                                };
                                studentAttendance.Add(studentAttendanceAdd);
                                StudentAttendanceId++;
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

        /// <summary>
        /// Get All Student Attendance List
        /// </summary>
        /// <param name="studentAttendanceAddViewModel"></param>
        /// <returns></returns>
        public StudentAttendanceAddViewModel GetAllStudentAttendanceList(StudentAttendanceAddViewModel studentAttendanceAddViewModel)
        {

            //StudentAttendanceAddViewModel studentAttendanceViewModel = new StudentAttendanceAddViewModel();
            try
            {
                var studentAttendanceData = this.context?.StudentAttendance.Include(c => c.StudentCoursesectionSchedule).Include(v=>v.StudentAttendanceComments).Where(x => x.TenantId == studentAttendanceAddViewModel.TenantId && x.SchoolId == studentAttendanceAddViewModel.SchoolId && x.CourseSectionId == studentAttendanceAddViewModel.CourseSectionId && x.AttendanceDate == studentAttendanceAddViewModel.AttendanceDate && x.PeriodId == studentAttendanceAddViewModel.PeriodId).ToList();

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
        /// <summary>
        /// Search Course Section For Student Attendance
        /// </summary>
        /// <param name="scheduledCourseSectionViewModel"></param>
        /// <returns></returns>
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
                        CourseSections.AttendanceTaken = scheduledCourseSection.CourseSection.AttendanceTaken;

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

        /// <summary>
        /// Add Update Student Attendance For Student360
        /// </summary>
        /// <param name="studentAttendanceAddViewModel"></param>
        /// <returns></returns>
        public StudentAttendanceAddViewModel AddUpdateStudentAttendanceForStudent360(StudentAttendanceAddViewModel studentAttendanceAddViewModel)
        {
            using (var transaction = this.context.Database.BeginTransaction())
            {
                try
                {
                    List<StudentAttendance> studentAttendance = new List<StudentAttendance>();

                    if (studentAttendanceAddViewModel.studentAttendance.Count > 0)
                    {
                        var courseSectionIds = studentAttendanceAddViewModel.studentAttendance.Select(v => v.CourseSectionId).ToList();
                        var attendanceDates = studentAttendanceAddViewModel.studentAttendance.Select(v => v.AttendanceDate).ToList();
                        var periodIds = studentAttendanceAddViewModel.studentAttendance.Select(v => v.PeriodId).ToList();

                        var attendanceDataExist = this.context?.StudentAttendance.Where(x => x.TenantId == studentAttendanceAddViewModel.TenantId && x.SchoolId == studentAttendanceAddViewModel.SchoolId && x.StudentId == studentAttendanceAddViewModel.StudentId /*&& x.CourseSectionId == studentAttendanceAddViewModel.CourseSectionId*/ /*&& x.AttendanceDate == studentAttendanceAddViewModel.AttendanceDate && x.PeriodId == studentAttendanceAddViewModel.PeriodId*/ && (courseSectionIds == null || (courseSectionIds.Contains(x.CourseSectionId))) && (attendanceDates == null || (attendanceDates.Contains(x.AttendanceDate))) && (periodIds == null || (periodIds.Contains(x.PeriodId)))).ToList();

                        int? StudentAttendanceId = 1;

                        var studentAttendanceData = this.context?.StudentAttendance.Where(x => x.SchoolId == studentAttendanceAddViewModel.SchoolId && x.TenantId == studentAttendanceAddViewModel.TenantId).OrderByDescending(x => x.StudentAttendanceId).FirstOrDefault();

                        if (studentAttendanceData != null)
                        {
                            StudentAttendanceId = studentAttendanceData.StudentAttendanceId + 1;
                        }

                        long? CommentId = Utility.GetMaxLongPK(this.context, new Func<StudentAttendanceComments, long>(x => x.CommentId));

                        foreach (var studentAttendances in studentAttendanceAddViewModel.studentAttendance)
                        {
                            if (studentAttendances.StudentAttendanceComments.Count() > 0)
                            {
                                foreach (var StudentAttendanceComment in studentAttendances.StudentAttendanceComments)
                                {
                                    StudentAttendanceComment.CommentId = (long)CommentId;
                                    CommentId++;
                                }
                            }
                        }

                        if (attendanceDataExist.Count > 0)
                        {
                            //this.context?.StudentAttendance.RemoveRange(attendanceDataExist);
                            var studentAttendanceIDs = attendanceDataExist.Select(v => v.StudentAttendanceId).ToList();

                            var studentAttendanceCommentData = this.context?.StudentAttendanceComments.Where(x => x.TenantId == studentAttendanceAddViewModel.TenantId && x.SchoolId == studentAttendanceAddViewModel.SchoolId && (studentAttendanceIDs == null || (studentAttendanceIDs.Contains(x.StudentAttendanceId))));

                            if (studentAttendanceCommentData.Count() > 0)
                            {
                                this.context?.StudentAttendanceComments.RemoveRange(studentAttendanceCommentData);
                            }
                            this.context?.StudentAttendance.RemoveRange(attendanceDataExist);
                            this.context?.SaveChanges();

                            foreach (var studentAttendancedata in studentAttendanceAddViewModel.studentAttendance.ToList())
                            {
                                var studentAttendanceUpdate = new StudentAttendance()
                                {
                                    TenantId = studentAttendanceAddViewModel.TenantId,
                                    SchoolId = studentAttendanceAddViewModel.SchoolId,
                                    StudentId = studentAttendanceAddViewModel.StudentId,
                                    StaffId = studentAttendancedata.StaffId,
                                    CourseId = studentAttendancedata.CourseId,
                                    CourseSectionId = studentAttendancedata.CourseSectionId,
                                    AttendanceCategoryId = studentAttendancedata.AttendanceCategoryId,
                                    AttendanceCode = studentAttendancedata.AttendanceCode,
                                    AttendanceDate = studentAttendancedata.AttendanceDate,
                                    //Comments = studentAttendancedata.Comments,
                                    UpdatedBy = studentAttendanceAddViewModel.UpdatedBy,
                                    UpdatedOn = DateTime.UtcNow,
                                    BlockId = studentAttendancedata.BlockId,
                                    PeriodId = studentAttendancedata.PeriodId,
                                    StudentAttendanceId = (int)StudentAttendanceId,
                                    StudentAttendanceComments = studentAttendancedata.StudentAttendanceComments.Select(c =>
                                    {
                                        c.UpdatedBy = studentAttendanceAddViewModel.UpdatedBy;
                                        c.UpdatedOn = DateTime.UtcNow;
                                        return c;
                                    }).ToList()
                                };
                                studentAttendance.Add(studentAttendanceUpdate);
                                StudentAttendanceId++;
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
                                    StudentId = studentAttendanceAddViewModel.StudentId,
                                    StaffId = studentAttendancedata.StaffId,
                                    CourseId = studentAttendancedata.CourseId,
                                    CourseSectionId = studentAttendancedata.CourseSectionId,
                                    AttendanceCategoryId = studentAttendancedata.AttendanceCategoryId,
                                    AttendanceCode = studentAttendancedata.AttendanceCode,
                                    AttendanceDate = studentAttendancedata.AttendanceDate,
                                    //Comments = studentAttendancedata.Comments,
                                    CreatedBy = studentAttendanceAddViewModel.UpdatedBy,
                                    CreatedOn = DateTime.UtcNow,
                                    BlockId = studentAttendancedata.BlockId,
                                    PeriodId = studentAttendancedata.PeriodId,
                                    StudentAttendanceId = (int)StudentAttendanceId,
                                    StudentAttendanceComments = studentAttendancedata.StudentAttendanceComments.Select(c =>
                                    {
                                        c.UpdatedBy = studentAttendanceAddViewModel.UpdatedBy;
                                        c.UpdatedOn = DateTime.UtcNow;
                                        return c;
                                    }).ToList()
                                };
                                studentAttendance.Add(studentAttendanceAdd);
                                StudentAttendanceId++;
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

        /// <summary>
        /// Staff List For Missing Attendance
        /// </summary>
        /// <param name="pageResult"></param>
        /// <returns></returns>
        public StaffListModel StaffListForMissingAttendance(PageResult pageResult)
        {
            StaffListModel staffListViewModel = new StaffListModel();
            IQueryable<StaffMaster> transactionIQ = null;
            List<StaffMaster> staffCoursesectionSchedule = new List<StaffMaster>();
            IQueryable<StaffCoursesectionSchedule> staffScheduleDataList = null;

            staffScheduleDataList = this.context?.StaffCoursesectionSchedule.Include(d => d.StaffMaster).Include(d => d.StudentAttendance).Include(b=>b.CourseSection).Where(e => e.SchoolId == pageResult.SchoolId && e.TenantId == pageResult.TenantId && e.IsDropped != true).Select(v => new StaffCoursesectionSchedule()
            {
                SchoolId = v.SchoolId,
                TenantId = v.TenantId,
                CourseSectionId = v.CourseSectionId,
                CourseId = v.CourseId,
                IsDropped = v.IsDropped,
                DurationStartDate = v.DurationStartDate,
                DurationEndDate = v.DurationEndDate,
                MeetingDays = v.MeetingDays,
                CourseSection=v.CourseSection,
                StaffMaster = new StaffMaster()
                {
                    SchoolId = v.StaffMaster.SchoolId,
                    TenantId = v.StaffMaster.TenantId,
                    StaffId = v.StaffMaster.StaffId,
                    FirstGivenName = v.StaffMaster.FirstGivenName,
                    MiddleName = v.StaffMaster.MiddleName,
                    LastFamilyName = v.StaffMaster.LastFamilyName,
                    StaffInternalId = v.StaffMaster.StaffInternalId,
                    Profile = v.StaffMaster.Profile,
                    JobTitle = v.StaffMaster.JobTitle,
                    SchoolEmail = v.StaffMaster.SchoolEmail,
                    MobilePhone = v.StaffMaster.MobilePhone
                }
            });

            var calendarData = this.context?.CourseCalendarSchedule.Where(v => v.SchoolId == pageResult.SchoolId && v.TenantId == pageResult.TenantId).ToList();


            if (pageResult.DobStartDate.HasValue && pageResult.DobEndDate.HasValue)
            {
                staffScheduleDataList = staffScheduleDataList.Where(e => ((pageResult.DobStartDate.Value.Date >= e.DurationStartDate.Value.Date && pageResult.DobStartDate.Value.Date <= e.DurationEndDate.Value.Date) || (pageResult.DobEndDate.Value.Date >= e.DurationStartDate.Value.Date && pageResult.DobEndDate.Value.Date <= e.DurationEndDate)));
            }

            List<int> ID = new List<int>();
            List<DateTime> missingAttendanceDatelist = new List<DateTime>();
            foreach (var staffScheduleData in staffScheduleDataList.ToList())
            {
                DateTime start;
                DateTime end;
                List<DateTime> dateList = new List<DateTime>();
                if (pageResult.DobStartDate.HasValue && pageResult.DobEndDate.HasValue)
                {
                    start = (DateTime)pageResult.DobStartDate;
                    end = (DateTime)pageResult.DobEndDate;
                }
                else
                {
                    start = (DateTime)staffScheduleData.DurationStartDate;
                    end = (DateTime)staffScheduleData.DurationEndDate;
                }

                if (staffScheduleData.CourseSection.ScheduleType== "Calendar Schedule (3)")
                {
                    if (calendarData.Count>0)
                    {
                        var courseCalenderScheduleDateList = calendarData.Where(c => c.CourseId == staffScheduleData.CourseId && c.CourseSectionId == staffScheduleData.CourseSectionId && c.Date >= start && c.Date <= end).Select(v => v.Date).ToList();

                        if (courseCalenderScheduleDateList.Count>0)
                        {
                            foreach (var courseCalenderScheduleDate in courseCalenderScheduleDateList)
                            {
                                var staffAttendanceData = this.context?.StudentAttendance.Where(b => b.SchoolId == staffScheduleData.SchoolId && b.TenantId == staffScheduleData.TenantId /*&& b.StaffId == staffScheduleData.StaffId*/ && b.AttendanceDate.Date == courseCalenderScheduleDate.Value.Date && b.CourseSectionId == staffScheduleData.CourseSectionId && b.CourseId == staffScheduleData.CourseId);

                                if (staffAttendanceData.Count() == 0)
                                {
                                    missingAttendanceDatelist.Add(courseCalenderScheduleDate.Value.Date);

                                    if (!ID.Contains(staffScheduleData.StaffMaster.StaffId))
                                    {
                                        //staffCoursesectionSchedule.Add(staffScheduleData.StaffMaster);
                                        ID.Add(staffScheduleData.StaffMaster.StaffId);
                                        //break;
                                    }
                                    //else
                                    //{
                                    //    break;
                                    //}
                                }
                            }
                        }
                    }
                }
                else
                {
                    string[] meetingDays = staffScheduleData.MeetingDays.Split("|");

                    bool allDays = meetingDays == null || !meetingDays.Any();

                    dateList = Enumerable.Range(0, 1 + end.Subtract(start).Days)
                                          .Select(offset => start.AddDays(offset))
                                          .Where(d => allDays || meetingDays.Contains(d.DayOfWeek.ToString()))
                                          .ToList();

                    if (pageResult.DobStartDate.HasValue && pageResult.DobEndDate.HasValue)
                    {
                        dateList = dateList.Where(s => dateList.Any(secL => s.Date > staffScheduleData.DurationStartDate && s.Date < staffScheduleData.DurationEndDate)).ToList();
                    }



                    foreach (var date in dateList)
                    {
                        var staffAttendanceData = this.context?.StudentAttendance.Where(b => b.SchoolId == staffScheduleData.SchoolId && b.TenantId == staffScheduleData.TenantId /*&& b.StaffId == staffScheduleData.StaffId*/ && b.AttendanceDate == date && b.CourseSectionId == staffScheduleData.CourseSectionId && b.CourseId == staffScheduleData.CourseId);

                        if (staffAttendanceData.Count() == 0)
                        {
                            missingAttendanceDatelist.Add(date.Date);

                            if (!ID.Contains(staffScheduleData.StaffMaster.StaffId))
                            {
                                //staffCoursesectionSchedule.Add(staffScheduleData.StaffMaster);
                                ID.Add(staffScheduleData.StaffMaster.StaffId);
                                //break;
                            }
                            //else
                            //{
                            //    break;
                            //}
                        }
                    }
                }                
            }
            var staffList = staffScheduleDataList.Where(x => (ID == null || (ID.Contains(x.StaffMaster.StaffId)))).Select(b => b.StaffMaster).ToList();
            if (staffList.Count > 0)
            {
                staffCoursesectionSchedule.AddRange(staffList);
            }
            staffCoursesectionSchedule = staffCoursesectionSchedule.GroupBy(c => c.StaffId).Select(c => c.FirstOrDefault()).ToList();
            missingAttendanceDatelist = missingAttendanceDatelist.GroupBy(b => b.Date).Select(c => c.FirstOrDefault()).ToList();

            try
            {
                if (pageResult.FilterParams == null || pageResult.FilterParams.Count == 0)
                {
                    transactionIQ = staffCoursesectionSchedule.AsQueryable();
                }
                else
                {
                    if (pageResult.FilterParams != null && pageResult.FilterParams.ElementAt(0).ColumnName == null && pageResult.FilterParams.Count == 1)
                    {
                        string Columnvalue = pageResult.FilterParams.ElementAt(0).FilterValue;

                        transactionIQ = staffCoursesectionSchedule.Where(x => x.FirstGivenName != null && x.FirstGivenName.ToLower().Contains(Columnvalue.ToLower()) || x.MiddleName != null && x.MiddleName.ToLower().Contains(Columnvalue.ToLower()) || x.LastFamilyName != null && x.LastFamilyName.ToLower().Contains(Columnvalue.ToLower()) || x.StaffInternalId != null && x.StaffInternalId.ToLower().Contains(Columnvalue.ToLower()) || x.Profile != null && x.Profile.ToLower().Contains(Columnvalue.ToLower()) || x.JobTitle != null && x.JobTitle.ToLower().Contains(Columnvalue.ToLower()) || x.SchoolEmail != null && x.SchoolEmail.ToLower().Contains(Columnvalue.ToLower()) || x.MobilePhone != null && x.MobilePhone.Contains(Columnvalue)).AsQueryable();
                    }
                }
                //transactionIQ = transactionIQ.Distinct();

                if (pageResult.SortingModel != null)
                {
                    transactionIQ = Utility.Sort(transactionIQ, pageResult.SortingModel.SortColumn, pageResult.SortingModel.SortDirection.ToLower());
                }

                int totalCount = transactionIQ.Count();

                if (pageResult.PageNumber > 0 && pageResult.PageSize > 0)
                {
                    transactionIQ = transactionIQ.Select(p => new StaffMaster
                    {
                        SchoolId = p.SchoolId,
                        TenantId = p.TenantId,
                        StaffId = p.StaffId,
                        StaffInternalId = p.StaffInternalId,
                        FirstGivenName = p.FirstGivenName,
                        MiddleName = p.MiddleName,
                        LastFamilyName = p.LastFamilyName,
                        Profile = p.Profile,
                        JobTitle = p.JobTitle,
                        SchoolEmail = p.SchoolEmail,
                        MobilePhone = p.MobilePhone
                    }).Skip((pageResult.PageNumber - 1) * pageResult.PageSize).Take(pageResult.PageSize);
                }

                staffListViewModel.staffMaster = transactionIQ.ToList();
                staffListViewModel.missingAttendanceDateList = missingAttendanceDatelist;
                staffListViewModel.TotalCount = totalCount;
                staffListViewModel.PageNumber = pageResult.PageNumber;
                staffListViewModel._pageSize = pageResult.PageSize;
                staffListViewModel._failure = false;
                staffListViewModel.TenantId = pageResult.TenantId;
                staffListViewModel._tenantName = pageResult._tenantName;
                staffListViewModel._token = pageResult._token;
                staffListViewModel._userName = pageResult._userName;
            }
            catch (Exception es)
            {
                staffListViewModel._failure = true;
                staffListViewModel._message = es.Message;
            }
            return staffListViewModel;
        }

        /// <summary>
        /// Missing Attendance List
        /// </summary>
        /// <param name="pageResult"></param>
        /// <returns></returns>
        public ScheduledCourseSectionViewModel MissingAttendanceList(PageResult pageResult)
        {
            ScheduledCourseSectionViewModel scheduledCourseSectionView = new ScheduledCourseSectionViewModel();
            IQueryable<CourseSectionViewList> transactionIQ = null;
            List<CourseSectionViewList> staffCoursesectionSchedule = new List<CourseSectionViewList>();
            CourseFixedSchedule courseFixedSchedule = null;
            List<CourseVariableSchedule> CourseVariableSchedule = new List<CourseVariableSchedule>();
            List<CourseCalendarSchedule> courseCalendarSchedule = new List<CourseCalendarSchedule>();
            List<CourseBlockSchedule> CourseBlockSchedule = new List<CourseBlockSchedule>();
            

            try
            {
                var staffCourseSectionDataList = this.context?.StaffCoursesectionSchedule.Include(x => x.CourseSection).Where(s => s.SchoolId == pageResult.SchoolId && s.TenantId == pageResult.TenantId && s.StaffId == pageResult.StaffId && ((pageResult.DobStartDate.Value.Date >= s.DurationStartDate.Value.Date && pageResult.DobStartDate.Value.Date <= s.DurationEndDate.Value.Date) || (pageResult.DobEndDate.Value.Date >= s.DurationStartDate.Value.Date && pageResult.DobEndDate.Value.Date <= s.DurationEndDate)) && s.IsDropped != true).Select(v => new StaffCoursesectionSchedule()
                {
                    SchoolId = v.SchoolId,
                    TenantId = v.TenantId,
                    CourseSectionId = v.CourseSectionId,
                    CourseId = v.CourseId,
                    IsDropped = v.IsDropped,
                    DurationStartDate = v.DurationStartDate,
                    DurationEndDate = v.DurationEndDate,
                    StaffId = v.StaffId,
                    MeetingDays = v.MeetingDays,
                    CourseSectionName = v.CourseSectionName,
                    CourseSection = v.CourseSection,
                    StaffMaster = new StaffMaster()
                    {
                        SchoolId = v.StaffMaster.SchoolId,
                        TenantId = v.StaffMaster.TenantId,
                        StaffId = v.StaffMaster.StaffId,
                        FirstGivenName = v.StaffMaster.FirstGivenName,
                        MiddleName = v.StaffMaster.MiddleName,
                        LastFamilyName = v.StaffMaster.LastFamilyName,
                        StaffInternalId = v.StaffMaster.StaffInternalId,
                        Profile = v.StaffMaster.Profile,
                        JobTitle = v.StaffMaster.JobTitle,
                        SchoolEmail = v.StaffMaster.SchoolEmail,
                        MobilePhone = v.StaffMaster.MobilePhone
                    }
                });

                if (staffCourseSectionDataList.Count() > 0)
                {                
                    
                    foreach (var staffCourseSectionData in staffCourseSectionDataList.ToList())
                    {
                        if (staffCourseSectionData.CourseSection.ScheduleType == "Fixed Schedule (1)")
                        {
                            courseFixedSchedule = this.context?.CourseFixedSchedule.Include(c => c.BlockPeriod).FirstOrDefault(x => x.TenantId == staffCourseSectionData.TenantId && x.SchoolId == staffCourseSectionData.SchoolId && x.CourseSectionId == staffCourseSectionData.CourseSectionId);                            
                        }
                        
                        if (staffCourseSectionData.CourseSection.ScheduleType == "Variable Schedule (2)")
                        {
                            CourseVariableSchedule = this.context?.CourseVariableSchedule.Include(c => c.BlockPeriod).Where(x => x.TenantId == staffCourseSectionData.TenantId && x.SchoolId == staffCourseSectionData.SchoolId && x.CourseSectionId == staffCourseSectionData.CourseSectionId).ToList();
                            
                        }
                        
                        if (staffCourseSectionData.CourseSection.ScheduleType == "Calendar Schedule (3)")
                        {
                            courseCalendarSchedule = this.context?.CourseCalendarSchedule.Include(c => c.BlockPeriod).Where(x => x.TenantId == staffCourseSectionData.TenantId && x.SchoolId == staffCourseSectionData.SchoolId && x.CourseSectionId == staffCourseSectionData.CourseSectionId).ToList();                            
                        }
                        
                        if (staffCourseSectionData.CourseSection.ScheduleType == "Block Schedule (4)")
                        {
                            CourseBlockSchedule = this.context?.CourseBlockSchedule.Include(c => c.BlockPeriod).Where(x => x.TenantId == staffCourseSectionData.TenantId && x.SchoolId == staffCourseSectionData.SchoolId && x.CourseSectionId == staffCourseSectionData.CourseSectionId).ToList();                            
                        }

                        if (staffCourseSectionData.CourseSection.ScheduleType == "Fixed Schedule (1)" || staffCourseSectionData.CourseSection.ScheduleType == "Variable Schedule (2)")
                        {

                            DateTime start = (DateTime)pageResult.DobStartDate;
                            DateTime end = (DateTime)pageResult.DobEndDate;

                            string[] meetingDays = staffCourseSectionData.MeetingDays.Split("|");

                            bool allDays = meetingDays == null || !meetingDays.Any();

                            var dateList = Enumerable.Range(0, 1 + end.Subtract(start).Days)
                                                  .Select(offset => start.AddDays(offset))
                                                  .Where(d => allDays || meetingDays.Contains(d.DayOfWeek.ToString()))
                                                  .ToList();

                            var newDateList = dateList.Where(s => dateList.Any(secL => s.Date > staffCourseSectionData.DurationStartDate && s.Date < staffCourseSectionData.DurationEndDate)).ToList();

                            foreach (var date in newDateList)
                            {
                                CourseSectionViewList CourseSections = new CourseSectionViewList();

                                var staffAttendanceData = this.context?.StudentAttendance.Where(b => b.SchoolId == staffCourseSectionData.SchoolId && b.TenantId == staffCourseSectionData.TenantId /*&& b.StaffId == staffCourseSectionData.StaffId*/ && b.AttendanceDate == date && b.CourseSectionId == staffCourseSectionData.CourseSectionId);

                                if (staffAttendanceData.Count() == 0)
                                {
                                    CourseSections.AttendanceDate = date;
                                    CourseSections.CourseId = staffCourseSectionData.CourseId;
                                    CourseSections.CourseSectionId = staffCourseSectionData.CourseSectionId;
                                    CourseSections.CourseSectionName = staffCourseSectionData.CourseSectionName;
                                    CourseSections.StaffFirstGivenName = staffCourseSectionData.StaffMaster.FirstGivenName;
                                    CourseSections.StaffMiddleName = staffCourseSectionData.StaffMaster.MiddleName;
                                    CourseSections.StaffLastFamilyName = staffCourseSectionData.StaffMaster.LastFamilyName;
                                    CourseSections.AttendanceCategoryId = staffCourseSectionData.CourseSection.AttendanceCategoryId != null ? staffCourseSectionData.CourseSection.AttendanceCategoryId : null;
                                    

                                    if (staffCourseSectionData.CourseSection.ScheduleType == "Fixed Schedule (1)")
                                    {
                                        CourseSections.ScheduleType = "Fixed Schedule";

                                        if (courseFixedSchedule != null)
                                        {
                                            CourseSections.PeriodTitle = (courseFixedSchedule.BlockPeriod != null) ? courseFixedSchedule.BlockPeriod.PeriodTitle : null;
                                            CourseSections.BlockId = courseFixedSchedule.BlockId != null ? courseFixedSchedule.BlockId : null;
                                            CourseSections.PeriodId = courseFixedSchedule.PeriodId != null ? courseFixedSchedule.PeriodId : null;
                                            CourseSections.AttendanceTaken = staffCourseSectionData.CourseSection.AttendanceTaken;
                                        }
                                    }
                                    if (staffCourseSectionData.CourseSection.ScheduleType == "Variable Schedule (2)")
                                    {
                                        CourseSections.ScheduleType = "Variable Schedule";

                                        if (CourseVariableSchedule.Count > 0)
                                        {
                                            var courseVariableScheduleData = CourseVariableSchedule.FirstOrDefault(e => e.Day.ToLower().Contains(date.DayOfWeek.ToString().ToLower()));

                                            if (courseVariableScheduleData != null)
                                            {
                                                CourseSections.PeriodTitle = (courseVariableScheduleData.BlockPeriod != null) ? courseVariableScheduleData.BlockPeriod.PeriodTitle : null;
                                                CourseSections.BlockId = courseVariableScheduleData.BlockId != null ? courseVariableScheduleData.BlockId : null;
                                                CourseSections.PeriodId = courseVariableScheduleData.PeriodId != null ? courseVariableScheduleData.PeriodId : null;
                                                CourseSections.AttendanceTaken = courseVariableScheduleData.TakeAttendance;
                                            }
                                        }
                                    }
                                    //if (staffCourseSectionData.CourseSection.ScheduleType == "Calendar Schedule (3)")
                                    //{
                                    //    CourseSections.ScheduleType = "Calendar Schedule";

                                    //    if (courseCalendarSchedule.Count > 0)
                                    //    {
                                    //        var data = courseCalendarSchedule.Where(e => e.Date == date).ToList();
                                    //        CourseSections.courseCalendarSchedule = data;
                                    //    }
                                    //}
                                    //if (staffCourseSectionData.CourseSection.ScheduleType == "Block Schedule (4)")
                                    //{
                                    //    CourseSections.ScheduleType = "Block Schedule";

                                    //    //var courseBlockScheduleData = this.context?.CourseBlockSchedule.Include(c => c.BlockPeriod).Where(x => x.TenantId == staffCourseSectionData.TenantId && x.SchoolId == staffCourseSectionData.SchoolId && x.CourseSectionId == staffCourseSectionData.CourseSectionId).ToList();

                                    //    if (CourseBlockSchedule.Count > 0)
                                    //    {
                                    //        var data = CourseBlockSchedule.Where(e => e.Date == date).ToList();
                                    //        CourseSections.courseBlockSchedule = data;
                                    //    }
                                    //}
                                    
                                    staffCoursesectionSchedule.Add(CourseSections);
                                }
                                //staffCoursesectionSchedule.Add(CourseSections);
                            }
                            
                        }
                        else
                        {
                            if (courseCalendarSchedule.Count > 0)
                            {
                                var dateList = courseCalendarSchedule.Where(c => c.Date >= pageResult.DobStartDate && c.Date <= pageResult.DobEndDate).Select(c => c.Date);

                                if (dateList.ToList().Count > 0)
                                {
                                    foreach (var date in dateList)
                                    {
                                        CourseSectionViewList CourseSection = new CourseSectionViewList();

                                        var staffAttendanceData = this.context?.StudentAttendance.Where(b => b.SchoolId == staffCourseSectionData.SchoolId && b.TenantId == staffCourseSectionData.TenantId /*&& b.StaffId == staffCourseSectionData.StaffId*/ && b.AttendanceDate == date && b.CourseSectionId == staffCourseSectionData.CourseSectionId);

                                        if (staffAttendanceData.Count() == 0)
                                        {
                                            CourseSection.AttendanceDate = (DateTime)date;
                                            CourseSection.CourseId = staffCourseSectionData.CourseId;
                                            CourseSection.CourseSectionId = staffCourseSectionData.CourseSectionId;
                                            CourseSection.CourseSectionName = staffCourseSectionData.CourseSectionName;
                                            CourseSection.StaffFirstGivenName = staffCourseSectionData.StaffMaster.FirstGivenName;
                                            CourseSection.StaffMiddleName = staffCourseSectionData.StaffMaster.MiddleName;
                                            CourseSection.StaffLastFamilyName = staffCourseSectionData.StaffMaster.LastFamilyName;
                                            CourseSection.AttendanceCategoryId = staffCourseSectionData.CourseSection.AttendanceCategoryId != null ? staffCourseSectionData.CourseSection.AttendanceCategoryId : null;

                                            var courseCalendarScheduleData = courseCalendarSchedule.FirstOrDefault(e => e.Date == date);

                                            if (courseCalendarScheduleData != null)
                                            {
                                                CourseSection.PeriodTitle = (courseCalendarScheduleData != null) ? courseCalendarScheduleData.BlockPeriod.PeriodTitle : null;
                                                CourseSection.BlockId = (courseCalendarScheduleData.BlockId != null) ? courseCalendarScheduleData.BlockId : null;
                                                CourseSection.PeriodId = courseCalendarScheduleData.PeriodId != null ? courseCalendarScheduleData.PeriodId : null;
                                                CourseSection.AttendanceTaken = courseCalendarScheduleData.TakeAttendance;
                                            }
                                            staffCoursesectionSchedule.Add(CourseSection);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (pageResult.FilterParams == null || pageResult.FilterParams.Count == 0)
                {
                    transactionIQ = staffCoursesectionSchedule.AsQueryable();
                }
                else
                {
                    if (pageResult.FilterParams != null && pageResult.FilterParams.ElementAt(0).ColumnName == null && pageResult.FilterParams.Count == 1)
                    {
                        string Columnvalue = pageResult.FilterParams.ElementAt(0).FilterValue;

                        transactionIQ = staffCoursesectionSchedule.Where(x => x.StaffFirstGivenName != null && x.StaffFirstGivenName.ToLower().Contains(Columnvalue.ToLower()) || x.StaffMiddleName != null && x.StaffMiddleName.ToLower().Contains(Columnvalue.ToLower()) || x.StaffLastFamilyName != null && x.StaffLastFamilyName.ToLower().Contains(Columnvalue.ToLower()) || x.CourseSectionName != null && x.CourseSectionName.ToLower().Contains(Columnvalue.ToLower()) || x.AttendanceDate != null && x.AttendanceDate.Date.ToString("yyyy-MM-dd").Contains(Columnvalue) || x.PeriodTitle.ToLower().Contains(Columnvalue.ToLower())).AsQueryable();
                    }
                }
                transactionIQ = transactionIQ.Distinct();

                if (pageResult.SortingModel != null)
                {
                    transactionIQ = Utility.Sort(transactionIQ, pageResult.SortingModel.SortColumn, pageResult.SortingModel.SortDirection.ToLower());
                }

                int totalCount = transactionIQ.Count();

                if (pageResult.PageNumber > 0 && pageResult.PageSize > 0)
                {
                    transactionIQ = transactionIQ.Skip((pageResult.PageNumber - 1) * pageResult.PageSize).Take(pageResult.PageSize);
                }

                scheduledCourseSectionView.courseSectionViewList = transactionIQ.ToList();
                scheduledCourseSectionView.MissingAttendanceCount = totalCount;
                scheduledCourseSectionView._pageSize = pageResult.PageSize;
                scheduledCourseSectionView.PageNumber = pageResult.PageNumber;
                scheduledCourseSectionView.TenantId = pageResult.TenantId;
                scheduledCourseSectionView.SchoolId = pageResult.SchoolId;
                scheduledCourseSectionView.StaffId = pageResult.StaffId;
                scheduledCourseSectionView._failure = false;
                scheduledCourseSectionView._tenantName = pageResult._tenantName;
                scheduledCourseSectionView._token = pageResult._token;
                scheduledCourseSectionView._userName = pageResult._userName;
            }
            catch (Exception es)
            {
                scheduledCourseSectionView._failure = true;
                scheduledCourseSectionView._message = es.Message;
            }
            return scheduledCourseSectionView;
        }

        /// <summary>
        /// Get All Student Attendance List Administration
        /// </summary>
        /// <param name="pageResult"></param>
        /// <returns></returns>
        public StudentAttendanceListViewModel GetAllStudentAttendanceListForAdministration(PageResult pageResult)
        {
            StudentAttendanceListViewModel studentAttendanceList = new StudentAttendanceListViewModel();
            studentAttendanceList.TenantId = pageResult.TenantId;
            studentAttendanceList.SchoolId = pageResult.SchoolId;
            studentAttendanceList._userName = pageResult._userName;
            studentAttendanceList._tenantName = pageResult._tenantName;
            studentAttendanceList._userName = pageResult._userName;
            IQueryable<StudendAttendanceAdministrationViewModel> transactionIQ = null;
           List< StudendAttendanceAdministrationViewModel> attendanceData = new List<StudendAttendanceAdministrationViewModel>();
            try
            {

                //var attendanceData = this.context?.StudentAttendance.Include(s => s.StudentAttendanceComments).Include(s => s.BlockPeriod).Include(s => s.AttendanceCodeNavigation).Include(s => s.StudentCoursesectionSchedule).ThenInclude(s => s.StudentMaster).ThenInclude(s => s.StudentEnrollment).Include(s => s.StudentCoursesectionSchedule.StudentMaster.Sections).Where(x => x.TenantId == pageResult.TenantId && x.SchoolId == pageResult.SchoolId && x.AttendanceDate == pageResult.AttendanceDate && (pageResult.AttendanceCode == null || x.AttendanceCode == pageResult.AttendanceCode)).Select(e => new StudendAttendanceAdministrationViewModel
                //{

                //    TenantId = e.StudentCoursesectionSchedule.StudentMaster.TenantId,
                //    SchoolId = e.StudentCoursesectionSchedule.StudentMaster.SchoolId,
                //    StudentId = e.StudentCoursesectionSchedule.StudentMaster.StudentId,
                //    StudentGuid = e.StudentCoursesectionSchedule.StudentMaster.StudentGuid,
                //    StudentInternalId = e.StudentCoursesectionSchedule.StudentMaster.StudentInternalId,
                //    FirstGivenName = e.StudentCoursesectionSchedule.StudentMaster.FirstGivenName,
                //    MiddleName = e.StudentCoursesectionSchedule.StudentMaster.MiddleName,
                //    LastFamilyName = e.StudentCoursesectionSchedule.StudentMaster.LastFamilyName,
                //    GradeLevelTitle = e.StudentCoursesectionSchedule.StudentMaster.StudentEnrollment.Where(x => x.IsActive == true).Select(s => s.GradeLevelTitle).FirstOrDefault(),
                //    Section = e.StudentCoursesectionSchedule.StudentMaster.Sections.Name,
                //    //StudentAttendanceComments = e.StudentAttendanceComments.ToList(),
                //    Attendance = e.AttendanceCodeNavigation.StateCode,
                //    AttendanceCode = e.AttendanceCodeNavigation.AttendanceCode1,
                //    //studentAttendance=e.s
                //});

                var studentAttendanceData = this.context?.StudentAttendance.Include(s => s.StudentAttendanceComments).Include(s => s.BlockPeriod).Include(s => s.AttendanceCodeNavigation).Include(s => s.StudentCoursesectionSchedule).ThenInclude(s => s.StudentMaster).ThenInclude(s => s.StudentEnrollment).Include(s => s.StudentCoursesectionSchedule.StudentMaster.Sections).Where(x => x.TenantId == pageResult.TenantId && x.SchoolId == pageResult.SchoolId && x.AttendanceDate == pageResult.AttendanceDate && (pageResult.AttendanceCode == null || x.AttendanceCode == pageResult.AttendanceCode)).ToList();

                var studentIds = studentAttendanceData.Select(a => a.StudentId).Distinct().ToList();

                foreach(var ide in studentIds)
                {
                    StudendAttendanceAdministrationViewModel administrationViewModel = new StudendAttendanceAdministrationViewModel();
                   var studentAttendance = studentAttendanceData.Where(x => x.StudentId == ide);

                   var attendance = studentAttendance.FirstOrDefault();
                    administrationViewModel.TenantId = attendance.TenantId;
                        administrationViewModel.SchoolId = attendance.SchoolId;
                        administrationViewModel.StudentId = attendance.StudentId;
                    administrationViewModel.StudentInternalId = attendance.StudentCoursesectionSchedule.StudentMaster.StudentInternalId;
                    administrationViewModel.StudentGuid = attendance.StudentCoursesectionSchedule.StudentMaster.StudentGuid;
                  administrationViewModel.FirstGivenName = attendance.StudentCoursesectionSchedule.StudentMaster.FirstGivenName;
                    administrationViewModel.MiddleName = attendance.StudentCoursesectionSchedule.StudentMaster.MiddleName;
                    administrationViewModel.LastFamilyName = attendance.StudentCoursesectionSchedule.StudentMaster.LastFamilyName;
                    administrationViewModel.GradeLevelTitle = attendance.StudentCoursesectionSchedule.StudentMaster.StudentEnrollment.Where(x => x.IsActive == true).Select(s => s.GradeLevelTitle).FirstOrDefault();
                        administrationViewModel.Section = attendance.StudentCoursesectionSchedule.StudentMaster.Sections.Name;
                    //administrationViewModel.AttendanceCode = attendance.AttendanceCodeNavigation.AttendanceCode1;
                    //administrationViewModel.Attendance = attendance.AttendanceCodeNavigation.StateCode;
                    studentAttendance.ToList().ForEach(x => { x.BlockPeriod.StudentAttendance = null; x.AttendanceCodeNavigation.StudentAttendance = null; x.StudentCoursesectionSchedule = null; });
                    administrationViewModel.studentAttendanceList = studentAttendance.ToList();

                    attendanceData.Add(administrationViewModel);
                }
                
                if (attendanceData.Count() > 0)
                {
                    if (pageResult.FilterParams == null || pageResult.FilterParams.Count == 0)
                    {
                        transactionIQ = attendanceData.AsQueryable();
                    }
                    else
                    {
                        string Columnvalue = pageResult.FilterParams.ElementAt(0).FilterValue;

                        if (pageResult.FilterParams != null && pageResult.FilterParams.ElementAt(0).ColumnName == null && pageResult.FilterParams.Count == 1)
                        {
                            transactionIQ = attendanceData.Where(x => x.FirstGivenName != null && x.FirstGivenName.ToLower().Contains(Columnvalue.ToLower()) || x.MiddleName != null && x.MiddleName.ToLower().Contains(Columnvalue.ToLower()) || x.LastFamilyName != null && x.LastFamilyName.ToLower().Contains(Columnvalue.ToLower()) || x.StudentInternalId != null && x.StudentInternalId.ToLower().Contains(Columnvalue.ToLower()) ||
                            x.GradeLevelTitle != null && x.GradeLevelTitle.ToLower().Contains(Columnvalue.ToLower()) ||
                            x.Section != null && x.Section.ToLower().Contains(Columnvalue.ToLower())).AsQueryable();
                        }
                        else
                        {
                            transactionIQ = Utility.FilteredData(pageResult.FilterParams, attendanceData).AsQueryable();
                        }
                    }

                    if (pageResult.SortingModel != null)
                    {
                        switch (pageResult.SortingModel.SortColumn.ToLower())
                        {
                            default:
                                transactionIQ = Utility.Sort(transactionIQ, pageResult.SortingModel.SortColumn, pageResult.SortingModel.SortDirection.ToLower());
                                break;
                        }
                    }

                    if (transactionIQ != null)
                    {
                        int? totalCount = transactionIQ.Count();
                        if (pageResult.PageNumber > 0 && pageResult.PageSize > 0)
                        {
                            transactionIQ = transactionIQ.Skip((pageResult.PageNumber - 1) * pageResult.PageSize).Take(pageResult.PageSize);
                            studentAttendanceList.PageNumber = pageResult.PageNumber;
                            studentAttendanceList._pageSize = pageResult.PageSize;
                        }
                        studentAttendanceList.studendAttendanceAdministrationList = transactionIQ.ToList();
                        studentAttendanceList.TotalCount = totalCount;
                    }
                    else
                    {
                        studentAttendanceList.TotalCount = 0;
                    }
                }
                else
                {
                    studentAttendanceList._failure = true;
                    studentAttendanceList._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {
                studentAttendanceList._failure = true;
                studentAttendanceList._message = es.Message;
            }
            return studentAttendanceList;
        }

        public CourseSectionForAttendanceViewModel CourseSectionListForAttendanceAdministration(CourseSectionForAttendanceViewModel courseSectionForAttendanceViewModel)
        {
            CourseSectionForAttendanceViewModel courseSectionList = new CourseSectionForAttendanceViewModel();
            var courseSections = new List<CourseSection>();
            var courseSectionViewList = new List<CourseSectionViewList>();
           
            courseSectionList.TenantId = courseSectionForAttendanceViewModel.TenantId;
            courseSectionList.SchoolId = courseSectionForAttendanceViewModel.SchoolId;
            courseSectionList._tenantName = courseSectionForAttendanceViewModel._tenantName;
            courseSectionList._userName = courseSectionForAttendanceViewModel._userName;
            try
            {
                //var studentScheduleData = this.context?.AllCourseSectionView.
                //                    Join(this.context?.StudentCoursesectionSchedule,
                //                    cs => cs.CourseSectionId, scs => scs.CourseSectionId,
                //                    (cs, scs) => new { cs, scs }).Where(e => e.cs.TenantId == courseSectionForAttendanceViewModel.TenantId && e.cs.SchoolId == courseSectionForAttendanceViewModel.SchoolId && e.cs.AcademicYear== courseSectionForAttendanceViewModel.AcademicYear&& e.cs.AttendanceTaken==true&& e.scs.TenantId == courseSectionForAttendanceViewModel.TenantId && e.scs.SchoolId == courseSectionForAttendanceViewModel.SchoolId && e.scs.AcademicYear == courseSectionForAttendanceViewModel.AcademicYear);

                var studentScheduleData = this.context?.CourseSection.Include(s => s.Course).Include(d => d.StudentCoursesectionSchedule).Where(e => e.TenantId == courseSectionForAttendanceViewModel.TenantId && e.SchoolId == courseSectionForAttendanceViewModel.SchoolId && e.AcademicYear == courseSectionForAttendanceViewModel.AcademicYear);

                if (studentScheduleData!=null)
                {
                    var courseSectionData = studentScheduleData.Where(x => x.StudentCoursesectionSchedule.Count > 0);
                    if(courseSectionData!=null)
                    {
                        foreach(var courseSection in courseSectionData.ToList())
                        {
                            var courseSectionView = new CourseSectionViewList();
                          
                            courseSectionView.CourseSectionName = courseSection.CourseSectionName;                         

                            if (courseSection.ScheduleType == "Fixed Schedule (1)")
                            {
                                var checkAttendance = this.context?.CourseSection.Where(x => x.TenantId == courseSectionForAttendanceViewModel.TenantId && x.SchoolId == courseSectionForAttendanceViewModel.SchoolId && x.CourseSectionId == courseSection.CourseSectionId && x.AttendanceTaken == true && x.ScheduleType == "Fixed Schedule (1)");
                                if (checkAttendance != null)
                                {
                                    var fixedScheduleData = this.context?.CourseFixedSchedule.FirstOrDefault(x => x.TenantId == courseSectionForAttendanceViewModel.TenantId && x.SchoolId == courseSectionForAttendanceViewModel.SchoolId && x.CourseSectionId == courseSection.CourseSectionId);
                                    //courseSections.Add(courseSection);
                                    courseSectionView.courseFixedSchedule = fixedScheduleData;
                                }
                            }
                            if (courseSection.ScheduleType == "Variable Schedule (2)")
                            {
                                var variableScheduleData = this.context?.CourseVariableSchedule.Where(x => x.TenantId == courseSectionForAttendanceViewModel.TenantId && x.SchoolId == courseSectionForAttendanceViewModel.SchoolId && x.CourseSectionId == courseSection.CourseSectionId && x.TakeAttendance == true);
                                if(variableScheduleData != null)
                                {
                                   // courseSections.Add(courseSection);
                                    courseSectionView.courseVariableSchedule = variableScheduleData.ToList();
                                }
                            }
                            if (courseSection.ScheduleType == "Calendar Schedule (3)")
                            {
                                var calendarScheduleData = this.context?.CourseCalendarSchedule.Where(x => x.TenantId == courseSectionForAttendanceViewModel.TenantId && x.SchoolId == courseSectionForAttendanceViewModel.SchoolId && x.CourseSectionId == courseSection.CourseSectionId && x.TakeAttendance == true);
                                if (calendarScheduleData != null)
                                {
                                    //courseSections.Add(courseSection);
                                    courseSectionView.courseCalendarSchedule = calendarScheduleData.ToList();
                                }
                            }
                            courseSectionViewList.Add(courseSectionView);
                        }
                        //courseSectionList.courseSectionsList = courseSections.ToList();
                        courseSectionList.courseSectionViewLists = courseSectionViewList;
                    }
                    
                }
                else
                {
                    courseSectionList._failure = true;
                    courseSectionList._message = NORECORDFOUND;
                }
            }
            catch(Exception es)
            {
                courseSectionList._failure = true;
                courseSectionList._message = es.Message;
            }
            return courseSectionList;
        }

        public StudentAttendanceListViewModel UpdateStudentsAttendanceForAdministration(StudentAttendanceListViewModel studentAttendanceListViewModel)
        {
           
            try
            {
                if(studentAttendanceListViewModel.studendAttendanceAdministrationList.Count>0)
                {
                    foreach (var studendAttendanceAdministration in studentAttendanceListViewModel.studendAttendanceAdministrationList)
                    {
                        int? StudentAttendanceId = 1;

                        var studentAttendanceData = this.context?.StudentAttendance.Where(x => x.SchoolId == studentAttendanceListViewModel.SchoolId && x.TenantId == studentAttendanceListViewModel.TenantId).OrderByDescending(x => x.StudentAttendanceId).FirstOrDefault();

                        if (studentAttendanceData != null)
                        {
                            StudentAttendanceId = studentAttendanceData.StudentAttendanceId + 1;
                        }

                        long? CommentId = Utility.GetMaxLongPK(this.context, new Func<StudentAttendanceComments, long>(x => x.CommentId));

                        foreach (var studentAttendances in studendAttendanceAdministration.studentAttendanceList)
                        {
                            studentAttendances.StudentAttendanceId =(int) StudentAttendanceId;
                            if (studentAttendances.StudentAttendanceComments.Count() > 0)
                            {
                                foreach (var StudentAttendanceComment in studentAttendances.StudentAttendanceComments)
                                {
                                    StudentAttendanceComment.CommentId = (long)CommentId;
                                    CommentId++;
                                }
                            }
                            StudentAttendanceId++;
                        }
                    }
                    foreach (var studendAttendanceAdministration in studentAttendanceListViewModel.studendAttendanceAdministrationList)
                    {
                        
                        foreach (var studentAttendances in studendAttendanceAdministration.studentAttendanceList)
                        {
                            var studentAttendanceData = this.context?.StudentAttendance.Include(s=>s.StudentAttendanceComments).Where(x => x.TenantId == studentAttendanceListViewModel.TenantId && x.SchoolId == studentAttendanceListViewModel.SchoolId && x.StudentId== studendAttendanceAdministration.StudentId&& x.AttendanceDate== studentAttendanceListViewModel.AttendanceDate).ToList();

                            if (studentAttendanceData.Count > 0)
                            {
                                var studentAttendanceIDs = studentAttendanceData.Select(v => v.StudentAttendanceId).ToList();

                                var studentAttendanceCommentData = this.context?.StudentAttendanceComments.Where(x => x.TenantId == studentAttendanceListViewModel.TenantId && x.SchoolId == studentAttendanceListViewModel.SchoolId && (studentAttendanceIDs == null || (studentAttendanceIDs.Contains(x.StudentAttendanceId))));
                                this.context?.StudentAttendanceComments.RemoveRange(studentAttendanceCommentData);
                            }
                            this.context?.StudentAttendance.RemoveRange(studentAttendanceData);
                            this.context?.SaveChanges();

                            this.context.StudentAttendance.Add(studentAttendances);
                            this.context.StudentAttendanceComments.AddRange(studentAttendances.StudentAttendanceComments);
                                                    
                        }
                    }
                    this.context?.SaveChanges();
                    studentAttendanceListViewModel._message = "Student Attendance updated succsesfully.";
                }
                else
                {
                    studentAttendanceListViewModel._failure = true;
                    studentAttendanceListViewModel._message = "Please Select Student";
                }
            }
            catch (Exception es)
            {
                studentAttendanceListViewModel._failure = true;
                studentAttendanceListViewModel._message = es.Message;
            }
            return studentAttendanceListViewModel;
        }

    }
}