using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.RoleBasedAccess
{
    public class RolePermissionViewModel : CommonFields
    {
        public PermissionGroup permissionGroup { get; set; }
    }
}
