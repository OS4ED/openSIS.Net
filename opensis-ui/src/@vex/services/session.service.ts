import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { UserViewModel } from '../../app/models/userModel';
@Injectable({
  providedIn: 'root'
})
export class SessionService {
  apiUrl: string = environment.apiURL;
  constructor(private http: HttpClient) { }

  RefreshToken(obj: UserViewModel) {
    let apiurl = this.apiUrl + obj._tenantName + "/User/RefreshToken";
    return this.http.post<UserViewModel>(apiurl, obj)
  }

}
