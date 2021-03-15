import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable, of, Subject } from 'rxjs';
import { GetAllMembersList, Membership,AddMembershipModel } from '../models/membershipModel';
import { NoticeAddViewModel, NoticeListViewModel } from '../models/noticeModel';
import { NoticeDeleteModel } from '../models/noticeDeleteModel';
@Injectable({
  providedIn: 'root'
})
export class MembershipService {

  apiUrl: string = environment.apiURL;
  constructor(private http: HttpClient) { }

  getAllMembers(obj: GetAllMembersList) {
    let apiurl = this.apiUrl + obj._tenantName + "/Membership/getAllMembers";
    return this.http.post<GetAllMembersList>(apiurl, obj)
  }

  addMembership(obj: AddMembershipModel) {
    let apiurl = this.apiUrl + obj._tenantName + "/Membership/addMembership";
    return this.http.post<AddMembershipModel>(apiurl, obj)
  }
  updateMembership(obj: AddMembershipModel) {
    let apiurl = this.apiUrl + obj._tenantName + "/Membership/updateMembership";
    return this.http.put<AddMembershipModel>(apiurl, obj)
  }

  deleteMembership(obj: AddMembershipModel) {
    let apiurl = this.apiUrl + obj._tenantName + "/Membership/deleteMembership";
    return this.http.post<AddMembershipModel>(apiurl, obj)
  }

}