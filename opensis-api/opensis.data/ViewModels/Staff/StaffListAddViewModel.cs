using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.Staff
{
    public class StaffListAddViewModel : CommonFields
    {
        public StaffListAddViewModel()
        {
            StaffAddViewModelList = new List<StaffAddViewModel>();
        }
        public List<StaffAddViewModel> StaffAddViewModelList { get; set; }
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }

    }
}
