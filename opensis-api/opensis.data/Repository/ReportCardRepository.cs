using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.ReportCard;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using opensis.data.ViewModels.CourseManager;
using System.Text.RegularExpressions;

namespace opensis.data.Repository
{
    public class ReportCardRepository : IReportCardRepository
    {
        private CRMContext context;
        private static readonly string NORECORDFOUND = "No Record Found";
        public ReportCardRepository(IDbContextFactory dbContextFactory)
        {
            this.context = dbContextFactory.Create();
        }
        /// <summary>
        /// Add Report Card Comments
        /// </summary>
        /// <param name="reportCardViewModel"></param>
        /// <returns></returns>

        //public ReportCardCommentsAddViewModel AddReportCardComments(ReportCardCommentsAddViewModel reportCardCommentsAddViewModel)
        //{
        //    using (var transaction = this.context.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            List<ReportCardComments> reportCardComments = new List<ReportCardComments>();

        //            int? id = 1;
        //            int? SortOrder = 1;

        //            if (reportCardCommentsAddViewModel.reportCardComments.Count > 0)
        //            {
        //                //var reportCardCommentData = this.context?.ReportCardComments.Where(x => x.TenantId == reportCardCommentsAddViewModel.TenantId && x.SchoolId == reportCardCommentsAddViewModel.SchoolId).OrderByDescending(x => x.Id).FirstOrDefault();

        //                //if (reportCardCommentData != null)
        //                //{
        //                //    id = Convert.ToInt32(reportCardCommentData.Id) + 1;
        //                //}

        //                //var reportCardCommentSortOrder = this.context?.ReportCardComments.Where(x => x.TenantId == reportCardCommentsAddViewModel.TenantId && x.SchoolId == reportCardCommentsAddViewModel.SchoolId && x.CourseCommentId == reportCardCommentsAddViewModel.CourseCommentId).OrderByDescending(x => x.Id).FirstOrDefault();

        //                //if (reportCardCommentSortOrder != null)
        //                //{
        //                //    SortOrder = reportCardCommentSortOrder.SortOrder + 1;
        //                //}

        //                //foreach (var reportComment in reportCardCommentsAddViewModel.reportCardComments.ToList())
        //                //{

        //                //    reportComment.TenantId = reportCardCommentsAddViewModel.TenantId;
        //                //    reportComment.SchoolId = reportCardCommentsAddViewModel.SchoolId;
        //                //    reportComment.CourseCommentId = reportCardCommentsAddViewModel.CourseCommentId;
        //                //    reportComment.CourseId = 1;
        //                //    reportComment.CourseSectionId = 1;
        //                //    reportComment.SortOrder = SortOrder;
        //                //    reportComment.Id = id;
        //                //    reportComment.CreatedOn = DateTime.UtcNow;
        //                //    reportCardComments.Add(reportComment);
        //                //    id++;
        //                //    SortOrder++;
        //                //}
        //                //reportCardCommentsAddViewModel._message = "Report Comments added succsesfully.";

        //                //this.context?.ReportCardComments.AddRange(reportCardComments);
        //                //this.context?.SaveChanges();
        //                //transaction.Commit();
        //                //reportCardCommentsAddViewModel._failure = false;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            transaction.Rollback();
        //            reportCardCommentsAddViewModel._failure = true;
        //            reportCardCommentsAddViewModel._message = ex.Message;
        //        }
        //        return reportCardCommentsAddViewModel;
        //    }
        //}

        ///// <summary>
        ///// Update Report Card Comments
        ///// </summary>
        ///// <param name="reportCardViewModel"></param>
        ///// <returns></returns>

        //public ReportCardCommentsAddViewModel UpdateReportCardComments(ReportCardCommentsAddViewModel reportCardCommentsAddViewModel)
        //{
        //    try
        //    {
        //        List<ReportCardComments> reportCardComments = new List<ReportCardComments>();

