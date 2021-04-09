using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using opensis.data.Helper;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels;
using opensis.data.ViewModels.School;
using System;
using System.Collections.Generic;
using System.Linq;

namespace opensis.data.Repository
{
    public class SchoolRepository : ISchoolRepository
    {
        private CRMContext context;
        private static readonly string NORECORDFOUND = "No Record Found";
        public SchoolRepository(IDbContextFactory dbContextFactory)
        {
            this.context = dbContextFactory.Create();
        }



        private static string ToFullAddress(string Address1, string Address2, string City, string State, string Country, string Zip)
        {
            string address = "";
            if (!string.IsNullOrWhiteSpace(Address1))
            {


                return address == null
                      ? null
                      : $"{Address1?.Trim()}{(!string.IsNullOrWhiteSpace(Address2) ? $", {Address2?.Trim()}" : string.Empty)}, {City?.Trim()}, {State?.Trim()} {Zip?.Trim()}";
            }
            return address;
        }
        /// <summary>
        /// Get SchoolsList with pagination
        /// </summary>
        /// <param name="pageResult"></param>
        /// <returns></returns>
        public SchoolListModel GetAllSchoolList(PageResult pageResult)
        {
            int resultData;
            SchoolListModel schoolListModel = new SchoolListModel();
            IQueryable<SchoolMaster> transactionIQ = null;
            
                var SchoolMasterList = this.context?.SchoolMaster
                         .Include(d => d.SchoolDetail)
                         .Where(x => x.TenantId == pageResult.TenantId && pageResult.IncludeInactive == false ? x.SchoolDetail.FirstOrDefault().Status == true : true);

                
            try
            {
                    if (pageResult.FilterParams == null || pageResult.FilterParams.Count == 0)
                    {
                        //string sortField = "SchoolName"; string sortOrder = "desc";

                        transactionIQ = SchoolMasterList;
                    }


                    else
                    {
                        if (pageResult.FilterParams != null && pageResult.FilterParams.ElementAt(0).ColumnName == null && pageResult.FilterParams.Count == 1)
                        {
                            string Columnvalue = pageResult.FilterParams.ElementAt(0).FilterValue;
                            transactionIQ = SchoolMasterList.Where(x => x.SchoolName.ToLower().Contains(Columnvalue.ToLower()) || x.StreetAddress1.ToLower().Contains(Columnvalue.ToLower()) || x.StreetAddress2.ToLower().Contains(Columnvalue.ToLower()) || x.Zip.ToLower().Contains(Columnvalue.ToLower()) || x.State.ToLower().Contains(Columnvalue.ToLower()) || x.City.ToLower().Contains(Columnvalue.ToLower()) || x.Country.ToLower().Contains(Columnvalue.ToLower()));

                            var childTelephoneFilter = SchoolMasterList.Where(x => x.SchoolDetail.FirstOrDefault() != null ? x.SchoolDetail.FirstOrDefault().Telephone.ToLower().Contains(Columnvalue.ToLower()) : string.Empty.Contains(Columnvalue));

                            if (childTelephoneFilter.ToList().Count > 0)
                            {
                                transactionIQ = transactionIQ.Concat(childTelephoneFilter);
                            }

                            var childNameOfPrincipalFilter = SchoolMasterList.Where(x => x.SchoolDetail.FirstOrDefault() != null ? x.SchoolDetail.FirstOrDefault().NameOfPrincipal.ToLower().Contains(Columnvalue.ToLower()) : string.Empty.Contains(Columnvalue));
                            if (childNameOfPrincipalFilter.ToList().Count > 0)
                            {
                                transactionIQ = transactionIQ.Concat(childNameOfPrincipalFilter);
                            }
                            //var countryFilter = this.context?.Country.Where(x => x.Name.ToLower().Contains(Columnvalue.ToLower()));
                            //if (countryFilter.ToList().Count > 0)
                            //{
                            //    foreach (var country in countryFilter.ToList())
                            //    {
                            //        var countrySearch = SchoolMasterList.Where(x => x.Country == country.Id.ToString());

                            //        if (countrySearch.ToList().Count > 0)
                            //        {
                            //            transactionIQ = transactionIQ.Concat(countrySearch);
                            //        }
                            //    }
                            //}
                        }
                        else
                        {
                            transactionIQ = Utility.FilteredData(pageResult.FilterParams, SchoolMasterList).AsQueryable();
                        }

                        transactionIQ = transactionIQ.Distinct();
                    }
                
            
                if (pageResult.SortingModel != null)
                {
                    switch (pageResult.SortingModel.SortColumn.ToLower())
                    {
                        case "nameofprincipal":

                            if (pageResult.SortingModel.SortDirection.ToLower() == "asc")
                            {

                                transactionIQ = transactionIQ.OrderBy(a => a.SchoolDetail.Count > 0 ? a.SchoolDetail.FirstOrDefault().NameOfPrincipal : null);
                            }
                            else
                            {
                                transactionIQ = transactionIQ.OrderByDescending(a => a.SchoolDetail.Count > 0 ? a.SchoolDetail.FirstOrDefault().NameOfPrincipal : null);
                            }
                            break;

                        default:
                            transactionIQ = Utility.Sort(transactionIQ, pageResult.SortingModel.SortColumn, pageResult.SortingModel.SortDirection.ToLower());
                            break;
                    }

                }

                int totalCount = transactionIQ.Count();
                if (pageResult.PageNumber > 0 && pageResult.PageSize > 0)
                {
                    transactionIQ = transactionIQ.Select(p => new SchoolMaster
                    {
                        SchoolId = p.SchoolId,
                        TenantId = p.TenantId,
                        SchoolName = p.SchoolName.Trim(),
                        Zip = p.Zip,
                        StreetAddress1 = p.StreetAddress1,
                        StreetAddress2 = p.StreetAddress2,
                        State = p.State,
                        City = p.City,
                        Country = p.Country,
                        SchoolDetail = p.SchoolDetail.Select(s => new SchoolDetail
                        {
                            Telephone = s.Telephone,
                            NameOfPrincipal = s.NameOfPrincipal,
                            Status = s.Status
                        }).ToList()
                    }).Skip((pageResult.PageNumber - 1) * pageResult.PageSize).Take(pageResult.PageSize);
                }
                //var schoollist = transactionIQ.AsNoTracking().Select(s => new GetSchoolForView
                //{
                //    SchoolId = s.SchoolId,
                //    SchoolName = s.SchoolName,
                //    TenantId = s.TenantId,
                //    Telephone = s.SchoolDetail.FirstOrDefault() == null ? string.Empty : s.SchoolDetail.FirstOrDefault().Telephone == null ? string.Empty : s.SchoolDetail.FirstOrDefault().Telephone.Trim(),
                //    NameOfPrincipal = s.SchoolDetail.FirstOrDefault() == null ? string.Empty : s.SchoolDetail.FirstOrDefault().NameOfPrincipal == null ? string.Empty : s.SchoolDetail.FirstOrDefault().NameOfPrincipal.Trim(),
                //    StreetAddress1 = s.SchoolDetail.FirstOrDefault() == null ? string.Empty : ToFullAddress(s.StreetAddress1, s.StreetAddress2,
                //    int.TryParse(s.City, out resultData) == true ? this.context.City.Where(x => x.Id == Convert.ToInt32(s.City)).FirstOrDefault().Name : s.City,
                //    int.TryParse(s.State, out resultData) == true ? this.context.State.Where(x => x.Id == Convert.ToInt32(s.State)).FirstOrDefault().Name : s.State,
                //    int.TryParse(s.Country, out resultData) == true ? this.context.Country.Where(x => x.Id == Convert.ToInt32(s.Country)).FirstOrDefault().Name : string.Empty, s.Zip),
                //    Status = s.SchoolDetail.FirstOrDefault() == null ? false : s.SchoolDetail.FirstOrDefault().Status == null ? false : s.SchoolDetail.FirstOrDefault().Status
                //}).ToList();

                schoolListModel.TenantId = pageResult.TenantId;
                //schoolListModel.GetSchoolForView = schoollist;
                schoolListModel.schoolMaster = transactionIQ.ToList();
                schoolListModel.TotalCount = totalCount;
                schoolListModel.PageNumber = pageResult.PageNumber;
                schoolListModel._pageSize = pageResult.PageSize;
                schoolListModel._tenantName = pageResult._tenantName;
                schoolListModel._token = pageResult._token;
                schoolListModel._failure = false;
            }
            catch (Exception es)
            {
                schoolListModel._message = es.Message;
                schoolListModel._failure = true;
                schoolListModel._tenantName = pageResult._tenantName;
                schoolListModel._token = pageResult._token;
            }
            return schoolListModel;

        }
        /// <summary>
        /// Get All school for dropdown
        /// </summary>
        /// <param name="school"></param>
        /// <returns></returns>
        public SchoolListModel GetAllSchools(SchoolListModel school)
        {
            SchoolListModel schoolListModel = new SchoolListModel();
            try
            {
                var schoolList = this.context?.SchoolMaster.Include(x => x.SchoolDetail).Where(x => x.TenantId == school.TenantId && x.SchoolDetail.FirstOrDefault().Status!=false).OrderBy(x => x.SchoolName)
                    .Select(e => new SchoolMaster()
                    {
                        SchoolId = e.SchoolId,
                        TenantId = e.TenantId,
                        SchoolName = e.SchoolName.Trim(),
                        SchoolDetail = e.SchoolDetail.Select(s => new SchoolDetail
                        {
                            DateSchoolOpened = s.DateSchoolOpened,
                            DateSchoolClosed = s.DateSchoolClosed
                        }).ToList()
                    }).ToList();
                        //    DateSchoolOpened = x.SchoolDetail.FirstOrDefault().DateSchoolOpened,
                        //    DateSchoolClosed = x.SchoolDetail.FirstOrDefault().DateSchoolClosed

                        //}).ToList();
                        //schoolListModel.GetSchoolForView = schoolList;
                schoolListModel.schoolMaster = schoolList;
                schoolListModel.PageNumber = null;
                schoolListModel._pageSize = null;
                schoolListModel._tenantName = school._tenantName;
                schoolListModel._token = school._token;
                schoolListModel._failure = false;
            }
            catch (Exception es)
            {
                schoolListModel._message = es.Message;
                schoolListModel._failure = true;
                schoolListModel._tenantName = school._tenantName;
                schoolListModel._token = school._token;
            }
            return schoolListModel;

        }
        /// <summary>
        /// Get School by id
        /// </summary>
        /// <param name="school"></param>
        /// <returns></returns>
        public SchoolAddViewModel ViewSchool(SchoolAddViewModel school)
        {
            try
            {
                SchoolAddViewModel SchoolAddViewModel = new SchoolAddViewModel();
                var schoolMaster = this.context?.SchoolMaster.Include(x => x.SchoolDetail).Include(x => x.FieldsCategory).ThenInclude(x => x.CustomFields).ThenInclude(x => x.CustomFieldsValue).FirstOrDefault(x => x.TenantId == school.schoolMaster.TenantId && x.SchoolId == school.schoolMaster.SchoolId);
                var schoolFieldCatagory = schoolMaster.FieldsCategory.Where(x => x.Module.Trim() == "School").OrderByDescending(x => x.IsSystemCategory).ThenBy(x => x.SortOrder).ToList();
                var schoolcustomFields = schoolFieldCatagory.FirstOrDefault().CustomFields.OrderByDescending(y => y.SystemField).ThenBy(y => y.SortOrder).ToList();
                if (schoolMaster != null)
                {
                    school.schoolMaster = schoolMaster;
                    school.schoolMaster.FieldsCategory = schoolFieldCatagory;
                    school.schoolMaster.FieldsCategory.FirstOrDefault().CustomFields = schoolcustomFields;
                    if (school.schoolMaster.SchoolDetail.ToList().Count > 0)
                    {
                        school.schoolMaster.SchoolDetail.FirstOrDefault().SchoolMaster = null;
                    }
                    school._tenantName = school._tenantName;
                    return school;
                }
                else
                {
                    SchoolAddViewModel._failure = true;
                    SchoolAddViewModel._message = NORECORDFOUND;
                    return SchoolAddViewModel;
                }
            }
            catch (Exception es)
            {

                throw;
            }

        }

