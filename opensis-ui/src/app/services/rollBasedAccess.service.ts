import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { PermissionGroupListViewModel, RolePermissionListViewModel } from '../models/rollBasedAccessModel';
import { BehaviorSubject } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class RollBasedAccessService {
    private permissionList = new BehaviorSubject([]);
    currentpermissionList= this.permissionList.asObservable();
    private member = new BehaviorSubject([]);
    selectedMember= this.member.asObservable();

    apiUrl: string = environment.apiURL;
    constructor(private http: HttpClient) { }
    getAllPermissionGroup(permission: PermissionGroupListViewModel) {
        let apiurl = this.apiUrl + permission._tenantName + "/RoleBasedAccess/getAllPermissionGroup";
        return this.http.post<PermissionGroupListViewModel>(apiurl, permission)
    }

    getAllRolePermission(permission: RolePermissionListViewModel) {
        let apiurl = this.apiUrl + permission._tenantName + "/RoleBasedAccess/getAllRolePermission";
        return this.http.post<RolePermissionListViewModel>(apiurl, permission)
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
}