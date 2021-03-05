using Microsoft.EntityFrameworkCore;
using opensis.data.Helper;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.RoleBasedAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace opensis.data.Repository
{
    public class RoleBasedAccessRepository : IRoleBasedAccessRepository
    {
        private CRMContext context;
        private static readonly string NORECORDFOUND = "No Record Found";
        public RoleBasedAccessRepository(IDbContextFactory dbContextFactory)
        {
            this.context = dbContextFactory.Create();
        }

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
                var permission = this.context?.PermissionGroup.Where(x => x.TenantId == permissionGroupListViewModel.TenantId && x.SchoolId == permissionGroupListViewModel.SchoolId);

                permissionGroupListView.permissionGroupList = permission.ToList();
                permissionGroupListView._token = permissionGroupListViewModel._token;

                if (permission.Count() > 0)
                {
                    permissionGroupListView._failure = false;
                    permissionGroupListView._message = "Permission Group List Fetched";
                }
                else
                {
                    permissionGroupListView._failure = true;
                    permissionGroupListView._message = NORECORDFOUND;
                }
            }
            catch (Exception ex)
            {
                permissionGroupListView._message = ex.Message;
                permissionGroupListView._failure = true;
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

                rolePermissionListView.PermissionList = new List<RolePermissionViewModel>();

                //var permissionGroup = this.context?.PermissionGroups.Include(pc => pc.PermissionCategories).ThenInclude(rp => rp.RolePermissions.Where//(a=>a.RoleId==objModel.role.RoleId)).Where(x => x.TenantId == objModel.role.TenantId && x.HospitalId == objModel.role.HospitalId && x.IsActive == true );

                var permissionGroup = this.context?.PermissionGroup.Select(pg => new PermissionGroup
                {
                    PermissionGroupId = pg.PermissionGroupId,
                    TenantId = pg.TenantId,
                    SchoolId = pg.SchoolId,
                    PermissionGroupName = pg.PermissionGroupName,
                    ShortName = pg.ShortName,
                    IsActive = pg.IsActive,
                    IsSystem = pg.IsSystem,
                    Title = pg.Title,
                    IconType = pg.IconType,
                    Icon = pg.Icon,
                    SortOrder = pg.SortOrder,
                    Type = pg.Type,
                    Path = pg.Path,
                    BadgeType = pg.BadgeType,
                    BadgeValue = pg.BadgeValue,
                    Active = pg.Active,
                    PermissionCategory = (ICollection<PermissionCategory>)pg.PermissionCategory.Select(pc => new PermissionCategory
                    {
                        PermissionCategoryId = pc.PermissionCategoryId,
                        TenantId = pc.TenantId,
                        SchoolId = pc.SchoolId,
                        PermissionGroupId = pc.PermissionGroupId,
                        PermissionCategoryName = pc.PermissionCategoryName,
                        ShortCode = pc.ShortCode,
                        Path = pc.Path,
                        Title = pc.Title,
                        Type = pc.Type,
                        EnableView = pc.EnableView,
                        EnableAdd = pc.EnableAdd,
                        EnableDelete = pc.EnableDelete,
                        EnableEdit = pc.EnableEdit,
                        PermissionSubcategory = (ICollection<PermissionSubcategory>)pc.PermissionSubcategory.Select(psc => new PermissionSubcategory
                        {
                            TenantId = psc.TenantId,
                            SchoolId = psc.SchoolId,
                            PermissionCategoryId = psc.PermissionCategoryId,
                            PermissionSubcategoryId = psc.PermissionSubcategoryId,
                            PermissionGroupId = psc.PermissionGroupId,
                            PermissionSubcategoryName = psc.PermissionSubcategoryName,
                            ShortCode = psc.ShortCode,
                            Path = psc.Path,
                            Title = psc.Title,
                            Type = psc.Type,
                            EnableView = psc.EnableView,
                            EnableAdd = psc.EnableAdd,
                            EnableDelete = psc.EnableDelete,
                            EnableEdit = psc.EnableEdit,
                            RolePermission = (ICollection<RolePermission>)psc.RolePermission.Select(scrp => new RolePermission
                            {
                                RolePermissionId = scrp.RolePermissionId,
                                TenantId = scrp.TenantId,
                                SchoolId = scrp.SchoolId,
                                MembershipId = scrp.MembershipId,
                                PermissionCategoryId = scrp.PermissionCategoryId,
                                PermissionSubcategoryId=scrp.PermissionSubcategoryId,
                                CanView = scrp.CanView,
                                CanAdd = scrp.CanAdd,
                                CanDelete = scrp.CanDelete,
                                CanEdit = scrp.CanEdit,

                            }).Where(x => x.TenantId == rolePermissionListViewModel.TenantId && x.SchoolId == rolePermissionListViewModel.SchoolId && x.MembershipId == rolePermissionListViewModel.MembershipId)
                        }).Where(x => x.TenantId == rolePermissionListViewModel.TenantId && x.SchoolId == rolePermissionListViewModel.SchoolId),
                        RolePermission = (ICollection<RolePermission>)pc.RolePermission.Select(rp => new RolePermission
                        {
                            RolePermissionId = rp.RolePermissionId,
                            TenantId = rp.TenantId,
                            SchoolId = rp.SchoolId,
                            MembershipId = rp.MembershipId,
                            PermissionCategoryId = rp.PermissionCategoryId,
                            CanView = rp.CanView,
                            CanAdd = rp.CanAdd,
                            CanDelete = rp.CanDelete,
                            CanEdit = rp.CanEdit,

                        }).Where(x => x.TenantId == rolePermissionListViewModel.TenantId && x.SchoolId == rolePermissionListViewModel.SchoolId && x.MembershipId == rolePermissionListViewModel.MembershipId)
                    }).Where(x => x.TenantId == rolePermissionListViewModel.TenantId && x.SchoolId == rolePermissionListViewModel.SchoolId)
                }).Where(x => x.TenantId == rolePermissionListViewModel.TenantId && x.SchoolId == rolePermissionListViewModel.SchoolId && x.IsActive == true).OrderBy(x => x.SortOrder).ToList();

                if (permissionGroup.Count() > 0)
                {
                    foreach (PermissionGroup pg in permissionGroup.ToList())
                    {
                        RolePermissionViewModel pgvm = new RolePermissionViewModel();
                        pgvm.permissionGroup = new PermissionGroup();
                        pgvm.permissionGroup = pg;

                        //Get Role permission 
                        foreach (PermissionCategory pc in pg.PermissionCategory)
                        {
                            if (pc.RolePermission.Count == 0)
                            {

                                RolePermission rp = new RolePermission();
                                rp.PermissionCategoryId = pc.PermissionCategoryId;
                                rp.MembershipId = rolePermissionListViewModel.MembershipId;
                                rp.CanAdd = false;
                                rp.CanDelete = false;
                                rp.CanEdit = false;
                                rp.CanView = false;
                                rp.TenantId = rolePermissionListViewModel.TenantId;
                                rp.SchoolId = rolePermissionListViewModel.SchoolId;
                                pc.RolePermission.Add(rp);
                            }
                        }
                        rolePermissionListView.PermissionList.Add(pgvm);
                    }
                    //objViewModel.PermissionList = permissionGroup.ToList(); 
                    rolePermissionListView._failure = false;
                    rolePermissionListView._message = "Permission List Fetched";
                }
                else
                {
                    rolePermissionListView._failure = true;
                    rolePermissionListView._message = NORECORDFOUND;
                }
                rolePermissionListView._token = rolePermissionListViewModel._token;
                rolePermissionListView.TenantId = rolePermissionListViewModel.TenantId;
                rolePermissionListView.SchoolId = rolePermissionListViewModel.SchoolId;
                rolePermissionListView.MembershipId = rolePermissionListViewModel.MembershipId;
            }
            catch (Exception ex)
            {
                rolePermissionListView._message = ex.Message;
                rolePermissionListView._failure = true;
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
            PermissionGroupAddViewModel permissionGroupUpdateModel = new PermissionGroupAddViewModel();
            try
            {
                var permissionGroup = this.context?.PermissionGroup.FirstOrDefault(x => x.TenantId == permissionGroupAddViewModel.permissionGroup.TenantId && x.SchoolId == permissionGroupAddViewModel.permissionGroup.SchoolId && x.PermissionGroupId == permissionGroupAddViewModel.permissionGroup.PermissionGroupId);

                if (permissionGroup != null)
                {
                    permissionGroup.IsActive = permissionGroupAddViewModel.permissionGroup.IsActive;

                    this.context?.SaveChanges();
                    permissionGroupUpdateModel.permissionGroup = permissionGroup;
                    permissionGroupUpdateModel._token = permissionGroupAddViewModel._token;
                    permissionGroupUpdateModel._message = "Update Successful";
                    permissionGroupUpdateModel._failure = false;
                }
                else
                {
                    permissionGroupUpdateModel.permissionGroup = null;
                    permissionGroupUpdateModel._failure = true;
                    permissionGroupUpdateModel._message = "No Permission Group Found";
                }
            }
            catch (Exception ex)
            {
                permissionGroupUpdateModel._failure = true;
                permissionGroupUpdateModel._message = ex.Message;
            }
            return permissionGroupUpdateModel;
        }

        /// <summary>
        /// Update Role Permission
        /// </summary>
        /// <param name="permissionGroupListViewModel"></param>
        /// <returns></returns>
        public PermissionGroupListViewModel UpdateRolePermission(PermissionGroupListViewModel permissionGroupListViewModel)
        {
            using var transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
            PermissionGroupListViewModel PermissionGroupViewModel = new PermissionGroupListViewModel();
            try
            {
                foreach (PermissionGroup p_group in permissionGroupListViewModel.permissionGroupList)
                {
                    var PermissionGroup = this.context?.PermissionGroup.FirstOrDefault(x => x.TenantId == p_group.TenantId && x.SchoolId == p_group.SchoolId && x.PermissionGroupId == p_group.PermissionGroupId);

                    if (PermissionGroup != null)
                    {
                        PermissionGroup.IsActive = p_group.IsActive;                     
                    }
                    foreach (PermissionCategory p_cat in p_group.PermissionCategory)
                    {
                        if (p_cat.PermissionSubcategory.Count > 0)
                        {
                            foreach (PermissionSubcategory p_subcat in p_cat.PermissionSubcategory)
                            {
                                foreach (RolePermission roleper in p_subcat.RolePermission)
                                {
                                    var RolePermission = this.context?.RolePermission.FirstOrDefault(x => x.TenantId == roleper.TenantId && x.SchoolId == roleper.SchoolId && x.RolePermissionId == roleper.RolePermissionId && x.PermissionCategoryId == roleper.PermissionCategoryId && x.PermissionSubcategoryId == roleper.PermissionSubcategoryId);

                                    if (RolePermission != null)
                                    {
                                        RolePermission.CanAdd = roleper.CanAdd;
                                        RolePermission.CanEdit = roleper.CanEdit;
                                        RolePermission.CanDelete = roleper.CanDelete;
                                        RolePermission.CanView = roleper.CanView;
                                        this.context?.SaveChanges();
                                    }
                                    else
                                    {
                                        int? AutoId = 1;

                                        var rolePermissionData = this.context?.RolePermission.Where(x => x.SchoolId == roleper.SchoolId && x.TenantId == roleper.TenantId).OrderByDescending(x => x.RolePermissionId).FirstOrDefault();

                                        if (rolePermissionData != null)
                                        {
                                            AutoId = rolePermissionData.RolePermissionId + 1;
                                        }
                                        roleper.RolePermissionId = (int)AutoId;
                                        roleper.CreatedBy = permissionGroupListViewModel.CreatedBy;
                                        roleper.CreatedOn = DateTime.UtcNow;
                                        this.context?.RolePermission.Add(roleper);
                                        this.context?.SaveChanges();
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (RolePermission roleper in p_cat.RolePermission)
                            {
                                var RolePermission = this.context?.RolePermission.FirstOrDefault(x => x.TenantId == roleper.TenantId && x.SchoolId == roleper.SchoolId && x.RolePermissionId == roleper.RolePermissionId && x.PermissionCategoryId == roleper.PermissionCategoryId);

                                if (RolePermission != null)
                                {
                                    RolePermission.CanAdd = roleper.CanAdd;
                                    RolePermission.CanEdit = roleper.CanEdit;
                                    RolePermission.CanDelete = roleper.CanDelete;
                                    RolePermission.CanView = roleper.CanView;
                                    this.context?.SaveChanges();
                                }
                                else
                                {
                                    int? AutoId = 1;

                                    var rolePermissionData = this.context?.RolePermission.Where(x => x.SchoolId == roleper.SchoolId && x.TenantId == roleper.TenantId).OrderByDescending(x => x.RolePermissionId).FirstOrDefault();

                                    if (rolePermissionData != null)
                                    {
                                        AutoId = rolePermissionData.RolePermissionId + 1;
                                    }
                                    roleper.RolePermissionId = (int)AutoId;
                                    roleper.CreatedBy = permissionGroupListViewModel.CreatedBy;
                                    roleper.CreatedOn = DateTime.UtcNow;
                                    this.context?.RolePermission.Add(roleper);
                                    this.context?.SaveChanges();
                                }
                            }
                        }
                    }
                }
                transaction.Commit();
                PermissionGroupViewModel.permissionGroupList = permissionGroupListViewModel.permissionGroupList;
                PermissionGroupViewModel._failure = false;
                PermissionGroupViewModel._message = "UPDATE SUCCESSFUL";
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            return PermissionGroupViewModel;
        }
    }
}
