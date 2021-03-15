using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.User;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.IO;
using opensis.data.Helper;
using opensis.data.ViewModels.RoleBasedAccess;

namespace opensis.data.Repository
{
    public class UserRepository : IUserRepository
    {
        private CRMContext context;
        private static string EMAILMESSAGE = "Email address is not registered in the system";
        private static string PASSWORDMESSAGE = "Email + password combination is incorrect, try again";
        public UserRepository(IDbContextFactory dbContextFactory)
        {
            this.context = dbContextFactory.Create();
        }



        /// <summary>
        ///  ValidateUserLogin method is used for authentcatred the login process
        /// </summary>
        /// <param name="objModel"></param>
        /// <returns></returns>

        public LoginViewModel ValidateUserLogin(LoginViewModel objModel)
        {
            LoginViewModel ReturnModel = new LoginViewModel();
            try
            {

                var decrypted = Utility.Decrypt(objModel.Password);

                string passwordHash = Utility.GetHashedPassword(decrypted);
                ReturnModel._tenantName = objModel._tenantName;
                //var encryptedPassword = EncodePassword(objModel.Password);
                var user = this.context?.UserMaster.Include(x => x.Membership).Where(x => x.EmailAddress == objModel.Email && x.TenantId == objModel.TenantId
                  && x.PasswordHash == passwordHash).FirstOrDefault();
                var correctEmailList = this.context?.UserMaster.Where(x => x.EmailAddress.Contains(objModel.Email)).ToList();
                var correctPasswordList = this.context?.UserMaster.Where(x => x.PasswordHash == passwordHash).ToList();
                if (user == null && correctEmailList.Count > 0 && correctPasswordList.Count == 0)
                {
                    ReturnModel.UserId = null;
                    ReturnModel._failure = true;
                    ReturnModel._message = PASSWORDMESSAGE;
                }
                else if (user == null && correctEmailList.Count == 0 && correctPasswordList.Count > 0)
                {
                    ReturnModel.UserId = null;
                    ReturnModel._failure = true;
                    ReturnModel._message = EMAILMESSAGE;
                }
                else if (user == null && correctEmailList.Count == 0 && correctPasswordList.Count == 0)
                {
                    ReturnModel.UserId = null;
                    ReturnModel._failure = true;
                    ReturnModel._message = PASSWORDMESSAGE;
                }
                else
                {
                    ReturnModel.UserId = user.UserId;
                    ReturnModel.TenantId = user.TenantId;
                    ReturnModel.Email = user.EmailAddress;
                    ReturnModel.Name = user.Name;
                    ReturnModel.MembershipName = user.Membership.Profile;
                    ReturnModel.MembershipId = user.Membership.MembershipId;
                    ReturnModel._failure = false;
                    ReturnModel._message = "";

                    if (objModel.SchoolId == null)
                    {
                        objModel.SchoolId = 1;
                    }
                    RolePermissionListViewModel rolePermissionListView = new RolePermissionListViewModel();
                    rolePermissionListView.PermissionList = new List<RolePermissionViewModel>();

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
                                    PermissionSubcategoryId = scrp.PermissionSubcategoryId,
                                    CanView = scrp.CanView,
                                    CanAdd = scrp.CanAdd,
                                    CanDelete = scrp.CanDelete,
                                    CanEdit = scrp.CanEdit,

                                }).Where(x => x.TenantId == objModel.TenantId && x.SchoolId == objModel.SchoolId && x.MembershipId == 1)
                            }).Where(x => x.TenantId == objModel.TenantId && x.SchoolId == objModel.SchoolId),
                            RolePermission = (ICollection<RolePermission>)pc.RolePermission.Select(crp => new RolePermission
                            {
                                RolePermissionId = crp.RolePermissionId,
                                TenantId = crp.TenantId,
                                SchoolId = crp.SchoolId,
                                MembershipId = crp.MembershipId,
                                PermissionCategoryId = crp.PermissionCategoryId,
                                CanView = crp.CanView,
                                CanAdd = crp.CanAdd,
                                CanDelete = crp.CanDelete,
                                CanEdit = crp.CanEdit,

                            }).Where(x => x.TenantId == objModel.TenantId && x.SchoolId == objModel.SchoolId && x.MembershipId == 1)
                        }).Where(x => x.TenantId == objModel.TenantId && x.SchoolId == objModel.SchoolId),
                        RolePermission = (ICollection<RolePermission>)pg.RolePermission.Select(grp => new RolePermission
                        {
                            RolePermissionId = grp.RolePermissionId,
                            TenantId = grp.TenantId,
                            SchoolId = grp.SchoolId,
                            MembershipId = grp.MembershipId,
                            PermissionGroupId = grp.PermissionGroupId,
                            CanView = grp.CanView,
                            CanAdd = grp.CanAdd,
                            CanDelete = grp.CanDelete,
                            CanEdit = grp.CanEdit,

                        }).Where(x => x.TenantId == objModel.TenantId && x.SchoolId == objModel.SchoolId && x.MembershipId == 1)
                    }).Where(x => x.TenantId == objModel.TenantId && x.SchoolId == objModel.SchoolId).OrderBy(x => x.SortOrder).ToList();

                    if (permissionGroup.Count() > 0)
                    {
                        foreach (PermissionGroup pg in permissionGroup.ToList())
                        {
                            RolePermissionViewModel pgvm = new RolePermissionViewModel();
                            pgvm.permissionGroup = new PermissionGroup();
                            pgvm.permissionGroup = pg;
                            rolePermissionListView.PermissionList.Add(pgvm);
                        }

                        ReturnModel.PermissionList = rolePermissionListView.PermissionList;
                        ReturnModel.SchoolId = objModel.SchoolId;
                    }
                }
            }

            catch (Exception ex)
            {
                ReturnModel._failure = true;
                ReturnModel._message = ex.Message;
            }


            return ReturnModel;
        }

        public CheckUserEmailAddressViewModel CheckUserLoginEmail(CheckUserEmailAddressViewModel checkUserEmailAddressViewModel)
        {
            var checkEmailAddress = this.context?.UserMaster.Where(x => x.TenantId == checkUserEmailAddressViewModel.TenantId && x.EmailAddress == checkUserEmailAddressViewModel.EmailAddress).ToList();
            if (checkEmailAddress.Count() > 0)
            {
                checkUserEmailAddressViewModel.IsValidEmailAddress = false;
                checkUserEmailAddressViewModel._message = "User Login Email Address Already Exist";
            }
            else
            {
                checkUserEmailAddressViewModel.IsValidEmailAddress = true;
                checkUserEmailAddressViewModel._message = "User Login Email Address Is Valid";
            }
            return checkUserEmailAddressViewModel;
        }

    }
}
