import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { DefaultValuesService } from '../common/default-values.service';
import { AttendanceCodeCategoryModel, AttendanceCodeModel, AttendanceCodeDragDropModel, GetAllAttendanceCategoriesListModel, GetAllAttendanceCodeModel } from '../models/attendance-code.model';
@Injectable({
  providedIn: 'root'
})
export class AttendanceCodeService {

  apiUrl: string = environment.apiURL;
  constructor(private http: HttpClient, private defaultValuesService: DefaultValuesService) { }

  addAttendanceCodeCategories(AttendanceCategory: AttendanceCodeCategoryModel) {
    AttendanceCategory = this.defaultValuesService.getAllMandatoryVariable(AttendanceCategory);
    AttendanceCategory.attendanceCodeCategories.schoolId = this.defaultValuesService.getSchoolID();
    AttendanceCategory.attendanceCodeCategories.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + AttendanceCategory._tenantName + "/AttendanceCode/addAttendanceCodeCategories";
    return this.http.post<AttendanceCodeCategoryModel>(apiurl, AttendanceCategory);
  }

  getAllAttendanceCodeCategories(AttendanceCategoryList: GetAllAttendanceCategoriesListModel) {
    AttendanceCategoryList = this.defaultValuesService.getAllMandatoryVariable(AttendanceCategoryList);
    let apiurl = this.apiUrl + AttendanceCategoryList._tenantName + "/AttendanceCode/getAllAttendanceCodeCategories";
    return this.http.post<GetAllAttendanceCategoriesListModel>(apiurl, AttendanceCategoryList);
  }

  updateAttendanceCodeCategories(AttendanceCategory: AttendanceCodeCategoryModel) {
    AttendanceCategory = this.defaultValuesService.getAllMandatoryVariable(AttendanceCategory);
    AttendanceCategory.attendanceCodeCategories.schoolId = this.defaultValuesService.getSchoolID();
    AttendanceCategory.attendanceCodeCategories.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + AttendanceCategory._tenantName + "/AttendanceCode/updateAttendanceCodeCategories";
    return this.http.put<AttendanceCodeCategoryModel>(apiurl, AttendanceCategory);
  }

  deleteAttendanceCodeCategories(AttendanceCategory: AttendanceCodeCategoryModel) {
    AttendanceCategory = this.defaultValuesService.getAllMandatoryVariable(AttendanceCategory);
    AttendanceCategory.attendanceCodeCategories.schoolId = this.defaultValuesService.getSchoolID();
    AttendanceCategory.attendanceCodeCategories.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + AttendanceCategory._tenantName + "/AttendanceCode/deleteAttendanceCodeCategories";
    return this.http.post<AttendanceCodeCategoryModel>(apiurl, AttendanceCategory);
  }

  getAllAttendanceCode(AttendanceCode: GetAllAttendanceCodeModel) {
    AttendanceCode = this.defaultValuesService.getAllMandatoryVariable(AttendanceCode);
    let apiurl = this.apiUrl + AttendanceCode._tenantName + "/AttendanceCode/getAllAttendanceCode";
    return this.http.post<GetAllAttendanceCodeModel>(apiurl, AttendanceCode);
  }

  addAttendanceCode(AttendanceCode: AttendanceCodeModel) {
    AttendanceCode = this.defaultValuesService.getAllMandatoryVariable(AttendanceCode);
    AttendanceCode.attendanceCode.schoolId = this.defaultValuesService.getSchoolID();
    AttendanceCode.attendanceCode.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + AttendanceCode._tenantName + "/AttendanceCode/addAttendanceCode";
    return this.http.post<AttendanceCodeModel>(apiurl, AttendanceCode);
  }

  updateAttendanceCode(AttendanceCode: AttendanceCodeModel) {
    AttendanceCode = this.defaultValuesService.getAllMandatoryVariable(AttendanceCode);
    AttendanceCode.attendanceCode.schoolId = this.defaultValuesService.getSchoolID();
    AttendanceCode.attendanceCode.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + AttendanceCode._tenantName + "/AttendanceCode/updateAttendanceCode";
    return this.http.put<AttendanceCodeModel>(apiurl, AttendanceCode);
  }

  deleteAttendanceCode(AttendanceCode: AttendanceCodeModel) {
    AttendanceCode = this.defaultValuesService.getAllMandatoryVariable(AttendanceCode);
    AttendanceCode.attendanceCode.schoolId = this.defaultValuesService.getSchoolID();
    AttendanceCode.attendanceCode.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + AttendanceCode._tenantName + "/AttendanceCode/deleteAttendanceCode";
    return this.http.post<AttendanceCodeModel>(apiurl, AttendanceCode);
  }

  updateAttendanceCodeSortOrder(AttendanceCode: AttendanceCodeDragDropModel) {
    AttendanceCode = this.defaultValuesService.getAllMandatoryVariable(AttendanceCode);
    let apiurl = this.apiUrl + AttendanceCode._tenantName + "/AttendanceCode/updateAttendanceCodeSortOrder";
    return this.http.post<AttendanceCodeDragDropModel>(apiurl, AttendanceCode);
  }


}
