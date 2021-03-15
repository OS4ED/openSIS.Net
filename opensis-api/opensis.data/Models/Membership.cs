using System;
using System.Collections.Generic;

namespace opensis.data.Models
{
    public partial class Membership
    {
        public Membership()
        {
            RolePermission = new HashSet<RolePermission>();
            UserMaster = new HashSet<UserMaster>();
        }

        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int MembershipId { get; set; }
        public string Profile { get; set; }
        public string ProfileType { get; set; }
        public bool IsActive { get; set; }
        public bool? IsSystem { get; set; }
        public bool? IsSuperadmin { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual SchoolMaster SchoolMaster { get; set; }
        public virtual ICollection<RolePermission> RolePermission { get; set; }
        public virtual ICollection<UserMaster> UserMaster { get; set; }
    }
}
