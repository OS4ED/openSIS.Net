using System;
using System.Collections.Generic;

namespace opensis.data.Models
{
    public partial class PermissionGroup
    {
        public PermissionGroup()
        {
            PermissionCategory = new HashSet<PermissionCategory>();
            RolePermission = new HashSet<RolePermission>();
        }

        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int PermissionGroupId { get; set; }
        public string PermissionGroupName { get; set; }
        public string ShortName { get; set; }
        public bool? IsActive { get; set; }
        public bool IsSystem { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string IconType { get; set; }
        public int? SortOrder { get; set; }
        public string Type { get; set; }
        public string Path { get; set; }
        public string BadgeType { get; set; }
        public string BadgeValue { get; set; }
        public bool? Active { get; set; }
        public virtual SchoolMaster SchoolMaster { get; set; }
        public virtual ICollection<PermissionCategory> PermissionCategory { get; set; }
        public virtual ICollection<RolePermission> RolePermission { get; set; }
    }
}
