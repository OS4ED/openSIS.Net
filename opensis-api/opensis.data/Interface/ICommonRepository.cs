using opensis.data.ViewModels.CommonModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.Interface
{
    public interface ICommonRepository
    {
        public CountryListModel GetAllCountries(CountryListModel country);

        public StateListModel GetAllStatesByCountry(StateListModel state);
        public CityListModel GetAllCitiesByState(CityListModel city);
        public LanguageListModel GetAllLanguage(LanguageListModel language);
        public DropdownValueAddModel AddDropdownValue(DropdownValueAddModel dpdownValue);
        public DropdownValueAddModel UpdateDropdownValue(DropdownValueAddModel dpdownValue);
        public DropdownValueAddModel ViewDropdownValue(DropdownValueAddModel dpdownValue);
        public DropdownValueListModel GetAllDropdownValues(DropdownValueListModel dpdownList);
        public LanguageAddModel AddLanguage(LanguageAddModel languageAdd);
        public LanguageAddModel UpdateLanguage(LanguageAddModel languageUpdate);
        public LanguageAddModel ViewLanguage(LanguageAddModel language);
        public CountryAddModel AddCountry(CountryAddModel countryAddModel);
        public CountryAddModel UpdateCountry(CountryAddModel countryAddModel);
        public CountryAddModel DeleteCountry(CountryAddModel countryDeleteModel);
        public DropdownValueAddModel DeleteDropdownValue(DropdownValueAddModel dpdownValue);
        public LanguageAddModel DeleteLanguage(LanguageAddModel languageAddModel);
        public LanguageListModel GetAllLanguageForLogin(LanguageListModel languageListModel);
        public DashboardViewModel GetDashboardView(DashboardViewModel dashboardViewModel);
        public ReleaseNumberAddViewModel GetReleaseNumber(ReleaseNumberAddViewModel releaseNumberAddViewModel);
    }
}