        //        if (reportCardCommentsAddViewModel.reportCardComments.Count > 0)
        //        {
        //           /* var reportCommentDataExist = this.context?.ReportCardComments.Where(x => x.TenantId == reportCardCommentsAddViewModel.TenantId && x.SchoolId == reportCardCommentsAddViewModel.SchoolId && x.CourseCommentId == reportCardCommentsAddViewModel.CourseCommentId).ToList();

        //            if (reportCommentDataExist.Count > 0)
        //            {
        //                this.context?.ReportCardComments.RemoveRange(reportCommentDataExist);
        //                this.context?.SaveChanges();
        //            }

        //            int? id = 1;
        //            int? SortOrder = 1;

        //            var reportCardCommentData = this.context?.ReportCardComments.Where(x => x.TenantId == reportCardCommentsAddViewModel.TenantId && x.SchoolId == reportCardCommentsAddViewModel.SchoolId).OrderByDescending(x => x.Id).FirstOrDefault();

        //            if (reportCardCommentData != null)
        //            {
        //                id = Convert.ToInt32(reportCardCommentData.Id) + 1;
        //            }

        //            var reportCardCommentSortOrder = this.context?.ReportCardComments.Where(x => x.TenantId == reportCardCommentsAddViewModel.TenantId && x.SchoolId == reportCardCommentsAddViewModel.SchoolId && x.CourseCommentId == reportCardCommentsAddViewModel.CourseCommentId).OrderByDescending(x => x.SortOrder).FirstOrDefault();

        //            if (reportCardCommentSortOrder != null)
        //            {
        //                SortOrder = reportCardCommentSortOrder.SortOrder + 1;
        //            }

        //            foreach (var reportComment in reportCardCommentsAddViewModel.reportCardComments.ToList())
        //            {

        //                reportComment.TenantId = reportCardCommentsAddViewModel.TenantId;
        //                reportComment.SchoolId = reportCardCommentsAddViewModel.SchoolId;
        //                reportComment.CourseCommentId = reportCardCommentsAddViewModel.CourseCommentId;
        //                reportComment.CourseId = 1;
        //                reportComment.CourseSectionId = 1;

        //                reportComment.SortOrder = SortOrder;
        //                reportComment.Id = id;
        //                reportComment.UpdatedOn = DateTime.UtcNow;
        //                reportCardComments.Add(reportComment);
        //                id++;
        //                SortOrder++;
        //            }
        //            reportCardCommentsAddViewModel._message = "Report Comments Updated succsesfully.";

        //            this.context?.ReportCardComments.AddRange(reportCardComments);
        //            this.context?.SaveChanges();
        //            //transaction.Commit();
        //            reportCardCommentsAddViewModel._failure = false;*/
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //transaction.Rollback();
        //        reportCardCommentsAddViewModel._failure = true;
        //        reportCardCommentsAddViewModel._message = ex.Message;
        //    }
        //    return reportCardCommentsAddViewModel;
        //}

        ///// <summary>
        ///// Delete Report Card Comments
        ///// </summary>
        ///// <param name="reportCardViewModel"></param>
        ///// <returns></returns>
        //public ReportCardCommentsAddViewModel DeleteReportCardComments(ReportCardCommentsAddViewModel reportCardCommentsAddViewModel)
        //{
        //    try
        //    {
        //       /* if (reportCardCommentsAddViewModel.reportCardComments.Count > 0)
        //        {
        //            var reportCardCommentDataExist = this.context?.ReportCardComments.FirstOrDefault(x => x.TenantId == reportCardCommentsAddViewModel.TenantId && x.SchoolId == reportCardCommentsAddViewModel.SchoolId && x.Id == reportCardCommentsAddViewModel.reportCardComments.FirstOrDefault().Id && x.CourseCommentId == reportCardCommentsAddViewModel.CourseCommentId);

