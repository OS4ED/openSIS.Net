using Microsoft.EntityFrameworkCore;
using opensis.data.Helper;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.Grades;
using opensis.data.ViewModels.InputFinalGrade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace opensis.data.Repository
{
    public class InputFinalGradeRepository : IInputFinalGradeRepository
    {
        private CRMContext context;
        private static readonly string NORECORDFOUND = "No Record Found";
        public InputFinalGradeRepository(IDbContextFactory dbContextFactory)
        {
            this.context = dbContextFactory.Create();
        }

        /// <summary>
        /// Add Update Student Final Grade
        /// </summary>
        /// <param name="studentFinalGradeListModel"></param>
        /// <returns></returns>
        public StudentFinalGradeListModel AddUpdateStudentFinalGrade(StudentFinalGradeListModel studentFinalGradeListModel)
        {
            using (var transaction = this.context.Database.BeginTransaction())
            {
                try
                {
                    List<StudentFinalGrade> studentFinalGradeList = new List<StudentFinalGrade>();
                    //List<StudentFinalGradeStandard> studentFinalGradeStandardList = new List<StudentFinalGradeStandard>();
                    List<StudentFinalGradeComments> studentFinalGradeCommentList = new List<StudentFinalGradeComments>();

                    int Id = 1;
                    var StudentFinalStandard = this.context?.StudentFinalGradeStandard.ToList();

                    if (StudentFinalStandard.Count > 0)
                    {
                        Id = Convert.ToInt32(StudentFinalStandard.OrderByDescending(s=>s.Id).FirstOrDefault().Id + 1);
                    }

                    int? YrMarkingPeriodId = 0;
                    int? SmstrMarkingPeriodId = 0;
                    int? QtrMarkingPeriodId = 0;

                    if (studentFinalGradeListModel.MarkingPeriodId != null)
                    {
                        var markingPeriodid = studentFinalGradeListModel.MarkingPeriodId.Split("_", StringSplitOptions.RemoveEmptyEntries);

                        if (markingPeriodid.First() == "2")
                        {
                            QtrMarkingPeriodId = Int32.Parse(markingPeriodid.ElementAt(1));
                        }
                        if (markingPeriodid.First() == "1")
                        {
                            SmstrMarkingPeriodId = Int32.Parse(markingPeriodid.ElementAt(1));
                        }
                        if (markingPeriodid.First() == "0")
                        {
                            YrMarkingPeriodId = Int32.Parse(markingPeriodid.ElementAt(1));
                        }
                    }
                    foreach (var studentFinalGrade in studentFinalGradeListModel.studentFinalGradeList)
                    {
                        foreach (var studentFinalGradeStandarddata in studentFinalGrade.StudentFinalGradeStandard)
                        {
                            studentFinalGradeStandarddata.Id = Id;
                            studentFinalGradeStandarddata.QtrMarkingPeriodId = (QtrMarkingPeriodId > 0) ? QtrMarkingPeriodId : null;
                            studentFinalGradeStandarddata.SmstrMarkingPeriodId = (SmstrMarkingPeriodId > 0) ? SmstrMarkingPeriodId : null;
                            studentFinalGradeStandarddata.YrMarkingPeriodId = (YrMarkingPeriodId > 0) ? YrMarkingPeriodId : null;
                            studentFinalGradeStandarddata.CalendarId = studentFinalGradeListModel.CalendarId;
                            studentFinalGradeStandarddata.AcademicYear = studentFinalGradeListModel.AcademicYear;
                            studentFinalGradeStandarddata.TeacherComment = studentFinalGrade.TeacherComment;
                            Id++;
                        }
                    }

                    if (studentFinalGradeListModel.studentFinalGradeList.Count > 0)
                    {
                        var studentFinalGradeData = new List<StudentFinalGrade>();

                        //if (studentFinalGradeListModel.MarkingPeriodId != null)
                        //{
                            studentFinalGradeData = this.context?.StudentFinalGrade.Where(e => e.SchoolId == studentFinalGradeListModel.SchoolId && e.TenantId == studentFinalGradeListModel.TenantId && e.CourseId == studentFinalGradeListModel.CourseId && e.CourseSectionId == studentFinalGradeListModel.CourseSectionId && e.CalendarId == studentFinalGradeListModel.CalendarId /*&& (YrMarkingPeriodId > 0 && e.YrMarkingPeriodId == YrMarkingPeriodId || SmstrMarkingPeriodId > 0 && e.SmstrMarkingPeriodId == SmstrMarkingPeriodId || QtrMarkingPeriodId > 0 && e.QtrMarkingPeriodId == QtrMarkingPeriodId)*/).ToList();
                        //}
                        //else
                        //{
                        //    studentFinalGradeData = this.context?.StudentFinalGrade.Where(e => e.SchoolId == studentFinalGradeListModel.SchoolId && e.TenantId == studentFinalGradeListModel.TenantId && e.CourseId == studentFinalGradeListModel.CourseId && e.CourseSectionId == studentFinalGradeListModel.CourseSectionId && e.CalendarId == studentFinalGradeListModel.CalendarId && e.YrMarkingPeriodId == null && e.SmstrMarkingPeriodId == null && e.QtrMarkingPeriodId == null).ToList();
                        //}

                        if (studentFinalGradeData.Count > 0)
                        {

                            var containStudentFinalGradeSrlno = studentFinalGradeData.Select(x => x.StudentFinalGradeSrlno).Distinct().ToList();

                            List<long> studentFinalGradeSrlnos = new List<long> { };
                            studentFinalGradeSrlnos = containStudentFinalGradeSrlno;

                            var studentFinalGradeStandardData = this.context?.StudentFinalGradeStandard.Where(e => e.SchoolId == studentFinalGradeListModel.SchoolId && e.TenantId == studentFinalGradeListModel.TenantId && e.CalendarId == studentFinalGradeListModel.CalendarId && /*(YrMarkingPeriodId > 0 && e.YrMarkingPeriodId == YrMarkingPeriodId || SmstrMarkingPeriodId > 0 && e.SmstrMarkingPeriodId == SmstrMarkingPeriodId || QtrMarkingPeriodId > 0 && e.QtrMarkingPeriodId == QtrMarkingPeriodId) &&*/ (studentFinalGradeSrlnos == null || (studentFinalGradeSrlnos.Contains(e.StudentFinalGradeSrlno)))).ToList();

                            if (studentFinalGradeStandardData.Count > 0)
                            {

                                this.context?.StudentFinalGradeStandard.RemoveRange(studentFinalGradeStandardData);
                            }

                            var studentFinalGradeCommentsData = this.context?.StudentFinalGradeComments.Where(e => e.SchoolId == studentFinalGradeListModel.SchoolId && e.TenantId == studentFinalGradeListModel.TenantId && (studentFinalGradeSrlnos == null || (studentFinalGradeSrlnos.Contains(e.StudentFinalGradeSrlno)))).ToList();

                            if (studentFinalGradeCommentsData.Count > 0)
                            {
                                this.context?.StudentFinalGradeComments.RemoveRange(studentFinalGradeCommentsData);
                            }
                            this.context?.StudentFinalGrade.RemoveRange(studentFinalGradeData);
                            this.context?.SaveChanges();


                            long? studentFinalGradeSrlno = 1;

                            var studentFinalGradeSrlnoData = this.context?.StudentFinalGrade.Where(x => x.SchoolId == studentFinalGradeListModel.SchoolId && x.TenantId == studentFinalGradeListModel.TenantId).OrderByDescending(x => x.StudentFinalGradeSrlno).FirstOrDefault();

                            if (studentFinalGradeSrlnoData != null)
                            {
                                studentFinalGradeSrlno = studentFinalGradeSrlnoData.StudentFinalGradeSrlno + 1;
                            }

                            foreach (var studentFinalGrade in studentFinalGradeListModel.studentFinalGradeList)
                            {
                                var studentFinalGradeUpdate = new StudentFinalGrade()
                                {
                                    TenantId = studentFinalGradeListModel.TenantId,
                                    SchoolId = studentFinalGradeListModel.SchoolId,
                                    StudentId = studentFinalGrade.StudentId,
                                    CourseId = studentFinalGradeListModel.CourseId,
                                    CourseSectionId = studentFinalGradeListModel.CourseSectionId,
                                    GradeId = studentFinalGrade.GradeId,
                                    GradeScaleId = studentFinalGrade.GradeScaleId,
                                    AcademicYear = studentFinalGradeListModel.AcademicYear,
                                    CalendarId = studentFinalGradeListModel.CalendarId,
                                    YrMarkingPeriodId = (YrMarkingPeriodId > 0) ? YrMarkingPeriodId : null,
                                    SmstrMarkingPeriodId = (SmstrMarkingPeriodId > 0) ? SmstrMarkingPeriodId : null,
                                    QtrMarkingPeriodId = (QtrMarkingPeriodId > 0) ? QtrMarkingPeriodId : null,
                                    IsPercent = studentFinalGradeListModel.IsPercent,
                                    PercentMarks = studentFinalGrade.PercentMarks,
                                    GradeObtained = studentFinalGrade.GradeObtained,
                                    UpdatedBy = studentFinalGradeListModel.CreatedOrUpdatedBy,
                                    UpdatedOn = DateTime.UtcNow,
                                    StudentFinalGradeSrlno = (long)studentFinalGradeSrlno,
                                    BasedOnStandardGrade = studentFinalGrade.BasedOnStandardGrade,
                                    TeacherComment = studentFinalGrade.TeacherComment,
                                    StudentFinalGradeComments = studentFinalGrade.StudentFinalGradeComments?.Select(c =>
                                    {
                                        c.UpdatedBy = studentFinalGradeListModel.CreatedOrUpdatedBy;
                                        c.UpdatedOn = DateTime.UtcNow;
                                        return c;
                                    }).ToList(),
                                    StudentFinalGradeStandard = studentFinalGrade.StudentFinalGradeStandard?.Select(c =>
                                     {
                                         c.UpdatedBy = studentFinalGradeListModel.CreatedOrUpdatedBy;
                                         c.UpdatedOn = DateTime.UtcNow;
                                         return c;
                                     }).ToList()
                                };
                                studentFinalGradeList.Add(studentFinalGradeUpdate);
                                studentFinalGradeSrlno++;
                            }
                            studentFinalGradeListModel._message = "Student Final Grade Updated Succsesfully.";
                        }
                        else
                        {
                            long? studentFinalGradeSrlno = 1;

                            var studentFinalGradeSrlnoData = this.context?.StudentFinalGrade.Where(x => x.SchoolId == studentFinalGradeListModel.SchoolId && x.TenantId == studentFinalGradeListModel.TenantId).OrderByDescending(x => x.StudentFinalGradeSrlno).FirstOrDefault();

                            if (studentFinalGradeSrlnoData != null)
                            {
                                studentFinalGradeSrlno = studentFinalGradeSrlnoData.StudentFinalGradeSrlno + 1;
                            }

                            foreach (var studentFinalGrade in studentFinalGradeListModel.studentFinalGradeList)
                            {
                                var studentFinalGradeAdd = new StudentFinalGrade()
                                {
                                    TenantId = studentFinalGradeListModel.TenantId,
                                    SchoolId = studentFinalGradeListModel.SchoolId,
                                    StudentId = studentFinalGrade.StudentId,
                                    CourseId = studentFinalGradeListModel.CourseId,
                                    CourseSectionId = studentFinalGradeListModel.CourseSectionId,
                                    GradeId = studentFinalGrade.GradeId,
                                    GradeScaleId = studentFinalGrade.GradeScaleId,
                                    AcademicYear = studentFinalGradeListModel.AcademicYear,
                                    CalendarId = studentFinalGradeListModel.CalendarId,
                                    YrMarkingPeriodId = (YrMarkingPeriodId > 0) ? YrMarkingPeriodId : null,
                                    SmstrMarkingPeriodId = (SmstrMarkingPeriodId > 0) ? SmstrMarkingPeriodId : null,
                                    QtrMarkingPeriodId = (QtrMarkingPeriodId > 0) ? QtrMarkingPeriodId : null,
                                    IsPercent = studentFinalGradeListModel.IsPercent,
                                    PercentMarks = studentFinalGrade.PercentMarks,
                                    GradeObtained = studentFinalGrade.GradeObtained,
                                    CreatedBy = studentFinalGradeListModel.CreatedOrUpdatedBy,
                                    CreatedOn = DateTime.UtcNow,
                                    StudentFinalGradeSrlno = (long)studentFinalGradeSrlno,
                                    BasedOnStandardGrade = studentFinalGrade.BasedOnStandardGrade,
                                    TeacherComment = studentFinalGrade.TeacherComment,
                                    StudentFinalGradeComments = studentFinalGrade.StudentFinalGradeComments?.Select(c => { c.CreatedBy = studentFinalGradeListModel.CreatedOrUpdatedBy; c.CreatedOn = DateTime.UtcNow; return c; }).ToList(),
                                    StudentFinalGradeStandard = studentFinalGrade.StudentFinalGradeStandard?.Select(c =>
                                    {
                                        c.CreatedBy = studentFinalGradeListModel.CreatedOrUpdatedBy;
                                        c.CreatedOn = DateTime.UtcNow;
                                        return c;
                                    }).ToList()
                                };
                                studentFinalGradeList.Add(studentFinalGradeAdd);
                                studentFinalGradeSrlno++;
                            }
                            studentFinalGradeListModel._message = "Student Final Grade Added succsesfully.";
                        }
                        this.context?.StudentFinalGrade.AddRange(studentFinalGradeList);
                        //this.context?.StudentFinalGradeStandard.AddRange(studentFinalGradeStandardList);
                        this.context?.SaveChanges();
                        transaction.Commit();
                        studentFinalGradeListModel._failure = false;
                    }
                }
                catch (Exception es)
                {
                    transaction.Rollback();
                    studentFinalGradeListModel._failure = true;
                    studentFinalGradeListModel._message = es.Message;
                }
            }
            return studentFinalGradeListModel;
        }

        /// <summary>
        /// Get All Student Final Grade List
        /// </summary>
        /// <param name="studentFinalGradeListModel"></param>
        /// <returns></returns>
        public StudentFinalGradeListModel GetAllStudentFinalGradeList(StudentFinalGradeListModel studentFinalGradeListModel)
        {
            StudentFinalGradeListModel studentFinalGradeList = new StudentFinalGradeListModel();
            try
            {
                int? YrMarkingPeriodId = 0;
                int? SmstrMarkingPeriodId = 0;
                int? QtrMarkingPeriodId = 0;

                if (studentFinalGradeListModel.MarkingPeriodId != null)
                {

                    var markingPeriodid = studentFinalGradeListModel.MarkingPeriodId.Split("_", StringSplitOptions.RemoveEmptyEntries);

                    if (markingPeriodid.First() == "2")
                    {
                        QtrMarkingPeriodId = Int32.Parse(markingPeriodid.ElementAt(1));
                    }
                    if (markingPeriodid.First() == "1")
                    {
                        SmstrMarkingPeriodId = Int32.Parse(markingPeriodid.ElementAt(1));
                    }
                    if (markingPeriodid.First() == "0")
                    {
                        YrMarkingPeriodId = Int32.Parse(markingPeriodid.ElementAt(1));
                    }
                }

                var studentFinalGradeData = new List<StudentFinalGrade>();

                //if (studentFinalGradeListModel.MarkingPeriodId != null)
                //{
                    studentFinalGradeData = this.context?.StudentFinalGrade.Include(x => x.StudentFinalGradeStandard).Include(d => d.StudentFinalGradeComments).ThenInclude(y => y.CourseCommentCategory).Where(e => e.SchoolId == studentFinalGradeListModel.SchoolId && e.TenantId == studentFinalGradeListModel.TenantId && e.CourseId == studentFinalGradeListModel.CourseId && e.CourseSectionId == studentFinalGradeListModel.CourseSectionId && e.CalendarId == studentFinalGradeListModel.CalendarId /*&& (YrMarkingPeriodId > 0 && e.YrMarkingPeriodId == YrMarkingPeriodId || SmstrMarkingPeriodId > 0 && e.SmstrMarkingPeriodId == SmstrMarkingPeriodId || QtrMarkingPeriodId > 0 && e.QtrMarkingPeriodId == QtrMarkingPeriodId)*/).ToList();
                //}
                //else
                //{
                //    studentFinalGradeData = this.context?.StudentFinalGrade.Include(x => x.StudentFinalGradeStandard).Include(d => d.StudentFinalGradeComments).ThenInclude(y => y.CourseCommentCategory).Where(e => e.SchoolId == studentFinalGradeListModel.SchoolId && e.TenantId == studentFinalGradeListModel.TenantId && e.CourseId == studentFinalGradeListModel.CourseId && e.CourseSectionId == studentFinalGradeListModel.CourseSectionId && e.CalendarId == studentFinalGradeListModel.CalendarId && e.YrMarkingPeriodId == null && e.SmstrMarkingPeriodId == null && e.QtrMarkingPeriodId == null).ToList();
                //}

                if (studentFinalGradeData.Count > 0)
                {
                    studentFinalGradeList.studentFinalGradeList = studentFinalGradeData;
                    studentFinalGradeList.TenantId = studentFinalGradeListModel.TenantId;
                    studentFinalGradeList.SchoolId = studentFinalGradeListModel.SchoolId;
                    studentFinalGradeList.CalendarId = studentFinalGradeListModel.CalendarId;
                    studentFinalGradeList.CourseId = studentFinalGradeListModel.CourseId;
                    studentFinalGradeList.CourseSectionId = studentFinalGradeListModel.CourseSectionId;
                    studentFinalGradeList.StandardGradeScaleId = studentFinalGradeListModel.StandardGradeScaleId;
                    studentFinalGradeList.MarkingPeriodId = studentFinalGradeListModel.MarkingPeriodId;
                    studentFinalGradeList.AcademicYear = studentFinalGradeListModel.AcademicYear;
                    studentFinalGradeList.IsPercent = studentFinalGradeListModel.IsPercent;
                    studentFinalGradeList.CreatedOrUpdatedBy = studentFinalGradeListModel.CreatedOrUpdatedBy;
                    studentFinalGradeList._userName = studentFinalGradeListModel._userName;
                    studentFinalGradeList._tenantName = studentFinalGradeListModel._tenantName;
                    studentFinalGradeList._token = studentFinalGradeListModel._token;
                    studentFinalGradeList._failure = false;

                    if (studentFinalGradeList.studentFinalGradeList.Count > 0)
                    {
                        //foreach (var studentFinalGrade in studentFinalGradeList.studentFinalGradeList)
                        //{
                        //    foreach (var FinalGrade in studentFinalGrade.StudentFinalGradeComments)
                        //    {
                        //        FinalGrade.CourseCommentCategory.StudentFinalGradeComments = null;
                        //    }
                        //}
                        studentFinalGradeList.studentFinalGradeList.ForEach(f => f.StudentFinalGradeComments.ToList().ForEach(r => r.CourseCommentCategory.StudentFinalGradeComments = null));
                    }
                }
                else
                {
                    studentFinalGradeList._failure = true;
                    studentFinalGradeList._message = NORECORDFOUND;
                }

            }
            catch (Exception es)
            {
                studentFinalGradeList._message = es.Message;
                studentFinalGradeList._failure = true;
            }
            return studentFinalGradeList;
        }

        //public ReportCardCommentListViewModel GetReportCardCommentsForInputFinalGrade(ReportCardCommentListViewModel reportCardCommentListViewModel)
        //{
        //    try
        //    {
        //        var ReportCardCommentsData = this.context?.ReportCardComments.Where(x => x.TenantId == reportCardCommentListViewModel.TenantId && x.SchoolId == reportCardCommentListViewModel.SchoolId && (x.CourseSectionId == reportCardCommentListViewModel.CourseSectionId || x.ApplicableAllCourses == true) && x.CourseId == reportCardCommentListViewModel.CourseId).ToList();

        //        if (ReportCardCommentsData.Count > 0)
        //        {
        //            reportCardCommentListViewModel.reportCardCommentList = ReportCardCommentsData;
        //            reportCardCommentListViewModel._failure = false;
        //        }
        //        else
        //        {
        //            reportCardCommentListViewModel._failure = true;
        //            reportCardCommentListViewModel._message = NORECORDFOUND;
        //        }
        //    }
        //    catch (Exception es)
        //    {
        //        reportCardCommentListViewModel._message = es.Message;
        //        reportCardCommentListViewModel._failure = true;
        //    }
        //    return reportCardCommentListViewModel;
        //}
    }
}
