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
using System.IO;
using System.Threading.Tasks;
using JSReport;

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

        /// <summary>
        /// Add Report Card
        /// </summary>
        /// <param name="reportCardViewModel"></param>
        /// <returns></returns>
        public ReportCardViewModel AddReportCard(ReportCardViewModel reportCardViewModel)
        {
            try
            {
                List<string> teacherComments = new List<string>();

                int i = 0;
                long? ide = 1;
                int teacherCommentsNo = 1;
                if (reportCardViewModel.studentsReportCardViewModelList.Count > 0)
                {
                    foreach (var student in reportCardViewModel.studentsReportCardViewModelList)
                    {
                        List<StudentReportCardMaster> studentReportCardMasterList = new List<StudentReportCardMaster>();
                        List<StudentReportCardDetail> studentReportCardDetailList = new List<StudentReportCardDetail>();

                        var existingStudentReportCardData = this.context?.StudentReportCardMaster.Where(x => x.SchoolId == reportCardViewModel.SchoolId && x.TenantId == reportCardViewModel.TenantId && x.StudentId == student.StudentId && x.SchoolYear == reportCardViewModel.AcademicYear.ToString()).ToList();

                        if (existingStudentReportCardData != null)
                        {
                            var existingStudentReportCardDetailsData = this.context?.StudentReportCardDetail.Where(x => x.SchoolId == reportCardViewModel.SchoolId && x.TenantId == reportCardViewModel.TenantId && x.StudentId == student.StudentId && x.SchoolYear == reportCardViewModel.AcademicYear.ToString()).ToList();
                            if (existingStudentReportCardDetailsData.Count > 0)
                            {
                                this.context?.StudentReportCardDetail.RemoveRange(existingStudentReportCardDetailsData);
                            }
                            this.context?.StudentReportCardMaster.RemoveRange(existingStudentReportCardData);
                            this.context.SaveChanges();
                        }
                        if (i == 0)
                        {
                            var idData = this.context?.StudentReportCardDetail.Where(x => x.TenantId == reportCardViewModel.TenantId && x.SchoolId == reportCardViewModel.SchoolId).OrderByDescending(x => x.Id).FirstOrDefault();

                            if (idData != null)
                            {
                                ide = idData.Id + 1;
                            }
                        }

                        int absencesInDays = 0;

                        var studentData = this.context?.StudentMaster.Include(x => x.StudentEnrollment).FirstOrDefault(x => x.TenantId == reportCardViewModel.TenantId && x.SchoolId == reportCardViewModel.SchoolId && x.StudentId == student.StudentId);

                        var GradeLevelTitle = studentData.StudentEnrollment.Where(x => x.IsActive == true).Select(s => s.GradeLevelTitle).FirstOrDefault();

                        var markingPeriodsData = reportCardViewModel.MarkingPeriods.Split(",");
                        DateTime? startDate = null;
                        DateTime? endDate = null;
                        string MarkingPeriodTitle = null;

                        foreach (var markingPeriod in markingPeriodsData)
                        {
                            int Absences = 0;
                            int ExcusedAbsences = 0;
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

                                    MarkingPeriodTitle = qtrData.Title;
                                    startDate = qtrData.StartDate;
                                    endDate = qtrData.EndDate;
                                }

                                if (markingPeriodid.First() == "1")
                                {
                                    SmstrMarkingPeriodId = Int32.Parse(markingPeriodid.ElementAt(1));

                                    var smstrData = this.context?.Semesters.FirstOrDefault(x => x.TenantId == reportCardViewModel.TenantId && x.SchoolId == reportCardViewModel.SchoolId && x.MarkingPeriodId == SmstrMarkingPeriodId && x.AcademicYear == reportCardViewModel.AcademicYear);

                                    MarkingPeriodTitle = smstrData.Title;
                                    startDate = smstrData.StartDate;
                                    endDate = smstrData.EndDate;
                                }

                                if (markingPeriodid.First() == "0")
                                {
                                    YrMarkingPeriodId = Int32.Parse(markingPeriodid.ElementAt(1));

                                    var yrData = this.context?.SchoolYears.FirstOrDefault(x => x.TenantId == reportCardViewModel.TenantId && x.SchoolId == reportCardViewModel.SchoolId && x.MarkingPeriodId == YrMarkingPeriodId && x.AcademicYear == reportCardViewModel.AcademicYear);

                                    MarkingPeriodTitle = yrData.Title;
                                    startDate = yrData.StartDate;
                                    endDate = yrData.EndDate;
                                }

                                var studentAttendanceData = this.context?.StudentAttendance.Include(x => x.AttendanceCodeNavigation).Where(x => x.TenantId == reportCardViewModel.TenantId && x.SchoolId == reportCardViewModel.SchoolId && x.StudentId == student.StudentId && x.AttendanceDate >= startDate && x.AttendanceDate <= endDate).ToList();

                                if (studentAttendanceData.Count > 0)
                                {
                                    Absences = studentAttendanceData.Where(x => x.AttendanceCodeNavigation.StateCode.ToLower() == "absent").Count();
                                    ExcusedAbsences = studentAttendanceData.Where(x => x.AttendanceCodeNavigation.StateCode.ToLower() == "excusedabsent").Count();
                                    var prasentData = studentAttendanceData.Where(x => x.AttendanceCodeNavigation.StateCode.ToLower() == "prasent");

                                    absencesInDays += Absences + ExcusedAbsences;
                                }

                                var reportCardData = this.context?.StudentFinalGrade.Include(x => x.StudentFinalGradeComments).Include(x => x.StudentFinalGradeStandard).Where(x => x.TenantId == reportCardViewModel.TenantId && x.SchoolId == reportCardViewModel.SchoolId && x.StudentId == student.StudentId && x.AcademicYear == reportCardViewModel.AcademicYear && ((x.YrMarkingPeriodId != null && x.YrMarkingPeriodId == YrMarkingPeriodId) || (x.SmstrMarkingPeriodId != null && x.SmstrMarkingPeriodId == SmstrMarkingPeriodId) || (x.QtrMarkingPeriodId != null && x.QtrMarkingPeriodId == QtrMarkingPeriodId))).ToList();

                                decimal? gPaValue = 0.0m;
                                decimal? CreditEarned = 0.0m;
                                decimal? CreditHours = 0.0m;


                                if (reportCardData.Count > 0)
                                {
                                    foreach (var reportCard in reportCardData)
                                    {
                                        var CourseSectionData = this.context?.CourseSection.Include(x => x.StaffCoursesectionSchedule).ThenInclude(x => x.StaffMaster).FirstOrDefault(x => x.TenantId == reportCardViewModel.TenantId && x.SchoolId == reportCardViewModel.SchoolId && x.CourseSectionId == reportCard.CourseSectionId && x.CourseId == reportCard.CourseId);

                                        var gradeData = this.context?.Grade.FirstOrDefault(x => x.TenantId == reportCard.TenantId && x.SchoolId == reportCard.SchoolId && x.Title.ToLower() == reportCard.GradeObtained.ToLower() && x.GradeScaleId==reportCard.GradeScaleId);

                                        if (gradeData != null)
                                        {
                                            CreditHours = CourseSectionData.CreditHours;
                                            CreditEarned = CourseSectionData.CreditHours;
                                            gPaValue = CourseSectionData.IsWeightedCourse != true ? gradeData.UnweightedGpValue * (CreditHours / CreditEarned) : gradeData.WeightedGpValue * (CreditHours / CreditEarned);

                                        }

                                        var comments = reportCard.StudentFinalGradeComments.Select(x => x.CourseCommentId).ToList();
                                        var Comments = string.Join(",", comments.Select(x => x.ToString()).ToArray());

                                        var studentReportCardDetail = new StudentReportCardDetail()
                                        {
                                            Id = (long)ide,
                                            TenantId = reportCardViewModel.TenantId,
                                            SchoolId = reportCardViewModel.SchoolId,
                                            StudentId = student.StudentId,
                                            SchoolYear = reportCardViewModel.AcademicYear.ToString(),
                                            GradeTitle = GradeLevelTitle,
                                            MarkingPeriodTitle = MarkingPeriodTitle,
                                            CourseName = CourseSectionData.CourseSectionName,
                                            Teacher = reportCardViewModel.TeacherName == true ? CourseSectionData.StaffCoursesectionSchedule.Count > 0 ? CourseSectionData.StaffCoursesectionSchedule.FirstOrDefault().StaffMaster.FirstGivenName + " " + CourseSectionData.StaffCoursesectionSchedule.FirstOrDefault().StaffMaster.MiddleName + " " + CourseSectionData.StaffCoursesectionSchedule.FirstOrDefault().StaffMaster.LastFamilyName : null : null,
                                            Grade = reportCardViewModel.Parcentage != true ? reportCard.GradeObtained : reportCard.GradeObtained + "(" + reportCard.PercentMarks + ")",
                                            Gpa = reportCardViewModel.GPA == true ? gPaValue : null,
                                            Comments = Comments,
                                            TeacherComments = reportCardViewModel.TeacherComments == true ? reportCard.TeacherComment != null ? (teacherCommentsNo++).ToString() : null:null,
                                            OverallTeacherComments = reportCard.TeacherComment,

                                            CreatedBy = reportCardViewModel.CreatedBy,
                                            CreatedOn = DateTime.UtcNow

                                        };
                                        studentReportCardDetailList.Add(studentReportCardDetail);
                                        ide++;

                                    }
                                    this.context?.StudentReportCardDetail.AddRange(studentReportCardDetailList);
                                }

                                double attendencePercent = 0;
                                int prasentDay = 0;

                                var calenderData = this.context?.SchoolCalendars.FirstOrDefault(x => x.TenantId == reportCardViewModel.TenantId && x.SchoolId == reportCardViewModel.SchoolId && x.DefaultCalender == true && x.AcademicYear == reportCardViewModel.AcademicYear);

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

                                    var studentPrasentAttendanceData = this.context?.StudentAttendance.Include(x => x.AttendanceCodeNavigation).Where(x => x.TenantId == reportCardViewModel.TenantId && x.SchoolId == reportCardViewModel.SchoolId && x.StudentId == student.StudentId && x.AttendanceDate >= calenderData.StartDate && x.AttendanceDate <= calenderData.EndDate).ToList();

                                    if (studentPrasentAttendanceData.Count > 0)
                                    {
                                        prasentDay = studentPrasentAttendanceData.Where(x => x.AttendanceCodeNavigation.StateCode.ToLower() == "prasent").Count();
                                    }
                                    var df = workDays - absencesInDays;
                                    attendencePercent = df * 100 / workDays;
                                    //attendencePercent = ((df / workDays) * 100);
                                }

                                var studentReportCardMaster = new StudentReportCardMaster
                                {
                                    TenantId = reportCardViewModel.TenantId,
                                    SchoolId = reportCardViewModel.SchoolId,
                                    StudentId = student.StudentId,
                                    SchoolYear = reportCardViewModel.AcademicYear.ToString(),
                                    GradeTitle = GradeLevelTitle,
                                    StudentInternalId = studentData.StudentInternalId,
                                    YodAbsence = absencesInDays,
                                    YodAttendance = Math.Round(attendencePercent, 2).ToString() + "%",
                                    ReportGenerationDate = DateTime.UtcNow,
                                    Absences = Absences,
                                    ExcusedAbsences = ExcusedAbsences,
                                    MarkingPeriodTitle = MarkingPeriodTitle,
                                    CreatedBy = reportCardViewModel.CreatedBy,
                                    CreatedOn = DateTime.UtcNow
                                };
                                studentReportCardMasterList.Add(studentReportCardMaster);
                            }
                        }
                        this.context?.StudentReportCardMaster.AddRange(studentReportCardMasterList);
                        i++;
                    }
                    this.context?.SaveChanges();
                    reportCardViewModel._message = "Added Successfully";
                }
                else
                {
                    reportCardViewModel._failure = true;
                    reportCardViewModel._message = "Select Student Please";
                }
            }
            catch (Exception es)
            {
                reportCardViewModel._failure = true;
                reportCardViewModel._message = es.Message;
            }
            return reportCardViewModel;
        }


        /// <summary>
        /// Generate Report Card
        /// </summary>
        /// <param name="reportCardViewModel"></param>
        /// <returns></returns>
        public async Task<ReportCardViewModel> GenerateReportCard(ReportCardViewModel reportCardViewModel)
        {
            ReportCardViewModel reportCardView = new ReportCardViewModel();
            try
            {
                reportCardView.TenantId = reportCardViewModel.TenantId;
                reportCardView.SchoolId = reportCardViewModel.SchoolId;
                reportCardView._tenantName = reportCardViewModel._tenantName;
                reportCardView._userName = reportCardViewModel._userName;
                string base64 = null;
                object data = new object();

                List<object> reportCardList = new List<object>();
                List<object> teacherCommentList = new List<object>();
             
                foreach (var student in reportCardViewModel.studentsReportCardViewModelList)
                {
                    var studentNameData = this.context?.StudentMaster.FirstOrDefault(x => x.SchoolId == reportCardViewModel.SchoolId && x.TenantId == reportCardViewModel.TenantId && x.StudentId == student.StudentId);

                    var schoolData = this.context?.SchoolMaster.FirstOrDefault(x => x.SchoolId == reportCardViewModel.SchoolId && x.TenantId == reportCardViewModel.TenantId);

                    var studentReportCardData = this.context?.StudentReportCardMaster.Include(x => x.StudentReportCardDetail).Where(x => x.SchoolId == reportCardViewModel.SchoolId && x.TenantId == reportCardViewModel.TenantId && x.SchoolYear == reportCardViewModel.AcademicYear.ToString() && x.StudentId == student.StudentId).ToList();

                    if (studentNameData != null && schoolData != null && studentReportCardData != null)
                    {
                        List<object> reportDetailsList = new List<object>();
                        foreach (var studentReportCard in studentReportCardData)
                        {                            
                            var studentReportCardDetailsData = studentReportCard.StudentReportCardDetail.Where(x => x.MarkingPeriodTitle == studentReportCard.MarkingPeriodTitle).ToList();

                            var teacherCommentsData = studentReportCardDetailsData.Where(x => x.OverallTeacherComments != null && x.TeacherComments != null).Select(x => new { x.TeacherComments, x.OverallTeacherComments }).ToList();
                            
                            teacherCommentList.AddRange(teacherCommentsData);
                            object markingPeriodWiseData = new
                            {
                                studentReportCard.MarkingPeriodTitle,
                                studentReportCard.ExcusedAbsences,
                                studentReportCard.Absences,        
                                Details = studentReportCardDetailsData,
                               
                            };
                            reportDetailsList.Add(markingPeriodWiseData);
                        }

                        object reportCard = new
                        {
                            SchoolData = schoolData,
                            studentInternalId = studentReportCardData.FirstOrDefault().StudentInternalId,
                            gradeTitle = studentReportCardData.FirstOrDefault().GradeTitle,
                            yodAttendance = studentReportCardData.FirstOrDefault().YodAttendance,
                            yodAbsence = studentReportCardData.FirstOrDefault().YodAbsence,
                            ReportdetailsData = reportDetailsList,
                            StudentName = studentNameData,

                        };
                        reportCardList.Add(reportCard);
                    }
                }

                if (reportCardList != null)
                {
                    var courseCommentCategoryData = this.context?.CourseCommentCategory.Where(x => x.TenantId == reportCardViewModel.TenantId && x.SchoolId == reportCardViewModel.SchoolId).ToList();

                    data = new
                    {
                        TotalData = reportCardList,
                        CourseCommentCategoryData = courseCommentCategoryData,
                        TeacherCommentList = reportCardViewModel.TeacherComments == true ? teacherCommentList : null
                    };
                }
     
                GenerateReportCard _report = new GenerateReportCard();
                var message = await _report.Generate(data);

                if(message == "success")
                {
                    using (var fileStream = new FileStream(@"ReportCard\\StudentReport.pdf", FileMode.Open))
                    {

                        using (var memoryStream = new MemoryStream())
                        {
                            fileStream.CopyTo(memoryStream);
                            byte[] bytes = memoryStream.ToArray();
                            base64 = Convert.ToBase64String(bytes);
                            fileStream.Close();
                        }
                    }
                    reportCardView.ReportCardPdf = base64;
                }
                else
                {
                    reportCardView._message = "Problem occur!!! Prlease Try Again";
                    reportCardView._failure = true;
                }
   
            }
            catch (Exception es)
            {
                reportCardView._message = es.Message;
                reportCardView._failure = true;

            }
            return reportCardView;
        }
    }
}
