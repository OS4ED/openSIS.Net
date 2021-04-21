import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { AddGradeLevelModel, GelAllGradeEquivalencyModel, GetAllGradeLevelsModel } from '../models/grade-level.model';
import { BehaviorSubject } from 'rxjs';
import { DefaultValuesService } from '../common/default-values.service';

@Injectable({
  providedIn: 'root'
})
export class GradeLevelService {

  apiUrl:string = environment.apiURL;
  constructor(private http: HttpClient,private defaultValuesService: DefaultValuesService) { }

  getAllGradeLevels(obj: GetAllGradeLevelsModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName+ "/Gradelevel/getAllGradeLevels";   
    return this.http.post<GetAllGradeLevelsModel>(apiurl,obj)
  }

  addGradelevel(obj:AddGradeLevelModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.tblGradelevel.schoolId= this.defaultValuesService.getSchoolID();
    obj.tblGradelevel.tenantId= this.defaultValuesService.getTenantID()
    let apiurl = this.apiUrl + obj._tenantName+ "/Gradelevel/addGradelevel";
    return this.http.post<AddGradeLevelModel>(apiurl,obj)
  }

  updateGradelevel(obj:AddGradeLevelModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.tblGradelevel.schoolId= this.defaultValuesService.getSchoolID();
    obj.tblGradelevel.tenantId= this.defaultValuesService.getTenantID()
    let apiurl = this.apiUrl + obj._tenantName+ "/Gradelevel/updateGradelevel";
    return this.http.put<AddGradeLevelModel>(apiurl,obj)
  }

  deleteGradelevel(obj:AddGradeLevelModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.tblGradelevel.schoolId= this.defaultValuesService.getSchoolID();
    obj.tblGradelevel.tenantId= this.defaultValuesService.getTenantID()
    let apiurl = this.apiUrl + obj._tenantName+ "/Gradelevel/deleteGradelevel";
    return this.http.post<AddGradeLevelModel>(apiurl,obj)
  }

  getAllGradeEquivalency(obj:GelAllGradeEquivalencyModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName+ "/Gradelevel/getAllGradeEquivalency";
    return this.http.post<GelAllGradeEquivalencyModel>(apiurl,obj)
  }
}
