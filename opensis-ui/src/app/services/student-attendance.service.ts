import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { DefaultValuesService } from '../common/default-values.service';
import { GetAllCourseListModel } from '../models/course-manager.model';
import { ScheduledCourseSectionViewModel } from '../models/dashboard.model';
import { GetAllStaffModel } from '../models/staff.model';
import {
  AddUpdateStudentAttendanceModel,
  AddUpdateStudentAttendanceModelFor360,
  GetAllStudentAttendanceListModel,
  SearchCourseSectionForStudentAttendance,
  StaffDetailsModel, 
  StudentUpdateAttendanceCommentsModel} from '../models/take-attendance-list.model';

@Injectable({
  providedIn: 'root'
})
export class StudentAttendanceService {
  apiUrl: string = environment.apiURL;
  userName = sessionStorage.getItem('user');
  staffDetails: StaffDetailsModel = new StaffDetailsModel();

  constructor(
    private http: HttpClient,
    private defaultValuesService: DefaultValuesService
  ) { }

  setStaffDetails(staffDetails) {
    this.staffDetails = staffDetails;
  }

  getStaffDetails() {
    return this.staffDetails;
  }

  getAllStaffList(obj: GetAllStaffModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    const apiurl = this.apiUrl + obj._tenantName + '/Staff/getAllStaffList';
    return this.http.post<GetAllStaffModel>(apiurl, obj);
  }

  getAllCourcesForStudentAttendance(obj: SearchCourseSectionForStudentAttendance) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    const apiurl = this.apiUrl + obj._tenantName + '/StudentAttendance/searchCourseSectionForStudentAttendance';
    return this.http.post<SearchCourseSectionForStudentAttendance>(apiurl, obj);
  }

  getAllCourseSectionList(courseManager: GetAllCourseListModel) {
    courseManager = this.defaultValuesService.getAllMandatoryVariable(courseManager);
    const apiurl = this.apiUrl + courseManager._tenantName + '/CourseManager/getAllCourseList';
    return this.http.post<GetAllCourseListModel>(apiurl, courseManager);
  }

  getAllStudentAttendanceList(studentAttendance: GetAllStudentAttendanceListModel) {
    studentAttendance = this.defaultValuesService.getAllMandatoryVariable(studentAttendance);
    const apiurl = this.apiUrl + studentAttendance._tenantName + '/StudentAttendance/getAllStudentAttendanceList';
    return this.http.post<GetAllStudentAttendanceListModel>(apiurl, studentAttendance);
  }

  addUpdateStudentAttendance(studentAttendance: AddUpdateStudentAttendanceModel) {
    studentAttendance = this.defaultValuesService.getAllMandatoryVariable(studentAttendance);
    const apiurl = this.apiUrl + studentAttendance._tenantName + '/StudentAttendance/addUpdateStudentAttendance';
    return this.http.post<AddUpdateStudentAttendanceModel>(apiurl, studentAttendance);
  }

  addUpdateStudentAttendanceForStudent360(studentAttendance: AddUpdateStudentAttendanceModelFor360) {
    studentAttendance = this.defaultValuesService.getAllMandatoryVariable(studentAttendance);
    const apiurl = this.apiUrl + studentAttendance._tenantName + '/StudentAttendance/addUpdateStudentAttendanceForStudent360';
    return this.http.post<AddUpdateStudentAttendanceModelFor360>(apiurl, studentAttendance);
  }

  staffListForMissingAttendance(obj: GetAllStaffModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    const apiurl = this.apiUrl + obj._tenantName + '/StudentAttendance/staffListForMissingAttendance';
    return this.http.post<GetAllStaffModel>(apiurl, obj);
  }

  missingAttendanceList(obj: GetAllStaffModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    const apiurl = this.apiUrl + obj._tenantName + '/StudentAttendance/missingAttendanceList';
    return this.http.post<ScheduledCourseSectionViewModel>(apiurl, obj);
  }

  addUpdateStudentAttendanceComments(obj: StudentUpdateAttendanceCommentsModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.studentAttendanceComments.tenantId = this.defaultValuesService.getTenantID();
    obj.studentAttendanceComments.schoolId = this.defaultValuesService.getSchoolID();
    const apiurl = this.apiUrl + obj._tenantName + '/StudentAttendance/addUpdateStudentAttendanceComments';
    return this.http.post<StudentUpdateAttendanceCommentsModel>(apiurl, obj);
  }
}
