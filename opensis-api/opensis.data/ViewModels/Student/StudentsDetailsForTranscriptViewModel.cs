using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.Student
{
    public class StudentsDetailsForTranscriptViewModel
    {
        public StudentsDetailsForTranscriptViewModel()
        {
            gradeDetailsForTranscriptViewModel = new List<GradeDetailsForTranscriptViewModel>();
        }

        public List<GradeDetailsForTranscriptViewModel> gradeDetailsForTranscriptViewModel { get; set; }
        public string FirstGivenName { get; set; }
        public string MiddleName { get; set; }
        public string LastFamilyName { get; set; }
        public int? StudentId { get; set; }
        public Guid? StudentGuid { get; set; }
        public byte[] StudentPhoto { get; set; }
        public string StudentInternalId { get; set; }
        public DateTime? Dob { get; set; }
        public string HomeAddressLineOne { get; set; }
        public string HomeAddressLineTwo { get; set; }
        public string HomeAddressCountry { get; set; }
        public string HomeAddressCity { get; set; }
        public string HomeAddressState { get; set; }
        public string HomeAddressZip { get; set; }
        public decimal? CumulativeGPA { get; set; }
        public decimal? TotalCreditAttempeted { get; set; }
        public decimal? TotalCreditEarned { get; set; }
    }
}
