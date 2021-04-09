using opensis.core.Grade.Interfaces;
using opensis.core.helper;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.Grades;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.Grade.Services
{
    public class GradeService : IGradeService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";

        public IGradeRepository gradeRepository;
        public GradeService(IGradeRepository gradeRepository)
        {
            this.gradeRepository = gradeRepository;
        }

        //Required for Unit Testing
        public GradeService() { }
        /// <summary>
        /// Add Grade Scale
        /// </summary>
        /// <param name="gradeScaleAddViewModel"></param>
        /// <returns></returns>
        public GradeScaleAddViewModel AddGradeScale(GradeScaleAddViewModel gradeScaleAddViewModel)
        {
            GradeScaleAddViewModel GradeScaleAddModel = new GradeScaleAddViewModel();
            try
            {
                if (TokenManager.CheckToken(gradeScaleAddViewModel._tenantName + gradeScaleAddViewModel._userName, gradeScaleAddViewModel._token))
                {
                    GradeScaleAddModel = this.gradeRepository.AddGradeScale(gradeScaleAddViewModel);
                }
                else
                {
                    GradeScaleAddModel._failure = true;
                    GradeScaleAddModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {

                GradeScaleAddModel._failure = true;
                GradeScaleAddModel._message = es.Message;
            }
            return GradeScaleAddModel;
        }
        /// <summary>
        /// Update Grade Scale
        /// </summary>
        /// <param name="gradeScaleAddViewModel"></param>
        /// <returns></returns>
        public GradeScaleAddViewModel UpdateGradeScale(GradeScaleAddViewModel gradeScaleAddViewModel)
        {
            GradeScaleAddViewModel GradeScaleUpdateModel = new GradeScaleAddViewModel();
            try
            {
                if (TokenManager.CheckToken(gradeScaleAddViewModel._tenantName + gradeScaleAddViewModel._userName, gradeScaleAddViewModel._token))
                {
                    GradeScaleUpdateModel = this.gradeRepository.UpdateGradeScale(gradeScaleAddViewModel);
                }
                else
                {
                    GradeScaleUpdateModel._failure = true;
                    GradeScaleUpdateModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {

                GradeScaleUpdateModel._failure = true;
                GradeScaleUpdateModel._message = es.Message;
            }
            return GradeScaleUpdateModel;
        }
        /// <summary>
        /// Delete Grade Scale
        /// </summary>
        /// <param name="gradeScaleAddViewModel"></param>
        /// <returns></returns>
        public GradeScaleAddViewModel DeleteGradeScale(GradeScaleAddViewModel gradeScaleAddViewModel)
        {
            GradeScaleAddViewModel gradeScaleDeleteModel = new GradeScaleAddViewModel();
            try
            {
                if (TokenManager.CheckToken(gradeScaleAddViewModel._tenantName + gradeScaleAddViewModel._userName, gradeScaleAddViewModel._token))
                {
                    gradeScaleDeleteModel = this.gradeRepository.DeleteGradeScale(gradeScaleAddViewModel);
                }
                else
                {
                    gradeScaleDeleteModel._failure = true;
                    gradeScaleDeleteModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                gradeScaleDeleteModel._failure = true;
                gradeScaleDeleteModel._message = es.Message;
            }

            return gradeScaleDeleteModel;
        }
        /// <summary>
        /// Add Grade
        /// </summary>
        /// <param name="gradeAddViewModel"></param>
        /// <returns></returns>
        public GradeAddViewModel AddGrade(GradeAddViewModel gradeAddViewModel)
        {
            GradeAddViewModel GradeAddModel = new GradeAddViewModel();
            try
            {
                if (TokenManager.CheckToken(gradeAddViewModel._tenantName + gradeAddViewModel._userName, gradeAddViewModel._token))
                {
                    GradeAddModel = this.gradeRepository.AddGrade(gradeAddViewModel);
                }
                else
                {
                    GradeAddModel._failure = true;
                    GradeAddModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {

                GradeAddModel._failure = true;
                GradeAddModel._message = es.Message;
            }
            return GradeAddModel;
        }
        /// <summary>
        /// Update Grade
        /// </summary>
        /// <param name="gradeAddViewModel"></param>
        /// <returns></returns>
        public GradeAddViewModel UpdateGrade(GradeAddViewModel gradeAddViewModel)
        {
            GradeAddViewModel GradeUpdateModel = new GradeAddViewModel();
            try
            {
                if (TokenManager.CheckToken(gradeAddViewModel._tenantName + gradeAddViewModel._userName, gradeAddViewModel._token))
                {
                    GradeUpdateModel = this.gradeRepository.UpdateGrade(gradeAddViewModel);
                }
                else
                {
                    GradeUpdateModel._failure = true;
                    GradeUpdateModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {

                GradeUpdateModel._failure = true;
                GradeUpdateModel._message = es.Message;
            }
            return GradeUpdateModel;
        }
        /// <summary>
        /// Delete Grade
        /// </summary>
        /// <param name="gradeAddViewModel"></param>
        /// <returns></returns>
        public GradeAddViewModel DeleteGrade(GradeAddViewModel gradeAddViewModel)
        {
            GradeAddViewModel gradeDeleteModel = new GradeAddViewModel();
            try
            {
                if (TokenManager.CheckToken(gradeAddViewModel._tenantName + gradeAddViewModel._userName, gradeAddViewModel._token))
                {
                    gradeDeleteModel = this.gradeRepository.DeleteGrade(gradeAddViewModel);
                }
                else
                {
                    gradeDeleteModel._failure = true;
                    gradeDeleteModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                gradeDeleteModel._failure = true;
                gradeDeleteModel._message = es.Message;
            }

            return gradeDeleteModel;
        }
        /// <summary>
        /// Get All Grade Scale List
        /// </summary>
        /// <param name="gradeScaleListViewModel"></param>
        /// <returns></returns>
        public GradeScaleListViewModel GetAllGradeScaleList(GradeScaleListViewModel gradeScaleListViewModel)
        {
            GradeScaleListViewModel GradeScaleListModel = new GradeScaleListViewModel();
            try
            {
                if (TokenManager.CheckToken(gradeScaleListViewModel._tenantName + gradeScaleListViewModel._userName, gradeScaleListViewModel._token))
                {
                    GradeScaleListModel = this.gradeRepository.GetAllGradeScaleList(gradeScaleListViewModel);
                }
                else
                {
                    GradeScaleListModel._failure = true;
                    GradeScaleListModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                GradeScaleListModel._failure = true;
                GradeScaleListModel._message = es.Message;
            }
            return GradeScaleListModel;
        }
        /// <summary>
        /// Update Grade Sort Order
        /// </summary>
        /// <param name="gradeSortOrderModel"></param>
        /// <returns></returns>
        public GradeSortOrderModel UpdateGradeSortOrder(GradeSortOrderModel gradeSortOrderModel)
        {
            GradeSortOrderModel gradeSortOrderUpdateModel = new GradeSortOrderModel();
            try
            {
                if (TokenManager.CheckToken(gradeSortOrderModel._tenantName + gradeSortOrderModel._userName, gradeSortOrderModel._token))
                {
                    gradeSortOrderUpdateModel = this.gradeRepository.UpdateGradeSortOrder(gradeSortOrderModel);
                }
                else
                {
                    gradeSortOrderUpdateModel._failure = true;
                    gradeSortOrderUpdateModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                gradeSortOrderUpdateModel._failure = true;
                gradeSortOrderUpdateModel._message = es.Message;
            }
            return gradeSortOrderUpdateModel;
        }
        /// <summary>
        /// Add EffortGrade Library Category
        /// </summary>
        /// <param name="effortGradeLibraryCategoryAddViewModel"></param>
        /// <returns></returns>
        public EffortGradeLibraryCategoryAddViewModel AddEffortGradeLibraryCategory(EffortGradeLibraryCategoryAddViewModel effortGradeLibraryCategoryAddViewModel)
        {
            EffortGradeLibraryCategoryAddViewModel EffortGradeLibraryCategoryAddModel = new EffortGradeLibraryCategoryAddViewModel();
            try
            {
                if (TokenManager.CheckToken(effortGradeLibraryCategoryAddViewModel._tenantName + effortGradeLibraryCategoryAddViewModel._userName, effortGradeLibraryCategoryAddViewModel._token))
                {
                    EffortGradeLibraryCategoryAddModel = this.gradeRepository.AddEffortGradeLibraryCategory(effortGradeLibraryCategoryAddViewModel);
                }
                else
                {
                    EffortGradeLibraryCategoryAddModel._failure = true;
                    EffortGradeLibraryCategoryAddModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {

                EffortGradeLibraryCategoryAddModel._failure = true;
                EffortGradeLibraryCategoryAddModel._message = es.Message;
            }
            return EffortGradeLibraryCategoryAddModel;
        }
        /// <summary>
        /// Update EffortGrade Library Category
        /// </summary>
        /// <param name="effortGradeLibraryCategoryAddViewModel"></param>
        /// <returns></returns>
        public EffortGradeLibraryCategoryAddViewModel UpdateEffortGradeLibraryCategory(EffortGradeLibraryCategoryAddViewModel effortGradeLibraryCategoryAddViewModel)
        {
            EffortGradeLibraryCategoryAddViewModel EffortGradeLibraryCategoryUpdateModel = new EffortGradeLibraryCategoryAddViewModel();
            try
            {
                if (TokenManager.CheckToken(effortGradeLibraryCategoryAddViewModel._tenantName + effortGradeLibraryCategoryAddViewModel._userName, effortGradeLibraryCategoryAddViewModel._token))
                {
                    EffortGradeLibraryCategoryUpdateModel = this.gradeRepository.UpdateEffortGradeLibraryCategory(effortGradeLibraryCategoryAddViewModel);
                }
                else
                {
                    EffortGradeLibraryCategoryUpdateModel._failure = true;
                    EffortGradeLibraryCategoryUpdateModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {

                EffortGradeLibraryCategoryUpdateModel._failure = true;
                EffortGradeLibraryCategoryUpdateModel._message = es.Message;
            }
            return EffortGradeLibraryCategoryUpdateModel;
        }
        /// <summary>
        /// Delete EffortGrade Library Category
        /// </summary>
        /// <param name="effortGradeLibraryCategoryAddViewModel"></param>
        /// <returns></returns>
        public EffortGradeLibraryCategoryAddViewModel DeleteEffortGradeLibraryCategory(EffortGradeLibraryCategoryAddViewModel effortGradeLibraryCategoryAddViewModel)
        {
            EffortGradeLibraryCategoryAddViewModel effortGradeLibraryCategoryDeleteModel = new EffortGradeLibraryCategoryAddViewModel();
            try
            {
                if (TokenManager.CheckToken(effortGradeLibraryCategoryAddViewModel._tenantName + effortGradeLibraryCategoryAddViewModel._userName, effortGradeLibraryCategoryAddViewModel._token))
                {
                    effortGradeLibraryCategoryDeleteModel = this.gradeRepository.DeleteEffortGradeLibraryCategory(effortGradeLibraryCategoryAddViewModel);
                }
                else
                {
                    effortGradeLibraryCategoryDeleteModel._failure = true;
                    effortGradeLibraryCategoryDeleteModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                effortGradeLibraryCategoryDeleteModel._failure = true;
                effortGradeLibraryCategoryDeleteModel._message = es.Message;
            }

            return effortGradeLibraryCategoryDeleteModel;
        }
        /// <summary>
        /// Add EffortGrade Library Category Item
        /// </summary>
        /// <param name="effortGradeLibraryCategoryItemAddViewModel"></param>
        /// <returns></returns>
        public EffortGradeLibraryCategoryItemAddViewModel AddEffortGradeLibraryCategoryItem(EffortGradeLibraryCategoryItemAddViewModel effortGradeLibraryCategoryItemAddViewModel)
        {
            EffortGradeLibraryCategoryItemAddViewModel EffortGradeLibraryCategoryItemAddModel = new EffortGradeLibraryCategoryItemAddViewModel();
            try
            {
                if (TokenManager.CheckToken(effortGradeLibraryCategoryItemAddViewModel._tenantName + effortGradeLibraryCategoryItemAddViewModel._userName, effortGradeLibraryCategoryItemAddViewModel._token))
                {
                    EffortGradeLibraryCategoryItemAddModel = this.gradeRepository.AddEffortGradeLibraryCategoryItem(effortGradeLibraryCategoryItemAddViewModel);
                }
                else
                {
                    EffortGradeLibraryCategoryItemAddModel._failure = true;
                    EffortGradeLibraryCategoryItemAddModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {

                EffortGradeLibraryCategoryItemAddModel._failure = true;
                EffortGradeLibraryCategoryItemAddModel._message = es.Message;
            }
            return EffortGradeLibraryCategoryItemAddModel;
        }
        /// <summary>
        /// Update EffortGrade Library Category Item
        /// </summary>
        /// <param name="effortGradeLibraryCategoryItemAddViewModel"></param>
        /// <returns></returns>
        public EffortGradeLibraryCategoryItemAddViewModel UpdateEffortGradeLibraryCategoryItem(EffortGradeLibraryCategoryItemAddViewModel effortGradeLibraryCategoryItemAddViewModel)
        {
            EffortGradeLibraryCategoryItemAddViewModel EffortGradeLibraryCategoryItemUpdateModel = new EffortGradeLibraryCategoryItemAddViewModel();
            try
            {
                if (TokenManager.CheckToken(effortGradeLibraryCategoryItemAddViewModel._tenantName + effortGradeLibraryCategoryItemAddViewModel._userName, effortGradeLibraryCategoryItemAddViewModel._token))
                {
                    EffortGradeLibraryCategoryItemUpdateModel = this.gradeRepository.UpdateEffortGradeLibraryCategoryItem(effortGradeLibraryCategoryItemAddViewModel);
                }
                else
                {
                    EffortGradeLibraryCategoryItemUpdateModel._failure = true;
                    EffortGradeLibraryCategoryItemUpdateModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {

                EffortGradeLibraryCategoryItemUpdateModel._failure = true;
                EffortGradeLibraryCategoryItemUpdateModel._message = es.Message;
            }
            return EffortGradeLibraryCategoryItemUpdateModel;
        }
        /// <summary>
        /// Delete EffortGrade Library Category Item
        /// </summary>
        /// <param name="effortGradeLibraryCategoryItemAddViewModel"></param>
        /// <returns></returns>
        public EffortGradeLibraryCategoryItemAddViewModel DeleteEffortGradeLibraryCategoryItem(EffortGradeLibraryCategoryItemAddViewModel effortGradeLibraryCategoryItemAddViewModel)
        {
            EffortGradeLibraryCategoryItemAddViewModel effortGradeLibraryCategoryItemDeleteModel = new EffortGradeLibraryCategoryItemAddViewModel();
            try
            {
                if (TokenManager.CheckToken(effortGradeLibraryCategoryItemAddViewModel._tenantName+ effortGradeLibraryCategoryItemAddViewModel._userName, effortGradeLibraryCategoryItemAddViewModel._token))
                {
                    effortGradeLibraryCategoryItemDeleteModel = this.gradeRepository.DeleteEffortGradeLibraryCategoryItem(effortGradeLibraryCategoryItemAddViewModel);
                }
                else
                {
                    effortGradeLibraryCategoryItemDeleteModel._failure = true;
                    effortGradeLibraryCategoryItemDeleteModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                effortGradeLibraryCategoryItemDeleteModel._failure = true;
                effortGradeLibraryCategoryItemDeleteModel._message = es.Message;
            }

            return effortGradeLibraryCategoryItemDeleteModel;
        }
        /// <summary>
        /// Get All EffortGrade Llibrary Category List
        /// </summary>
        /// <param name="effortGradeLlibraryCategoryListViewModel"></param>
        /// <returns></returns>
        public EffortGradeLlibraryCategoryListViewModel GetAllEffortGradeLlibraryCategoryList(EffortGradeLlibraryCategoryListViewModel effortGradeLlibraryCategoryListViewModel)
        {
            EffortGradeLlibraryCategoryListViewModel effortGradeLlibraryCategoryListModel = new EffortGradeLlibraryCategoryListViewModel();
            try
            {
                if (TokenManager.CheckToken(effortGradeLlibraryCategoryListViewModel._tenantName + effortGradeLlibraryCategoryListViewModel._userName, effortGradeLlibraryCategoryListViewModel._token))
                {
                    effortGradeLlibraryCategoryListModel = this.gradeRepository.GetAllEffortGradeLlibraryCategoryList(effortGradeLlibraryCategoryListViewModel);
                }
                else
                {
                    effortGradeLlibraryCategoryListModel._failure = true;
                    effortGradeLlibraryCategoryListModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                effortGradeLlibraryCategoryListModel._failure = true;
                effortGradeLlibraryCategoryListModel._message = es.Message;
            }
            return effortGradeLlibraryCategoryListModel;
        }
        /// <summary>
        /// Update EffortGrade Llibrary Category SortOrder
        /// </summary>
        /// <param name="effortgradeLibraryCategorySortOrderModel"></param>
        /// <returns></returns>
        public EffortgradeLibraryCategorySortOrderModel UpdateEffortGradeLlibraryCategorySortOrder(EffortgradeLibraryCategorySortOrderModel effortgradeLibraryCategorySortOrderModel)
        {
            EffortgradeLibraryCategorySortOrderModel effortgradeLibraryCategorySortOrderUpdateModel = new EffortgradeLibraryCategorySortOrderModel();
            try
            {
                if (TokenManager.CheckToken(effortgradeLibraryCategorySortOrderModel._tenantName + effortgradeLibraryCategorySortOrderModel._userName, effortgradeLibraryCategorySortOrderModel._token))
                {
                    effortgradeLibraryCategorySortOrderUpdateModel = this.gradeRepository.UpdateEffortGradeLlibraryCategorySortOrder(effortgradeLibraryCategorySortOrderModel);
                }
                else
                {
                    effortgradeLibraryCategorySortOrderUpdateModel._failure = true;
                    effortgradeLibraryCategorySortOrderUpdateModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                effortgradeLibraryCategorySortOrderUpdateModel._failure = true;
                effortgradeLibraryCategorySortOrderUpdateModel._message = es.Message;
            }
            return effortgradeLibraryCategorySortOrderUpdateModel;
        }

        /// <summary>
        /// Add EffortGradeScale
        /// </summary>
        /// <param name="effortGradeScaleAddViewModel"></param>
        /// <returns></returns>
        public EffortGradeScaleAddViewModel AddEffortGradeScale(EffortGradeScaleAddViewModel effortGradeScaleAddViewModel)
        {
            EffortGradeScaleAddViewModel effortGradeScaleAdd = new EffortGradeScaleAddViewModel();
            try
            {
                if (TokenManager.CheckToken(effortGradeScaleAddViewModel._tenantName + effortGradeScaleAddViewModel._userName, effortGradeScaleAddViewModel._token))
                {
                    effortGradeScaleAdd = this.gradeRepository.AddEffortGradeScale(effortGradeScaleAddViewModel);
                }
                else
                {
                    effortGradeScaleAdd._failure = true;
                    effortGradeScaleAdd._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                effortGradeScaleAdd._failure = true;
                effortGradeScaleAdd._message = es.Message;
            }
            return effortGradeScaleAdd;
        }

        /// <summary>
        /// Update EffortGradeScale
        /// </summary>
        /// <param name="effortGradeScaleAddViewModel"></param>
        /// <returns></returns>
        public EffortGradeScaleAddViewModel UpdateEffortGradeScale(EffortGradeScaleAddViewModel effortGradeScaleAddViewModel)
        {
            EffortGradeScaleAddViewModel effortGradeScaleUpdate = new EffortGradeScaleAddViewModel();
            try
            {
                if (TokenManager.CheckToken(effortGradeScaleAddViewModel._tenantName + effortGradeScaleAddViewModel._userName, effortGradeScaleAddViewModel._token))
                {
                    effortGradeScaleUpdate = this.gradeRepository.UpdateEffortGradeScale(effortGradeScaleAddViewModel);
                }
                else
                {
                    effortGradeScaleUpdate._failure = true;
                    effortGradeScaleUpdate._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                effortGradeScaleUpdate._failure = true;
                effortGradeScaleUpdate._message = es.Message;
            }
            return effortGradeScaleUpdate;
        }

        /// <summary>
        /// Delete EffortGradeScale
        /// </summary>
        /// <param name="effortGradeScaleAddViewModel"></param>
        /// <returns></returns>
        public EffortGradeScaleAddViewModel DeleteEffortGradeScale(EffortGradeScaleAddViewModel effortGradeScaleAddViewModel)
        {
            EffortGradeScaleAddViewModel effortGradeScaleDelete = new EffortGradeScaleAddViewModel();
            try
            {
                if (TokenManager.CheckToken(effortGradeScaleAddViewModel._tenantName + effortGradeScaleAddViewModel._userName, effortGradeScaleAddViewModel._token))
                {
                    effortGradeScaleDelete = this.gradeRepository.DeleteEffortGradeScale(effortGradeScaleAddViewModel);
                }
                else
                {
                    effortGradeScaleDelete._failure = true;
                    effortGradeScaleDelete._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                effortGradeScaleDelete._failure = true;
                effortGradeScaleDelete._message = es.Message;
            }
            return effortGradeScaleDelete;
        }

        /// <summary>
        /// Get All EffortGradeScale
        /// </summary>
        /// <param name="pageResult"></param>
        /// <returns></returns>
        public EffortGradeScaleListModel GetAllEffortGradeScale(PageResult pageResult)
        {
            logger.Info("Method getAllEffortGradeScaleList called.");
            EffortGradeScaleListModel effortGradeScaleList = new EffortGradeScaleListModel();
            try
            {
                if (TokenManager.CheckToken(pageResult._tenantName+pageResult._userName, pageResult._token))
                {
                    effortGradeScaleList = this.gradeRepository.GetAllEffortGradeScale(pageResult);
                    effortGradeScaleList._message = SUCCESS;
                    effortGradeScaleList._failure = false;
                    logger.Info("Method getAllEffortGradeScaleList end with success.");
                }
                else
                {
                    effortGradeScaleList._failure = true;
                    effortGradeScaleList._message = TOKENINVALID;
                    return effortGradeScaleList;
                }
            }
            catch (Exception ex)
            {
                effortGradeScaleList._message = ex.Message;
                effortGradeScaleList._failure = true;
                logger.Error("Method getAllEffortGradeScaleList end with error :" + ex.Message);
            }
            return effortGradeScaleList;
        }

        /// <summary>
        /// Update EffortGradeScale SortOrder
        /// </summary>
        /// <param name="effortGradeScaleSortOrderViewModel"></param>
        /// <returns></returns>
        public EffortGradeScaleSortOrderViewModel UpdateEffortGradeScaleSortOrder(EffortGradeScaleSortOrderViewModel effortGradeScaleSortOrderViewModel)
        {
            EffortGradeScaleSortOrderViewModel effortGradeScaleSortOrderUpdate = new EffortGradeScaleSortOrderViewModel();
            try
            {
                if (TokenManager.CheckToken(effortGradeScaleSortOrderViewModel._tenantName + effortGradeScaleSortOrderViewModel._userName, effortGradeScaleSortOrderViewModel._token))
                {
                    effortGradeScaleSortOrderUpdate = this.gradeRepository.UpdateEffortGradeScaleSortOrder(effortGradeScaleSortOrderViewModel);
                }
                else
                {
                    effortGradeScaleSortOrderUpdate._failure = true;
                    effortGradeScaleSortOrderUpdate._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                effortGradeScaleSortOrderUpdate._failure = true;
                effortGradeScaleSortOrderUpdate._message = es.Message;
            }
            return effortGradeScaleSortOrderUpdate;
        }

        /// <summary>
        /// Add GradeUsStandard
        /// </summary>
        /// <param name="gradeUsStandardAddViewModel"></param>
        /// <returns></returns>
        public GradeUsStandardAddViewModel AddGradeUsStandard(GradeUsStandardAddViewModel gradeUsStandardAddViewModel)
        {
            GradeUsStandardAddViewModel gradeUsStandardAdd = new GradeUsStandardAddViewModel();
            try
            {
                if (TokenManager.CheckToken(gradeUsStandardAddViewModel._tenantName + gradeUsStandardAddViewModel._userName, gradeUsStandardAddViewModel._token))
                {
                    gradeUsStandardAdd = this.gradeRepository.AddGradeUsStandard(gradeUsStandardAddViewModel);
                }
                else
                {
                    gradeUsStandardAdd._failure = true;
                    gradeUsStandardAdd._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                gradeUsStandardAdd._failure = true;
                gradeUsStandardAdd._message = es.Message;
            }
            return gradeUsStandardAdd;
        }

        /// <summary>
        /// Update GradeUsStandard
        /// </summary>
        /// <param name="gradeUsStandardAddViewModel"></param>
        /// <returns></returns>
        public GradeUsStandardAddViewModel UpdateGradeUsStandard(GradeUsStandardAddViewModel gradeUsStandardAddViewModel)
        {
            GradeUsStandardAddViewModel gradeUsStandardUpdate = new GradeUsStandardAddViewModel();
            try
            {
                if (TokenManager.CheckToken(gradeUsStandardAddViewModel._tenantName + gradeUsStandardAddViewModel._userName, gradeUsStandardAddViewModel._token))
                {
                    gradeUsStandardUpdate = this.gradeRepository.UpdateGradeUsStandard(gradeUsStandardAddViewModel);
                }
                else
                {
                    gradeUsStandardUpdate._failure = true;
                    gradeUsStandardUpdate._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                gradeUsStandardUpdate._failure = true;
                gradeUsStandardUpdate._message = es.Message;
            }
            return gradeUsStandardUpdate;
        }

        /// <summary>
        /// Delete GradeUsStandard
        /// </summary>
        /// <param name="gradeUsStandardAddViewModel"></param>
        /// <returns></returns>
        public GradeUsStandardAddViewModel DeleteGradeUsStandard(GradeUsStandardAddViewModel gradeUsStandardAddViewModel)
        {
            GradeUsStandardAddViewModel gradeUsStandardDelete = new GradeUsStandardAddViewModel();
            try
            {
                if (TokenManager.CheckToken(gradeUsStandardAddViewModel._tenantName + gradeUsStandardAddViewModel._userName, gradeUsStandardAddViewModel._token))
                {
                    gradeUsStandardDelete = this.gradeRepository.DeleteGradeUsStandard(gradeUsStandardAddViewModel);
                }
                else
                {
                    gradeUsStandardDelete._failure = true;
                    gradeUsStandardDelete._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                gradeUsStandardDelete._failure = true;
                gradeUsStandardDelete._message = es.Message;
            }
            return gradeUsStandardDelete;
        }

        /// <summary>
        /// Get All GradeUsStandard List
        /// </summary>
        /// <param name="pageResult"></param>
        /// <returns></returns>
        public GradeUsStandardListModel GetAllGradeUsStandardList(PageResult pageResult)
        {
            logger.Info("Method getAllGradeUsStandardList called.");
            GradeUsStandardListModel gradeUsStandardList = new GradeUsStandardListModel();
            try
            {
                if (TokenManager.CheckToken(pageResult._tenantName+pageResult._userName, pageResult._token))
                {
                    gradeUsStandardList = this.gradeRepository.GetAllGradeUsStandardList(pageResult);
                    gradeUsStandardList._message = SUCCESS;
                    gradeUsStandardList._failure = false;
                    logger.Info("Method getAllGradeUsStandardList end with success.");
                }
                else
                {
                    gradeUsStandardList._failure = true;
                    gradeUsStandardList._message = TOKENINVALID;
                    return gradeUsStandardList;
                }
            }
            catch (Exception ex)
            {
                gradeUsStandardList._message = ex.Message;
                gradeUsStandardList._failure = true;
                logger.Error("Method getAllGradeUsStandardList end with error :" + ex.Message);
            }
            return gradeUsStandardList;
        }

        /// <summary>
        /// Get All Subject Standard List
        /// </summary>
        /// <param name="gradeUsStandardListModel"></param>
        /// <returns></returns>
        public GradeUsStandardListModel GetAllSubjectStandardList(GradeUsStandardListModel gradeUsStandardListModel)
        {
            GradeUsStandardListModel subjectStandardList = new GradeUsStandardListModel();
            try
            {
                if (TokenManager.CheckToken(gradeUsStandardListModel._tenantName + gradeUsStandardListModel._userName, gradeUsStandardListModel._token))
                {
                    subjectStandardList = this.gradeRepository.GetAllSubjectStandardList(gradeUsStandardListModel);
                }
                else
                {
                    subjectStandardList._failure = true;
                    subjectStandardList._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                subjectStandardList._failure = true;
                subjectStandardList._message = es.Message;
            }
            return subjectStandardList;
        }

        /// <summary>
        /// Get All Course Standard List
        /// </summary>
        /// <param name="gradeUsStandardListModel"></param>
        /// <returns></returns>
        public GradeUsStandardListModel GetAllCourseStandardList(GradeUsStandardListModel gradeUsStandardListModel)
        {
            GradeUsStandardListModel courseStandardList = new GradeUsStandardListModel();
            try
            {
                if (TokenManager.CheckToken(gradeUsStandardListModel._tenantName + gradeUsStandardListModel._userName, gradeUsStandardListModel._token))
                {
                    courseStandardList = this.gradeRepository.GetAllCourseStandardList(gradeUsStandardListModel);
                }
                else
                {
                    courseStandardList._failure = true;
                    courseStandardList._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                courseStandardList._failure = true;
                courseStandardList._message = es.Message;
            }
            return courseStandardList;
        }

        /// <summary>
        /// Check StandardRefNo Is Valid Or Not
        /// </summary>
        /// <param name="checkStandardRefNoViewModel"></param>
        /// <returns></returns>
        public CheckStandardRefNoViewModel CheckStandardRefNo(CheckStandardRefNoViewModel checkStandardRefNoViewModel)
        {
            CheckStandardRefNoViewModel checkStandardRefNo = new CheckStandardRefNoViewModel();
            try
            {
                if (TokenManager.CheckToken(checkStandardRefNoViewModel._tenantName + checkStandardRefNoViewModel._userName, checkStandardRefNoViewModel._token))
                {
                    checkStandardRefNo = this.gradeRepository.CheckStandardRefNo(checkStandardRefNoViewModel);
                }
                else
                {
                    checkStandardRefNo._failure = true;
                    checkStandardRefNo._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                checkStandardRefNo._failure = true;
                checkStandardRefNo._message = es.Message;
            }
            return checkStandardRefNo;
        }
        /// <summary>
        /// Add Honor Roll
        /// </summary>
        /// <param name="honorRollsAddViewModel"></param>
        /// <returns></returns>
        public HonorRollsAddViewModel AddHonorRoll(HonorRollsAddViewModel honorRollsAddViewModel)
        {
            HonorRollsAddViewModel HonorRollsdAdd = new HonorRollsAddViewModel();
            try
            {
                if (TokenManager.CheckToken(honorRollsAddViewModel._tenantName+honorRollsAddViewModel._userName, honorRollsAddViewModel._token))
                {
                    HonorRollsdAdd = this.gradeRepository.AddHonorRoll(honorRollsAddViewModel);
                }
                else
                {
                    HonorRollsdAdd._failure = true;
                    HonorRollsdAdd._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                HonorRollsdAdd._failure = true;
                HonorRollsdAdd._message = es.Message;
            }
            return HonorRollsdAdd;
        }
        /// <summary>
        /// Update Honor Roll
        /// </summary>
        /// <param name="honorRollsAddViewModel"></param>
        /// <returns></returns>
        public HonorRollsAddViewModel UpdateHonorRoll(HonorRollsAddViewModel honorRollsAddViewModel)
        {
            HonorRollsAddViewModel honorRollUpdate = new HonorRollsAddViewModel();
            try
            {
                if (TokenManager.CheckToken(honorRollsAddViewModel._tenantName + honorRollsAddViewModel._userName, honorRollsAddViewModel._token))
                {
                    honorRollUpdate = this.gradeRepository.UpdateHonorRoll(honorRollsAddViewModel);
                }
                else
                {
                    honorRollUpdate._failure = true;
                    honorRollUpdate._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                honorRollUpdate._failure = true;
                honorRollUpdate._message = es.Message;
            }
            return honorRollUpdate;
        }
        /// <summary>
        /// Get All Honor Roll List
        /// </summary>
        /// <param name="pageResult"></param>
        /// <returns></returns>
        public HonorRollsListViewModel GetAllHonorRollList(PageResult pageResult)
        {
            logger.Info("Method GetAllHonorRollList called.");
            HonorRollsListViewModel honorRollList = new HonorRollsListViewModel();
            try
            {
                if (TokenManager.CheckToken(pageResult._tenantName + pageResult._userName, pageResult._token))
                {
                    honorRollList = this.gradeRepository.GetAllHonorRollList(pageResult);
                    honorRollList._message = SUCCESS;
                    honorRollList._failure = false;
                    logger.Info("Method GetAllHonorRollList end with success.");
                }
                else
                {
                    honorRollList._failure = true;
                    honorRollList._message = TOKENINVALID;
                    return honorRollList;
                }
            }
            catch (Exception ex)
            {
                honorRollList._message = ex.Message;
                honorRollList._failure = true;
                logger.Error("Method GetAllHonorRollList end with error :" + ex.Message);
            }
            return honorRollList;
        }
        /// <summary>
        /// Delete Honor Roll
        /// </summary>
        /// <param name="honorRollsAddViewModel"></param>
        /// <returns></returns>
        public HonorRollsAddViewModel DeleteHonorRoll(HonorRollsAddViewModel honorRollsAddViewModel)
        {
            HonorRollsAddViewModel honorRollDelete = new HonorRollsAddViewModel();
            try
            {
                if (TokenManager.CheckToken(honorRollsAddViewModel._tenantName + honorRollsAddViewModel._userName, honorRollsAddViewModel._token))
                {
                    honorRollDelete = this.gradeRepository.DeleteHonorRoll(honorRollsAddViewModel);
                }
                else
                {
                    honorRollDelete._failure = true;
                    honorRollDelete._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                honorRollDelete._failure = true;
                honorRollDelete._message = es.Message;
            }
            return honorRollDelete;
        }
    }
}  

