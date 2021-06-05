import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { DefaultValuesService } from '../common/default-values.service';
import { StudentTranscript } from '../models/student-transcript.model';

@Injectable({
  providedIn: 'root'
})

export class StudentTranscriptService {
    apiUrl: string = environment.apiURL;

    constructor(
      private http: HttpClient,
      private defaultValuesService: DefaultValuesService) { }
    
      addTranscriptForStudent(obj: StudentTranscript) {
        obj = this.defaultValuesService.getAllMandatoryVariable(obj);
        const apiurl = this.apiUrl + obj._tenantName + '/Student/addTranscriptForStudent';
        return this.http.post<StudentTranscript>(apiurl, obj);
      }

      generateTranscriptForStudent(obj: StudentTranscript) {
        obj = this.defaultValuesService.getAllMandatoryVariable(obj);
        const apiurl = this.apiUrl + obj._tenantName + '/Student/generateTranscriptForStudent';
        return this.http.post<StudentTranscript>(apiurl, obj);
      }
}