        public SchoolAddViewModel UpdateSchool(SchoolAddViewModel school)
        {
            try
            {
                var schoolMaster = this.context?.SchoolMaster.Include(x => x.SchoolDetail).Include(x => x.FieldsCategory).ThenInclude(x => x.CustomFields).ThenInclude(x => x.CustomFieldsValue).FirstOrDefault(x => x.TenantId == school.schoolMaster.TenantId && x.SchoolId == school.schoolMaster.SchoolId);

                var checkInternalId = this.context?.SchoolMaster.Where(x => x.TenantId == school.schoolMaster.TenantId && x.SchoolInternalId == school.schoolMaster.SchoolInternalId && x.SchoolInternalId != null && x.SchoolId != school.schoolMaster.SchoolId).ToList();

                if (checkInternalId.Count() > 0)
                {
                    school.schoolMaster = null;
                    school._failure = true;
                    school._message = "School InternalID Already Exist";
                }
                else
                {
                    if (string.IsNullOrEmpty(school.schoolMaster.SchoolInternalId))
                    {
                        school.schoolMaster.SchoolInternalId = schoolMaster.SchoolInternalId;
                    }
                    school.schoolMaster.CreatedBy = schoolMaster.CreatedBy;
                    school.schoolMaster.DateCreated = schoolMaster.DateCreated;
                    school.schoolMaster.SchoolGuid = schoolMaster.SchoolGuid;
                    school.schoolMaster.PlanId = schoolMaster.PlanId;
                    school.schoolMaster.DateModifed = DateTime.Now;
                    this.context.Entry(schoolMaster).CurrentValues.SetValues(school.schoolMaster);

                    if (schoolMaster.SchoolDetail.ToList().Count == 0 && school.schoolMaster.SchoolDetail.ToList().Count > 0)
                    {
                        school.schoolMaster.SchoolDetail.ToList().ForEach(p => p.Id = (int)Utility.GetMaxPK(this.context, new Func<SchoolDetail, int>(x => x.Id)));
                        school.schoolMaster.SchoolDetail.ToList().ForEach(p => p.SchoolId = school.schoolMaster.SchoolId);
                        school.schoolMaster.SchoolDetail.ToList().ForEach(p => p.TenantId = school.schoolMaster.TenantId);
                        this.context?.SchoolDetail.AddRange(school.schoolMaster.SchoolDetail);
                    }

                    if (schoolMaster.SchoolDetail.ToList().Count > 0)
                    {
                        foreach (var detailes in schoolMaster.SchoolDetail.ToList())
                        {
                            this.context.Entry(schoolMaster.SchoolDetail.FirstOrDefault()).CurrentValues.SetValues(school.schoolMaster.SchoolDetail.FirstOrDefault());
                        }
                    }

                    //Student Custom Field value With Delete
                    if (school.schoolMaster.FieldsCategory != null && school.schoolMaster.FieldsCategory.ToList().Count > 0)
                    {
                        var fieldsCategory = school.schoolMaster.FieldsCategory.FirstOrDefault(x => x.CategoryId == school.SelectedCategoryId);

                        if (fieldsCategory != null)
                        {
                            foreach (var customFields in fieldsCategory.CustomFields.ToList())
                            {
                                var customFieldValueData = this.context?.CustomFieldsValue.FirstOrDefault(x => x.TenantId == school.schoolMaster.TenantId && x.SchoolId == school.schoolMaster.SchoolId && x.CategoryId == customFields.CategoryId && x.FieldId == customFields.FieldId && x.Module == "School" && x.TargetId == school.schoolMaster.SchoolId);
                                if (customFieldValueData != null)
                                {
                                    this.context?.CustomFieldsValue.RemoveRange(customFieldValueData);
                                }

                                if (customFields.CustomFieldsValue != null && customFields.CustomFieldsValue.ToList().Count > 0)
                                {
                                    customFields.CustomFieldsValue.FirstOrDefault().Module = "School";
                                    customFields.CustomFieldsValue.FirstOrDefault().CategoryId = customFields.CategoryId;
                                    customFields.CustomFieldsValue.FirstOrDefault().FieldId = customFields.FieldId;
                                    customFields.CustomFieldsValue.FirstOrDefault().CustomFieldTitle = customFields.Title;
                                    customFields.CustomFieldsValue.FirstOrDefault().CustomFieldType = customFields.Type;
                                    customFields.CustomFieldsValue.FirstOrDefault().SchoolId = school.schoolMaster.SchoolId;
                                    customFields.CustomFieldsValue.FirstOrDefault().TargetId = school.schoolMaster.SchoolId;
                                    this.context?.CustomFieldsValue.AddRange(customFields.CustomFieldsValue);
                                }
                            }

                        }
                    }
                    this.context?.SaveChanges();                    
                    if (school.schoolMaster.SchoolDetail.ToList().Count > 0)
                    {
                        school.schoolMaster.SchoolDetail.FirstOrDefault().SchoolMaster = null;
                    }
                    school._failure = false;
                    school._message = "School Updated Successfully";
                }               
                return school;
            }
            catch (Exception ex)
            {
                school.schoolMaster = null;
                school._failure = true;
                school._message = NORECORDFOUND;
                return school;
            }

        }

