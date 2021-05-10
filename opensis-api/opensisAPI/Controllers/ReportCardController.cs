using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using opensis.core.ReportCard.Interfaces;
//using opensis.data.Models;
using opensis.data.ViewModels.ReportCard;

namespace opensisAPI.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("{tenant}/ReportCard")]
    [ApiController]
    public class ReportCardController : ControllerBase
    {
        private IReportCardService _reportCardService;
        public ReportCardController(IReportCardService reportCardService)
        {
            _reportCardService = reportCardService;
        }

        //[HttpPost("addReportCardComments")]

        //public ActionResult<ReportCardCommentsAddViewModel> AddReportCardComments(ReportCardCommentsAddViewModel reportCardCommentsAddViewModel)
        //{
        //    ReportCardCommentsAddViewModel reportCardCommentsAdd = new ReportCardCommentsAddViewModel();

        //    try
        //    {
        //        reportCardCommentsAdd = _reportCardService.AddReportCardComments(reportCardCommentsAddViewModel);
        //    }
        //    catch(Exception ex)
        //    {
        //        reportCardCommentsAdd._message = ex.Message;
        //        reportCardCommentsAdd._failure = true;
        //    }
        //    return reportCardCommentsAdd;
        //}


        //[HttpPut("updateReportCardComments")]
        //public ActionResult<ReportCardCommentsAddViewModel> UpdateReportCardComments(ReportCardCommentsAddViewModel reportCardCommentsAddViewModel)
        //{
        //    ReportCardCommentsAddViewModel reportCardCommentsUpdate = new ReportCardCommentsAddViewModel();
        //    try
        //    {
        //        reportCardCommentsUpdate = _reportCardService.UpdateReportCardComments(reportCardCommentsAddViewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        reportCardCommentsUpdate._message = ex.Message;
        //        reportCardCommentsUpdate._failure = true;
        //    }
        //    return reportCardCommentsUpdate;
        //}


        //[HttpPost("deleteReportCardComments")]
        //public ActionResult<ReportCardCommentsAddViewModel> DeleteReportCardComments(ReportCardCommentsAddViewModel reportCardCommentsAddViewModel)
        //{
        //    ReportCardCommentsAddViewModel reportCardCommentsDelete = new ReportCardCommentsAddViewModel();
        //    try
        //    {
        //        reportCardCommentsDelete = _reportCardService.DeleteReportCardComments(reportCardCommentsAddViewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        reportCardCommentsDelete._message = ex.Message;
        //        reportCardCommentsDelete._failure = true;
        //    }
        //    return reportCardCommentsDelete;
        //}


        [HttpPost("addCourseCommentCategory")]
        public ActionResult<CourseCommentCategoryAddViewModel> AddCourseCommentCategory(CourseCommentCategoryAddViewModel courseCommentCategoryAddViewModel)
        {
            CourseCommentCategoryAddViewModel courseCommentAdd = new CourseCommentCategoryAddViewModel();
            try
            {
                courseCommentAdd = _reportCardService.SaveCourseCommentCategory(courseCommentCategoryAddViewModel);
            }
            catch (Exception ex)
            {

                courseCommentAdd._message = ex.Message;
                courseCommentAdd._failure = true;
            }
            return courseCommentAdd;
        }


        [HttpPost("deleteCourseCommentCategory")]
        public ActionResult<CourseCommentCategoryDeleteViewModel> DeleteCourseCommentCategory(CourseCommentCategoryDeleteViewModel courseCommentCategoryDeleteViewModel)
        {
            CourseCommentCategoryDeleteViewModel courseCommentDelete = new CourseCommentCategoryDeleteViewModel();
            try
            {
                courseCommentDelete = _reportCardService.DeleteCourseCommentCategory(courseCommentCategoryDeleteViewModel);
            }
            catch (Exception ex)
            {

                courseCommentDelete._message = ex.Message;
                courseCommentDelete._failure = true;
            }
            return courseCommentDelete;
        }


        [HttpPost("updateSortOrderForCourseCommentCategory")]
        public ActionResult<CourseCommentCategorySortOrderViewModel> UpdateSortOrderForCourseCommentCategory(CourseCommentCategorySortOrderViewModel courseCommentCategorySortOrderViewModel)
        {
            CourseCommentCategorySortOrderViewModel courseCommentCategorySort = new CourseCommentCategorySortOrderViewModel();
            try
            {
                courseCommentCategorySort = _reportCardService.UpdateSortOrderForCourseCommentCategory(courseCommentCategorySortOrderViewModel);
            }
            catch (Exception ex)
            {
                courseCommentCategorySort._message = ex.Message;
                courseCommentCategorySort._failure = true;
            }
            return courseCommentCategorySort;
        }

        [HttpPost("getAllCourseCommentCategory")]
        public ActionResult<CourseCommentCategoryListViewModel> GetAllCourseCommentCategory(CourseCommentCategoryListViewModel courseCommentCategoryListViewModel)
        {
            CourseCommentCategoryListViewModel courseCommentCategoryList = new CourseCommentCategoryListViewModel();
            try
            {
                courseCommentCategoryList = _reportCardService.GetAllCourseCommentCategory(courseCommentCategoryListViewModel);
            }
            catch (Exception es)
            {
                courseCommentCategoryList._message = es.Message;
                courseCommentCategoryList._failure = true;
            }
            return courseCommentCategoryList;
        }

        [HttpPost("viewReportCard")]
        public ActionResult<ReportCardViewModel> ViewReportCard(ReportCardViewModel reportCardViewModel)
        {
            ReportCardViewModel reportCardView = new ReportCardViewModel();
            try
            {
                reportCardView = _reportCardService.ViewReportCard(reportCardViewModel);
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
