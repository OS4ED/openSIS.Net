using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.CommonModel
{
    public class SearchFilterListViewModel : CommonFields
    {
        public List<SearchFilter> searchFilterList { get; set; }
        public Guid? TenantId { get; set; }
        public int? SchoolId { get; set; }
        public string Module { get; set; }
    }
}
