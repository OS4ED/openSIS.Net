import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { MarkingPeriodListModel,MarkingPeriodAddModel,SemesterAddModel,QuarterAddModel,ProgressPeriodAddModel, GetAcademicYearListModel, GetMarkingPeriodTitleListModel,GetAllMarkingPeriodTitle} from '../models/markingPeriodModel';
import { BehaviorSubject } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class MarkingPeriodService {

  apiUrl:string = environment.apiURL;
  private currentYear = new BehaviorSubject(false);
  currentY = this.currentYear.asObservable();

  constructor(private http: HttpClient) { }

  GetMarkingPeriod(obj: MarkingPeriodListModel){
    let apiurl = this.apiUrl + obj._tenantName+ "/MarkingPeriod/getMarkingPeriod";   
    return this.http.post<MarkingPeriodListModel>(apiurl,obj)
  }

  AddSchoolYear(obj: MarkingPeriodAddModel){
    let apiurl = this.apiUrl + obj._tenantName+ "/MarkingPeriod/addSchoolYear";   
    return this.http.post<MarkingPeriodAddModel>(apiurl,obj)
  }
  UpdateSchoolYear(obj: MarkingPeriodAddModel){
    let apiurl = this.apiUrl + obj._tenantName+ "/MarkingPeriod/updateSchoolYear";   
    return this.http.put<MarkingPeriodAddModel>(apiurl,obj)
  }
  DeleteSchoolYear(obj: MarkingPeriodAddModel){
    let apiurl = this.apiUrl + obj._tenantName+ "/MarkingPeriod/deleteSchoolYear";   
    return this.http.post<MarkingPeriodAddModel>(apiurl,obj)
  }
  AddSemester(obj: SemesterAddModel){
    let apiurl = this.apiUrl + obj._tenantName+ "/MarkingPeriod/addSemester";   
    return this.http.post<SemesterAddModel>(apiurl,obj)
  }
  UpdateSemester(obj: SemesterAddModel){
   
    let apiurl = this.apiUrl + obj._tenantName+ "/MarkingPeriod/updateSemester";   
    return this.http.put<SemesterAddModel>(apiurl,obj)
  }
  DeleteSemester(obj: SemesterAddModel){
    let apiurl = this.apiUrl + obj._tenantName+ "/MarkingPeriod/deleteSemester";   
    return this.http.post<SemesterAddModel>(apiurl,obj)
  }
  
  AddQuarter(obj: QuarterAddModel){
    let apiurl = this.apiUrl + obj._tenantName+ "/MarkingPeriod/addQuarter";   
    return this.http.post<QuarterAddModel>(apiurl,obj)
  }
  UpdateQuarter(obj: QuarterAddModel){
    let apiurl = this.apiUrl + obj._tenantName+ "/MarkingPeriod/updateQuarter";   
    return this.http.put<QuarterAddModel>(apiurl,obj)
  }
  DeleteQuarter(obj: QuarterAddModel){
    let apiurl = this.apiUrl + obj._tenantName+ "/MarkingPeriod/deleteQuarter";   
    return this.http.post<QuarterAddModel>(apiurl,obj)
  }
  AddProgressPeriod(obj: ProgressPeriodAddModel){
    let apiurl = this.apiUrl + obj._tenantName+ "/MarkingPeriod/addProgressPeriod";   
    return this.http.post<ProgressPeriodAddModel>(apiurl,obj)
  }
  UpdateProgressPeriod(obj: ProgressPeriodAddModel){
    let apiurl = this.apiUrl + obj._tenantName+ "/MarkingPeriod/updateProgressPeriod";   
    return this.http.put<ProgressPeriodAddModel>(apiurl,obj)
  }
  DeleteProgressPeriod(obj: ProgressPeriodAddModel){    
    let apiurl = this.apiUrl + obj._tenantName+ "/MarkingPeriod/deleteProgressPeriod";   
    return this.http.post<ProgressPeriodAddModel>(apiurl,obj)
  }

  // getAcademicYearList and getMarkingPeriodTitleList
  //  is for Select Dropdown Bar for selecting academic year and period
  // which is in right upper corner of opensisv2 site.
  
  getAcademicYearList(obj:GetAcademicYearListModel){
    let apiurl = this.apiUrl + obj._tenantName+ "/MarkingPeriod/getAcademicYearList";   
    return this.http.post<GetAcademicYearListModel>(apiurl,obj)
  }

  getMarkingPeriodTitleList(obj:GetMarkingPeriodTitleListModel){
    let apiurl = this.apiUrl + obj._tenantName+ "/MarkingPeriod/getMarkingPeriodTitleList";   
    return this.http.post<GetMarkingPeriodTitleListModel>(apiurl,obj)
  }
  getCurrentYear(message: boolean) {
    this.currentYear.next(message)
  } 

  getAllMarkingPeriodList(obj:GetMarkingPeriodTitleListModel){
    let apiurl = this.apiUrl + obj._tenantName+ "/MarkingPeriod/getAllMarkingPeriodList";   
    return this.http.post<GetAllMarkingPeriodTitle>(apiurl,obj)
  }
}
