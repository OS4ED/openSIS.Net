import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { StudentCourseSectionScheduleAddViewModel } from '../models/studentCourseSectionScheduleAddViewModel';

@Injectable({
  providedIn: 'root'
})
export class StudentScheduleService {
  apiUrl:string = environment.apiURL;
  constructor(private http: HttpClient) { }

  addStudentCourseSectionSchedule(Obj:StudentCourseSectionScheduleAddViewModel){
    let apiurl=this.apiUrl+Obj._tenantName+"/StudentSchedule/addStudentCourseSectionSchedule";  
    return this.http.post<StudentCourseSectionScheduleAddViewModel>(apiurl,Obj)
  }

}