using opensis.data.ViewModels.RoleBasedAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.RoleBasedAccess.Interfaces
{
    public interface IRoleBasedAccessService
    {
        public PermissionGroupListViewModel GetAllPermissionGroup(PermissionGroupListViewModel permissionGroupListViewModel);
        public PermissionGroupAddViewModel UpdatePermissionGroup(PermissionGroupAddViewModel permissionGroupAddViewModel);
        public PermissionGroupListViewModel UpdateRolePermission(PermissionGroupListViewModel permissionGroupListViewModel);
        public RolePermissionListViewModel GetAllRolePermission(RolePermissionListViewModel rolePermissionListViewModel);
    }
}
