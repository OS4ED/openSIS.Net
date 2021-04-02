import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { ScheduledStudentDropModel, ScheduleStudentListViewModel, StudentCourseSectionScheduleAddViewModel, StudentScheduleReportViewModel } from '../models/studentCourseSectionScheduleAddViewModel';

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

  searchScheduledStudentForGroupDrop(Obj:ScheduleStudentListViewModel){
    let apiurl=this.apiUrl+Obj._tenantName+"/StudentSchedule/searchScheduledStudentForGroupDrop";  
    return this.http.post<ScheduleStudentListViewModel>(apiurl,Obj)
  }

  groupDropForScheduledStudent(Obj:ScheduledStudentDropModel){
    let apiurl=this.apiUrl+Obj._tenantName+"/StudentSchedule/groupDropForScheduledStudent";  
    return this.http.put<ScheduledStudentDropModel>(apiurl,Obj)
  }

  studentScheduleReport(Obj:StudentScheduleReportViewModel){
    let apiurl=this.apiUrl+Obj._tenantName+"/StudentSchedule/studentScheduleReport";  
    return this.http.post<StudentScheduleReportViewModel>(apiurl,Obj)
  }
}