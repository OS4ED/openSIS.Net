using opensis.data.Models;
using opensis.data.ViewModels.School;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace opensis.core.School.Interfaces
{
    public interface ISchoolRegisterService
    {
        //public SchoolListViewModel getAllSchools(SchoolViewModel objModel);

        //public SchoolListViewModel SaveSchool(Schools school);
        //public bool IsMandatoryFieldsArePresent(Schools schools);


        public SchoolAddViewModel SaveSchool(SchoolAddViewModel schools);
        public SchoolAddViewModel UpdateSchool(SchoolAddViewModel schools);

        public SchoolAddViewModel ViewSchool(SchoolAddViewModel schools);
        public SchoolListModel GetAllSchools(SchoolListModel school);

        public SchoolListModel GetAllSchoolList(PageResult pageResult);
        public CheckSchoolInternalIdViewModel CheckSchoolInternalId(CheckSchoolInternalIdViewModel checkSchoolInternalIdViewModel);
        public SchoolListViewModel StudentEnrollmentSchoolList(SchoolListViewModel schoolListViewModel);
        public SchoolAddViewModel AddUpdateSchoolLogo(SchoolAddViewModel schoolAddViewModel);
        public CopySchoolViewModel CopySchool(CopySchoolViewModel copySchoolViewModel);
    }
}
