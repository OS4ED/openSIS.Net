using opensis.data.Helper;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.StudentSchedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

                            if (studentCourseSectionScheduleAddViewModel.studentMasterList.Count > 0)
                            {
                                if (restSeats > 0)
                                {
                                    studentData = studentCourseSectionScheduleAddViewModel.studentMasterList.Take(restSeats).ToList();
                                }
                                else
                                {
                                    studentData = studentCourseSectionScheduleAddViewModel.studentMasterList.ToList();
                                }

                                if (studentData.Count > 0)
                                {
                                    foreach (var student in studentData)
                                    {
                                        var studentCourseSectionSchedule = this.context?.StudentCoursesectionSchedule.FirstOrDefault(c => c.SchoolId == student.SchoolId && c.TenantId == student.TenantId && c.StudentId == student.StudentId && c.CourseSectionId == courseSection.CourseSectionId && c.AcademicYear == courseSection.AcademicYear);

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
                                                StudentName = student.FirstGivenName,
                                                Scheduled = false,
                                                ConflictComment = "Student is already scheduled in the course section"
                                            };
                                            this.context.StudentScheduleView.Add(conflictStudent);
                                        }
                                        else
                                        {
                                            var checkStudentConflict = this.context.CourseSection.FirstOrDefault(c => c.TenantId == courseSection.TenantId && c.SchoolId == courseSection.SchoolId && c.CourseSectionId == courseSection.CourseSectionId);

                                            if (checkStudentConflict.AllowStudentConflict != null && (bool)checkStudentConflict.AllowStudentConflict)
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
                                                    FirstLanguageId = (int)student.FirstLanguageId,
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
                                                    StudentName = student.FirstGivenName,
                                                    Scheduled = true,
                                                };
                                                this.context?.StudentScheduleView.Add(conflictStudent);
                                            }
                                            if (checkStudentConflict.AllowStudentConflict == null || !(bool)checkStudentConflict.AllowStudentConflict)
                                            {
                                                var courseSectionAllData = this.context?.AllCourseSectionView.Where(c => c.SchoolId == courseSection.SchoolId && c.TenantId == courseSection.TenantId && c.CourseSectionId == courseSection.CourseSectionId && c.AcademicYear == courseSection.AcademicYear).ToList();

                                                if (courseSectionAllData.Count > 0)
                                                {
                                                    bool isPeriodConflict = false;
                                                    foreach (var courseSectionAll in courseSectionAllData)
                                                    {
                                                        var courseSectionData = this.context?.AllCourseSectionView.
                                                                                Join(this.context?.StudentCoursesectionSchedule,
                                                                                cs => cs.CourseSectionId, ccs => ccs.CourseSectionId,
                                                                                (cs, ccs) => new { cs, ccs }).FirstOrDefault(c => c.cs.TenantId == courseSection.TenantId && c.cs.SchoolId == courseSection.SchoolId && c.ccs.CourseSectionId != courseSection.CourseSectionId && c.ccs.StudentId == student.StudentId && c.ccs.AcademicYear == courseSectionAll.AcademicYear && c.cs.DurationEndDate > courseSectionAll.DurationStartDate && (c.cs.FixedPeriodId != null && c.cs.FixedPeriodId == courseSectionAll.FixedPeriodId) || (c.cs.VarPeriodId != null && c.cs.VarPeriodId == courseSectionAll.VarPeriodId) || (c.cs.CalPeriodId != null && c.cs.CalPeriodId == courseSectionAll.CalPeriodId) || (c.cs.BlockPeriodId != null && c.cs.BlockPeriodId == courseSectionAll.BlockPeriodId));

                                                        
                                                        if (courseSectionData != null)
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
                                                            FirstLanguageId = (int)student.FirstLanguageId,
                                                            GradeId = student.StudentEnrollment.FirstOrDefault().GradeId,
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
                                                            StudentName = student.FirstGivenName,
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
                                                            StudentName = student.FirstGivenName,
                                                            Scheduled = false,
                                                            ConflictComment = "There is a period conflict"
                                                        };
                                                        this.context?.StudentScheduleView.Add(conflictStudent);
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
                                return studentCourseSectionScheduleAddViewModel;
                            }
                        }
                        var studentScheduleViewData = this.context?.StudentScheduleView.Where(e => e.SchoolId == studentCourseSectionScheduleAddViewModel.SchoolId && e.TenantId == studentCourseSectionScheduleAddViewModel.TenantId).ToList();

                        if (studentScheduleViewData.Count>0)
                        {
                            this.context?.StudentScheduleView.RemoveRange(studentScheduleViewData);
                        }
                        this.context?.SaveChanges();
                        transaction.Commit();
                        studentCourseSectionScheduleAddViewModel._message = "Student Schedule Added Successfully";
                        studentCourseSectionScheduleAddViewModel._failure = false;
                    }
                    else
                    {
                        studentCourseSectionScheduleAddViewModel._message = "Select Atleast One Course Section";
                        studentCourseSectionScheduleAddViewModel._failure = true;
                        return studentCourseSectionScheduleAddViewModel;
                    }
                }
                catch (Exception es)
                {
                    transaction.Rollback();
                    studentCourseSectionScheduleAddViewModel._failure = true;
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
        public ScheduleStudentListViewModel SearchScheduledStudentForGroupDrop(ScheduleStudentListViewModel scheduleStudentListViewModel)
        {
            ScheduleStudentListViewModel scheduleStudentListView = new ScheduleStudentListViewModel();
            try
            {
                var scheduledStudentData= this.context?.StudentCoursesectionSchedule.
                                    Join(this.context?.StudentMaster,
                                    scs => scs.StudentId, sm => sm.StudentId,
                                    (scs, sm) => new { scs, sm }).Where(c => c.scs.TenantId == scheduleStudentListViewModel.TenantId && c.scs.SchoolId == scheduleStudentListViewModel.SchoolId && c.scs.CourseSectionId == scheduleStudentListViewModel.CourseSectionId &&c.sm.SchoolId== scheduleStudentListViewModel.SchoolId && c.sm.TenantId== scheduleStudentListViewModel.TenantId).ToList().Select(ssv=>new ScheduleStudentForView 
                                    { 
                                        SchoolId=ssv.sm.SchoolId,
                                        TenantId=ssv.sm.TenantId,
                                        StudentId=ssv.sm.StudentId,
                                        FirstGivenName= ssv.sm.FirstGivenName,
                                        LastFamilyName= ssv.sm.LastFamilyName,
                                        AlternateId =ssv.sm.AlternateId,
                                        GradeLevel = this.context.Gradelevels.FirstOrDefault(c=>c.TenantId==ssv.sm.TenantId && c.SchoolId == ssv.sm.SchoolId && c.GradeId==ssv.scs.GradeId)?.Title,
                                        Section= this.context.Sections.FirstOrDefault(c => c.TenantId == ssv.sm.TenantId && c.SchoolId == ssv.sm.SchoolId && c.SectionId == ssv.sm.SectionId)?.Name,
                                        PhoneNumber= ssv.sm.MobilePhone,
                                        Action= ssv.scs.IsDropped
                                    }).ToList();

                if (scheduledStudentData.Count()>0)
                {
                    scheduleStudentListView.scheduleStudentForView=scheduledStudentData;
                    scheduleStudentListView.TenantId = scheduleStudentListViewModel.TenantId;
                    scheduleStudentListView.SchoolId = scheduleStudentListViewModel.SchoolId;
                    scheduleStudentListView.CourseSectionId = scheduleStudentListViewModel.CourseSectionId;
                    scheduleStudentListView._failure = false;
                }
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
                if (scheduledStudentDropModel.studentCoursesectionScheduleList.Count>0)
                {
                    if (scheduledStudentDropModel.EffectiveDropDate>=DateTime.UtcNow)
                    {
                        foreach (var scheduledStudent in scheduledStudentDropModel.studentCoursesectionScheduleList)
                        {
                            var studentData = this.context?.StudentCoursesectionSchedule.FirstOrDefault(x => x.SchoolId == scheduledStudent.SchoolId && x.TenantId == scheduledStudent.TenantId && x.StudentId == scheduledStudent.StudentId && x.CourseSectionId == scheduledStudentDropModel.CourseSectionId);

                            if (studentData != null)
                            {
                                studentData.IsDropped = true;
                                studentData.EffectiveDropDate = scheduledStudentDropModel.EffectiveDropDate;
                                this.context?.UpdateRange(studentData);
                            }
                            else
                            {
                                scheduledStudentDropModel._message = NORECORDFOUND;
                                scheduledStudentDropModel._failure = true;
                            }
                        }
                        this.context?.SaveChanges();
                        scheduledStudentDropModel._failure = false;
                        scheduledStudentDropModel._message = "";
                    }
                    else
                    {
                        scheduledStudentDropModel._message = "";
                        scheduledStudentDropModel._failure = true;
                    }
                    
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
                var scheduleReport = this.context?.StudentScheduleView.Where(x => x.SchoolId == 1).ToPivotTable(
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
    }
}
