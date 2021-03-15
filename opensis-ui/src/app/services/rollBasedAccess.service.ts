import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { PermissionGroupListViewModel, RolePermissionListViewModel } from '../models/rollBasedAccessModel';
import { BehaviorSubject } from 'rxjs';
import { CryptoService } from './Crypto.service';

@Injectable({
    providedIn: 'root'
})
export class RollBasedAccessService {
    private permissionList = new BehaviorSubject([]);
    private permission;
    currentpermissionList= this.permissionList.asObservable();
    private obj={"memberId": 0, "memberTitle":"","memberDescription":""}
    private member = new BehaviorSubject(this.obj);
    selectedMember= this.member.asObservable();

    apiUrl: string = environment.apiURL;
    constructor(private http: HttpClient, private cryptoService: CryptoService) { }
    getAllPermissionGroup(permission: PermissionGroupListViewModel) {
        let apiurl = this.apiUrl + permission._tenantName + "/RoleBasedAccess/getAllPermissionGroup";
        return this.http.post<PermissionGroupListViewModel>(apiurl, permission)
    }

    getAllRolePermission(permission: RolePermissionListViewModel) {
        let apiurl = this.apiUrl + permission._tenantName + "/RoleBasedAccess/getAllRolePermission";
        return this.http.post<RolePermissionListViewModel>(apiurl, permission)
    }

    getPermission(){
      return this.permission;
    }

    setPermission(){
        this.permission = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')))
    }
    
    updateRolePermission(permission: PermissionGroupListViewModel) {
        let apiurl = this.apiUrl + permission._tenantName + "/RoleBasedAccess/updateRolePermission";
        return this.http.put<PermissionGroupListViewModel>(apiurl, permission)
    }
    send(message){       
        this.permissionList.next(message)
    }
    sendSelectedMember(member){
        this.member.next(member)
    }

    private accessControlStatus = new BehaviorSubject<boolean>(false);
    permissionsChanged = this.accessControlStatus.asObservable();
  
    changeAccessControl(message:boolean) {
      this.accessControlStatus.next(message)
    }

}