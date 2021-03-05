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
