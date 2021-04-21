import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { GetAllSectionModel , SectionAddModel} from 'src/app/models/section.model';
import { DefaultValuesService } from '../common/default-values.service';

@Injectable({
  providedIn: 'root'
})
export class SectionService {

  apiUrl: string = environment.apiURL;
  constructor(private http: HttpClient, private defaultValuesService: DefaultValuesService) { }

  GetAllSection(obj: GetAllSectionModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName + '/Section/getAllSection';
    return this.http.post<GetAllSectionModel>(apiurl, obj);
  }
  SaveSection(obj: SectionAddModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.tableSections.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + obj._tenantName + '/Section/addSection';
    return this.http.post<SectionAddModel>(apiurl, obj);
  }
  UpdateSection(obj: SectionAddModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.tableSections.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + obj._tenantName + '/Section/updateSection';
    return this.http.put<SectionAddModel>(apiurl, obj);
  }

  deleteSection(obj: SectionAddModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.tableSections.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + obj._tenantName + '/Section/deleteSection';
    return this.http.post<SectionAddModel>(apiurl, obj);
  }
}
