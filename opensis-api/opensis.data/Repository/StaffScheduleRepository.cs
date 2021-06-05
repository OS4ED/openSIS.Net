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
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.CourseManager;
using opensis.data.ViewModels.StaffSchedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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

                                    var calender = this.context.SchoolCalendars.FirstOrDefault(x => x.TenantId == staffScheduleViewModel.TenantId && x.SchoolId == staffScheduleViewModel.SchoolId && x.CalenderId == courseSectionData.FirstOrDefault().CalendarId);

                                    if(calender != null)
                                    {
                                        CourseSections.WeekDays = calender.Days;
                                    }

                                    var staffSchedule = this.context.StaffCoursesectionSchedule.Include(x => x.StaffMaster).Where(x => x.TenantId == staffScheduleViewModel.TenantId && x.SchoolId == staffScheduleViewModel.SchoolId && x.CourseSectionId == getCourseSection.CourseSectionId && x.IsDropped!=false).ToList();

                                    if (staffSchedule.Count > 0)
                                    {
                                        foreach (var staff in staffSchedule)
                                        {
                                            var staffName = staff.StaffMaster.FirstGivenName + " " + staff.StaffMaster.MiddleName + " " + staff.StaffMaster.LastFamilyName;
                                            CourseSections.ScheduledStaff = CourseSections.ScheduledStaff != null ? CourseSections.ScheduledStaff + "|" + staffName : staffName;
                                        }
                                    }

                                    var variableScheduleData = courseSectionData.Where(x => x.TenantId == staffScheduleViewModel.TenantId && x.SchoolId == staffScheduleViewModel.SchoolId && x.CourseSectionId == getCourseSection.CourseSectionId && x.ScheduleType == "Variable Schedule (2)").ToList();

                                    if (variableScheduleData.Count > 0)
                                    {
                                        CourseSections.ScheduleType = "Variable Schedule";
                                        foreach (var variableSchedule in variableScheduleData)
                                        {
                                            concatDay = concatDay != null ? concatDay + "|" + variableSchedule.VarDay : variableSchedule.VarDay;

                                            var variableList = this.context.CourseVariableSchedule.Include(x => x.BlockPeriod).Include(x => x.Rooms).Where(x => x.TenantId == variableSchedule.TenantId && x.SchoolId == variableSchedule.SchoolId && x.CourseSectionId == variableSchedule.CourseSectionId).Select(s => new CourseVariableSchedule { TenantId = s.TenantId, SchoolId = s.SchoolId, CourseId = s.CourseId, CourseSectionId = s.CourseSectionId, GradeScaleId = s.GradeScaleId, Serial = s.Serial, Day = s.Day, RoomId = s.RoomId, TakeAttendance = s.TakeAttendance, PeriodId = s.PeriodId, BlockId = s.BlockId, CreatedBy = s.CreatedBy, CreatedOn = s.CreatedOn, UpdatedBy = s.UpdatedBy, UpdatedOn = s.UpdatedOn, Rooms = new Rooms { Title = s.Rooms.Title }, BlockPeriod = new BlockPeriod { PeriodTitle = s.BlockPeriod.PeriodTitle } }).ToList();

                                            CourseSections.courseVariableSchedule = variableList;
                                        }
                                    }

                                    var fixedScheduleData = courseSectionData.Where(x => x.TenantId == staffScheduleViewModel.TenantId && x.SchoolId == staffScheduleViewModel.SchoolId && x.CourseSectionId == getCourseSection.CourseSectionId && x.ScheduleType == "Fixed Schedule (1)").FirstOrDefault();

                                    if (fixedScheduleData != null)
                                    {
                                        CourseSections.TakeAttendanceForFixedSchedule = fixedScheduleData.AttendanceTaken;
                                        CourseSections.ScheduleType = "Fixed Schedule";
                                        concatDay = fixedScheduleData.FixedDays;

                                        var fixedSchedule = this.context?.CourseFixedSchedule.Include(f => f.Rooms).Include(f => f.BlockPeriod).Where(x => x.TenantId == staffScheduleViewModel.TenantId && x.SchoolId == staffScheduleViewModel.SchoolId && x.CourseId == fixedScheduleData.CourseId && x.CourseSectionId == fixedScheduleData.CourseSectionId).Select(s => new CourseFixedSchedule { TenantId = s.TenantId, SchoolId = s.SchoolId, CourseId = s.CourseId, CourseSectionId = s.CourseSectionId, GradeScaleId = s.GradeScaleId, Serial = s.Serial, RoomId = s.RoomId, PeriodId = s.PeriodId, BlockId = s.BlockId, CreatedBy = s.CreatedBy, CreatedOn = s.CreatedOn, UpdatedBy = s.UpdatedBy, UpdatedOn = s.UpdatedOn, Rooms = new Rooms { Title = s.Rooms.Title }, BlockPeriod = new BlockPeriod { PeriodTitle = s.BlockPeriod.PeriodTitle } }).FirstOrDefault();

                                        CourseSections.courseFixedSchedule = fixedSchedule;

                                    }

                                    var calendarScheduleData = courseSectionData.Where(x => x.TenantId == staffScheduleViewModel.TenantId && x.SchoolId == staffScheduleViewModel.SchoolId && x.CourseSectionId == getCourseSection.CourseSectionId && x.ScheduleType == "Calendar Schedule (3)").ToList();

                                    if (calendarScheduleData.Count > 0)
                                    {
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


                                    var courseSection = courseSectionData.FirstOrDefault();

                                    CourseSections.CourseId = courseSection.CourseId;
                                    CourseSections.CourseSectionId = courseSection.CourseSectionId;
                                    CourseSections.CourseTitle = courseSection.CourseTitle;
                                    CourseSections.CourseSectionName = courseSection.CourseSectionName;
                                    CourseSections.DurationStartDate = courseSection.DurationStartDate;
                                    CourseSections.DurationEndDate = courseSection.DurationEndDate;
                                    CourseSections.YrMarkingPeriodId = courseSection.YrMarkingPeriodId;
                                    CourseSections.SmstrMarkingPeriodId = courseSection.SmstrMarkingPeriodId;
                                    CourseSections.QtrMarkingPeriodId = courseSection.QtrMarkingPeriodId;
                                    CourseSections.MeetingDays = concatDay;
                                    teacherSchedules.courseSectionViewList.Add(CourseSections);
                                }
                            }

                            teacherSchedules.StaffId = staffData.StaffId;
                            teacherSchedules.StaffInternalId = staffData.StaffInternalId;
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
                staffScheduleList.staffScheduleViewList = null;
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
                if (staffScheduleViewModel.staffScheduleViewList.Count() > 0)
                
                {
                    foreach (var staffSchedule in staffScheduleViewModel.staffScheduleViewList.ToList())
                    {
                        var courseSectionList = staffSchedule.courseSectionViewList.ToList();

                        if (courseSectionList.Count > 0)
                        {
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
                                    CreatedOn = DateTime.UtcNow,
                                    IsAssigned=true
                                };
                                this.context?.StaffCoursesectionSchedule.Add(staffCoursesectionSchedule);
                            }
                            this.context?.SaveChanges();
                            staffScheduleViewModel._message = "Teacher scheduled successfully";
                            staffScheduleViewModel._failure = false;
                        }
                        else
                        {
                            staffScheduleViewModel._failure = true;
                            staffScheduleViewModel._message = "Select CourseSection For Teacher scheduled ";
                        }
                    }
                }
                else
                {
                    staffScheduleViewModel._failure = true;
                    staffScheduleViewModel._message = "Select Staff For Teacher scheduled ";
                }               
            }
            catch (Exception es)
            {
                staffScheduleViewModel._failure = true;
                staffScheduleViewModel._message = es.Message;
            }
            return staffScheduleViewModel;
        }

        /// <summary>
        /// Check Availability Staff CourseSection Schedule
        /// </summary>
        /// <param name="staffScheduleViewModel"></param>
        /// <returns></returns>
        public StaffScheduleViewModel CheckAvailabilityStaffCourseSectionSchedule(StaffScheduleViewModel staffScheduleViewModel)
        {
            try
            {
                if (staffScheduleViewModel.staffScheduleViewList.Count() > 0)
                {
                    staffScheduleViewModel._message = "No Conflict Detected";
                    List<int> conflictInInputCourseSection = new List<int>();

                    var courseSectionListCheck = staffScheduleViewModel.staffScheduleViewList.FirstOrDefault().courseSectionViewList.ToList();

                    if (courseSectionListCheck.Count() > 0)
                    {
                        //This Block For Checking Period,Day Conflict Between Input Course Sections
                        foreach (var courseSectionCheck in courseSectionListCheck)
                        {
                            var courseSectionCheckData = this.context?.AllCourseSectionView.Where(c => c.SchoolId == staffScheduleViewModel.SchoolId && c.TenantId == staffScheduleViewModel.TenantId && c.CourseSectionId == courseSectionCheck.CourseSectionId).ToList();

                            var courseSectionIds = courseSectionListCheck.Where(x => x.CourseSectionId != courseSectionCheck.CourseSectionId).Select(s => s.CourseSectionId);
                            if (courseSectionIds.Count() > 0)
                            {
                                var courseSectionInputData = this.context?.AllCourseSectionView.Where(c => c.SchoolId == staffScheduleViewModel.SchoolId && c.TenantId == staffScheduleViewModel.TenantId && courseSectionIds.Contains(c.CourseSectionId)).ToList();

                                foreach (var courseSec in courseSectionCheckData)
                                {
                                    courseSectionInputData = courseSectionInputData.Where(x => x.TenantId == courseSec.TenantId && x.SchoolId == courseSec.SchoolId &&
                                      ((courseSec.FixedPeriodId != null && ((courseSec.FixedPeriodId == x.FixedPeriodId && (Regex.IsMatch(x.FixedDays.ToLower(), courseSec.FixedDays.ToLower(), RegexOptions.IgnoreCase))) || (courseSec.FixedPeriodId == x.VarPeriodId && courseSec.FixedDays.ToLower().Contains(x.VarDay.ToLower())) || (courseSec.FixedPeriodId == x.CalPeriodId && courseSec.FixedDays.ToLower().Contains(x.CalDay.ToLower())))) ||

                                 (courseSec.VarPeriodId != null && ((courseSec.VarPeriodId == x.FixedPeriodId && x.FixedDays.ToLower().Contains(courseSec.VarDay.ToLower())) || (courseSec.VarPeriodId == x.VarPeriodId && courseSec.VarDay.ToLower() == x.VarDay.ToLower()) || (courseSec.VarPeriodId == x.CalPeriodId && courseSec.VarDay.ToLower() == x.CalDay.ToLower()))) ||

                                 (courseSec.CalPeriodId != null && ((courseSec.CalPeriodId == x.FixedPeriodId && x.FixedDays.ToLower().Contains(courseSec.CalDay.ToLower())) || (courseSec.CalPeriodId == x.VarPeriodId && courseSec.CalDay.ToLower() == x.VarDay.ToLower()) || (courseSec.CalPeriodId == x.CalPeriodId && courseSec.CalDay.ToLower() == x.CalDay.ToLower())))) && courseSec.DurationEndDate > x.DurationStartDate).ToList();

                                    if (courseSectionInputData.Count() > 0)
                                    {
                                        var ids = courseSectionInputData.Select(s => s.CourseSectionId).Distinct().ToList();
                                        conflictInInputCourseSection.Add(ids.First());
                                    }
                                }
                            }
                        }
                    } //end of input course section checking

                    foreach (var staff in staffScheduleViewModel.staffScheduleViewList.ToList())
                    {
                        var courseSectionList = staff.courseSectionViewList.ToList();

                        if (courseSectionList.Count() > 0)
                        {
                            foreach (var courseSection in courseSectionList)
                            {
                                var check = conflictInInputCourseSection.Contains((int)courseSection.CourseSectionId);
                                if (check == true) //checking this courseSection is in conflict Input CourseSection or not
                                {
                                    staffScheduleViewModel._failure = true;
                                    //staffScheduleViewModel._message = "Staff already exits in course section";
                                    staffScheduleViewModel._message = "Conflict Detected";
                                    staff.ConflictStaff = true;
                                    courseSection.ConflictCourseSection = true;
                                }
                                else
                                {
                                    var checkStaffInCourseSection = this.context.StaffCoursesectionSchedule.Where(x => x.TenantId == staffScheduleViewModel.TenantId && x.SchoolId == staffScheduleViewModel.SchoolId && x.StaffId == staff.StaffId && x.CourseId == courseSection.CourseId && x.CourseSectionId == courseSection.CourseSectionId && x.IsAssigned == true && x.IsDropped != true).ToList();

                                    if (checkStaffInCourseSection.Count() > 0)
                                    {
                                        staffScheduleViewModel._failure = true;
                                        //staffScheduleViewModel._message = "Staff already exits in course section";
                                        staffScheduleViewModel._message = "Conflict Detected";
                                        staff.ConflictStaff = true;
                                        courseSection.ConflictCourseSection = true;

                                    }
                                    else
                                    {
                                        var courseSectionAllData = this.context?.AllCourseSectionView.Where(c => c.SchoolId == staffScheduleViewModel.SchoolId && c.TenantId == staffScheduleViewModel.TenantId && c.CourseSectionId == courseSection.CourseSectionId).ToList();

                                        if (courseSectionAllData.FirstOrDefault().AllowTeacherConflict != true)
                                        {
                                            if (courseSectionAllData.Count > 0)
                                            {
                                                foreach (var courseSectionData in courseSectionAllData)
                                                {
                                                    var checkForConflict = this.context.AllCourseSectionView.Join(this.context.StaffCoursesectionSchedule, acsv => acsv.CourseSectionId, scss => scss.CourseSectionId, (acsv, scss) => new { acsv, scss }).AsEnumerable().Where(c => c.acsv.TenantId == staffScheduleViewModel.TenantId && c.acsv.SchoolId == staffScheduleViewModel.SchoolId && c.scss.StaffId == staff.StaffId &&

                                    ((c.acsv.FixedPeriodId != null && ((c.acsv.FixedPeriodId == courseSectionData.FixedPeriodId && (Regex.IsMatch(courseSectionData.FixedDays.ToLower(), c.acsv.FixedDays.ToLower(), RegexOptions.IgnoreCase))) || (c.acsv.FixedPeriodId == courseSectionData.VarPeriodId && c.acsv.FixedDays.ToLower().Contains(courseSectionData.VarDay.ToLower())) || (c.acsv.FixedPeriodId == courseSectionData.CalPeriodId && c.acsv.FixedDays.ToLower().Contains(courseSectionData.CalDay.ToLower())))) ||

                                    (c.acsv.VarPeriodId != null && ((c.acsv.VarPeriodId == courseSectionData.FixedPeriodId && courseSectionData.FixedDays.ToLower().Contains(c.acsv.VarDay.ToLower())) || (c.acsv.VarPeriodId == courseSectionData.VarPeriodId && c.acsv.VarDay.ToLower() == courseSectionData.VarDay.ToLower()) || (c.acsv.VarPeriodId == courseSectionData.CalPeriodId && c.acsv.VarDay.ToLower() == courseSectionData.CalDay.ToLower()))) ||

                                    (c.acsv.CalPeriodId != null && ((c.acsv.CalPeriodId == courseSectionData.FixedPeriodId && courseSectionData.FixedDays.ToLower().Contains(c.acsv.CalDay.ToLower())) || (c.acsv.CalPeriodId == courseSectionData.VarPeriodId && c.acsv.CalDay.ToLower() == courseSectionData.VarDay.ToLower()) || (c.acsv.CalPeriodId == courseSectionData.CalPeriodId && c.acsv.CalDay.ToLower() == courseSectionData.CalDay.ToLower())))) && c.acsv.DurationEndDate > courseSectionData.DurationStartDate).ToList();

                                                    if (checkForConflict.Count() > 0)
                                                    {
                                                        staffScheduleViewModel._failure = true;
                                                        //staffScheduleViewModel._message = "Period and Room Conflict";
                                                        staffScheduleViewModel._message = "Conflict Detected";
                                                        staff.ConflictStaff = true;
                                                        courseSection.ConflictCourseSection = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            staffScheduleViewModel._failure = true;
                            staffScheduleViewModel._message = "Select CourseSection For Teacher scheduled";
                            return staffScheduleViewModel;
                        }
                    }
                }
                else
                {
                    staffScheduleViewModel._failure = true;
                    staffScheduleViewModel._message = "Select Staff For Teacher scheduled";
                    return staffScheduleViewModel;
                }
            }
            catch (Exception es)
            {
                staffScheduleViewModel._failure = true;
                staffScheduleViewModel._message = es.Message;
            }
            return staffScheduleViewModel;
        }

        public ScheduledCourseSectionViewModel GetAllScheduledCourseSectionForStaff(ScheduledCourseSectionViewModel scheduledCourseSectionViewModel)
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
                    List<int> csIds = new List<int> { };
                    csIds = scheduledCourseSectionData.Select(x => x.CourseSectionId).ToList();

                    var scheduledStaffDataForCourseSection = this.context?.StaffCoursesectionSchedule.Include(x => x.StaffMaster).Where(x => x.TenantId == scheduledCourseSectionViewModel.TenantId && x.SchoolId == scheduledCourseSectionViewModel.SchoolId && x.StaffId != scheduledCourseSectionViewModel.StaffId && (csIds == null || (csIds.Contains(x.CourseSectionId))) && x.IsDropped != true).ToList();

                    foreach (var scheduledCourseSection in scheduledCourseSectionData)
                    {
                        CourseSectionViewList CourseSections = new CourseSectionViewList();

                        //var scheduledStaffData = this.context?.StaffCoursesectionSchedule.Include(x => x.StaffMaster).Where(x => x.TenantId == scheduledCourseSectionViewModel.TenantId && x.SchoolId == scheduledCourseSectionViewModel.SchoolId && x.StaffId != scheduledCourseSection.StaffId&& x.CourseSectionId== scheduledCourseSection.CourseSectionId && x.IsDropped != true).ToList();

                        var scheduledStaffData = scheduledStaffDataForCourseSection.Where(x => x.TenantId == scheduledCourseSectionViewModel.TenantId && x.SchoolId == scheduledCourseSectionViewModel.SchoolId && x.CourseSectionId == scheduledCourseSection.CourseSectionId).ToList();


                        if (scheduledStaffData.Count() > 0)
                        {
                            foreach (var scheduledStaff in scheduledStaffData)
                            {
                                var staffName = scheduledStaff.StaffMaster.FirstGivenName + " " + scheduledStaff.StaffMaster.MiddleName + " " + scheduledStaff.StaffMaster.LastFamilyName;
                                CourseSections.ScheduledStaff = CourseSections.ScheduledStaff != null ? CourseSections.ScheduledStaff + "|" + staffName : staffName;
                            }
                        }

                        CourseSections.WeekDays = scheduledCourseSection.CourseSection.SchoolCalendars.Days;
                        CourseSections.CourseTitle = scheduledCourseSection.CourseSection.Course.CourseTitle;

                        if (scheduledCourseSection.CourseSection.ScheduleType == "Fixed Schedule (1)")
                        {
                            CourseSections.ScheduleType = "Fixed Schedule";                            
                        }
                        if (scheduledCourseSection.CourseSection.ScheduleType == "Variable Schedule (2)")
                        {
                            CourseSections.ScheduleType = "Variable Schedule";                            
                        }
                        if (scheduledCourseSection.CourseSection.ScheduleType == "Calendar Schedule (3)")
                        {
                            CourseSections.ScheduleType = "Calendar Schedule";                           
                        }
                        if (scheduledCourseSection.CourseSection.ScheduleType == "Block Schedule (4)")
                        {
                            CourseSections.ScheduleType = "Block Schedule";                           
                        }
                        CourseSections.CalendarId = scheduledCourseSection.CourseSection.CalendarId;
                        CourseSections.CourseId = scheduledCourseSection.CourseId;
                        CourseSections.CourseSectionId = scheduledCourseSection.CourseSectionId;
                        CourseSections.GradeScaleId = scheduledCourseSection.CourseSection.GradeScaleId;
                        CourseSections.StandardGradeScaleId = scheduledCourseSection.CourseSection.StandardGradeScaleId;
                        CourseSections.CourseSectionName = scheduledCourseSection.CourseSectionName;
                        CourseSections.YrMarkingPeriodId = scheduledCourseSection.YrMarkingPeriodId;
                        CourseSections.SmstrMarkingPeriodId = scheduledCourseSection.SmstrMarkingPeriodId;
                        CourseSections.QtrMarkingPeriodId = scheduledCourseSection.QtrMarkingPeriodId;
                        CourseSections.DurationStartDate = scheduledCourseSection.DurationStartDate;
                        CourseSections.DurationEndDate = scheduledCourseSection.DurationEndDate;
                        CourseSections.MeetingDays = scheduledCourseSection.MeetingDays;
                        CourseSections.GradeScaleType = scheduledCourseSection.CourseSection.GradeScaleType;
                        CourseSections.UsedStandard = scheduledCourseSection.CourseSection.UseStandards;

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
        /// Add Staff CourseSection ReSchedule
        /// </summary>
        /// <param name="staffScheduleViewModel"></param>
        /// <returns></returns>
        public StaffScheduleViewModel AddStaffCourseSectionReSchedule(StaffScheduleViewModel staffScheduleViewModel)
        {
            try
            {
                if (staffScheduleViewModel.staffScheduleViewList.Count() > 0)
                {
                    foreach (var staffSchedule in staffScheduleViewModel.staffScheduleViewList.ToList())
                    {
                        var courseSectionList = staffSchedule.courseSectionViewList.ToList();

                        if (courseSectionList.Count > 0)
                        {
                            var staffGuid = this.context.StaffMaster.Where(x => x.StaffId == staffScheduleViewModel.staffScheduleViewList.FirstOrDefault().StaffId && x.SchoolId == staffScheduleViewModel.SchoolId && x.TenantId == staffScheduleViewModel.TenantId).FirstOrDefault().StaffGuid;

                            foreach (var courseSection in courseSectionList)
                            {
                                var StaffScheduleData = this.context?.StaffCoursesectionSchedule.FirstOrDefault(x => x.TenantId == staffScheduleViewModel.TenantId && x.SchoolId == staffScheduleViewModel.SchoolId && x.StaffId == staffScheduleViewModel.ExistingStaff && x.CourseSectionId == courseSection.CourseSectionId);

                                if (StaffScheduleData != null)
                                {
                                    StaffScheduleData.IsDropped = true;
                                    StaffScheduleData.EffectiveDropDate = DateTime.UtcNow;
                                }
                                else
                                {
                                    staffScheduleViewModel._failure = true;
                                    staffScheduleViewModel._message = "Exixting Staff Does't in this CourseSection";
                                    return staffScheduleViewModel;
                                }

                                var staffCoursesectionSchedule = new StaffCoursesectionSchedule()
                                {
                                    TenantId = staffScheduleViewModel.TenantId,
                                    SchoolId = staffScheduleViewModel.SchoolId,
                                    StaffId = staffSchedule.StaffId,
                                    CourseId = (int)courseSection.CourseId,
                                    CourseSectionId = (int)courseSection.CourseSectionId,
                                    StaffGuid = staffGuid,
                                    CourseSectionName = courseSection.CourseSectionName,
                                    YrMarkingPeriodId = courseSection.YrMarkingPeriodId,
                                    SmstrMarkingPeriodId = courseSection.SmstrMarkingPeriodId,
                                    QtrMarkingPeriodId = courseSection.QtrMarkingPeriodId,
                                    DurationStartDate = courseSection.DurationStartDate,
                                    DurationEndDate = courseSection.DurationEndDate,
                                    MeetingDays = courseSection.MeetingDays,
                                    CreatedBy = staffScheduleViewModel.CreatedBy,
                                    CreatedOn = DateTime.UtcNow,
                                    IsAssigned = true
                                };
                                this.context?.StaffCoursesectionSchedule.Add(staffCoursesectionSchedule);
                            }
                            this.context?.SaveChanges();
                            staffScheduleViewModel._message = "Staff  Re-Schedule In CourseSection Successfully";
                            staffScheduleViewModel._failure = false;
                        }
                        else
                        {
                            staffScheduleViewModel._failure = true;
                            staffScheduleViewModel._message = "Select CourseSection For Staff  Re-Schedule";
                        }
                    }
                }
                else
                {
                    staffScheduleViewModel._failure = true;
                    staffScheduleViewModel._message = "Select Staff For Staff  Re-Schedule";
                }
            }
            catch (Exception es)
            {
                staffScheduleViewModel._failure = true;
                staffScheduleViewModel._message = es.Message;
            }
            return staffScheduleViewModel;
        }

        public StaffListViewModel checkAvailabilityStaffCourseSectionReSchedule(StaffListViewModel staffListViewModel)
        {
            try
            {
                if (staffListViewModel.courseSectionsList.Count() > 0)
                {
                    staffListViewModel._message = "No Conflict Detected";

                    foreach (var courseSection in staffListViewModel.courseSectionsList.ToList())
                    {
                        if(courseSection.StaffCoursesectionSchedule.Count() > 0)
                        {
                            var checkStaffInCourseSection = this.context.StaffCoursesectionSchedule.Where(x => x.TenantId == staffListViewModel.TenantId && x.SchoolId == staffListViewModel.SchoolId && x.StaffId == staffListViewModel.ReScheduleStaffId
                                                    && x.CourseId == courseSection.CourseId && x.CourseSectionId == courseSection.CourseSectionId && x.IsAssigned == true && x.IsDropped != true).ToList();

                            if (checkStaffInCourseSection.Count() > 0)
                            {
                                staffListViewModel._failure = true;
                                staffListViewModel._message = "Conflict Detected";
                                var indexValue = staffListViewModel.courseSectionsList.FindIndex(x => x.CourseSectionId == courseSection.CourseSectionId);
                                staffListViewModel.ConflictIndexNo = staffListViewModel.ConflictIndexNo != null ? staffListViewModel.ConflictIndexNo + "," + indexValue.ToString() : indexValue.ToString();

                            }
                            else
                            {
                                var checkAllowStaffConflict = this.context.CourseSection.FirstOrDefault(x => x.TenantId == staffListViewModel.TenantId && x.SchoolId == staffListViewModel.SchoolId && x.CourseId == courseSection.CourseId && x.CourseSectionId == courseSection.CourseSectionId && x.AllowTeacherConflict == true);


                                if (checkAllowStaffConflict == null)
                                {
                                    var courseSectionAllData = this.context?.AllCourseSectionView.Where(c => c.SchoolId == staffListViewModel.SchoolId && c.TenantId == staffListViewModel.TenantId && c.CourseSectionId == courseSection.CourseSectionId).ToList();

                                    if (courseSectionAllData.Count > 0)
                                    {
                                        foreach (var courseSectionData in courseSectionAllData)
                                        {
                                            var checkForConflict = this.context.AllCourseSectionView.Join(this.context.StaffCoursesectionSchedule, acsv => acsv.CourseSectionId, scss => scss.CourseSectionId, (acsv, scss) => new { acsv, scss }).AsEnumerable().Where(c => c.acsv.TenantId == staffListViewModel.TenantId && c.acsv.SchoolId == staffListViewModel.SchoolId && c.scss.StaffId == staffListViewModel.ReScheduleStaffId &&

                            ((c.acsv.FixedPeriodId != null && ((c.acsv.FixedPeriodId == courseSectionData.FixedPeriodId && (Regex.IsMatch(courseSectionData.FixedDays.ToLower(), c.acsv.FixedDays.ToLower(), RegexOptions.IgnoreCase))) || (c.acsv.FixedPeriodId == courseSectionData.VarPeriodId && c.acsv.FixedDays.ToLower().Contains(courseSectionData.VarDay.ToLower())) || (c.acsv.FixedPeriodId == courseSectionData.CalPeriodId && c.acsv.FixedDays.ToLower().Contains(courseSectionData.CalDay.ToLower())))) ||

                            (c.acsv.VarPeriodId != null && ((c.acsv.VarPeriodId == courseSectionData.FixedPeriodId && courseSectionData.FixedDays.ToLower().Contains(c.acsv.VarDay.ToLower())) || (c.acsv.VarPeriodId == courseSectionData.VarPeriodId && c.acsv.VarDay.ToLower() == courseSectionData.VarDay.ToLower()) || (c.acsv.VarPeriodId == courseSectionData.CalPeriodId && c.acsv.VarDay.ToLower() == courseSectionData.CalDay.ToLower()))) ||

                            (c.acsv.CalPeriodId != null && ((c.acsv.CalPeriodId == courseSectionData.FixedPeriodId && courseSectionData.FixedDays.ToLower().Contains(c.acsv.CalDay.ToLower())) || (c.acsv.CalPeriodId == courseSectionData.VarPeriodId && c.acsv.CalDay.ToLower() == courseSectionData.VarDay.ToLower()) || (c.acsv.CalPeriodId == courseSectionData.CalPeriodId && c.acsv.CalDay.ToLower() == courseSectionData.CalDay.ToLower())))) && c.acsv.DurationEndDate > courseSectionData.DurationStartDate).ToList();

                                            if (checkForConflict.Count() > 0)
                                            {
                                                staffListViewModel._failure = true;
                                                staffListViewModel._message = "Conflict Detected";
                                                var indexValue = staffListViewModel.courseSectionsList.FindIndex(x => x.CourseSectionId == courseSection.CourseSectionId);
                                                staffListViewModel.ConflictIndexNo = staffListViewModel.ConflictIndexNo != null ? staffListViewModel.ConflictIndexNo + "," + indexValue.ToString() : indexValue.ToString();

                                            }
                                        }
                                    }
                                }
                            }
                        }
                        
                    }
                }
                else
                {
                    staffListViewModel._failure = true;
                    staffListViewModel._message = "Select CourseSection For StaffCourseSection Re-Schedule";
                    return staffListViewModel;
                }
            }
            catch (Exception es)
            {
                staffListViewModel._failure = true;
                staffListViewModel._message = es.Message;
            }
            return staffListViewModel;
        }

        /// <summary>
        /// Add Staff CourseSection Re-Schedule By Course Wise
        /// </summary>
        /// <param name="staffListViewModel"></param>
        /// <returns></returns>
        public StaffListViewModel AddStaffCourseSectionReScheduleByCourse(StaffListViewModel staffListViewModel)
        {
            try
            {
                if (staffListViewModel.courseSectionsList.Count() > 0)
                {
                    var staffGuid = this.context.StaffMaster.Where(x => x.StaffId == staffListViewModel.ReScheduleStaffId && x.SchoolId == staffListViewModel.SchoolId && x.TenantId == staffListViewModel.TenantId).FirstOrDefault().StaffGuid;

                    foreach (var courseSection in staffListViewModel.courseSectionsList.ToList())
                    {
                        var StaffScheduleData = this.context.StaffCoursesectionSchedule.FirstOrDefault(x => x.TenantId == staffListViewModel.TenantId && x.SchoolId == staffListViewModel.SchoolId && x.StaffId == courseSection.StaffCoursesectionSchedule.FirstOrDefault().StaffId && x.CourseSectionId == courseSection.CourseSectionId);
                        
                        if (StaffScheduleData != null)
                        {
                            StaffScheduleData.IsDropped = true;
                            StaffScheduleData.EffectiveDropDate = DateTime.UtcNow;
                        }
                        else
                        {
                            staffListViewModel._failure = true;
                            staffListViewModel._message = "Exixting Staff Does't in this CourseSection";
                            return staffListViewModel;
                        }

                        var staffCoursesectionSchedule = new StaffCoursesectionSchedule()
                        {
                            TenantId = (Guid)staffListViewModel.TenantId,
                            SchoolId = (int)staffListViewModel.SchoolId,
                            StaffId = (int)staffListViewModel.ReScheduleStaffId,
                            CourseId = courseSection.CourseId,
                            CourseSectionId = courseSection.CourseSectionId,
                            StaffGuid = staffGuid,
                            CourseSectionName = courseSection.CourseSectionName,
                            YrMarkingPeriodId = courseSection.YrMarkingPeriodId,
                            SmstrMarkingPeriodId = courseSection.SmstrMarkingPeriodId,
                            QtrMarkingPeriodId = courseSection.QtrMarkingPeriodId,
                            DurationStartDate = courseSection.DurationStartDate,
                            DurationEndDate = courseSection.DurationEndDate,
                            MeetingDays = courseSection.StaffCoursesectionSchedule.FirstOrDefault().MeetingDays,
                            CreatedBy = staffListViewModel.CreatedBy,
                            CreatedOn = DateTime.UtcNow,
                            IsAssigned = true
                        };
                        this.context?.StaffCoursesectionSchedule.Add(staffCoursesectionSchedule);
                    }
                    this.context?.SaveChanges();
                    staffListViewModel._message = "Staff  Re-Schedule In CourseSection Successfully";
                    staffListViewModel._failure = false;
                }
                else
                {
                    staffListViewModel._failure = true;
                    staffListViewModel._message = "Select CourseSection For Staff  Re-Schedule";
                }
            }
            catch (Exception es)
            {
                staffListViewModel._failure = true;
                staffListViewModel._message = es.Message;
            }
            return staffListViewModel;
        }

    }
}
