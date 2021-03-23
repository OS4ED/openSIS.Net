using Microsoft.EntityFrameworkCore;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.CourseManager;
using opensis.data.ViewModels.StaffSchedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace opensis.data.Repository
{
    public class StaffScheduleRepository : IStaffScheduleRepository
    {
        private CRMContext context;
        private static readonly string NORECORDFOUND = "No Record Found";
        public StaffScheduleRepository(IDbContextFactory dbContextFactory)
        {
            this.context = dbContextFactory.Create();
        }

        /// <summary>
        /// Teacher Schedule View For CourseSection
        /// </summary>
        /// <param name="staffScheduleViewModel"></param>
        /// <returns></returns>
        public StaffScheduleViewModel StaffScheduleViewForCourseSection(StaffScheduleViewModel staffScheduleViewModel)
        {
            StaffScheduleViewModel staffScheduleList = new StaffScheduleViewModel();
            try
            {
                if (staffScheduleViewModel.staffScheduleViewList.Count() > 0)
                {

                    foreach (StaffScheduleViewList teacherScheduleView in staffScheduleViewModel.staffScheduleViewList.ToList())
                    {
                        StaffScheduleViewList teacherSchedules = new StaffScheduleViewList();

                        var staffData = this.context?.StaffMaster.FirstOrDefault(x => x.TenantId == staffScheduleViewModel.TenantId && x.SchoolId == staffScheduleViewModel.SchoolId && x.StaffId == teacherScheduleView.StaffId);

                        if (staffData != null)
                        {
                            var courseSectionList = staffScheduleViewModel.courseSectionViewList.ToList();
                            foreach (var getCourseSection in courseSectionList)
                            {
                                string concatDay = null;

                                var courseSectionData = this.context?.AllCourseSectionView.Where(x => x.TenantId == staffScheduleViewModel.TenantId && x.SchoolId == staffScheduleViewModel.SchoolId && x.CourseSectionId == getCourseSection.CourseSectionId);

                                if (courseSectionData != null)
                                {
                                    CourseSectionViewList CourseSections = new CourseSectionViewList();

                                    var variableScheduleData = courseSectionData.Where(x => x.TenantId == staffScheduleViewModel.TenantId && x.SchoolId == staffScheduleViewModel.SchoolId && x.CourseSectionId == getCourseSection.CourseSectionId && x.ScheduleType == "Variable Schedule (2)").ToList();

                                    if (variableScheduleData.Count > 0)
                                    {
                                        CourseSections.ScheduleType = "Variable Schedule";
                                        foreach (var variableSchedule in variableScheduleData)
                                        {
                                            concatDay = concatDay != null ? concatDay + "|" + variableSchedule.VarDay.Substring(0, 3) : variableSchedule.VarDay.Substring(0, 3);

                                            var variableList = this.context.CourseVariableSchedule.Include(x => x.BlockPeriod).Include(x => x.Rooms).Where(x => x.TenantId == variableSchedule.TenantId && x.SchoolId == variableSchedule.SchoolId && x.CourseSectionId == variableSchedule.CourseSectionId).Select(s => new CourseVariableSchedule { TenantId = s.TenantId, SchoolId = s.SchoolId, CourseId = s.CourseId, CourseSectionId = s.CourseSectionId, GradeScaleId = s.GradeScaleId, Serial = s.Serial, Day = s.Day, RoomId = s.RoomId, TakeAttendance = s.TakeAttendance, PeriodId = s.PeriodId, BlockId = s.BlockId, CreatedBy = s.CreatedBy, CreatedOn = s.CreatedOn, UpdatedBy = s.UpdatedBy, UpdatedOn = s.UpdatedOn, Rooms = new Rooms { Title = s.Rooms.Title }, BlockPeriod = new BlockPeriod { PeriodTitle = s.BlockPeriod.PeriodTitle } }).ToList();

                                            CourseSections.courseVariableSchedule = variableList;
                                        }
                                    }

                                    var fixedScheduleData = courseSectionData.Where(x => x.TenantId == staffScheduleViewModel.TenantId && x.SchoolId == staffScheduleViewModel.SchoolId && x.CourseSectionId == getCourseSection.CourseSectionId && x.ScheduleType == "Fixed Schedule (1)").FirstOrDefault();

                                    if (fixedScheduleData != null)
                                    {
                                        CourseSections.ScheduleType = "Fixed Schedule";
                                        concatDay = fixedScheduleData.FixedDays;

                                        var fixedSchedule = this.context?.CourseFixedSchedule.Include(f => f.Rooms).Include(f => f.BlockPeriod).Where(x => x.TenantId == staffScheduleViewModel.TenantId && x.SchoolId == staffScheduleViewModel.SchoolId && x.CourseId == fixedScheduleData.CourseId && x.CourseSectionId == fixedScheduleData.CourseSectionId).Select(s => new CourseFixedSchedule { TenantId = s.TenantId, SchoolId = s.SchoolId, CourseId = s.CourseId, CourseSectionId = s.CourseSectionId, GradeScaleId = s.GradeScaleId, Serial = s.Serial, RoomId = s.RoomId, PeriodId = s.PeriodId, BlockId = s.BlockId, CreatedBy = s.CreatedBy, CreatedOn = s.CreatedOn, UpdatedBy = s.UpdatedBy, UpdatedOn = s.UpdatedOn, Rooms = new Rooms { Title = s.Rooms.Title }, BlockPeriod = new BlockPeriod { PeriodTitle = s.BlockPeriod.PeriodTitle } }).FirstOrDefault();

                                        CourseSections.courseFixedSchedule = fixedSchedule;

                                    }

                                    var calendarScheduleData = courseSectionData.Where(x => x.TenantId == staffScheduleViewModel.TenantId && x.SchoolId == staffScheduleViewModel.SchoolId && x.CourseSectionId == getCourseSection.CourseSectionId && x.ScheduleType == "Calendar Schedule (3)").ToList();

                                    if (calendarScheduleData.Count > 0)
                                    {
                                        var calData = this.context.SchoolCalendars.FirstOrDefault(x => x.TenantId == staffScheduleViewModel.TenantId && x.SchoolId == staffScheduleViewModel.SchoolId && x.CalenderId == calendarScheduleData.FirstOrDefault().CalendarId);
                                       
                                        CourseSections.WeekDays = calData.Days;

                                        CourseSections.ScheduleType = "Calendar Schedule";
                                        concatDay = "Calendar Days";

                                        foreach (var calendarSchedule in calendarScheduleData)
                                        {
                                            var calendarList = this.context.CourseCalendarSchedule.Include(x => x.BlockPeriod).Include(x => x.Rooms).Where(x => x.TenantId == calendarSchedule.TenantId && x.SchoolId == calendarSchedule.SchoolId && x.CourseSectionId == calendarSchedule.CourseSectionId).Select(s => new CourseCalendarSchedule { TenantId = s.TenantId, SchoolId = s.SchoolId, CourseId = s.CourseId, CourseSectionId = s.CourseSectionId, GradeScaleId = s.GradeScaleId, Serial = s.Serial, Date = s.Date, RoomId = s.RoomId, TakeAttendance = s.TakeAttendance, PeriodId = s.PeriodId, BlockId = s.BlockId, CreatedBy = s.CreatedBy, CreatedOn = s.CreatedOn, UpdatedBy = s.UpdatedBy, UpdatedOn = s.UpdatedOn, Rooms = new Rooms { Title = s.Rooms.Title }, BlockPeriod = new BlockPeriod { PeriodTitle = s.BlockPeriod.PeriodTitle } }).ToList();

                                            CourseSections.courseCalendarSchedule = calendarList;
                                        }
                                    }

                                    var blockScheduleData = courseSectionData.Where(x => x.TenantId == staffScheduleViewModel.TenantId && x.SchoolId == staffScheduleViewModel.SchoolId && x.CourseSectionId == getCourseSection.CourseSectionId && x.ScheduleType == "Block Schedule (4)").ToList();

                                    if (blockScheduleData.Count > 0)
                                    {
                                        CourseSections.ScheduleType = "Block Schedule";
                                        concatDay = "Block Days";
                                        foreach (var blockSchedule in blockScheduleData)
                                        {

                                            var blockScheduleList = this.context.CourseBlockSchedule.Include(x => x.Block).Include(x => x.BlockPeriod).Include(x => x.Rooms).Where(x => x.TenantId == blockSchedule.TenantId && x.SchoolId == blockSchedule.SchoolId && x.CourseSectionId == blockSchedule.CourseSectionId).Select(s => new CourseBlockSchedule { TenantId = s.TenantId, SchoolId = s.SchoolId, CourseId = s.CourseId, CourseSectionId = s.CourseSectionId, GradeScaleId = s.GradeScaleId, Serial = s.Serial, RoomId = s.RoomId, PeriodId = s.PeriodId, BlockId = s.BlockId, TakeAttendance = s.TakeAttendance, CreatedBy = s.CreatedBy, CreatedOn = s.CreatedOn, UpdatedBy = s.UpdatedBy, UpdatedOn = s.UpdatedOn, Rooms = new Rooms { Title = s.Rooms.Title }, BlockPeriod = new BlockPeriod { PeriodTitle = s.BlockPeriod.PeriodTitle }, Block = new Block { BlockTitle = s.Block.BlockTitle } }).ToList();
                                            CourseSections.courseBlockSchedule = blockScheduleList;
                                        }
                                    }

                                    foreach (var courseSection in courseSectionData)
                                    {
                                        CourseSections.CourseId = courseSection.CourseId;
                                        CourseSections.CourseSectionId = courseSection.CourseSectionId;
                                        CourseSections.CourseTitle = courseSection.CourseTitle;
                                        CourseSections.CourseSectionName = courseSection.CourseSectionName;
                                        CourseSections.DurationStartDate = courseSection.DurationStartDate;
                                        CourseSections.DurationEndDate = courseSection.DurationEndDate;
                                        //CourseSections.ScheduleType = courseSection.ScheduleType;
                                        CourseSections.YrMarkingPeriodId = courseSection.YrMarkingPeriodId;
                                        CourseSections.SmstrMarkingPeriodId = courseSection.SmstrMarkingPeriodId;
                                        CourseSections.QtrMarkingPeriodId = courseSection.QtrMarkingPeriodId;
                                        CourseSections.MeetingDays = concatDay.ToString();
                                        teacherSchedules.courseSectionViewList.Add(CourseSections);
                                        break;
                                    }
                                }
                            }

                            teacherSchedules.StaffId = staffData.StaffId;
                            teacherSchedules.StaffFullName = staffData.FirstGivenName + " " + staffData.MiddleName + " " + staffData.LastFamilyName;
                            teacherSchedules.StaffEmail = staffData.LoginEmailAddress != null ? staffData.LoginEmailAddress : staffData.PersonalEmail;
                            teacherSchedules.HomeroomTeacher = staffData.HomeroomTeacher;
                            teacherSchedules.StaffGuid = staffData.StaffGuid;

                            staffScheduleList.staffScheduleViewList.Add(teacherSchedules);
                        }
                    }
                    staffScheduleList.TenantId = staffScheduleViewModel.TenantId;
                    staffScheduleList._tenantName = staffScheduleViewModel._tenantName;
                    staffScheduleList.SchoolId = staffScheduleViewModel.SchoolId;
                    staffScheduleList._token = staffScheduleViewModel._token;
                    staffScheduleList._failure = false;
                }
            }
            catch (Exception es)
            {
                staffScheduleList._failure = true;
                staffScheduleList._message = es.Message;
            }
            return staffScheduleList;
        }

        /// <summary>
        ///  Add Staff CourseSection Schedule
        /// </summary>
        /// <param name="staffScheduleViewModel"></param>
        /// <returns></returns>
        public StaffScheduleViewModel AddStaffCourseSectionSchedule(StaffScheduleViewModel staffScheduleViewModel)
        {

            try
            {
                //List<StaffCoursesectionSchedule> staffCoursesectionScheduleList = new List<StaffCoursesectionSchedule>();
                if (staffScheduleViewModel.staffScheduleViewList.Count() > 0)
                
                {
                    foreach (var staffSchedule in staffScheduleViewModel.staffScheduleViewList.ToList())
                    {
                        var courseSectionList = staffSchedule.courseSectionViewList.ToList();

                        foreach (var CourseSection in courseSectionList)
                        {

                            var staffCoursesectionSchedule = new StaffCoursesectionSchedule()
                            {
                                TenantId = staffScheduleViewModel.TenantId,
                                SchoolId = staffScheduleViewModel.SchoolId,
                                StaffId = staffSchedule.StaffId,
                                CourseId = (int)CourseSection.CourseId,
                                CourseSectionId = (int)CourseSection.CourseSectionId,
                                StaffGuid = (Guid)staffSchedule.StaffGuid,
                                CourseSectionName = CourseSection.CourseSectionName,
                                YrMarkingPeriodId = CourseSection.YrMarkingPeriodId,
                                SmstrMarkingPeriodId = CourseSection.SmstrMarkingPeriodId,
                                QtrMarkingPeriodId = CourseSection.QtrMarkingPeriodId,
                                DurationStartDate = CourseSection.DurationStartDate,
                                DurationEndDate = CourseSection.DurationEndDate,
                                MeetingDays = CourseSection.MeetingDays,
                                CreatedBy = staffScheduleViewModel.CreatedBy,
                                CreatedOn = DateTime.UtcNow
                            };
                            this.context?.StaffCoursesectionSchedule.Add(staffCoursesectionSchedule);
                        }
                    }

                }
                else
                {
                    staffScheduleViewModel._failure = true;
                    staffScheduleViewModel._message = "Select Staff For CourseSection Schedule";
                }
                this.context?.SaveChanges();
                //transaction.Commit();
                staffScheduleViewModel._message = "Staff CourseSection Schedule Added Successfully";
                staffScheduleViewModel._failure = false;
            }
            catch (Exception es)
            {
                staffScheduleViewModel._failure = true;
                staffScheduleViewModel._message = es.Message;
            }
            return staffScheduleViewModel;
        }

        public StaffScheduleViewModel CheckAvailabilityStaffCourseSectionSchedule(StaffScheduleViewModel staffScheduleViewModel)
        {
            try
            {
                if (staffScheduleViewModel.staffScheduleViewList.Count() > 0)
                {
                    foreach (var staff in staffScheduleViewModel.staffScheduleViewList.ToList())
                    {
                        var courseSectionList = staff.courseSectionViewList.ToList();
                        if (courseSectionList.Count() > 0)
                        {

                            foreach (var CourseSection in courseSectionList)
                            {
                                var checkStaffInCourseSection = this.context.StaffCoursesectionSchedule.Where(x => x.TenantId == staffScheduleViewModel.TenantId && x.SchoolId == staffScheduleViewModel.SchoolId && x.StaffId == staff.StaffId && x.CourseId == CourseSection.CourseId && x.CourseSectionId == CourseSection.CourseSectionId).ToList();

                                if (checkStaffInCourseSection.Count() > 0)
                                {
                                    staffScheduleViewModel._failure = true;
                                    staffScheduleViewModel._message = "Staff already exits in course section";
                                    return staffScheduleViewModel;
                                }
                                else
                                {
                                    var checkAllowStaffConflict = this.context.CourseSection.FirstOrDefault(x => x.TenantId == staffScheduleViewModel.TenantId && x.SchoolId == staffScheduleViewModel.SchoolId && x.CourseId == CourseSection.CourseId && x.CourseSectionId == CourseSection.CourseSectionId && x.AllowTeacherConflict == true);

                                    if (checkAllowStaffConflict != null)
                                    {
                                        var staffCoursesectionSchedule = new StaffCoursesectionSchedule()
                                        {
                                            TenantId = staffScheduleViewModel.TenantId,
                                            SchoolId = staffScheduleViewModel.SchoolId,
                                            StaffId = staff.StaffId,
                                            CourseId = (int)CourseSection.CourseId,
                                            CourseSectionId = (int)CourseSection.CourseSectionId,
                                            StaffGuid = (Guid)staff.StaffGuid,
                                            CourseSectionName = CourseSection.CourseSectionName,
                                            YrMarkingPeriodId = CourseSection.YrMarkingPeriodId,
                                            SmstrMarkingPeriodId = CourseSection.SmstrMarkingPeriodId,
                                            QtrMarkingPeriodId = CourseSection.QtrMarkingPeriodId,
                                            DurationStartDate = CourseSection.DurationStartDate,
                                            DurationEndDate = CourseSection.DurationEndDate,
                                            MeetingDays = CourseSection.MeetingDays,
                                            CreatedBy = staffScheduleViewModel.CreatedBy,
                                            CreatedOn = DateTime.UtcNow
                                        };
                                        this.context?.StaffCoursesectionSchedule.Add(staffCoursesectionSchedule);
                                    }
                                    else
                                    {
                                        var courseSectionAllData = this.context?.AllCourseSectionView.Where(c => c.SchoolId == staffScheduleViewModel.SchoolId && c.TenantId == staffScheduleViewModel.TenantId && c.CourseSectionId == CourseSection.CourseSectionId).ToList();

                                        if (courseSectionAllData.Count > 0)
                                        {
                                            foreach (var courseSectionData in courseSectionAllData)
                                            {
                                                var checkForConflict = this.context.AllCourseSectionView.Join(this.context.StaffCoursesectionSchedule, acsv => acsv.CourseSectionId, scss => scss.CourseSectionId, (acsv, scss) => new { acsv, scss }).FirstOrDefault(x => x.acsv.TenantId == staffScheduleViewModel.TenantId && x.acsv.SchoolId == staffScheduleViewModel.SchoolId && x.scss.StaffId == staff.StaffId && (x.acsv.FixedPeriodId != null && x.acsv.FixedPeriodId == courseSectionData.FixedPeriodId) || (x.acsv.VarPeriodId != null && x.acsv.VarPeriodId == courseSectionData.VarPeriodId) || (x.acsv.CalPeriodId != null && x.acsv.CalPeriodId == courseSectionData.CalPeriodId) || (x.acsv.BlockPeriodId != null && x.acsv.BlockPeriodId == courseSectionData.BlockPeriodId));

                                                if (checkForConflict != null)
                                                {
                                                    staffScheduleViewModel._failure = true;
                                                    staffScheduleViewModel._message = "Period and Room Conflict";
                                                    return staffScheduleViewModel;
                                                }
                                            }
                                            var staffCoursesectionSchedule = new StaffCoursesectionSchedule()
                                            {
                                                TenantId = staffScheduleViewModel.TenantId,
                                                SchoolId = staffScheduleViewModel.SchoolId,
                                                StaffId = staff.StaffId,
                                                CourseId = (int)CourseSection.CourseId,
                                                CourseSectionId = (int)CourseSection.CourseSectionId,
                                                StaffGuid = (Guid)staff.StaffGuid,
                                                CourseSectionName = CourseSection.CourseSectionName,
                                                YrMarkingPeriodId = CourseSection.YrMarkingPeriodId,
                                                SmstrMarkingPeriodId = CourseSection.SmstrMarkingPeriodId,
                                                QtrMarkingPeriodId = CourseSection.QtrMarkingPeriodId,
                                                DurationStartDate = CourseSection.DurationStartDate,
                                                DurationEndDate = CourseSection.DurationEndDate,
                                                MeetingDays = CourseSection.MeetingDays,
                                                CreatedBy = staffScheduleViewModel.CreatedBy,
                                                CreatedOn = DateTime.UtcNow
                                            };
                                            this.context?.StaffCoursesectionSchedule.Add(staffCoursesectionSchedule);
                                        }                                    
                                    }
                                }
                            }
                        }
                        else
                        {
                            staffScheduleViewModel._failure = true;
                            staffScheduleViewModel._message = "Select CourseSection For CourseSection Schedule";
                        }
                    }
                }
                else
                {
                    staffScheduleViewModel._failure = true;
                    staffScheduleViewModel._message = "Select Staff For CourseSection Schedule";
                }
                this.context?.SaveChanges();
                staffScheduleViewModel._message = "Staff CourseSection Schedule Added Successfully";
                staffScheduleViewModel._failure = false;
            }
            catch (Exception es)
            {
                staffScheduleViewModel._failure = true;
                staffScheduleViewModel._message = es.Message;
            }
            return staffScheduleViewModel;
        }

    }
}
