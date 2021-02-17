using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.MarkingPeriods
{
    public class MarkingPeriodListViewModel : CommonFields
    {
        public MarkingPeriodListViewModel()
        {
            getMarkingPeriodView = new List<GetMarkingPeriodView>();
        }
        public Guid? TenantId { get; set; }
        public int? SchoolId { get; set; }        
        public decimal? AcademicYear { get; set; }
        public List<GetMarkingPeriodView> getMarkingPeriodView { get; set; }
    }
}
