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
using opensis.data.ViewModels.CommonModel;
using opensis.data.ViewModels.StaffSchedule;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.Common.Interfaces
{
    public interface ICommonService
    {
        public CountryListModel GetAllCountries(CountryListModel countryListModel);
        public StateListModel GetAllStatesByCountry(StateListModel stateListModel);
        public CityListModel GetAllCitiesByState(CityListModel cityListModel);
        public LanguageListModel GetAllLanguage(LanguageListModel languageListModel);
        public DropdownValueAddModel AddDropdownValue(DropdownValueAddModel dpdownValue);
        public DropdownValueAddModel ViewDropdownValue(DropdownValueAddModel dpdownValue);
        public DropdownValueAddModel UpdateDropdownValue(DropdownValueAddModel dpdownValue);
        public DropdownValueListModel GetAllDropdownValues(DropdownValueListModel dpdownList);
        public LanguageAddModel AddLanguage(LanguageAddModel languageAdd);
        public LanguageAddModel UpdateLanguage(LanguageAddModel languageUpdate);
        public LanguageAddModel ViewLanguage(LanguageAddModel language);
        public CountryAddModel AddCountry(CountryAddModel countryAddModel);
        public CountryAddModel UpdateCountry(CountryAddModel countryAddModel);
        public LanguageAddModel DeleteLanguage(LanguageAddModel languageDelete);
        public CountryAddModel DeleteCountry(CountryAddModel countryDeleteModel);
        public DropdownValueAddModel DeleteDropdownValue(DropdownValueAddModel dpdownValue);
        public LanguageListModel GetAllLanguageForLogin(LanguageListModel languageListModel);
        public DashboardViewModel GetDashboardView(DashboardViewModel dashboardViewModel);
        public ReleaseNumberAddViewModel GetReleaseNumber(ReleaseNumberAddViewModel releaseNumberAddViewModel);
        public SearchFilterAddViewModel AddSearchFilter(SearchFilterAddViewModel searchFilterAddViewModel);
        public SearchFilterAddViewModel UpdateSearchFilter(SearchFilterAddViewModel searchFilterAddViewModel);
        public SearchFilterAddViewModel DeleteSearchFilter(SearchFilterAddViewModel searchFilterAddViewModel);
        public SearchFilterListViewModel GetAllSearchFilter(SearchFilterListViewModel searchFilterListViewModel);
        public GradeAgeRangeListViewModel GetAllGradeAgeRange(GradeAgeRangeListViewModel gradeAgeRangeListViewModel);
        public GradeEducationalStageListViewModel GetAllGradeEducationalStage(GradeEducationalStageListViewModel gradeEducationalStageListViewModel);
        public ScheduledCourseSectionViewModel GetDashboardViewForStaff(ScheduledCourseSectionViewModel scheduledCourseSectionViewModel);
        public DashboardViewModel GetDashboardViewForCalendarView(DashboardViewModel dashboardViewModel);
        public ResetPasswordModel ResetPasswordForUser(ResetPasswordModel resetPasswordModel);
        public ExcelHeaderModel GetExcelHeader(ExcelHeaderModel excelHeaderModel);
        public SchoolPreferenceAddViewModel AddUpdateSchoolPreference(SchoolPreferenceAddViewModel schoolPreferenceAddViewModel);
        public SchoolPreferenceAddViewModel ViewSchoolPreference(SchoolPreferenceAddViewModel schoolPreferenceAddViewModel);
    }
}
