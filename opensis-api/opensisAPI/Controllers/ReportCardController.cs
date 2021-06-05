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

        [HttpPost("addReportCard")]
        public ActionResult<ReportCardViewModel> AddReportCard(ReportCardViewModel reportCardViewModel)
        {
            ReportCardViewModel reportCardView = new ReportCardViewModel();
            try
            {
                reportCardView = _reportCardService.AddReportCard(reportCardViewModel);
            }
            catch (Exception es)
            {
                reportCardView._message = es.Message;
                reportCardView._failure = true;
            }
            return reportCardView;
        }

        [HttpPost("generateReportCard")]
        public async Task<ReportCardViewModel> GenerateReportCard(ReportCardViewModel reportCardViewModel)
        {
            ReportCardViewModel reportCardView = new ReportCardViewModel();
            try
            {
                reportCardView = await _reportCardService.GenerateReportCard(reportCardViewModel);
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
