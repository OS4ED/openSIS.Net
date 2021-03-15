using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.Models
{
   public partial class StudentMaster
    {
        public StudentMaster()
        {
            //ParentAddress = new HashSet<ParentAddress>();
            StudentComments = new HashSet<StudentComments>();
            StudentDocuments = new HashSet<StudentDocuments>();
            StudentEnrollment = new HashSet<StudentEnrollment>();
            StudentCoursesectionSchedule = new HashSet<StudentCoursesectionSchedule>();
        }
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int StudentId { get; set; }
        public Guid StudentGuid { get; set; }
        public string StudentInternalId { get; set; }
        public string AlternateId { get; set; }
        public string DistrictId { get; set; }
        public string StateId { get; set; }
        public string AdmissionNumber { get; set; }
        public string RollNumber { get; set; }
        public string Salutation { get; set; }
        public string FirstGivenName { get; set; }
        public string MiddleName { get; set; }
        public string LastFamilyName { get; set; }
        public string Suffix { get; set; }
        public string PreferredName { get; set; }
        public string PreviousName { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string OtherGovtIssuedNumber { get; set; }
        public byte[] StudentPhoto { get; set; }
        public DateTime? Dob { get; set; }
        public string StudentPortalId { get; set; }
        public string Gender { get; set; }
        public string Race { get; set; }
        public string Ethnicity { get; set; }
        public string MaritalStatus { get; set; }
        public int? CountryOfBirth { get; set; }
        public int? Nationality { get; set; }
        public int? FirstLanguageId { get; set; }
        public int? SecondLanguageId { get; set; }
        public int? ThirdLanguageId { get; set; }
        public int? SectionId { get; set; }
        public DateTime? EstimatedGradDate { get; set; }
        public bool? Eligibility504 { get; set; }
        public bool? EconomicDisadvantage { get; set; }
        public bool? FreeLunchEligibility { get; set; }
        public bool? SpecialEducationIndicator { get; set; }
        public bool? LepIndicator { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public string PersonalEmail { get; set; }
        public string SchoolEmail { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Youtube { get; set; }
        public string Linkedin { get; set; }
        public string HomeAddressLineOne { get; set; }
        public string HomeAddressLineTwo { get; set; }
        public string HomeAddressCountry { get; set; }
        public string HomeAddressCity { get; set; }
        public string HomeAddressState { get; set; }
        public string HomeAddressZip { get; set; }
        public string BusNo { get; set; }
        public bool? SchoolBusPickUp { get; set; }
        public bool? SchoolBusDropOff { get; set; }
        public bool? MailingAddressSameToHome { get; set; }
        public string MailingAddressLineOne { get; set; }
        public string MailingAddressLineTwo { get; set; }
        public string MailingAddressCountry { get; set; }
        public string MailingAddressCity { get; set; }
        public string MailingAddressState { get; set; }
        public string MailingAddressZip { get; set; }
        public string CriticalAlert { get; set; }
        public string AlertDescription { get; set; }
        public string PrimaryCarePhysician { get; set; }
        public string PrimaryCarePhysicianPhone { get; set; }
        public string MedicalFacility { get; set; }
        public string MedicalFacilityPhone { get; set; }
        public string InsuranceCompany { get; set; }
        public string InsuranceCompanyPhone { get; set; }
        public string PolicyNumber { get; set; }
        public string PolicyHolder { get; set; }
        public string Dentist { get; set; }
        public string DentistPhone { get; set; }
        public string Vision { get; set; }
        public string VisionPhone { get; set; }
        public string Associationship { get; set; }
        public string EnrollmentType { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Language FirstLanguage { get; set; }
        public virtual SchoolMaster SchoolMaster { get; set; }
        public virtual Language SecondLanguage { get; set; }
        public virtual Language ThirdLanguage { get; set; }
        public virtual Sections Sections { get; set; }

        //public virtual ICollection<ParentAddress> ParentAddress { get; set; }
        public virtual ICollection<StudentCoursesectionSchedule> StudentCoursesectionSchedule { get; set; }
        public virtual ICollection<StudentComments> StudentComments { get; set; }
        public virtual ICollection<StudentDocuments> StudentDocuments { get; set; }
        public virtual ICollection<StudentEnrollment> StudentEnrollment { get; set; }
    }
}
