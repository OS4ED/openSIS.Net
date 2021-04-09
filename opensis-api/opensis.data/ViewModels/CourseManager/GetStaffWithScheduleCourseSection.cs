using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.CourseManager
{
    public class GetStaffWithScheduleCourseSection
    {
        public Guid? TenantId { get; set; }
        public int? SchoolId { get; set; }
        public int? StaffId { get; set; }
        public Guid? StaffGuid { get; set; }
        public byte[] StaffPhoto { get; set; }
        public string StaffName { get; set; }
        public string StaffInternalId { get; set; }
        public string LoginEmailAddress { get; set; }
        public int? FirstLanguage { get; set; }
        public int? SecondLanguage { get; set; }
        public int? ThirdLanguage { get; set; }
        public string JobTitle { get; set; }
        public bool? HomeroomTeacher { get; set; }
        public string PrimaryGradeLevelTaught { get; set; }
        public string PrimarySubjectTaught { get; set; }
        public string OtherGradeLevelTaught { get; set; }
        public string OtherSubjectTaught { get; set; }
        public string PersonalEmail { get; set; }
        public string SchoolEmail { get; set; }
        public string CourseSectionName { get; set; }
        public int? CourseId { get; set; }
        public int? CourseSectionId { get; set; }
        public int? YrMarkingPeriodId { get; set; }
        public int? SmstrMarkingPeriodId { get; set; }
        public int? QtrMarkingPeriodId { get; set; }
        public DateTime? DurationStartDate { get; set; }
        public DateTime? DurationEndDate { get; set; }
    }
}
