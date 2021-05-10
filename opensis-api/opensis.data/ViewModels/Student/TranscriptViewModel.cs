using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.Student
{
    public class TranscriptViewModel : CommonFields
    {
        public TranscriptViewModel()
        {
            StudentsDetailsForTranscriptViewModelList = new List<StudentsDetailsForTranscriptViewModel>();
            gradeList = new List<Grade>();
        }
        public Guid? TenantId { get; set; }
        public int? SchoolId { get; set; }
        public bool? SchoolLogo { get; set; }
        public bool? StudentPhoto { get; set; }
        public bool? GradeLagend { get; set; }
        public string GradeLavels { get; set; }
        public List<StudentsDetailsForTranscriptViewModel> StudentsDetailsForTranscriptViewModelList { get; set; }
        public string SchoolName { get; set; }
        public byte[] SchoolPicture { get; set; }
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public List<Grade> gradeList{ get; set; }
    }
}
