using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.RoleBasedAccess
{
    public class RolePermissionListViewModel : CommonFields
    {
        public List<RolePermissionViewModel> PermissionList { get; set; }
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int? MembershipId { get; set; }
    }
}
