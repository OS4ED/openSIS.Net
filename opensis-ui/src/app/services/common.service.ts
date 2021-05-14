import { LovList, LovAddView } from '../models/lov.model';
import { CountryModel,CountryAddModel } from '../models/country.model';
import { StateModel } from '../models/state.model';
import { CityModel } from '../models/city.model';
import { HttpClient} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { LanguageModel,LanguageAddModel } from '../models/language.model';
import { ReleaseNumberAddViewModel } from '../models/release-number-model';
import { SearchFilterAddViewModel, SearchFilterListViewModel } from '../models/search-filter.model';
import { AgeRangeList, EducationalStage } from '../models/common.model';
import { DefaultValuesService } from '../common/default-values.service';
@Injectable({
  providedIn: 'root'
})
export class CommonService {
  apiUrl:string = environment.apiURL;
  private searchResult;
  private moduleName: string;

  constructor(private http: HttpClient,private defaultValuesService: DefaultValuesService) { }
  GetAllCountry(obj: CountryModel){  
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName+ "/Common/getAllCountries"; 
    return this.http.post<CountryModel>(apiurl,obj)
  }  

  AddCountry(obj: CountryAddModel){  
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName+ "/Common/addCountry"; 
    return this.http.post<CountryAddModel>(apiurl,obj)
  } 
  
  UpdateCountry(obj: CountryAddModel){  
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName+ "/Common/updateCountry"; 
    return this.http.put<CountryAddModel>(apiurl,obj)
  }  

  DeleteCountry(obj: CountryAddModel){  
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName+ "/Common/deleteCountry"; 
    return this.http.post<CountryAddModel>(apiurl,obj)
  }  

  GetAllState(obj: StateModel){  
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName+ "/Common/getAllStatesByCountry"; 
    return this.http.post<StateModel>(apiurl,obj)
  } 
  GetAllCity(obj: CityModel){  
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName+ "/Common/getAllCitiesByState"; 
    return this.http.post<CityModel>(apiurl,obj)
  }  
  GetAllLanguage(obj: LanguageModel){  
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName+ "/Common/getAllLanguage"; 
    return this.http.post<LanguageModel>(apiurl,obj)
  }  

  AddLanguage(obj: LanguageAddModel){  
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName+ "/Common/addLanguage"; 
    return this.http.post<LanguageAddModel>(apiurl,obj)
  }  

  UpdateLanguage(obj: LanguageAddModel){   
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName+ "/Common/updateLanguage"; 
    return this.http.post<LanguageAddModel>(apiurl,obj)
  }  

  DeleteLanguage(obj: LanguageAddModel){  
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName+ "/Common/deleteLanguage"; 
    return this.http.post<LanguageAddModel>(apiurl,obj)
  } 
  getAllDropdownValues(obj:LovList){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl =this.apiUrl + obj._tenantName+ "/Common/getAllDropdownValues"; 
    return this.http.post<LovList>(apiurl,obj);
  }
  addDropdownValue(obj:LovAddView){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.dropdownValue.schoolId= this.defaultValuesService.getSchoolID();
    obj.dropdownValue.tenantId= this.defaultValuesService.getTenantID();
    let apiurl =this.apiUrl + obj._tenantName+ "/Common/addDropdownValue"; 
    return this.http.post<LovAddView>(apiurl,obj);
  }
  updateDropdownValue(obj:LovAddView){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.dropdownValue.schoolId= this.defaultValuesService.getSchoolID();
    obj.dropdownValue.tenantId= this.defaultValuesService.getTenantID();
    let apiurl =this.apiUrl + obj._tenantName+ "/Common/updateDropdownValue"; 
    return this.http.put<LovAddView>(apiurl,obj);
  }
  deleteDropdownValue(obj:LovAddView){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl =this.apiUrl + obj._tenantName+ "/Common/deleteDropdownValue"; 
    return this.http.post<LovAddView>(apiurl,obj);
  }

  getReleaseNumber(obj:ReleaseNumberAddViewModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl =this.apiUrl + obj._tenantName+ "/Common/getReleaseNumber"; 
    return this.http.post<ReleaseNumberAddViewModel>(apiurl,obj);
  }

  addSearchFilter(obj:SearchFilterAddViewModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl =this.apiUrl + obj._tenantName+ "/Common/addSearchFilter"; 
    return this.http.post<SearchFilterAddViewModel>(apiurl,obj);
  }

  updateSearchFilter(obj:SearchFilterAddViewModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl =this.apiUrl + obj._tenantName+ "/Common/updateSearchFilter"; 
    return this.http.put<SearchFilterAddViewModel>(apiurl,obj);
  }

  deleteSearchFilter(obj:SearchFilterAddViewModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl =this.apiUrl + obj._tenantName+ "/Common/deleteSearchFilter"; 
    return this.http.post<SearchFilterAddViewModel>(apiurl,obj);
  }

  getAllSearchFilter(obj:SearchFilterListViewModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl =this.apiUrl + obj._tenantName+ "/Common/getAllSearchFilter"; 
    return this.http.post<SearchFilterListViewModel>(apiurl,obj);
  }
  
  getAllGradeAgeRange(obj:AgeRangeList){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName+ "/Common/getAllGradeAgeRange";
    return this.http.post<AgeRangeList>(apiurl,obj)
  }

 getAllGradeEducationalStage(obj:EducationalStage){
  obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName+ "/Common/getAllGradeEducationalStage";
    return this.http.post<EducationalStage>(apiurl,obj)
  }

  setSearchResult(result) {
    this.searchResult = result;
  }
  getSearchResult() {
    return this.searchResult;
  }

  setModuleName(moduleName){
    this.moduleName = moduleName;
  }
  getModuleName(){
    return this.moduleName;
  }
}
