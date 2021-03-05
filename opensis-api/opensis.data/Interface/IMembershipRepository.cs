using opensis.data.ViewModels.Membership;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.Interface
{
    public interface IMembershipRepository
    {
        GetAllMembersList GetAllMemberList(GetAllMembersList membersList);
        public MembershipAddViewModel AddMembership(MembershipAddViewModel membershipAddViewModel);
        public MembershipAddViewModel UpdateMembership(MembershipAddViewModel membershipAddViewModel);
    }
}
