import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { ScheduledStaffForCourseSection } from '../models/course-section.model';
import { AllScheduledCourseSectionForStaffModel, StaffScheduleViewModel } from '../models/teacher-schedule.model';
import { DefaultValuesService } from '../common/default-values.service';

@Injectable({
  providedIn: 'root'
})
export class TeacherScheduleService {
  apiUrl: string = environment.apiURL;

  constructor(private http: HttpClient, private defaultValuesService: DefaultValuesService) { }

  staffScheduleViewForCourseSection(teacherSchedule: StaffScheduleViewModel){
    teacherSchedule = this.defaultValuesService.getAllMandatoryVariable(teacherSchedule);
    let apiurl = this.apiUrl + teacherSchedule._tenantName + "/StaffSchedule/staffScheduleViewForCourseSection";
    return this.http.post<StaffScheduleViewModel>(apiurl, teacherSchedule);
  }
  
  addStaffCourseSectionSchedule(teacherSchedule: StaffScheduleViewModel){
    teacherSchedule = this.defaultValuesService.getAllMandatoryVariable(teacherSchedule);
    let apiurl = this.apiUrl + teacherSchedule._tenantName + "/StaffSchedule/addStaffCourseSectionSchedule";
    return this.http.post<StaffScheduleViewModel>(apiurl, teacherSchedule);
  }
  
  checkAvailabilityStaffCourseSectionSchedule(teacherSchedule: StaffScheduleViewModel){
    teacherSchedule = this.defaultValuesService.getAllMandatoryVariable(teacherSchedule);
    let apiurl = this.apiUrl + teacherSchedule._tenantName + "/StaffSchedule/checkAvailabilityStaffCourseSectionSchedule";
    return this.http.post<StaffScheduleViewModel>(apiurl, teacherSchedule);
  }

  getAllScheduledCourseSectionForStaff(reassignmentDetails: AllScheduledCourseSectionForStaffModel){
    reassignmentDetails = this.defaultValuesService.getAllMandatoryVariable(reassignmentDetails);
    let apiurl = this.apiUrl + reassignmentDetails._tenantName + "/StaffSchedule/getAllScheduledCourseSectionForStaff";
    return this.http.post<AllScheduledCourseSectionForStaffModel>(apiurl, reassignmentDetails);
  }

    addStaffCourseSectionReSchedule(reassignmentDetails: StaffScheduleViewModel) {
    reassignmentDetails = this.defaultValuesService.getAllMandatoryVariable(reassignmentDetails);
    let apiurl = this.apiUrl + reassignmentDetails._tenantName + "/StaffSchedule/AddStaffCourseSectionReSchedule";
    return this.http.post<StaffScheduleViewModel>(apiurl, reassignmentDetails);
  }

  checkAvailabilityStaffCourseSectionReSchedule(reassignmentDetails: ScheduledStaffForCourseSection){
    reassignmentDetails = this.defaultValuesService.getAllMandatoryVariable(reassignmentDetails);
    let apiurl = this.apiUrl + reassignmentDetails._tenantName + "/StaffSchedule/checkAvailabilityStaffCourseSectionReSchedule";
    return this.http.post<ScheduledStaffForCourseSection>(apiurl, reassignmentDetails)
  }
    
  addStaffCourseSectionReScheduleByCourse(reassignmentDetails : ScheduledStaffForCourseSection){
    let apiurl = this.apiUrl + reassignmentDetails._tenantName + "/StaffSchedule/addStaffCourseSectionReScheduleByCourse";
    return this.http.post<ScheduledStaffForCourseSection>(apiurl, reassignmentDetails)
  }


}
