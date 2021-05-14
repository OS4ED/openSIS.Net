using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.ParentInfos
{
    public class ParentInfoDeleteViewModel : CommonFields
    {
        public ParentInfo parentInfo { get; set; }
        public int StudentId { get; set; }
        public int StudentSchoolId { get; set; }
    }

}
