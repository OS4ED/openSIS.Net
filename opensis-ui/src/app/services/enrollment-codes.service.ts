import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import {EnrollmentCodeAddView,EnrollmentCodeListView} from '../models/enrollment-code.model'

@Injectable({
  providedIn: 'root'
})
export class EnrollmentCodesService {

  apiUrl:string = environment.apiURL;
  constructor(private http: HttpClient) { }

  addStudentEnrollmentCode(obj:EnrollmentCodeAddView){
    let apiurl=this.apiUrl+obj._tenantName+"/StudentEnrollmentCode/addStudentEnrollmentCode";  
    return this.http.post<EnrollmentCodeAddView>(apiurl,obj)
  }
  getAllStudentEnrollmentCode(obj:EnrollmentCodeListView){
    let apiurl=this.apiUrl+obj._tenantName+"/StudentEnrollmentCode/getAllStudentEnrollmentCode";
    return this.http.post<EnrollmentCodeListView>(apiurl,obj)
  }
  updateStudentEnrollmentCode(obj:EnrollmentCodeAddView){
    let apiurl=this.apiUrl+obj._tenantName+"/StudentEnrollmentCode/updateStudentEnrollmentCode";
    return this.http.put<EnrollmentCodeAddView>(apiurl,obj)
  }
  deleteStudentEnrollmentCode(obj:EnrollmentCodeAddView){
    let apiurl=this.apiUrl+obj._tenantName+"/StudentEnrollmentCode/deleteStudentEnrollmentCode";
    return this.http.post<EnrollmentCodeAddView>(apiurl,obj)
  }

}
