using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.Models
{
    public partial class StaffMaster
    {
        public StaffMaster()
        {
            StaffCertificateInfo = new HashSet<StaffCertificateInfo>();
            StaffSchoolInfo = new HashSet<StaffSchoolInfo>();
            StaffCoursesectionSchedule = new HashSet<StaffCoursesectionSchedule>();
        }
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int StaffId { get; set; }
        public Guid StaffGuid { get; set; }
        public byte[] StaffPhoto { get; set; }
        public string Salutation { get; set; }
        public string Suffix { get; set; }
        public string FirstGivenName { get; set; }
        public string MiddleName { get; set; }
        public string LastFamilyName { get; set; }
        public string StaffInternalId { get; set; }
        public string AlternateId { get; set; }
        public string DistrictId { get; set; }
        public string StateId { get; set; }
        public string PreferredName { get; set; }
        public string PreviousName { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string OtherGovtIssuedNumber { get; set; }
        public string Gender { get; set; }
        public string Race { get; set; }
        public string Ethnicity { get; set; }
        public DateTime? Dob { get; set; }
        public string MaritalStatus { get; set; }
        public int? CountryOfBirth { get; set; }
        public int? Nationality { get; set; }
        public int? FirstLanguage { get; set; }
        public int? SecondLanguage { get; set; }
        public int? ThirdLanguage { get; set; }
        public bool? PhysicalDisability { get; set; }
        public string DisabilityDescription { get; set; }
        public bool? PortalAccess { get; set; }
        public string LoginEmailAddress { get; set; }
        public string Profile { get; set; }
        public string JobTitle { get; set; }
        public DateTime? JoiningDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? HomeroomTeacher { get; set; }
        public string PrimaryGradeLevelTaught { get; set; }
        public string PrimarySubjectTaught { get; set; }
        public string OtherGradeLevelTaught { get; set; }
        public string OtherSubjectTaught { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public string OfficePhone { get; set; }
        public string PersonalEmail { get; set; }
        public string SchoolEmail { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Youtube { get; set; }
        public string Linkedin { get; set; }
        public string HomeAddressLineOne { get; set; }
        public string HomeAddressLineTwo { get; set; }
        public string HomeAddressCity { get; set; }
        public string HomeAddressState { get; set; }
        public string HomeAddressCountry { get; set; }
        public string HomeAddressZip { get; set; }
        public bool? MailingAddressSameToHome { get; set; }
        public string MailingAddressLineOne { get; set; }
        public string MailingAddressLineTwo { get; set; }
        public string MailingAddressCity { get; set; }
        public string MailingAddressState { get; set; }
        public string MailingAddressCountry { get; set; }
        public string MailingAddressZip { get; set; }
        public string BusNo { get; set; }
        public bool? BusPickup { get; set; }
        public bool? BusDropoff { get; set; }
        public string EmergencyFirstName { get; set; }
        public string EmergencyLastName { get; set; }
        public string RelationshipToStaff { get; set; }
        public string EmergencyHomePhone { get; set; }
        public string EmergencyWorkPhone { get; set; }
        public string EmergencyMobilePhone { get; set; }
        public string EmergencyEmail { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }

        public virtual Language FirstLanguageNavigation { get; set; }
        public virtual SchoolMaster SchoolMaster { get; set; }
        public virtual Language SecondLanguageNavigation { get; set; }
        public virtual Language ThirdLanguageNavigation { get; set; }
        public virtual ICollection<StaffCertificateInfo> StaffCertificateInfo { get; set; }
        public virtual ICollection<StaffSchoolInfo> StaffSchoolInfo { get; set; }
        public virtual ICollection<StaffCoursesectionSchedule> StaffCoursesectionSchedule { get; set; }
    }
}