        /// <summary>
        /// School Add
        /// </summary>
        /// <param name="school"></param>
        /// <returns></returns>
        public SchoolAddViewModel AddSchool(SchoolAddViewModel school)
        {
            using (var transaction = this.context.Database.BeginTransaction())
            {
                try
                {
                    int? MasterSchoolId = Utility.GetMaxPK(this.context, new Func<SchoolMaster, int>(x => x.SchoolId));
                    //int? MemberShipId = Utility.GetMaxPK(this.context, new Func<Membership, int>(x => x.MembershipId));
                    //int? CategoryId = Utility.GetMaxPK(this.context, new Func<FieldsCategory, int>(x => x.CategoryId));
                    school.schoolMaster.SchoolId = (int)MasterSchoolId;
                    Guid GuidId = Guid.NewGuid();
                    var GuidIdExist = this.context?.SchoolMaster.FirstOrDefault(x => x.SchoolGuid == GuidId);
                    if (GuidIdExist != null)
                    {
                        school._failure = true;
                        school._message = "Guid is already exist, Please try again.";
                        return school;
                    }
                    school.schoolMaster.SchoolGuid = GuidId;

                    if (school.schoolMaster.SchoolDetail.ToList().Count > 0)
                    {
                        school.schoolMaster.SchoolDetail.ToList().ForEach(p => p.Id = (int)Utility.GetMaxPK(this.context, new Func<SchoolDetail, int>(x => x.Id)));
                    }
                    school.schoolMaster.DateCreated = DateTime.UtcNow;
                    school.schoolMaster.TenantId = school.schoolMaster.TenantId;

                    if (!string.IsNullOrEmpty(school.schoolMaster.SchoolInternalId))
                    {
                        bool checkInternalID = CheckInternalID(school.schoolMaster.TenantId, school.schoolMaster.SchoolInternalId);
                        if (checkInternalID == false)
                        {
                            school.schoolMaster = null;
                            school._failure = true;
                            school._message = "School InternalID Already Exist";
                            return school;
                        }
                    }
                    else
                    {
                        school.schoolMaster.SchoolInternalId = MasterSchoolId.ToString();
                    }

                    school.schoolMaster.Membership = new List<Membership>() {
                    new Membership(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,Profile= "Super Administrator", IsActive= true, IsSuperadmin= true, IsSystem= true, MembershipId= 1, ProfileType= "Super Administrator"},
                    new Membership(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,Profile= "School Administrator", IsActive= true, IsSuperadmin= false, IsSystem= true, MembershipId= 2, ProfileType= "School Administrator"},
                    new Membership(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,Profile= "Admin Assistant", IsActive= true, IsSuperadmin= false, IsSystem= true, MembershipId= 3, ProfileType= "Admin Assistant"},
                    new Membership(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,Profile= "Teacher", IsActive= true, IsSuperadmin= false, IsSystem= true, MembershipId= 4, ProfileType= "Teacher"},
                    new Membership(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,Profile= "Homeroom Teacher", IsActive= true, IsSuperadmin= false, IsSystem= true, MembershipId= 5, ProfileType= "Homeroom Teacher"},
                    new Membership(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,Profile= "Parent", IsActive= true, IsSuperadmin= false, IsSystem= true, MembershipId= 6, ProfileType= "Parent"},
                    new Membership(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,Profile= "Student", IsActive= true, IsSuperadmin= false, IsSystem= true, MembershipId= 7, ProfileType= "Student"},
                };

                    long? dpdownValueId = Utility.GetMaxLongPK(this.context, new Func<DpdownValuelist, long>(x => x.Id));

                    school.schoolMaster.DpdownValuelist = new List<DpdownValuelist>() {
                    new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Grade Level",LovColumnValue="PK",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId},
                    new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Grade Level",LovColumnValue="K",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+1},
                    new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Grade Level",LovColumnValue="1",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+2},
                    new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Grade Level",LovColumnValue="2",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+3},
                    new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Grade Level",LovColumnValue="3",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+4},
                    new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Grade Level",LovColumnValue="4",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+5},
                    new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Grade Level",LovColumnValue="5",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+6},
                    new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Grade Level",LovColumnValue="6",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+7},
                    new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Grade Level",LovColumnValue="7",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+8},
                    new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Grade Level",LovColumnValue="8",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+9},
                    new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Grade Level",LovColumnValue="9",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+10},
                    new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Grade Level",LovColumnValue="10",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+11},
                    new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Grade Level",LovColumnValue="11",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+12},
                    new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Grade Level",LovColumnValue="12",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+13},
                    new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Grade Level",LovColumnValue="13",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+14},
                    new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Grade Level",LovColumnValue="14",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+15},
                    new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Grade Level",LovColumnValue="15",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+16},
                    new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Grade Level",LovColumnValue="16",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+17},
                    new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Grade Level",LovColumnValue="17",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+18},
                    new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Grade Level",LovColumnValue="18",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+19},
                    new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Grade Level",LovColumnValue="19",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+20},
                    new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Grade Level",LovColumnValue="20",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+21},


                    new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="School Gender",LovColumnValue="Boys",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+22},
                    new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="School Gender",LovColumnValue="Girls",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+23},
                    new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="School Gender",LovColumnValue="Mixed",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+24},


                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Salutation",LovColumnValue="Mr.",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+25},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Salutation",LovColumnValue="Miss.",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+26},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Salutation",LovColumnValue="Mrs.",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+27},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Salutation",LovColumnValue="Ms.",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+28},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Salutation",LovColumnValue="Dr.",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+29},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Salutation",LovColumnValue="Rev.",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+30},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Salutation",LovColumnValue="Prof.",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+31},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Salutation",LovColumnValue="Sir.",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+32},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Salutation",LovColumnValue="Lord ",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+33},


                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Suffix",LovColumnValue="Jr.",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+34},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Suffix",LovColumnValue="Sr",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+35},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Suffix",LovColumnValue="Sr",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+36},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Suffix",LovColumnValue="II",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+37},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Suffix",LovColumnValue="III",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+38},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Suffix",LovColumnValue="IV",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+39},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Suffix",LovColumnValue="V",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+40},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Suffix",LovColumnValue="PhD",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+41},


                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Gender",LovColumnValue="Male",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+42},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Gender",LovColumnValue="Female",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+43},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Gender",LovColumnValue="Other",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+44},


                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Marital Status",LovColumnValue="Single",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+45},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Marital Status",LovColumnValue="Married",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+46},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Marital Status",LovColumnValue="Partnered",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+47},


                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Rolling/Retention Option",LovColumnValue="Next grade at current school",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+48},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Rolling/Retention Option",LovColumnValue="Retain",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+49},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Rolling/Retention Option",LovColumnValue="Do not enroll after this school year",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+50},


                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Relationship",LovColumnValue="Mother",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+51},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Relationship",LovColumnValue="Father",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+52},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Relationship",LovColumnValue="Legal Guardian",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+53},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Relationship",LovColumnValue="Other",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+54},


                    new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Enrollment Type",LovColumnValue="Add",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+55},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Enrollment Type",LovColumnValue="Drop",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+56},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Enrollment Type",LovColumnValue="Rolled Over",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+57},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Enrollment Type",LovColumnValue="Drop (Transfer)",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+58},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Enrollment Type",LovColumnValue="Enroll (Transfer)",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+59},


                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Field Type",LovColumnValue="Dropdown",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+60},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Field Type",LovColumnValue="Editable Dropdown",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+61},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Field Type",LovColumnValue="Text",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+62},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Field Type",LovColumnValue="Checkbox",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+63},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Field Type",LovColumnValue="Number",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+64},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Field Type",LovColumnValue="Multiple SelectBox",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+65},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Field Type",LovColumnValue="Date",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+66},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="Field Type",LovColumnValue="Textarea",CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+67},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="School Level",LovColumnValue=school.schoolMaster.SchoolLevel,CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+68},
                     new DpdownValuelist(){UpdatedOn=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy, TenantId= school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,LovName="School Classification",LovColumnValue=school.schoolMaster.SchoolClassification,CreatedBy=school.schoolMaster.CreatedBy,CreatedOn=DateTime.UtcNow,Id=(long)dpdownValueId+69}
                };

                school.schoolMaster.FieldsCategory = new List<FieldsCategory>()
                {
                    new FieldsCategory(){ TenantId=school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,IsSystemCategory=true,Search=true, Title="General Information",Module="School",SortOrder=1,Required=true,Hide=false,LastUpdate=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy,CategoryId=1},
                    new FieldsCategory(){ TenantId=school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,IsSystemCategory=true,Search=true, Title="Wash Information",Module="School",SortOrder=2,Required=true,Hide=false,LastUpdate=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy,CategoryId=2},
                    new FieldsCategory(){ TenantId=school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,IsSystemCategory=true,Search=true, Title="General Info",Module="Student",SortOrder=1,Required=true,Hide=false,LastUpdate=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy,CategoryId=3},
                    new FieldsCategory(){ TenantId=school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,IsSystemCategory=true,Search=true, Title="Enrollment Info",Module="Student",SortOrder=2,Required=true,Hide=false,LastUpdate=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy,CategoryId=4},
                    new FieldsCategory(){ TenantId=school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,IsSystemCategory=true,Search=true, Title="Address & Contact",Module="Student",SortOrder=3,Required=true,Hide=false,LastUpdate=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy,CategoryId=5},

                    new FieldsCategory(){ TenantId=school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,IsSystemCategory=true,Search=true, Title="Family Info",Module="Student",SortOrder=4,Required=true,Hide=false,LastUpdate=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy,CategoryId=6},
                    new FieldsCategory(){ TenantId=school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,IsSystemCategory=true,Search=true, Title="Medical Info",Module="Student",SortOrder=5,Required=true,Hide=false,LastUpdate=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy,CategoryId=7},
                    new FieldsCategory(){ TenantId=school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,IsSystemCategory=true,Search=true, Title="Comments",Module="Student",SortOrder=6,Required=true,Hide=false,LastUpdate=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy,CategoryId=8},
                    new FieldsCategory(){ TenantId=school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,IsSystemCategory=true,Search=true, Title="Documents",Module="Student",SortOrder=7,Required=true,Hide=false,LastUpdate=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy,CategoryId=9},

                    new FieldsCategory(){ TenantId=school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,IsSystemCategory=true,Search=true, Title="General Info",Module="Parent",SortOrder=1,Required=true,Hide=false,LastUpdate=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy,CategoryId=10},
                    new FieldsCategory(){ TenantId=school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,IsSystemCategory=true,Search=true, Title="Address Info",Module="Parent",SortOrder=2,Required=true,Hide=false,LastUpdate=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy,CategoryId=11},

                    new FieldsCategory(){ TenantId=school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,IsSystemCategory=true,Search=true, Title="General Info",Module="Staff",SortOrder=1,Required=true,Hide=false,LastUpdate=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy,CategoryId=12},
                    new FieldsCategory(){ TenantId=school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,IsSystemCategory=true,Search=true, Title="School Info",Module="Staff",SortOrder=2,Required=true,Hide=false,LastUpdate=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy,CategoryId=13},
                    new FieldsCategory(){ TenantId=school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,IsSystemCategory=true,Search=true, Title="Address & Contact",Module="Staff",SortOrder=3,Required=true,Hide=false,LastUpdate=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy,CategoryId=14},
                    new FieldsCategory(){ TenantId=school.schoolMaster.TenantId,SchoolId=school.schoolMaster.SchoolId,IsSystemCategory=true,Search=true, Title="Certification Info",Module="Staff",SortOrder=4,Required=true,Hide=false,LastUpdate=DateTime.UtcNow,UpdatedBy=school.schoolMaster.ModifiedBy,CategoryId=15}


                };
                    school.schoolMaster.StudentEnrollmentCode = new List<StudentEnrollmentCode>()
                {
                     new StudentEnrollmentCode(){TenantId=school.schoolMaster.TenantId, SchoolId=school.schoolMaster.SchoolId, EnrollmentCode=1, Title="New", ShortName="NEW", SortOrder=1, Type="Add", LastUpdated=DateTime.UtcNow, UpdatedBy=school.schoolMaster.CreatedBy },
                     new StudentEnrollmentCode(){TenantId=school.schoolMaster.TenantId, SchoolId=school.schoolMaster.SchoolId, EnrollmentCode=2, Title="Dropped Out", ShortName="DROP", SortOrder=2, Type="Drop", LastUpdated=DateTime.UtcNow, UpdatedBy=school.schoolMaster.CreatedBy },
                     new StudentEnrollmentCode(){TenantId=school.schoolMaster.TenantId, SchoolId=school.schoolMaster.SchoolId, EnrollmentCode=3, Title="Rolled Over", ShortName="ROLL", SortOrder=3, Type="Rolled Over", LastUpdated=DateTime.UtcNow, UpdatedBy=school.schoolMaster.CreatedBy },
                     new StudentEnrollmentCode(){TenantId=school.schoolMaster.TenantId, SchoolId=school.schoolMaster.SchoolId, EnrollmentCode=4, Title="Transferred In", ShortName="TRAN", SortOrder=4, Type="Enroll (Transfer)", LastUpdated=DateTime.UtcNow, UpdatedBy=school.schoolMaster.CreatedBy },
                     new StudentEnrollmentCode(){TenantId=school.schoolMaster.TenantId, SchoolId=school.schoolMaster.SchoolId, EnrollmentCode=5, Title="Transferred Out", ShortName="TRAN", SortOrder=5, Type="Drop (Transfer)", LastUpdated=DateTime.UtcNow, UpdatedBy=school.schoolMaster.CreatedBy }
                };

                    school.schoolMaster.Block = new List<Block>()
                {
                     new Block(){TenantId=school.schoolMaster.TenantId, SchoolId=school.schoolMaster.SchoolId, BlockId=1, BlockTitle="All Day", BlockSortOrder=1, CreatedOn=DateTime.UtcNow, CreatedBy=school.schoolMaster.CreatedBy }
                };

                ReleaseNumber releaseNumber = new ReleaseNumber();
                {
                        releaseNumber.TenantId = school.schoolMaster.TenantId;
                        releaseNumber.SchoolId = school.schoolMaster.SchoolId;
                        releaseNumber.ReleaseNumber1 = "v1.0.0";
                        releaseNumber.ReleaseDate = DateTime.UtcNow;
                }
                

                    //insert into permission group
                    var dataGroup = System.IO.File.ReadAllText(@"Group.json");
                    JsonSerializerSettings settingGrp = new JsonSerializerSettings();
                    List<PermissionGroup> objGroup = JsonConvert.DeserializeObject<List<PermissionGroup>>(dataGroup, settingGrp);

                    foreach (PermissionGroup permisionGrp in objGroup)
                    {

                        permisionGrp.TenantId = school.schoolMaster.TenantId;
                        permisionGrp.SchoolId = school.schoolMaster.SchoolId;
                        permisionGrp.IsActive = true;
                        permisionGrp.PermissionCategory = null;
                        this.context?.PermissionGroup.Add(permisionGrp);
                        //this.context?.SaveChanges(objModel.UserName, objModel.HostName, objModel.IpAddress, objModel.Page);
                    }

                    //insert into permission category
                    var dataCategory = System.IO.File.ReadAllText(@"Category.json");
                    JsonSerializerSettings settingCat = new JsonSerializerSettings();
                    List<PermissionCategory> objCat = JsonConvert.DeserializeObject<List<PermissionCategory>>(dataCategory, settingCat);
                    foreach (PermissionCategory permissionCate in objCat)
                    {
                        permissionCate.TenantId = school.schoolMaster.TenantId;
                        permissionCate.SchoolId = school.schoolMaster.SchoolId;
                        permissionCate.PermissionGroup = null;
                        permissionCate.RolePermission = null;
                        permissionCate.CreatedBy = school.schoolMaster.CreatedBy;
                        permissionCate.CreatedOn = DateTime.UtcNow;
                        this.context?.PermissionCategory.Add(permissionCate);
                        //this.context?.SaveChanges(objModel.UserName, objModel.HostName, objModel.IpAddress, objModel.Page);
                    }

                    //insert into permission subcategory
                    var dataSubCategory = System.IO.File.ReadAllText(@"SubCategory.json");
                    JsonSerializerSettings settingSubCat = new JsonSerializerSettings();
                    List<PermissionSubcategory> objSubCat = JsonConvert.DeserializeObject<List<PermissionSubcategory>>(dataSubCategory, settingSubCat);
                    foreach (PermissionSubcategory permissionSubCate in objSubCat)
                    {
                        permissionSubCate.TenantId = school.schoolMaster.TenantId;
                        permissionSubCate.SchoolId = school.schoolMaster.SchoolId;
                        permissionSubCate.RolePermission = null;
                        permissionSubCate.CreatedBy = school.schoolMaster.CreatedBy;
                        permissionSubCate.CreatedOn = DateTime.UtcNow;
                        this.context?.PermissionSubcategory.Add(permissionSubCate);
                        //this.context?.SaveChanges(objModel.UserName, objModel.HostName, objModel.IpAddress, objModel.Page);
                    }

                    //insert into role permission
                    var dataRolePermission = System.IO.File.ReadAllText(@"RolePermission.json");
                    JsonSerializerSettings settingRole = new JsonSerializerSettings();
                    List<RolePermission> objRole = JsonConvert.DeserializeObject<List<RolePermission>>(dataRolePermission, settingRole);
                    foreach (RolePermission permissionRole in objRole)
                    {
                        permissionRole.TenantId = school.schoolMaster.TenantId;
                        permissionRole.SchoolId = school.schoolMaster.SchoolId;
                        permissionRole.PermissionCategory = null;
                        permissionRole.Membership = null;
                        permissionRole.CreatedBy = school.schoolMaster.CreatedBy;
                        permissionRole.CreatedOn = DateTime.UtcNow;
                        this.context?.RolePermission.Add(permissionRole);
                        //this.context?.SaveChanges(objModel.UserName, objModel.HostName, objModel.IpAddress, objModel.Page);
                    }

                    this.context?.SchoolMaster.Add(school.schoolMaster);
                    this.context?.ReleaseNumber.Add(releaseNumber);
                    this.context?.SaveChanges();
                    transaction.Commit();
                    school._failure = false;
                    school._message = "School Added Successfully";

                    school.schoolMaster.Membership = null;
                    school.schoolMaster.DpdownValuelist = null;
                    school.schoolMaster.PermissionGroup = null;

                    //school.schoolMaster.Membership.ToList().ForEach(x=>x.SchoolMaster=null);
                    /*if (school.schoolMaster.SchoolDetail.ToList().Count>0)
                    {
                        school.schoolMaster.SchoolDetail.FirstOrDefault().SchoolMaster = null;
                    }*/
                    //school.schoolMaster.FieldsCategory.ToList().ForEach(x => x.SchoolMaster = null);
                    /*if (school.schoolMaster.SchoolDetail.ToList().Count > 0)
                    {
                        school.schoolMaster.SchoolDetail.FirstOrDefault().SchoolMaster = null;
                    }*/                   

                }
                catch (Exception es)
                {
                    transaction.Rollback();
                    school._failure = true;
                    school._message = es.Message;
                }
            }
            return school;
        }
        private bool CheckInternalID(Guid TenantId, string InternalID)
        {
            if (InternalID != null && InternalID != "")
            {
                var checkInternalId = this.context?.SchoolMaster.Where(x => x.TenantId == TenantId && x.SchoolInternalId == InternalID).ToList();
                if (checkInternalId.Count() > 0)
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
                return true;
            }
        }


        /// <summary>
        ///  Check School InternalId Exist or Not
        /// </summary>
        /// <param name="checkSchoolInternalIdViewModel"></param>
        /// <returns></returns>
        public CheckSchoolInternalIdViewModel CheckSchoolInternalId(CheckSchoolInternalIdViewModel checkSchoolInternalIdViewModel)
        {
            var checkInternalId = this.context?.SchoolMaster.Where(x => x.TenantId == checkSchoolInternalIdViewModel.TenantId && x.SchoolInternalId == checkSchoolInternalIdViewModel.SchoolInternalId).ToList();
            if (checkInternalId.Count() > 0)
            {
                checkSchoolInternalIdViewModel.IsValidInternalId = false;
                checkSchoolInternalIdViewModel._message = "School Internal Id Already Exist";
            }
            else
            {
                checkSchoolInternalIdViewModel.IsValidInternalId = true;
                checkSchoolInternalIdViewModel._message = "School Internal Id Is Valid";
            }
            return checkSchoolInternalIdViewModel;
        }
        /// <summary>
        /// Student Enrollment School List
        /// </summary>
        /// <param name="schoolListViewModel"></param>
        /// <returns></returns>
        public SchoolListViewModel StudentEnrollmentSchoolList(SchoolListViewModel schoolListViewModel)
        {
            SchoolListViewModel schoolListView = new SchoolListViewModel();
            try
            {
                var schoolListWithGradeLevel = this.context?.SchoolMaster.Include(x=>x.SchoolDetail).Include(x=>x.Gradelevels).Include(x=>x.StudentEnrollmentCode).Where(x => x.TenantId == schoolListViewModel.TenantId && x.SchoolDetail.FirstOrDefault().Status==true).ToList();
                schoolListView.schoolMaster = schoolListWithGradeLevel;
                schoolListView._tenantName = schoolListViewModel._tenantName;
                schoolListView._token = schoolListViewModel._token;
                schoolListView._failure = false;
            }
            catch (Exception es)
            {
                schoolListView._message = es.Message;
                schoolListView._failure = true;
                schoolListView._tenantName = schoolListViewModel._tenantName;
                schoolListView._token = schoolListViewModel._token;
            }
            return schoolListView;
        }

        /// <summary>
        /// Add Update School Logo
        /// </summary>
        /// <param name="schoolAddViewModel"></param>
        /// <returns></returns>
        public SchoolAddViewModel AddUpdateSchoolLogo(SchoolAddViewModel schoolAddViewModel)
        {
            try
            {
                var schoolLogoUpdate = this.context?.SchoolDetail.FirstOrDefault(x => x.TenantId == schoolAddViewModel.schoolMaster.TenantId && x.SchoolId == schoolAddViewModel.schoolMaster.SchoolId && x.Id == schoolAddViewModel.schoolMaster.SchoolDetail.FirstOrDefault().Id);
                
                if (schoolLogoUpdate != null)
                {
                    schoolLogoUpdate.SchoolLogo = schoolAddViewModel.schoolMaster.SchoolDetail.FirstOrDefault().SchoolLogo;
                    this.context?.SaveChanges();
                    schoolAddViewModel._message = "School Logo Updated Successfully";
                }
                else
                {
                    schoolAddViewModel._failure = true;
                    schoolAddViewModel._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {
                schoolAddViewModel._failure = true;
                schoolAddViewModel._message = es.Message;
            }
            return schoolAddViewModel;
        }
    }
}
