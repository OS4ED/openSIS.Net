using opensis.data.Models;
using opensis.data.ViewModels.Staff;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.Staff.Interfaces
{
    public interface IStaffService
    {
        StaffAddViewModel AddStaff(StaffAddViewModel staffAddViewModel);
        StaffListModel GetAllStaffList(PageResult pageResult);
        StaffAddViewModel ViewStaff(StaffAddViewModel staffAddViewModel);
        StaffAddViewModel UpdateStaff(StaffAddViewModel staffAddViewModel);
        public CheckStaffInternalIdViewModel CheckStaffInternalId(CheckStaffInternalIdViewModel checkStaffInternalIdViewModel);
        StaffSchoolInfoAddViewModel AddStaffSchoolInfo(StaffSchoolInfoAddViewModel staffSchoolInfoAddViewModel);
        StaffSchoolInfoAddViewModel ViewStaffSchoolInfo(StaffSchoolInfoAddViewModel staffSchoolInfoAddViewModel);
        StaffSchoolInfoAddViewModel UpdateStaffSchoolInfo(StaffSchoolInfoAddViewModel staffSchoolInfoAddViewModel);
        

        public StaffCertificateInfoAddViewModel AddStaffCertificateInfo(StaffCertificateInfoAddViewModel staffCertificateInfoAddViewModel);
        public StaffCertificateInfoListModel GetAllStaffCertificateInfo(StaffCertificateInfoListModel staffCertificateInfoListModel);
        public StaffCertificateInfoAddViewModel UpdateStaffCertificateInfo(StaffCertificateInfoAddViewModel staffCertificateInfoAddViewModel);
        public StaffCertificateInfoAddViewModel DeleteStaffCertificateInfo(StaffCertificateInfoAddViewModel staffCertificateInfoAddViewModel);
        public StaffAddViewModel AddUpdateStaffPhoto(StaffAddViewModel staffAddViewModel);
        public StaffListAddViewModel AddStaffList(StaffListAddViewModel staffListAddViewModel);
    }
}
