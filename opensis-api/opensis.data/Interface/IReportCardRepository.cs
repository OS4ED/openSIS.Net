using opensis.data.ViewModels.ReportCard;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.Interface
{
    public interface IReportCardRepository
    {
        //public ReportCardCommentsAddViewModel AddReportCardComments(ReportCardCommentsAddViewModel reportCardCommentsAddViewModel);
        //public ReportCardCommentsAddViewModel UpdateReportCardComments(ReportCardCommentsAddViewModel reportCardCommentsAddViewModel);
        //public ReportCardCommentsAddViewModel DeleteReportCardComments(ReportCardCommentsAddViewModel reportCardCommentsAddViewModel);
        public CourseCommentCategoryAddViewModel AddCourseCommentCategory(CourseCommentCategoryAddViewModel courseCommentCategoryAddViewModel);
        public CourseCommentCategoryDeleteViewModel DeleteCourseCommentCategory(CourseCommentCategoryDeleteViewModel courseCommentCategoryDeleteViewModel);   
        public CourseCommentCategorySortOrderViewModel UpdateSortOrderForCourseCommentCategory(CourseCommentCategorySortOrderViewModel courseCommentCategorySortOrderViewModel);
        public CourseCommentCategoryListViewModel GetAllCourseCommentCategory(CourseCommentCategoryListViewModel courseCommentCategoryListViewModel);
        public ReportCardViewModel ViewReportCard(ReportCardViewModel reportCardViewModel);
    }
}
