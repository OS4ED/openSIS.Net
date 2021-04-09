using opensis.core.helper;
using opensis.core.RoleBasedAccess.Interfaces;
using opensis.data.Interface;
using opensis.data.ViewModels.RoleBasedAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.RoleBasedAccess.Services
{
    public class RoleBasedAccessService : IRoleBasedAccessService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";

        public IRoleBasedAccessRepository roleBasedAccessRepository;
        public RoleBasedAccessService(IRoleBasedAccessRepository roleBasedAccessRepository)
        {
            this.roleBasedAccessRepository = roleBasedAccessRepository;
        }

        //Required for Unit Testing
        public RoleBasedAccessService() { }

        /// <summary>
        /// Get All Permission Group
        /// </summary>
        /// <param name="permissionGroupListViewModel"></param>
        /// <returns></returns>
        public PermissionGroupListViewModel GetAllPermissionGroup(PermissionGroupListViewModel permissionGroupListViewModel)
        {
            PermissionGroupListViewModel permissionGroupListView = new PermissionGroupListViewModel();
            try
            {
                if (TokenManager.CheckToken(permissionGroupListViewModel._tenantName + permissionGroupListViewModel._userName, permissionGroupListViewModel._token))
                {
                    permissionGroupListView = this.roleBasedAccessRepository.GetAllPermissionGroup(permissionGroupListViewModel);
                }
                else
                {
                    permissionGroupListView._failure = true;
                    permissionGroupListView._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                permissionGroupListView._failure = true;
                permissionGroupListView._message = es.Message;
            }
            return permissionGroupListView;
        }

        /// <summary>
        /// Get All Role Permission
        /// </summary>
        /// <param name="rolePermissionListViewModel"></param>
        /// <returns></returns>
        public RolePermissionListViewModel GetAllRolePermission(RolePermissionListViewModel rolePermissionListViewModel)
        {
            RolePermissionListViewModel rolePermissionListView = new RolePermissionListViewModel();
            try
            {
                if (TokenManager.CheckToken(rolePermissionListViewModel._tenantName + rolePermissionListViewModel._userName, rolePermissionListViewModel._token))
                {
                    rolePermissionListView = this.roleBasedAccessRepository.GetAllRolePermission(rolePermissionListViewModel);
                }
                else
                {
                    rolePermissionListView._failure = true;
                    rolePermissionListView._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                rolePermissionListView._failure = true;
                rolePermissionListView._message = es.Message;
            }
            return rolePermissionListView;
        }

        /// <summary>
        /// Update Permission Group
        /// </summary>
        /// <param name="permissionGroupAddViewModel"></param>
        /// <returns></returns>
        public PermissionGroupAddViewModel UpdatePermissionGroup(PermissionGroupAddViewModel permissionGroupAddViewModel)
        {
            PermissionGroupAddViewModel permissionGroupUpdate = new PermissionGroupAddViewModel();
            try
            {
                if (TokenManager.CheckToken(permissionGroupAddViewModel._tenantName + permissionGroupAddViewModel._userName, permissionGroupAddViewModel._token))
                {
                    permissionGroupUpdate = this.roleBasedAccessRepository.UpdatePermissionGroup(permissionGroupAddViewModel);
                }
                else
                {
                    permissionGroupUpdate._failure = true;
                    permissionGroupUpdate._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                permissionGroupUpdate._failure = true;
                permissionGroupUpdate._message = es.Message;
            }
            return permissionGroupUpdate;
        }

        /// <summary>
        /// Update Role Permission
        /// </summary>
        /// <param name="permissionGroupListViewModel"></param>
        /// <returns></returns>
        public PermissionGroupListViewModel UpdateRolePermission(PermissionGroupListViewModel permissionGroupListViewModel)
        {
            PermissionGroupListViewModel rolePermissionUpdate = new PermissionGroupListViewModel();
            try
            {
                if (TokenManager.CheckToken(permissionGroupListViewModel._tenantName + permissionGroupListViewModel._userName, permissionGroupListViewModel._token))
                {
                    rolePermissionUpdate = this.roleBasedAccessRepository.UpdateRolePermission(permissionGroupListViewModel);
                }
                else
                {
                    rolePermissionUpdate._failure = true;
                    rolePermissionUpdate._message = TOKENINVALID;
                }
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
