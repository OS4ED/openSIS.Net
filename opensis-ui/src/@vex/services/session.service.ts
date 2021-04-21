import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { UserViewModel } from '../../app/models/user.model';
import { DefaultValuesService } from '../../app/common/default-values.service';
@Injectable({
  providedIn: 'root'
})
export class SessionService {
  apiUrl: string = environment.apiURL;
  constructor(private http: HttpClient, private defaultValuesService: DefaultValuesService) { }

  RefreshToken(obj: UserViewModel) {
    obj.email = this.defaultValuesService.getEmailId();
    let apiurl = this.apiUrl + obj._tenantName + "/User/RefreshToken";
    return this.http.post<UserViewModel>(apiurl, obj)
  }

}
