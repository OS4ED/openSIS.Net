using opensis.data.ViewModels.CommonModel;
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
    }
}
