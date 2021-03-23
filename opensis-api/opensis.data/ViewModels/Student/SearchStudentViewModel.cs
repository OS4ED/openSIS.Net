using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.Student
{
    public class SearchStudentViewModel : CommonFields
    {
        public SearchStudentViewModel()
        {
            searchStudentForView = new List<SearchStudentForView>();
        }
        public List<SearchStudentForView> searchStudentForView { get; set; }
        public List<StudentMaster> StudentMasters { get; set; }
        public Guid? TenantId { get; set; }
        public int? SchoolId { get; set; }
        public string StudentName { get; set; }
        public int? StudentId { get; set; }
        public string AlternateId { get; set; }
        public int? GradeId { get; set; }
        public int? SectionId { get; set; }
        public int? FirstLanguageId { get; set; }

        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
