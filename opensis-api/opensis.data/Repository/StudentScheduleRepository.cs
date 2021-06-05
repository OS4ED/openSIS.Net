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
using opensis.data.ViewModels.StudentSchedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace opensis.data.Repository
{
    public class StudentScheduleRepository : IStudentScheduleRepository
    {
        private CRMContext context;
        private static readonly string NORECORDFOUND = "No Record Found";
        public StudentScheduleRepository(IDbContextFactory dbContextFactory)
        {
            this.context = dbContextFactory.Create();
        }

        /// <summary>
        /// Add Student Course Section Schedule
        /// </summary>
        /// <param name="studentCourseSectionScheduleAddViewModel"></param>
        /// <returns></returns>
        public StudentCourseSectionScheduleAddViewModel AddStudentCourseSectionSchedule(StudentCourseSectionScheduleAddViewModel studentCourseSectionScheduleAddViewModel)
        {
            using (var transaction = this.context.Database.BeginTransaction())
            {                
                try
                {
                    string conflictMessage = "All Student Scheduled Successfully";
                    studentCourseSectionScheduleAddViewModel._conflictFailure = false;
                    if (studentCourseSectionScheduleAddViewModel.courseSectionList.Count > 0)
                    {
                        int restSeats = 0;
                        List<StudentMaster> studentData = null;

                        foreach (var courseSection in studentCourseSectionScheduleAddViewModel.courseSectionList)
                        {

                            var studentCourseSectionScheduleData = this.context?.StudentCoursesectionSchedule.Where(c => c.SchoolId == courseSection.SchoolId && c.TenantId == courseSection.TenantId && c.CourseSectionId == courseSection.CourseSectionId && c.AcademicYear == courseSection.AcademicYear).ToList();

                            if (studentCourseSectionScheduleData.Count > 0)
                            {

                                restSeats = (int)courseSection.Seats - studentCourseSectionScheduleData.Count;
                            }
                            else
                            {
                                restSeats = (int)courseSection.Seats;
                            }

                            if (restSeats > 0)
                            {
                                if (studentCourseSectionScheduleAddViewModel.studentMasterList.Count > 0)
                                {
                                    if (restSeats < studentCourseSectionScheduleAddViewModel.studentMasterList.Count)
                                    {
                                        studentData = studentCourseSectionScheduleAddViewModel.studentMasterList.Take(restSeats).ToList();
                                        
                                        if (studentData.Count > 0)
                                        {
                                            var restStudentCount = studentCourseSectionScheduleAddViewModel.studentMasterList.Count - studentData.Count;
                                            
                                            if (restStudentCount > 0)
                                            {
                                                var restStudentList = studentCourseSectionScheduleAddViewModel.studentMasterList.TakeLast(restStudentCount).ToList();
                                                
                                                if (restStudentList.Count>0)
                                                {
                                                    foreach (var restStudent in restStudentList)
                                                    {
                                                        var conflictStudent = new StudentScheduleView()
                                                        {
                                                            TenantId = restStudent.TenantId,
                                                            SchoolId = restStudent.SchoolId,
                                                            StudentId = restStudent.StudentId,
                                                            CourseId = courseSection.CourseId,
                                                            CourseSectionId = courseSection.CourseSectionId,
                                                            CourseSectionName = courseSection.CourseSectionName,
                                                            StudentInternalId = restStudent.StudentInternalId,
                                                            StudentName = restStudent.FirstGivenName + " " + restStudent.MiddleName + " " + restStudent.LastFamilyName,
                                                            Scheduled = false,
                                                            ConflictComment = "Seats Not Avalaible"
                                                        };
                                                        studentCourseSectionScheduleAddViewModel._conflictFailure = true;
                                                        this.context?.StudentScheduleView.Add(conflictStudent);

                                                        if (studentCourseSectionScheduleAddViewModel.studentMasterList.Count > 1)
                                                        {
                                                            conflictMessage = "Some Student could not be scheduled due to conflicts. Please find the detailed report below.";
                                                        }
                                                        else
                                                        {
                                                            conflictMessage = "This Student could not be scheduled due to conflicts.";
                                                        }
                                                    }
                                                }
                                            }
                                        }                                        
                                    }
                                    else
                                    {
                                        studentData = studentCourseSectionScheduleAddViewModel.studentMasterList.ToList();
                                    }

                                    if (studentData.Count > 0)
                                    {
                                        //var studentScheduleViewData = this.context?.StudentScheduleView.Where(e => e.SchoolId == studentCourseSectionScheduleAddViewModel.SchoolId && e.TenantId == studentCourseSectionScheduleAddViewModel.TenantId).ToList();

                                        //if (studentScheduleViewData.Count > 0)
                                        //{
                                        //    this.context?.StudentScheduleView.RemoveRange(studentScheduleViewData);
                                        //}

                                        foreach (var student in studentData)
                                        {
                                            var studentCourseSectionSchedule = this.context?.StudentCoursesectionSchedule.FirstOrDefault(c => c.SchoolId == student.SchoolId && c.TenantId == student.TenantId && c.StudentId == student.StudentId && c.CourseSectionId == courseSection.CourseSectionId && c.AcademicYear == courseSection.AcademicYear && c.IsDropped != true);

                                            if (studentCourseSectionSchedule != null)
                                            {
                                                var conflictStudent = new StudentScheduleView()
                                                {
                                                    TenantId = student.TenantId,
                                                    SchoolId = student.SchoolId,
                                                    StudentId = student.StudentId,
                                                    CourseId = courseSection.CourseId,
                                                    CourseSectionId = courseSection.CourseSectionId,
                                                    CourseSectionName = courseSection.CourseSectionName,
                                                    StudentInternalId = student.StudentInternalId,
                                                    StudentName = student.FirstGivenName +" "+ student.MiddleName +" "+ student.LastFamilyName,
                                                    Scheduled = false,
                                                    ConflictComment = "Student is already scheduled in the course section"
                                                };
                                                studentCourseSectionScheduleAddViewModel._conflictFailure = true;
                                                this.context.StudentScheduleView.Add(conflictStudent);
                                                
                                                if (studentCourseSectionScheduleAddViewModel.studentMasterList.Count > 1)
                                                {
                                                    conflictMessage = "Some Student could not be scheduled due to conflicts. Please find the detailed report below."; 
                                                }
                                                else
                                                {
                                                    conflictMessage = "This Student could not be scheduled due to conflicts.";
                                                }
                                            }
                                            else
                                            {
                                                var courseSectionAllData = this.context?.AllCourseSectionView.Where(c => c.TenantId == courseSection.TenantId && c.SchoolId == courseSection.SchoolId && c.CourseSectionId == courseSection.CourseSectionId).ToList();

                                                if (courseSectionAllData.FirstOrDefault().AllowStudentConflict == true)
                                                {
                                                    var studentCourseScheduling = new StudentCoursesectionSchedule()
                                                    {
                                                        TenantId = courseSection.TenantId,
                                                        SchoolId = courseSection.SchoolId,
                                                        StudentId = student.StudentId,
                                                        CourseId = courseSection.CourseId,
                                                        CourseSectionId = courseSection.CourseSectionId,
                                                        StudentGuid = student.StudentGuid,
                                                        AlternateId = student.AlternateId,
                                                        StudentInternalId = student.StudentInternalId,
                                                        FirstGivenName = student.FirstGivenName,
                                                        MiddleName = student.MiddleName,
                                                        LastFamilyName = student.LastFamilyName,
                                                        FirstLanguageId = student.FirstLanguageId,
                                                        GradeId = student.StudentEnrollment.FirstOrDefault().GradeId,
                                                        AcademicYear = (decimal)courseSection.AcademicYear,
                                                        GradeScaleId = courseSection.GradeScaleId,
                                                        CourseSectionName = courseSection.CourseSectionName,
                                                        CalendarId = courseSection.CalendarId,
                                                        CreatedBy = studentCourseSectionScheduleAddViewModel.CreatedBy,
                                                        CreatedOn = DateTime.UtcNow
                                                    };
                                                    this.context.StudentCoursesectionSchedule.Add(studentCourseScheduling);

                                                    var conflictStudent = new StudentScheduleView()
                                                    {
                                                        TenantId = student.TenantId,
                                                        SchoolId = student.SchoolId,
                                                        StudentId = student.StudentId,
                                                        CourseId = courseSection.CourseId,
                                                        CourseSectionId = courseSection.CourseSectionId,
                                                        CourseSectionName = courseSection.CourseSectionName,
                                                        StudentInternalId = student.StudentInternalId,
                                                        StudentName = student.FirstGivenName + " " + student.MiddleName + " " + student.LastFamilyName,
                                                        Scheduled = true,
                                                    };
                                                    this.context?.StudentScheduleView.Add(conflictStudent);
                                                }
                                                else
                                                {
                                                    //var courseSectionAllData = this.context?.AllCourseSectionView.Where(c => c.TenantId == courseSection.TenantId && c.SchoolId == courseSection.SchoolId && c.CourseSectionId == courseSection.CourseSectionId).ToList();


                                                    if (courseSectionAllData.Count > 0)
                                                    {
                                                        bool isPeriodConflict = false;

                                                        foreach (var courseSectionAll in courseSectionAllData)
                                                        {                                                            
                                                            var courseSectionData = this.context?.AllCourseSectionView.
                                                                                   Join(this.context?.StudentCoursesectionSchedule,
                                                                                   acsv => acsv.CourseSectionId, scs => scs.CourseSectionId,
                                                                                   (acsv, scs) => new { acsv, scs }).AsEnumerable().Where(x => x.scs.SchoolId == courseSection.SchoolId && x.scs.StudentId == student.StudentId && x.acsv.DurationEndDate > courseSectionAll.DurationStartDate
                                                                                   &&
                                                                                   (
                                                                                   courseSectionAll.FixedPeriodId != null && ((x.acsv.FixedPeriodId == courseSectionAll.FixedPeriodId || x.acsv.VarPeriodId == courseSectionAll.FixedPeriodId || x.acsv.CalPeriodId == courseSectionAll.FixedPeriodId) && ((x.acsv.FixedDays != null && (Regex.IsMatch(courseSectionAll.FixedDays.ToLower(), x.acsv.FixedDays.ToLower(), RegexOptions.IgnoreCase))) || (x.acsv.VarDay != null && (courseSectionAll.FixedDays.ToLower().Contains(x.acsv.VarDay.ToLower()))) || (x.acsv.CalDay != null && (courseSectionAll.FixedDays.ToLower().Contains(x.acsv.CalDay.ToLower())))))
                                                                                   ||
                                                                                   courseSectionAll.VarPeriodId != null && ((x.acsv.FixedPeriodId == courseSectionAll.VarPeriodId || x.acsv.VarPeriodId == courseSectionAll.VarPeriodId || x.acsv.CalPeriodId == courseSectionAll.VarPeriodId) && ((x.acsv.FixedDays != null && (Regex.IsMatch(courseSectionAll.VarDay.ToLower(), x.acsv.FixedDays.ToLower(), RegexOptions.IgnoreCase))) || (x.acsv.VarDay != null && (courseSectionAll.VarDay.ToLower().Contains(x.acsv.VarDay.ToLower()))) || (x.acsv.CalDay != null && (courseSectionAll.VarDay.ToLower().Contains(x.acsv.CalDay.ToLower())))))
                                                                                   ||
                                                                                   courseSectionAll.CalPeriodId != null && ((x.acsv.FixedPeriodId == courseSectionAll.CalPeriodId || x.acsv.VarPeriodId == courseSectionAll.CalPeriodId || x.acsv.CalPeriodId == courseSectionAll.CalPeriodId) && ((x.acsv.FixedDays != null && (Regex.IsMatch(courseSectionAll.CalDay.ToLower(), x.acsv.FixedDays.ToLower(), RegexOptions.IgnoreCase))) || (x.acsv.VarDay != null && (courseSectionAll.CalDay.ToLower().Contains(x.acsv.VarDay.ToLower()))) || (x.acsv.CalDay != null && (courseSectionAll.CalDay.ToLower().Contains(x.acsv.CalDay.ToLower())))))
                                                                                   )
                                                                                );

                                                            if (courseSectionData.ToList().Count > 0)
                                                            {
                                                                isPeriodConflict = true;
                                                                break;
                                                            }
                                                        }
                                                        if (!(bool)isPeriodConflict)
                                                        {
                                                            var studentCourseScheduling = new StudentCoursesectionSchedule()
                                                            {
                                                                TenantId = courseSection.TenantId,
                                                                SchoolId = courseSection.SchoolId,
                                                                StudentId = student.StudentId,
                                                                CourseId = courseSection.CourseId,
                                                                CourseSectionId = courseSection.CourseSectionId,
                                                                StudentGuid = student.StudentGuid,
                                                                AlternateId = student.AlternateId,
                                                                StudentInternalId = student.StudentInternalId,
                                                                FirstGivenName = student.FirstGivenName,
                                                                MiddleName = student.MiddleName,
                                                                LastFamilyName = student.LastFamilyName,
                                                                FirstLanguageId = student.FirstLanguageId,
                                                                GradeId = student.StudentEnrollment.FirstOrDefault()?.GradeId,
                                                                AcademicYear = (decimal)courseSection.AcademicYear,
                                                                GradeScaleId = courseSection.GradeScaleId,
                                                                CourseSectionName = courseSection.CourseSectionName,
                                                                CalendarId = courseSection.CalendarId,
                                                                CreatedBy = studentCourseSectionScheduleAddViewModel.CreatedBy,
                                                                CreatedOn = DateTime.UtcNow
                                                            };
                                                            this.context?.StudentCoursesectionSchedule.Add(studentCourseScheduling);

                                                            var conflictStudent = new StudentScheduleView()
                                                            {
                                                                TenantId = student.TenantId,
                                                                SchoolId = student.SchoolId,
                                                                StudentId = student.StudentId,
                                                                CourseId = courseSection.CourseId,
                                                                CourseSectionId = courseSection.CourseSectionId,
                                                                CourseSectionName = courseSection.CourseSectionName,
                                                                StudentInternalId = student.StudentInternalId,
                                                                StudentName = student.FirstGivenName + " " + student.MiddleName + " " + student.LastFamilyName,
                                                                Scheduled = true,
                                                            };
                                                            this.context?.StudentScheduleView.Add(conflictStudent);
                                                        }
                                                        else
                                                        {
                                                            var conflictStudent = new StudentScheduleView()
                                                            {
                                                                TenantId = student.TenantId,
                                                                SchoolId = student.SchoolId,
                                                                StudentId = student.StudentId,
                                                                CourseId = courseSection.CourseId,
                                                                CourseSectionId = courseSection.CourseSectionId,
                                                                CourseSectionName = courseSection.CourseSectionName,
                                                                StudentInternalId = student.StudentInternalId,
                                                                StudentName = student.FirstGivenName + " " + student.MiddleName + " " + student.LastFamilyName,
                                                                Scheduled = false,
                                                                ConflictComment = "There is a period conflict"
                                                            };
                                                            studentCourseSectionScheduleAddViewModel._conflictFailure = true;
                                                            this.context?.StudentScheduleView.Add(conflictStudent);
                                                            
                                                            if (studentCourseSectionScheduleAddViewModel.studentMasterList.Count > 1)
                                                            {
                                                                conflictMessage = "Some Student could not be scheduled due to conflicts. Please find the detailed report below.";
                                                            }
                                                            else
                                                            {
                                                                conflictMessage = "This Student could not be scheduled due to conflicts.";
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        studentCourseSectionScheduleAddViewModel._failure = true;
                                                        studentCourseSectionScheduleAddViewModel._message = "Course Section Does Not Exist";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    studentCourseSectionScheduleAddViewModel._message = "Select Atleast One Student";
                                    studentCourseSectionScheduleAddViewModel._failure = true;
                                    studentCourseSectionScheduleAddViewModel.ConflictMessage = null;
                                    return studentCourseSectionScheduleAddViewModel;
                                }
                            }
                            else
                            {
                                if (studentCourseSectionScheduleAddViewModel.studentMasterList.Count>0)
                                {
                                    foreach (var studentMaster in studentCourseSectionScheduleAddViewModel.studentMasterList)
                                    {
                                        var conflictStudent = new StudentScheduleView()
                                        {
                                            TenantId = studentMaster.TenantId,
                                            SchoolId = studentMaster.SchoolId,
                                            StudentId = studentMaster.StudentId,
                                            CourseId = courseSection.CourseId,
                                            CourseSectionId = courseSection.CourseSectionId,
                                            CourseSectionName = courseSection.CourseSectionName,
                                            StudentInternalId = studentMaster.StudentInternalId,
                                            StudentName = (studentMaster.FirstGivenName!=null)? studentMaster.FirstGivenName :""+ " " + studentMaster.MiddleName + " " + studentMaster.LastFamilyName,
                                            Scheduled = false,
                                            ConflictComment = "Seats Not Avalaible"
                                        };
                                        studentCourseSectionScheduleAddViewModel._conflictFailure = true;
                                        this.context?.StudentScheduleView.Add(conflictStudent);
                                        
                                        if (studentCourseSectionScheduleAddViewModel.studentMasterList.Count > 1)
                                        {
                                            conflictMessage = "Some Student could not be scheduled due to conflicts. Please find the detailed report below.";
                                        }
                                        else
                                        {
                                            conflictMessage = "This Student could not be scheduled due to conflicts.";
                                        }
                                    }
                                }                               
                            }
                        }
                        var studentScheduleViewData = this.context?.StudentScheduleView.Where(e => e.SchoolId == studentCourseSectionScheduleAddViewModel.SchoolId && e.TenantId == studentCourseSectionScheduleAddViewModel.TenantId).ToList();

                        if (studentScheduleViewData.Count > 0)
                        {
                            this.context?.StudentScheduleView.RemoveRange(studentScheduleViewData);
                        }
                        this.context?.SaveChanges();
                        transaction.Commit();
                        studentCourseSectionScheduleAddViewModel._message = "Student Schedule Added Successfully";
                        studentCourseSectionScheduleAddViewModel.ConflictMessage = conflictMessage;
                        studentCourseSectionScheduleAddViewModel._failure = false;
                    }
                    else
                    {
                        studentCourseSectionScheduleAddViewModel._message = "Select Atleast One Course Section";
                        studentCourseSectionScheduleAddViewModel.ConflictMessage = null;
                        studentCourseSectionScheduleAddViewModel._failure = true;
                        return studentCourseSectionScheduleAddViewModel;
                    }
                }
                catch (Exception es)
                {
                    transaction.Rollback();
                    studentCourseSectionScheduleAddViewModel._failure = true;
                    studentCourseSectionScheduleAddViewModel.ConflictMessage = null;
                    studentCourseSectionScheduleAddViewModel._message = es.Message;
                }
            }
            return studentCourseSectionScheduleAddViewModel;
        }
        /// <summary>
        /// Search Scheduled Student For Group Drop
        /// </summary>
        /// <param name="scheduleStudentListViewModel"></param>
        /// <returns></returns>
        public ScheduleStudentListViewModel SearchScheduledStudentForGroupDrop(PageResult pageResult)
        {
            ScheduleStudentListViewModel scheduleStudentListView = new ScheduleStudentListViewModel();
            IQueryable<ScheduleStudentForView> transactionIQ = null;
            try
            {
                var scheduledStudentData = this.context?.StudentCoursesectionSchedule.
                                    Join(this.context?.StudentMaster,
                                    scs => scs.StudentId, sm => sm.StudentId,
                                    (scs, sm) => new { scs, sm }).Where(c => c.scs.TenantId == pageResult.TenantId && c.scs.SchoolId == pageResult.SchoolId && c.scs.CourseSectionId == pageResult.CourseSectionId && c.sm.SchoolId == pageResult.SchoolId && c.sm.TenantId == pageResult.TenantId && (pageResult.IncludeInactive == false ? c.sm.IsActive == true : true)).ToList().Select(ssv => new ScheduleStudentForView
                                    {
                                        SchoolId = ssv.sm.SchoolId,
                                        TenantId = ssv.sm.TenantId,
                                        StudentId = ssv.sm.StudentId,
                                        FirstGivenName = ssv.sm.FirstGivenName,
                                        LastFamilyName = ssv.sm.LastFamilyName,
                                        AlternateId = ssv.sm.AlternateId,
                                        StudentInternalId=ssv.sm.StudentInternalId,
                                        GradeLevel = this.context.Gradelevels.FirstOrDefault(c => c.TenantId == ssv.sm.TenantId && c.SchoolId == ssv.sm.SchoolId && c.GradeId == ssv.scs.GradeId)?.Title,
                                        Section = this.context.Sections.FirstOrDefault(c => c.TenantId == ssv.sm.TenantId && c.SchoolId == ssv.sm.SchoolId && c.SectionId == ssv.sm.SectionId)?.Name,
                                        GradeId= ssv.scs.GradeId,
                                        GradeScaleId= this.context.Grade.FirstOrDefault(c => c.TenantId == ssv.sm.TenantId && c.SchoolId == ssv.sm.SchoolId && c.GradeId == ssv.scs.GradeId)?.GradeScaleId,
                                        PhoneNumber = ssv.sm.MobilePhone,
                                        Action = ssv.scs.IsDropped,
                                        ScheduleDate = ssv.scs.CreatedOn,
                                        StudentPhoto=(pageResult.ProfilePhoto ==true)?ssv.sm.StudentPhoto:null
                                    }).ToList();

                if (scheduledStudentData.Count > 0)
                {
                    if (pageResult.FilterParams == null || pageResult.FilterParams.Count == 0)
                    {
                        transactionIQ = scheduledStudentData.AsQueryable();
                    }
                    else
                    {
                        if (pageResult.FilterParams != null && pageResult.FilterParams.ElementAt(0).ColumnName == null && pageResult.FilterParams.Count == 1)
                        {
                            string Columnvalue = pageResult.FilterParams.ElementAt(0).FilterValue;

                            transactionIQ = scheduledStudentData.Where(x => x.FirstGivenName.ToLower().Contains(Columnvalue.ToLower()) || x.LastFamilyName.ToLower().Contains(Columnvalue.ToLower()) || x.GradeLevel.ToLower().Contains(Columnvalue.ToLower()) || x.ScheduleDate.ToString() == Columnvalue || (x.Section != null && x.Section.ToLower().Contains(Columnvalue.ToLower())) || (x.StudentInternalId != null && x.StudentInternalId.ToLower().Contains(Columnvalue.ToLower())) || (x.AlternateId != null && x.AlternateId.ToLower().Contains(Columnvalue.ToLower())) || (x.PhoneNumber != null && x.PhoneNumber.ToLower().Contains(Columnvalue.ToLower()))).AsQueryable();
                        }
                        else
                        {
                            transactionIQ = Utility.FilteredData(pageResult.FilterParams, scheduledStudentData).AsQueryable();
                        }
                        transactionIQ = transactionIQ.Distinct();
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

                    int totalCount = transactionIQ.Count();
                    if (pageResult.PageNumber > 0 && pageResult.PageSize > 0)
                    {
                        transactionIQ = transactionIQ.Skip((pageResult.PageNumber - 1) * pageResult.PageSize).Take(pageResult.PageSize);
                    }
                    scheduleStudentListView.scheduleStudentForView = transactionIQ.ToList();
                    scheduleStudentListView.TotalCount = totalCount;
                }
                else
                {
                    scheduleStudentListView.scheduleStudentForView = scheduledStudentData;
                    scheduleStudentListView._failure = false;
                    scheduleStudentListView._message = NORECORDFOUND;
                }

                scheduleStudentListView.TenantId = pageResult.TenantId;
                scheduleStudentListView.SchoolId = pageResult.SchoolId;
                scheduleStudentListView.CourseSectionId = pageResult.CourseSectionId;
                scheduleStudentListView.PageNumber = pageResult.PageNumber;
                scheduleStudentListView._pageSize = pageResult.PageSize;
                scheduleStudentListView._tenantName = pageResult._tenantName;
                scheduleStudentListView._token = pageResult._token;
                scheduleStudentListView._failure = false;
            }
            catch (Exception es)
            {
                scheduleStudentListView._failure = true;
                scheduleStudentListView._message = es.Message; ;
            }
            return scheduleStudentListView;

        }

        /// <summary>
        /// Group Drop For Scheduled Student
        /// </summary>
        /// <param name="scheduledStudentDropModel"></param>
        /// <returns></returns>
        public ScheduledStudentDropModel GroupDropForScheduledStudent(ScheduledStudentDropModel scheduledStudentDropModel)
        {
            try
            {
                List<StudentCoursesectionSchedule> studentCoursesectionScheduleList = new List<StudentCoursesectionSchedule>();
                var currentDate = DateTime.UtcNow.Date;
                
                if (scheduledStudentDropModel.studentCoursesectionScheduleList.Count > 0 )
                {
                    if (!string.IsNullOrEmpty(scheduledStudentDropModel.StudentId.ToString()) && scheduledStudentDropModel.StudentId > 0)
                    {
                        foreach (var scheduledStudent in scheduledStudentDropModel.studentCoursesectionScheduleList)
                        {
                            var studentData = this.context?.StudentCoursesectionSchedule.Include(c=>c.CourseSection).FirstOrDefault(x => x.SchoolId == scheduledStudentDropModel.SchoolId && x.TenantId == scheduledStudentDropModel.TenantId && x.StudentId == scheduledStudentDropModel.StudentId && x.CourseSectionId == scheduledStudent.CourseSectionId && x.IsDropped!=true);
                            
                            if (studentData != null)
                            {
                                if (studentData.CourseSection.DurationEndDate>= scheduledStudent.EffectiveDropDate && currentDate<= scheduledStudent.EffectiveDropDate)
                                {
                                    studentData.EffectiveDropDate = scheduledStudent.EffectiveDropDate;
                                    studentData.UpdatedBy = scheduledStudentDropModel.UpdatedBy;
                                    studentData.UpdatedOn = DateTime.UtcNow;
                                    studentCoursesectionScheduleList.Add(studentData);
                                }
                                else
                                {
                                    scheduledStudentDropModel._failure = true;
                                    scheduledStudentDropModel._message = "Effective Drop Date Must Be Equal or Lower Than Course Section End Date And Equal or Greater Than Current Date";
                                    return scheduledStudentDropModel;
                                }
                            }
                            else
                            {
                                scheduledStudentDropModel._message = NORECORDFOUND;
                                scheduledStudentDropModel._failure = true;
                                return scheduledStudentDropModel;
                            }
                        }
                        scheduledStudentDropModel._message = "End/Drop Date Updated Successfully.";
                    }
                    else
                    {

                        if (!string.IsNullOrEmpty(scheduledStudentDropModel.EffectiveDropDate.ToString()) && scheduledStudentDropModel.EffectiveDropDate.Value.Date >= currentDate)
                        {
                            foreach (var scheduledStudent in scheduledStudentDropModel.studentCoursesectionScheduleList)
                            {
                                var studentData = this.context?.StudentCoursesectionSchedule.Include(c=>c.CourseSection).FirstOrDefault(x => x.SchoolId == scheduledStudent.SchoolId && x.TenantId == scheduledStudent.TenantId && x.StudentId == scheduledStudent.StudentId && x.CourseSectionId == scheduledStudentDropModel.CourseSectionId);

                                if (studentData != null)
                                {
                                    if (studentData.CourseSection.DurationEndDate>= scheduledStudentDropModel.EffectiveDropDate)
                                    {
                                        studentData.IsDropped = true;
                                        studentData.EffectiveDropDate = scheduledStudentDropModel.EffectiveDropDate;
                                        studentData.UpdatedBy = scheduledStudentDropModel.UpdatedBy;
                                        studentData.UpdatedOn = DateTime.UtcNow;
                                        studentCoursesectionScheduleList.Add(studentData);
                                    }
                                    else
                                    {
                                        scheduledStudentDropModel._failure = true;
                                        scheduledStudentDropModel._message = "Effective Drop Date Must Be Equal or Lower Than Course Section End Date";
                                        return scheduledStudentDropModel;
                                    }
                                }
                                else
                                {
                                    scheduledStudentDropModel._message = NORECORDFOUND;
                                    scheduledStudentDropModel._failure = true;
                                    return scheduledStudentDropModel;
                                }
                            }
                            scheduledStudentDropModel._message = "Selected students have been dropped from the course section.";
                        }
                        else
                        {
                            scheduledStudentDropModel._message = "Effective Drop Date Must Be Equal or Greater Than Current Date";
                            scheduledStudentDropModel._failure = true;
                            return scheduledStudentDropModel;
                        }
                    }
                    this.context?.StudentCoursesectionSchedule.UpdateRange(studentCoursesectionScheduleList);
                    this.context?.SaveChanges();
                    scheduledStudentDropModel._failure = false;
                    //scheduledStudentDropModel._message = "Selected students have been dropped from the course section.";
                }
                else
                {
                    scheduledStudentDropModel._message = "Select Atleast One Student";
                    scheduledStudentDropModel._failure = true;
                }
            }
            catch (Exception es)
            {
                scheduledStudentDropModel._failure = true;
                scheduledStudentDropModel._message = es.Message;
            }
            return scheduledStudentDropModel;
        }

        public StudentScheduleReportViewModel StudentScheduleReport(StudentScheduleReportViewModel studentScheduleReportViewModel)
        {
            StudentScheduleReportViewModel studentScheduleReportView = new StudentScheduleReportViewModel();
            try
            {
                var scheduleReport = this.context?.StudentScheduleView.Where(x => x.SchoolId == studentScheduleReportViewModel.SchoolId).ToPivotTable(
                    item => item.CourseSectionName,
                    item => new { item.StudentId, item.StudentName, item.StudentInternalId },
                    items => items.Any() ? items.First().Scheduled + " | " + items.First().ConflictComment : null);

                studentScheduleReportView.ScheduleReport = scheduleReport;

                studentScheduleReportView.TenantId = studentScheduleReportViewModel.TenantId;
                studentScheduleReportView.SchoolId = studentScheduleReportViewModel.SchoolId;
            }
            catch (Exception es)
            {
                studentScheduleReportViewModel._failure = true;
                studentScheduleReportViewModel._message = es.Message;
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
            try
            {
                var studentScheduleViewData = this.context?.StudentScheduleView.Where(e => e.SchoolId == studentCourseSectionScheduleAddViewModel.SchoolId && e.TenantId == studentCourseSectionScheduleAddViewModel.TenantId).ToList();

                if (studentScheduleViewData.Count > 0)
                {
                    this.context?.StudentScheduleView.RemoveRange(studentScheduleViewData);
                    this.context?.SaveChanges();
                    studentCourseSectionScheduleAddViewModel._failure = false;
                    studentCourseSectionScheduleAddViewModel._message = "Student Schedule Report Deleted Successfully";
                }
                else
                {
                    studentCourseSectionScheduleAddViewModel._message = NORECORDFOUND;
                    studentCourseSectionScheduleAddViewModel._failure = true;
                }
            }
            catch (Exception es)
            {
                studentCourseSectionScheduleAddViewModel._failure = true;
                studentCourseSectionScheduleAddViewModel._message = es.Message;
            }
            return studentCourseSectionScheduleAddViewModel;
        }
        /// <summary>
        /// Schedule Courses For Student360
        /// </summary>
        /// <param name="student360ScheduleCourseSectionListViewModel"></param>
        /// <returns></returns>
        public Student360ScheduleCourseSectionListViewModel ScheduleCoursesForStudent360(Student360ScheduleCourseSectionListViewModel student360ScheduleCourseSectionListViewModel)
        {
            Student360ScheduleCourseSectionListViewModel student360ScheduleCourseSectionListView = new Student360ScheduleCourseSectionListViewModel();
            List<Student360ScheduleCourseSectionForView> student360ScheduleCourseSectionForViewList = new List<Student360ScheduleCourseSectionForView>();
            try
            {
                var StudentData = this.context?.StudentCoursesectionSchedule.Include(v => v.CourseSection.Course).Include(x => x.CourseSection).ThenInclude(y => y.SchoolYears).ThenInclude(s => s.Semesters).ThenInclude(e=>e.Quarters).ThenInclude(c => c.StaffCoursesectionSchedule).Include(e=>e.StudentAttendance).Where(x => (student360ScheduleCourseSectionListViewModel.IsDropped==false)?  x.SchoolId == student360ScheduleCourseSectionListViewModel.SchoolId && x.TenantId == student360ScheduleCourseSectionListViewModel.TenantId && x.StudentId == student360ScheduleCourseSectionListViewModel.StudentId && x.IsDropped != true : x.SchoolId == student360ScheduleCourseSectionListViewModel.SchoolId && x.TenantId == student360ScheduleCourseSectionListViewModel.TenantId && x.StudentId == student360ScheduleCourseSectionListViewModel.StudentId).ToList();

                var staffCoursesectionScheduleData = this.context?.StaffCoursesectionSchedule.Include(d => d.StaffMaster).Select(v => new StaffCoursesectionSchedule() { SchoolId=v.SchoolId,TenantId=v.TenantId,CourseSectionId=v.CourseSectionId,IsDropped=v.IsDropped,StaffMaster=new StaffMaster() {
                    FirstGivenName=v.StaffMaster.FirstGivenName,
                    MiddleName=v.StaffMaster.MiddleName,
                    LastFamilyName=v.StaffMaster.LastFamilyName} 
                     }).ToList();
                var studentFinalGradeData = this.context?.StudentFinalGrade;

                if (StudentData.Count > 0)
                {
                    foreach (var Student in StudentData)
                    {
                        var staffData = staffCoursesectionScheduleData.Where(c => c.TenantId == Student.TenantId && c.SchoolId == Student.SchoolId && c.CourseSectionId == Student.CourseSectionId && c.IsDropped != true).Select(v=>v.StaffMaster).ToList();

                        var studentInputFinalGradeData = studentFinalGradeData.FirstOrDefault(b => b.TenantId == Student.TenantId && b.SchoolId == Student.SchoolId && b.StudentId== Student.StudentId && b.CourseSectionId == Student.CourseSectionId);

                        var Student360ScheduleCourseSectionData = new Student360ScheduleCourseSectionForView()
                        {
                            TenantId = Student.TenantId,
                            SchoolId = Student.SchoolId,
                            CourseId = Student.CourseId,
                            CourseSectionId = Student.CourseSectionId,
                            CourseTitle = Student.CourseSection.Course.CourseTitle,
                            CourseSectionName = Student.CourseSectionName,
                            CourseSectionDurationStartDate= Student.CourseSection.DurationStartDate,
                            CourseSectionDurationEndDate=Student.CourseSection.DurationEndDate,
                            YrMarkingPeriodId = Student.CourseSection.YrMarkingPeriodId,
                            SmstrMarkingPeriodId = Student.CourseSection.SmstrMarkingPeriodId,
                            QtrMarkingPeriodId = Student.CourseSection.QtrMarkingPeriodId,
                            EnrolledDate = Student.CreatedOn,
                            EffectiveDropDate = (Student.EffectiveDropDate == null) ? Student.CourseSection.DurationEndDate : Student.EffectiveDropDate,
                            DayOfWeek = (Student.CourseSection.StaffCoursesectionSchedule.Count > 0)? Student.CourseSection.StaffCoursesectionSchedule.FirstOrDefault().MeetingDays:null,
                            staffMasterList = /*(staffData.Count > 0) ? staffData : null*/ staffData,
                            CreatedBy =Student.CreatedBy,
                            CreatedOn=Student.CreatedOn,
                            UpdatedBy=Student.UpdatedBy,
                            UpdatedOn=Student.UpdatedOn,
                            IsDropped=Student.IsDropped,
                            IsAssociationship=(Student.StudentAttendance.Count>0 || studentInputFinalGradeData!=null)?true:false,
                            studentAttendanceList= /*(Student.StudentAttendance.Count > 0)? Student.StudentAttendance.ToList():null*/ Student.StudentAttendance.ToList(),
                            SchoolYears=Student.CourseSection.SchoolYears,
                            Semesters=Student.CourseSection.Semesters,
                            Quarters=Student.CourseSection.Quarters,
                            AttendanceTaken= Student.CourseSection.AttendanceTaken
                        };
                        if (Student.CourseSection.ScheduleType == "Fixed Schedule (1)")
                        {
                            var fixedData = this.context?.CourseFixedSchedule.Include(v => v.BlockPeriod).Include(f => f.Rooms).FirstOrDefault(c => c.SchoolId == Student.SchoolId && c.TenantId == Student.TenantId && c.CourseSectionId == Student.CourseSectionId);

                            if (fixedData != null)
                            {
                                fixedData.BlockPeriod.CourseFixedSchedule = null;
                                fixedData.BlockPeriod.CourseVariableSchedule = null;
                                fixedData.BlockPeriod.CourseCalendarSchedule = null;
                                fixedData.BlockPeriod.CourseBlockSchedule = null;
                                fixedData.BlockPeriod.StudentAttendance = null;
                                fixedData.Rooms.CourseFixedSchedule = null;
                                fixedData.Rooms.CourseVariableSchedule = null;
                                fixedData.Rooms.CourseCalendarSchedule = null;
                                fixedData.Rooms.CourseBlockSchedule = null;
                                Student360ScheduleCourseSectionData.courseFixedSchedule = fixedData;
                                Student360ScheduleCourseSectionData.DayOfWeek = Student.CourseSection?.MeetingDays;
                            }
                        }
                        if (Student.CourseSection.ScheduleType == "Variable Schedule (2)")
                        {
                            var variableData = this.context?.CourseVariableSchedule.Include(v => v.BlockPeriod).Include(f => f.Rooms).Where(c => c.SchoolId == Student.SchoolId && c.TenantId == Student.TenantId && c.CourseSectionId == Student.CourseSectionId).ToList();

                            if (variableData.Count > 0)
                            {
                                variableData.ForEach(x => { x.BlockPeriod.CourseFixedSchedule = null; x.BlockPeriod.CourseVariableSchedule = null; x.BlockPeriod.CourseCalendarSchedule = null; x.BlockPeriod.CourseBlockSchedule = null;x.BlockPeriod.StudentAttendance=null ; x.Rooms.CourseFixedSchedule = null; x.Rooms.CourseVariableSchedule = null; x.Rooms.CourseCalendarSchedule = null; x.Rooms.CourseBlockSchedule = null; });
                                
                                Student360ScheduleCourseSectionData.courseVariableScheduleList = variableData;
                            }
                        }
                        if (Student.CourseSection.ScheduleType == "Calendar Schedule (3)")
                        {
                            var calenderData = this.context?.CourseCalendarSchedule.Include(v => v.BlockPeriod).Include(f => f.Rooms).Where(c => c.SchoolId == Student.SchoolId && c.TenantId == Student.TenantId && c.CourseSectionId == Student.CourseSectionId).ToList();

                            if (calenderData.Count > 0)
                            {
                                calenderData.ForEach(x => { x.BlockPeriod.CourseFixedSchedule = null; x.BlockPeriod.CourseVariableSchedule = null; x.BlockPeriod.CourseCalendarSchedule = null; x.BlockPeriod.CourseBlockSchedule = null; x.BlockPeriod.StudentAttendance = null; x.Rooms.CourseFixedSchedule = null; x.Rooms.CourseVariableSchedule = null; x.Rooms.CourseCalendarSchedule = null; x.Rooms.CourseBlockSchedule = null; });

                                Student360ScheduleCourseSectionData.courseCalendarScheduleList = calenderData;
                            }
                        }
                        if (Student.CourseSection.ScheduleType == "Block Schedule (4)")
                        {
                            var blockData = this.context?.CourseBlockSchedule.Include(v => v.BlockPeriod).Include(f => f.Rooms).Where(c => c.SchoolId == Student.SchoolId && c.TenantId == Student.TenantId && c.CourseSectionId == Student.CourseSectionId).ToList();

                            if (blockData.Count > 0)
                            {
                                blockData.ForEach(x => { x.BlockPeriod.CourseFixedSchedule = null; x.BlockPeriod.CourseVariableSchedule = null; x.BlockPeriod.CourseCalendarSchedule = null; x.BlockPeriod.CourseBlockSchedule = null; x.BlockPeriod.StudentAttendance = null; x.Rooms.CourseFixedSchedule = null; x.Rooms.CourseVariableSchedule = null; x.Rooms.CourseCalendarSchedule = null; x.Rooms.CourseBlockSchedule = null; });

                                Student360ScheduleCourseSectionData.courseBlockScheduleList = blockData;
                            }
                        }
                        
                        if (Student360ScheduleCourseSectionData.studentAttendanceList.ToList().Count()>0)
                        {
                            Student360ScheduleCourseSectionData.studentAttendanceList.ForEach(b => 
                            {
                                b.StudentCoursesectionSchedule = null;
                                b.StaffCoursesectionSchedule = null;
                                b.BlockPeriod = null;
                              //b.BlockPeriod.CourseFixedSchedule = null;
                              //b.BlockPeriod.CourseVariableSchedule = null; 
                              //b.BlockPeriod.CourseCalendarSchedule = null;
                              //b.BlockPeriod.CourseBlockSchedule = null;                               
                              //b.BlockPeriod.StudentAttendance = null; 
                              //b.BlockPeriod.SchoolMaster = null;                               
                            });
                        }
                        if (Student360ScheduleCourseSectionData !=null)
                        {
                            if (Student360ScheduleCourseSectionData.SchoolYears !=null)
                            {
                                Student360ScheduleCourseSectionData.SchoolYears.CourseSection = null;
                                Student360ScheduleCourseSectionData.SchoolYears.HonorRolls = null;
                                Student360ScheduleCourseSectionData.SchoolYears.Semesters = null;
                                Student360ScheduleCourseSectionData.SchoolYears.StaffCoursesectionSchedule = null;
                                Student360ScheduleCourseSectionData.SchoolYears.StudentEffortGradeMaster = null;
                                Student360ScheduleCourseSectionData.SchoolYears.StudentFinalGrade = null;
                            }
                            if (Student360ScheduleCourseSectionData.Quarters != null)
                            {
                                Student360ScheduleCourseSectionData.Quarters.CourseSection = null;
                                Student360ScheduleCourseSectionData.Quarters.Semesters = null;
                                Student360ScheduleCourseSectionData.Quarters.StaffCoursesectionSchedule = null;
                                Student360ScheduleCourseSectionData.Quarters.StudentEffortGradeMaster = null;
                                Student360ScheduleCourseSectionData.Quarters.StudentFinalGrade = null;
                                Student360ScheduleCourseSectionData.Quarters.ProgressPeriods = null;
                            }
                            if (Student360ScheduleCourseSectionData.Semesters != null)
                            {
                                Student360ScheduleCourseSectionData.Semesters.CourseSection = null;
                                Student360ScheduleCourseSectionData.Semesters.Quarters = null;
                                Student360ScheduleCourseSectionData.Semesters.StaffCoursesectionSchedule = null;
                                Student360ScheduleCourseSectionData.Semesters.StudentEffortGradeMaster = null;
                                Student360ScheduleCourseSectionData.Semesters.StudentFinalGrade = null;
                                Student360ScheduleCourseSectionData.Semesters.SchoolYears = null;
                            }
                                
                        }
                            student360ScheduleCourseSectionForViewList.Add(Student360ScheduleCourseSectionData);
                    }
                    student360ScheduleCourseSectionListView.scheduleCourseSectionForView = student360ScheduleCourseSectionForViewList;
                    student360ScheduleCourseSectionListView._failure = false;
                    student360ScheduleCourseSectionListView.SchoolId = student360ScheduleCourseSectionListViewModel.SchoolId;
                    student360ScheduleCourseSectionListView.TenantId = student360ScheduleCourseSectionListViewModel.TenantId;
                    student360ScheduleCourseSectionListView.StudentId = student360ScheduleCourseSectionListViewModel.StudentId;
                    student360ScheduleCourseSectionListView.IsDropped = student360ScheduleCourseSectionListViewModel.IsDropped;
                    student360ScheduleCourseSectionListView._tenantName = student360ScheduleCourseSectionListViewModel._tenantName;
                    student360ScheduleCourseSectionListView._token = student360ScheduleCourseSectionListViewModel._token;
                    student360ScheduleCourseSectionListView._userName = student360ScheduleCourseSectionListViewModel._userName;
                }
                else
                {
                    student360ScheduleCourseSectionListView._failure = true;
                    student360ScheduleCourseSectionListView._message = NORECORDFOUND;
                    student360ScheduleCourseSectionListView.SchoolId = student360ScheduleCourseSectionListViewModel.SchoolId;
                    student360ScheduleCourseSectionListView.TenantId = student360ScheduleCourseSectionListViewModel.TenantId;
                    student360ScheduleCourseSectionListView.StudentId = student360ScheduleCourseSectionListViewModel.StudentId;
                    student360ScheduleCourseSectionListView.IsDropped = student360ScheduleCourseSectionListViewModel.IsDropped;
                    student360ScheduleCourseSectionListView._tenantName = student360ScheduleCourseSectionListViewModel._tenantName;
                    student360ScheduleCourseSectionListView._token = student360ScheduleCourseSectionListViewModel._token;
                    student360ScheduleCourseSectionListView._userName = student360ScheduleCourseSectionListViewModel._userName;
                }
            }
            catch (Exception es)
            {
                student360ScheduleCourseSectionListView._failure = true;
                student360ScheduleCourseSectionListView._message = es.Message;
                student360ScheduleCourseSectionListView.SchoolId = student360ScheduleCourseSectionListViewModel.SchoolId;
                student360ScheduleCourseSectionListView.TenantId = student360ScheduleCourseSectionListViewModel.TenantId;
                student360ScheduleCourseSectionListView.StudentId = student360ScheduleCourseSectionListViewModel.StudentId;
                student360ScheduleCourseSectionListView.IsDropped = student360ScheduleCourseSectionListViewModel.IsDropped;
                student360ScheduleCourseSectionListView._tenantName = student360ScheduleCourseSectionListViewModel._tenantName;
                student360ScheduleCourseSectionListView._token = student360ScheduleCourseSectionListViewModel._token;
                student360ScheduleCourseSectionListView._userName = student360ScheduleCourseSectionListViewModel._userName;
            }            
            return student360ScheduleCourseSectionListView;
        }

        /// <summary>
        /// Drop Scheduled Course Section For Student360
        /// </summary>
        /// <param name="scheduledStudentDropModel"></param>
        /// <returns></returns>
        public ScheduledStudentDropModel DropScheduledCourseSectionForStudent360(ScheduledStudentDropModel scheduledStudentDropModel)
        {
            try
            {
                var studentCourseSectionScheduleData = this.context?.StudentCoursesectionSchedule.Include(e => e.StudentAttendance).FirstOrDefault(r => r.TenantId == scheduledStudentDropModel.TenantId && r.SchoolId == scheduledStudentDropModel.SchoolId && r.CourseSectionId == scheduledStudentDropModel.CourseSectionId && r.StudentId == scheduledStudentDropModel.StudentId && r.IsDropped != true);

                //        var studentCourseSectionScheduleData = this.context?.StudentCoursesectionSchedule.
                //Join(this.context?.StudentFinalGrade, u => u.StudentId, uir => uir.StudentId,
                //(u, uir) => new { u, uir }).Join(this.context?.StudentAttendance, st => st.u.StudentId, SA => SA.StudentId,
                //(st, SA) => new { st, SA }).Where(c =>c.st.u.SchoolId == scheduledStudentDropModel.SchoolId && c.st.uir.SchoolId == scheduledStudentDropModel.SchoolId && c.SA.SchoolId == scheduledStudentDropModel.SchoolId && c.st.u.TenantId == scheduledStudentDropModel.TenantId && c.st.uir.TenantId == scheduledStudentDropModel.TenantId && c.SA.TenantId == scheduledStudentDropModel.TenantId && c.st.u.StudentId == scheduledStudentDropModel.StudentId && c.st.uir.StudentId == scheduledStudentDropModel.StudentId && c.SA.StudentId == scheduledStudentDropModel.StudentId && c.st.u.CourseSectionId == scheduledStudentDropModel.CourseSectionId && c.st.uir.CourseSectionId == scheduledStudentDropModel.CourseSectionId && c.SA.CourseSectionId == scheduledStudentDropModel.CourseSectionId)
                //.Select(m => new
                // {
                //    m.st.u,m.st.uir,m.SA
                // }).FirstOrDefault();

                if (studentCourseSectionScheduleData!=null)
                {
                   var studentFinalGradeData= this.context?.StudentFinalGrade.FirstOrDefault( r => r.TenantId == scheduledStudentDropModel.TenantId && r.SchoolId == scheduledStudentDropModel.SchoolId && r.CourseSectionId == scheduledStudentDropModel.CourseSectionId && r.StudentId == scheduledStudentDropModel.StudentId);


                    if (studentCourseSectionScheduleData.StudentAttendance.Count > 0|| studentFinalGradeData !=null)
                    {
                        scheduledStudentDropModel._failure = true;
                        scheduledStudentDropModel._message = "Deletion Disallowed Due To Association.";
                    }
                    else
                    {
                        studentCourseSectionScheduleData.IsDropped = true;
                        studentCourseSectionScheduleData.EffectiveDropDate = DateTime.UtcNow;
                        studentCourseSectionScheduleData.UpdatedBy = scheduledStudentDropModel.UpdatedBy;
                        studentCourseSectionScheduleData.UpdatedOn = DateTime.UtcNow;
                        this.context.StudentCoursesectionSchedule.Update(studentCourseSectionScheduleData);
                        this.context?.SaveChanges();
                        scheduledStudentDropModel._failure = false;
                        scheduledStudentDropModel._message = "Drop Course Section Successfully.";
                    }                    
                }
                else
                {
                    scheduledStudentDropModel._failure = true;
                    scheduledStudentDropModel._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {
                scheduledStudentDropModel._failure = true;
                scheduledStudentDropModel._message = es.Message;
            }
            return scheduledStudentDropModel;
        }

        public Student360ScheduleCourseSectionListViewModel ScheduleCourseSectionListForStudent360(Student360ScheduleCourseSectionListViewModel student360ScheduleCourseSectionListViewModel)
        {
            Student360ScheduleCourseSectionListViewModel student360ScheduleCourseSectionListView = new Student360ScheduleCourseSectionListViewModel();
            List<Student360ScheduleCourseSectionForView> student360ScheduleCourseSectionForViewList = new List<Student360ScheduleCourseSectionForView>();
            try
            {
                //var studentdata = this.context?.CourseSection.Where(r => r.SchoolId == student360ScheduleCourseSectionListViewModel.SchoolId && r.TenantId == student360ScheduleCourseSectionListViewModel.TenantId && r.DurationStartDate.Value.Date >= student360ScheduleCourseSectionListViewModel.DurationStartDate.Value.Date && r.DurationEndDate.Value.Date <= student360ScheduleCourseSectionListViewModel.DurationEndDate.Value.Date).Select(e => e.CourseSectionId).ToList();

                var studentdata = this.context?.CourseSection.Where(r => r.SchoolId == student360ScheduleCourseSectionListViewModel.SchoolId && r.TenantId == student360ScheduleCourseSectionListViewModel.TenantId &&((student360ScheduleCourseSectionListViewModel.DurationStartDate.Value.Date >= r.DurationStartDate.Value.Date && student360ScheduleCourseSectionListViewModel.DurationStartDate.Value.Date <= r.DurationEndDate.Value.Date)||(student360ScheduleCourseSectionListViewModel.DurationEndDate.Value.Date >= r.DurationStartDate.Value.Date && student360ScheduleCourseSectionListViewModel.DurationEndDate.Value.Date <= r.DurationEndDate))).Select(e => e.CourseSectionId).ToList();

                if (studentdata.ToList().Count > 0)
                {
                    var studentCourseSectionScheduleData = this.context?.StudentCoursesectionSchedule.Include(o => o.CourseSection).Include(c=>c.StudentAttendance).ThenInclude(v=>v.StudentAttendanceComments).Include(p=>p.CourseSection.SchoolCalendars).Where(e => e.TenantId == student360ScheduleCourseSectionListViewModel.TenantId && e.SchoolId == student360ScheduleCourseSectionListViewModel.SchoolId && e.StudentId== student360ScheduleCourseSectionListViewModel.StudentId && (studentdata == null || (studentdata.Contains(e.CourseSectionId))) && e.IsDropped !=true).ToList();

                    if (studentCourseSectionScheduleData.Count > 0)
                    {
                        var attendanceCategoriesData = this.context?.AttendanceCodeCategories.Include(c=>c.AttendanceCode).Where(x => x.SchoolId == student360ScheduleCourseSectionListViewModel.SchoolId && x.TenantId == student360ScheduleCourseSectionListViewModel.TenantId).ToList();

                        foreach (var studentCourseSectionSchedule in studentCourseSectionScheduleData)
                        {
                            var Student360ScheduleCourseSection = new Student360ScheduleCourseSectionForView()
                            {
                                TenantId = studentCourseSectionSchedule.TenantId,
                                SchoolId = studentCourseSectionSchedule.SchoolId,
                                CourseId = studentCourseSectionSchedule.CourseId,
                                CourseSectionId = studentCourseSectionSchedule.CourseSectionId,
                                CourseSectionName = studentCourseSectionSchedule.CourseSectionName,
                                CourseSectionDurationStartDate = studentCourseSectionSchedule.CourseSection.DurationStartDate,
                                CourseSectionDurationEndDate = studentCourseSectionSchedule.CourseSection.DurationEndDate,
                                YrMarkingPeriodId = studentCourseSectionSchedule.CourseSection.YrMarkingPeriodId,
                                SmstrMarkingPeriodId = studentCourseSectionSchedule.CourseSection.SmstrMarkingPeriodId,
                                QtrMarkingPeriodId = studentCourseSectionSchedule.CourseSection.QtrMarkingPeriodId,
                                EnrolledDate = studentCourseSectionSchedule.CreatedOn,
                                EffectiveDropDate = (studentCourseSectionSchedule.EffectiveDropDate == null) ? studentCourseSectionSchedule.CourseSection.DurationEndDate : studentCourseSectionSchedule.EffectiveDropDate,
                                CreatedBy = studentCourseSectionSchedule.CreatedBy,
                                CreatedOn = studentCourseSectionSchedule.CreatedOn,
                                UpdatedBy = studentCourseSectionSchedule.UpdatedBy,
                                UpdatedOn = studentCourseSectionSchedule.UpdatedOn,
                                IsDropped = studentCourseSectionSchedule.IsDropped,
                                AttendanceTaken = studentCourseSectionSchedule.CourseSection.AttendanceTaken,
                                studentAttendanceList = (studentCourseSectionSchedule.StudentAttendance.Count() > 0) ? studentCourseSectionSchedule.StudentAttendance.Where(x => x.AttendanceDate.Date >= student360ScheduleCourseSectionListViewModel.DurationStartDate.Value.Date && x.AttendanceDate.Date <= student360ScheduleCourseSectionListViewModel.DurationEndDate.Value.Date).ToList() : null,
                                //SchoolYears = Student.CourseSection.SchoolYears,
                                //Semesters = Student.CourseSection.Semesters,
                                //Quarters = Student.CourseSection.Quarters
                                AttendanceCodeCategories = attendanceCategoriesData.FirstOrDefault(z => z.AttendanceCategoryId == studentCourseSectionSchedule.CourseSection.AttendanceCategoryId),
                                WeekDays= studentCourseSectionSchedule.CourseSection.SchoolCalendars.Days
                            };

                            if (studentCourseSectionSchedule.CourseSection.ScheduleType == "Fixed Schedule (1)")
                            {
                                var fixedData = this.context?.CourseFixedSchedule.Include(v => v.BlockPeriod).FirstOrDefault(c => c.SchoolId == studentCourseSectionSchedule.SchoolId && c.TenantId == studentCourseSectionSchedule.TenantId && c.CourseSectionId == studentCourseSectionSchedule.CourseSectionId);

                                if (fixedData != null)
                                {
                                    fixedData.BlockPeriod.CourseFixedSchedule = null;
                                    fixedData.BlockPeriod.CourseVariableSchedule = null;
                                    fixedData.BlockPeriod.CourseCalendarSchedule = null;
                                    fixedData.BlockPeriod.CourseBlockSchedule = null;
                                    fixedData.BlockPeriod.StudentAttendance = null;
                                    Student360ScheduleCourseSection.courseFixedSchedule = fixedData;
                                    Student360ScheduleCourseSection.DayOfWeek = studentCourseSectionSchedule.CourseSection?.MeetingDays;
                                }
                            }
                            if (studentCourseSectionSchedule.CourseSection.ScheduleType == "Variable Schedule (2)")
                            {
                                var variableData = this.context?.CourseVariableSchedule.Include(v => v.BlockPeriod).Where(c => c.SchoolId == studentCourseSectionSchedule.SchoolId && c.TenantId == studentCourseSectionSchedule.TenantId && c.CourseSectionId == studentCourseSectionSchedule.CourseSectionId).ToList();

                                if (variableData.Count > 0)
                                {
                                    variableData.ForEach(x => { x.BlockPeriod.CourseFixedSchedule = null; x.BlockPeriod.CourseVariableSchedule = null; x.BlockPeriod.CourseCalendarSchedule = null; x.BlockPeriod.CourseBlockSchedule = null; x.BlockPeriod.StudentAttendance = null; });

                                    Student360ScheduleCourseSection.courseVariableScheduleList = variableData;
                                }
                            }
                            if (studentCourseSectionSchedule.CourseSection.ScheduleType == "Calendar Schedule (3)")
                            {
                                var calenderData = this.context?.CourseCalendarSchedule.Include(v => v.BlockPeriod).Where(c => c.SchoolId == studentCourseSectionSchedule.SchoolId && c.TenantId == studentCourseSectionSchedule.TenantId && c.CourseSectionId == studentCourseSectionSchedule.CourseSectionId).ToList();

                                if (calenderData.Count > 0)
                                {
                                    calenderData.ForEach(x => { x.BlockPeriod.CourseFixedSchedule = null; x.BlockPeriod.CourseVariableSchedule = null; x.BlockPeriod.CourseCalendarSchedule = null; x.BlockPeriod.CourseBlockSchedule = null; x.BlockPeriod.StudentAttendance = null; });

                                    Student360ScheduleCourseSection.courseCalendarScheduleList = calenderData;
                                }
                            }
                            if (studentCourseSectionSchedule.CourseSection.ScheduleType == "Block Schedule (4)")
                            {
                                var blockData = this.context?.CourseBlockSchedule.Include(v => v.BlockPeriod).Include(f => f.Rooms).Where(c => c.SchoolId == studentCourseSectionSchedule.SchoolId && c.TenantId == studentCourseSectionSchedule.TenantId && c.CourseSectionId == studentCourseSectionSchedule.CourseSectionId).ToList();

                                if (blockData.Count > 0)
                                {
                                    blockData.ForEach(x => { x.BlockPeriod.CourseFixedSchedule = null; x.BlockPeriod.CourseVariableSchedule = null; x.BlockPeriod.CourseCalendarSchedule = null; x.BlockPeriod.CourseBlockSchedule = null; x.BlockPeriod.StudentAttendance = null; });

                                    Student360ScheduleCourseSection.courseBlockScheduleList = blockData;
                                }
                            }
                            if (Student360ScheduleCourseSection.AttendanceCodeCategories!=null)
                            {                               
                                Student360ScheduleCourseSection.AttendanceCodeCategories.CourseSection = null;
                                Student360ScheduleCourseSection.AttendanceCodeCategories.AttendanceCode.ToList().ForEach(v => v.StudentAttendance = null);
                            }                            

                            if ( Student360ScheduleCourseSection.studentAttendanceList!=null)
                            {
                                Student360ScheduleCourseSection.studentAttendanceList.ForEach(u => { u.AttendanceCodeNavigation = null; u.BlockPeriod = null; u.StudentCoursesectionSchedule = null; });
                            }
                            student360ScheduleCourseSectionForViewList.Add(Student360ScheduleCourseSection);
                        }
                        student360ScheduleCourseSectionListView.scheduleCourseSectionForView = student360ScheduleCourseSectionForViewList;
                        student360ScheduleCourseSectionListView._failure = false;
                        student360ScheduleCourseSectionListView.SchoolId = student360ScheduleCourseSectionListViewModel.SchoolId;
                        student360ScheduleCourseSectionListView.TenantId = student360ScheduleCourseSectionListViewModel.TenantId;
                        student360ScheduleCourseSectionListView.DurationStartDate = student360ScheduleCourseSectionListViewModel.DurationStartDate;
                        student360ScheduleCourseSectionListView.DurationEndDate= student360ScheduleCourseSectionListViewModel.DurationEndDate;
                        student360ScheduleCourseSectionListView.StudentId = student360ScheduleCourseSectionListViewModel.StudentId;
                        student360ScheduleCourseSectionListView.IsDropped = student360ScheduleCourseSectionListViewModel.IsDropped;
                        student360ScheduleCourseSectionListView._tenantName = student360ScheduleCourseSectionListViewModel._tenantName;
                        student360ScheduleCourseSectionListView._token = student360ScheduleCourseSectionListViewModel._token;
                        student360ScheduleCourseSectionListView._userName = student360ScheduleCourseSectionListViewModel._userName;
                    }
                    else
                    {
                        student360ScheduleCourseSectionListView.scheduleCourseSectionForView = student360ScheduleCourseSectionForViewList;
                        student360ScheduleCourseSectionListView._failure = true;
                        student360ScheduleCourseSectionListView._message = NORECORDFOUND;
                        student360ScheduleCourseSectionListView.SchoolId = student360ScheduleCourseSectionListViewModel.SchoolId;
                        student360ScheduleCourseSectionListView.TenantId = student360ScheduleCourseSectionListViewModel.TenantId;
                        student360ScheduleCourseSectionListView.StudentId = student360ScheduleCourseSectionListViewModel.StudentId;
                    }
                }                
            }
            catch (Exception es)
            {
                student360ScheduleCourseSectionListView._failure = true;
                student360ScheduleCourseSectionListView._message = es.Message;
                student360ScheduleCourseSectionListView.SchoolId = student360ScheduleCourseSectionListViewModel.SchoolId;
                student360ScheduleCourseSectionListView.TenantId = student360ScheduleCourseSectionListViewModel.TenantId;
                student360ScheduleCourseSectionListView.StudentId = student360ScheduleCourseSectionListViewModel.StudentId;
                student360ScheduleCourseSectionListView._tenantName = student360ScheduleCourseSectionListViewModel._tenantName;
                student360ScheduleCourseSectionListView._token = student360ScheduleCourseSectionListViewModel._token;
                student360ScheduleCourseSectionListView._userName = student360ScheduleCourseSectionListViewModel._userName;
            }
            return student360ScheduleCourseSectionListView;
        }
    }
}
