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

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using opensis.core.InputFinalGrade.Interfaces;
using opensis.data.ViewModels.InputFinalGrade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace opensisAPI.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("{tenant}/InputFinalGrade")]
    [ApiController]
    public class InputFinalGradeController : ControllerBase
    {
        private IInputFinalGradeService _inputFinalGradeService;
        public InputFinalGradeController(IInputFinalGradeService inputFinalGradeService)
        {
            _inputFinalGradeService = inputFinalGradeService;
        }

        /// <summary>
        /// Add Update Student Final Grade
        /// </summary>
        /// <param name="studentFinalGradeListModel"></param>
        /// <returns></returns>
        [HttpPost("addUpdateStudentFinalGrade")]
        public ActionResult<StudentFinalGradeListModel> AddUpdateStudentFinalGrade(StudentFinalGradeListModel studentFinalGradeListModel)
        {
            StudentFinalGradeListModel studentFinalGradeAdd = new StudentFinalGradeListModel();
            try
            {
                studentFinalGradeAdd = _inputFinalGradeService.AddUpdateStudentFinalGrade(studentFinalGradeListModel);
            }
            catch (Exception ex)
            {

                studentFinalGradeAdd._message = ex.Message;
                studentFinalGradeAdd._failure = true;
            }
            return studentFinalGradeAdd;
        }

        /// <summary>
        /// Get All Student Final Grade List
        /// </summary>
        /// <param name="studentFinalGradeListModel"></param>
        /// <returns></returns>
        [HttpPost("getAllStudentFinalGradeList")]
        public ActionResult<StudentFinalGradeListModel> GetAllStudentFinalGradeList(StudentFinalGradeListModel studentFinalGradeListModel)
        {
            StudentFinalGradeListModel studentFinalGradeList = new StudentFinalGradeListModel();
            try
            {
                studentFinalGradeList = _inputFinalGradeService.GetAllStudentFinalGradeList(studentFinalGradeListModel);
            }
            catch (Exception ex)
            {

                studentFinalGradeList._message = ex.Message;
                studentFinalGradeList._failure = true;
            }
            return studentFinalGradeList;
        }

        //[HttpPost("getReportCardCommentsForInputFinalGrade")]
        //public ActionResult<ReportCardCommentListViewModel> GetReportCardCommentsForInputFinalGrade(ReportCardCommentListViewModel reportCardCommentListViewModel)
        //{
        //    ReportCardCommentListViewModel reportCardCommentList = new ReportCardCommentListViewModel();
        //    try
        //    {
        //        reportCardCommentList = _inputFinalGradeService.GetReportCardCommentsForInputFinalGrade(reportCardCommentListViewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        reportCardCommentList._message = ex.Message;
        //        reportCardCommentList._failure = true;
        //    }
        //    return reportCardCommentList;
        //}
    }
}
