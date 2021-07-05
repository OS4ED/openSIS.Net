import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { DefaultValuesService } from '../common/default-values.service';
import { SchoolPreferenceAddViewModel } from '../models/school-preference.model';

@Injectable({
    providedIn: 'root'
})
export class SchoolPreferenceService {
    apiUrl: string = environment.apiURL;
    constructor(private http: HttpClient, private defaultValuesService: DefaultValuesService,) { }

    viewPreference(Obj: SchoolPreferenceAddViewModel) {
        Obj = this.defaultValuesService.getAllMandatoryVariable(Obj);
        Obj.schoolPreference.schoolId = this.defaultValuesService.getSchoolID();
        Obj.schoolPreference.tenantId = this.defaultValuesService.getTenantID();
        let apiurl = this.apiUrl + Obj._tenantName + '/Common/viewSchoolPreference';
        return this.http.post<SchoolPreferenceAddViewModel>(apiurl, Obj);
    }

    addUpdateSchoolPreference(Obj: SchoolPreferenceAddViewModel) {
        Obj = this.defaultValuesService.getAllMandatoryVariable(Obj);
        Obj.schoolPreference.schoolId = this.defaultValuesService.getSchoolID();
        Obj.schoolPreference.tenantId = this.defaultValuesService.getTenantID();
        let apiurl = this.apiUrl + Obj._tenantName + '/Common/addUpdateSchoolPreference';
        return this.http.post<SchoolPreferenceAddViewModel>(apiurl, Obj);
    }
}