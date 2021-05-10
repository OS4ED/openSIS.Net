using opensis.core.helper;
using opensis.core.ReportCard.Interfaces;
using opensis.data.Interface;
using opensis.data.ViewModels.ReportCard;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.ReportCard.Services
{
    public class ReportCardService : IReportCardService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";

        public IReportCardRepository reportCardRepository;
        public ReportCardService(IReportCardRepository reportCardRepository)
        {
            this.reportCardRepository = reportCardRepository;
        }

        public ReportCardService() { }

        /// <summary>
        /// Add Report Card Comments
        /// </summary>
        /// <param name="reportCardViewModel"></param>
        /// <returns></returns>

        //public ReportCardCommentsAddViewModel AddReportCardComments(ReportCardCommentsAddViewModel reportCardCommentsAddViewModel)
        //{
        //    ReportCardCommentsAddViewModel reportCardCommentsAdd = new ReportCardCommentsAddViewModel();
        //    if(TokenManager.CheckToken(reportCardCommentsAddViewModel._tenantName + reportCardCommentsAddViewModel._userName, reportCardCommentsAddViewModel._token))
        //    {
        //        reportCardCommentsAdd = this.reportCardRepository.AddReportCardComments(reportCardCommentsAddViewModel);
        //    }
        //    else
        //    {
        //        reportCardCommentsAdd._message= TOKENINVALID;
        //        reportCardCommentsAdd._failure = true;
        //    }
            
        //    return reportCardCommentsAdd;
        //}

        /// <summary>
        /// Update Report Card Comments
        /// </summary>
        /// <param name="reportCardViewModel"></param>
        /// <returns></returns>

        //public ReportCardCommentsAddViewModel UpdateReportCardComments(ReportCardCommentsAddViewModel reportCardCommentsAddViewModel)
        //{
        //    ReportCardCommentsAddViewModel reportCardCommentsUpdate = new ReportCardCommentsAddViewModel();
        //    if (TokenManager.CheckToken(reportCardCommentsAddViewModel._tenantName + reportCardCommentsAddViewModel._userName, reportCardCommentsAddViewModel._token))
        //    {
        //        reportCardCommentsUpdate = this.reportCardRepository.UpdateReportCardComments(reportCardCommentsAddViewModel);
        //    }
        //    else
        //    {
        //        reportCardCommentsUpdate._message = TOKENINVALID;
        //        reportCardCommentsUpdate._failure = true;
        //    }
        //    return reportCardCommentsUpdate;
        //}

        /// <summary>
        /// Delete Report Card Comments
        /// </summary>
        /// <param name="reportCardViewModel"></param>
        /// <returns></returns>
        //public ReportCardCommentsAddViewModel DeleteReportCardComments(ReportCardCommentsAddViewModel reportCardCommentsAddViewModel)
        //{
        //    ReportCardCommentsAddViewModel reportCardCommentsDelete = new ReportCardCommentsAddViewModel();
        //    if (TokenManager.CheckToken(reportCardCommentsAddViewModel._tenantName + reportCardCommentsAddViewModel._userName, reportCardCommentsAddViewModel._token))
        //    {
        //        reportCardCommentsDelete = this.reportCardRepository.DeleteReportCardComments(reportCardCommentsAddViewModel);
        //    }
        //    else
        //    {
        //        reportCardCommentsDelete._message = TOKENINVALID;
        //        reportCardCommentsDelete._failure = true;
        //    }

        //    return reportCardCommentsDelete;
        //}

        /// <summary>
        /// Add Course Comment Category
        /// </summary>
        /// <param name="courseCommentCategoryAddViewModel"></param>
        /// <returns></returns>

        public CourseCommentCategoryAddViewModel SaveCourseCommentCategory(CourseCommentCategoryAddViewModel courseCommentCategoryAddViewModel)
        {
            CourseCommentCategoryAddViewModel courseCommentcategoryAdd = new CourseCommentCategoryAddViewModel();
            if (TokenManager.CheckToken(courseCommentCategoryAddViewModel._tenantName + courseCommentCategoryAddViewModel._userName, courseCommentCategoryAddViewModel._token))
            {
                courseCommentcategoryAdd = this.reportCardRepository.AddCourseCommentCategory(courseCommentCategoryAddViewModel);
            }
            else
            {
                courseCommentcategoryAdd._message = TOKENINVALID;
                courseCommentcategoryAdd._failure = true;
            }

            return courseCommentcategoryAdd;
        }

        /// <summary>
        /// Delete Course Comment Category
        /// </summary>
        /// <param name="courseCommentCategoryAddViewModel"></param>
        /// <returns></returns>
        public CourseCommentCategoryDeleteViewModel DeleteCourseCommentCategory(CourseCommentCategoryDeleteViewModel courseCommentCategoryDeleteViewModel)
        {
            CourseCommentCategoryDeleteViewModel courseCommentcategoryDelete = new CourseCommentCategoryDeleteViewModel();

            if (TokenManager.CheckToken(courseCommentCategoryDeleteViewModel._tenantName + courseCommentCategoryDeleteViewModel._userName, courseCommentCategoryDeleteViewModel._token))
            {
                courseCommentcategoryDelete = this.reportCardRepository.DeleteCourseCommentCategory(courseCommentCategoryDeleteViewModel);
            }
            else
            {
                courseCommentcategoryDelete._message = TOKENINVALID;
                courseCommentcategoryDelete._failure = true;
            }

            return courseCommentcategoryDelete;
        }

        /// <summary>
        /// Update SortOrder For CourseCommentCategory
        /// </summary>
        /// <param name="reportCardCommentSortOrderViewModel"></param>
        /// <returns></returns>

        public CourseCommentCategorySortOrderViewModel UpdateSortOrderForCourseCommentCategory(CourseCommentCategorySortOrderViewModel courseCommentCategorySortOrderViewModel)
        {
            CourseCommentCategorySortOrderViewModel courseCommentCategorySort = new CourseCommentCategorySortOrderViewModel();
            if (TokenManager.CheckToken(courseCommentCategorySortOrderViewModel._tenantName + courseCommentCategorySortOrderViewModel._userName, courseCommentCategorySortOrderViewModel._token))
            {
                courseCommentCategorySort = this.reportCardRepository.UpdateSortOrderForCourseCommentCategory(courseCommentCategorySortOrderViewModel);
            }
            else
            {
                courseCommentCategorySort._message = TOKENINVALID;
                courseCommentCategorySort._failure = true;
            }
            return courseCommentCategorySort;
        }

        /// <summary>
        /// Get All CourseCommentCategory With ReportCardComments
        /// </summary>
        /// <param name="courseCommentCategoryListViewModel"></param>
        /// <returns></returns>
        public CourseCommentCategoryListViewModel GetAllCourseCommentCategory(CourseCommentCategoryListViewModel courseCommentCategoryListViewModel)
        {
            CourseCommentCategoryListViewModel courseCommentCategoryList = new CourseCommentCategoryListViewModel();
            if (TokenManager.CheckToken(courseCommentCategoryListViewModel._tenantName + courseCommentCategoryListViewModel._userName, courseCommentCategoryListViewModel._token))
            {
                courseCommentCategoryList = this.reportCardRepository.GetAllCourseCommentCategory(courseCommentCategoryListViewModel);
            }
            else
            {
                courseCommentCategoryList._message = TOKENINVALID;
                courseCommentCategoryList._failure = true;
            }

            return courseCommentCategoryList;
        }

        public ReportCardViewModel ViewReportCard(ReportCardViewModel reportCardViewModel)
        {
            ReportCardViewModel reportCardView = new ReportCardViewModel();
            try
            {
                if (TokenManager.CheckToken(reportCardViewModel._tenantName + reportCardViewModel._userName, reportCardViewModel._token))
                {
                    reportCardView = this.reportCardRepository.ViewReportCard(reportCardViewModel);
                }
                else
                {
                    reportCardView._failure = true;
                    reportCardView._message = TOKENINVALID;
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
