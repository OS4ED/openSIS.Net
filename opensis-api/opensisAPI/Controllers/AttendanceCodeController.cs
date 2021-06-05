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
using opensis.core.AttendanceCode.Interfaces;
using opensis.data.ViewModels.AttendanceCodes;

namespace opensisAPI.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("{tenant}/AttendanceCode")]
    [ApiController]
    public class AttendanceCodeController : ControllerBase
    {
        private IAttendanceCodeRegisterService _attendanceCodeRegisterService;
        public AttendanceCodeController(IAttendanceCodeRegisterService attendanceCodeRegisterService)
        {
            _attendanceCodeRegisterService = attendanceCodeRegisterService;
        }
        [HttpPost("addAttendanceCode")]
        public ActionResult<AttendanceCodeAddViewModel> AddAttendanceCode(AttendanceCodeAddViewModel attendanceCodeAddViewModel)
        {
            AttendanceCodeAddViewModel attendanceCodeAdd = new AttendanceCodeAddViewModel();
            try
            {
                attendanceCodeAdd = _attendanceCodeRegisterService.SaveAttendanceCode(attendanceCodeAddViewModel);
            }
            catch (Exception es)
            {
                attendanceCodeAdd._failure = true;
                attendanceCodeAdd._message = es.Message;
            }
            return attendanceCodeAdd;
        }
        [HttpPost("viewAttendanceCode")]

        public ActionResult<AttendanceCodeAddViewModel> ViewAttendanceCode(AttendanceCodeAddViewModel attendanceCodeAddViewModel)
        {
            AttendanceCodeAddViewModel attendanceCodeView = new AttendanceCodeAddViewModel();
            try
            {
                attendanceCodeView = _attendanceCodeRegisterService.ViewAttendanceCode(attendanceCodeAddViewModel);
            }
            catch (Exception es)
            {
                attendanceCodeView._failure = true;
                attendanceCodeView._message = es.Message;
            }
            return attendanceCodeView;
        }
        [HttpPut("updateAttendanceCode")]

        public ActionResult<AttendanceCodeAddViewModel> UpdateAttendanceCode(AttendanceCodeAddViewModel attendanceCodeAddViewModel)
        {
            AttendanceCodeAddViewModel attendanceCodeUpdate = new AttendanceCodeAddViewModel();
            try
            {
                attendanceCodeUpdate = _attendanceCodeRegisterService.UpdateAttendanceCode(attendanceCodeAddViewModel);
            }
            catch (Exception es)
            {
                attendanceCodeUpdate._failure = true;
                attendanceCodeUpdate._message = es.Message;
            }
            return attendanceCodeUpdate;
        }
        [HttpPost("getAllAttendanceCode")]

        public ActionResult<AttendanceCodeListViewModel> GetAllAttendanceCode(AttendanceCodeListViewModel attendanceCodeListViewModel)
        {
            AttendanceCodeListViewModel attendanceCodeList = new AttendanceCodeListViewModel();
            try
            {
                if (attendanceCodeListViewModel.SchoolId > 0)
                {
                    attendanceCodeList = _attendanceCodeRegisterService.GetAllAttendanceCode(attendanceCodeListViewModel);
                }
                else
                {
                    attendanceCodeList._token = attendanceCodeListViewModel._token;
                    attendanceCodeList._tenantName = attendanceCodeListViewModel._tenantName;
                    attendanceCodeList._failure = true;
                    attendanceCodeList._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                attendanceCodeList._message = es.Message;
                attendanceCodeList._failure = true;
            }
            return attendanceCodeList;
        }
        [HttpPost("deleteAttendanceCode")]

        public ActionResult<AttendanceCodeAddViewModel> DeleteAttendanceCode(AttendanceCodeAddViewModel attendanceCodeAddViewModel)
        {
            AttendanceCodeAddViewModel attendanceCodelDelete = new AttendanceCodeAddViewModel();
            try
            {
                attendanceCodelDelete = _attendanceCodeRegisterService.DeleteAttendanceCode(attendanceCodeAddViewModel);
            }
            catch (Exception es)
            {
                attendanceCodelDelete._failure = true;
                attendanceCodelDelete._message = es.Message;
            }
            return attendanceCodelDelete;
        }

        [HttpPost("addAttendanceCodeCategories")]
        public ActionResult<AttendanceCodeCategoriesAddViewModel> AddAttendanceCodeCategories(AttendanceCodeCategoriesAddViewModel attendanceCodeCategoriesAddViewModel)
        {
            AttendanceCodeCategoriesAddViewModel attendanceCodeCategoriesAdd = new AttendanceCodeCategoriesAddViewModel();
            try
            {
                attendanceCodeCategoriesAdd = _attendanceCodeRegisterService.SaveAttendanceCodeCategories(attendanceCodeCategoriesAddViewModel);
            }
            catch (Exception es)
            {
                attendanceCodeCategoriesAdd._failure = true;
                attendanceCodeCategoriesAdd._message = es.Message;
            }
            return attendanceCodeCategoriesAdd;
        }
        [HttpPost("viewAttendanceCodeCategories")]

        public ActionResult<AttendanceCodeCategoriesAddViewModel> ViewAttendanceCodeCategories(AttendanceCodeCategoriesAddViewModel attendanceCodeCategoriesAddViewModel)
        {
            AttendanceCodeCategoriesAddViewModel attendanceCodeCategoriesView = new AttendanceCodeCategoriesAddViewModel();
            try
            {
                attendanceCodeCategoriesView = _attendanceCodeRegisterService.ViewAttendanceCodeCategories(attendanceCodeCategoriesAddViewModel);
            }
            catch (Exception es)
            {
                attendanceCodeCategoriesView._failure = true;
                attendanceCodeCategoriesView._message = es.Message;
            }
            return attendanceCodeCategoriesView;
        }
        [HttpPut("updateAttendanceCodeCategories")]

        public ActionResult<AttendanceCodeCategoriesAddViewModel> UpdateAttendanceCodeCategories(AttendanceCodeCategoriesAddViewModel attendanceCodeCategoriesAddViewModel)
        {
            AttendanceCodeCategoriesAddViewModel attendanceCodeCategoriesUpdate = new AttendanceCodeCategoriesAddViewModel();
            try
            {
                attendanceCodeCategoriesUpdate = _attendanceCodeRegisterService.UpdateAttendanceCodeCategories(attendanceCodeCategoriesAddViewModel);
            }
            catch (Exception es)
            {
                attendanceCodeCategoriesUpdate._failure = true;
                attendanceCodeCategoriesUpdate._message = es.Message;
            }
            return attendanceCodeCategoriesUpdate;
        }
        [HttpPost("getAllAttendanceCodeCategories")]

        public ActionResult<AttendanceCodeCategoriesListViewModel> GetAllAttendanceCodeCategories(AttendanceCodeCategoriesListViewModel attendanceCodeCategoriesListViewModel)
        {
            AttendanceCodeCategoriesListViewModel attendanceCodeCategoriesList = new AttendanceCodeCategoriesListViewModel();
            try
            {
                if (attendanceCodeCategoriesListViewModel.SchoolId > 0)
                {
                    attendanceCodeCategoriesList = _attendanceCodeRegisterService.GetAllAttendanceCodeCategories(attendanceCodeCategoriesListViewModel);
                }
                else
                {
                    attendanceCodeCategoriesList._token = attendanceCodeCategoriesListViewModel._token;
                    attendanceCodeCategoriesList._tenantName = attendanceCodeCategoriesListViewModel._tenantName;
                    attendanceCodeCategoriesList._failure = true;
                    attendanceCodeCategoriesList._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                attendanceCodeCategoriesList._message = es.Message;
                attendanceCodeCategoriesList._failure = true;
            }
            return attendanceCodeCategoriesList;
        }
        [HttpPost("deleteAttendanceCodeCategories")]

        public ActionResult<AttendanceCodeCategoriesAddViewModel> DeleteAttendanceCodeCategories(AttendanceCodeCategoriesAddViewModel attendanceCodeCategoriesAddViewModel)
        {
            AttendanceCodeCategoriesAddViewModel attendanceCodeCategorieslDelete = new AttendanceCodeCategoriesAddViewModel();
            try
            {
                attendanceCodeCategorieslDelete = _attendanceCodeRegisterService.DeleteAttendanceCodeCategories(attendanceCodeCategoriesAddViewModel);
            }
            catch (Exception es)
            {
                attendanceCodeCategorieslDelete._failure = true;
                attendanceCodeCategorieslDelete._message = es.Message;
            }
            return attendanceCodeCategorieslDelete;
        }


        [HttpPost("updateAttendanceCodeSortOrder")]
        public ActionResult<AttendanceCodeSortOrderModel> UpdateAttendanceCodeSortOrder(AttendanceCodeSortOrderModel attendanceCodeSortOrderModel)
        {
            AttendanceCodeSortOrderModel attendanceCodeSortOrderUpdate = new AttendanceCodeSortOrderModel();
            try
            {
                attendanceCodeSortOrderUpdate = _attendanceCodeRegisterService.UpdateAttendanceCodeSortOrder(attendanceCodeSortOrderModel);
            }
            catch (Exception es)
            {
                attendanceCodeSortOrderUpdate._failure = true;
                attendanceCodeSortOrderUpdate._message = es.Message;
            }
            return attendanceCodeSortOrderUpdate;
        }
    }
}
