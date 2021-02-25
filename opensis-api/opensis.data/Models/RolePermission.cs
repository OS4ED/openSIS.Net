using System;
using System.Collections.Generic;

namespace opensis.data.Models
{
    public partial class RolePermission
    {

        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int RolePermissionId { get; set; }
        public int? MembershipId { get; set; }
        public int? PermissionCategoryId { get; set; }
        public bool? CanView { get; set; }
        public bool? CanAdd { get; set; }
        public bool? CanEdit { get; set; }
        public bool? CanDelete { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual Membership Membership { get; set; }
        public virtual PermissionCategory PermissionCategory { get; set; }
    }
}
