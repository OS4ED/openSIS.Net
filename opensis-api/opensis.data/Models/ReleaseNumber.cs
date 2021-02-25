using System;
using System.Collections.Generic;

namespace opensis.data.Models
{
    public partial class ReleaseNumber
    {
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public string ReleaseNumber1 { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
