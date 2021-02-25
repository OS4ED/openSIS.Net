using Microsoft.EntityFrameworkCore;
using opensis.data.Helper;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.ParentInfos;
using opensis.data.ViewModels.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace opensis.data.Repository
{
    public class ParentInfoRepository : IParentInfoRepository
    {
        private CRMContext context;
        private static readonly string NORECORDFOUND = "NO RECORD FOUND";
        public ParentInfoRepository(IDbContextFactory dbContextFactory)
        {
            this.context = dbContextFactory.Create();
        }

        /// <summary>
        /// Add Parent For Student
        /// </summary>
        /// <param name="parentInfoAddViewModel"></param>
        /// <returns></returns>
        public ParentInfoAddViewModel AddParentForStudent(ParentInfoAddViewModel parentInfoAddViewModel)
        {
            using (var transaction = this.context.Database.BeginTransaction())
            {
                try
                {
                    if (parentInfoAddViewModel.parentInfo.ParentId > 0)
                    {
                        var parentInfo = this.context?.ParentInfo.FirstOrDefault(x => x.ParentId == parentInfoAddViewModel.parentInfo.ParentId);
                        if (parentInfo != null)
                        {
                            var AssociationshipData = this.context?.ParentAssociationship.FirstOrDefault(x => x.TenantId == parentInfo.TenantId && x.SchoolId == parentInfo.SchoolId && x.ParentId == parentInfo.ParentId && x.StudentId== parentInfoAddViewModel.parentAssociationship.StudentId);
                            if (AssociationshipData != null)
                            {
                                AssociationshipData.Associationship = true;
                            }
                            else
                            {
                                var parentAssociationship = new ParentAssociationship { TenantId = parentInfoAddViewModel.parentInfo.TenantId, SchoolId = parentInfoAddViewModel.parentInfo.SchoolId, ParentId = parentInfoAddViewModel.parentInfo.ParentId, StudentId = parentInfoAddViewModel.parentAssociationship.StudentId, Associationship = true, LastUpdated = DateTime.UtcNow, UpdatedBy = parentInfoAddViewModel.parentInfo.UpdatedBy, IsCustodian = parentInfoAddViewModel.parentAssociationship.IsCustodian, Relationship = parentInfoAddViewModel.parentAssociationship.Relationship,ContactType= parentInfoAddViewModel.parentAssociationship.ContactType };
                                this.context?.ParentAssociationship.Add(parentAssociationship);
                            }
                        }
                    }
                    else
                    {
                        int? ParentId = Utility.GetMaxPK(this.context, new Func<ParentInfo, int>(x => x.ParentId));
                        parentInfoAddViewModel.parentInfo.ParentId = (int)ParentId;
                        parentInfoAddViewModel.parentInfo.LastUpdated = DateTime.UtcNow;
                        Guid GuidId = Guid.NewGuid();
                        var GuidIdExist = this.context?.ParentInfo.FirstOrDefault(x => x.ParentGuid == GuidId);
                        if (GuidIdExist != null)
                        {
                            parentInfoAddViewModel._failure = true;
                            parentInfoAddViewModel._message = "Guid is already exist, Please try again.";
                            return parentInfoAddViewModel;
                        }
                        parentInfoAddViewModel.parentInfo.ParentGuid = GuidId;

                        //Add Parent Portal Access
                        if (!string.IsNullOrWhiteSpace(parentInfoAddViewModel.PasswordHash) && !string.IsNullOrWhiteSpace(parentInfoAddViewModel.parentInfo.LoginEmail))
                        {
                            UserMaster userMaster = new UserMaster();

                            var decrypted = Utility.Decrypt(parentInfoAddViewModel.PasswordHash);
                            string passwordHash = Utility.GetHashedPassword(decrypted);

                            var loginInfo = this.context?.UserMaster.FirstOrDefault(x => x.TenantId == parentInfoAddViewModel.parentInfo.TenantId && x.EmailAddress == parentInfoAddViewModel.parentInfo.LoginEmail);

                            if (loginInfo == null)
                            {
                                var membership = this.context?.Membership.FirstOrDefault(x => x.TenantId == parentInfoAddViewModel.parentInfo.TenantId && x.SchoolId == parentInfoAddViewModel.parentInfo.SchoolId && x.Profile == "Parent");

                                userMaster.SchoolId = parentInfoAddViewModel.parentInfo.SchoolId;
                                userMaster.TenantId = parentInfoAddViewModel.parentInfo.TenantId;
                                userMaster.UserId = parentInfoAddViewModel.parentInfo.ParentId;
                                userMaster.LangId = 1;
                                userMaster.MembershipId = membership.MembershipId;
                                userMaster.EmailAddress = parentInfoAddViewModel.parentInfo.LoginEmail;
                                userMaster.PasswordHash = passwordHash;
                                userMaster.Name = parentInfoAddViewModel.parentInfo.Firstname;
                                userMaster.IsActive = parentInfoAddViewModel.parentInfo.IsPortalUser;

                                parentInfoAddViewModel.parentInfo.LoginEmail = parentInfoAddViewModel.parentInfo.LoginEmail;

                                this.context?.UserMaster.Add(userMaster);
                                this.context?.SaveChanges();
                            }
                            else
                            {
                                parentInfoAddViewModel.parentInfo = null;
                                parentInfoAddViewModel._failure = true;
                                parentInfoAddViewModel._message = "Parent Login Email Already Exist";
                                return parentInfoAddViewModel;
                            }
                        }

                        this.context?.ParentInfo.Add(parentInfoAddViewModel.parentInfo);
                        var parentAssociationship = new ParentAssociationship { TenantId = parentInfoAddViewModel.parentInfo.TenantId, SchoolId = parentInfoAddViewModel.parentInfo.SchoolId, ParentId = parentInfoAddViewModel.parentInfo.ParentId, StudentId = parentInfoAddViewModel.parentAssociationship.StudentId, Associationship = true, LastUpdated = DateTime.UtcNow, UpdatedBy = parentInfoAddViewModel.parentInfo.UpdatedBy, IsCustodian = parentInfoAddViewModel.parentAssociationship.IsCustodian, Relationship = parentInfoAddViewModel.parentAssociationship.Relationship, ContactType = parentInfoAddViewModel.parentAssociationship.ContactType};
                        this.context?.ParentAssociationship.Add(parentAssociationship);

                    }
                    this.context?.SaveChanges();                
                    transaction.Commit();
                    parentInfoAddViewModel._failure = false;
                    parentInfoAddViewModel._message = "Parent Added Successfully";
                }
                catch (Exception es)
                {
                    transaction.Rollback();
                    parentInfoAddViewModel._message = es.Message;
                    parentInfoAddViewModel._failure = true;
                    parentInfoAddViewModel._tenantName = parentInfoAddViewModel._tenantName;
                    parentInfoAddViewModel._token = parentInfoAddViewModel._token;
                }
            }
            return parentInfoAddViewModel;
        }
        /// <summary>
        /// View Parent List For Student
        /// </summary>
        /// <param name="parentInfoList"></param>
        /// <returns></returns>
        public ParentInfoListModel ViewParentListForStudent(ParentInfoListModel parentInfoList)
        {
            ParentInfoListModel parentInfoListViewModel = new ParentInfoListModel();
            try
            {
                var parentAssociationship = this.context?.ParentAssociationship.Where(x => x.StudentId == parentInfoList.StudentId && x.SchoolId == parentInfoList.SchoolId && x.TenantId == parentInfoList.TenantId && x.Associationship == true).ToList();
                if(parentAssociationship.Count() > 0)
                {
                    foreach(var parent in parentAssociationship)
                    {
                        var parentData = this.context?.ParentInfo.Include(x => x.ParentAddress).FirstOrDefault(x => x.ParentId == parent.ParentId);
                        if(parentData != null)
                        {
                            if(parentData.ParentAddress.FirstOrDefault().StudentAddressSame == true)
                            {
                                var parentAddress = this.context?.StudentMaster.FirstOrDefault(x => x.StudentId == parentInfoList.StudentId && x.SchoolId == parentInfoList.SchoolId);
                                parentData.ParentAddress.FirstOrDefault().AddressLineOne = parentAddress.HomeAddressLineOne;
                                parentData.ParentAddress.FirstOrDefault().AddressLineTwo = parentAddress.HomeAddressLineTwo;
                                parentData.ParentAddress.FirstOrDefault().Country = parentAddress.HomeAddressCountry;
                                parentData.ParentAddress.FirstOrDefault().State = parentAddress.HomeAddressState;
                                parentData.ParentAddress.FirstOrDefault().City = parentAddress.HomeAddressCity;
                                parentData.ParentAddress.FirstOrDefault().Zip = parentAddress.HomeAddressZip;
                            }
                        }
                        var parentInfoListData = new ParentInfoListForView
                        {
                            TenantId= parentData.TenantId,
                            SchoolId= parentData.SchoolId,
                            ParentId= parentData.ParentId,
                            Firstname = parentData.Firstname,
                            Lastname = parentData.Lastname,
                            HomePhone = parentData.HomePhone,
                            WorkPhone = parentData.WorkPhone,
                            Mobile = parentData.Mobile,
                            IsPortalUser = parentData.IsPortalUser,
                            BusPickup = parentData.BusPickup,
                            BusDropoff = parentData.BusDropoff,
                            ContactType = parent.ContactType,
                            LastUpdated = parentData.LastUpdated,
                            UpdatedBy = parentData.UpdatedBy,
                            BusNo = parentData.BusNo,
                            Middlename = parentData.Middlename,
                            PersonalEmail = parentData.PersonalEmail,
                            Salutation = parentData.Salutation,
                            Suffix = parentData.Suffix,
                            UserProfile = parentData.UserProfile,
                            WorkEmail = parentData.WorkEmail,
                            ParentPhoto = parentData.ParentPhoto,
                            IsCustodian = parent.IsCustodian,
                            Relationship = parent.Relationship,
                            ParentAddress=parentData.ParentAddress.FirstOrDefault(),
                            LoginEmail= parentData.LoginEmail
                        };
                        parentInfoListViewModel.parentInfoListForView.Add(parentInfoListData);
                    }                 
                    parentInfoListViewModel._tenantName = parentInfoList._tenantName;
                    parentInfoListViewModel._token = parentInfoList._token;
                    parentInfoListViewModel._failure = false;
                }
                else
                {
                    parentInfoListViewModel._failure = true;
                    parentInfoListViewModel._message = NORECORDFOUND;
                    return parentInfoListViewModel;
                }
            }
            catch (Exception es)
            {
                parentInfoListViewModel._failure = true;
                parentInfoListViewModel._message = es.Message;
            }
            return parentInfoListViewModel;
        }
        /// <summary>
        /// Update Parent Info
        /// </summary>
        /// <param name="parentInfoAddViewModel"></param>
        /// <returns></returns>
        public ParentInfoAddViewModel UpdateParentInfo(ParentInfoAddViewModel parentInfoAddViewModel)
        {
            using (var transaction = this.context.Database.BeginTransaction())
            {
                try
                {
                    var parentInfoUpdate = this.context?.ParentInfo.Include(x => x.ParentAddress).FirstOrDefault(x => x.TenantId == parentInfoAddViewModel.parentInfo.TenantId && x.SchoolId == parentInfoAddViewModel.parentInfo.SchoolId && x.ParentId == parentInfoAddViewModel.parentInfo.ParentId);

                    //Add or Update parent portal access
                    if (parentInfoUpdate.LoginEmail != null)
                    {
                        if (!string.IsNullOrWhiteSpace(parentInfoAddViewModel.parentInfo.LoginEmail))
                        {
                            if (parentInfoUpdate.LoginEmail != parentInfoAddViewModel.parentInfo.LoginEmail)
                            {
                                var loginInfo = this.context?.UserMaster.FirstOrDefault(x => x.TenantId == parentInfoAddViewModel.parentInfo.TenantId && x.EmailAddress == parentInfoAddViewModel.parentInfo.LoginEmail);

                                if (loginInfo != null)
                                {
                                    parentInfoAddViewModel.parentInfo = null;
                                    parentInfoAddViewModel._failure = true;
                                    parentInfoAddViewModel._message = "Parent Login Email Already Exist";
                                    return parentInfoAddViewModel;
                                }
                                else
                                {
                                    var loginInfoData = this.context?.UserMaster.FirstOrDefault(x => x.TenantId == parentInfoAddViewModel.parentInfo.TenantId && x.EmailAddress == parentInfoUpdate.LoginEmail);

                                    loginInfoData.EmailAddress = parentInfoAddViewModel.parentInfo.LoginEmail;
                                    loginInfoData.IsActive = parentInfoAddViewModel.parentInfo.IsPortalUser;

                                    this.context?.UserMaster.Add(loginInfoData);
                                    this.context?.SaveChanges();

                                    //Update Parent Login in ParentInfo table.
                                    //parentInfoUpdate.LoginEmail = parentInfoAddViewModel.parentInfo.LoginEmail;
                                }
                            }
                            else
                            {
                                var loginInfo = this.context?.UserMaster.FirstOrDefault(x => x.TenantId == parentInfoAddViewModel.parentInfo.TenantId && x.EmailAddress == parentInfoUpdate.LoginEmail);

                                loginInfo.IsActive = parentInfoAddViewModel.parentInfo.IsPortalUser;

                                this.context?.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(parentInfoAddViewModel.parentInfo.LoginEmail) && !string.IsNullOrWhiteSpace(parentInfoAddViewModel.PasswordHash))
                        {
                            var decrypted = Utility.Decrypt(parentInfoAddViewModel.PasswordHash);
                            string passwordHash = Utility.GetHashedPassword(decrypted);

                            UserMaster userMaster = new UserMaster();

                            var loginInfo = this.context?.UserMaster.FirstOrDefault(x => x.TenantId == parentInfoAddViewModel.parentInfo.TenantId && x.EmailAddress == parentInfoAddViewModel.parentInfo.LoginEmail);

                            if (loginInfo == null)
                            {
                                var membership = this.context?.Membership.FirstOrDefault(x => x.TenantId == parentInfoAddViewModel.parentInfo.TenantId && x.SchoolId == parentInfoAddViewModel.parentInfo.SchoolId && x.Profile == "Parent");

                                userMaster.SchoolId = parentInfoAddViewModel.parentInfo.SchoolId;
                                userMaster.TenantId = parentInfoAddViewModel.parentInfo.TenantId;
                                userMaster.UserId = parentInfoAddViewModel.parentInfo.ParentId;
                                userMaster.LangId = 1;
                                userMaster.MembershipId = membership.MembershipId;
                                userMaster.EmailAddress = parentInfoAddViewModel.parentInfo.LoginEmail;
                                userMaster.PasswordHash = passwordHash;
                                userMaster.Name = parentInfoAddViewModel.parentInfo.Firstname;
                                userMaster.LastUpdated = DateTime.UtcNow;
                                userMaster.IsActive = parentInfoAddViewModel.parentInfo.IsPortalUser;

                                this.context?.UserMaster.Add(userMaster);
                                this.context?.SaveChanges();

                                //Update LoginEmail in ParentInfo table.
                                //parentInfoUpdate.LoginEmail = parentInfoAddViewModel.parentInfo.LoginEmail;
                            }
                            else
                            {
                                parentInfoAddViewModel.parentInfo = null;
                                parentInfoAddViewModel._failure = true;
                                parentInfoAddViewModel._message = "Parent Login Email Already Exist";
                                return parentInfoAddViewModel;
                            }
                        }
                    }
                    
                    parentInfoAddViewModel.parentInfo.UserProfile = parentInfoUpdate.UserProfile;
                    parentInfoAddViewModel.parentInfo.ParentGuid =parentInfoUpdate.ParentGuid;
                    parentInfoAddViewModel.parentInfo.LastUpdated = DateTime.UtcNow;
                    this.context.Entry(parentInfoUpdate).CurrentValues.SetValues(parentInfoAddViewModel.parentInfo);
                    this.context?.SaveChanges();

                    if (parentInfoUpdate.ParentAddress.Count() > 0 && parentInfoAddViewModel.parentInfo.ParentAddress.Count() > 0)
                    {                      
                        if (parentInfoAddViewModel.parentInfo.ParentAddress.FirstOrDefault().StudentAddressSame == true)
                        {
                            var studentAddress = this.context?.StudentMaster.FirstOrDefault(x => x.StudentId == parentInfoUpdate.ParentAddress.FirstOrDefault().StudentId && x.SchoolId == parentInfoUpdate.SchoolId);
                            studentAddress.HomeAddressLineOne = parentInfoAddViewModel.parentInfo.ParentAddress.FirstOrDefault().AddressLineOne;
                            studentAddress.HomeAddressLineTwo = parentInfoAddViewModel.parentInfo.ParentAddress.FirstOrDefault().AddressLineTwo;
                            studentAddress.HomeAddressCountry = parentInfoAddViewModel.parentInfo.ParentAddress.FirstOrDefault().Country;
                            studentAddress.HomeAddressState = parentInfoAddViewModel.parentInfo.ParentAddress.FirstOrDefault().State;
                            studentAddress.HomeAddressCity = parentInfoAddViewModel.parentInfo.ParentAddress.FirstOrDefault().City;
                            studentAddress.HomeAddressZip = parentInfoAddViewModel.parentInfo.ParentAddress.FirstOrDefault().Zip;
                            if (studentAddress.MailingAddressSameToHome == true)
                            {
                                studentAddress.MailingAddressLineOne = parentInfoAddViewModel.parentInfo.ParentAddress.FirstOrDefault().AddressLineOne;
                                studentAddress.MailingAddressLineTwo = parentInfoAddViewModel.parentInfo.ParentAddress.FirstOrDefault().AddressLineTwo;
                                studentAddress.MailingAddressCountry = parentInfoAddViewModel.parentInfo.ParentAddress.FirstOrDefault().Country;
                                studentAddress.MailingAddressState = parentInfoAddViewModel.parentInfo.ParentAddress.FirstOrDefault().State;
                                studentAddress.MailingAddressCity = parentInfoAddViewModel.parentInfo.ParentAddress.FirstOrDefault().City;
                                studentAddress.MailingAddressZip = parentInfoAddViewModel.parentInfo.ParentAddress.FirstOrDefault().Zip;
                            }
                            parentInfoUpdate.ParentAddress.FirstOrDefault().StudentAddressSame = true;
                            parentInfoUpdate.ParentAddress.FirstOrDefault().AddressLineOne = null;
                            parentInfoUpdate.ParentAddress.FirstOrDefault().AddressLineTwo = null;
                            parentInfoUpdate.ParentAddress.FirstOrDefault().Country = null;
                            parentInfoUpdate.ParentAddress.FirstOrDefault().City = null;
                            parentInfoUpdate.ParentAddress.FirstOrDefault().State = null;
                            parentInfoUpdate.ParentAddress.FirstOrDefault().Zip = null;
                            parentInfoUpdate.ParentAddress.FirstOrDefault().UpdatedBy = parentInfoAddViewModel.parentInfo.ParentAddress.FirstOrDefault().UpdatedBy;
                            parentInfoUpdate.ParentAddress.FirstOrDefault().LastUpdated = DateTime.UtcNow;
                            this.context?.SaveChanges();
                        }
                        else
                        {                          
                            parentInfoAddViewModel.parentInfo.ParentAddress.FirstOrDefault().StudentAddressSame = false;
                            parentInfoAddViewModel.parentInfo.ParentAddress.FirstOrDefault().LastUpdated = DateTime.UtcNow;
                            this.context.Entry(parentInfoUpdate.ParentAddress.FirstOrDefault()).CurrentValues.SetValues(parentInfoAddViewModel.parentInfo.ParentAddress.FirstOrDefault());
                            this.context?.SaveChanges();
                        }
                    }
                    this.context?.SaveChanges();
                    parentInfoAddViewModel._message = "Parent Updated Successfully";
                    parentInfoAddViewModel._failure = false;
                    transaction.Commit();
                }
                catch (Exception es)
                {
                    transaction.Rollback();
                    parentInfoAddViewModel._failure = true;
                    parentInfoAddViewModel._message = es.Message;
                }
            }
            return parentInfoAddViewModel;
        }
        private static string ToFullAddress(string Address1, string Address2, string City, string State, string Country, string Zip)
        {
            string address = "";
            if (!string.IsNullOrWhiteSpace(Address1))
            {


                return address == null
                      ? null
                      : $"{Address1?.Trim()}{(!string.IsNullOrWhiteSpace(Address2) ? $", {Address2?.Trim()}" : string.Empty)}, {City?.Trim()}, {State?.Trim()} {Zip?.Trim()}";
            }
            return address;
        }
        /// <summary>
        /// Get All Parent Info
        /// </summary>
        /// <param name="pageResult"></param>
        /// <returns></returns>
        public GetAllParentInfoListForView GetAllParentInfoList(PageResult pageResult)
        {
            GetAllParentInfoListForView parentInfoListModel = new GetAllParentInfoListForView();
            IQueryable<ParentInfo> transactionIQ = null;

            var containParentId = this.context?.ParentAssociationship.Where(x => x.TenantId == pageResult.TenantId && x.SchoolId == pageResult.SchoolId  && x.Associationship == true).Select(x => x.ParentId).Distinct().ToList();
            
            List<int> parentIDs = new List<int> { };
            parentIDs = containParentId;

            var ParentInfoList = this.context?.ParentInfo.Include(x=>x.ParentAddress).ToList().Where(x => x.TenantId == pageResult.TenantId && x.SchoolId==pageResult.SchoolId &&(parentIDs == null || (parentIDs.Contains(x.ParentId))));
            
            try
            {
                transactionIQ = ParentInfoList.AsQueryable();
                int totalCount = transactionIQ.Count();

                foreach(var parent in ParentInfoList.ToList())
                {
                    if(parent.ParentAddress.FirstOrDefault().StudentAddressSame==true)
                    {
                        var studentAddress = this.context?.StudentMaster.Where(x => x.TenantId == parent.ParentAddress.FirstOrDefault().TenantId && x.SchoolId == parent.ParentAddress.FirstOrDefault().SchoolId && x.StudentId==parent.ParentAddress.FirstOrDefault().StudentId).FirstOrDefault();

                        if(studentAddress != null)
                        {
                            parent.ParentAddress.FirstOrDefault().AddressLineOne = studentAddress.HomeAddressLineOne;
                            parent.ParentAddress.FirstOrDefault().AddressLineTwo = studentAddress.HomeAddressLineTwo;
                            parent.ParentAddress.FirstOrDefault().City = studentAddress.HomeAddressCity;
                            parent.ParentAddress.FirstOrDefault().Country = studentAddress.HomeAddressCountry;
                            parent.ParentAddress.FirstOrDefault().State = studentAddress.HomeAddressState;
                            parent.ParentAddress.FirstOrDefault().Zip = studentAddress.HomeAddressZip;
                        }
                    }                  
                }
                var parentInfo = transactionIQ.AsNoTracking().Select(y => new GetParentInfoForView
                {
                    SchoolId = y.SchoolId,
                    TenantId = y.TenantId,
                    ParentId=y.ParentId,
                    Firstname = y.Firstname,
                    Middlename=y.Middlename,
                    Lastname = y.Lastname,
                    Salutation=y.Salutation,
                    Suffix=y.Suffix,
                    WorkEmail=y.WorkEmail,
                    WorkPhone=y.WorkPhone,
                    HomePhone=y.HomePhone,
                    PersonalEmail=y.PersonalEmail,
                    Mobile=y.Mobile,
                    UserProfile=y.UserProfile,
                    AddressLineOne = ToFullAddress(y.ParentAddress.FirstOrDefault().AddressLineOne, y.ParentAddress.FirstOrDefault().AddressLineTwo, y.ParentAddress.FirstOrDefault().City,
                    y.ParentAddress.FirstOrDefault().State, y.ParentAddress.FirstOrDefault().Country, y.ParentAddress.FirstOrDefault().Zip)
                }).ToList();                
                parentInfoListModel.parentInfoForView = parentInfo;
                foreach (var ParentInfo in parentInfoListModel.parentInfoForView)
                {
                    var studentAssociateWithParents = this.context?.ParentAssociationship.Where(x => x.ParentId == ParentInfo.ParentId && x.Associationship == true).ToList();
                    List<string> studentArray = new List<string>();
                    if (studentAssociateWithParents.Count() > 0)
                    {
                        foreach (var studentAssociateWithParent in studentAssociateWithParents)
                        {
                            var student = this.context?.StudentMaster.FirstOrDefault(x => x.StudentId == studentAssociateWithParent.StudentId && x.SchoolId == studentAssociateWithParent.SchoolId && x.TenantId == studentAssociateWithParent.TenantId);
                            if (student != null)
                            {                               
                                studentArray.Add(student.FirstGivenName + " " + student.LastFamilyName + "|" + student.StudentId);
                                ParentInfo.students = studentArray.ToArray();
                            }
                        }
                    }
                }
                parentInfoListModel.TenantId = pageResult.TenantId;
                parentInfoListModel.TotalCount = totalCount;
                parentInfoListModel.PageNumber = 0;
                parentInfoListModel._pageSize = 0;
                parentInfoListModel._tenantName = pageResult._tenantName;
                parentInfoListModel._token = pageResult._token;
                parentInfoListModel._failure = false;
            }
            catch (Exception es)
            {
                parentInfoListModel._message = es.Message;
                parentInfoListModel._failure = true;
                parentInfoListModel._tenantName = pageResult._tenantName;
                parentInfoListModel._token = pageResult._token;
            }
            return parentInfoListModel;
        }
        /// <summary>
        /// Delete Parent Info
        /// </summary>
        /// <param name="parentInfoAddViewModel"></param>
        /// <returns></returns>
        public ParentInfoAddViewModel DeleteParentInfo(ParentInfoAddViewModel parentInfoAddViewModel)
        {
            try
            {
                var ParentInfo = this.context?.ParentInfo.FirstOrDefault(x => x.TenantId == parentInfoAddViewModel.parentInfo.TenantId && x.SchoolId == parentInfoAddViewModel.parentInfo.SchoolId && x.ParentId == parentInfoAddViewModel.parentInfo.ParentId);
                this.context?.ParentInfo.Remove(ParentInfo);
                this.context?.SaveChanges();
                parentInfoAddViewModel._failure = false;
                parentInfoAddViewModel._message = "Parent Deleted Successfully";
            }
            catch (Exception es)
            {
                parentInfoAddViewModel._failure = true;
                parentInfoAddViewModel._message = es.Message;
            }
            return parentInfoAddViewModel;
        }
        /// <summary>
        /// Search ParentInfo For Student
        /// </summary>
        /// <param name="getAllParentInfoListForView"></param>
        /// <returns></returns>
        public GetAllParentInfoListForView SearchParentInfoForStudent(GetAllParentInfoListForView getAllParentInfoListForView)
        {
            GetAllParentInfoListForView parentInfoListModel = new GetAllParentInfoListForView();
            try
            {
                var containParentId = this.context?.ParentAssociationship.Where(x => x.TenantId == getAllParentInfoListForView.TenantId && x.SchoolId == getAllParentInfoListForView.SchoolId && x.StudentId == getAllParentInfoListForView.StudentId && x.Associationship == true).Select(x => x.ParentId).ToList();
                //string parentIDs = null;
                //if (containParentId.Count() > 0)
                //{
                //    parentIDs = string.Join(",", containParentId);
                //}

                List<int> parentIDs = new List<int> { };
                parentIDs = containParentId;

                var ParentInfoList = this.context?.ParentInfo.Include(x => x.ParentAddress).ToList().Where(x => x.TenantId == getAllParentInfoListForView.TenantId && (getAllParentInfoListForView.Firstname == null || (x.Firstname == getAllParentInfoListForView.Firstname)) && (getAllParentInfoListForView.Lastname == null || (x.Lastname == getAllParentInfoListForView.Lastname)) && (getAllParentInfoListForView.Email == null || (x.PersonalEmail == getAllParentInfoListForView.Email)) && (getAllParentInfoListForView.Mobile == null || (x.Mobile == getAllParentInfoListForView.Mobile)) && (getAllParentInfoListForView.StreetAddress == null || (x.ParentAddress.FirstOrDefault().AddressLineOne == getAllParentInfoListForView.StreetAddress)) && (getAllParentInfoListForView.City == null || (x.ParentAddress.FirstOrDefault().City == getAllParentInfoListForView.City)) && (getAllParentInfoListForView.State == null || (x.ParentAddress.FirstOrDefault().State == getAllParentInfoListForView.State)) && (getAllParentInfoListForView.Zip == null || (x.ParentAddress.FirstOrDefault().Zip == getAllParentInfoListForView.Zip)) && (parentIDs == null || (!parentIDs.Contains(x.ParentId))));

                //var ParentInfoList1 = (from parent in this.context?.ParentInfo join address in this.context?.ParentAddress on parent.ParentId equals address.ParentId where parent.TenantId == getAllParentInfoListForView.TenantId && parent.Firstname == getAllParentInfoListForView.Firstname && parent.Lastname == getAllParentInfoListForView.Lastname && parent.PersonalEmail == getAllParentInfoListForView.Email && parent.Mobile == getAllParentInfoListForView.Mobile && address.AddressLineOne == getAllParentInfoListForView.StreetAddress && address.City == getAllParentInfoListForView.City && address.State == getAllParentInfoListForView.State && address.Zip == getAllParentInfoListForView.Zip select new { parent.TenantId, parent.Firstname, parent.Lastname, parent.PersonalEmail, parent.Mobile, address.AddressLineOne, address.City, address.State, address.Zip, }).ToList();

                //var parentInfo = ParentInfoList.Select(y => new GetParentInfoForView
                //{
                //    SchoolId = y.SchoolId,
                //    ParentId = y.ParentId,
                //    Firstname = y.Firstname,
                //    Lastname = y.Lastname,
                //    Mobile = y.Mobile,
                //    WorkPhone = y.WorkPhone,
                //    HomePhone = y.HomePhone,
                //    PersonalEmail = y.PersonalEmail,
                //    WorkEmail = y.WorkEmail,
                //    LoginEmail = y.LoginEmail,
                //    UserProfile = y.UserProfile,
                //    IsPortalUser = y.IsPortalUser,                    
                //    TenantId = y.TenantId,
                //    AddressLineOne = ToFullAddress(y.ParentAddress.FirstOrDefault().AddressLineOne, y.ParentAddress.FirstOrDefault().AddressLineTwo,
                //int.TryParse(y.ParentAddress.FirstOrDefault().City, out resultData) == true ? this.context.City.Where(x => x.Id == Convert.ToInt32(y.ParentAddress.FirstOrDefault().City)).FirstOrDefault().Name : y.ParentAddress.FirstOrDefault().City,
                //int.TryParse(y.ParentAddress.FirstOrDefault().State, out resultData) == true ? this.context.State.Where(x => x.Id == Convert.ToInt32(y.ParentAddress.FirstOrDefault().State)).FirstOrDefault().Name : y.ParentAddress.FirstOrDefault().State,
                //int.TryParse(y.ParentAddress.FirstOrDefault().Country, out resultData) == true ? this.context.Country.Where(x => x.Id == Convert.ToInt32(y.ParentAddress.FirstOrDefault().Country)).FirstOrDefault().Name : string.Empty, y.ParentAddress.FirstOrDefault().Zip),
                //}).ToList();

                foreach (var parent in ParentInfoList)
                {
                    if (parent.ParentAddress.FirstOrDefault().StudentAddressSame == true)
                    {
                        var parentAddress = this.context?.StudentMaster.FirstOrDefault(x => x.StudentId == parent.ParentAddress.FirstOrDefault().StudentId && x.SchoolId == parent.SchoolId);
                        parent.ParentAddress.FirstOrDefault().AddressLineOne = parentAddress.HomeAddressLineOne;
                        parent.ParentAddress.FirstOrDefault().AddressLineTwo = parentAddress.HomeAddressLineTwo;
                        parent.ParentAddress.FirstOrDefault().Country = parentAddress.HomeAddressCountry;
                        parent.ParentAddress.FirstOrDefault().State = parentAddress.HomeAddressState;
                        parent.ParentAddress.FirstOrDefault().City = parentAddress.HomeAddressCity;
                        parent.ParentAddress.FirstOrDefault().Zip = parentAddress.HomeAddressZip;
                    }
                }

                parentInfoListModel.TenantId = getAllParentInfoListForView.TenantId;
                //parentInfoListModel.parentInfoForView = parentInfo;
                parentInfoListModel.parentInfo = ParentInfoList.ToList();
                parentInfoListModel._tenantName = getAllParentInfoListForView._tenantName;
                parentInfoListModel._token = getAllParentInfoListForView._token;
                parentInfoListModel._failure = false;
            }
            catch (Exception es)
            {
                parentInfoListModel._message = es.Message;
                parentInfoListModel._failure = true;
                parentInfoListModel._tenantName = getAllParentInfoListForView._tenantName;
                parentInfoListModel._token = getAllParentInfoListForView._token;
            }
            return parentInfoListModel;
        }
        /// <summary>
        /// View Parent Info By Id 
        /// </summary>
        /// <param name="parentInfoAddViewModel"></param>
        /// <returns></returns>
        public ParentInfoAddViewModel ViewParentInfo(ParentInfoAddViewModel parentInfoAddViewModel)
        {
            ParentInfoAddViewModel parentInfoViewModel = new ParentInfoAddViewModel();
            try
            {
                var parentInfo = this.context?.ParentInfo.Include(x=>x.ParentAddress).FirstOrDefault( x =>x.TenantId== parentInfoAddViewModel.parentInfo.TenantId && x.SchoolId== parentInfoAddViewModel.parentInfo.SchoolId && x.ParentId== parentInfoAddViewModel.parentInfo.ParentId);
                if (parentInfo!= null)
                {
                    if (parentInfo.ParentAddress.FirstOrDefault().StudentAddressSame == true)
                    {
                        var parentAddress = this.context?.StudentMaster.FirstOrDefault(x => x.StudentId == parentInfo.ParentAddress.FirstOrDefault().StudentId && x.SchoolId == parentInfo.SchoolId);
                        parentInfo.ParentAddress.FirstOrDefault().AddressLineOne = (parentAddress.HomeAddressLineOne!=null)?parentAddress.HomeAddressLineOne.TrimEnd(','):null;
                        parentInfo.ParentAddress.FirstOrDefault().AddressLineTwo = (parentAddress.HomeAddressLineOne != null) ? parentAddress.HomeAddressLineTwo.TrimEnd(','):null;
                        parentInfo.ParentAddress.FirstOrDefault().Country = (parentAddress.HomeAddressCountry != null) ? parentAddress.HomeAddressCountry.TrimEnd(','):null;
                        parentInfo.ParentAddress.FirstOrDefault().State = (parentAddress.HomeAddressState != null) ? parentAddress.HomeAddressState.TrimEnd(','):null;
                        parentInfo.ParentAddress.FirstOrDefault().City = (parentAddress.HomeAddressCity != null) ? parentAddress.HomeAddressCity.TrimEnd(','):null;
                        parentInfo.ParentAddress.FirstOrDefault().Zip = (parentAddress.HomeAddressZip != null) ? parentAddress.HomeAddressZip.TrimEnd(','):null;
                    }
                    else
                    {
                        parentInfo.ParentAddress.FirstOrDefault().AddressLineOne = (parentInfo.ParentAddress.FirstOrDefault().AddressLineOne != null) ? parentInfo.ParentAddress.FirstOrDefault().AddressLineOne.TrimEnd(','):null;
                        parentInfo.ParentAddress.FirstOrDefault().AddressLineTwo = (parentInfo.ParentAddress.FirstOrDefault().AddressLineTwo != null) ? parentInfo.ParentAddress.FirstOrDefault().AddressLineTwo.TrimEnd(','):null;
                        parentInfo.ParentAddress.FirstOrDefault().Country = (parentInfo.ParentAddress.FirstOrDefault().Country!=null)?parentInfo.ParentAddress.FirstOrDefault().Country.TrimEnd(','):null;
                        parentInfo.ParentAddress.FirstOrDefault().State = (parentInfo.ParentAddress.FirstOrDefault().State != null) ? parentInfo.ParentAddress.FirstOrDefault().State.TrimEnd(','):null;
                        parentInfo.ParentAddress.FirstOrDefault().City = (parentInfo.ParentAddress.FirstOrDefault().City != null) ? parentInfo.ParentAddress.FirstOrDefault().City.TrimEnd(','):null;
                        parentInfo.ParentAddress.FirstOrDefault().Zip = (parentInfo.ParentAddress.FirstOrDefault().Zip != null) ? parentInfo.ParentAddress.FirstOrDefault().Zip.TrimEnd(','):null;
                    }
                    parentInfoViewModel.parentInfo = parentInfo;
                    var AssociationshipData = this.context?.ParentAssociationship.Where(x => x.TenantId == parentInfo.TenantId && x.ParentId == parentInfo.ParentId && x.Associationship == true).ToList();
                    
                    if(AssociationshipData.Count() >0)
                    {
                        foreach (var studentAssociateWithParent in AssociationshipData)
                        {
                            var student = this.context?.StudentMaster.Include(s=>s.StudentEnrollment).Include(s=>s.SchoolMaster).Where(x => x.StudentId == studentAssociateWithParent.StudentId && x.SchoolId == studentAssociateWithParent.SchoolId && x.TenantId==studentAssociateWithParent.TenantId).ToList();

                            //var student = this.context?.StudentMaster.FirstOrDefault(x => x.StudentId == studentAssociateWithParent.StudentId && x.SchoolId == studentAssociateWithParent.SchoolId);
                            var studentForView = new GetStudentForView()
                            {
                                TenantId = student.FirstOrDefault().TenantId,                                
                                SchoolId = student.FirstOrDefault().SchoolId,
                                StudentId = student.FirstOrDefault().StudentId,
                                StudentInternalId = (student.FirstOrDefault().StudentInternalId!=null)?student.FirstOrDefault().StudentInternalId:null,
                                FirstGivenName = (student.FirstOrDefault().FirstGivenName!=null)?student.FirstOrDefault().FirstGivenName:null,
                                MiddleName = (student.FirstOrDefault().MiddleName != null) ? student.FirstOrDefault().MiddleName:null,
                                LastFamilyName = (student.FirstOrDefault().LastFamilyName != null) ? student.FirstOrDefault().LastFamilyName:null,
                                Dob = (student.FirstOrDefault().Dob != null) ? student.FirstOrDefault().Dob:null,
                                Gender = (student.FirstOrDefault().Gender != null) ? student.FirstOrDefault().Gender:null,
                                Address = ToFullAddress((student.FirstOrDefault().HomeAddressLineOne!=null)?student.FirstOrDefault().HomeAddressLineOne.TrimEnd(','):null, (student.FirstOrDefault().HomeAddressLineTwo != null) ? student.FirstOrDefault().HomeAddressLineTwo.TrimEnd(','):null, (student.FirstOrDefault().HomeAddressCity != null) ? student.FirstOrDefault().HomeAddressCity.TrimEnd(','):null, (student.FirstOrDefault().HomeAddressState != null) ? student.FirstOrDefault().HomeAddressState.TrimEnd(','):null,
                                (student.FirstOrDefault().HomeAddressCountry != null) ? student.FirstOrDefault().HomeAddressCountry.TrimEnd(','):null, (student.FirstOrDefault().HomeAddressZip != null) ? student.FirstOrDefault().HomeAddressZip.TrimEnd(','):null),
                                SchoolName = (student.FirstOrDefault().SchoolMaster.SchoolName!=null)?student.FirstOrDefault().SchoolMaster.SchoolName:null,
                                GradeLevelTitle = student.FirstOrDefault().StudentEnrollment.OrderByDescending(x => x.EnrollmentDate).FirstOrDefault().GradeLevelTitle,
                                IsCustodian = studentAssociateWithParent.IsCustodian,
                                Relationship = studentAssociateWithParent.Relationship,
                                StudentPhoto = (student.FirstOrDefault().StudentPhoto != null) ? student.FirstOrDefault().StudentPhoto:null
                            };
                            //var studentForView = new GetStudentForView()
                            //{
                            //    TenantId = student.TenantId,
                            //    SchoolId = student.SchoolId,
                            //    StudentId = student.StudentId,
                            //    StudentInternalId = student.StudentInternalId,
                            //    FirstGivenName = student.FirstGivenName,
                            //    MiddleName = student.MiddleName,
                            //    LastFamilyName = student.LastFamilyName,
                            //    Dob = student.Dob,
                            //    Gender = student.Gender,
                            //    Address = ToFullAddress(student.HomeAddressLineOne, student.HomeAddressLineTwo, student.HomeAddressCity, student.HomeAddressState,
                            //    student.HomeAddressCountry, student.HomeAddressZip),
                            //    SchoolName = this.context?.SchoolMaster.Where(x => x.SchoolId == student.SchoolId)?.Select(s => s.SchoolName).FirstOrDefault(),
                            //    GradeLevelTitle = this.context?.StudentEnrollment.Where(x => x.TenantId == student.TenantId && x.SchoolId == student.SchoolId && x.StudentId == student.StudentId).OrderByDescending(x => x.EnrollmentDate).LastOrDefault().GradeLevelTitle,
                            //    IsCustodian = studentAssociateWithParent.IsCustodian,
                            //    Relationship = studentAssociateWithParent.Relationship,
                            //    StudentPhoto = student.StudentPhoto
                            //};
                            parentInfoViewModel.getStudentForView.Add(studentForView);

                        }
                    }
                    parentInfoViewModel._tenantName = parentInfoAddViewModel._tenantName;
                    parentInfoViewModel._token = parentInfoAddViewModel._token;
                    parentInfoViewModel._failure = false;
                }
                else
                {
                    parentInfoViewModel._failure = true;
                    parentInfoViewModel._message = NORECORDFOUND;
                    return parentInfoViewModel;
                }
            }
            catch (Exception es)
            {
                parentInfoViewModel._failure = true;
                parentInfoViewModel._message = es.Message;
            }
            return parentInfoViewModel;
        }
        /// <summary>
        /// Add Parent Info
        /// </summary>
        /// <param name="parentInfoAddViewModel"></param>
        /// <returns></returns>
        public ParentInfoAddViewModel AddParentInfo(ParentInfoAddViewModel parentInfoAddViewModel)
        {
            try
            {
                int? ParentId = Utility.GetMaxPK(this.context, new Func<ParentInfo, int>(x => x.ParentId));
                parentInfoAddViewModel.parentInfo.ParentId = (int)ParentId;
                parentInfoAddViewModel.parentInfo.LastUpdated = DateTime.UtcNow;
                parentInfoAddViewModel.parentInfo.LoginEmail = parentInfoAddViewModel.parentInfo.LoginEmail;
                this.context?.ParentInfo.Add(parentInfoAddViewModel.parentInfo);
                this.context?.SaveChanges();
                parentInfoAddViewModel._failure = false;
                parentInfoAddViewModel._message = "Parent Added Successfully";
            }
            catch (Exception es)
            {
                parentInfoAddViewModel._failure = true;
                parentInfoAddViewModel._message = es.Message;
            }
            return parentInfoAddViewModel;
        }

        /// <summary>
        /// Remove Associated Parent
        /// </summary>
        /// <param name="parentInfoDeleteViewModel"></param>
        /// <returns></returns>
        public ParentInfoDeleteViewModel RemoveAssociatedParent(ParentInfoDeleteViewModel parentInfoDeleteViewModel)
        {
            try
            {
                var ParentInfo = this.context?.ParentAssociationship.FirstOrDefault(x => x.TenantId == parentInfoDeleteViewModel.parentInfo.TenantId && x.SchoolId == parentInfoDeleteViewModel.parentInfo.SchoolId && x.ParentId == parentInfoDeleteViewModel.parentInfo.ParentId && x.StudentId==parentInfoDeleteViewModel.StudentId);
                if (ParentInfo != null)
                {
                    ParentInfo.Associationship = false;
                    this.context?.SaveChanges();
                }
                parentInfoDeleteViewModel._failure = false;
                parentInfoDeleteViewModel._message = "Parent Associationship Removed Successfully";
            }
            catch (Exception es)
            {
                parentInfoDeleteViewModel._failure = true;
                parentInfoDeleteViewModel._message = es.Message;
            }
            return parentInfoDeleteViewModel;
        }
        /// <summary>
        /// Add Update Parent Photo
        /// </summary>
        /// <param name="parentInfoAddViewModel"></param>
        /// <returns></returns>
        public ParentInfoAddViewModel AddUpdateParentPhoto(ParentInfoAddViewModel parentInfoAddViewModel)
        {
            try
            {
                var parentPhotoUpdate = this.context?.ParentInfo.FirstOrDefault(x => x.TenantId == parentInfoAddViewModel.parentInfo.TenantId && x.SchoolId == parentInfoAddViewModel.parentInfo.SchoolId && x.ParentId == parentInfoAddViewModel.parentInfo.ParentId);

                if (parentPhotoUpdate != null)
                {
                    parentPhotoUpdate.ParentPhoto = parentInfoAddViewModel.parentInfo.ParentPhoto;
                    this.context?.SaveChanges();
                    parentInfoAddViewModel._message = "Parent Photo Updated Successfully";
                }
                else
                {
                    parentInfoAddViewModel._failure = true;
                    parentInfoAddViewModel._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {
                parentInfoAddViewModel._failure = true;
                parentInfoAddViewModel._message = es.Message;
            }
            return parentInfoAddViewModel;
        }
    }
}
