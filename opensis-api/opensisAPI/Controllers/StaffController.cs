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
using opensis.data.ViewModels.Staff;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using opensis.core.Staff.Interfaces;
using opensis.data.Models;

namespace opensisAPI.Controllers
{

    [EnableCors("AllowOrigin")]
    [Route("{tenant}/Staff")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private IStaffService _staffService;
        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        [HttpPost("addStaff")]
        public ActionResult<StaffAddViewModel> AddStaff(StaffAddViewModel staffAddViewModel)
        {
            StaffAddViewModel staffInfoAddViewModel = new StaffAddViewModel();
            try
            {
                staffInfoAddViewModel = _staffService.AddStaff(staffAddViewModel);

            }
            catch (Exception es)
            {
                staffInfoAddViewModel._failure = true;
                staffInfoAddViewModel._message = es.Message;
            }
            return staffInfoAddViewModel;
        }

        [HttpPost("getAllStaffList")]
        public ActionResult<StaffListModel> GetAllStaffList(PageResult pageResult)
        {
            StaffListModel staffList = new StaffListModel();
            try
            {
                staffList = _staffService.GetAllStaffList(pageResult);
            }
            catch (Exception es)
            {
                staffList._message = es.Message;
                staffList._failure = true;
            }
            return staffList;
        }

        [HttpPost("viewStaff")]
        public ActionResult<StaffAddViewModel> ViewStaff(StaffAddViewModel staffAddViewModel)
        {
            StaffAddViewModel staffView = new StaffAddViewModel();
            try
            {
                if (staffAddViewModel.staffMaster.SchoolId > 0)
                {
                    staffView = _staffService.ViewStaff(staffAddViewModel);
                }
                else
                {
                    staffView._token = staffAddViewModel._token;
                    staffView._tenantName = staffAddViewModel._tenantName;
                    staffView._failure = true;
                    staffView._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                staffView._failure = true;
                staffView._message = es.Message;
            }
            return staffView;
        }

        [HttpPut("updateStaff")]
        public ActionResult<StaffAddViewModel> UpdateStaff(StaffAddViewModel staffAddViewModel)
        {
            StaffAddViewModel staffUpdate = new StaffAddViewModel();
            try
            {
                if (staffAddViewModel.staffMaster.SchoolId > 0)
                {
                    staffUpdate = _staffService.UpdateStaff(staffAddViewModel);
                }
                else
                {
                    staffUpdate._token = staffAddViewModel._token;
                    staffUpdate._tenantName = staffAddViewModel._tenantName;
                    staffUpdate._failure = true;
                    staffUpdate._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                staffUpdate._failure = true;
                staffUpdate._message = es.Message;
            }
            return staffUpdate;
        }

        [HttpPost("checkStaffInternalId")]
        public ActionResult<CheckStaffInternalIdViewModel> CheckStaffInternalId(CheckStaffInternalIdViewModel checkStaffInternalIdViewModel)
        {
            CheckStaffInternalIdViewModel checkInternalId = new CheckStaffInternalIdViewModel();
            try
            {
                checkInternalId = _staffService.CheckStaffInternalId(checkStaffInternalIdViewModel);
            }
            catch (Exception es)
            {
                checkInternalId._message = es.Message;
                checkInternalId._failure = true;
            }
            return checkInternalId;
        }

        [HttpPost("addStaffSchoolInfo")]
        public ActionResult<StaffSchoolInfoAddViewModel> AddStaffSchoolInfo(StaffSchoolInfoAddViewModel staffSchoolInfoAddViewModel)
        {
            StaffSchoolInfoAddViewModel staffSchoolInfoAdd = new StaffSchoolInfoAddViewModel();
            try
            {
                staffSchoolInfoAdd = _staffService.AddStaffSchoolInfo(staffSchoolInfoAddViewModel);
            }
            catch (Exception es)
            {
                staffSchoolInfoAdd._failure = true;
                staffSchoolInfoAdd._message = es.Message;
            }
            return staffSchoolInfoAdd;
        }

        [HttpPost("viewStaffSchoolInfo")]
        public ActionResult<StaffSchoolInfoAddViewModel> ViewStaffSchoolInfo(StaffSchoolInfoAddViewModel staffSchoolInfoAddViewModel)
        {
            StaffSchoolInfoAddViewModel staffSchoolInfoView = new StaffSchoolInfoAddViewModel();
            try
            {
                if (staffSchoolInfoAddViewModel.SchoolId > 0)
                {
                    staffSchoolInfoView = _staffService.ViewStaffSchoolInfo(staffSchoolInfoAddViewModel);
                }
                else
                {
                    staffSchoolInfoView._token = staffSchoolInfoAddViewModel._token;
                    staffSchoolInfoView._tenantName = staffSchoolInfoAddViewModel._tenantName;
                    staffSchoolInfoView._failure = true;
                    staffSchoolInfoView._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                staffSchoolInfoView._failure = true;
                staffSchoolInfoView._message = es.Message;
            }
            return staffSchoolInfoView;
        }

        [HttpPut("updateStaffSchoolInfo")]
        public ActionResult<StaffSchoolInfoAddViewModel> UpdateStaffSchoolInfo(StaffSchoolInfoAddViewModel staffSchoolInfoAddViewModel)
        {
            StaffSchoolInfoAddViewModel staffSchoolInfoUpdate = new StaffSchoolInfoAddViewModel();
            try
            {
                if (staffSchoolInfoAddViewModel.SchoolId > 0)
                {
                    staffSchoolInfoUpdate = _staffService.UpdateStaffSchoolInfo(staffSchoolInfoAddViewModel);
                }
                else
                {
                    staffSchoolInfoUpdate._token = staffSchoolInfoAddViewModel._token;
                    staffSchoolInfoUpdate._tenantName = staffSchoolInfoAddViewModel._tenantName;
                    staffSchoolInfoUpdate._failure = true;
                    staffSchoolInfoUpdate._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                staffSchoolInfoUpdate._failure = true;
                staffSchoolInfoUpdate._message = es.Message;
            }
            return staffSchoolInfoUpdate;
        }


        [HttpPost("addStaffCertificateInfo")]
        public ActionResult<StaffCertificateInfoAddViewModel> AddStaffCertificateInfo(StaffCertificateInfoAddViewModel staffCertificateInfoAddViewModel)
        {
            StaffCertificateInfoAddViewModel staffCertificateInfoAdd = new StaffCertificateInfoAddViewModel();
            try
            {
                staffCertificateInfoAdd = _staffService.AddStaffCertificateInfo(staffCertificateInfoAddViewModel);

            }
            catch (Exception es)
            {
                staffCertificateInfoAdd._failure = true;
                staffCertificateInfoAdd._message = es.Message;
            }
            return staffCertificateInfoAdd;
        }

        [HttpPost("getAllStaffCertificateInfo")]
        public ActionResult<StaffCertificateInfoListModel> GetAllStaffCertificateInfo(StaffCertificateInfoListModel staffCertificateInfoListModel)
        {
            StaffCertificateInfoListModel staffCertificateInfoList = new StaffCertificateInfoListModel();
            try
            {
                if (staffCertificateInfoListModel.SchoolId > 0)
                {
                    staffCertificateInfoList = _staffService.GetAllStaffCertificateInfo(staffCertificateInfoListModel);
                }
                else
                {
                    staffCertificateInfoList._token = staffCertificateInfoListModel._token;
                    staffCertificateInfoList._tenantName = staffCertificateInfoListModel._tenantName;
                    staffCertificateInfoList._failure = true;
                    staffCertificateInfoList._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                staffCertificateInfoList._message = es.Message;
                staffCertificateInfoList._failure = true;
            }
            return staffCertificateInfoList;
        }

        [HttpPut("updateStaffCertificateInfo")]
        public ActionResult<StaffCertificateInfoAddViewModel> UpdateStaffCertificateInfo(StaffCertificateInfoAddViewModel staffCertificateInfoAddViewModel)
        {
            StaffCertificateInfoAddViewModel staffCertificateInfoUpdate = new StaffCertificateInfoAddViewModel();
            try
            {
                staffCertificateInfoUpdate = _staffService.UpdateStaffCertificateInfo(staffCertificateInfoAddViewModel);
            }
            catch (Exception es)
            {
                staffCertificateInfoUpdate._failure = true;
                staffCertificateInfoUpdate._message = es.Message;
            }
            return staffCertificateInfoUpdate;
        }

        [HttpPost("deleteStaffCertificateInfo")]
        public ActionResult<StaffCertificateInfoAddViewModel> DeleteStaffCertificateInfo(StaffCertificateInfoAddViewModel staffCertificateInfoAddViewModel)
        {
            StaffCertificateInfoAddViewModel staffCertificateInfolDelete = new StaffCertificateInfoAddViewModel();
            try
            {
                staffCertificateInfolDelete = _staffService.DeleteStaffCertificateInfo(staffCertificateInfoAddViewModel);
            }
            catch (Exception es)
            {
                staffCertificateInfolDelete._failure = true;
                staffCertificateInfolDelete._message = es.Message;
            }
            return staffCertificateInfolDelete;
        }

        [HttpPut("addUpdateStaffPhoto")]
        public ActionResult<StaffAddViewModel> AddUpdateStaffPhoto(StaffAddViewModel staffAddViewModel)
        {
            StaffAddViewModel staffPhotoUpdate = new StaffAddViewModel();
            try
            {
                staffPhotoUpdate = _staffService.AddUpdateStaffPhoto(staffAddViewModel);
            }
            catch (Exception es)
            {
                staffPhotoUpdate._failure = true;
                staffPhotoUpdate._message = es.Message;
            }
            return staffPhotoUpdate;
        }

        [HttpPost("addStaffList")]
        public ActionResult<StaffListAddViewModel> AddStaffList(StaffListAddViewModel staffListAddViewModel)
        {
            StaffListAddViewModel staffListAdd = new StaffListAddViewModel();
            try
            {
                staffListAdd = _staffService.AddStaffList(staffListAddViewModel);
            }
            catch (Exception es)
            {
                staffListAdd._failure = true;
                staffListAdd._message = es.Message;
            }
            return staffListAdd;
        }
    }
}
