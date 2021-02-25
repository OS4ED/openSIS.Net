using opensis.data.Models;
using opensis.data.ViewModels.ParentInfos;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.ParentInfo.Interfaces
{
    public interface IParentInfoRegisterService
    {
        public ParentInfoAddViewModel AddParentForStudent(ParentInfoAddViewModel parentInfoAddViewModel);
        public ParentInfoListModel ViewParentListForStudent(ParentInfoListModel parentInfoList);
        public ParentInfoAddViewModel UpdateParentInfo(ParentInfoAddViewModel parentInfoAddViewModel);
        public GetAllParentInfoListForView GetAllParentInfoList(PageResult pageResult);
        public ParentInfoAddViewModel DeleteParentInfo(ParentInfoAddViewModel parentInfoAddViewModel);
        public GetAllParentInfoListForView SearchParentInfoForStudent(GetAllParentInfoListForView getAllParentInfoListForView);
        public ParentInfoAddViewModel ViewParentInfo(ParentInfoAddViewModel parentInfoAddViewModel);
        public ParentInfoAddViewModel AddParentInfo(ParentInfoAddViewModel parentInfoAddViewModel);
        public ParentInfoDeleteViewModel RemoveAssociatedParent(ParentInfoDeleteViewModel parentInfoDeleteViewModel);
        public ParentInfoAddViewModel AddUpdateParentPhoto(ParentInfoAddViewModel parentInfoAddViewModel);
    }
}
