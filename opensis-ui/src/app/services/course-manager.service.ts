import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { GetAllSubjectModel,AddSubjectModel,MassUpdateSubjectModel,MassUpdateProgramModel,AddProgramModel,DeleteSubjectModel,DeleteProgramModel,GetAllProgramModel} from '../models/courseManagerModel';
import { GetAllCourseListModel,AddCourseModel} from '../models/courseManagerModel';
import { CryptoService } from './Crypto.service';

@Injectable({
  providedIn: 'root'
})
export class CourseManagerService {  
    apiUrl: string = environment.apiURL;
    constructor(private http: HttpClient, private cryptoService: CryptoService) { }

    GetAllSubjectList(courseManager: GetAllSubjectModel) {
        let apiurl = this.apiUrl + courseManager._tenantName + "/CourseManager/getAllSubjectList";
        return this.http.post<GetAllSubjectModel>(apiurl, courseManager)
    }
    
    AddEditSubject(courseManager: MassUpdateSubjectModel) {
        let apiurl = this.apiUrl + courseManager._tenantName + "/CourseManager/addEditSubject";
        return this.http.put<AddSubjectModel>(apiurl, courseManager)
    }
   
    DeleteSubject(courseManager: DeleteSubjectModel) {
        let apiurl = this.apiUrl + courseManager._tenantName + "/CourseManager/deleteSubject";
        return this.http.post<DeleteSubjectModel>(apiurl, courseManager)
    }

    GetAllProgramsList(courseManager: GetAllProgramModel) {
        let apiurl = this.apiUrl + courseManager._tenantName + "/CourseManager/getAllProgram";
        return this.http.post<GetAllProgramModel>(apiurl, courseManager)
    }
    AddEditPrograms(courseManager: MassUpdateProgramModel) {
        let apiurl = this.apiUrl + courseManager._tenantName + "/CourseManager/addEditProgram";
        return this.http.put<AddProgramModel>(apiurl, courseManager)
    }
    
    DeletePrograms(courseManager: DeleteProgramModel) {
        let apiurl = this.apiUrl + courseManager._tenantName + "/CourseManager/deleteProgram";
        return this.http.post<DeleteProgramModel>(apiurl, courseManager)
    }



    GetAllCourseList(courseManager: GetAllCourseListModel) {
        let apiurl = this.apiUrl + courseManager._tenantName + "/CourseManager/getAllCourseList";
        return this.http.post<GetAllCourseListModel>(apiurl, courseManager)
    }
    AddCourse(courseManager: AddCourseModel) {
        let apiurl = this.apiUrl + courseManager._tenantName + "/CourseManager/addCourse";
        return this.http.post<AddCourseModel>(apiurl, courseManager)
    }
    UpdateCourse(courseManager: AddCourseModel) {
        let apiurl = this.apiUrl + courseManager._tenantName + "/CourseManager/updateCourse";
        return this.http.put<AddCourseModel>(apiurl, courseManager)
    }
    
    DeleteCourse(courseManager: AddCourseModel) {
        
        let apiurl = this.apiUrl + courseManager._tenantName + "/CourseManager/deleteCourse";
        return this.http.post<AddCourseModel>(apiurl, courseManager)
    }
}
