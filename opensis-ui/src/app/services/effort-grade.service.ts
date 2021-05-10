import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { DefaultValuesService } from '../common/default-values.service';
import { StudentEffortGradeListModel } from '../models/student-effort-grade.model';

@Injectable({
    providedIn: 'root'
})
export class EffotrGradeService {
    apiUrl: string = environment.apiURL;
    userName = sessionStorage.getItem('user');
    
    constructor(
        private http: HttpClient,
        private defaultValuesService: DefaultValuesService
    ) { }

    addUpdateStudentEffortGrade(obj: StudentEffortGradeListModel) {
        obj = this.defaultValuesService.getAllMandatoryVariable(obj);
        let apiurl = this.apiUrl + obj._tenantName + "/StudentEffortGrade/addUpdateStudentEffortGrade";
        return this.http.post<StudentEffortGradeListModel>(apiurl, obj)
    }

    getAllStudentEffortGradeList(obj: StudentEffortGradeListModel) {
        obj = this.defaultValuesService.getAllMandatoryVariable(obj);
        let apiurl = this.apiUrl + obj._tenantName + "/StudentEffortGrade/getAllStudentEffortGradeList";
        return this.http.post<StudentEffortGradeListModel>(apiurl, obj)
    }
}
