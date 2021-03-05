import { LovList, LovAddView } from './../models/lovModel';
import { CountryModel,CountryAddModel } from '../models/countryModel';
import { StateModel } from '../models/stateModel';
import { CityModel } from '../models/cityModel';
import { HttpClient} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { LanguageModel,LanguageAddModel } from '../models/languageModel';
import { ReleaseNumberAddViewModel } from '../models/releaseNumberModel';

@Injectable({
  providedIn: 'root'
})
export class CommonService {
  apiUrl:string = environment.apiURL;
  constructor(private http: HttpClient) { }

  GetAllCountry(obj: CountryModel){  
    let apiurl = this.apiUrl + obj._tenantName+ "/Common/getAllCountries"; 
    return this.http.post<CountryModel>(apiurl,obj)
  }  

  AddCountry(obj: CountryAddModel){  
    let apiurl = this.apiUrl + obj._tenantName+ "/Common/addCountry"; 
    return this.http.post<CountryAddModel>(apiurl,obj)
  } 
  
  UpdateCountry(obj: CountryAddModel){  
    let apiurl = this.apiUrl + obj._tenantName+ "/Common/updateCountry"; 
    return this.http.put<CountryAddModel>(apiurl,obj)
  }  

  DeleteCountry(obj: CountryAddModel){  
    let apiurl = this.apiUrl + obj._tenantName+ "/Common/deleteCountry"; 
    return this.http.post<CountryAddModel>(apiurl,obj)
  }  

  GetAllState(obj: StateModel){  
     
    let apiurl = this.apiUrl + obj._tenantName+ "/Common/getAllStatesByCountry"; 
    return this.http.post<StateModel>(apiurl,obj)
  } 
  GetAllCity(obj: CityModel){  
  
    let apiurl = this.apiUrl + obj._tenantName+ "/Common/getAllCitiesByState"; 
    return this.http.post<CityModel>(apiurl,obj)
  }  
  GetAllLanguage(obj: LanguageModel){  
  
    let apiurl = this.apiUrl + obj._tenantName+ "/Common/getAllLanguage"; 
    return this.http.post<LanguageModel>(apiurl,obj)
  }  

  AddLanguage(obj: LanguageAddModel){  
    let apiurl = this.apiUrl + obj._tenantName+ "/Common/addLanguage"; 
    return this.http.post<LanguageAddModel>(apiurl,obj)
  }  

  UpdateLanguage(obj: LanguageAddModel){   
    let apiurl = this.apiUrl + obj._tenantName+ "/Common/updateLanguage"; 
    return this.http.post<LanguageAddModel>(apiurl,obj)
  }  

  DeleteLanguage(obj: LanguageAddModel){  
    let apiurl = this.apiUrl + obj._tenantName+ "/Common/deleteLanguage"; 
    return this.http.post<LanguageAddModel>(apiurl,obj)
  } 
  getAllDropdownValues(obj:LovList){
    let apiurl =this.apiUrl + obj._tenantName+ "/Common/getAllDropdownValues"; 
    return this.http.post<LovList>(apiurl,obj);
  }
  addDropdownValue(obj:LovAddView){
    let apiurl =this.apiUrl + obj._tenantName+ "/Common/addDropdownValue"; 
    return this.http.post<LovAddView>(apiurl,obj);
  }
  updateDropdownValue(obj:LovAddView){
    let apiurl =this.apiUrl + obj._tenantName+ "/Common/updateDropdownValue"; 
    return this.http.put<LovAddView>(apiurl,obj);
  }
  deleteDropdownValue(obj:LovAddView){
    let apiurl =this.apiUrl + obj._tenantName+ "/Common/deleteDropdownValue"; 
    return this.http.post<LovAddView>(apiurl,obj);
  }

  getReleaseNumber(obj:ReleaseNumberAddViewModel){
    let apiurl =this.apiUrl + obj._tenantName+ "/Common/getReleaseNumber"; 
    return this.http.post<ReleaseNumberAddViewModel>(apiurl,obj);
  }

}