        //            if (reportCardCommentDataExist != null)
        //            {
        //                this.context?.ReportCardComments.Remove(reportCardCommentDataExist);
        //                this.context?.SaveChanges();
        //                reportCardCommentsAddViewModel._failure = false;
        //                reportCardCommentsAddViewModel._message = "Deleted Successfully";
        //            }
        //            else
        //            {
        //                reportCardCommentsAddViewModel._failure = true;
        //                reportCardCommentsAddViewModel._message = NORECORDFOUND;
        //            }
        //        }*/
        //    }
        //    catch (Exception ex)
        //    {
        //        reportCardCommentsAddViewModel._failure = true;
        //        reportCardCommentsAddViewModel._message = ex.Message;
        //    }
        //    return reportCardCommentsAddViewModel;
        //}


        /// <summary>
        /// Add Course Comment Category
        /// </summary>
        /// <param name="courseCommentCategoryAddViewModel"></param>
        /// <returns></returns>

        public CourseCommentCategoryAddViewModel AddCourseCommentCategory(CourseCommentCategoryAddViewModel courseCommentCategoryAddViewModel)
        {
            try
            {
                List<CourseCommentCategory> courseCommentCategoryList = new List<CourseCommentCategory>();
                int i = 0;
                var distinctCourseData = courseCommentCategoryAddViewModel.courseCommentCategory.Select(s => new { s.CourseId, s.TenantId, s.SchoolId }).Distinct().ToList();

                int? courseCommentId = 1;

                foreach (var course in distinctCourseData.ToList())
                {
                    var courseCommentCategoryDataExist = this.context?.CourseCommentCategory.Where(x => x.TenantId == course.TenantId && x.SchoolId == course.SchoolId && x.CourseId == course.CourseId).ToList();

                    if (courseCommentCategoryDataExist.Count > 0)
                    {
                        this.context?.CourseCommentCategory.RemoveRange(courseCommentCategoryDataExist);
                        this.context?.SaveChanges();
                    }

                    var courseCommentCategoryData = courseCommentCategoryAddViewModel.courseCommentCategory.Where(x => x.CourseId == course.CourseId).ToList();

                    int? sortOrder = 1;
                    int? sortOrderForAllCourse = 1;

                    if (i == 0)
                    {
                        var courseCommentCategoryDataForID = this.context?.CourseCommentCategory.Where(x => x.TenantId == course.TenantId && x.SchoolId == course.SchoolId).OrderByDescending(x => x.CourseCommentId).FirstOrDefault();

                        if (courseCommentCategoryDataForID != null)
                        {
                            courseCommentId = courseCommentCategoryDataForID.CourseCommentId + 1;
                        }
                    }

                    foreach (var courseCommentCategory in courseCommentCategoryData)
                    {
                        courseCommentCategory.CourseCommentId = (int)courseCommentId;
                        courseCommentCategory.SortOrder = courseCommentCategory.CourseId != null ? sortOrder : sortOrderForAllCourse;
                        courseCommentCategory.CreatedOn = DateTime.UtcNow;
                        courseCommentCategoryList.Add(courseCommentCategory);
                        courseCommentId++;
                        sortOrder++;
                        sortOrderForAllCourse++;
                    }
                    i++;
                }

                this.context?.CourseCommentCategory.AddRange(courseCommentCategoryList);
                this.context?.SaveChanges();
                courseCommentCategoryAddViewModel._failure = false;
                courseCommentCategoryAddViewModel._message = "Course Comment Category Added Successfully";

            }
            catch (Exception es)
            {
                courseCommentCategoryAddViewModel._message = es.Message;
                courseCommentCategoryAddViewModel._failure = true;
            }
            return courseCommentCategoryAddViewModel;
        }

