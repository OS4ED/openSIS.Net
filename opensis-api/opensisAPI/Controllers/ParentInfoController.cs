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
using opensis.core.ParentInfo.Interfaces;
using opensis.data.Models;
using opensis.data.ViewModels.ParentInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace opensisAPI.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("{tenant}/ParentInfo")]
    [ApiController]
    public class ParentInfoController : ControllerBase
    {
        private IParentInfoRegisterService _parentInfoRegisterService;
        public ParentInfoController(IParentInfoRegisterService parentInfoRegisterService)
        {
            _parentInfoRegisterService = parentInfoRegisterService;
        }
        [HttpPost("addParentForStudent")]
        public ActionResult<ParentInfoAddViewModel> AddParentForStudent(ParentInfoAddViewModel parentInfoAddViewModel)
        {
            ParentInfoAddViewModel parentInfoAdd = new ParentInfoAddViewModel();
            try
            {
                parentInfoAdd = _parentInfoRegisterService.AddParentForStudent(parentInfoAddViewModel);
            }
            catch (Exception es)
            {
                parentInfoAdd._failure = true;
                parentInfoAdd._message = es.Message;
            }
            return parentInfoAdd;
        }
        [HttpPost("viewParentListForStudent")]

        public ActionResult<ParentInfoListModel> ViewParentListForStudent(ParentInfoListModel parentInfoListViewModel)
        {
            ParentInfoListModel parentInfoViewList = new ParentInfoListModel();
            try
            {                
                parentInfoViewList = _parentInfoRegisterService.ViewParentListForStudent(parentInfoListViewModel);
            }
            catch (Exception es)
            {
                parentInfoViewList._failure = true;
                parentInfoViewList._message = es.Message;
            }
            return parentInfoViewList;
        }
        [HttpPut("updateParentInfo")]

        public ActionResult<ParentInfoAddViewModel> UpdateParentInfo(ParentInfoAddViewModel parentInfoAddViewModel)
        {
            ParentInfoAddViewModel parentInfoUpdate = new ParentInfoAddViewModel();
            try
            {
                parentInfoUpdate = _parentInfoRegisterService.UpdateParentInfo(parentInfoAddViewModel);
            }
            catch (Exception es)
            {
                parentInfoUpdate._failure = true;
                parentInfoUpdate._message = es.Message;
            }
            return parentInfoUpdate;
        }
        [HttpPost("getAllParentInfo")]

        public ActionResult<GetAllParentInfoListForView> GetAllParentInfo(PageResult pageResult)
        {

            GetAllParentInfoListForView parentInfoList = new GetAllParentInfoListForView();
            try
            {
                parentInfoList = _parentInfoRegisterService.GetAllParentInfoList(pageResult);
            }
            catch (Exception es)
            {
                parentInfoList._message = es.Message;
                parentInfoList._failure = true;
            }
            return parentInfoList;
        }
        [HttpPost("deleteParentInfo")]

        public ActionResult<ParentInfoAddViewModel> DeleteParentInfo(ParentInfoAddViewModel parentInfoAddViewModel)
        {
            ParentInfoAddViewModel parentInfoDelete = new ParentInfoAddViewModel();
            try
            {
                if (parentInfoAddViewModel.parentInfo.SchoolId > 0)
                {
                    parentInfoDelete = _parentInfoRegisterService.DeleteParentInfo(parentInfoAddViewModel);
                }
                else
                {
                    parentInfoDelete._token = parentInfoAddViewModel._token;
                    parentInfoDelete._tenantName = parentInfoAddViewModel._tenantName;
                    parentInfoDelete._failure = true;
                    parentInfoDelete._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                parentInfoDelete._failure = true;
                parentInfoDelete._message = es.Message;
            }
            return parentInfoDelete;
        }

        [HttpPost("searchParentInfoForStudent")]

        public ActionResult<GetAllParentInfoListForView> SearchParentInfoForStudent(GetAllParentInfoListForView getAllParentInfoListForView)
        {

            GetAllParentInfoListForView parentInfoList = new GetAllParentInfoListForView();
            try
            {
                parentInfoList = _parentInfoRegisterService.SearchParentInfoForStudent(getAllParentInfoListForView);
            }
            catch (Exception es)
            {
                parentInfoList._message = es.Message;
                parentInfoList._failure = true;
            }
            return parentInfoList;
        }

        [HttpPost("viewParentInfo")]
        public ActionResult<ParentInfoAddViewModel> ViewParentInfo(ParentInfoAddViewModel parentInfoAddViewModel)
        {
            ParentInfoAddViewModel parentInfoView = new ParentInfoAddViewModel();
            try
            {
                parentInfoView = _parentInfoRegisterService.ViewParentInfo(parentInfoAddViewModel);
            }
            catch (Exception es)
            {

                parentInfoView._failure = true;
                parentInfoView._message = es.Message;
            }
            return parentInfoView;
        }

        [HttpPost("addParentInfo")]
        public ActionResult<ParentInfoAddViewModel> AddParentInfo(ParentInfoAddViewModel parentInfoAddViewModel)
        {
            ParentInfoAddViewModel parentInfoAdd = new ParentInfoAddViewModel();
            try
            {
                parentInfoAdd = _parentInfoRegisterService.AddParentInfo(parentInfoAddViewModel);
            }
            catch (Exception es)
            {
                parentInfoAdd._failure = true;
                parentInfoAdd._message = es.Message;
            }
            return parentInfoAdd;
        }

        [HttpPost("removeAssociatedParent")]
        public ActionResult<ParentInfoDeleteViewModel> RemoveAssociatedParent(ParentInfoDeleteViewModel parentInfoDeleteViewModel)
        {
            ParentInfoDeleteViewModel parentAssociationshipDelete = new ParentInfoDeleteViewModel();
            try
            {
                if (parentInfoDeleteViewModel.parentInfo.SchoolId > 0)
                {
                    parentAssociationshipDelete = _parentInfoRegisterService.RemoveAssociatedParent(parentInfoDeleteViewModel);
                }
                else
                {
                    parentAssociationshipDelete._token = parentInfoDeleteViewModel._token;
                    parentAssociationshipDelete._tenantName = parentInfoDeleteViewModel._tenantName;
                    parentAssociationshipDelete._failure = true;
                    parentAssociationshipDelete._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                parentAssociationshipDelete._failure = true;
                parentAssociationshipDelete._message = es.Message;
            }
            return parentAssociationshipDelete;
        }

        [HttpPut("addUpdateParentPhoto")]
        public ActionResult<ParentInfoAddViewModel> AddUpdateParentPhoto(ParentInfoAddViewModel parentInfoAddViewModel)
        {
            ParentInfoAddViewModel parentPhotoUpdate = new ParentInfoAddViewModel();
            try
            {
                parentPhotoUpdate = _parentInfoRegisterService.AddUpdateParentPhoto(parentInfoAddViewModel);
            }
            catch (Exception es)
            {
                parentPhotoUpdate._failure = true;
                parentPhotoUpdate._message = es.Message;
            }
            return parentPhotoUpdate;
        }
    }
}
