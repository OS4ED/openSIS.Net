import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { NoticeAddViewModel, NoticeListViewModel } from '../models/notice.model';
import { NoticeDeleteModel } from '../models/notice-delete.model';
import { DefaultValuesService } from '../common/default-values.service';

@Injectable({
  providedIn: 'root'
})
export class NoticeService {
  private noticeSource = new BehaviorSubject(false);
  currentNotice = this.noticeSource.asObservable();

  apiUrl: string = environment.apiURL;
  constructor(private http: HttpClient, private defaultValuesService: DefaultValuesService) { }

  addNotice(notice: NoticeAddViewModel) {
    notice = this.defaultValuesService.getAllMandatoryVariable(notice);
    notice.notice.schoolId = this.defaultValuesService.getSchoolID();
    notice.notice.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + notice._tenantName + '/Notice/addNotice';
    return this.http.post<NoticeAddViewModel>(apiurl, notice);
  }
  updateNotice(notice: NoticeAddViewModel) {
    notice = this.defaultValuesService.getAllMandatoryVariable(notice);
    notice.notice.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + notice._tenantName + '/Notice/updateNotice';
    return this.http.post<NoticeAddViewModel>(apiurl, notice);
  }
  getAllNotice(notice: NoticeListViewModel) {
    notice = this.defaultValuesService.getAllMandatoryVariable(notice);
    let apiurl = this.apiUrl + notice._tenantName + '/Notice/getAllNotice';
    return this.http.post<NoticeListViewModel>(apiurl, notice);
  }
  deleteNotice(notice: NoticeDeleteModel) {
    notice._tenantName = this.defaultValuesService.getTenent();
    notice.tenantId = this.defaultValuesService.getTenantID();
    notice._token = this.defaultValuesService.getToken();
    let apiurl = this.apiUrl + notice._tenantName + '/Notice/deleteNotice';
    return this.http.post<NoticeDeleteModel>(apiurl, notice);
  }
  viewNotice(notice: NoticeAddViewModel) {
    let apiurl = this.apiUrl + notice._tenantName + '/Notice/viewNotice';
    return this.http.post<NoticeAddViewModel>(apiurl, notice);
  }
  changeNotice(message: boolean) {
    this.noticeSource.next(message);
  }
}
