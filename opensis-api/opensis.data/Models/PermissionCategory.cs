using System;
using System.Collections.Generic;

namespace opensis.data.Models
{
    public partial class PermissionCategory
    {
        public PermissionCategory()
        {
            RolePermission = new HashSet<RolePermission>();
        }

        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int PermissionCategoryId { get; set; }
        public int PermissionGroupId { get; set; }
        public string PermissionCategoryName { get; set; }
        public string ShortCode { get; set; }
        public string Path { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public bool? EnableView { get; set; }
        public bool? EnableAdd { get; set; }
        public bool? EnableEdit { get; set; }
        public bool? EnableDelete { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual PermissionGroup PermissionGroup { get; set; }
        public virtual ICollection<RolePermission> RolePermission { get; set; }
    }
}
