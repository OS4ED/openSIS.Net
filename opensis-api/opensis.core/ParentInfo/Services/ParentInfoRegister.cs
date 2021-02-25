using opensis.core.helper;
using opensis.core.ParentInfo.Interfaces;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.ParentInfos;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.ParentInfo.Services
{
    public class ParentInfoRegister : IParentInfoRegisterService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";

        public IParentInfoRepository parentInfoRepository;
        public ParentInfoRegister(IParentInfoRepository parentInfoRepository)
        {
            this.parentInfoRepository = parentInfoRepository;
        }
        public ParentInfoRegister() { }

        /// <summary>
        /// Add Parent For Student
        /// </summary>
        /// <param name="parentInfoAddViewModel"></param>
        /// <returns></returns>
        public ParentInfoAddViewModel AddParentForStudent(ParentInfoAddViewModel parentInfoAddViewModel)
        {
            ParentInfoAddViewModel ParentInfoAddModel = new ParentInfoAddViewModel();
            try
            {
                if (TokenManager.CheckToken(parentInfoAddViewModel._tenantName, parentInfoAddViewModel._token))
                {

                    ParentInfoAddModel = this.parentInfoRepository.AddParentForStudent(parentInfoAddViewModel);

                }
                else
                {
                    ParentInfoAddModel._failure = true;
                    ParentInfoAddModel._message = TOKENINVALID;

                }
            }
            catch (Exception es)
            {

                ParentInfoAddModel._failure = true;
                ParentInfoAddModel._message = es.Message;
            }
            return ParentInfoAddModel;

        }
        /// <summary>
        /// View Parent List For Student
        /// </summary>
        /// <param name="parentInfoList"></param>
        /// <returns></returns>
        public ParentInfoListModel ViewParentListForStudent(ParentInfoListModel parentInfoList)
        {
            ParentInfoListModel parentInfoViewListModel = new ParentInfoListModel();
            try
            {
                if (TokenManager.CheckToken(parentInfoList._tenantName, parentInfoList._token))
                {
                    parentInfoViewListModel = this.parentInfoRepository.ViewParentListForStudent(parentInfoList);
                }
                else
                {
                    parentInfoViewListModel._failure = true;
                    parentInfoViewListModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
{
                parentInfoViewListModel._failure = true;
                parentInfoViewListModel._message = es.Message;
}

            return parentInfoViewListModel;
        }
        /// <summary>
        /// Update parent Info
        /// </summary>
        /// <param name="parentInfoAddViewModel"></param>
        /// <returns></returns>
        public ParentInfoAddViewModel UpdateParentInfo(ParentInfoAddViewModel parentInfoAddViewModel)
        {
            ParentInfoAddViewModel parentInfoUpdateModel = new ParentInfoAddViewModel();
            try
            {
                if (TokenManager.CheckToken(parentInfoAddViewModel._tenantName, parentInfoAddViewModel._token))
                {
                    parentInfoUpdateModel = this.parentInfoRepository.UpdateParentInfo(parentInfoAddViewModel);
                }
                else
                {
                    parentInfoUpdateModel._failure = true;
                    parentInfoUpdateModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                parentInfoUpdateModel._failure = true;
                parentInfoUpdateModel._message = es.Message;
            }

            return parentInfoUpdateModel;
        }
        /// <summary>
        /// Get All parent Info
        /// </summary>
        /// <param name="pageResult"></param>
        /// <returns></returns>
        public GetAllParentInfoListForView GetAllParentInfoList(PageResult pageResult)
        {
            logger.Info("Method getAllParentInfoList called.");
            GetAllParentInfoListForView parentInfoList = new GetAllParentInfoListForView();
            try
            {
                if (TokenManager.CheckToken(pageResult._tenantName, pageResult._token))
                {
                    parentInfoList = this.parentInfoRepository.GetAllParentInfoList(pageResult);
                    if (parentInfoList.parentInfoForView.Count > 0)
                    {
                        parentInfoList._message = SUCCESS;
                        parentInfoList._failure = false;
                    }
                    else
                    {
                        parentInfoList._message = "NO RECORD FOUND";
                        parentInfoList._failure = true;
                    }
                    logger.Info("Method getAllParentInfoList end with success.");
                }

                else
                {
                    parentInfoList._failure = true;
                    parentInfoList._message = TOKENINVALID;
                    return parentInfoList;
                }
            }
            catch (Exception ex)
            {
                parentInfoList._message = ex.Message;
                parentInfoList._failure = true;
                logger.Error("Method getAllParentInfoList end with error :" + ex.Message);
            }


            return parentInfoList;


        }
        /// <summary>
        /// Delete parent Info
        /// </summary>
        /// <param name="parentInfoAddViewModel"></param>
        /// <returns></returns>
        public ParentInfoAddViewModel DeleteParentInfo(ParentInfoAddViewModel parentInfoAddViewModel)
        {
            ParentInfoAddViewModel ParentInfodelete = new ParentInfoAddViewModel();
            try
            {
                if (TokenManager.CheckToken(parentInfoAddViewModel._tenantName, parentInfoAddViewModel._token))
                {
                    ParentInfodelete = this.parentInfoRepository.DeleteParentInfo(parentInfoAddViewModel);
                }
                else
                {
                    ParentInfodelete._failure = true;
                    ParentInfodelete._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                ParentInfodelete._failure = true;
                ParentInfodelete._message = es.Message;
            }

            return ParentInfodelete;
        }
        /// <summary>
        /// Search Parent Info For Student
        /// </summary>
        /// <param name="getAllParentInfoListForView"></param>
        /// <returns></returns>
        public GetAllParentInfoListForView SearchParentInfoForStudent(GetAllParentInfoListForView getAllParentInfoListForView)
        {
            logger.Info("Method SearchParentInfoForStudent called.");
            GetAllParentInfoListForView parentInfoList = new GetAllParentInfoListForView();
            try
            {
                if (TokenManager.CheckToken(getAllParentInfoListForView._tenantName, getAllParentInfoListForView._token))
                {
                    parentInfoList = this.parentInfoRepository.SearchParentInfoForStudent(getAllParentInfoListForView);
                    parentInfoList._message = SUCCESS;
                    parentInfoList._failure = false;
                    logger.Info("Method SearchParentInfoForStudent end with success.");
                }
                else
                {
                    parentInfoList._failure = true;
                    parentInfoList._message = TOKENINVALID;
                    return parentInfoList;
                }
            }
            catch (Exception ex)
            {
                parentInfoList._message = ex.Message;
                parentInfoList._failure = true;
                logger.Error("Method SearchParentInfoForStudent end with error :" + ex.Message);
            }
            return parentInfoList;
        }
        /// <summary>
        /// View Parent Info By Id
        /// </summary>
        /// <param name="parentInfoAddViewModel"></param>
        /// <returns></returns>
        public ParentInfoAddViewModel ViewParentInfo(ParentInfoAddViewModel parentInfoAddViewModel)
        {
            logger.Info("Method viewParentInfo called.");
            ParentInfoAddViewModel parentInfoViewModel = new ParentInfoAddViewModel();
            try
            {
                if (TokenManager.CheckToken(parentInfoAddViewModel._tenantName, parentInfoAddViewModel._token))
                {
                    parentInfoViewModel = this.parentInfoRepository.ViewParentInfo(parentInfoAddViewModel);
                    parentInfoViewModel._message = SUCCESS;
                    parentInfoViewModel._failure = false;
                    logger.Info("Method viewParentInfo end with success.");

                }
                else
                {
                    parentInfoViewModel._failure = true;
                    parentInfoViewModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                parentInfoViewModel._message = es.Message;
                parentInfoViewModel._failure = true;
                logger.Error("Method viewParentInfo end with error :" + es.Message);
            }            
            return parentInfoViewModel;
        }
        /// <summary>
        /// Add Parent Info
        /// </summary>
        /// <param name="parentInfoAddViewModel"></param>
        /// <returns></returns>
        public ParentInfoAddViewModel AddParentInfo(ParentInfoAddViewModel parentInfoAddViewModel)
        {
            ParentInfoAddViewModel ParentInfoAddModel = new ParentInfoAddViewModel();
            try
            {
                if (TokenManager.CheckToken(parentInfoAddViewModel._tenantName, parentInfoAddViewModel._token))
                {
                    ParentInfoAddModel = this.parentInfoRepository.AddParentInfo(parentInfoAddViewModel);
                }
                else
                {
                    ParentInfoAddModel._failure = true;
                    ParentInfoAddModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {

                ParentInfoAddModel._failure = true;
                ParentInfoAddModel._message = es.Message;
            }
            return ParentInfoAddModel;
        }

        /// <summary>
        /// Remove Associated Parent
        /// </summary>
        /// <param name="parentInfoDeleteViewModel"></param>
        /// <returns></returns>
        public ParentInfoDeleteViewModel RemoveAssociatedParent(ParentInfoDeleteViewModel parentInfoDeleteViewModel)
        {
            ParentInfoDeleteViewModel parentAssociationshipDelete = new ParentInfoDeleteViewModel();
            try
            {
                if (TokenManager.CheckToken(parentInfoDeleteViewModel._tenantName, parentInfoDeleteViewModel._token))
                {
                    parentAssociationshipDelete = this.parentInfoRepository.RemoveAssociatedParent(parentInfoDeleteViewModel);
                }
                else
                {
                    parentAssociationshipDelete._failure = true;
                    parentAssociationshipDelete._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                parentAssociationshipDelete._failure = true;
                parentAssociationshipDelete._message = es.Message;
            }

            return parentAssociationshipDelete;
        }

        /// <summary>
        /// Add Update Parent Photo
        /// </summary>
        /// <param name="parentInfoAddViewModel"></param>
        /// <returns></returns>
        public ParentInfoAddViewModel AddUpdateParentPhoto(ParentInfoAddViewModel parentInfoAddViewModel)
        {
            ParentInfoAddViewModel parentPhotoUpdate = new ParentInfoAddViewModel();
            try
            {
                if (TokenManager.CheckToken(parentInfoAddViewModel._tenantName, parentInfoAddViewModel._token))
                {
                    parentPhotoUpdate = this.parentInfoRepository.AddUpdateParentPhoto(parentInfoAddViewModel);
                }
                else
                {
                    parentPhotoUpdate._failure = true;
                    parentPhotoUpdate._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                parentPhotoUpdate._message = es.Message;
                parentPhotoUpdate._failure = true;
            }
            return parentPhotoUpdate;
        }
    }
}
