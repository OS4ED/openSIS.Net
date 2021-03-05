using opensis.data.Models;
using opensis.data.ViewModels.Student;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.ParentInfos
{
     public class GetParentInfoForView
    {
        public GetParentInfoForView()
        {
            getStudentForView = new List<GetStudentForView>();
        }
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
        public int ParentId { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string Salutation { get; set; }
        public string Suffix { get; set; }
        public string Mobile { get; set; }
        public string WorkPhone { get; set; }
        public string HomePhone { get; set; }        
        public string PersonalEmail { get; set; }
        public string WorkEmail { get; set; }
        public string LoginEmail { get; set; }
        public string UserProfile { get; set; }
        public bool IsPortalUser { get; set; }
        public bool IsCustodian { get; set; }
        public string FullAddress { get; set; }
        public List<GetStudentForView> getStudentForView { get; set; }
        public string[] students { get; set; }
        public string AddressLineOne { get; set; }
        public string AddressLineTwo { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }
}
