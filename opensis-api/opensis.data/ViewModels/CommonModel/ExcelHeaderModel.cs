using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.CommonModel
{
    public class ExcelHeaderModel : CommonFields
    {
        public byte[]  FileContentResult { get; set; }
        public string[] CustomfieldTitle { get; set; }
        public Guid? TenantId { get; set; }
        public int? SchoolId { get; set; }
        public string Module { get; set; }
    }
}
