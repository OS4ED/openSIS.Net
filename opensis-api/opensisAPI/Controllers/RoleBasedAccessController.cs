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
using opensis.core.RoleBasedAccess.Interfaces;
using opensis.data.ViewModels.RoleBasedAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace opensisAPI.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("{tenant}/RoleBasedAccess")]
    [ApiController]
    public class RoleBasedAccessController : ControllerBase
    {
        private IRoleBasedAccessService _roleBasedAccessService;
        public RoleBasedAccessController(IRoleBasedAccessService roleBasedAccessService)
        {
            _roleBasedAccessService = roleBasedAccessService;
        }

        [HttpPost("getAllPermissionGroup")]
        public ActionResult<PermissionGroupListViewModel> GetAllPermissionGroup(PermissionGroupListViewModel permissionGroupListViewModel)
        {
            PermissionGroupListViewModel permissionGroupListView = new PermissionGroupListViewModel();
            try
            {
                permissionGroupListView = _roleBasedAccessService.GetAllPermissionGroup(permissionGroupListViewModel);
            }
            catch (Exception es)
            {
                permissionGroupListView._failure = true;
                permissionGroupListView._message = es.Message;
            }
            return permissionGroupListView;
        }

        [HttpPost("getAllRolePermission")]
        public ActionResult<RolePermissionListViewModel> GetAllRolePermission(RolePermissionListViewModel rolePermissionListViewModel)
        {
            RolePermissionListViewModel rolePermissionListView = new RolePermissionListViewModel();
            try
            {
                rolePermissionListView = _roleBasedAccessService.GetAllRolePermission(rolePermissionListViewModel);
            }
            catch (Exception es)
            {
                rolePermissionListView._failure = true;
                rolePermissionListView._message = es.Message;
            }
            return rolePermissionListView;
        }


        [HttpPut("updatePermissionGroup")]
        public ActionResult<PermissionGroupAddViewModel> UpdatePermissionGroup(PermissionGroupAddViewModel permissionGroupAddViewModel)
        {
            PermissionGroupAddViewModel permissionGroupUpdate = new PermissionGroupAddViewModel();
            try
            {
                permissionGroupUpdate = _roleBasedAccessService.UpdatePermissionGroup(permissionGroupAddViewModel);
            }
            catch (Exception es)
            {
                permissionGroupUpdate._failure = true;
                permissionGroupUpdate._message = es.Message;
            }
            return permissionGroupUpdate;
        }

        [HttpPut("updateRolePermission")]
        public ActionResult<PermissionGroupListViewModel> UpdateRolePermission(PermissionGroupListViewModel permissionGroupListViewModel)
        {
            PermissionGroupListViewModel rolePermissionUpdate = new PermissionGroupListViewModel();
            try
            {
                rolePermissionUpdate = _roleBasedAccessService.UpdateRolePermission(permissionGroupListViewModel);
            }
            catch (Exception es)
            {
                rolePermissionUpdate._failure = true;
                rolePermissionUpdate._message = es.Message;
            }
            return rolePermissionUpdate;
        }
    }
}
