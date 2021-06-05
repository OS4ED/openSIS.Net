import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { environment } from '../../environments/environment';
import { DefaultValuesService } from '../common/default-values.service';
import { SearchCourseSectionViewModel } from '../models/course-manager.model';
import {
  ClassRoomURLInCourseSectionModel,
  CourseSectionAddViewModel,
  DeleteCourseSectionSchedule,
  GetAllCourseSectionModel,
  GetAllCourseStandardForCourseSectionModel,
  ScheduledStaffForCourseSection } from '../models/course-section.model';
import { CryptoService } from './Crypto.service';

@Injectable({
  providedIn: 'root'
})
export class CourseSectionService {
  apiUrl: string = environment.apiURL;
  constructor(private http: HttpClient, private cryptoService: CryptoService,
    private defaultValuesService: DefaultValuesService) { }

  getAllCourseSection(courseSection: GetAllCourseSectionModel) {
    courseSection = this.defaultValuesService.getAllMandatoryVariable(courseSection);
    let apiurl = this.apiUrl + courseSection._tenantName + "/CourseManager/getAllCourseSection";
    return this.http.post<GetAllCourseSectionModel>(apiurl, courseSection)
  }
  addCourseSection(courseSection: CourseSectionAddViewModel) {
    courseSection = this.defaultValuesService.getAllMandatoryVariable(courseSection);
    courseSection.courseFixedSchedule.schoolId = this.defaultValuesService.getSchoolID();
    courseSection.courseFixedSchedule.tenantId = this.defaultValuesService.getTenantID();
    courseSection.courseSection.schoolId = this.defaultValuesService.getSchoolID();
    courseSection.courseSection.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + courseSection._tenantName + "/CourseManager/addCourseSection";
    return this.http.post<GetAllCourseSectionModel>(apiurl, courseSection)
  }
  updateCourseSection(courseSection: CourseSectionAddViewModel) {
    courseSection = this.defaultValuesService.getAllMandatoryVariable(courseSection);
    courseSection.courseFixedSchedule.schoolId = this.defaultValuesService.getSchoolID();
    courseSection.courseFixedSchedule.tenantId = this.defaultValuesService.getTenantID();
    courseSection.courseSection.schoolId = this.defaultValuesService.getSchoolID();
    courseSection.courseSection.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + courseSection._tenantName + "/CourseManager/updateCourseSection";
    return this.http.put<GetAllCourseSectionModel>(apiurl, courseSection)
  }
  deleteCourseSection(courseSection: CourseSectionAddViewModel) {
    courseSection = this.defaultValuesService.getAllMandatoryVariable(courseSection);
    courseSection.courseFixedSchedule.schoolId = this.defaultValuesService.getSchoolID();
    courseSection.courseFixedSchedule.tenantId = this.defaultValuesService.getTenantID();
    courseSection.courseSection.schoolId = this.defaultValuesService.getSchoolID();
    courseSection.courseSection.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + courseSection._tenantName + "/CourseManager/deleteCourseSection";
    return this.http.post<CourseSectionAddViewModel>(apiurl, courseSection)
  }

  private dataSource = new Subject;
  currentUpdate = this.dataSource.asObservable();

  sendCurrentData(message: boolean) {
    this.dataSource.next(message)
  }

  getAllCourseStandardForCourseSection(courseSection: GetAllCourseStandardForCourseSectionModel) {
    courseSection = this.defaultValuesService.getAllMandatoryVariable(courseSection);
    let apiurl = this.apiUrl + courseSection._tenantName + "/CourseManager/getAllCourseStandardForCourseSection";
    return this.http.post<GetAllCourseStandardForCourseSectionModel>(apiurl, courseSection)
  }

  deleteSchedule(courseSection: DeleteCourseSectionSchedule) {
    courseSection = this.defaultValuesService.getAllMandatoryVariable(courseSection);
    let apiurl = this.apiUrl + courseSection._tenantName + "/CourseManager/deleteSchedule";
    return this.http.post<DeleteCourseSectionSchedule>(apiurl, courseSection);
  }

  updateOnlineClassRoomURLInCourseSection(obj: ClassRoomURLInCourseSectionModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.courseSection.tenantId = this.defaultValuesService.getTenantID();
    obj.courseSection.schoolId = this.defaultValuesService.getSchoolID();
    const apiurl = this.apiUrl + obj._tenantName + '/StaffPortal/updateOnlineClassRoomURLInCourseSection';
    return this.http.put<any>(apiurl, obj);
  }

  private afterDeleted = new Subject;
  callCourseSection = this.afterDeleted.asObservable();

  scheduleDeleted(message: boolean) {
    this.afterDeleted.next(message)
  }

  searchCourseSectionForSchedule(courseSection: SearchCourseSectionViewModel) {
    courseSection = this.defaultValuesService.getAllMandatoryVariable(courseSection);
    let apiurl = this.apiUrl + courseSection._tenantName + "/CourseManager/searchCourseSectionForSchedule";
    return this.http.post<SearchCourseSectionViewModel>(apiurl, courseSection)
  }
  getAllStaffScheduleInCourseSection(courseSection: ScheduledStaffForCourseSection) {
    courseSection = this.defaultValuesService.getAllMandatoryVariable(courseSection);
    let apiurl = this.apiUrl + courseSection._tenantName + "/CourseManager/getAllStaffScheduleInCourseSection";
    return this.http.post<ScheduledStaffForCourseSection>(apiurl, courseSection)
  }
}
