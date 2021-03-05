using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.RoleBasedAccess
{
    public class PermissionGroupListViewModel : CommonFields
    {
        public List<PermissionGroup> permissionGroupList { get; set; }
        public Guid? TenantId { get; set; }
        public int? SchoolId { get; set; }
        public string CreatedBy { get; set; }
    }
}
