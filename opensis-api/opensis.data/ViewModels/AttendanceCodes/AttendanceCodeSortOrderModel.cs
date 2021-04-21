using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.AttendanceCodes
{
    public class AttendanceCodeSortOrderModel : CommonFields
    {

        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int AttendanceCategoryId { get; set; }
        public int? PreviousSortOrder { get; set; }
        public int? CurrentSortOrder { get; set; }
    }
}
