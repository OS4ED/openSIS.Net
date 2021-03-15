using opensis.data.Helper;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.CommonModel;
using opensis.data.ViewModels.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace opensis.data.Repository
{
    public class CommonRepository : ICommonRepository
    {
        private CRMContext context;
        private static readonly string NORECORDFOUND = "No Record Found";
        public CommonRepository(IDbContextFactory dbContextFactory)
        {
            this.context = dbContextFactory.Create();
        }
        public CityListModel GetAllCitiesByState(CityListModel city)
        {
            CityListModel cityListModel = new CityListModel();
            try
            {
                var CityList = this.context?.City.Where(x => x.StateId == city.StateId).ToList();
                if (CityList.Count > 0)
                {
                    cityListModel.TableCity = CityList;
                }
                cityListModel._tenantName = city._tenantName;
                cityListModel._token = city._token;
                cityListModel._failure = false;
            }
            catch (Exception es)
            {
                cityListModel._message = es.Message;
                cityListModel._failure = true;
                cityListModel._tenantName = city._tenantName;
                cityListModel._token = city._token;
            }
            return cityListModel;

        }
        public StateListModel GetAllStatesByCountry(StateListModel state)
        {
            StateListModel stateListModel = new StateListModel();
            try
            {
                stateListModel.TableState = null;
                var StateList = this.context?.State.Where(x => x.CountryId == state.CountryId).ToList();
                if (StateList.Count > 0)
                {
                    stateListModel.TableState = StateList;
                }
                stateListModel._tenantName = state._tenantName;
                stateListModel._token = state._token;
                stateListModel._failure = false;
            }
            catch (Exception es)
            {
                stateListModel._message = es.Message;
                stateListModel._failure = true;
                stateListModel._tenantName = state._tenantName;
                stateListModel._token = state._token;
            }
            return stateListModel;

        }
        public CountryListModel GetAllCountries(CountryListModel country)
        {
            CountryListModel countryListModel = new CountryListModel();
            try
            {
                var CountryList = this.context?.Country.ToList();

                countryListModel.TableCountry = CountryList;

                if (CountryList.Count > 0)
                {
                    int? StateCount = this.context?.State.Count();
                    countryListModel.StateCount = StateCount;
                    countryListModel._failure = false;
                }
                else
                {
                    countryListModel._failure = true;
                    countryListModel._message = NORECORDFOUND;
                }
                countryListModel._tenantName = country._tenantName;
                countryListModel._token = country._token;

            }
            catch (Exception es)
            {
                countryListModel._message = es.Message;
                countryListModel._failure = true;
                countryListModel._tenantName = country._tenantName;
                countryListModel._token = country._token;
            }
            return countryListModel;

        }

        /// <summary>
        /// Add Language 
        /// </summary>
        /// <param name="languageAdd"></param>
        /// <returns></returns>
        public LanguageAddModel AddLanguage(LanguageAddModel languageAdd)
        {
            try
            {
                var getLanguage = this.context?.Language.FirstOrDefault(x => x.Locale.ToLower() == languageAdd.Language.Locale.ToLower());

                if (getLanguage != null)
                {
                    languageAdd._message = "This Language Name Already Exist";
                    languageAdd._failure = true;
                }
                else
                {
                    int? languageId = Utility.GetMaxPK(this.context, new Func<Language, int>(x => x.LangId));
                    languageAdd.Language.LangId = (int)languageId;
                    languageAdd.Language.CreatedOn = DateTime.UtcNow;
                    this.context?.Language.Add(languageAdd.Language);
                    this.context?.SaveChanges();
                    languageAdd._failure = false;
                    languageAdd._message = "Language Added Successfully";
                }
            }
            catch (Exception es)
            {
                languageAdd._message = es.Message;
                languageAdd._failure = true;
                languageAdd._tenantName = languageAdd._tenantName;
                languageAdd._token = languageAdd._token;
            }
            return languageAdd;
        }

        /// <summary>
        /// View Language 
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public LanguageAddModel ViewLanguage(LanguageAddModel language)
        {
            try
            {
                LanguageAddModel languageAddModel = new LanguageAddModel();
                var getLanguageValue = this.context?.Language.FirstOrDefault(x => x.LangId == language.Language.LangId);
                if (getLanguageValue != null)
                {
                    languageAddModel.Language = getLanguageValue;
                    languageAddModel._failure = false;
                    return languageAddModel;
                }
                else
                {
                    languageAddModel._failure = true;
                    languageAddModel._message = NORECORDFOUND;
                    return languageAddModel;
                }
            }
            catch (Exception es)
            {

                throw;
            }
        }


        /// <summary>
        /// Language Update
        /// </summary>
        /// <param name="languageUpdate"></param>
        /// <returns></returns>
        public LanguageAddModel UpdateLanguage(LanguageAddModel languageUpdate)
        {
            var getLanguage = this.context?.Language.Where(x => x.Locale.ToLower() == languageUpdate.Language.Locale.ToLower() && x.LangId != languageUpdate.Language.LangId).ToList();

            if (getLanguage.Count > 0)
            {
                languageUpdate._message = "This Language Name Already Exist";
                languageUpdate._failure = true;
            }
            else
            {
                var getLanguageData = this.context?.Language.FirstOrDefault(x => x.LangId == languageUpdate.Language.LangId);

                if (getLanguageData.Lcid.ToLower() == "en-us".ToLower() || getLanguageData.Lcid.ToLower() == "fr-fr".ToLower() || getLanguageData.Lcid.ToLower() == "es-es".ToLower())
                {
                    languageUpdate._message = "This Language is not editable";
                    languageUpdate._failure = true;
                }
                else
                {
                    languageUpdate.Language.CreatedBy = getLanguageData.CreatedBy;
                    languageUpdate.Language.CreatedOn = getLanguageData.CreatedOn;
                    languageUpdate.Language.UpdatedOn = DateTime.Now;
                    this.context.Entry(getLanguageData).CurrentValues.SetValues(languageUpdate.Language);
                    this.context?.SaveChanges();
                    languageUpdate._failure = false;
                    languageUpdate._message = "Language Updated Successfully";
                }   
            }
            return languageUpdate;
        }



        public LanguageListModel GetAllLanguage(LanguageListModel language)
        {
            LanguageListModel languageListModel = new LanguageListModel();
            try
            {
                var languages = this.context?.Language.ToList();

                languageListModel.TableLanguage = languages;

                if (languages.Count > 0)
                {
                    languageListModel._failure = false;
                }
                else
                {
                    languageListModel._failure = true;
                    languageListModel._message = NORECORDFOUND;
                }
                languageListModel._tenantName = language._tenantName;
                languageListModel._token = language._token;

            }
            catch (Exception es)
            {
                languageListModel._message = es.Message;
                languageListModel._failure = true;
                languageListModel._tenantName = language._tenantName;
                languageListModel._token = language._token;
            }
            return languageListModel;

        }

        /// <summary>
        /// Delete Language
        /// </summary>
        /// <param name="languageAddModel"></param>
        /// <returns></returns>
        public LanguageAddModel DeleteLanguage(LanguageAddModel languageAddModel)
        {
            LanguageAddModel deleteLanguageModel = new LanguageAddModel();
            try
            {
                var getLanguageValue = this.context?.Language.FirstOrDefault(x => x.LangId == languageAddModel.Language.LangId);

                if (getLanguageValue != null)
                {

                    var getStudentLanguageData = this.context?.StudentMaster.Where(x => x.FirstLanguageId == languageAddModel.Language.LangId || x.SecondLanguageId == languageAddModel.Language.LangId || x.ThirdLanguageId == languageAddModel.Language.LangId).ToList();

                    if (getLanguageValue.Lcid.ToLower() == "en-us".ToLower() || getLanguageValue.Lcid.ToLower() == "fr-fr".ToLower() || getLanguageValue.Lcid.ToLower() == "es-es".ToLower())
                    {
                        deleteLanguageModel._message = "This Language is not deletable";
                        deleteLanguageModel._failure = true;
                        return deleteLanguageModel;
                    }
                    if (getStudentLanguageData.Count > 0)
                    {
                        deleteLanguageModel._message = "Language cannot be deleted because it has its association";
                        deleteLanguageModel._failure = true;
                        return deleteLanguageModel;
                    }
                    var getStaffLanguageData = this.context?.StaffMaster.Where(x => x.FirstLanguage == languageAddModel.Language.LangId || x.SecondLanguage == languageAddModel.Language.LangId || x.ThirdLanguage == languageAddModel.Language.LangId).ToList();
                    if (getStaffLanguageData.Count > 0)
                    {
                        deleteLanguageModel._message = "Language cannot be deleted because it has its association";
                        deleteLanguageModel._failure = true;
                        return deleteLanguageModel;
                    }
                    var getUserLanguageData = this.context?.UserMaster.Where(x => x.LangId == languageAddModel.Language.LangId).ToList();
                    if (getUserLanguageData.Count > 0)
                    {
                        deleteLanguageModel._message = "Language cannot be deleted because it has its association";
                        deleteLanguageModel._failure = true;
                        return deleteLanguageModel;
                    }

                    this.context?.Language.Remove(getLanguageValue);
                    this.context?.SaveChanges();
                    deleteLanguageModel._failure = false;
                    deleteLanguageModel._message = "Language Deleted Successfully";
                }
            }
            catch (Exception ex)
            {
                deleteLanguageModel._failure = true;
                deleteLanguageModel._message = ex.Message;
            }
            return deleteLanguageModel;
        }

        /// <summary>
        /// Dropdown value Add
        /// </summary>
        /// <param name="dpdownValue"></param>
        /// <returns></returns>
        public DropdownValueAddModel AddDropdownValue(DropdownValueAddModel dpdownValue)
        {
            var getDpvalue = this.context?.DpdownValuelist.FirstOrDefault(x => x.LovColumnValue.ToLower() == dpdownValue.DropdownValue.LovColumnValue.ToLower() && x.LovName.ToLower() == dpdownValue.DropdownValue.LovName.ToLower() && x.SchoolId == dpdownValue.DropdownValue.SchoolId);

            if (getDpvalue != null)
            {
                dpdownValue._message = "This Title Already Exist";
                dpdownValue._failure = true;
            }
            else
            {
                long? dpdownValueId = Utility.GetMaxLongPK(this.context, new Func<DpdownValuelist, long>(x => x.Id));
                dpdownValue.DropdownValue.Id = (long)dpdownValueId;
                dpdownValue.DropdownValue.CreatedOn = DateTime.UtcNow;
                this.context?.DpdownValuelist.Add(dpdownValue.DropdownValue);
                this.context?.SaveChanges();
                dpdownValue._failure = false;
                dpdownValue._message = "Data Added Successfully";
            }
            return dpdownValue;
        }

        /// <summary>
        /// Dropdown value Update
        /// </summary>
        /// <param name="dpdownValue"></param>
        /// <returns></returns>
        public DropdownValueAddModel UpdateDropdownValue(DropdownValueAddModel dpdownValue)
        {
            using (var transaction = this.context.Database.BeginTransaction())
            {
                try
                {
                    var getDpdownData = this.context?.DpdownValuelist.Where(x => x.LovColumnValue.ToLower() == dpdownValue.DropdownValue.LovColumnValue.ToLower() && x.LovName.ToLower() == dpdownValue.DropdownValue.LovName.ToLower() && x.Id != dpdownValue.DropdownValue.Id && x.SchoolId == dpdownValue.DropdownValue.SchoolId).ToList();

                    if (getDpdownData.Count > 0)
                    {
                        dpdownValue._message = "This Title Already Exist";
                        dpdownValue._failure = true;
                    }
                    else
                    {

                        var getDpdownValue = this.context?.DpdownValuelist.FirstOrDefault(x => x.TenantId == dpdownValue.DropdownValue.TenantId && x.SchoolId == dpdownValue.DropdownValue.SchoolId && x.Id == dpdownValue.DropdownValue.Id);

                        if (getDpdownValue != null)
                        {

                            switch (getDpdownValue.LovName)
                            {
                                case "Race":
                                    var raceValueStaff = this.context?.StaffMaster.Where(x => x.Race.ToLower() == getDpdownValue.LovColumnValue.ToLower()).ToList();
                                    var raceValueStudent = this.context?.StudentMaster.Where(x => x.Race.ToLower() == getDpdownValue.LovColumnValue.ToLower()).ToList();

                                    if (raceValueStaff.Count > 0)
                                    {
                                        foreach (var staff in raceValueStaff)
                                        {
                                            staff.Race = dpdownValue.DropdownValue.LovColumnValue;
                                        }
                                        this.context?.StaffMaster.UpdateRange(raceValueStaff);
                                    }
                                    if (raceValueStudent.Count > 0)
                                    {
                                        foreach (var student in raceValueStudent)
                                        {
                                            student.Race = dpdownValue.DropdownValue.LovColumnValue;
                                        }
                                        this.context?.StudentMaster.UpdateRange(raceValueStudent);
                                    }
                                    break;
                                case "School Level":
                                    var schoolLevelValue = this.context?.SchoolMaster.Where(x => x.SchoolLevel.ToLower() == getDpdownValue.LovColumnValue.ToLower()).ToList();

                                    if (schoolLevelValue.Count > 0)
                                    {
                                        foreach (var schoolLevel in schoolLevelValue)
                                        {
                                            schoolLevel.SchoolLevel = dpdownValue.DropdownValue.LovColumnValue;
                                        }
                                        this.context?.SchoolMaster.UpdateRange(schoolLevelValue);

                                    }

                                    break;
                                case "School Classification":
                                    var schoolClassificationValue = this.context?.SchoolMaster.Where(x => x.SchoolClassification.ToLower() == getDpdownValue.LovColumnValue.ToLower()).ToList();

                                    if (schoolClassificationValue.Count > 0)
                                    {
                                        foreach (var schoolClassification in schoolClassificationValue)
                                        {
                                            schoolClassification.SchoolClassification = dpdownValue.DropdownValue.LovColumnValue;
                                        }
                                        this.context?.SchoolMaster.UpdateRange(schoolClassificationValue);
                                    }

                                    break;
                                case "Female Toilet Type":
                                    var femaleToiletTypeValue = this.context?.SchoolDetail.Where(x => x.FemaleToiletType.ToLower() == getDpdownValue.LovColumnValue.ToLower()).ToList();

                                    if (femaleToiletTypeValue.Count > 0)
                                    {
                                        foreach (var femaleToilet in femaleToiletTypeValue)
                                        {
                                            femaleToilet.FemaleToiletType = dpdownValue.DropdownValue.LovColumnValue;
                                        }
                                        this.context?.SchoolDetail.UpdateRange(femaleToiletTypeValue);
                                    }

                                    break;
                                case "Male Toilet Type":
                                    var maleToiletTypeValue = this.context?.SchoolDetail.Where(x => x.MaleToiletType.ToLower() == getDpdownValue.LovColumnValue.ToLower()).ToList();

                                    if (maleToiletTypeValue.Count > 0)
                                    {
                                        foreach (var maleToilet in maleToiletTypeValue)
                                        {
                                            maleToilet.MaleToiletType = dpdownValue.DropdownValue.LovColumnValue;
                                        }
                                        this.context?.SchoolDetail.UpdateRange(maleToiletTypeValue);
                                    }

                                    break;
                                case "Common Toilet Type":
                                    var commonToiletTypeValue = this.context?.SchoolDetail.Where(x => x.ComonToiletType.ToLower() == getDpdownValue.LovColumnValue.ToLower()).ToList();

                                    if (commonToiletTypeValue.Count > 0)
                                    {
                                        foreach (var ComonToilet in commonToiletTypeValue)
                                        {
                                            ComonToilet.ComonToiletType = dpdownValue.DropdownValue.LovColumnValue;
                                        }
                                        this.context?.SchoolDetail.UpdateRange(commonToiletTypeValue);
                                    }

                                    break;
                                case "Ethnicity":
                                    var ethnicityValueStaff = this.context?.StaffMaster.Where(x => x.Ethnicity.ToLower() == getDpdownValue.LovColumnValue.ToLower()).ToList();
                                    var ethnicityValueStudent = this.context?.StudentMaster.Where(x => x.Ethnicity.ToLower() == getDpdownValue.LovColumnValue.ToLower()).ToList();

                                    if (ethnicityValueStaff.Count > 0)
                                    {
                                        foreach (var staff in ethnicityValueStaff)
                                        {
                                            staff.Ethnicity = dpdownValue.DropdownValue.LovColumnValue;
                                        }
                                        this.context?.StaffMaster.UpdateRange(ethnicityValueStaff);
                                    }
                                    if (ethnicityValueStudent.Count > 0)
                                    {
                                        foreach (var student in ethnicityValueStudent)
                                        {
                                            student.Ethnicity = dpdownValue.DropdownValue.LovColumnValue;
                                        }
                                        this.context?.StudentMaster.UpdateRange(ethnicityValueStudent);
                                    }
                                    break;
                            }
                        }

                        getDpdownValue.LovColumnValue = dpdownValue.DropdownValue.LovColumnValue;
                        getDpdownValue.UpdatedOn = DateTime.UtcNow;
                        getDpdownValue.UpdatedBy = dpdownValue.DropdownValue.UpdatedBy;
                        this.context?.SaveChanges();
                        dpdownValue._failure = false;
                        dpdownValue._message = "Data Updated Successfully";
                        transaction.Commit();
                    }
                    return dpdownValue;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    dpdownValue.DropdownValue = null;
                    dpdownValue._failure = true;
                    dpdownValue._message = ex.Message;
                    return dpdownValue;
                }
            }
        }


        /// <summary>
        /// Get DropdownValue By Id
        /// </summary>
        /// <param name="dpdownValue"></param>
        /// <returns></returns>
        public DropdownValueAddModel ViewDropdownValue(DropdownValueAddModel dpdownValue)
        {
            try
            {
                DropdownValueAddModel dropdownValueAddModel = new DropdownValueAddModel();
                var getDpdownValue = this.context?.DpdownValuelist.FirstOrDefault(x => x.TenantId == dpdownValue.DropdownValue.TenantId && x.SchoolId == dpdownValue.DropdownValue.SchoolId && x.Id == dpdownValue.DropdownValue.Id);
                if (getDpdownValue != null)
                {
                    dropdownValueAddModel.DropdownValue = getDpdownValue;
                    dropdownValueAddModel._tenantName = dpdownValue._tenantName;
                    dropdownValueAddModel._failure = false;
                    return dropdownValueAddModel;
                }
                else
                {
                    dropdownValueAddModel._failure = true;
                    dropdownValueAddModel._message = NORECORDFOUND;
                    return dropdownValueAddModel;
                }
            }
            catch (Exception es)
            {

                throw;
            }
        }


        /// <summary>
        /// Get All Dropdown Value
        /// </summary>
        /// <param name="dpdownList"></param>
        /// <returns></returns>
        public DropdownValueListModel GetAllDropdownValues(DropdownValueListModel dpdownList)
        {
            DropdownValueListModel dpdownListModel = new DropdownValueListModel();
            try
            {

                var dropdownList = this.context?.DpdownValuelist.Where(x => x.TenantId == dpdownList.TenantId && x.SchoolId == dpdownList.SchoolId && x.LovName.ToLower() == dpdownList.LovName.ToLower()).ToList();

                dpdownListModel.dropdownList = dropdownList;

                if (dropdownList.Count > 0)
                {
                    dpdownListModel._failure = false;
                }
                else
                {
                    dpdownListModel._failure = true;
                    dpdownListModel._message = NORECORDFOUND;
                }
                dpdownListModel._tenantName = dpdownList._tenantName;
                dpdownListModel._token = dpdownList._token;

            }
            catch (Exception es)
            {
                dpdownListModel._message = es.Message;
                dpdownListModel._failure = true;
                dpdownListModel._tenantName = dpdownList._tenantName;
                dpdownListModel._token = dpdownList._token;
            }
            return dpdownListModel;

        }

        /// <summary>
        /// Add Country
        /// </summary>
        /// <param name="countryAddModel"></param>
        /// <returns></returns>
        public CountryAddModel AddCountry(CountryAddModel countryAddModel)
        {
            try
            {
                var getCountry = this.context?.Country.FirstOrDefault(x => x.Name.ToLower() == countryAddModel.country.Name.ToLower());

                if (getCountry != null)
                {
                    countryAddModel._message = "This Country Name Already Exist";
                    countryAddModel._failure = true;
                }
                else
                {
                    int? CountryId = Utility.GetMaxPK(this.context, new Func<Country, int>(x => x.Id));
                    countryAddModel.country.Id = (int)CountryId;
                    countryAddModel.country.CreatedOn = DateTime.UtcNow;
                    this.context?.Country.Add(countryAddModel.country);
                    this.context?.SaveChanges();
                    countryAddModel._failure = true;
                    countryAddModel._message = "Country Added Successfully";
                }
            }
            catch (Exception es)
            {
                countryAddModel._message = es.Message;
                countryAddModel._failure = true;
                countryAddModel._tenantName = countryAddModel._tenantName;
                countryAddModel._token = countryAddModel._token;
            }
            return countryAddModel;
        }

        /// <summary>
        /// Update Country
        /// </summary>
        /// <param name="countryAddModel"></param>
        /// <returns></returns>
        public CountryAddModel UpdateCountry(CountryAddModel countryAddModel)
        {
            try
            {
                var getCountry = this.context?.Country.Where(x => x.Name.ToLower() == countryAddModel.country.Name.ToLower() && x.Id != countryAddModel.country.Id).ToList();

                if (getCountry.Count > 0)
                {
                    countryAddModel._message = "This Country Name Already Exist";
                    countryAddModel._failure = true;
                }
                else
                {
                    var getCountryData = this.context?.Country.FirstOrDefault(x => x.Id == countryAddModel.country.Id);

                    countryAddModel.country.CreatedBy = getCountryData.CreatedBy;
                    countryAddModel.country.CreatedOn = getCountryData.CreatedOn;
                    countryAddModel.country.UpdatedOn = DateTime.Now;
                    this.context.Entry(getCountryData).CurrentValues.SetValues(countryAddModel.country);
                    this.context?.SaveChanges();
                    countryAddModel._failure = false;
                    countryAddModel._message = "Country Updated Successfully";
                }
            }
            catch (Exception es)
            {
                countryAddModel._message = es.Message;
                countryAddModel._failure = true;
                countryAddModel._tenantName = countryAddModel._tenantName;
                countryAddModel._token = countryAddModel._token;
            }
            return countryAddModel;
        }

        /// <summary>
        /// Delete Country
        /// </summary>
        /// <param name="countryDeleteModel"></param>
        /// <returns></returns>
        public CountryAddModel DeleteCountry(CountryAddModel countryDeleteModel)
        {
            CountryAddModel deleteCountryModel = new CountryAddModel();
            try
            {
                var getCountryValue = this.context?.Country.FirstOrDefault(x => x.Id == countryDeleteModel.country.Id);

                if (getCountryValue != null)
                {
                    var getPatrentCountryData = this.context?.ParentAddress.Where(x => x.Country == countryDeleteModel.country.Id.ToString()).ToList();
                    if (getPatrentCountryData.Count > 0)
                    {
                        deleteCountryModel._message = "Country cannot be deleted because it has its association";
                        deleteCountryModel._failure = true;
                        return deleteCountryModel;
                    }
                    var getStudentCountryData = this.context?.StudentMaster.Where(x => x.CountryOfBirth == countryDeleteModel.country.Id || x.HomeAddressCountry == countryDeleteModel.country.Id.ToString() || x.MailingAddressCountry == countryDeleteModel.country.Id.ToString()).ToList();
                    if (getStudentCountryData.Count > 0)
                    {
                        deleteCountryModel._message = "Country cannot be deleted because it has its association";
                        deleteCountryModel._failure = true;
                        return deleteCountryModel;
                    }
                    var getStaffCountryData = this.context?.StaffMaster.Where(x => x.CountryOfBirth == countryDeleteModel.country.Id || x.HomeAddressCountry == countryDeleteModel.country.Id.ToString() || x.MailingAddressCountry == countryDeleteModel.country.Id.ToString()).ToList();
                    if (getStaffCountryData.Count > 0)
                    {
                        deleteCountryModel._message = "Country cannot be deleted because it has its association";
                        deleteCountryModel._failure = true;
                        return deleteCountryModel;
                    }
                    var getSchoolCountryData = this.context?.SchoolMaster.Where(x => x.Country == countryDeleteModel.country.Id.ToString()).ToList();
                    if (getSchoolCountryData.Count > 0)
                    {
                        deleteCountryModel._message = "Country cannot be deleted because it has its association";
                        deleteCountryModel._failure = true;
                        return deleteCountryModel;
                    }
                    var getSateData = this.context?.State.Where(x => x.CountryId == countryDeleteModel.country.Id).ToList();
                    if (getSateData.Count > 0)
                    {
                        deleteCountryModel._message = "Country cannot be deleted because it has its association";
                        deleteCountryModel._failure = true;
                        return deleteCountryModel;
                    }

                    this.context?.Country.Remove(getCountryValue);
                    this.context?.SaveChanges();
                    deleteCountryModel._failure = false;
                    deleteCountryModel._message = "Country Deleted Successfully";

                }

            }
            catch (Exception ex)
            {
                deleteCountryModel._failure = true;
                deleteCountryModel._message = ex.Message;
            }
            return deleteCountryModel;
        }

        /// <summary>
        /// Delete Dropdown Value
        /// </summary>
        /// <param name="dpdownValue"></param>
        /// <returns></returns>
        public DropdownValueAddModel DeleteDropdownValue(DropdownValueAddModel dpdownValue)
        {
            DropdownValueAddModel dropdownValueAddModel = new DropdownValueAddModel();
            try
            {
                var getDpValue = this.context?.DpdownValuelist.FirstOrDefault(x => x.Id == dpdownValue.DropdownValue.Id);

                if (getDpValue != null)
                {

                    switch (getDpValue.LovName)
                    {
                        case "Race":

                            var raceValueStaff = this.context?.StaffMaster.Where(x => x.Race.ToLower() == getDpValue.LovColumnValue.ToLower() && x.SchoolId == getDpValue.SchoolId).ToList();
                            if (raceValueStaff.Count > 0)
                            {
                                dropdownValueAddModel._message = "Race cannot be deleted because it has its association";
                                dropdownValueAddModel._failure = true;
                                return dropdownValueAddModel;
                            }

                            var raceValueStudent = this.context?.StudentMaster.Where(x => x.Race.ToLower() == getDpValue.LovColumnValue.ToLower() && x.SchoolId == getDpValue.SchoolId).ToList();

                            if (raceValueStudent.Count > 0)
                            {
                                dropdownValueAddModel._message = "Race cannot be deleted because it has its association";
                                dropdownValueAddModel._failure = true;
                                return dropdownValueAddModel;
                            }


                            break;
                        case "School Level":
                            var schoolLevelValue = this.context?.SchoolMaster.Where(x => x.SchoolLevel.ToLower() == getDpValue.LovColumnValue.ToLower() && x.SchoolId == getDpValue.SchoolId).ToList();

                            if (schoolLevelValue.Count > 0)
                            {
                                dropdownValueAddModel._message = "School Level cannot be deleted because it has its association";
                                dropdownValueAddModel._failure = true;
                                return dropdownValueAddModel;
                            }

                            break;
                        case "School Classification":
                            var schoolClassificationValue = this.context?.SchoolMaster.Where(x => x.SchoolClassification.ToLower() == getDpValue.LovColumnValue.ToLower() && x.SchoolId == getDpValue.SchoolId).ToList();

                            if (schoolClassificationValue.Count > 0)
                            {
                                dropdownValueAddModel._message = "School Classification cannot be deleted because it has its association";
                                dropdownValueAddModel._failure = true;
                                return dropdownValueAddModel;
                            }

                            break;
                        case "Female Toilet Type":
                            var femaleToiletTypeValue = this.context?.SchoolDetail.Where(x => x.FemaleToiletType.ToLower() == getDpValue.LovColumnValue.ToLower() && x.SchoolId == getDpValue.SchoolId).ToList();

                            if (femaleToiletTypeValue.Count > 0)
                            {
                                dropdownValueAddModel._message = "Female Toilet Type cannot be deleted because it has its association";
                                dropdownValueAddModel._failure = true;
                                return dropdownValueAddModel;
                            }

                            break;
                        case "Male Toilet Type":
                            var maleToiletTypeValue = this.context?.SchoolDetail.Where(x => x.MaleToiletType.ToLower() == getDpValue.LovColumnValue.ToLower() && x.SchoolId == getDpValue.SchoolId).ToList();

                            if (maleToiletTypeValue.Count > 0)
                            {
                                dropdownValueAddModel._message = "Male Toilet Type cannot be deleted because it has its association";
                                dropdownValueAddModel._failure = true;
                                return dropdownValueAddModel;
                            }

                            break;
                        case "Common Toilet Type":
                            var commonToiletTypeValue = this.context?.SchoolDetail.Where(x => x.ComonToiletType.ToLower() == getDpValue.LovColumnValue.ToLower() && x.SchoolId == getDpValue.SchoolId).ToList();

                            if (commonToiletTypeValue.Count > 0)
                            {
                                dropdownValueAddModel._message = "Common Toilet Type cannot be deleted because it has its association";
                                dropdownValueAddModel._failure = true;
                                return dropdownValueAddModel;
                            }

                            break;
                        case "Ethnicity":
                            var ethnicityValueStaff = this.context?.StaffMaster.Where(x => x.Ethnicity.ToLower() == getDpValue.LovColumnValue.ToLower() && x.SchoolId == getDpValue.SchoolId).ToList();
                            if (ethnicityValueStaff.Count > 0)
                            {
                                dropdownValueAddModel._message = "Ethnicity cannot be deleted because it has its association";
                                dropdownValueAddModel._failure = true;
                                return dropdownValueAddModel;
                            }
                            var ethnicityValueStudent = this.context?.StudentMaster.Where(x => x.Ethnicity.ToLower() == getDpValue.LovColumnValue.ToLower() && x.SchoolId == getDpValue.SchoolId).ToList();

                            if (ethnicityValueStudent.Count > 0)
                            {
                                dropdownValueAddModel._message = "Ethnicity cannot be deleted because it has its association";
                                dropdownValueAddModel._failure = true;
                                return dropdownValueAddModel;
                            }

                            break;
                    }
                    this.context?.DpdownValuelist.Remove(getDpValue);
                    this.context?.SaveChanges();
                    dropdownValueAddModel._failure = false;
                    dropdownValueAddModel._message = "Data Deleted Successfully";
                }

            }
            catch (Exception es)
            {
                dropdownValueAddModel._failure = true;
                dropdownValueAddModel._message = es.Message;
            }
            return dropdownValueAddModel;

        }

        /// <summary>
        /// Get All Language For Login 
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public LanguageListModel GetAllLanguageForLogin(LanguageListModel language)
        {
            LanguageListModel languageListModel = new LanguageListModel();
            try
            {
                languageListModel.TableLanguage = null;
                var languages = this.context?.Language.Where(x => x.Lcid.ToLower() == "en-us".ToLower() || x.Lcid.ToLower() == "fr-fr".ToLower() || x.Lcid.ToLower() == "es-es".ToLower()).ToList();
                if (languages.Count > 0)
                {
                    languageListModel.TableLanguage = languages;
                    languageListModel._failure = false;
                }
                else
                {
                    languageListModel._failure = true;
                    languageListModel._message = NORECORDFOUND;
                }
                languageListModel._tenantName = language._tenantName;
                languageListModel._token = language._token;

            }
            catch (Exception es)
            {
                languageListModel._message = es.Message;
                languageListModel._failure = true;
                languageListModel._tenantName = language._tenantName;
                languageListModel._token = language._token;
            }
            return languageListModel;

        }

        /// <summary>
        /// Dashboard View
        /// </summary>
        /// <param name="dashboardViewModel"></param>
        /// <returns></returns>
        public DashboardViewModel GetDashboardView(DashboardViewModel dashboardViewModel)
        {
            DashboardViewModel dashboardView = new DashboardViewModel();
            try
            {
                var todayDate = DateTime.Today;
                dashboardView.TenantId = dashboardViewModel.TenantId;
                dashboardView.SchoolId = dashboardViewModel.SchoolId;
                dashboardView.AcademicYear = dashboardViewModel.AcademicYear;

                dashboardView.SuperAdministratorName = this.context?.UserMaster.FirstOrDefault(x => x.TenantId == dashboardViewModel.TenantId && x.SchoolId == dashboardViewModel.SchoolId && x.MembershipId == 1)?.Name;

                dashboardView.SchoolName = this.context?.SchoolMaster.FirstOrDefault(x => x.TenantId == dashboardViewModel.TenantId && x.SchoolId == dashboardViewModel.SchoolId)?.SchoolName;

                dashboardView.TotalStudent= this.context?.StudentMaster.Where(x => x.TenantId == dashboardViewModel.TenantId && x.SchoolId == dashboardViewModel.SchoolId && x.IsActive == true).ToList().Count();

                //dashboardView.TotalParent = this.context?.ParentAssociationship.Where(x => x.TenantId == dashboardViewModel.TenantId && x.SchoolId == dashboardViewModel.SchoolId && x.Associationship == true).Select(s => new { s.TenantId, s.SchoolId, s.ParentId }).Distinct().ToList().Count();

                dashboardView.TotalStaff= this.context?.StaffMaster.Where(x => x.TenantId == dashboardViewModel.TenantId && x.SchoolId == dashboardViewModel.SchoolId).ToList().Count();

                dashboardView.TotalParent = this.context?.ParentAssociationship.Where(x => x.TenantId == dashboardViewModel.TenantId && x.SchoolId == dashboardViewModel.SchoolId && x.Associationship == true).Select(x => x.ParentId).ToList().Distinct().Count();

                var notice = this.context?.Notice.Where(x => x.TenantId == dashboardViewModel.TenantId && x.SchoolId == dashboardViewModel.SchoolId && x.Isactive == true && (x.ValidFrom <= todayDate && todayDate <= x.ValidTo)).OrderByDescending(x => x.ValidFrom).FirstOrDefault();
                if (notice != null)
                {
                    dashboardView.NoticeTitle = notice.Title;
                    dashboardView.NoticeBody = notice.Body;
                }

                var defaultCalender = this.context?.SchoolCalendars.Where(x => x.TenantId == dashboardViewModel.TenantId && x.SchoolId == dashboardViewModel.SchoolId && x.AcademicYear == dashboardViewModel.AcademicYear && x.DefaultCalender == true).FirstOrDefault();
                if (defaultCalender != null)
                {
                    dashboardView.schoolCalendar = defaultCalender;
                    dashboardView.schoolCalendar.SchoolMaster = null;

                    var Events = this.context?.CalendarEvents.Where(x => x.TenantId == dashboardViewModel.TenantId && x.SchoolId == dashboardViewModel.SchoolId && x.AcademicYear == dashboardViewModel.AcademicYear && ((x.CalendarId == defaultCalender.CalenderId && x.SystemWideEvent == false) || x.SystemWideEvent == true) &&(x.StartDate >= todayDate || (x.StartDate <= todayDate && todayDate <= x.EndDate))).ToList();
                    if (Events.Count > 0)
                    {
                        dashboardView.calendarEventList = Events;
                    }
                }

                dashboardView._tenantName = dashboardViewModel._tenantName;
                dashboardView._token = dashboardViewModel._token;
            }
            catch (Exception es)
            {
                dashboardView._failure = true;
                dashboardView._message = es.Message;
            }
            return dashboardView;
        }

        /// <summary>
        /// Get Release Number
        /// </summary>
        /// <param name="releaseNumberAddViewModel"></param>
        /// <returns></returns>
        public ReleaseNumberAddViewModel GetReleaseNumber(ReleaseNumberAddViewModel releaseNumberAddViewModel)
        {
            ReleaseNumberAddViewModel ReleaseNumberViewModel = new ReleaseNumberAddViewModel();
            try
            {
                var releaseNumber = this.context?.ReleaseNumber.OrderByDescending(c => c.ReleaseDate).FirstOrDefault(c=>c.SchoolId== releaseNumberAddViewModel.releaseNumber.SchoolId && c.TenantId== releaseNumberAddViewModel.releaseNumber.TenantId);

                ReleaseNumberViewModel.releaseNumber = releaseNumber;

                if (releaseNumber!=null)
                {             
                    ReleaseNumberViewModel._failure = false;                
                }
                else
                {                    
                    ReleaseNumberViewModel._message = NORECORDFOUND;
                    ReleaseNumberViewModel._failure = true;                    
                }
                ReleaseNumberViewModel._tenantName = releaseNumberAddViewModel._tenantName;
                ReleaseNumberViewModel._token = releaseNumberAddViewModel._token;
            }
            catch (Exception es)
            {
                ReleaseNumberViewModel._message = es.Message;
                ReleaseNumberViewModel._failure = true;
                ReleaseNumberViewModel._tenantName = releaseNumberAddViewModel._tenantName;
                ReleaseNumberViewModel._token = releaseNumberAddViewModel._token;
            }
            return ReleaseNumberViewModel;
        }

        /// <summary>
        /// Add Search Filter
        /// </summary>
        /// <param name="searchFilterAddViewModel"></param>
        /// <returns></returns>
        public SearchFilterAddViewModel AddSearchFilter(SearchFilterAddViewModel searchFilterAddViewModel)
        {
            try
            {
                var checkFilterName = this.context?.SearchFilter.Where(x => x.SchoolId == searchFilterAddViewModel.searchFilter.SchoolId && x.TenantId == searchFilterAddViewModel.searchFilter.TenantId && x.FilterName.ToLower() == searchFilterAddViewModel.searchFilter.FilterName.ToLower() && x.Module.ToLower()== searchFilterAddViewModel.searchFilter.Module.ToLower()).FirstOrDefault();
                
                if (checkFilterName != null)
                {
                    searchFilterAddViewModel._failure = true;
                    searchFilterAddViewModel._message = "Filter Name Already Exists";
                }
                else
                {
                    int? FilterId = 1;

                    var searchFilterData = this.context?.SearchFilter.Where(x => x.SchoolId == searchFilterAddViewModel.searchFilter.SchoolId && x.TenantId == searchFilterAddViewModel.searchFilter.TenantId).OrderByDescending(x => x.FilterId).FirstOrDefault();

                    if (searchFilterData != null)
                    {
                        FilterId = searchFilterData.FilterId + 1;
                    }

                    searchFilterAddViewModel.searchFilter.FilterId = (int)FilterId;
                    searchFilterAddViewModel.searchFilter.DateCreated = DateTime.UtcNow;
                    this.context?.SearchFilter.Add(searchFilterAddViewModel.searchFilter);
                    this.context?.SaveChanges();
                    searchFilterAddViewModel._failure = false;
                    searchFilterAddViewModel._message = "Search Filter Added Successfully";
                }
            }
            catch (Exception es)
            {
                searchFilterAddViewModel._failure = true;
                searchFilterAddViewModel._message = es.Message;
            }
            return searchFilterAddViewModel;
        }

        /// <summary>
        /// Update Search Filter
        /// </summary>
        /// <param name="searchFilterAddViewModel"></param>
        /// <returns></returns>
        public SearchFilterAddViewModel UpdateSearchFilter(SearchFilterAddViewModel searchFilterAddViewModel)
        {
            try
            {
                var searchFilterUpdate = this.context?.SearchFilter.FirstOrDefault(x => x.TenantId == searchFilterAddViewModel.searchFilter.TenantId && x.SchoolId == searchFilterAddViewModel.searchFilter.SchoolId && x.FilterId == searchFilterAddViewModel.searchFilter.FilterId);

                if (searchFilterUpdate != null)
                {
                    var checkFilterName = this.context?.SearchFilter.Where(x => x.SchoolId == searchFilterAddViewModel.searchFilter.SchoolId && x.TenantId == searchFilterAddViewModel.searchFilter.TenantId && x.FilterId != searchFilterAddViewModel.searchFilter.FilterId && x.FilterName.ToLower() == searchFilterAddViewModel.searchFilter.FilterName.ToLower() && x.Module.ToLower()==searchFilterAddViewModel.searchFilter.Module.ToLower()).FirstOrDefault();

                    if (checkFilterName != null)
                    {
                        searchFilterAddViewModel._failure = true;
                        searchFilterAddViewModel._message = "Filter Name Already Exists";
                    }
                    else
                    {
                        searchFilterAddViewModel.searchFilter.FilterName = searchFilterUpdate.FilterName;
                        searchFilterAddViewModel.searchFilter.CreatedBy = searchFilterUpdate.CreatedBy;
                        searchFilterAddViewModel.searchFilter.DateCreated = searchFilterUpdate.DateCreated;
                        searchFilterAddViewModel.searchFilter.DateModifed = DateTime.Now;
                        this.context.Entry(searchFilterUpdate).CurrentValues.SetValues(searchFilterAddViewModel.searchFilter);
                        this.context?.SaveChanges();
                        searchFilterAddViewModel._failure = false;
                        searchFilterAddViewModel._message = "Search Filter Updated Successfully";
                    }
                }
                else
                {
                    searchFilterAddViewModel._failure = true;
                    searchFilterAddViewModel._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {

                searchFilterAddViewModel._failure = true;
                searchFilterAddViewModel._message = es.Message;
            }
            return searchFilterAddViewModel;
        }

        /// <summary>
        /// Delete Search Filter
        /// </summary>
        /// <param name="searchFilterAddViewModel"></param>
        /// <returns></returns>
        public SearchFilterAddViewModel DeleteSearchFilter(SearchFilterAddViewModel searchFilterAddViewModel)
        {
            try
            {
                var searchFilterDelete = this.context?.SearchFilter.FirstOrDefault(x => x.TenantId == searchFilterAddViewModel.searchFilter.TenantId && x.SchoolId == searchFilterAddViewModel.searchFilter.SchoolId && x.FilterId == searchFilterAddViewModel.searchFilter.FilterId);                

                if (searchFilterDelete != null)
                {
                    this.context?.SearchFilter.Remove(searchFilterDelete);
                    this.context?.SaveChanges();
                    searchFilterAddViewModel._failure = false;
                    searchFilterAddViewModel._message = "Search Filter Deleted Successfully";
                    
                }
                else
                {
                    searchFilterAddViewModel._failure = true;
                    searchFilterAddViewModel._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {

                searchFilterAddViewModel._failure = true;
                searchFilterAddViewModel._message = es.Message;
            }
            return searchFilterAddViewModel;
        }

        /// <summary>
        /// Get All Search Filter
        /// </summary>
        /// <param name="searchFilterListViewModel"></param>
        /// <returns></returns>
        public SearchFilterListViewModel GetAllSearchFilter(SearchFilterListViewModel searchFilterListViewModel)
        {
            SearchFilterListViewModel searchFilterListModel = new SearchFilterListViewModel();
            try
            {

                var searchFilterList = this.context?.SearchFilter.Where(x => x.TenantId == searchFilterListViewModel.TenantId && x.SchoolId == searchFilterListViewModel.SchoolId && x.Module.ToLower() == searchFilterListViewModel.Module.ToLower()).ToList();
                
                if (searchFilterList.Count > 0)
                {
                    searchFilterListModel.searchFilterList = searchFilterList;
                    searchFilterListModel._tenantName = searchFilterListViewModel._tenantName;
                    searchFilterListModel._token = searchFilterListViewModel._token;
                    searchFilterListModel._failure = false;
                }
                else
                {                    
                    searchFilterListModel._tenantName = searchFilterListViewModel._tenantName;
                    searchFilterListModel._token = searchFilterListViewModel._token;
                    searchFilterListModel._failure = true;
                    searchFilterListModel._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {
                searchFilterListModel._message = es.Message;
                searchFilterListModel._failure = true;
                searchFilterListModel._tenantName = searchFilterListViewModel._tenantName;
                searchFilterListModel._token = searchFilterListViewModel._token;
            }
            return searchFilterListModel;
        }

        /// <summary>
        /// Get All Grade Age Range
        /// </summary>
        /// <param name="gradeAgeRangeListViewModel"></param>
        /// <returns></returns>
        public GradeAgeRangeListViewModel GetAllGradeAgeRange(GradeAgeRangeListViewModel gradeAgeRangeListViewModel)
        {
            GradeAgeRangeListViewModel gradeAgeRangeListModel = new GradeAgeRangeListViewModel();
            try
            {
                var gradeAgeRangeList = this.context?.GradeAgeRange.ToList();
                gradeAgeRangeListModel.gradeAgeRangeList = gradeAgeRangeList;
                gradeAgeRangeListModel._tenantName = gradeAgeRangeListViewModel._tenantName;
                gradeAgeRangeListModel._token = gradeAgeRangeListViewModel._token;

                if (gradeAgeRangeList.Count>0)
                {                    
                    gradeAgeRangeListModel._failure = false;
                }
                else
                {
                    gradeAgeRangeListModel._failure = true;
                    gradeAgeRangeListModel._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {
                gradeAgeRangeListModel._message = es.Message;
                gradeAgeRangeListModel._failure = true;
                gradeAgeRangeListModel._tenantName = gradeAgeRangeListViewModel._tenantName;
                gradeAgeRangeListModel._token = gradeAgeRangeListViewModel._token;
            }
            return gradeAgeRangeListModel;
        }

        /// <summary>
        /// Get All Grade Educational Stage
        /// </summary>
        /// <param name="gradeEducationalStageListViewModel"></param>
        /// <returns></returns>
        public GradeEducationalStageListViewModel GetAllGradeEducationalStage(GradeEducationalStageListViewModel gradeEducationalStageListViewModel)
        {
            GradeEducationalStageListViewModel gradeEducationalStageListModel = new GradeEducationalStageListViewModel();
            try
            {
                var gradeEducationalStageList = this.context?.GradeEducationalStage.ToList();
                gradeEducationalStageListModel.gradeEducationalStageList = gradeEducationalStageList;
                gradeEducationalStageListModel._tenantName = gradeEducationalStageListViewModel._tenantName;
                gradeEducationalStageListModel._token = gradeEducationalStageListViewModel._token;

                if (gradeEducationalStageList.Count > 0)
                {
                    gradeEducationalStageListModel._failure = false;
                }
                else
                {
                    gradeEducationalStageListModel._failure = true;
                    gradeEducationalStageListModel._message = NORECORDFOUND;
                }
            }
            catch (Exception es)
            {
                gradeEducationalStageListModel._message = es.Message;
                gradeEducationalStageListModel._failure = true;
                gradeEducationalStageListModel._tenantName = gradeEducationalStageListViewModel._tenantName;
                gradeEducationalStageListModel._token = gradeEducationalStageListViewModel._token;
            }
            return gradeEducationalStageListModel;
        }
    }
}
