using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.StudentSchedule
{
    public class ScheduleStudentForView
    {
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int StudentId { get; set; }
        public string FirstGivenName { get; set; }
        public string LastFamilyName { get; set; }
        public string AlternateId { get; set; }
        public string StudentInternalId { get; set; }
        public string GradeLevel { get; set; }
        public string Section { get; set; }
        public string PhoneNumber { get; set; }
        public bool? Action { get; set; }
        public byte[] StudentPhoto { get; set; }
        public DateTime? ScheduleDate { get; set; }
    }
}
