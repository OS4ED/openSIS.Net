using System;
using System.Collections.Generic;

namespace opensis.data.Models
{
    public partial class SearchFilter
    {
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public string Module { get; set; }
        public int FilterId { get; set; }
        public string FilterName { get; set; }
        public string Emailaddress { get; set; }
        public string JsonList { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? DateModifed { get; set; }

        public virtual SchoolMaster SchoolMaster { get; set; }
        public virtual UserMaster UserMaster { get; set; }
    }
}
