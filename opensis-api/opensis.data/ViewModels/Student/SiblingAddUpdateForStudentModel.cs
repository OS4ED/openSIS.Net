using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.Student
{
    public class SiblingAddUpdateForStudentModel :CommonFields
    {
        public StudentMaster studentMaster { get; set; }
        public int? SchoolId { get; set; }
        public int StudentId { get; set; }
        public Guid? TenantId { get; set; }
    }
}
