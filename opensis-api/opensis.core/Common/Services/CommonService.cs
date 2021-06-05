/***********************************************************************************
openSIS is a free student information system for public and non-public
schools from Open Solutions for Education, Inc.Website: www.os4ed.com.

Visit the openSIS product website at https://opensis.com to learn more.
If you have question regarding this software or the license, please contact
via the website.

The software is released under the terms of the GNU Affero General Public License as
published by the Free Software Foundation, version 3 of the License.
See https://www.gnu.org/licenses/agpl-3.0.en.html.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

Copyright (c) Open Solutions for Education, Inc.

All rights reserved.
***********************************************************************************/

using Microsoft.AspNetCore.Mvc;
using opensis.core.Common.Interfaces;
using opensis.core.helper;
using opensis.data.Interface;
using opensis.data.ViewModels.CommonModel;
using opensis.data.ViewModels.School;
using opensis.data.ViewModels.StaffSchedule;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.Common.Services
{
    public class CommonService : ICommonService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";

        public ICommonRepository commonRepository;
        public CommonService(ICommonRepository commonRepository)
        {
            this.commonRepository = commonRepository;
        }
        public CountryListModel GetAllCountries(CountryListModel country)
        {
            CountryListModel countryListModel = new CountryListModel();
            if (TokenManager.CheckToken(country._tenantName + country._userName, country._token))
            {
                countryListModel = this.commonRepository.GetAllCountries(country);
                return countryListModel;
            }
            else
            {
                countryListModel._failure = true;
                countryListModel._message = TOKENINVALID;
                return countryListModel;
            }

        }

        public StateListModel GetAllStatesByCountry(StateListModel state)
        {
            StateListModel stateListModel = new StateListModel();
            if (TokenManager.CheckToken(state._tenantName + state._userName, state._token))
            {
                stateListModel = this.commonRepository.GetAllStatesByCountry(state);
                return stateListModel;
            }
            else
            {
                stateListModel._failure = true;
                stateListModel._message = TOKENINVALID;
                return stateListModel;
            }

        }

        public CityListModel GetAllCitiesByState(CityListModel city)
        {
            CityListModel cityListModel = new CityListModel();
            if (TokenManager.CheckToken(city._tenantName + city._userName, city._token))
            {
                cityListModel = this.commonRepository.GetAllCitiesByState(city);
                return cityListModel;
            }
            else
            {
                cityListModel._failure = true;
                cityListModel._message = TOKENINVALID;
                return cityListModel;
            }

        }


        /// <summary>
        /// Add Language
        /// </summary>
        /// <param name="languageAdd"></param>
        /// <returns></returns>
        public LanguageAddModel AddLanguage(LanguageAddModel languageAdd)
        {
            LanguageAddModel languageAddModel = new LanguageAddModel();
            if (TokenManager.CheckToken(languageAdd._tenantName + languageAdd._userName, languageAdd._token))
            {
                languageAddModel = this.commonRepository.AddLanguage(languageAdd);
                return languageAddModel;
            }
            else
            {
                languageAddModel._failure = true;
                languageAddModel._message = TOKENINVALID;
                return languageAddModel;
            }

        }

        /// <summary>
        /// Get Language By Id
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public LanguageAddModel ViewLanguage(LanguageAddModel language)
        {
            LanguageAddModel languageViewModel = new LanguageAddModel();
            if (TokenManager.CheckToken(language._tenantName+ language._userName, language._token))
            {
                languageViewModel = this.commonRepository.ViewLanguage(language);
                return languageViewModel;

            }
            else
            {
                languageViewModel._failure = true;
                languageViewModel._message = TOKENINVALID;
                return languageViewModel;
            }

        }

        /// <summary>
        /// Update Language
        /// </summary>
        /// <param name="languageUpdate"></param>
        /// <returns></returns>
        public LanguageAddModel UpdateLanguage(LanguageAddModel languageUpdate)
        {
            LanguageAddModel languageUpdateModel = new LanguageAddModel();
            if (TokenManager.CheckToken(languageUpdate._tenantName + languageUpdate._userName, languageUpdate._token))
            {
                languageUpdateModel = this.commonRepository.UpdateLanguage(languageUpdate);
                return languageUpdateModel;
            }
            else
            {
                languageUpdateModel._failure = true;
                languageUpdateModel._message = TOKENINVALID;
                return languageUpdateModel;
            }

        }

        public LanguageListModel GetAllLanguage(LanguageListModel language)
        {
            LanguageListModel languageListModel = new LanguageListModel();
            try
            {
                languageListModel = this.commonRepository.GetAllLanguage(language);
                return languageListModel;
            }
            catch (Exception ex)
            {

                languageListModel._failure = true;
                languageListModel._message = null;
                return languageListModel;
            }

        }

        /// <summary>
        /// Delete Language Value By Id
        /// </summary>
        /// <param name="languageDelete"></param>
        /// <returns></returns>
        public LanguageAddModel DeleteLanguage(LanguageAddModel languageDelete)
        {
            LanguageAddModel languageModel = new LanguageAddModel();
            if (TokenManager.CheckToken(languageDelete._tenantName + languageDelete._userName, languageDelete._token))
            {
                languageModel = this.commonRepository.DeleteLanguage(languageDelete);
                return languageModel;

            }
            else
            {
                languageModel._failure = true;
                languageModel._message = TOKENINVALID;
                return languageModel;
            }

        }

        /// <summary>
        /// Add Dropdown Value
        /// </summary>
        /// <param name="dpdownValue"></param>
        /// <returns></returns>
        public DropdownValueAddModel AddDropdownValue(DropdownValueAddModel dpdownValue)
        {
            DropdownValueAddModel dropdownValueModel = new DropdownValueAddModel();
            if (TokenManager.CheckToken(dpdownValue._tenantName + dpdownValue._userName, dpdownValue._token))
            {
                dropdownValueModel = this.commonRepository.AddDropdownValue(dpdownValue);
                return dropdownValueModel;
            }
            else
            {
                dropdownValueModel._failure = true;
                dropdownValueModel._message = TOKENINVALID;
                return dropdownValueModel;
            }

        }
        /// <summary>
        /// Get Dropdown Value By Id
        /// </summary>
        /// <param name="dpdownValue"></param>
        /// <returns></returns>
        public DropdownValueAddModel ViewDropdownValue(DropdownValueAddModel dpdownValue)
        {
            DropdownValueAddModel dropdownValueModel = new DropdownValueAddModel();
            if (TokenManager.CheckToken(dpdownValue._tenantName + dpdownValue._userName, dpdownValue._token))
            {
                dropdownValueModel = this.commonRepository.ViewDropdownValue(dpdownValue);
                return dropdownValueModel;

            }
            else
            {
                dropdownValueModel._failure = true;
                dropdownValueModel._message = TOKENINVALID;
                return dropdownValueModel;
            }

        }
        /// <summary>
        /// Update Dropdown Value
        /// </summary>
        /// <param name="dpdownValue"></param>
        /// <returns></returns>
        public DropdownValueAddModel UpdateDropdownValue(DropdownValueAddModel dpdownValue)
        {
            DropdownValueAddModel dropdownValueAddModel = new DropdownValueAddModel();
            if (TokenManager.CheckToken(dpdownValue._tenantName + dpdownValue._userName, dpdownValue._token))
            {
                dropdownValueAddModel = this.commonRepository.UpdateDropdownValue(dpdownValue);
                return dropdownValueAddModel;
            }
            else
            {
                dropdownValueAddModel._failure = true;
                dropdownValueAddModel._message = TOKENINVALID;
                return dropdownValueAddModel;
            }

        }
        /// <summary>
        /// Get All Dropdown Value
        /// </summary>
        /// <param name="DropdownValue"></param>
        /// <returns></returns>
        public DropdownValueListModel GetAllDropdownValues(DropdownValueListModel dpdownList)
        {
            DropdownValueListModel dropdownValueListModel = new DropdownValueListModel();
            try
            {
                if (TokenManager.CheckToken(dpdownList._tenantName + dpdownList._userName, dpdownList._token))
                {
                    dropdownValueListModel = this.commonRepository.GetAllDropdownValues(dpdownList);
                }
                else
                {
                    dropdownValueListModel._failure = true;
                    dropdownValueListModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                dropdownValueListModel._failure = true;
                dropdownValueListModel._message = es.Message;
            }

            return dropdownValueListModel;
        }

        /// <summary>
        /// Add Country
        /// </summary>
        /// <param name="countryAddModel"></param>
        /// <returns></returns>
        public CountryAddModel AddCountry(CountryAddModel countryAddModel)
        {
            CountryAddModel countryAdd = new CountryAddModel();
            if (TokenManager.CheckToken(countryAddModel._tenantName + countryAddModel._userName, countryAddModel._token))
            {
                countryAdd = this.commonRepository.AddCountry(countryAddModel);
            }
            else
            {
                countryAdd._failure = true;
                countryAdd._message = TOKENINVALID;
            }
            return countryAdd;
        }

        /// <summary>
        /// Update Country
        /// </summary>
        /// <param name="countryAddModel"></param>
        /// <returns></returns>
        public CountryAddModel UpdateCountry(CountryAddModel countryAddModel)
        {
            CountryAddModel countryUpdate = new CountryAddModel();
            if (TokenManager.CheckToken(countryAddModel._tenantName + countryAddModel._userName, countryAddModel._token))
            {
                countryUpdate = this.commonRepository.UpdateCountry(countryAddModel);
            }
            else
            {
                countryUpdate._failure = true;
                countryUpdate._message = TOKENINVALID;
            }
            return countryUpdate;
        }

        /// <summary>
        /// Delete Country Value By Id
        /// </summary>
        /// <param name="countryDeleteModel"></param>
        /// <returns></returns>
        public CountryAddModel DeleteCountry(CountryAddModel countryDeleteModel)
        {
            CountryAddModel countryModel = new CountryAddModel();
            if (TokenManager.CheckToken(countryDeleteModel._tenantName + countryDeleteModel._userName, countryDeleteModel._token))
            {
                countryModel = this.commonRepository.DeleteCountry(countryDeleteModel);
                return countryModel;

            }
            else
            {
                countryModel._failure = true;
                countryModel._message = TOKENINVALID;
                return countryModel;
            }

        }

        /// <summary>
        /// Delete Dropdown Value By Id
        /// </summary>
        /// <param name="dpdownValue"></param>
        /// <returns></returns>
        public DropdownValueAddModel DeleteDropdownValue(DropdownValueAddModel dpdownValue)
        {
            DropdownValueAddModel dropdownValueModel = new DropdownValueAddModel();
            if (TokenManager.CheckToken(dpdownValue._tenantName + dpdownValue._userName, dpdownValue._token))
            {
                dropdownValueModel = this.commonRepository.DeleteDropdownValue(dpdownValue);
                return dropdownValueModel;

            }
            else
            {
                dropdownValueModel._failure = true;
                dropdownValueModel._message = TOKENINVALID;
                return dropdownValueModel;
            }

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
                languageListModel = this.commonRepository.GetAllLanguageForLogin(language);
                return languageListModel;
            }
            catch (Exception ex)
            {

                languageListModel._failure = true;
                languageListModel._message = null;
                return languageListModel;
            }

        }

        /// <summary>
        /// Dashboard View
        /// </summary>
        /// <param name="dashboardViewModel"></param>
        /// <returns></returns>
        public DashboardViewModel GetDashboardView(DashboardViewModel dashboardViewModel)
        {
            DashboardViewModel dashboardView = new DashboardViewModel();
            if (TokenManager.CheckToken(dashboardViewModel._tenantName + dashboardViewModel._userName, dashboardViewModel._token))
            {
                dashboardView = this.commonRepository.GetDashboardView(dashboardViewModel);
            }
            else
            {
                dashboardView._failure = true;
                dashboardView._message = TOKENINVALID;              
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
            ReleaseNumberAddViewModel releaseNumberView = new ReleaseNumberAddViewModel();
            try
            {                
                releaseNumberView = this.commonRepository.GetReleaseNumber(releaseNumberAddViewModel);
                return releaseNumberView;
            }
            catch (Exception ex)
            {
                releaseNumberView._failure = true;
                releaseNumberView._message = null;
                return releaseNumberView;
            }
        }

        /// <summary>
        /// Add Search Filter
        /// </summary>
        /// <param name="searchFilterAddViewModel"></param>
        /// <returns></returns>
        public SearchFilterAddViewModel AddSearchFilter(SearchFilterAddViewModel searchFilterAddViewModel)
        {
            SearchFilterAddViewModel SearchFilterAddModel = new SearchFilterAddViewModel();
            try
            {
                if (TokenManager.CheckToken(searchFilterAddViewModel._tenantName + searchFilterAddViewModel._userName, searchFilterAddViewModel._token))
                {
                    SearchFilterAddModel = this.commonRepository.AddSearchFilter(searchFilterAddViewModel);
                }
                else
                {
                    SearchFilterAddModel._failure = true;
                    SearchFilterAddModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {

                SearchFilterAddModel._failure = true;
                SearchFilterAddModel._message = es.Message;
            }
            return SearchFilterAddModel;
        }
        /// <summary>
        /// Update Search Filter
        /// </summary>
        /// <param name="searchFilterAddViewModel"></param>
        /// <returns></returns>
        public SearchFilterAddViewModel UpdateSearchFilter(SearchFilterAddViewModel searchFilterAddViewModel)
        {
            SearchFilterAddViewModel SearchFilterUpdateModel = new SearchFilterAddViewModel();
            try
            {
                if (TokenManager.CheckToken(searchFilterAddViewModel._tenantName + searchFilterAddViewModel._userName, searchFilterAddViewModel._token))
                {
                    SearchFilterUpdateModel = this.commonRepository.UpdateSearchFilter(searchFilterAddViewModel);
                }
                else
                {
                    SearchFilterUpdateModel._failure = true;
                    SearchFilterUpdateModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {

                SearchFilterUpdateModel._failure = true;
                SearchFilterUpdateModel._message = es.Message;
            }
            return SearchFilterUpdateModel;
        }

        /// <summary>
        /// Delete Search Filter
        /// </summary>
        /// <param name="searchFilterAddViewModel"></param>
        /// <returns></returns>
        public SearchFilterAddViewModel DeleteSearchFilter(SearchFilterAddViewModel searchFilterAddViewModel)
        {
            SearchFilterAddViewModel searchFilterDeleteModel = new SearchFilterAddViewModel();
            try
            {
                if (TokenManager.CheckToken(searchFilterAddViewModel._tenantName + searchFilterAddViewModel._userName, searchFilterAddViewModel._token))
                {
                    searchFilterDeleteModel = this.commonRepository.DeleteSearchFilter(searchFilterAddViewModel);
                }
                else
                {
                    searchFilterDeleteModel._failure = true;
                    searchFilterDeleteModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                searchFilterDeleteModel._failure = true;
                searchFilterDeleteModel._message = es.Message;
            }
            return searchFilterDeleteModel;
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
                if (TokenManager.CheckToken(searchFilterListViewModel._tenantName + searchFilterListViewModel._userName, searchFilterListViewModel._token))
                {
                    searchFilterListModel = this.commonRepository.GetAllSearchFilter(searchFilterListViewModel);
                }
                else
                {
                    searchFilterListModel._failure = true;
                    searchFilterListModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                searchFilterListModel._failure = true;
                searchFilterListModel._message = es.Message;
            }
            return searchFilterListModel;
        }

        public GradeAgeRangeListViewModel GetAllGradeAgeRange(GradeAgeRangeListViewModel gradeAgeRangeListViewModel)
        {
            GradeAgeRangeListViewModel gradeAgeRangelListModel = new GradeAgeRangeListViewModel();
            try
            {
                if (TokenManager.CheckToken(gradeAgeRangeListViewModel._tenantName + gradeAgeRangeListViewModel._userName, gradeAgeRangeListViewModel._token))
                {
                    gradeAgeRangelListModel = this.commonRepository.GetAllGradeAgeRange(gradeAgeRangeListViewModel);
                }
                else
                {
                    gradeAgeRangelListModel._failure = true;
                    gradeAgeRangelListModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                gradeAgeRangelListModel._failure = true;
                gradeAgeRangelListModel._message = es.Message;
            }

            return gradeAgeRangelListModel;
        }

        public GradeEducationalStageListViewModel GetAllGradeEducationalStage(GradeEducationalStageListViewModel gradeEducationalStageListViewModel)
        {
            GradeEducationalStageListViewModel gradeEducationalStagelListModel = new GradeEducationalStageListViewModel();
            try
            {
                if (TokenManager.CheckToken(gradeEducationalStageListViewModel._tenantName + gradeEducationalStageListViewModel._userName, gradeEducationalStageListViewModel._token))
                {
                    gradeEducationalStagelListModel = this.commonRepository.GetAllGradeEducationalStage(gradeEducationalStageListViewModel);
                }
                else
                {
                    gradeEducationalStagelListModel._failure = true;
                    gradeEducationalStagelListModel._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                gradeEducationalStagelListModel._failure = true;
                gradeEducationalStagelListModel._message = es.Message;
            }

            return gradeEducationalStagelListModel;
        }

        /// <summary>
        /// Get Dashboard View For Staff
        /// </summary>
        /// <param name="scheduledCourseSectionViewModel"></param>
        /// <returns></returns>
        public ScheduledCourseSectionViewModel GetDashboardViewForStaff(ScheduledCourseSectionViewModel scheduledCourseSectionViewModel)
        {
            ScheduledCourseSectionViewModel scheduledCourseSectionView = new ScheduledCourseSectionViewModel();
            try
            {
                if (TokenManager.CheckToken(scheduledCourseSectionViewModel._tenantName + scheduledCourseSectionViewModel._userName, scheduledCourseSectionViewModel._token))
                {
                    scheduledCourseSectionView = this.commonRepository.GetDashboardViewForStaff(scheduledCourseSectionViewModel);
                }
                else
                {
                    scheduledCourseSectionView._failure = true;
                    scheduledCourseSectionView._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                scheduledCourseSectionView._failure = true;
                scheduledCourseSectionView._message = es.Message;
            }
            return scheduledCourseSectionView;
        }

        /// <summary>
        /// Get Dashboard View For CalendarView
        /// </summary>
        /// <param name="dashboardViewModel"></param>
        /// <returns></returns>
        public DashboardViewModel GetDashboardViewForCalendarView(DashboardViewModel dashboardViewModel)
        {
            DashboardViewModel dashboardView = new DashboardViewModel();
            if (TokenManager.CheckToken(dashboardViewModel._tenantName + dashboardViewModel._userName, dashboardViewModel._token))
            {
                dashboardView = this.commonRepository.GetDashboardViewForCalendarView(dashboardViewModel);
            }
            else
            {
                dashboardView._failure = true;
                dashboardView._message = TOKENINVALID;
            }
            return dashboardView;
        }

        /// <summary>
        /// Reset Password For User
        /// </summary>
        /// <param name="ResetPasswordModel"></param>
        /// <returns></returns>
        public ResetPasswordModel ResetPasswordForUser(ResetPasswordModel resetPasswordModel)
        {
            ResetPasswordModel resetPassword = new ResetPasswordModel();
            if (TokenManager.CheckToken(resetPasswordModel._tenantName + resetPasswordModel._userName, resetPasswordModel._token))
            {
                resetPassword = this.commonRepository.ResetPasswordForUser(resetPasswordModel);
            }
            else
            {
                resetPassword._failure = true;
                resetPassword._message = TOKENINVALID;
            }
            return resetPassword;
        }

        /// <summary>
        /// Export Excel
        /// </summary>
        /// <param name="exportExcelModel"></param>
        /// <returns></returns>
        public ExcelHeaderModel GetExcelHeader(ExcelHeaderModel excelHeaderModel)
        {
            ExcelHeaderModel excelHeader = new ExcelHeaderModel();
            
            if (TokenManager.CheckToken(excelHeaderModel._tenantName + excelHeaderModel._userName, excelHeaderModel._token))
            {
                excelHeader = this.commonRepository.GetExcelHeader(excelHeaderModel);
            }
            else
            {
                excelHeader._failure = true;
                excelHeader._message = TOKENINVALID;
            }
            return excelHeader;
        }

        /// <summary>
        /// Add Update School Preference
        /// </summary>
        /// <param name="schoolPreferenceAddViewModel"></param>
        /// <returns></returns>
        public SchoolPreferenceAddViewModel AddUpdateSchoolPreference(SchoolPreferenceAddViewModel schoolPreferenceAddViewModel)
        {
            SchoolPreferenceAddViewModel schoolPreferenceAddModel = new SchoolPreferenceAddViewModel();

            if (TokenManager.CheckToken(schoolPreferenceAddViewModel._tenantName + schoolPreferenceAddViewModel._userName, schoolPreferenceAddViewModel._token))
            {
                schoolPreferenceAddModel = this.commonRepository.AddUpdateSchoolPreference(schoolPreferenceAddViewModel);
            }
            else
            {
                schoolPreferenceAddModel._failure = true;
                schoolPreferenceAddModel._message = TOKENINVALID;
            }
            return schoolPreferenceAddModel;
        }

        /// <summary>
        /// View School Preference
        /// </summary>
        /// <param name="schoolPreferenceAddViewModel"></param>
        /// <returns></returns>
        public SchoolPreferenceAddViewModel ViewSchoolPreference(SchoolPreferenceAddViewModel schoolPreferenceAddViewModel)
        {
            SchoolPreferenceAddViewModel schoolPreferenceAddModel = new SchoolPreferenceAddViewModel();

            if (TokenManager.CheckToken(schoolPreferenceAddViewModel._tenantName + schoolPreferenceAddViewModel._userName, schoolPreferenceAddViewModel._token))
            {
                schoolPreferenceAddModel = this.commonRepository.ViewSchoolPreference(schoolPreferenceAddViewModel);
            }
            else
            {
                schoolPreferenceAddModel._failure = true;
                schoolPreferenceAddModel._message = TOKENINVALID;
            }
            return schoolPreferenceAddModel;
        }
    }
}
