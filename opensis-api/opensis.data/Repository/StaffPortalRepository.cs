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
using opensis.data.ViewModels.StaffPortal;
using opensis.data.ViewModels.StaffSchedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace opensis.data.Repository
{
    public class StaffPortalRepository : IStaffPortalRepository
    {
        private CRMContext context;
        private static readonly string NORECORDFOUND = "No Record Found";
        public StaffPortalRepository(IDbContextFactory dbContextFactory)
        {
            this.context = dbContextFactory.Create();
        }

        public ScheduledCourseSectionViewModel MissingAttendanceListForCourseSection(PageResult pageResult)
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
                var staffCourseSectionScheduleData = this.context?.StaffCoursesectionSchedule.Include(c => c.CourseSection).FirstOrDefault(e => e.SchoolId == pageResult.SchoolId && e.TenantId == pageResult.TenantId && e.StaffId == pageResult.StaffId && e.CourseSectionId == pageResult.CourseSectionId && e.IsDropped != true);

                if (staffCourseSectionScheduleData != null)
                {


                    if (staffCourseSectionScheduleData.CourseSection.ScheduleType == "Fixed Schedule (1)")
                    {

                        courseFixedSchedule = this.context?.CourseFixedSchedule.Include(c => c.BlockPeriod).FirstOrDefault(x => x.TenantId == staffCourseSectionScheduleData.TenantId && x.SchoolId == staffCourseSectionScheduleData.SchoolId && x.CourseSectionId == staffCourseSectionScheduleData.CourseSectionId);
                    }

                    if (staffCourseSectionScheduleData.CourseSection.ScheduleType == "Variable Schedule (2)")
                    {

                        CourseVariableSchedule = this.context?.CourseVariableSchedule.Include(c => c.BlockPeriod).Where(x => x.TenantId == staffCourseSectionScheduleData.TenantId && x.SchoolId == staffCourseSectionScheduleData.SchoolId && x.CourseSectionId == staffCourseSectionScheduleData.CourseSectionId).ToList();
                    }

                    if (staffCourseSectionScheduleData.CourseSection.ScheduleType == "Calendar Schedule (3)")
                    {
                        courseCalendarSchedule = this.context?.CourseCalendarSchedule.Include(c => c.BlockPeriod).Where(x => x.TenantId == staffCourseSectionScheduleData.TenantId && x.SchoolId == staffCourseSectionScheduleData.SchoolId && x.CourseSectionId == staffCourseSectionScheduleData.CourseSectionId).ToList();
                    }

                    if (staffCourseSectionScheduleData.CourseSection.ScheduleType == "Block Schedule (4)")
                    {

                        CourseBlockSchedule = this.context?.CourseBlockSchedule.Include(c => c.BlockPeriod).Where(x => x.TenantId == staffCourseSectionScheduleData.TenantId && x.SchoolId == staffCourseSectionScheduleData.SchoolId && x.CourseSectionId == staffCourseSectionScheduleData.CourseSectionId).ToList();
                    }



                    if (staffCourseSectionScheduleData.CourseSection.ScheduleType == "Fixed Schedule (1)" || staffCourseSectionScheduleData.CourseSection.ScheduleType == "Variable Schedule (2)")
                    {

                        DateTime start = (DateTime)staffCourseSectionScheduleData.DurationStartDate;
                        DateTime end = (DateTime)staffCourseSectionScheduleData.DurationEndDate;

                        string[] meetingDays = staffCourseSectionScheduleData.MeetingDays.Split("|");

                        bool allDays = meetingDays == null || !meetingDays.Any();

                        var dateList = Enumerable.Range(0, 1 + end.Subtract(start).Days)
                                              .Select(offset => start.AddDays(offset))
                                              .Where(d => allDays || meetingDays.Contains(d.DayOfWeek.ToString()))
                                              .ToList();

                        foreach (var date in dateList)
                        {
                            CourseSectionViewList CourseSections = new CourseSectionViewList();

                            var staffAttendanceData = this.context?.StudentAttendance.Where(b => b.SchoolId == pageResult.SchoolId && b.TenantId == pageResult.TenantId /*&& b.StaffId == staffCourseSectionScheduleData.StaffId*/ && b.AttendanceDate == date && b.CourseSectionId == pageResult.CourseSectionId);

                            if (staffAttendanceData.Count() == 0)
                            {
                                CourseSections.AttendanceDate = date;
                                CourseSections.CourseId = staffCourseSectionScheduleData.CourseId;
                                CourseSections.CourseSectionId = staffCourseSectionScheduleData.CourseSectionId;
                                CourseSections.CourseSectionName = staffCourseSectionScheduleData.CourseSectionName;

                                if (staffCourseSectionScheduleData.CourseSection.ScheduleType == "Fixed Schedule (1)")
                                {
                                    CourseSections.ScheduleType = "Fixed Schedule";

                                    if (courseFixedSchedule != null)
                                    {
                                        //CourseSections.courseFixedSchedule = courseFixedSchedule;
                                        CourseSections.PeriodTitle = (courseFixedSchedule.BlockPeriod != null) ? courseFixedSchedule.BlockPeriod.PeriodTitle : null;
                                    }
                                }
                                if (staffCourseSectionScheduleData.CourseSection.ScheduleType == "Variable Schedule (2)")
                                {
                                    CourseSections.ScheduleType = "Variable Schedule";

                                    if (CourseVariableSchedule.Count > 0)
                                    {
                                        var courseVariableScheduleData = CourseVariableSchedule.FirstOrDefault(e => e.Day.ToLower().Contains(date.DayOfWeek.ToString().ToLower()));
                                        //CourseSections.courseVariableSchedule = data;

                                        if (courseVariableScheduleData != null)
                                        {
                                            CourseSections.PeriodTitle = (courseVariableScheduleData.BlockPeriod != null) ? courseVariableScheduleData.BlockPeriod.PeriodTitle : null;
                                        }
                                    }
                                }
                                staffCoursesectionSchedule.Add(CourseSections);
                            }                            
                        }
                    }
                    else
                    {
                        if (courseCalendarSchedule.Count > 0)
                        {
                            var dateList = courseCalendarSchedule.Select(c => c.Date);

                            if (dateList.ToList().Count > 0)
                            {
                                foreach (var date in dateList)
                                {
                                    CourseSectionViewList CourseSection = new CourseSectionViewList();

                                    var staffAttendanceData = this.context?.StudentAttendance.Where(b => b.SchoolId == staffCourseSectionScheduleData.SchoolId && b.TenantId == staffCourseSectionScheduleData.TenantId && b.StaffId == staffCourseSectionScheduleData.StaffId && b.AttendanceDate == date && b.CourseSectionId == staffCourseSectionScheduleData.CourseSectionId);

                                    if (staffAttendanceData.Count() == 0)
                                    {
                                        CourseSection.AttendanceDate = (DateTime)date;
                                        CourseSection.CourseId = staffCourseSectionScheduleData.CourseId;
                                        CourseSection.CourseSectionId = staffCourseSectionScheduleData.CourseSectionId;
                                        CourseSection.CourseSectionName = staffCourseSectionScheduleData.CourseSectionName;
                                        var courseCalendarScheduleData = courseCalendarSchedule.FirstOrDefault(e => e.Date == date);

                                        if (courseCalendarScheduleData != null)
                                        {
                                            CourseSection.PeriodTitle = (courseCalendarScheduleData != null) ? courseCalendarScheduleData.BlockPeriod.PeriodTitle : null;
                                        }
                                        staffCoursesectionSchedule.Add(CourseSection);
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

                        transactionIQ = staffCoursesectionSchedule.Where(x => x.CourseSectionName != null && x.CourseSectionName.ToLower().Contains(Columnvalue.ToLower()) || x.AttendanceDate != null && x.AttendanceDate.Date.ToString("yyyy-MM-dd").Contains(Columnvalue) || x.PeriodTitle.ToLower().Contains(Columnvalue.ToLower())).AsQueryable();
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
                scheduledCourseSectionView.CourseSectionId = pageResult.CourseSectionId;
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
        /// Update Online Class Room URL & Password In CourseSection
        /// </summary>
        /// <param name="courseSectionUpdateViewModel"></param>
        /// <returns></returns>
        public CourseSectionUpdateViewModel UpdateOnlineClassRoomURLInCourseSection(CourseSectionUpdateViewModel courseSectionUpdateViewModel)
        {
            try
            {
                var courseSectionData = this.context.CourseSection.FirstOrDefault(x => x.TenantId == courseSectionUpdateViewModel.courseSection.TenantId && x.SchoolId == courseSectionUpdateViewModel.courseSection.SchoolId && x.CourseId == courseSectionUpdateViewModel.courseSection.CourseId && x.CourseSectionId == courseSectionUpdateViewModel.courseSection.CourseSectionId);
                if (courseSectionData!=null)
                {
                    courseSectionData.OnlineClassroomUrl = courseSectionUpdateViewModel.courseSection.OnlineClassroomUrl;
                    courseSectionData.OnlineClassroomPassword = courseSectionUpdateViewModel.courseSection.OnlineClassroomPassword;
                    courseSectionData.UpdatedBy = courseSectionUpdateViewModel.courseSection.UpdatedBy;
                    courseSectionData.UpdatedOn = DateTime.UtcNow;
                    this.context.SaveChanges();
                    courseSectionUpdateViewModel._failure = false;
                    courseSectionUpdateViewModel._message = "Updated Successfully";
                }
                else
                {
                    courseSectionUpdateViewModel._failure = true;
                    courseSectionUpdateViewModel._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {
                courseSectionUpdateViewModel._failure = true;
                courseSectionUpdateViewModel._message = es.Message;
            }
            return courseSectionUpdateViewModel;
        }
    }
}
