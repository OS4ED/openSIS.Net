using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.School
{
    public class CopySchoolViewModel : CommonFields
    {

        public SchoolMaster schoolMaster { get; set; }
        public Guid TenantId { get; set; }
        public int? FromSchoolId { get; set; }
        public bool? Periods { get; set; }           
        public bool? MarkingPeriods { get; set; }               
        public bool? Calendar { get; set; }
        public bool? Sections { get; set; }
        public bool? Rooms { get; set; }
        public bool? GradeLevels { get; set; }
       // public bool? Hierarchy { get; set; }
       // public bool? Preference { get; set; }
        public bool? SchoolFields { get; set; }
        public bool? StudentFields { get; set; }
        public bool? EnrollmentCodes { get; set; }
        public bool? StaffFields { get; set; }
        public bool? Subjets { get; set; }
        public bool? Programs { get; set; }
        public bool? Course { get; set; }      
        public bool? AttendanceCode { get; set; }
        public bool? ReportCardGrades { get; set; }
        public bool? ReportCardComments { get; set; }
        public bool? StandardGrades { get; set; }
        public bool? HonorRollSetup { get; set; }
        public bool? EffortGrades { get; set; }

        public bool? ProfilePermission { get; set; }

        public bool? SchoolLevel { get; set; }
        public bool? SchoolClassification { get; set; }
        public bool? Countries { get; set; }
        public bool? FemaleToiletType { get; set; }
        public bool? FemaleToiletAccessibility { get; set; }
        public bool? MaleToiletType { get; set; }
        public bool? MaleToiletAccessibility { get; set; }
        public bool? CommonToiletType { get; set; }
        public bool? CommonToiletAccessibility { get; set; }
        public bool? Race { get; set; }
        public bool? Ethnicity { get; set; }
        public bool? Language { get; set; }

    }
}