        /// <summary>
        /// Delete Course Comment Category
        /// </summary>
        /// <param name="courseCommentCategoryAddViewModel"></param>
        /// <returns></returns>
            public CourseCommentCategoryDeleteViewModel DeleteCourseCommentCategory(CourseCommentCategoryDeleteViewModel courseCommentCategoryDeleteViewModel)
        {
            try
            {
                if (courseCommentCategoryDeleteViewModel.CourseCommentId != null)
                {
                    var courseCommentData = this.context?.CourseCommentCategory.Where(x => x.TenantId == courseCommentCategoryDeleteViewModel.TenantId && x.SchoolId == courseCommentCategoryDeleteViewModel.SchoolId && x.CourseId == courseCommentCategoryDeleteViewModel.CourseId && x.CourseCommentId == courseCommentCategoryDeleteViewModel.CourseCommentId).FirstOrDefault();

                    if (courseCommentData != null)
                    {
                        this.context?.CourseCommentCategory.Remove(courseCommentData);
                    }
                }
                else
                {
                    var courseComments = this.context?.CourseCommentCategory.Where(x => x.TenantId == courseCommentCategoryDeleteViewModel.TenantId && x.SchoolId == courseCommentCategoryDeleteViewModel.SchoolId && x.CourseId == courseCommentCategoryDeleteViewModel.CourseId).ToList();

                    if (courseComments.Count > 0)
                    {
                        this.context?.CourseCommentCategory.RemoveRange(courseComments);
                    }
                }
                this.context?.SaveChanges();
                courseCommentCategoryDeleteViewModel._failure = false;
                courseCommentCategoryDeleteViewModel._message = "Course Comments Deleted Successfully";
            }
            catch (Exception es)
            {
                courseCommentCategoryDeleteViewModel._failure = true;
                courseCommentCategoryDeleteViewModel._message = es.Message;
            }
            return courseCommentCategoryDeleteViewModel;
        }

        /// <summary>
        /// Update SortOrder For CourseCommentCategory
        /// </summary>
        /// <param name="reportCardCommentSortOrderViewModel"></param>
        /// <returns></returns>

        public CourseCommentCategorySortOrderViewModel UpdateSortOrderForCourseCommentCategory(CourseCommentCategorySortOrderViewModel courseCommentCategorySortOrderViewModel)
        {
            try
            {
                //if (courseCommentCategorySortOrderViewModel.CourseId > 0)
                //{
                    var SortOrderItem = new List<CourseCommentCategory>();

                    var targetSortOrderItem = this.context?.CourseCommentCategory.FirstOrDefault(x => x.SortOrder == courseCommentCategorySortOrderViewModel.PreviousSortOrder && x.SchoolId == courseCommentCategorySortOrderViewModel.SchoolId && x.TenantId == courseCommentCategorySortOrderViewModel.TenantId && x.CourseId == courseCommentCategorySortOrderViewModel.CourseId);

                    if (targetSortOrderItem != null)
                    {
                        targetSortOrderItem.SortOrder = courseCommentCategorySortOrderViewModel.CurrentSortOrder;

                        if (courseCommentCategorySortOrderViewModel.PreviousSortOrder > courseCommentCategorySortOrderViewModel.CurrentSortOrder)
                        {
                            SortOrderItem = this.context?.CourseCommentCategory.Where(x => x.SortOrder >= courseCommentCategorySortOrderViewModel.CurrentSortOrder && x.SortOrder < courseCommentCategorySortOrderViewModel.PreviousSortOrder && x.SchoolId == courseCommentCategorySortOrderViewModel.SchoolId && x.TenantId == courseCommentCategorySortOrderViewModel.TenantId && x.CourseId == courseCommentCategorySortOrderViewModel.CourseId).ToList();

                            if (SortOrderItem.Count > 0)
                            {
                                SortOrderItem.ForEach(x => x.SortOrder = x.SortOrder + 1);
                            }
                        }

                        if (courseCommentCategorySortOrderViewModel.CurrentSortOrder > courseCommentCategorySortOrderViewModel.PreviousSortOrder)
                        {
                            SortOrderItem = this.context?.CourseCommentCategory.Where(x => x.SortOrder <= courseCommentCategorySortOrderViewModel.CurrentSortOrder && x.SortOrder > courseCommentCategorySortOrderViewModel.PreviousSortOrder && x.SchoolId == courseCommentCategorySortOrderViewModel.SchoolId && x.TenantId == courseCommentCategorySortOrderViewModel.TenantId && x.CourseId == courseCommentCategorySortOrderViewModel.CourseId).ToList();

                            if (SortOrderItem.Count > 0)
                            {
                                SortOrderItem.ForEach(x => x.SortOrder = x.SortOrder - 1);
                            }
                        }
                    }
                    this.context?.SaveChanges();
                    courseCommentCategorySortOrderViewModel._failure = false;
                //}
            }
            catch (Exception es)
            {
                courseCommentCategorySortOrderViewModel._message = es.Message;
                courseCommentCategorySortOrderViewModel._failure = true;
            }
            return courseCommentCategorySortOrderViewModel;
        }
         
