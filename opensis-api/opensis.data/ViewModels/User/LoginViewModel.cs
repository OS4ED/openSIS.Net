using opensis.data.ViewModels.RoleBasedAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace opensis.data.ViewModels.User
{
    public class LoginViewModel : CommonFields
    {
        [Required]
        public string Email { get; set; }
       
        public string Password { get; set; }

        public Guid? TenantId { get; set; }
        public int? SchoolId { get; set; }
        public int? UserId { get; set; }
        public string Name { get; set; }
        public string MembershipName { get; set; }
        public int? MembershipId { get; set; }
        public List<RolePermissionViewModel> PermissionList { get; set; }
    }
}
