using opensis.data.Models;
using opensis.data.ViewModels.School;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace opensis.data.Interface
{
    public interface ISchoolRepository
    {
      

        public SchoolAddViewModel AddSchool(SchoolAddViewModel school);
        public SchoolAddViewModel UpdateSchool(SchoolAddViewModel school);
        public SchoolAddViewModel ViewSchool(SchoolAddViewModel school);
        public SchoolListModel GetAllSchoolList(PageResult pageResult);
        public SchoolListModel GetAllSchools(SchoolListModel school);
        public CheckSchoolInternalIdViewModel CheckSchoolInternalId(CheckSchoolInternalIdViewModel checkSchoolInternalIdViewModel);
        public SchoolListViewModel StudentEnrollmentSchoolList(SchoolListViewModel schoolListViewModel);
        public SchoolAddViewModel AddUpdateSchoolLogo(SchoolAddViewModel schoolAddViewModel);
        //Task<SchoolLogoUpdateModel> updateSchoolLogo(Guid guid, SchoolLogoUpdateModel schoolLogoUpdateModel);
    }
}