        /// <summary>
        /// Get All CourseCommentCategory With ReportCardComments
        /// </summary>
        /// <param name="courseCommentCategoryListViewModel"></param>
        /// <returns></returns>
        public CourseCommentCategoryListViewModel GetAllCourseCommentCategory(CourseCommentCategoryListViewModel courseCommentCategoryListViewModel)
        {
            CourseCommentCategoryListViewModel courseCommentCategoryList = new CourseCommentCategoryListViewModel();
            try
            {
                courseCommentCategoryList.TenantId = courseCommentCategoryListViewModel.TenantId;
                courseCommentCategoryList.SchoolId = courseCommentCategoryListViewModel.SchoolId;
                courseCommentCategoryList._tenantName = courseCommentCategoryListViewModel._tenantName;
                courseCommentCategoryList._token = courseCommentCategoryListViewModel._token;
                courseCommentCategoryList._userName = courseCommentCategoryListViewModel._userName;

                var courseCommentCategoryData = this.context?.CourseCommentCategory.Where(x => x.TenantId == courseCommentCategoryListViewModel.TenantId && x.SchoolId == courseCommentCategoryListViewModel.SchoolId).ToList();

                if (courseCommentCategoryData.Count > 0)
                {
                    courseCommentCategoryList.courseCommentCategories = courseCommentCategoryData;
                    courseCommentCategoryList._failure = false;
                }
                else
                {
                    courseCommentCategoryList._message = NORECORDFOUND;
                    courseCommentCategoryList._failure = true;
                }
            }
            catch (Exception es)
            {
                courseCommentCategoryList.courseCommentCategories = null;
                courseCommentCategoryList._message = es.Message;
                courseCommentCategoryList._failure = true;
            }
            return courseCommentCategoryList;
        }

