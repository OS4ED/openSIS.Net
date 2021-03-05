using opensis.data.ViewModels.Membership;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.School.Interfaces
{
    public interface IMembershipService
    {
        GetAllMembersList GetAllMembersForNotice(GetAllMembersList allMembersList);
        MembershipAddViewModel AddMembership(MembershipAddViewModel membershipAddViewModel);
        public MembershipAddViewModel UpdateMembership(MembershipAddViewModel membershipAddViewModel);
    }
}
