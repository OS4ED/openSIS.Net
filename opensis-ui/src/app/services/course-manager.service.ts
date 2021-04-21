import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { DefaultValuesService } from '../common/default-values.service';
import { GetAllSubjectModel, AddSubjectModel, MassUpdateSubjectModel, MassUpdateProgramModel, AddProgramModel, DeleteSubjectModel, DeleteProgramModel, GetAllProgramModel, SearchCourseForScheduleModel } from '../models/course-manager.model';
import { GetAllCourseListModel, AddCourseModel } from '../models/course-manager.model';
import { CryptoService } from './Crypto.service';

@Injectable({
    providedIn: 'root'
})
export class CourseManagerService {
    apiUrl: string = environment.apiURL;
    constructor(private http: HttpClient, private cryptoService: CryptoService,
        private defaultValuesService: DefaultValuesService) { }

    GetAllSubjectList(courseManager: GetAllSubjectModel) {
        courseManager = this.defaultValuesService.getAllMandatoryVariable(courseManager);
        let apiurl = this.apiUrl + courseManager._tenantName + "/CourseManager/getAllSubjectList";
        return this.http.post<GetAllSubjectModel>(apiurl, courseManager)
    }

    AddEditSubject(courseManager: MassUpdateSubjectModel) {
        courseManager = this.defaultValuesService.getAllMandatoryVariable(courseManager);
        let apiurl = this.apiUrl + courseManager._tenantName + "/CourseManager/addEditSubject";
        return this.http.put<AddSubjectModel>(apiurl, courseManager)
    }

    DeleteSubject(courseManager: DeleteSubjectModel) {
        courseManager = this.defaultValuesService.getAllMandatoryVariable(courseManager);
        courseManager.subject.tenantId = this.defaultValuesService.getTenantID();
        courseManager.subject.schoolId = this.defaultValuesService.getSchoolID();
        let apiurl = this.apiUrl + courseManager._tenantName + "/CourseManager/deleteSubject";
        return this.http.post<DeleteSubjectModel>(apiurl, courseManager)
    }

    GetAllProgramsList(courseManager: GetAllProgramModel) {
        courseManager = this.defaultValuesService.getAllMandatoryVariable(courseManager);
        let apiurl = this.apiUrl + courseManager._tenantName + "/CourseManager/getAllProgram";
        return this.http.post<GetAllProgramModel>(apiurl, courseManager)
    }
    AddEditPrograms(courseManager: MassUpdateProgramModel) {
        courseManager = this.defaultValuesService.getAllMandatoryVariable(courseManager);
        let apiurl = this.apiUrl + courseManager._tenantName + "/CourseManager/addEditProgram";
        return this.http.put<AddProgramModel>(apiurl, courseManager);
    }

    DeletePrograms(courseManager: DeleteProgramModel) {
        courseManager = this.defaultValuesService.getAllMandatoryVariable(courseManager);
        courseManager.programs.schoolId = this.defaultValuesService.getSchoolID();
        courseManager.programs.tenantId =this.defaultValuesService.getTenantID();
        let apiurl = this.apiUrl + courseManager._tenantName + "/CourseManager/deleteProgram";
        return this.http.post<DeleteProgramModel>(apiurl, courseManager)
    }



    GetAllCourseList(courseManager: GetAllCourseListModel) {
        courseManager = this.defaultValuesService.getAllMandatoryVariable(courseManager);
        let apiurl = this.apiUrl + courseManager._tenantName + "/CourseManager/getAllCourseList";
        return this.http.post<GetAllCourseListModel>(apiurl, courseManager)
    }
    AddCourse(courseManager: AddCourseModel) {
        courseManager = this.defaultValuesService.getAllMandatoryVariable(courseManager);
        courseManager.course.schoolId = this.defaultValuesService.getSchoolID();
        courseManager.course.tenantId = this.defaultValuesService.getTenantID();
        let apiurl = this.apiUrl + courseManager._tenantName + "/CourseManager/addCourse";
        return this.http.post<AddCourseModel>(apiurl, courseManager)
    }
    UpdateCourse(courseManager: AddCourseModel) {
        courseManager = this.defaultValuesService.getAllMandatoryVariable(courseManager);
        courseManager.course.schoolId = this.defaultValuesService.getSchoolID();
        courseManager.course.tenantId = this.defaultValuesService.getTenantID();
        let apiurl = this.apiUrl + courseManager._tenantName + "/CourseManager/updateCourse";
        return this.http.put<AddCourseModel>(apiurl, courseManager)
    }

    DeleteCourse(courseManager: AddCourseModel) {
        courseManager = this.defaultValuesService.getAllMandatoryVariable(courseManager);
        courseManager.course.schoolId = this.defaultValuesService.getSchoolID();
        courseManager.course.tenantId =  this.defaultValuesService.getTenantID();
        let apiurl = this.apiUrl + courseManager._tenantName + "/CourseManager/deleteCourse";
        return this.http.post<AddCourseModel>(apiurl, courseManager)
    }
    searchCourseForSchedule(searchParams: SearchCourseForScheduleModel) {
        searchParams = this.defaultValuesService.getAllMandatoryVariable(searchParams);
        let apiurl = this.apiUrl + searchParams._tenantName + "/CourseManager/searchCourseForSchedule";
        return this.http.post<SearchCourseForScheduleModel>(apiurl, searchParams)
    }
}