        public ReportCardViewModel ViewReportCard(ReportCardViewModel reportCardViewModel)
        {
            ReportCardViewModel reportCardView = new ReportCardViewModel();
            try
            {
                var teacherComments =new List<String>();
                var reportCardData = new List<StudentFinalGrade>();
                DateTime? startDate=null;
                DateTime? endDate=null;
                int? absencesInDays = 0;
                if (reportCardViewModel.studentsReportCardViewModelList.Count > 0)
                {
                    var schoolData = this.context?.SchoolMaster.Include(x=>x.SchoolCalendars).FirstOrDefault(x => x.TenantId == reportCardViewModel.TenantId && x.SchoolId == reportCardViewModel.SchoolId);

                    reportCardView.SchoolName = schoolData.SchoolName;
                    reportCardView.StreetAddress1 = schoolData.StreetAddress1;
                    reportCardView.StreetAddress2 = schoolData.StreetAddress2;
                    reportCardView.State = schoolData.State;
                    reportCardView.City = schoolData.City;
                    reportCardView.District = schoolData.District;
                    reportCardView.Zip = schoolData.Zip;

                    foreach (var student in reportCardViewModel.studentsReportCardViewModelList)
                    {
                        var StudentsReportCard = new StudentsReportCardViewModel();

                        var studentData = this.context?.StudentMaster.Include(x => x.StudentEnrollment).FirstOrDefault(x => x.TenantId == reportCardViewModel.TenantId && x.SchoolId == reportCardViewModel.SchoolId && x.StudentId == student.StudentId);

                        StudentsReportCard.StudentId = studentData.StudentId;
                        StudentsReportCard.StudentInternalId = studentData.StudentInternalId;
                        StudentsReportCard.FirstGivenName = studentData.FirstGivenName;
                        StudentsReportCard.MiddleName = studentData.MiddleName;
                        StudentsReportCard.LastFamilyName = studentData.LastFamilyName;
                        StudentsReportCard.AlternateId = studentData.AlternateId;
                        StudentsReportCard.GradeTitle = studentData.StudentEnrollment.Where(x => x.IsActive == true).Select(s => s.GradeLevelTitle).FirstOrDefault();

                        var markingPeriodsData = reportCardViewModel.MarkingPeriods.Split(",");

                        foreach (var markingPeriod in markingPeriodsData)
                        {
                            var ReportCardMarkingPeriod = new ReportCardMarkingPeriodsDetails();

                            int? QtrMarkingPeriodId = null;
                            int? SmstrMarkingPeriodId = null;
                            int? YrMarkingPeriodId = null;

                            if (markingPeriod != null)
                            {
                                var markingPeriodid = markingPeriod.Split("_", StringSplitOptions.RemoveEmptyEntries);

                                if (markingPeriodid.First() == "2")
                                {
                                    QtrMarkingPeriodId = Int32.Parse(markingPeriodid.ElementAt(1));

                                    var qtrData = this.context?.Quarters.FirstOrDefault(x => x.TenantId == reportCardViewModel.TenantId && x.SchoolId == reportCardViewModel.SchoolId && x.MarkingPeriodId == QtrMarkingPeriodId && x.AcademicYear == reportCardViewModel.AcademicYear);

                                    ReportCardMarkingPeriod.MarkingPeriod = qtrData.Title;
                                    startDate = qtrData.StartDate;
                                    endDate = qtrData.EndDate;
                                }

                                if (markingPeriodid.First() == "1")
                                {
                                    SmstrMarkingPeriodId = Int32.Parse(markingPeriodid.ElementAt(1));

                                    var smstrData = this.context?.Semesters.FirstOrDefault(x => x.TenantId == reportCardViewModel.TenantId && x.SchoolId == reportCardViewModel.SchoolId && x.MarkingPeriodId == SmstrMarkingPeriodId && x.AcademicYear == reportCardViewModel.AcademicYear);

                                    ReportCardMarkingPeriod.MarkingPeriod = smstrData.Title;
                                    startDate = smstrData.StartDate;
                                    endDate = smstrData.EndDate;
                                }

                                if (markingPeriodid.First() == "0")
                                {
                                    YrMarkingPeriodId = Int32.Parse(markingPeriodid.ElementAt(1));

                                    var yrData = this.context?.SchoolYears.FirstOrDefault(x => x.TenantId == reportCardViewModel.TenantId && x.SchoolId == reportCardViewModel.SchoolId && x.MarkingPeriodId == YrMarkingPeriodId && x.AcademicYear == reportCardViewModel.AcademicYear);

                                    ReportCardMarkingPeriod.MarkingPeriod = yrData.Title;
                                    startDate = yrData.StartDate;
                                    endDate = yrData.EndDate;
                                }

                                var studentAttendanceData = this.context?.StudentAttendance.Include(x => x.AttendanceCodeNavigation).Where(x => x.TenantId == reportCardViewModel.TenantId && x.SchoolId == reportCardViewModel.SchoolId && x.StudentId == student.StudentId && x.AttendanceDate >= startDate && x.AttendanceDate <= endDate).ToList();

                                if (studentAttendanceData.Count > 0)
                                {
                                    int? absentData = studentAttendanceData.Where(x => x.AttendanceCodeNavigation.StateCode.ToLower() == "absent").Count();
                                    int? excusedabsentData = studentAttendanceData.Where(x => x.AttendanceCodeNavigation.StateCode.ToLower() == "excusedabsent").Count();
                                    var prasentData = studentAttendanceData.Where(x => x.AttendanceCodeNavigation.StateCode.ToLower() == "prasent");

                                    ReportCardMarkingPeriod.Absences = reportCardViewModel.DailyAbsencesThisMarkingPeriod == true ? absentData : null;
                                    ReportCardMarkingPeriod.ExcusedAbsences = excusedabsentData;
                                    absencesInDays += absentData + ReportCardMarkingPeriod.ExcusedAbsences;
                                }

                                reportCardData = this.context?.StudentFinalGrade.Include(x => x.StudentFinalGradeComments).Include(x=>x.StudentFinalGradeStandard).Where(x => x.TenantId == reportCardViewModel.TenantId && x.SchoolId == reportCardViewModel.SchoolId && x.StudentId == student.StudentId && x.AcademicYear == reportCardViewModel.AcademicYear && ((x.YrMarkingPeriodId != null && x.YrMarkingPeriodId == YrMarkingPeriodId) || (x.SmstrMarkingPeriodId != null && x.SmstrMarkingPeriodId == SmstrMarkingPeriodId) || (x.QtrMarkingPeriodId != null && x.QtrMarkingPeriodId == QtrMarkingPeriodId))).ToList();

                                decimal? gPaValue = 0.0m;
                                decimal? CreditEarned = 0.0m;
                                decimal? CreditHours = 0.0m;
     
                                if (reportCardData.Count > 0)
                                {
                                    foreach (var reportCard in reportCardData)
                                    {
                                        var ReportCard = new ReportCardDetails();

                                        var CourseSectionData = this.context?.CourseSection.Include(x => x.StaffCoursesectionSchedule).ThenInclude(x => x.StaffMaster).FirstOrDefault(x => x.TenantId == reportCardViewModel.TenantId && x.SchoolId == reportCardViewModel.SchoolId && x.CourseSectionId == reportCard.CourseSectionId && x.CourseId == reportCard.CourseId);

                                        ReportCard.CourseSectionName = CourseSectionData.CourseSectionName;

                                        ReportCard.StaffName = reportCardViewModel.TeacherName == true ? CourseSectionData.StaffCoursesectionSchedule.FirstOrDefault().StaffMaster.FirstGivenName + CourseSectionData.StaffCoursesectionSchedule.FirstOrDefault().StaffMaster.MiddleName + CourseSectionData.StaffCoursesectionSchedule.FirstOrDefault().StaffMaster.LastFamilyName : null;
                                        ReportCard.PercentMarks = reportCardViewModel.Parcentage == true ? reportCard.PercentMarks : null;
                                        ReportCard.GradeObtained = reportCard.GradeObtained;

                                        var gradeData = this.context?.Grade.FirstOrDefault(x => x.TenantId == reportCard.TenantId && x.SchoolId == reportCard.SchoolId && x.GradeId == reportCard.StudentFinalGradeStandard.FirstOrDefault().GradeObtained);

                                        if (gradeData != null)
                                        {
                                            if (gradeData.Title == "F" || gradeData.Title == "Inc")
                                            {
                                                gPaValue = 0;
                                            }
                                            else
                                            {
                                                CreditHours = CourseSectionData.CreditHours;
                                                CreditEarned = CourseSectionData.CreditHours;
                                                gPaValue = CourseSectionData.IsWeightedCourse != true ? gradeData.UnweightedGpValue * (CreditHours / CreditEarned) : gradeData.WeightedGpValue * (CreditHours / CreditEarned);
                                            }
                                        }

                                        ReportCard.GPA = gPaValue; //(WeightedGpValue or UnweightedGpValue) of Grade * (CreditEarned/CreditHours)

                                        var comments = reportCard.StudentFinalGradeComments.Select(x => x.CourseCommentId).ToList();
                                        ReportCard.Comments= string.Join(",", comments.Select(x => x.ToString()).ToArray());
                                        if(reportCard.TeacherComment!=null)
                                        {
                                            teacherComments.Add(reportCard.TeacherComment);
                                            int? index = teacherComments.ToList().Count();
                                            ReportCard.TeacherComments = index.ToString();
                                        }

                                        ReportCardMarkingPeriod.reportCardDetails.Add(ReportCard);
                                    }
                                }
                                StudentsReportCard.reportCardMarkingPeriodsDetails.Add(ReportCardMarkingPeriod);
                            }                                                        
                        }
                        decimal attendencePercent = 0.0m;
                        int prasentDay = 0;

                        var calenderData = schoolData.SchoolCalendars.FirstOrDefault(x => x.DefaultCalender == true && x.AcademicYear == reportCardViewModel.AcademicYear);

                        if (calenderData != null)
                        {
                            DateTime schoolYearStartDate = (DateTime)calenderData.StartDate;
                            DateTime schoolYearEndDate = (DateTime)calenderData.EndDate;
                            var daysValue = "0123456";
                            var weekdays = calenderData.Days;
                            var WeekOffDays = Regex.Split(daysValue, weekdays);
                            var WeekOfflist = new List<string>();
                            foreach (var WeekOffDay in WeekOffDays)
                            {
                                Days days = new Days();
                                var Day = Enum.GetName(days.GetType(), Convert.ToInt32(WeekOffDay));
                                WeekOfflist.Add(Day);
                            }

                            int workDays = 0;
                            while (schoolYearStartDate != schoolYearEndDate)
                            {     
                                if (!WeekOfflist.Contains(schoolYearStartDate.DayOfWeek.ToString()))
                                {
                                    workDays++;
                                }
                                schoolYearStartDate = schoolYearStartDate.AddDays(1);
                            }

                            var studentAttendanceData = this.context?.StudentAttendance.Include(x => x.AttendanceCodeNavigation).Where(x => x.TenantId == reportCardViewModel.TenantId && x.SchoolId == reportCardViewModel.SchoolId && x.StudentId == student.StudentId && x.AttendanceDate >= calenderData.StartDate && x.AttendanceDate <= calenderData.EndDate).ToList();

                            if (studentAttendanceData.Count > 0)
                            {
                                prasentDay = studentAttendanceData.Where(x => x.AttendanceCodeNavigation.StateCode.ToLower() == "prasent").Count();
                            }

                            attendencePercent = (prasentDay / workDays) * 100;
                        }

                        StudentsReportCard.YearToDateAttendencePercent = attendencePercent;
                        StudentsReportCard.YearToDateAbsencesInDays = reportCardViewModel.YearToDateDailyAbsences == true ? absencesInDays : null;
                        reportCardView.studentsReportCardViewModelList.Add(StudentsReportCard);
                    }
                    reportCardView.teacherCommentList = teacherComments;
                    var courseCommentCategoryData = this.context?.CourseCommentCategory.Where(x => x.TenantId == reportCardViewModel.TenantId && x.SchoolId == reportCardViewModel.SchoolId).ToList();
                    if (courseCommentCategoryData.Count > 0)
                    {
                        courseCommentCategoryData.ToList().ForEach(x => x.StudentFinalGradeComments = null);
                        reportCardView.courseCommentCategories = courseCommentCategoryData;
                    }
                }
                else
                {
                    reportCardView._failure = true;
                    reportCardView._message = "Select Student Please";
                }
            }
            catch (Exception es)
            {
                reportCardView._failure = true;
                reportCardView._message = es.Message;
            }
            return reportCardView;
        }

    }
}
