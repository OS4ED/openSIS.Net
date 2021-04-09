import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { ScheduledStaffForCourseSection } from '../models/courseSectionModel';
import { AllScheduledCourseSectionForStaffModel, StaffScheduleViewModel } from '../models/teacher-schedule.model';

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

  getAllScheduledCourseSectionForStaff(reassignmentDetails:AllScheduledCourseSectionForStaffModel){
    let apiurl = this.apiUrl + reassignmentDetails._tenantName + "/StaffSchedule/getAllScheduledCourseSectionForStaff";
    return this.http.post<AllScheduledCourseSectionForStaffModel>(apiurl, reassignmentDetails);
  }

  AddStaffCourseSectionReSchedule(reassignmentDetails:StaffScheduleViewModel){
    let apiurl = this.apiUrl + reassignmentDetails._tenantName + "/StaffSchedule/AddStaffCourseSectionReSchedule";
    return this.http.post<StaffScheduleViewModel>(apiurl, reassignmentDetails);
  }

  checkAvailabilityStaffCourseSectionReSchedule(reassignmentDetails : ScheduledStaffForCourseSection){
    let apiurl = this.apiUrl + reassignmentDetails._tenantName + "/StaffSchedule/checkAvailabilityStaffCourseSectionReSchedule";
    return this.http.post<ScheduledStaffForCourseSection>(apiurl, reassignmentDetails)
  }
}
