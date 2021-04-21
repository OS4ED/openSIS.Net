import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable, of, Subject } from 'rxjs';
import { GetAllMembersList, Membership,AddMembershipModel } from '../models/membership.model';
import { NoticeAddViewModel, NoticeListViewModel } from '../models/notice.model';
import { NoticeDeleteModel } from '../models/notice-delete.model';
import { DefaultValuesService } from '../common/default-values.service';
@Injectable({
  providedIn: 'root'
})
export class MembershipService {

  apiUrl: string = environment.apiURL;
  constructor(private http: HttpClient, private defaultValuesService: DefaultValuesService) { }

  getAllMembers(obj: GetAllMembersList) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName + '/Membership/getAllMembers';
    return this.http.post<GetAllMembersList>(apiurl, obj);
  }

  addMembership(obj: AddMembershipModel) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName + '/Membership/addMembership';
    return this.http.post<AddMembershipModel>(apiurl, obj);
  }
  updateMembership(obj: AddMembershipModel) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName + '/Membership/updateMembership';
    return this.http.put<AddMembershipModel>(apiurl, obj);
  }

  deleteMembership(obj: AddMembershipModel) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName + '/Membership/deleteMembership';
    return this.http.post<AddMembershipModel>(apiurl, obj);
  }

}
