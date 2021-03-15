using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using opensis.core.School.Interfaces;
using opensis.data.ViewModels.Membership;

namespace opensisAPI.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("{tenant}/Membership")]
    [ApiController]
    public class MembershipController : ControllerBase
    {
        private IMembershipService _membershipService;


        public MembershipController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }



        [HttpPost("getAllMembers")]
        public ActionResult<GetAllMembersList> GetAllMembers(GetAllMembersList allMembersList)
        {
            GetAllMembersList getAllMembers = new GetAllMembersList();
            try
            {
                if (allMembersList.SchoolId > 0)
                {
                    getAllMembers = _membershipService.GetAllMembersForNotice(allMembersList);
                }
                else
                {
                    getAllMembers._token = allMembersList._token;
                    getAllMembers._tenantName = allMembersList._tenantName;
                    getAllMembers._failure = true;
                    getAllMembers._message = "Please enter valid scholl id";
                }
            }
            catch (Exception es)
            {
                getAllMembers._failure = true;
                getAllMembers._message = es.Message;
            }
            return getAllMembers;
        }

        [HttpPost("addMembership")]
        public ActionResult<MembershipAddViewModel> AddMembership(MembershipAddViewModel membershipAddViewModel)
        {
            MembershipAddViewModel MembershipAdd = new MembershipAddViewModel();
            try
            {
                MembershipAdd = _membershipService.AddMembership(membershipAddViewModel);
            }
            catch (Exception es)
            {
                MembershipAdd._failure = true;
                MembershipAdd._message = es.Message;
            }
            return MembershipAdd;
        }

        [HttpPut("updateMembership")]
        public ActionResult<MembershipAddViewModel> UpdateMembership(MembershipAddViewModel membershipAddViewModel)
        {
            MembershipAddViewModel membershipUpdate = new MembershipAddViewModel();
            try
            {
                membershipUpdate = _membershipService.UpdateMembership(membershipAddViewModel);
            }
            catch (Exception es)
            {
                membershipUpdate._failure = true;
                membershipUpdate._message = es.Message;
            }
            return membershipUpdate;
        }

        [HttpPost("deleteMembership")]
        public ActionResult<MembershipAddViewModel> DeleteMembership(MembershipAddViewModel membershipAddViewModel)
        {
            MembershipAddViewModel membershipDelete = new MembershipAddViewModel();
            try
            {
                membershipDelete = _membershipService.DeleteMembership(membershipAddViewModel);
            }
            catch (Exception es)
            {
                membershipDelete._failure = true;
                membershipDelete._message = es.Message;
            }
            return membershipDelete;
        }
    }
}
