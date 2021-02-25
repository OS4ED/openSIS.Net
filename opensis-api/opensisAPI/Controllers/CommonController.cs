using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using opensis.core.Common.Interfaces;
using opensis.data.ViewModels.CommonModel;
using opensis.data.ViewModels.School;

namespace opensisAPI.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("{tenant}/Common")]
    [ApiController]
    public class CommonController : ControllerBase
    {

        private ICommonService _commonService;
        public CommonController(ICommonService commonService)
        {
            _commonService = commonService;
        }
        [HttpPost("getAllCountries")]

        public ActionResult<CountryListModel> GetAllCountries(CountryListModel country)
        {
            CountryListModel countryListModel = new CountryListModel();
            try
            {
                countryListModel = _commonService.GetAllCountries(country);
            }
            catch (Exception es)
            {
                countryListModel._failure = true;
                countryListModel._message = es.Message;
            }
            return countryListModel;
        }

        [HttpPost("getAllStatesByCountry")]

        public ActionResult<StateListModel> GetAllStatesByCountry(StateListModel state)
        {
            StateListModel stateListModel = new StateListModel();
            try
            {
                stateListModel = _commonService.GetAllStatesByCountry(state);
            }
            catch (Exception es)
            {
                stateListModel._failure = true;
                stateListModel._message = es.Message;
            }
            return stateListModel;
        }

        [HttpPost("getAllCitiesByState")]

        public ActionResult<CityListModel> GetAllCitiesByState(CityListModel city)
        {
            CityListModel cityListModel = new CityListModel();
            try
            {
                cityListModel = _commonService.GetAllCitiesByState(city);
            }
            catch (Exception es)
            {
                cityListModel._failure = true;
                cityListModel._message = es.Message;
            }
            return cityListModel;
        }

        [HttpPost("addLanguage")]
        public ActionResult<LanguageAddModel> AddLanguage(LanguageAddModel languageAdd)
        {
            LanguageAddModel languageAddModel = new LanguageAddModel();
            try
            {
                languageAddModel = _commonService.AddLanguage(languageAdd);
            }
            catch (Exception es)
            {
                languageAddModel._failure = true;
                languageAddModel._message = es.Message;
            }
            return languageAddModel;
        }

        [HttpPost("viewLanguage")]
        public ActionResult<LanguageAddModel> ViewLanguage(LanguageAddModel language)
        {
            LanguageAddModel languageViewModel = new LanguageAddModel();
            try
            {
                languageViewModel = _commonService.ViewLanguage(language);
            }
            catch (Exception es)
            {
                languageViewModel._failure = true;
                languageViewModel._message = es.Message;
            }
            return languageViewModel;
        }

        [HttpPost("updateLanguage")]
        public ActionResult<LanguageAddModel> UpdateLanguage(LanguageAddModel languageUpdate)
        {
            LanguageAddModel languageUpdateModel = new LanguageAddModel();
            try
            {
                languageUpdateModel = _commonService.UpdateLanguage(languageUpdate);
            }
            catch (Exception es)
            {
                languageUpdateModel._failure = true;
                languageUpdateModel._message = es.Message;
            }
            return languageUpdateModel;
        }

        [HttpPost("getAllLanguage")]

        public ActionResult<LanguageListModel> GetAllLanguage(LanguageListModel language)
        {
            LanguageListModel languageListModel = new LanguageListModel();
            try
            {
                languageListModel = _commonService.GetAllLanguage(language);
            }
            catch (Exception es)
            {
                languageListModel._failure = true;
                languageListModel._message = es.Message;
            }
            return languageListModel;
        }

        [HttpPost("deleteLanguage")]
        public ActionResult<LanguageAddModel> DeleteLanguage(LanguageAddModel languageDelete)
        {
            LanguageAddModel languageModel = new LanguageAddModel();
            try
            {
                languageModel = _commonService.DeleteLanguage(languageDelete);
            }
            catch (Exception es)
            {
                languageModel._failure = true;
                languageModel._message = es.Message;
            }
            return languageModel;
        }

        [HttpPost("addDropdownValue")]
        public ActionResult<DropdownValueAddModel> AddDropdownValue(DropdownValueAddModel dpdownValue)
        {
            DropdownValueAddModel addDropdownModel = new DropdownValueAddModel();
            try
            {
                addDropdownModel = _commonService.AddDropdownValue(dpdownValue);
            }
            catch (Exception es)
            {
                addDropdownModel._failure = true;
                addDropdownModel._message = es.Message;
            }
            return addDropdownModel;
        }

        [HttpPost("viewDropdownValue")]
        public ActionResult<DropdownValueAddModel> ViewDropdownValue(DropdownValueAddModel dpdownValue)
        {
            DropdownValueAddModel addDropdownModel = new DropdownValueAddModel();
            try
            {
                addDropdownModel = _commonService.ViewDropdownValue(dpdownValue);
            }
            catch (Exception es)
            {
                addDropdownModel._failure = true;
                addDropdownModel._message = es.Message;
            }
            return addDropdownModel;
        }

        [HttpPut("updateDropdownValue")]
        public ActionResult<DropdownValueAddModel> UpdateDropdownValue(DropdownValueAddModel dpdownValue)
        {
            DropdownValueAddModel updateDropdownModel = new DropdownValueAddModel();
            try
            {
                updateDropdownModel = _commonService.UpdateDropdownValue(dpdownValue);
            }
            catch (Exception es)
            {
                updateDropdownModel._failure = true;
                updateDropdownModel._message = es.Message;
            }
            return updateDropdownModel;
        }

        [HttpPost("getAllDropdownValues")]
        public ActionResult<DropdownValueListModel> GetAllDropdownValues(DropdownValueListModel dpdownList)
        {
            DropdownValueListModel dropdownListModel = new DropdownValueListModel();
            try
            {
                dropdownListModel = _commonService.GetAllDropdownValues(dpdownList);
            }
            catch (Exception es)
            {
                dropdownListModel._failure = true;
                dropdownListModel._message = es.Message;
            }
            return dropdownListModel;
        }

        [HttpPost("addCountry")]
        public ActionResult<CountryAddModel> AddCountry(CountryAddModel countryAddModel)
        {
            CountryAddModel countryAdd = new CountryAddModel();
            try
            {
                countryAdd = _commonService.AddCountry(countryAddModel);
            }
            catch (Exception es)
            {
                countryAdd._failure = true;
                countryAdd._message = es.Message;
            }
            return countryAdd;
        }

        [HttpPut("updateCountry")]
        public ActionResult<CountryAddModel> UpdateCountry(CountryAddModel countryAddModel)
        {
            CountryAddModel countryUpdate = new CountryAddModel();
            try
            {
                countryUpdate = _commonService.UpdateCountry(countryAddModel);
            }
            catch (Exception es)
            {
                countryUpdate._failure = true;
                countryUpdate._message = es.Message;
            }
            return countryUpdate;
        }


        [HttpPost("deleteDropdownValue")]
        public ActionResult<DropdownValueAddModel> DeleteDropdownValue(DropdownValueAddModel dpdownValue)
        {
            DropdownValueAddModel updateDropdownModel = new DropdownValueAddModel();
            try
            {
                updateDropdownModel = _commonService.DeleteDropdownValue(dpdownValue);
            }
            catch (Exception es)
            {
                updateDropdownModel._failure = true;
                updateDropdownModel._message = es.Message;
            }
            return updateDropdownModel;
        }

        [HttpPost("deleteCountry")]
        public ActionResult<CountryAddModel> DeleteCountry(CountryAddModel countryValue)
        {
            CountryAddModel countryModel = new CountryAddModel();
            try
            {
                countryModel = _commonService.DeleteCountry(countryValue);
            }
            catch (Exception es)
            {
                countryModel._failure = true;
                countryModel._message = es.Message;
            }
            return countryModel;
        }


        [HttpPost("getAllLanguageForLogin")]
        public ActionResult<LanguageListModel> GetAllLanguageForLogin(LanguageListModel language)
        {
            LanguageListModel languageListModel = new LanguageListModel();
            try
            {
                languageListModel = _commonService.GetAllLanguageForLogin(language);
            }
            catch (Exception es)
            {
                languageListModel._failure = true;
                languageListModel._message = es.Message;
            }
            return languageListModel;
        }

        [HttpPost("getDashboardView")]
        public ActionResult<DashboardViewModel> GetDashboardView(DashboardViewModel dashboardViewModel)
        {
            DashboardViewModel dashboardView = new DashboardViewModel();
            try
            {
                dashboardView = _commonService.GetDashboardView(dashboardViewModel);
            }
            catch (Exception es)
            {
                dashboardView._failure = true;
                dashboardView._message = es.Message;
            }
            return dashboardView;
        }

        [HttpPost("getReleaseNumber")]

        public ActionResult<ReleaseNumberAddViewModel> GetReleaseNumber(ReleaseNumberAddViewModel releaseNumberAddViewModel)
        {
            ReleaseNumberAddViewModel releaseNumberView = new ReleaseNumberAddViewModel();
            try
            {
                releaseNumberView = _commonService.GetReleaseNumber(releaseNumberAddViewModel);
            }
            catch (Exception es)
            {
                releaseNumberView._failure = true;
                releaseNumberView._message = es.Message;
            }
            return releaseNumberView;
        }
    }
}
