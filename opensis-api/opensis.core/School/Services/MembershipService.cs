using opensis.core.helper;
using opensis.core.School.Interfaces;
using opensis.data.Interface;
using opensis.data.ViewModels.Membership;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.School.Services
{
    public class MembershipService : IMembershipService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";
        public IMembershipRepository membershipRepository;
        public MembershipService(IMembershipRepository membershipRepository)
        {
            this.membershipRepository = membershipRepository;
        }
        public GetAllMembersList GetAllMembersForNotice(GetAllMembersList allMembersList)
        {
            GetAllMembersList getAllMembers = new GetAllMembersList();

            if (TokenManager.CheckToken(allMembersList._tenantName + allMembersList._userName, allMembersList._token))
            {
                getAllMembers = this.membershipRepository.GetAllMemberList(allMembersList);
                return getAllMembers;
            }
            else
            {
                getAllMembers._failure = true;
                getAllMembers._message = TOKENINVALID;
                return getAllMembers;
            }
        }

        /// <summary>
        /// Add Member
        /// </summary>
        /// <param name="membershipAddViewModel"></param>
        /// <returns></returns>
        public MembershipAddViewModel AddMembership(MembershipAddViewModel membershipAddViewModel)
        {
            MembershipAddViewModel MembershipAddModel = new MembershipAddViewModel();
            try
            {
                if (TokenManager.CheckToken(membershipAddViewModel._tenantName + membershipAddViewModel._userName, membershipAddViewModel._token))
                {
                    MembershipAddModel = this.membershipRepository.AddMembership(membershipAddViewModel);
                }
                else
                {
                    MembershipAddModel._failure = true;
                    MembershipAddModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                MembershipAddModel._failure = true;
                MembershipAddModel._message = es.Message;
            }  
            return MembershipAddModel;
        }

        /// <summary>
        /// Update Member
        /// </summary>
        /// <param name="membershipAddViewModel"></param>
        /// <returns></returns>
        public MembershipAddViewModel UpdateMembership(MembershipAddViewModel membershipAddViewModel)
        {
            MembershipAddViewModel MembershipUpdateModel = new MembershipAddViewModel();
            try
            { 
                if (TokenManager.CheckToken(membershipAddViewModel._tenantName + membershipAddViewModel._userName, membershipAddViewModel._token))
                {
                    MembershipUpdateModel = this.membershipRepository.UpdateMembership(membershipAddViewModel);                    
                }
                else
                {
                    MembershipUpdateModel._failure = true;
                    MembershipUpdateModel._message = TOKENINVALID;                    
                }
            }
            catch (Exception es)
            {
                MembershipUpdateModel._failure = true;
                MembershipUpdateModel._message = es.Message;
            }
            return MembershipUpdateModel;
        }

        /// <summary>
        /// Delete Membership
        /// </summary>
        /// <param name="membershipAddViewModel"></param>
        /// <returns></returns>
        public MembershipAddViewModel DeleteMembership(MembershipAddViewModel membershipAddViewModel)
        {
            MembershipAddViewModel MembershipDeleteModel = new MembershipAddViewModel();
            try
            {
                if (TokenManager.CheckToken(membershipAddViewModel._tenantName + membershipAddViewModel._userName, membershipAddViewModel._token))
                {
                    MembershipDeleteModel = this.membershipRepository.DeleteMembership(membershipAddViewModel);
                }
                else
                {
                    MembershipDeleteModel._failure = true;
                    MembershipDeleteModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                MembershipDeleteModel._failure = true;
                MembershipDeleteModel._message = es.Message;
            }
            return MembershipDeleteModel;
        }
    }
}
