using System;
using System.Collections.Generic;

namespace opensis.data.Models
{
    public partial class UserMaster
    {
        public UserMaster()
        {
            SearchFilter = new HashSet<SearchFilter>();
        }
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
        public int LangId { get; set; }
        public int MembershipId { get; set; }
        public bool? IsActive { get; set; }
        public string Description { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Language Lang { get; set; }
        public virtual Membership Membership { get; set; }
        public virtual UserSecretQuestions UserSecretQuestions { get; set; }
        public virtual ICollection<SearchFilter> SearchFilter { get; set; }
    }
}
