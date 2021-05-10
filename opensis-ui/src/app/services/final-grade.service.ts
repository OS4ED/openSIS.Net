import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { DefaultValuesService } from '../common/default-values.service';
import { CourseStandardForCourseViewModel, GetAllCourseListModel } from '../models/course-manager.model';
import { GetAllStaffModel } from '../models/staff.model';
import { AddUpdateStudentFinalGradeModel } from '../models/student-final-grade.model';
import { AddUpdateStudentAttendanceModel, GetAllStudentAttendanceListModel, SearchCourseSectionForStudentAttendance, StaffDetailsModel } from '../models/take-attendance-list.model';

@Injectable({
    providedIn: 'root'
})
export class FinalGradeService {
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

    addUpdateStudentFinalGrade(obj: AddUpdateStudentFinalGradeModel) {
        obj = this.defaultValuesService.getAllMandatoryVariable(obj);
        let apiurl = this.apiUrl + obj._tenantName + "/InputFinalGrade/addUpdateStudentFinalGrade";
        return this.http.post<AddUpdateStudentFinalGradeModel>(apiurl, obj)
    }

    getAllStudentFinalGradeList(obj: AddUpdateStudentFinalGradeModel) {
        obj = this.defaultValuesService.getAllMandatoryVariable(obj);
        let apiurl = this.apiUrl + obj._tenantName + "/InputFinalGrade/getAllStudentFinalGradeList";
        return this.http.post<AddUpdateStudentFinalGradeModel>(apiurl, obj)
    }
}