using Microsoft.EntityFrameworkCore;
using opensis.data.Helper;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.Grades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace opensis.data.Repository
{
    public class GradeRepository : IGradeRepository
    {
        private CRMContext context;
        private static readonly string NORECORDFOUND = "No Record Found";
        public GradeRepository(IDbContextFactory dbContextFactory)
        {
            this.context = dbContextFactory.Create();
        }
        /// <summary>
        /// Add Grade Scale
        /// </summary>
        /// <param name="gradeScaleAddViewModel"></param>
        /// <returns></returns>
        public GradeScaleAddViewModel AddGradeScale(GradeScaleAddViewModel gradeScaleAddViewModel)
        {
            try
            {
                var checkGradeScaleName = this.context?.GradeScale.Where(x => x.SchoolId == gradeScaleAddViewModel.gradeScale.SchoolId && x.TenantId == gradeScaleAddViewModel.gradeScale.TenantId && x.GradeScaleName.ToLower() == gradeScaleAddViewModel.gradeScale.GradeScaleName.ToLower()).FirstOrDefault();
                if (checkGradeScaleName != null)
                {
                    gradeScaleAddViewModel._failure = true;
                    gradeScaleAddViewModel._message = "Grade Scale Name Already Exists";
                }
                else
                {
                    int? GradeScaleId = 1;
                    int? SortOrder = 1;

                    var gradeScaleData = this.context?.GradeScale.Where(x => x.SchoolId == gradeScaleAddViewModel.gradeScale.SchoolId && x.TenantId == gradeScaleAddViewModel.gradeScale.TenantId).OrderByDescending(x => x.GradeScaleId).FirstOrDefault();

                    if (gradeScaleData != null)
                    {
                        GradeScaleId = gradeScaleData.GradeScaleId + 1;
                    }

                    var gradeScaleSortOrder = this.context?.GradeScale.Where(x => x.SchoolId == gradeScaleAddViewModel.gradeScale.SchoolId && x.TenantId == gradeScaleAddViewModel.gradeScale.TenantId).OrderByDescending(x => x.SortOrder).FirstOrDefault();

                    if (gradeScaleSortOrder != null)
                    {
                        SortOrder = gradeScaleData.SortOrder + 1;
                    }

                    gradeScaleAddViewModel.gradeScale.GradeScaleId = (int)GradeScaleId;
                    gradeScaleAddViewModel.gradeScale.SortOrder = (int)SortOrder;
                    gradeScaleAddViewModel.gradeScale.CreatedOn = DateTime.UtcNow;
                    this.context?.GradeScale.Add(gradeScaleAddViewModel.gradeScale);
                    this.context?.SaveChanges();
                    gradeScaleAddViewModel._failure = false;
                    gradeScaleAddViewModel._message = "Grade Scale Added Successfully";
                }                
            }
            catch (Exception es)
            {
                gradeScaleAddViewModel._failure = true;
                gradeScaleAddViewModel._message = es.Message;
            }
            return gradeScaleAddViewModel;
        }
        /// <summary>
        /// Update Grade Scale
        /// </summary>
        /// <param name="gradeScaleAddViewModel"></param>
        /// <returns></returns>
        public GradeScaleAddViewModel UpdateGradeScale(GradeScaleAddViewModel gradeScaleAddViewModel)
        {            
            try
            {
                var gradeScaleUpdate = this.context?.GradeScale.FirstOrDefault(x => x.TenantId == gradeScaleAddViewModel.gradeScale.TenantId && x.SchoolId == gradeScaleAddViewModel.gradeScale.SchoolId && x.GradeScaleId == gradeScaleAddViewModel.gradeScale.GradeScaleId);
                
                if (gradeScaleUpdate != null)
                {
                    var checkGradeScaleName = this.context?.GradeScale.Where(x => x.SchoolId == gradeScaleAddViewModel.gradeScale.SchoolId && x.TenantId == gradeScaleAddViewModel.gradeScale.TenantId && x.GradeScaleId != gradeScaleAddViewModel.gradeScale.GradeScaleId && x.GradeScaleName.ToLower() == gradeScaleAddViewModel.gradeScale.GradeScaleName.ToLower()).FirstOrDefault();

                    if (checkGradeScaleName != null)
                    {
                        gradeScaleAddViewModel._failure = true;
                        gradeScaleAddViewModel._message = "Grade Scale Name Already Exists";
                    }
                    else
                    {
                        gradeScaleAddViewModel.gradeScale.CreatedBy = gradeScaleUpdate.CreatedBy;
                        gradeScaleAddViewModel.gradeScale.CreatedOn = gradeScaleUpdate.CreatedOn;
                        gradeScaleAddViewModel.gradeScale.UpdatedOn = DateTime.Now;
                        this.context.Entry(gradeScaleUpdate).CurrentValues.SetValues(gradeScaleAddViewModel.gradeScale);
                        this.context?.SaveChanges();
                        gradeScaleAddViewModel._failure = false;
                        gradeScaleAddViewModel._message = "Grade Scale Updated Successfully";
                    }                    
                }
                else
                {
                    gradeScaleAddViewModel.gradeScale = null;
                    gradeScaleAddViewModel._failure = true;
                    gradeScaleAddViewModel._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {

                gradeScaleAddViewModel._failure = true;
                gradeScaleAddViewModel._message = es.Message;
            }
            return gradeScaleAddViewModel;
        }
        /// <summary>
        /// Delete Grade Scale
        /// </summary>
        /// <param name="gradeScaleAddViewModel"></param>
        /// <returns></returns>
        public GradeScaleAddViewModel DeleteGradeScale(GradeScaleAddViewModel gradeScaleAddViewModel)
        {
            try
            {
                var gradeScaleDelete = this.context?.GradeScale.FirstOrDefault(x => x.TenantId == gradeScaleAddViewModel.gradeScale.TenantId && x.SchoolId == gradeScaleAddViewModel.gradeScale.SchoolId && x.GradeScaleId == gradeScaleAddViewModel.gradeScale.GradeScaleId);
                
                var gradeList = this.context?.Grade.Where(e => e.GradeScaleId == gradeScaleDelete.GradeScaleId && e.SchoolId == gradeScaleDelete.SchoolId && e.TenantId == gradeScaleDelete.TenantId).ToList();
                
                if (gradeScaleDelete!=null)
                {
                    if (gradeList.Count > 0)
                    {
                        gradeScaleAddViewModel._message = "It Has Associationship";
                        gradeScaleAddViewModel._failure = true;
                    }
                    else
                    {
                        this.context?.GradeScale.Remove(gradeScaleDelete);
                        this.context?.SaveChanges();
                        gradeScaleAddViewModel._failure = false;
                        gradeScaleAddViewModel._message = "Grade Scale Deleted Successfully";
                    }
                }
                else
                {
                    gradeScaleAddViewModel._failure = true;
                    gradeScaleAddViewModel._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {

                gradeScaleAddViewModel._failure = true;
                gradeScaleAddViewModel._message = es.Message;
            }
            return gradeScaleAddViewModel;
        }
        /// <summary>
        /// Add Grade
        /// </summary>
        /// <param name="gradeAddViewModel"></param>
        /// <returns></returns>
        public GradeAddViewModel AddGrade(GradeAddViewModel gradeAddViewModel)
        {
            try
            {
                var checkGradeTitle = this.context?.Grade.Where(x => x.SchoolId == gradeAddViewModel.grade.SchoolId && x.TenantId == gradeAddViewModel.grade.TenantId && x.GradeScaleId == gradeAddViewModel.grade.GradeScaleId && x.Title.ToLower()== gradeAddViewModel.grade.Title.ToLower()).FirstOrDefault();
                if (checkGradeTitle != null)
                {
                    gradeAddViewModel._failure = true;
                    gradeAddViewModel._message = "Grade Title Already Exists";
                }
                else
                {
                    int? GradeId = 1;
                    int? SortOrder = 1;

                    var gradeData = this.context?.Grade.Where(x => x.SchoolId == gradeAddViewModel.grade.SchoolId && x.TenantId == gradeAddViewModel.grade.TenantId).OrderByDescending(x => x.GradeId).FirstOrDefault();

                    if (gradeData != null)
                    {
                        GradeId = gradeData.GradeId + 1;
                    }
                    var gradeSortOrder = this.context?.Grade.Where(x => x.SchoolId == gradeAddViewModel.grade.SchoolId && x.TenantId == gradeAddViewModel.grade.TenantId && x.GradeScaleId == gradeAddViewModel.grade.GradeScaleId).OrderByDescending(x => x.SortOrder).FirstOrDefault();

                    if (gradeSortOrder != null)
                    {
                        SortOrder = gradeSortOrder.SortOrder + 1;
                    }
                    gradeAddViewModel.grade.GradeId = (int)GradeId;
                    gradeAddViewModel.grade.SortOrder = (int)SortOrder;
                    gradeAddViewModel.grade.CreatedOn = DateTime.UtcNow;
                    this.context?.Grade.Add(gradeAddViewModel.grade);
                    this.context?.SaveChanges();
                    gradeAddViewModel._failure = false;
                    gradeAddViewModel._message = "Grade Added Successfully";
                }                
            }
            catch (Exception es)
            {

                gradeAddViewModel._failure = true;
                gradeAddViewModel._message = es.Message;
            }
            return gradeAddViewModel;
        }
        /// <summary>
        /// Update Grade
        /// </summary>
        /// <param name="gradeAddViewModel"></param>
        /// <returns></returns>
        public GradeAddViewModel UpdateGrade(GradeAddViewModel gradeAddViewModel)
        {
            try
            {
                var gradeUpdate = this.context?.Grade.FirstOrDefault(x => x.TenantId == gradeAddViewModel.grade.TenantId && x.SchoolId == gradeAddViewModel.grade.SchoolId && x.GradeId == gradeAddViewModel.grade.GradeId);
                if (gradeUpdate != null)
                {
                    var checkGradeTitle = this.context?.Grade.Where(x => x.SchoolId == gradeAddViewModel.grade.SchoolId && x.TenantId == gradeAddViewModel.grade.TenantId && x.GradeScaleId == gradeAddViewModel.grade.GradeScaleId && x.GradeId != gradeAddViewModel.grade.GradeId && x.Title.ToLower() == gradeAddViewModel.grade.Title.ToLower()).FirstOrDefault();

                    if (checkGradeTitle != null)
                    {
                        gradeAddViewModel._failure = true;
                        gradeAddViewModel._message = "Grade Title Already Exists";
                    }
                    else
                    {
                        gradeAddViewModel.grade.CreatedBy = gradeUpdate.CreatedBy;
                        gradeAddViewModel.grade.CreatedOn = gradeUpdate.CreatedOn;
                        gradeAddViewModel.grade.SortOrder = gradeUpdate.SortOrder;
                        gradeAddViewModel.grade.UpdatedOn = DateTime.Now;
                        this.context.Entry(gradeUpdate).CurrentValues.SetValues(gradeAddViewModel.grade);                        
                        this.context?.SaveChanges();
                        gradeAddViewModel._failure = false;
                        gradeAddViewModel._message = "Grade Updated Successfully";
                    }                    
                }
                else
                {
                    gradeAddViewModel.grade = null;
                    gradeAddViewModel._failure = true;
                    gradeAddViewModel._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {

                gradeAddViewModel._failure = true;
                gradeAddViewModel._message = es.Message;
            }
            return gradeAddViewModel;
        }
        /// <summary>
        /// Delete Grade
        /// </summary>
        /// <param name="gradeAddViewModel"></param>
        /// <returns></returns>
        public GradeAddViewModel DeleteGrade(GradeAddViewModel gradeAddViewModel)
        {
            try
            {
                var gradeDelete = this.context?.Grade.FirstOrDefault(x => x.TenantId == gradeAddViewModel.grade.TenantId && x.SchoolId == gradeAddViewModel.grade.SchoolId && x.GradeId==gradeAddViewModel.grade.GradeId);
                
                if (gradeDelete != null)
                {
                        this.context?.Grade.Remove(gradeDelete);
                        this.context?.SaveChanges();
                        gradeAddViewModel._failure = false;
                        gradeAddViewModel._message = "Grade Deleted Successfully";                    
                }
                else
                {
                        gradeAddViewModel._failure = true;
                        gradeAddViewModel._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {
                gradeAddViewModel._failure = true;
                gradeAddViewModel._message = es.Message;
            }
            return gradeAddViewModel;
        }
        /// <summary>
        /// Get All GradeScale List
        /// </summary>
        /// <param name="gradeScaleListViewModel"></param>
        /// <returns></returns>
        public GradeScaleListViewModel GetAllGradeScaleList(GradeScaleListViewModel gradeScaleListViewModel)
        {
            GradeScaleListViewModel GradeScaleListModel = new GradeScaleListViewModel();
            try
            {

                var GradeScaleList = this.context?.GradeScale.Include(x => x.Grade).Where(e => e.TenantId == gradeScaleListViewModel.TenantId && e.SchoolId == gradeScaleListViewModel.SchoolId).OrderBy(e=>e.SortOrder).ToList();                
                
                if (GradeScaleList.Count>0)
                {
                    foreach (var GradeScale in GradeScaleList)
                    {
                        GradeScale.Grade = GradeScale.Grade.OrderBy(y => y.SortOrder).ToList();
                    }

                    GradeScaleListModel.gradeScaleList = GradeScaleList;
                    GradeScaleListModel._failure = false;
                }
                else
                {
                    GradeScaleListModel.gradeScaleList = GradeScaleList;
                    GradeScaleListModel._message = NORECORDFOUND;
                    GradeScaleListModel._failure = true;
                }
                GradeScaleListModel._tenantName = gradeScaleListViewModel._tenantName;
                GradeScaleListModel._token = gradeScaleListViewModel._token;
            }
            catch (Exception es)
            {
                GradeScaleListModel._message = es.Message;
                GradeScaleListModel._failure = true;
                GradeScaleListModel._tenantName = gradeScaleListViewModel._tenantName;
                GradeScaleListModel._token = gradeScaleListViewModel._token;
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
            try
            {
                var GradeRecords = new List<Grade>();

                var targetGrade = this.context?.Grade.FirstOrDefault(x => x.SortOrder == gradeSortOrderModel.PreviousSortOrder && x.SchoolId == gradeSortOrderModel.SchoolId && x.GradeScaleId == gradeSortOrderModel.GradeScaleId && x.TenantId== gradeSortOrderModel.TenantId);
                
                targetGrade.SortOrder = gradeSortOrderModel.CurrentSortOrder;

                if (gradeSortOrderModel.PreviousSortOrder > gradeSortOrderModel.CurrentSortOrder)
                {
                    GradeRecords = this.context?.Grade.Where(x => x.SortOrder >= gradeSortOrderModel.CurrentSortOrder && x.SortOrder < gradeSortOrderModel.PreviousSortOrder && x.SchoolId == gradeSortOrderModel.SchoolId && x.GradeScaleId == gradeSortOrderModel.GradeScaleId && x.TenantId == gradeSortOrderModel.TenantId).ToList();

                    if (GradeRecords.Count > 0)
                    {
                        GradeRecords.ForEach(x => x.SortOrder = x.SortOrder + 1);
                    }
                }
                if (gradeSortOrderModel.CurrentSortOrder > gradeSortOrderModel.PreviousSortOrder)
                {
                    GradeRecords = this.context?.Grade.Where(x => x.SortOrder <= gradeSortOrderModel.CurrentSortOrder && x.SortOrder > gradeSortOrderModel.PreviousSortOrder && x.SchoolId == gradeSortOrderModel.SchoolId && x.GradeScaleId == gradeSortOrderModel.GradeScaleId && x.TenantId == gradeSortOrderModel.TenantId).ToList();
                    if (GradeRecords.Count > 0)
                    {
                        GradeRecords.ForEach(x => x.SortOrder = x.SortOrder - 1);
                    }
                }
                this.context?.SaveChanges();
                gradeSortOrderModel._failure = false;
            }
            catch (Exception es)
            {
                gradeSortOrderModel._message = es.Message;
                gradeSortOrderModel._failure = true;
            }
            return gradeSortOrderModel;
        }
        /// <summary>
        /// Add EffortGrade Library Category
        /// </summary>
        /// <param name="effortGradeLibraryCategoryAddViewModel"></param>
        /// <returns></returns>
        public EffortGradeLibraryCategoryAddViewModel AddEffortGradeLibraryCategory(EffortGradeLibraryCategoryAddViewModel effortGradeLibraryCategoryAddViewModel)
        {
            try
            {
                var effortGradeLibraryCategoryList = this.context?.EffortGradeLibraryCategory.FirstOrDefault(x => x.SchoolId == effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory.SchoolId && x.TenantId == effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory.TenantId && x.CategoryName.ToLower() == effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory.CategoryName.ToLower());

                if (effortGradeLibraryCategoryList == null)
                {
                    int? EffortCategoryId = 1;
                    int? SortOrder = 1;

                    var effortGradeLibraryCategoryData = this.context?.EffortGradeLibraryCategory.Where(x => x.SchoolId == effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory.SchoolId && x.TenantId == effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory.TenantId).OrderByDescending(x => x.EffortCategoryId).FirstOrDefault();

                    if (effortGradeLibraryCategoryData != null)
                    {
                        EffortCategoryId = effortGradeLibraryCategoryData.EffortCategoryId + 1;
                    }
                    var effortGradeLibraryCategorySortOrder = this.context?.EffortGradeLibraryCategory.Where(x => x.SchoolId == effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory.SchoolId && x.TenantId == effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory.TenantId).OrderByDescending(x => x.SortOrder).FirstOrDefault();

                    if (effortGradeLibraryCategorySortOrder != null)
                    {
                        SortOrder = effortGradeLibraryCategorySortOrder.SortOrder + 1;
                    }

                    effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory.EffortCategoryId = (int)EffortCategoryId;
                    effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory.SortOrder = (int)SortOrder;
                    effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory.CreatedOn = DateTime.UtcNow;
                    this.context?.EffortGradeLibraryCategory.Add(effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory);
                    this.context?.SaveChanges();
                    effortGradeLibraryCategoryAddViewModel._failure = false;
                    effortGradeLibraryCategoryAddViewModel._message = "Effort Grade Library Category Added Successfully";
                }
                else
                {
                    effortGradeLibraryCategoryAddViewModel._failure = true;
                    effortGradeLibraryCategoryAddViewModel._message = "Effort Grade Library Category Name Already Exists";
                }
            }
            catch (Exception es)
            {

                effortGradeLibraryCategoryAddViewModel._failure = true;
                effortGradeLibraryCategoryAddViewModel._message = es.Message;
            }
            return effortGradeLibraryCategoryAddViewModel;
        }
        /// <summary>
        /// Update EffortGrade Library Category
        /// </summary>
        /// <param name="effortGradeLibraryCategoryAddViewModel"></param>
        /// <returns></returns>
        public EffortGradeLibraryCategoryAddViewModel UpdateEffortGradeLibraryCategory(EffortGradeLibraryCategoryAddViewModel effortGradeLibraryCategoryAddViewModel)
        {
            try
            {
                var EffortGradeLibraryCategoryUpdate = this.context?.EffortGradeLibraryCategory.FirstOrDefault(x => x.TenantId == effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory.TenantId && x.SchoolId == effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory.SchoolId && x.EffortCategoryId == effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory.EffortCategoryId);
                
                if (EffortGradeLibraryCategoryUpdate != null)
                {
                    var EffortGradeLibraryCategoryData= this.context?.EffortGradeLibraryCategory.FirstOrDefault(x => x.TenantId == effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory.TenantId && x.SchoolId == effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory.SchoolId && x.EffortCategoryId != effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory.EffortCategoryId && x.CategoryName.ToLower() == effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory.CategoryName.ToLower());

                    if (EffortGradeLibraryCategoryData!=null)
                    {
                        effortGradeLibraryCategoryAddViewModel._failure = true;
                        effortGradeLibraryCategoryAddViewModel._message = "Effort Grade Library Category Name Already Exists";
                    }
                    else
                    {
                        effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory.CreatedBy = EffortGradeLibraryCategoryUpdate.CreatedBy;
                        effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory.CreatedOn = EffortGradeLibraryCategoryUpdate.CreatedOn;
                        effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory.SortOrder = EffortGradeLibraryCategoryUpdate.SortOrder;
                        effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory.UpdatedOn = DateTime.Now;
                        this.context.Entry(EffortGradeLibraryCategoryUpdate).CurrentValues.SetValues(effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory);
                        this.context?.SaveChanges();
                        effortGradeLibraryCategoryAddViewModel._failure = false;
                        effortGradeLibraryCategoryAddViewModel._message = "Effort Grade Library Category Updated Successfully";
                    }                    
                }
                else
                {
                    effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory = null;
                    effortGradeLibraryCategoryAddViewModel._failure = true;
                    effortGradeLibraryCategoryAddViewModel._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {
                effortGradeLibraryCategoryAddViewModel._failure = true;
                effortGradeLibraryCategoryAddViewModel._message = es.Message;
            }
            return effortGradeLibraryCategoryAddViewModel;
        }
        /// <summary>
        /// Delete EffortGrade Library Category
        /// </summary>
        /// <param name="effortGradeLibraryCategoryAddViewModel"></param>
        /// <returns></returns>
        public EffortGradeLibraryCategoryAddViewModel DeleteEffortGradeLibraryCategory(EffortGradeLibraryCategoryAddViewModel effortGradeLibraryCategoryAddViewModel)
        {
            try
            {
                var EffortGradeLibraryCategoryDelete = this.context?.EffortGradeLibraryCategory.FirstOrDefault(x => x.TenantId == effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory.TenantId && x.SchoolId == effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory.SchoolId && x.EffortCategoryId == effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory.EffortCategoryId);

                if (EffortGradeLibraryCategoryDelete != null)
                {
                    var effortGradeLibraryCategoryItemExits = this.context?.EffortGradeLibraryCategoryItem.FirstOrDefault(e => e.TenantId == EffortGradeLibraryCategoryDelete.TenantId && e.SchoolId == EffortGradeLibraryCategoryDelete.SchoolId && e.EffortCategoryId == EffortGradeLibraryCategoryDelete.EffortCategoryId);
                    if (effortGradeLibraryCategoryItemExits!=null)
                    {
                        effortGradeLibraryCategoryAddViewModel._failure = true;
                        effortGradeLibraryCategoryAddViewModel._message = "Cannot be deleted because it has association.";
                    }
                    else
                    {
                        this.context?.EffortGradeLibraryCategory.Remove(EffortGradeLibraryCategoryDelete);
                        this.context?.SaveChanges();
                        effortGradeLibraryCategoryAddViewModel._failure = false;
                        effortGradeLibraryCategoryAddViewModel._message = "Effort Grade Library Category Deleted Successfully";
                    }
                    
                }
                else
                {
                    effortGradeLibraryCategoryAddViewModel._failure = true;
                    effortGradeLibraryCategoryAddViewModel._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {
                effortGradeLibraryCategoryAddViewModel._failure = true;
                effortGradeLibraryCategoryAddViewModel._message = es.Message;
            }
            return effortGradeLibraryCategoryAddViewModel;
        }
        /// <summary>
        /// Add EffortGrade Library Category Item
        /// </summary>
        /// <param name="effortGradeLibraryCategoryItemAddViewModel"></param>
        /// <returns></returns>
        public EffortGradeLibraryCategoryItemAddViewModel AddEffortGradeLibraryCategoryItem(EffortGradeLibraryCategoryItemAddViewModel effortGradeLibraryCategoryItemAddViewModel)
        {
            try
            {
                var effortGradeLibraryCategoryItemList = this.context?.EffortGradeLibraryCategoryItem.FirstOrDefault(x => x.SchoolId == effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.SchoolId && x.TenantId == effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.TenantId && x.EffortCategoryId == effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.EffortCategoryId && x.EffortItemTitle.ToLower() == effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.EffortItemTitle.ToLower());

                if (effortGradeLibraryCategoryItemList == null)
                {
                    int? EffortCategoryItemId = 1;
                    int? SortOrder = 1;

                    var effortGradeLibraryCategoryItemData = this.context?.EffortGradeLibraryCategoryItem.Where(x => x.SchoolId == effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.SchoolId && x.TenantId == effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.TenantId).OrderByDescending(x => x.EffortItemId).FirstOrDefault();

                    if (effortGradeLibraryCategoryItemData != null)
                    {
                        EffortCategoryItemId = effortGradeLibraryCategoryItemData.EffortItemId + 1;
                    }

                    var effortGradeLibraryCategoryItemSortOrder = this.context?.EffortGradeLibraryCategoryItem.Where(x => x.SchoolId == effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.SchoolId && x.TenantId == effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.TenantId && x.EffortCategoryId == effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.EffortCategoryId).OrderByDescending(x => x.SortOrder).FirstOrDefault();

                    if (effortGradeLibraryCategoryItemSortOrder != null)
                    {
                        SortOrder = effortGradeLibraryCategoryItemSortOrder.SortOrder + 1;
                    }

                    effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.EffortItemId = (int)EffortCategoryItemId;
                    effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.SortOrder = (int)SortOrder;
                    effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.CreatedOn = DateTime.UtcNow;
                    this.context?.EffortGradeLibraryCategoryItem.Add(effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem);
                    this.context?.SaveChanges();
                    effortGradeLibraryCategoryItemAddViewModel._failure = false;
                    effortGradeLibraryCategoryItemAddViewModel._message = "Effort Grade Library Category Item Added Successfully";
                }
                else
                {
                    effortGradeLibraryCategoryItemAddViewModel._failure = true;
                    effortGradeLibraryCategoryItemAddViewModel._message = "Effort Grade Library Category Item Name Already Exists";
                }                
            }
            catch (Exception es)
            {

                effortGradeLibraryCategoryItemAddViewModel._failure = true;
                effortGradeLibraryCategoryItemAddViewModel._message = es.Message;
            }
            return effortGradeLibraryCategoryItemAddViewModel;
        }
        /// <summary>
        /// Update EffortGrade Library Category Item
        /// </summary>
        /// <param name="effortGradeLibraryCategoryItemAddViewModel"></param>
        /// <returns></returns>
        public EffortGradeLibraryCategoryItemAddViewModel UpdateEffortGradeLibraryCategoryItem(EffortGradeLibraryCategoryItemAddViewModel effortGradeLibraryCategoryItemAddViewModel)
        {
            try
            {
                var EffortGradeLibraryCategoryItemUpdate = this.context?.EffortGradeLibraryCategoryItem.FirstOrDefault(x => x.TenantId == effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.TenantId && x.SchoolId == effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.SchoolId && x.EffortItemId == effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.EffortItemId);
                if (EffortGradeLibraryCategoryItemUpdate != null)
                {
                    var effortGradeLibraryCategoryItemList = this.context?.EffortGradeLibraryCategoryItem.FirstOrDefault(x => x.SchoolId == effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.SchoolId && x.TenantId == effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.TenantId  && x.EffortCategoryId == effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.EffortCategoryId && x.EffortItemId!= EffortGradeLibraryCategoryItemUpdate.EffortItemId && x.EffortItemTitle.ToLower() == effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.EffortItemTitle.ToLower());
                    if (effortGradeLibraryCategoryItemList !=null)
                    {
                        effortGradeLibraryCategoryItemAddViewModel._failure = true;
                        effortGradeLibraryCategoryItemAddViewModel._message = "Effort Grade Library Category Item Name Already Exists";
                    }
                    else
                    {
                        effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.CreatedBy = EffortGradeLibraryCategoryItemUpdate.CreatedBy;
                        effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.CreatedOn = EffortGradeLibraryCategoryItemUpdate.CreatedOn;
                        effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.SortOrder = EffortGradeLibraryCategoryItemUpdate.SortOrder;
                        effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.UpdatedOn = DateTime.Now;
                        this.context.Entry(EffortGradeLibraryCategoryItemUpdate).CurrentValues.SetValues(effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem);

                        this.context?.SaveChanges();
                        effortGradeLibraryCategoryItemAddViewModel._failure = false;
                        effortGradeLibraryCategoryItemAddViewModel._message = "Effort Grade Library Category Item Updated Successfully";
                    }                    
                }
                else
                {
                    effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem = null;
                    effortGradeLibraryCategoryItemAddViewModel._failure = true;
                    effortGradeLibraryCategoryItemAddViewModel._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {

                effortGradeLibraryCategoryItemAddViewModel._failure = true;
                effortGradeLibraryCategoryItemAddViewModel._message = es.Message;
            }
            return effortGradeLibraryCategoryItemAddViewModel;
        }
        /// <summary>
        /// Delete EffortGrade Library Category Item
        /// </summary>
        /// <param name="effortGradeLibraryCategoryItemAddViewModel"></param>
        /// <returns></returns>
        public EffortGradeLibraryCategoryItemAddViewModel DeleteEffortGradeLibraryCategoryItem(EffortGradeLibraryCategoryItemAddViewModel effortGradeLibraryCategoryItemAddViewModel)
        {
            try
            {
                var EffortGradeLibraryCategoryItemDelete = this.context?.EffortGradeLibraryCategoryItem.FirstOrDefault(x => x.TenantId == effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.TenantId && x.SchoolId == effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.SchoolId && x.EffortItemId == effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.EffortItemId);
                
                if (EffortGradeLibraryCategoryItemDelete != null)
                {
                    this.context?.EffortGradeLibraryCategoryItem.Remove(EffortGradeLibraryCategoryItemDelete);
                    this.context?.SaveChanges();
                    effortGradeLibraryCategoryItemAddViewModel._failure = false;
                    effortGradeLibraryCategoryItemAddViewModel._message = "Effort Grade Library Category Item Deleted Successfully";
                }
                else
                {
                    effortGradeLibraryCategoryItemAddViewModel._failure = true;
                    effortGradeLibraryCategoryItemAddViewModel._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {

                effortGradeLibraryCategoryItemAddViewModel._failure = true;
                effortGradeLibraryCategoryItemAddViewModel._message = es.Message;
            }
            return effortGradeLibraryCategoryItemAddViewModel;
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

                var effortGradeLlibraryCategoryList = this.context?.EffortGradeLibraryCategory.Include(x => x.EffortGradeLibraryCategoryItem).Where(e => e.TenantId == effortGradeLlibraryCategoryListViewModel.TenantId && e.SchoolId == effortGradeLlibraryCategoryListViewModel.SchoolId).OrderBy(e => e.SortOrder)
                    .Select(p => new EffortGradeLibraryCategory()
                    {
                        SchoolId = p.SchoolId,
                        TenantId = p.TenantId,
                        EffortCategoryId = p.EffortCategoryId,
                        CategoryName=p.CategoryName,
                        CreatedBy=p.CreatedBy,
                        CreatedOn=p.CreatedOn,
                        SortOrder = p.SortOrder,
                        UpdatedBy = p.UpdatedBy,
                        UpdatedOn=p.UpdatedOn,
                        EffortGradeLibraryCategoryItem = p.EffortGradeLibraryCategoryItem.OrderBy(c => c.SortOrder).ToList() 
                    }).ToList();
                if(effortGradeLlibraryCategoryList.Count >0)
                {
                    effortGradeLlibraryCategoryListModel.effortGradeLibraryCategoryList = effortGradeLlibraryCategoryList;
                    effortGradeLlibraryCategoryListModel._failure = false;
                }
                else
                {
                    effortGradeLlibraryCategoryListModel.effortGradeLibraryCategoryList = effortGradeLlibraryCategoryList;
                    effortGradeLlibraryCategoryListModel._failure = true;
                    effortGradeLlibraryCategoryListModel._message = NORECORDFOUND;
                }
                effortGradeLlibraryCategoryListModel._tenantName = effortGradeLlibraryCategoryListViewModel._tenantName;
                effortGradeLlibraryCategoryListModel._token = effortGradeLlibraryCategoryListViewModel._token;
            }
            catch (Exception es)
            {
                effortGradeLlibraryCategoryListModel._message = es.Message;
                effortGradeLlibraryCategoryListModel._failure = true;
                effortGradeLlibraryCategoryListModel._tenantName = effortGradeLlibraryCategoryListViewModel._tenantName;
                effortGradeLlibraryCategoryListModel._token = effortGradeLlibraryCategoryListViewModel._token;
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
            try
            {
                if( effortgradeLibraryCategorySortOrderModel.EffortCategoryId>0)
                {
                    var EffortGradeLibraryCategoryItemRecords = new List<EffortGradeLibraryCategoryItem>();

                    var targetEffortGradeLibraryCategoryItem = this.context?.EffortGradeLibraryCategoryItem.FirstOrDefault(x => x.SortOrder == effortgradeLibraryCategorySortOrderModel.PreviousSortOrder && x.SchoolId == effortgradeLibraryCategorySortOrderModel.SchoolId && x.TenantId == effortgradeLibraryCategorySortOrderModel.TenantId && x.EffortCategoryId == effortgradeLibraryCategorySortOrderModel.EffortCategoryId);

                    if (targetEffortGradeLibraryCategoryItem != null)
                    {
                        targetEffortGradeLibraryCategoryItem.SortOrder = effortgradeLibraryCategorySortOrderModel.CurrentSortOrder;

                        if (effortgradeLibraryCategorySortOrderModel.PreviousSortOrder > effortgradeLibraryCategorySortOrderModel.CurrentSortOrder)
                        {
                            EffortGradeLibraryCategoryItemRecords = this.context?.EffortGradeLibraryCategoryItem.Where(x => x.SortOrder >= effortgradeLibraryCategorySortOrderModel.CurrentSortOrder && x.SortOrder < effortgradeLibraryCategorySortOrderModel.PreviousSortOrder && x.SchoolId == effortgradeLibraryCategorySortOrderModel.SchoolId && x.TenantId == effortgradeLibraryCategorySortOrderModel.TenantId && x.EffortCategoryId == effortgradeLibraryCategorySortOrderModel.EffortCategoryId).ToList();

                            if (EffortGradeLibraryCategoryItemRecords.Count > 0)
                            {
                                EffortGradeLibraryCategoryItemRecords.ForEach(x => x.SortOrder = x.SortOrder + 1);
                            }
                        }
                        if (effortgradeLibraryCategorySortOrderModel.CurrentSortOrder > effortgradeLibraryCategorySortOrderModel.PreviousSortOrder)
                        {
                            EffortGradeLibraryCategoryItemRecords = this.context?.EffortGradeLibraryCategoryItem.Where(x => x.SortOrder <= effortgradeLibraryCategorySortOrderModel.CurrentSortOrder && x.SortOrder > effortgradeLibraryCategorySortOrderModel.PreviousSortOrder && x.SchoolId == effortgradeLibraryCategorySortOrderModel.SchoolId && x.TenantId == effortgradeLibraryCategorySortOrderModel.TenantId && x.EffortCategoryId == effortgradeLibraryCategorySortOrderModel.EffortCategoryId).ToList();
                            if (EffortGradeLibraryCategoryItemRecords.Count > 0)
                            {
                                EffortGradeLibraryCategoryItemRecords.ForEach(x => x.SortOrder = x.SortOrder - 1);
                            }
                        }
                    }

                }                
                else
                {
                    var EffortGradeLibraryCategoryRecords = new List<EffortGradeLibraryCategory>();

                    var targetEffortGradeLibraryCategory = this.context?.EffortGradeLibraryCategory.FirstOrDefault(x => x.SortOrder == effortgradeLibraryCategorySortOrderModel.PreviousSortOrder && x.SchoolId == effortgradeLibraryCategorySortOrderModel.SchoolId && x.TenantId == effortgradeLibraryCategorySortOrderModel.TenantId);
                    if (targetEffortGradeLibraryCategory != null)
                    {
                        targetEffortGradeLibraryCategory.SortOrder = effortgradeLibraryCategorySortOrderModel.CurrentSortOrder;

                        if (effortgradeLibraryCategorySortOrderModel.PreviousSortOrder > effortgradeLibraryCategorySortOrderModel.CurrentSortOrder)
                        {
                            EffortGradeLibraryCategoryRecords = this.context?.EffortGradeLibraryCategory.Where(x => x.SortOrder >= effortgradeLibraryCategorySortOrderModel.CurrentSortOrder && x.SortOrder < effortgradeLibraryCategorySortOrderModel.PreviousSortOrder && x.SchoolId == effortgradeLibraryCategorySortOrderModel.SchoolId && x.TenantId == effortgradeLibraryCategorySortOrderModel.TenantId).ToList();

                            if (EffortGradeLibraryCategoryRecords.Count > 0)
                            {
                                EffortGradeLibraryCategoryRecords.ForEach(x => x.SortOrder = x.SortOrder + 1);
                            }
                        }
                        if (effortgradeLibraryCategorySortOrderModel.CurrentSortOrder > effortgradeLibraryCategorySortOrderModel.PreviousSortOrder)
                        {
                            EffortGradeLibraryCategoryRecords = this.context?.EffortGradeLibraryCategory.Where(x => x.SortOrder <= effortgradeLibraryCategorySortOrderModel.CurrentSortOrder && x.SortOrder > effortgradeLibraryCategorySortOrderModel.PreviousSortOrder && x.SchoolId == effortgradeLibraryCategorySortOrderModel.SchoolId && x.TenantId == effortgradeLibraryCategorySortOrderModel.TenantId).ToList();
                            if (EffortGradeLibraryCategoryRecords.Count > 0)
                            {
                                EffortGradeLibraryCategoryRecords.ForEach(x => x.SortOrder = x.SortOrder - 1);
                            }
                        }
                    }

                }
                this.context?.SaveChanges();
                effortgradeLibraryCategorySortOrderModel._failure = false;
            }
            catch (Exception es)
            {
                effortgradeLibraryCategorySortOrderModel._message = es.Message;
                effortgradeLibraryCategorySortOrderModel._failure = true;
            }
            return effortgradeLibraryCategorySortOrderModel;
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
                var gradeScaleValue = this.context?.EffortGradeScale.FirstOrDefault(x => x.TenantId == effortGradeScaleAddViewModel.effortGradeScale.TenantId && x.SchoolId == effortGradeScaleAddViewModel.effortGradeScale.SchoolId && x.GradeScaleValue.ToString() == effortGradeScaleAddViewModel.effortGradeScale.GradeScaleValue.ToString());

                if (gradeScaleValue == null)
                {
                    int? effortGradeScaleId = 1;
                    int? SortOrder = 1;

                    var effortGradeScaleData = this.context?.EffortGradeScale.Where(x => x.TenantId == effortGradeScaleAddViewModel.effortGradeScale.TenantId && x.SchoolId == effortGradeScaleAddViewModel.effortGradeScale.SchoolId).OrderByDescending(x => x.EffortGradeScaleId).FirstOrDefault();

                    if (effortGradeScaleData != null)
                    {
                        effortGradeScaleId = effortGradeScaleData.EffortGradeScaleId + 1;
                    }
                    var sortOrderData = this.context?.EffortGradeScale.Where(x => x.TenantId == effortGradeScaleAddViewModel.effortGradeScale.TenantId && x.SchoolId == effortGradeScaleAddViewModel.effortGradeScale.SchoolId).OrderByDescending(x => x.SortOrder).FirstOrDefault();

                    if (sortOrderData != null)
                    {
                        SortOrder = sortOrderData.SortOrder + 1;
                    }

                    effortGradeScaleAddViewModel.effortGradeScale.EffortGradeScaleId = (int)effortGradeScaleId;
                    effortGradeScaleAddViewModel.effortGradeScale.SortOrder = (int)SortOrder;
                    effortGradeScaleAddViewModel.effortGradeScale.CreatedOn = DateTime.UtcNow;
                    this.context?.EffortGradeScale.Add(effortGradeScaleAddViewModel.effortGradeScale);
                    this.context?.SaveChanges();
                    effortGradeScaleAddViewModel._failure = false;
                    effortGradeScaleAddViewModel._message = "Effort Grade Scale Added Successfully";
                }
                else
                {
                    effortGradeScaleAddViewModel._failure = true;
                    effortGradeScaleAddViewModel._message = "GradeScaleValue Already Exits";
                }
            }
            catch (Exception es)
            {
                effortGradeScaleAddViewModel._failure = true;
                effortGradeScaleAddViewModel._message = es.Message;
            }
            return effortGradeScaleAddViewModel;
        }

        /// <summary>
        /// Update EffortGradeScale
        /// </summary>
        /// <param name="effortGradeScaleAddViewModel"></param>
        /// <returns></returns>
        public EffortGradeScaleAddViewModel UpdateEffortGradeScale(EffortGradeScaleAddViewModel effortGradeScaleAddViewModel)
        {
            try
            {
                var effortGradeScaleUpdate = this.context?.EffortGradeScale.FirstOrDefault(x => x.TenantId == effortGradeScaleAddViewModel.effortGradeScale.TenantId && x.SchoolId == effortGradeScaleAddViewModel.effortGradeScale.SchoolId && x.EffortGradeScaleId == effortGradeScaleAddViewModel.effortGradeScale.EffortGradeScaleId);

                if (effortGradeScaleUpdate != null)
                {
                    var gradeScaleValue = this.context?.EffortGradeScale.FirstOrDefault(x => x.TenantId == effortGradeScaleAddViewModel.effortGradeScale.TenantId && x.SchoolId == effortGradeScaleAddViewModel.effortGradeScale.SchoolId && x.GradeScaleValue.ToString() == effortGradeScaleAddViewModel.effortGradeScale.GradeScaleValue.ToString() && x.EffortGradeScaleId != effortGradeScaleUpdate.EffortGradeScaleId);

                    if (gradeScaleValue == null)
                    {
                        effortGradeScaleAddViewModel.effortGradeScale.CreatedBy = effortGradeScaleUpdate.CreatedBy;
                        effortGradeScaleAddViewModel.effortGradeScale.CreatedOn = effortGradeScaleUpdate.CreatedOn;
                        effortGradeScaleAddViewModel.effortGradeScale.SortOrder = effortGradeScaleUpdate.SortOrder;
                        effortGradeScaleAddViewModel.effortGradeScale.UpdatedOn = DateTime.Now;
                        this.context.Entry(effortGradeScaleUpdate).CurrentValues.SetValues(effortGradeScaleAddViewModel.effortGradeScale);
                        this.context?.SaveChanges();
                        effortGradeScaleAddViewModel._failure = false;
                        effortGradeScaleAddViewModel._message = "Effort Grade Scale Updated Successfully";
                    }
                    else
                    {
                        effortGradeScaleAddViewModel._failure = true;
                        effortGradeScaleAddViewModel._message = "GradeScaleValue Already Exits";
                    }
                }
                else
                {
                    effortGradeScaleAddViewModel.effortGradeScale = null;
                    effortGradeScaleAddViewModel._message = NORECORDFOUND;
                    effortGradeScaleAddViewModel._failure = true;
                }
            }
            catch (Exception es)
            {
                effortGradeScaleAddViewModel._failure = true;
                effortGradeScaleAddViewModel._message = es.Message;
            }
            return effortGradeScaleAddViewModel;
        }

        /// <summary>
        /// Delete EffortGradeScale
        /// </summary>
        /// <param name="effortGradeScaleAddViewModel"></param>
        /// <returns></returns>
        public EffortGradeScaleAddViewModel DeleteEffortGradeScale(EffortGradeScaleAddViewModel effortGradeScaleAddViewModel)
        {
            try
            {
                var effortGradeScaleDelete = this.context?.EffortGradeScale.FirstOrDefault(x => x.TenantId == effortGradeScaleAddViewModel.effortGradeScale.TenantId && x.SchoolId == effortGradeScaleAddViewModel.effortGradeScale.SchoolId && x.EffortGradeScaleId == effortGradeScaleAddViewModel.effortGradeScale.EffortGradeScaleId);

                if (effortGradeScaleDelete != null)
                {
                    this.context?.EffortGradeScale.Remove(effortGradeScaleDelete);
                    this.context?.SaveChanges();
                    effortGradeScaleAddViewModel._failure = false;
                    effortGradeScaleAddViewModel._message = "Effort Grade Scale Deleted Successfully";
                }
                else
                {
                    effortGradeScaleAddViewModel._message = NORECORDFOUND;
                    effortGradeScaleAddViewModel._failure = true;
                }
            }
            catch (Exception es)
            {
                effortGradeScaleAddViewModel._failure = true;
                effortGradeScaleAddViewModel._message = es.Message;
            }
            return effortGradeScaleAddViewModel;
        }

        /// <summary>
        /// Get All EffortGradeScale
        /// </summary>
        /// <param name="pageResult"></param>
        /// <returns></returns>
        public EffortGradeScaleListModel GetAllEffortGradeScale(PageResult pageResult)
        {
            EffortGradeScaleListModel effortGradeScaleList = new EffortGradeScaleListModel();
            IQueryable<EffortGradeScale> transactionIQ = null;

            var effortGradeScaleData = this.context?.EffortGradeScale.Where(x => x.TenantId == pageResult.TenantId && x.SchoolId==pageResult.SchoolId);
            try
            {
                if (pageResult.FilterParams == null || pageResult.FilterParams.Count == 0)
                {
                    transactionIQ = effortGradeScaleData;
                }
                else
                {
                    if (pageResult.FilterParams != null && pageResult.FilterParams.ElementAt(0).ColumnName == null && pageResult.FilterParams.Count == 1)
                    {
                        string Columnvalue = pageResult.FilterParams.ElementAt(0).FilterValue;
                        transactionIQ = effortGradeScaleData.Where(x => x.GradeScaleValue != null && x.GradeScaleValue.ToString() == Columnvalue.ToString() || x.GradeScaleComment != null && x.GradeScaleComment.ToLower().Contains(Columnvalue.ToLower()));
                    }
                    else
                    {
                        transactionIQ = Utility.FilteredData(pageResult.FilterParams, effortGradeScaleData).AsQueryable();
                    }
                }
                if (pageResult.SortingModel != null)
                {
                    transactionIQ = Utility.Sort(transactionIQ, pageResult.SortingModel.SortColumn, pageResult.SortingModel.SortDirection.ToLower());
                }
                int totalCount = transactionIQ.Count();
                if (pageResult.PageNumber > 0 && pageResult.PageSize > 0)
                {
                    transactionIQ = transactionIQ.Skip((pageResult.PageNumber - 1) * pageResult.PageSize).Take(pageResult.PageSize);
                }
                //var effortList = transactionIQ.AsNoTracking().Select(s => new GetEffortGradeScaleForView
                //{
                //    TenantId = s.TenantId,
                //    SchoolId = s.SchoolId,
                //    EffortGradeScaleId = s.EffortGradeScaleId,
                //    GradeScaleValue = s.GradeScaleValue,
                //    GradeScaleComment = s.GradeScaleComment,
                //    SortOrder = s.SortOrder
                //}).ToList();

                effortGradeScaleList.TenantId = pageResult.TenantId;
                effortGradeScaleList.SchoolId = pageResult.SchoolId;
                effortGradeScaleList.effortGradeScaleList = transactionIQ.ToList();
                //effortGradeScaleList.getEffortGradeScaleForView = effortList;
                effortGradeScaleList.TotalCount = totalCount;
                effortGradeScaleList.PageNumber = pageResult.PageNumber;
                effortGradeScaleList._pageSize = pageResult.PageSize;
                effortGradeScaleList._tenantName = pageResult._tenantName;
                effortGradeScaleList._token = pageResult._token;
                effortGradeScaleList._failure = false;
            }
            catch (Exception es)
            {
                effortGradeScaleList._message = es.Message;
                effortGradeScaleList._failure = true;
                effortGradeScaleList._tenantName = pageResult._tenantName;
                effortGradeScaleList._token = pageResult._token;
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
            try
            {
                var effortGradeScaleRecords = new List<EffortGradeScale>();

                var targetEffortGradeScale = this.context?.EffortGradeScale.FirstOrDefault(x => x.SortOrder == effortGradeScaleSortOrderViewModel.PreviousSortOrder && x.SchoolId == effortGradeScaleSortOrderViewModel.SchoolId && x.TenantId == effortGradeScaleSortOrderViewModel.TenantId);
                targetEffortGradeScale.SortOrder = effortGradeScaleSortOrderViewModel.CurrentSortOrder;

                if (effortGradeScaleSortOrderViewModel.PreviousSortOrder > effortGradeScaleSortOrderViewModel.CurrentSortOrder)
                {
                    effortGradeScaleRecords = this.context?.EffortGradeScale.Where(x => x.SortOrder >= effortGradeScaleSortOrderViewModel.CurrentSortOrder && x.SortOrder < effortGradeScaleSortOrderViewModel.PreviousSortOrder && x.TenantId == effortGradeScaleSortOrderViewModel.TenantId && x.SchoolId == effortGradeScaleSortOrderViewModel.SchoolId).ToList();

                    if (effortGradeScaleRecords.Count > 0)
                    {
                        effortGradeScaleRecords.ForEach(x => x.SortOrder = x.SortOrder + 1);
                    }
                }
                if (effortGradeScaleSortOrderViewModel.CurrentSortOrder > effortGradeScaleSortOrderViewModel.PreviousSortOrder)
                {
                    effortGradeScaleRecords = this.context?.EffortGradeScale.Where(x => x.SortOrder <= effortGradeScaleSortOrderViewModel.CurrentSortOrder && x.SortOrder > effortGradeScaleSortOrderViewModel.PreviousSortOrder && x.SchoolId == effortGradeScaleSortOrderViewModel.SchoolId && x.TenantId == effortGradeScaleSortOrderViewModel.TenantId).ToList();
                    if (effortGradeScaleRecords.Count > 0)
                    {
                        effortGradeScaleRecords.ForEach(x => x.SortOrder = x.SortOrder - 1);
                    }
                }
                this.context?.SaveChanges();
                effortGradeScaleSortOrderViewModel._failure = false;
            }
            catch (Exception es)
            {
                effortGradeScaleSortOrderViewModel._message = es.Message;
                effortGradeScaleSortOrderViewModel._failure = true;
            }
            return effortGradeScaleSortOrderViewModel;
        }

        /// <summary>
        /// Add GradeUsStandard
        /// </summary>
        /// <param name="gradeUsStandardAddViewModel"></param>
        /// <returns></returns>
        public GradeUsStandardAddViewModel AddGradeUsStandard(GradeUsStandardAddViewModel gradeUsStandardAddViewModel)
        {
            try
            {
                bool validStandardRefNo = CheckStandardRefNo(gradeUsStandardAddViewModel.gradeUsStandard.TenantId, gradeUsStandardAddViewModel.gradeUsStandard.StandardRefNo);
                if (validStandardRefNo == true)
                {
                    int? GradeStandardId = 1;
                    var gradeUsStandardData = this.context?.GradeUsStandard.Where(x => x.TenantId == gradeUsStandardAddViewModel.gradeUsStandard.TenantId && x.SchoolId == gradeUsStandardAddViewModel.gradeUsStandard.SchoolId).OrderByDescending(x => x.GradeStandardId).FirstOrDefault();

                    if (gradeUsStandardData != null)
                    {
                        GradeStandardId = gradeUsStandardData.GradeStandardId + 1;
                    }
                    gradeUsStandardAddViewModel.gradeUsStandard.GradeStandardId = (int)GradeStandardId;
                    gradeUsStandardAddViewModel.gradeUsStandard.CreatedOn = DateTime.UtcNow;                    
                    gradeUsStandardAddViewModel.gradeUsStandard.IsSchoolSpecific = true;
                    this.context?.GradeUsStandard.Add(gradeUsStandardAddViewModel.gradeUsStandard);
                    this.context?.SaveChanges();
                    gradeUsStandardAddViewModel._failure = false;
                    gradeUsStandardAddViewModel._message = "GradeUs Standard Added Successfully";
                }
                else
                {
                    gradeUsStandardAddViewModel._failure = true;
                    gradeUsStandardAddViewModel._message = "StandardRefNo Already Exits";
                }
            }
            catch (Exception es)
            {
                gradeUsStandardAddViewModel._failure = true;
                gradeUsStandardAddViewModel._message = es.Message;
            }
            return gradeUsStandardAddViewModel;
        }

        //Checking StandardRefNo is Exits or not.
        private bool CheckStandardRefNo(Guid TenantId,string StandardRefNo)
        {
            if (StandardRefNo != null && StandardRefNo != "")
            {
                var checkStandardRefNo = this.context?.GradeUsStandard.Where(x => x.TenantId == TenantId && x.StandardRefNo == StandardRefNo).ToList();
                if (checkStandardRefNo.Count() > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Update GradeUsStandard
        /// </summary>
        /// <param name="gradeUsStandardAddViewModel"></param>
        /// <returns></returns>
        public GradeUsStandardAddViewModel UpdateGradeUsStandard(GradeUsStandardAddViewModel gradeUsStandardAddViewModel)
        {
            try
            {
                var gradeUsStandardUpdate = this.context?.GradeUsStandard.FirstOrDefault(x => x.TenantId == gradeUsStandardAddViewModel.gradeUsStandard.TenantId && x.SchoolId == gradeUsStandardAddViewModel.gradeUsStandard.SchoolId && x.StandardRefNo == gradeUsStandardAddViewModel.gradeUsStandard.StandardRefNo);
                if (gradeUsStandardUpdate != null)
                {
                    gradeUsStandardAddViewModel.gradeUsStandard.CreatedBy = gradeUsStandardUpdate.CreatedBy;
                    gradeUsStandardAddViewModel.gradeUsStandard.CreatedOn = gradeUsStandardUpdate.CreatedOn;
                    gradeUsStandardAddViewModel.gradeUsStandard.UpdatedOn = DateTime.Now;
                    gradeUsStandardAddViewModel.gradeUsStandard.IsSchoolSpecific = true;
                    this.context.Entry(gradeUsStandardUpdate).CurrentValues.SetValues(gradeUsStandardAddViewModel.gradeUsStandard);
                    this.context?.SaveChanges();
                    gradeUsStandardAddViewModel._failure = false;
                    gradeUsStandardAddViewModel._message = "GradeUs Standard Updated Successfully";
                }
                else
                {
                    gradeUsStandardAddViewModel.gradeUsStandard = null;
                    gradeUsStandardAddViewModel._message = NORECORDFOUND;
                    gradeUsStandardAddViewModel._failure = true;
                }
            }
            catch (Exception es)
            {
                gradeUsStandardAddViewModel._failure = true;
                gradeUsStandardAddViewModel._message = es.Message;
            }
            return gradeUsStandardAddViewModel;
        }

        /// <summary>
        /// Delete GradeUsStandard
        /// </summary>
        /// <param name="gradeUsStandardAddViewModel"></param>
        /// <returns></returns>
        public GradeUsStandardAddViewModel DeleteGradeUsStandard(GradeUsStandardAddViewModel gradeUsStandardAddViewModel)
        {
            try
            {
                var gradeUsStandardDelete = this.context?.GradeUsStandard.FirstOrDefault(x => x.TenantId == gradeUsStandardAddViewModel.gradeUsStandard.TenantId && x.SchoolId == gradeUsStandardAddViewModel.gradeUsStandard.SchoolId && x.StandardRefNo == gradeUsStandardAddViewModel.gradeUsStandard.StandardRefNo);

                if (gradeUsStandardDelete != null)
                {
                    this.context?.GradeUsStandard.Remove(gradeUsStandardDelete);
                    this.context?.SaveChanges();
                    gradeUsStandardAddViewModel._failure = false;
                    gradeUsStandardAddViewModel._message = "GradeUs Standard Deleted Successfully";
                }
                else
                {
                    gradeUsStandardAddViewModel._message = NORECORDFOUND;
                    gradeUsStandardAddViewModel._failure = true;
                }
            }
            catch (Exception es)
            {
                gradeUsStandardAddViewModel._failure = true;
                gradeUsStandardAddViewModel._message = es.Message;
            }
            return gradeUsStandardAddViewModel;
        }

        /// <summary>
        /// Get All GradeUsStandard List
        /// </summary>
        /// <param name="pageResult"></param>
        /// <returns></returns>
        public GradeUsStandardListModel GetAllGradeUsStandardList(PageResult pageResult)
        {
            GradeUsStandardListModel gradeUsStandardList = new GradeUsStandardListModel();
            IQueryable<GradeUsStandard> transactionIQ = null;

            var gradeUsStandardData = this.context?.GradeUsStandard.Where(x => x.TenantId == pageResult.TenantId && x.SchoolId == pageResult.SchoolId && x.IsSchoolSpecific == true);
            try
            {
                if (pageResult.FilterParams == null || pageResult.FilterParams.Count == 0)
                {
                    transactionIQ = gradeUsStandardData;
                }
                else
                {
                    if (pageResult.FilterParams != null && pageResult.FilterParams.ElementAt(0).ColumnName == null && pageResult.FilterParams.Count == 1)
                    {
                        string Columnvalue = pageResult.FilterParams.ElementAt(0).FilterValue;
                        transactionIQ = gradeUsStandardData.Where(x => x.StandardRefNo != null && x.StandardRefNo.ToLower().Contains(Columnvalue.ToLower()) || x.GradeLevel != null && x.GradeLevel.ToLower().Contains(Columnvalue.ToLower()) || x.Domain != null && x.Domain.ToLower().Contains(Columnvalue.ToLower()) || x.Subject != null && x.Subject.ToLower().Contains(Columnvalue.ToLower()) || x.Course != null && x.Course.ToLower().Contains(Columnvalue.ToLower()) || x.Topic != null && x.Topic.ToLower().Contains(Columnvalue.ToLower()) || x.StandardDetails != null && x.StandardDetails.ToLower().Contains(Columnvalue.ToLower()));
                    }
                    else
                    {
                        if (pageResult.FilterParams.Count == 3 && pageResult.FilterParams.ElementAt(0).ColumnName.ToLower() == "subject" && pageResult.FilterParams.ElementAt(1).ColumnName.ToLower() == "course" && pageResult.FilterParams.ElementAt(2).ColumnName.ToLower() == "gradelevel")
                        {
                            transactionIQ = gradeUsStandardData.Where(x => x.TenantId == pageResult.TenantId && (pageResult.FilterParams.ElementAt(0).FilterValue == null || (x.Subject == pageResult.FilterParams.ElementAt(0).FilterValue)) && (pageResult.FilterParams.ElementAt(1).FilterValue == null || (x.Course == pageResult.FilterParams.ElementAt(1).FilterValue)) && (pageResult.FilterParams.ElementAt(2).FilterValue == null || (x.GradeLevel == pageResult.FilterParams.ElementAt(2).FilterValue)));                     
                        }

                        //if (pageResult.FilterParams.Count == 3 && pageResult.FilterParams.ElementAt(0).ColumnName.ToLower() == "subject" && pageResult.FilterParams.ElementAt(1).ColumnName.ToLower() == "course" && pageResult.FilterParams.ElementAt(2).ColumnName.ToLower() == "gradelevel")
                        //{
                        //    transactionIQ = Utility.FilteredData(pageResult.FilterParams, gradeUsStandardData).AsQueryable();
                        //}
                        //else
                        //{
                        //    gradeUsStandardList._message = NORECORDFOUND;
                        //    gradeUsStandardList._failure = true;
                        //    gradeUsStandardList._tenantName = pageResult._tenantName;
                        //    gradeUsStandardList._token = pageResult._token;
                        //    return gradeUsStandardList;
                        //}

                    }
                }
                if (pageResult.SortingModel != null)
                {
                    transactionIQ = Utility.Sort(transactionIQ, pageResult.SortingModel.SortColumn, pageResult.SortingModel.SortDirection.ToLower());
                }
                int totalCount = transactionIQ.Count();
                if (pageResult.PageNumber > 0 && pageResult.PageSize > 0)
                {
                    transactionIQ = transactionIQ.Skip((pageResult.PageNumber - 1) * pageResult.PageSize).Take(pageResult.PageSize);
                }
                //var gradeUsList = transactionIQ.AsNoTracking().Select(s => new GetGradeUsStandardForView
                //{
                //    TenantId = s.TenantId,
                //    SchoolId = s.SchoolId,
                //    StandardRefNo = s.StandardRefNo,
                //    GradeLevel=s.GradeLevel,
                //    Domain = s.Domain,
                //    Subject = s.Subject,
                //    Course = s.Course,
                //    StandardDetails = s.StandardDetails,
                //    GradeStandardId=s.GradeStandardId,
                //    Topic = s.Topic
                //}).ToList();

                gradeUsStandardList.TenantId = pageResult.TenantId;
                gradeUsStandardList.SchoolId = pageResult.SchoolId;
                //gradeUsStandardList.getGradeUsStandardView = gradeUsList;
                gradeUsStandardList.gradeUsStandardList = transactionIQ.ToList();
                gradeUsStandardList.TotalCount = totalCount;
                gradeUsStandardList.PageNumber = pageResult.PageNumber;
                gradeUsStandardList._pageSize = pageResult.PageSize;
                gradeUsStandardList._tenantName = pageResult._tenantName;
                gradeUsStandardList._token = pageResult._token;
                gradeUsStandardList._failure = false;
            }
            catch (Exception es)
            {
                gradeUsStandardList._message = es.Message;
                gradeUsStandardList._failure = true;
                gradeUsStandardList._tenantName = pageResult._tenantName;
                gradeUsStandardList._token = pageResult._token;
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
                var subjectStandardData = this.context?.GradeUsStandard.Where(x => x.TenantId == gradeUsStandardListModel.TenantId && x.SchoolId == gradeUsStandardListModel.SchoolId && x.IsSchoolSpecific == true).Select(s => new { s.Subject, s.TenantId, s.SchoolId }).Distinct().ToList();

                if (subjectStandardData.Count > 0)
                {
                    var subjectList = subjectStandardData.Select(s => new GradeUsStandard
                    {
                        TenantId = s.TenantId,
                        SchoolId = s.SchoolId,
                        Subject = s.Subject
                    }).ToList();

                    //subjectStandardList.getGradeUsStandardView = subjectList;
                    subjectStandardList.gradeUsStandardList = subjectList;
                    subjectStandardList._failure = false;
                }
                else
                {
                    subjectStandardList._failure = true;
                    subjectStandardList._message = NORECORDFOUND;
                }
                subjectStandardList._tenantName = gradeUsStandardListModel._tenantName;
                subjectStandardList._token = gradeUsStandardListModel._token;
            }
            catch (Exception es)
            {
                subjectStandardList.gradeUsStandardList = null;
                subjectStandardList._message = es.Message;
                subjectStandardList._failure = true;
                subjectStandardList._tenantName = gradeUsStandardListModel._tenantName;
                subjectStandardList._token = gradeUsStandardListModel._token;
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
                var subjectCourseData = this.context?.GradeUsStandard.Where(x => x.TenantId == gradeUsStandardListModel.TenantId && x.SchoolId == gradeUsStandardListModel.SchoolId && x.IsSchoolSpecific == true).Select(s => new { s.Course, s.TenantId, s.SchoolId }).Distinct().ToList();

                if (subjectCourseData.Count > 0)
                {
                    var courseList = subjectCourseData.Select(s => new GradeUsStandard
                    {
                        TenantId = s.TenantId,
                        SchoolId = s.SchoolId,
                        Course = s.Course
                    }).ToList();
                    //courseStandardList.getGradeUsStandardView = courseList;
                    courseStandardList.gradeUsStandardList = courseList;
                    courseStandardList._failure = false;
                }
                else
                {
                    courseStandardList._failure = true;
                    courseStandardList._message = NORECORDFOUND;
                }
                courseStandardList._tenantName = gradeUsStandardListModel._tenantName;
                courseStandardList._token = gradeUsStandardListModel._token;
            }
            catch (Exception es)
            {
                courseStandardList.gradeUsStandardList = null;
                courseStandardList._message = es.Message;
                courseStandardList._failure = true;
                courseStandardList._tenantName = gradeUsStandardListModel._tenantName;
                courseStandardList._token = gradeUsStandardListModel._token;
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
            var checkStandardRefNo = this.context?.GradeUsStandard.Where(x => x.TenantId == checkStandardRefNoViewModel.TenantId && x.StandardRefNo == checkStandardRefNoViewModel.StandardRefNo).ToList();
            if (checkStandardRefNo.Count() > 0)
            {
                checkStandardRefNoViewModel.IsValidStandardRefNo = false;
                checkStandardRefNoViewModel._message = "StandardRefNo Already Exist,Please Try Again!!";
            }
            else
            {
                checkStandardRefNoViewModel.IsValidStandardRefNo = true;
                checkStandardRefNoViewModel._message = "StandardRefNo Id Is Valid";
            }
            return checkStandardRefNoViewModel;
        }
        /// <summary>
        /// Add Honor Roll
        /// </summary>
        /// <param name="honorRollsAddViewModel"></param>
        /// <returns></returns>
        public HonorRollsAddViewModel AddHonorRoll(HonorRollsAddViewModel honorRollsAddViewModel)
        {
            try
            {
                var checkHonorRoll = this.context?.HonorRolls.Where(x => x.SchoolId == honorRollsAddViewModel.honorRolls.SchoolId && x.TenantId == honorRollsAddViewModel.honorRolls.TenantId && x.HonorRoll.ToLower() == honorRollsAddViewModel.honorRolls.HonorRoll.ToLower()).FirstOrDefault();
                if (checkHonorRoll != null)
                {
                    honorRollsAddViewModel._failure = false;
                    honorRollsAddViewModel._message = "Honor Roll Already Exists";
                }
                else
                {
                    int? HonorRollId = 1;

                    var HonorRollData = this.context?.HonorRolls.Where(x => x.SchoolId == honorRollsAddViewModel.honorRolls.SchoolId && x.TenantId == honorRollsAddViewModel.honorRolls.TenantId).OrderByDescending(x => x.HonorRollId).FirstOrDefault();

                    if (HonorRollData != null)
                    {
                        HonorRollId = HonorRollData.HonorRollId + 1;
                    }

                    honorRollsAddViewModel.honorRolls.HonorRollId = (int)HonorRollId;                    
                    honorRollsAddViewModel.honorRolls.CreatedOn = DateTime.UtcNow;
                    this.context?.HonorRolls.Add(honorRollsAddViewModel.honorRolls);
                    this.context?.SaveChanges();
                    honorRollsAddViewModel._failure = false;
                    honorRollsAddViewModel._message = "Honor Roll Added Successfully";
                }
            }
            catch (Exception es)
            {
                honorRollsAddViewModel._failure = true;
                honorRollsAddViewModel._message = es.Message;
            }
            return honorRollsAddViewModel;
        }
        /// <summary>
        /// Update Honor Roll
        /// </summary>
        /// <param name="honorRollsAddViewModel"></param>
        /// <returns></returns>
        public HonorRollsAddViewModel UpdateHonorRoll(HonorRollsAddViewModel honorRollsAddViewModel)
        {
            try
            {
                var honorRollUpdate = this.context?.HonorRolls.FirstOrDefault(x => x.TenantId == honorRollsAddViewModel.honorRolls.TenantId && x.SchoolId == honorRollsAddViewModel.honorRolls.SchoolId && x.HonorRollId == honorRollsAddViewModel.honorRolls.HonorRollId);
                if (honorRollUpdate != null)
                {
                    var checkHonorRoll= this.context?.HonorRolls.Where(x => x.SchoolId == honorRollsAddViewModel.honorRolls.SchoolId && x.TenantId == honorRollsAddViewModel.honorRolls.TenantId  && x.HonorRollId != honorRollsAddViewModel.honorRolls.HonorRollId && x.HonorRoll.ToLower() == honorRollsAddViewModel.honorRolls.HonorRoll.ToLower()).FirstOrDefault();

                    if (checkHonorRoll != null)
                    {
                        honorRollsAddViewModel._failure = true;
                        honorRollsAddViewModel._message = "Honor Roll Already Exists";
                    }
                    else
                    {
                        honorRollsAddViewModel.honorRolls.CreatedBy = honorRollUpdate.CreatedBy;
                        honorRollsAddViewModel.honorRolls.CreatedOn = honorRollUpdate.CreatedOn;
                        honorRollsAddViewModel.honorRolls.UpdatedOn = DateTime.Now;
                        this.context.Entry(honorRollUpdate).CurrentValues.SetValues(honorRollsAddViewModel.honorRolls);
                        this.context?.SaveChanges();
                        honorRollsAddViewModel._failure = false;
                        honorRollsAddViewModel._message = "Honor Roll Updated Successfully";
                    }
                }
                else
                {
                    honorRollsAddViewModel.honorRolls = null;
                    honorRollsAddViewModel._failure = true;
                    honorRollsAddViewModel._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {

                honorRollsAddViewModel._failure = true;
                honorRollsAddViewModel._message = es.Message;
            }
            return honorRollsAddViewModel;
        }
        /// <summary>
        /// Get All Honor Roll List
        /// </summary>
        /// <param name="pageResult"></param>
        /// <returns></returns>
        public HonorRollsListViewModel GetAllHonorRollList(PageResult pageResult)
        {
            HonorRollsListViewModel honorRollList = new HonorRollsListViewModel();
            IQueryable<HonorRolls> transactionIQ = null;

            var honorRollsData = this.context?.HonorRolls.Where(x => x.TenantId == pageResult.TenantId && x.SchoolId == pageResult.SchoolId);
            try
            {
                if (pageResult.FilterParams == null || pageResult.FilterParams.Count == 0)
                {
                    transactionIQ = honorRollsData;
                }
                else
                {
                    if (pageResult.FilterParams != null && pageResult.FilterParams.ElementAt(0).ColumnName == null && pageResult.FilterParams.Count == 1)
                    {
                        string Columnvalue = pageResult.FilterParams.ElementAt(0).FilterValue;
                        transactionIQ = honorRollsData.Where(x => x.HonorRoll != null && x.Breakoff.ToString() == Columnvalue.ToString() || x.HonorRoll != null && x.HonorRoll.ToLower().Contains(Columnvalue.ToLower()));
                    }
                    else
                    {
                        transactionIQ = Utility.FilteredData(pageResult.FilterParams, honorRollsData).AsQueryable();
                    }
                }
                if (pageResult.SortingModel != null)
                {
                    transactionIQ = Utility.Sort(transactionIQ, pageResult.SortingModel.SortColumn, pageResult.SortingModel.SortDirection.ToLower());
                }
                int totalCount = transactionIQ.Count();
                if (pageResult.PageNumber > 0 && pageResult.PageSize > 0)
                {
                    transactionIQ = transactionIQ.Select(e=>new HonorRolls
                    {                        
                        MarkingPeriodId=e.MarkingPeriodId,
                        HonorRollId=e.HonorRollId,
                        HonorRoll=e.HonorRoll,
                        Breakoff=e.Breakoff
                    }).Skip((pageResult.PageNumber - 1) * pageResult.PageSize).Take(pageResult.PageSize);
                }
                honorRollList.TenantId = pageResult.TenantId;
                honorRollList.SchoolId = pageResult.SchoolId;
                honorRollList.honorRollList = transactionIQ.ToList();               
                honorRollList.TotalCount = totalCount;
                honorRollList.PageNumber = pageResult.PageNumber;
                honorRollList._pageSize = pageResult.PageSize;
                honorRollList._tenantName = pageResult._tenantName;
                honorRollList._token = pageResult._token;
                honorRollList._failure = false;
            }
            catch (Exception es)
            {
                honorRollList._message = es.Message;
                honorRollList._failure = true;
                honorRollList._tenantName = pageResult._tenantName;
                honorRollList._token = pageResult._token;
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
            try
            {
                var honorRollDelete = this.context?.HonorRolls.FirstOrDefault(x => x.TenantId == honorRollsAddViewModel.honorRolls.TenantId && x.SchoolId == honorRollsAddViewModel.honorRolls.SchoolId && x.HonorRollId == honorRollsAddViewModel.honorRolls.HonorRollId);

                if (honorRollDelete != null)
                {
                    this.context?.HonorRolls.Remove(honorRollDelete);
                    this.context?.SaveChanges();
                    honorRollsAddViewModel._failure = false;
                    honorRollsAddViewModel._message = "Honor Roll Deleted Successfully";
                }
                else
                {
                    honorRollsAddViewModel._message = NORECORDFOUND;
                    honorRollsAddViewModel._failure = true;
                }
            }
            catch (Exception es)
            {
                honorRollsAddViewModel._failure = true;
                honorRollsAddViewModel._message = es.Message;
            }
            return honorRollsAddViewModel;
        }
    }
}
