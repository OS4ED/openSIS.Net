import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { StaffScheduleViewModel } from '../models/teacher-schedule.model';

@Injectable({
  providedIn: 'root'
})
export class TeacherScheduleService {
  apiUrl: string = environment.apiURL;

  constructor(private http: HttpClient) { }

  staffScheduleViewForCourseSection(teacherSchedule : StaffScheduleViewModel){
    let apiurl = this.apiUrl + teacherSchedule._tenantName + "/StaffSchedule/staffScheduleViewForCourseSection";
    return this.http.post<StaffScheduleViewModel>(apiurl, teacherSchedule);
  }
  
  addStaffCourseSectionSchedule(teacherSchedule : StaffScheduleViewModel){
    let apiurl = this.apiUrl + teacherSchedule._tenantName + "/StaffSchedule/addStaffCourseSectionSchedule";
    return this.http.post<StaffScheduleViewModel>(apiurl, teacherSchedule);
  }
  
  checkAvailabilityStaffCourseSectionSchedule(teacherSchedule : StaffScheduleViewModel){
    let apiurl = this.apiUrl + teacherSchedule._tenantName + "/StaffSchedule/checkAvailabilityStaffCourseSectionSchedule";
    return this.http.post<StaffScheduleViewModel>(apiurl, teacherSchedule);
  }
}
