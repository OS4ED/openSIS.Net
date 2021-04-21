using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.Models
{
   public partial class CustomFields
    {
        public CustomFields()
        {
            CustomFieldsValue = new HashSet<CustomFieldsValue>();
        }
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int CategoryId { get; set; }
        public int FieldId { get; set; }
        public string Module { get; set; }
        public string Type { get; set; }
        public bool? Search { get; set; }
        public string FieldName { get; set; }
        public string Title { get; set; }
        public int? SortOrder { get; set; }
        public string SelectOptions { get; set; }
        public bool? SystemField { get; set; }
        public bool? Required { get; set; }
        public string DefaultSelection { get; set; }
        public bool? Hide { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string UpdatedBy { get; set; }
        public virtual FieldsCategory FieldsCategory { get; set; }
        public virtual SchoolMaster SchoolMaster { get; set; }
        public virtual ICollection<CustomFieldsValue> CustomFieldsValue { get; set; }
    }
}
