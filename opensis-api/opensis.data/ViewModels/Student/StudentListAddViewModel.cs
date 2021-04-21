using opensis.data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels.Student
{
    public class StudentListAddViewModel : CommonFields
    {
        public StudentListAddViewModel()
        {
            studentAddViewModelList = new List<StudentAddViewModel>();
        }
        public List<StudentAddViewModel> studentAddViewModelList { get; set; }
        public Guid TenantId { get; set; }
        public int SchoolId { get; set; }
    }
}
