import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { DashboardViewModel, ScheduledCourseSectionViewModel } from '../models/dashboard.model';
import { Observable,BehaviorSubject } from 'rxjs';
import { DefaultValuesService } from '../common/default-values.service';

@Injectable({
  providedIn: 'root'
})
export class DasboardService {

  apiUrl:string = environment.apiURL;
  private dashboardSubject = new BehaviorSubject(false);;
  constructor(private http: HttpClient , private defaultValuesService: DefaultValuesService) { }

  getDashboardView(obj: DashboardViewModel) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.academicYear= this.defaultValuesService.getAcademicYear();
    let apiurl = this.apiUrl + obj._tenantName + "/Common/getDashboardView";
    return this.http.post<DashboardViewModel>(apiurl, obj)
  }

  getDashboardViewForCalendarView(obj: DashboardViewModel) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.academicYear= this.defaultValuesService.getAcademicYear();
    let apiurl = this.apiUrl + obj._tenantName + "/Common/getDashboardViewForCalendarView";
    return this.http.post<DashboardViewModel>(apiurl, obj)
  }
  getDashboardViewForStaff(obj: ScheduledCourseSectionViewModel) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName + "/Common/getDashboardViewForStaff";
    return this.http.post<ScheduledCourseSectionViewModel>(apiurl, obj)
  }

  sendPageLoadEvent(event) {
    this.dashboardSubject.next(event);
  }
  getPageLoadEvent(): Observable<any> {
    return this.dashboardSubject.asObservable();
  }

  getMissingAttendanceCountForDashboardView(obj: ScheduledCourseSectionViewModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName + "/Common/getMissingAttendanceCountForDashboardView";
    return this.http.post<ScheduledCourseSectionViewModel>(apiurl, obj)
  }


}