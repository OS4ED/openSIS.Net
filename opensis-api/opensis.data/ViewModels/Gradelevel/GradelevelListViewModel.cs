using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace opensis.data.ViewModels.GradeLevel
{
    public class GradelevelListViewModel : CommonFields
    {
        public GradelevelListViewModel()
        {
            TableGradelevelList = new List<GradeLevelView>();
        }
        public List<GradeLevelView> TableGradelevelList { get; set; }
        public Guid? TenantId { get; set; }
        public int? SchoolId { get; set; }
    }
}
