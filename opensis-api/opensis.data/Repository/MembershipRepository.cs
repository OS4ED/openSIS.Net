using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace opensis.data.Repository
{
    public class MembershipRepository : IMembershipRepository
    {
        private CRMContext context;
        private static readonly string NORECORDFOUND = "No Record Found";
        public MembershipRepository(IDbContextFactory dbContextFactory)
        {
            this.context = dbContextFactory.Create();
        }

        /// <summary>
        /// Get All Members
        /// </summary>
        /// <returns></returns>
        public GetAllMembersList GetAllMemberList(GetAllMembersList membersList)
        {
            GetAllMembersList getAllMembersList = new GetAllMembersList();
            try
            {
                var membershipRepository = this.context?.Membership.Where(x => x.TenantId == membersList.TenantId && x.SchoolId == membersList.SchoolId).ToList();

                getAllMembersList.GetAllMemberList = membershipRepository;
                getAllMembersList.TenantId = membersList.TenantId;
                getAllMembersList.SchoolId = membersList.SchoolId;
                getAllMembersList._tenantName = membersList._tenantName;
                getAllMembersList._token = membersList._token;

                return getAllMembersList;
            }
            catch (Exception ex)
            {
                getAllMembersList = null;
                getAllMembersList._failure = true;
                getAllMembersList._message = NORECORDFOUND;
                return getAllMembersList;
            }
        }

        /// <summary>
        /// Add Member
        /// </summary>
        /// <param name="membershipAddViewModel"></param>
        /// <returns></returns>
        public MembershipAddViewModel AddMembership(MembershipAddViewModel membershipAddViewModel)
        {
            try
            {
                var checkProfile = this.context?.Membership.Where(x => x.SchoolId == membershipAddViewModel.membership.SchoolId && x.TenantId == membershipAddViewModel.membership.TenantId && x.Profile.ToLower() == membershipAddViewModel.membership.Profile.ToLower()).FirstOrDefault();

                if (checkProfile != null)
                {
                    membershipAddViewModel._failure = true;
                    membershipAddViewModel._message = "Profile Already Exists";
                }
                else
                {
                    int? MembershipId = 1;

                    var MembershipData = this.context?.Membership.Where(x => x.SchoolId == membershipAddViewModel.membership.SchoolId && x.TenantId == membershipAddViewModel.membership.TenantId).OrderByDescending(x => x.MembershipId).FirstOrDefault();

                    if (MembershipData != null)
                    {
                        MembershipId = MembershipData.MembershipId + 1;
                    }
                    membershipAddViewModel.membership.MembershipId = (int)MembershipId;
                    membershipAddViewModel.membership.CreatedOn = DateTime.UtcNow;                    
                    this.context?.Membership.Add(membershipAddViewModel.membership);
                    this.context?.SaveChanges();
                    membershipAddViewModel._failure = false;
                    membershipAddViewModel._message = "Membership Added Successfully";
                }
            }
            catch (Exception es)
            {
                membershipAddViewModel._message = es.Message;
                membershipAddViewModel._failure = true;
            }
            return membershipAddViewModel;
        }

        /// <summary>
        /// Update Member
        /// </summary>
        /// <param name="membershipAddViewModel"></param>
        /// <returns></returns>
        public MembershipAddViewModel UpdateMembership(MembershipAddViewModel membershipAddViewModel)
        {
            try
            {
                var memeberShipData = this.context?.Membership.FirstOrDefault(x => x.TenantId == membershipAddViewModel.membership.TenantId && x.SchoolId == membershipAddViewModel.membership.SchoolId && x.MembershipId == membershipAddViewModel.membership.MembershipId);
                
                if (memeberShipData != null)
                {
                    var checkProfile = this.context?.Membership.Where(x => x.SchoolId == membershipAddViewModel.membership.SchoolId && x.TenantId == membershipAddViewModel.membership.TenantId && x.MembershipId != membershipAddViewModel.membership.MembershipId && x.Profile.ToLower() == membershipAddViewModel.membership.Profile.ToLower()).FirstOrDefault();

                    if (checkProfile != null)
                    {
                        membershipAddViewModel._failure = true;
                        membershipAddViewModel._message = "Profile Title Already Exists";
                    }
                    else
                    {
                        membershipAddViewModel.membership.CreatedBy = memeberShipData.CreatedBy;
                        membershipAddViewModel.membership.CreatedOn = memeberShipData.CreatedOn;
                        membershipAddViewModel.membership.UpdatedOn = DateTime.UtcNow;
                        this.context.Entry(memeberShipData).CurrentValues.SetValues(membershipAddViewModel.membership);
                        this.context?.SaveChanges();
                        membershipAddViewModel._failure = false;
                        membershipAddViewModel._message = "Membership Updated Successfully";
                    }
                }
                else
                {
                    membershipAddViewModel.membership = null;
                    membershipAddViewModel._failure = true;
                    membershipAddViewModel._message = NORECORDFOUND;
                }
            }
            catch (Exception ex)
            {
                membershipAddViewModel._failure = true;
                membershipAddViewModel._message = ex.Message;
            }
            return membershipAddViewModel;
        }
    }
}
