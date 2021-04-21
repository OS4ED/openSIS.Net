import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { DefaultValuesService } from '../common/default-values.service';
import { RoomAddView, RoomListViewModel } from '../models/room.model';

@Injectable({
  providedIn: 'root'
})
export class RoomService {
  apiUrl: string = environment.apiURL;
  constructor(private http: HttpClient,private defaultValuesService: DefaultValuesService,) { }

  addRoom(Obj: RoomAddView){
    Obj = this.defaultValuesService.getAllMandatoryVariable(Obj);
    Obj.tableRoom.schoolId = this.defaultValuesService.getSchoolID();
    Obj.tableRoom.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + Obj._tenantName + '/Room/addRoom';
    return this.http.post<RoomAddView>(apiurl, Obj);
  }
  updateRoom(Obj: RoomAddView){
    Obj = this.defaultValuesService.getAllMandatoryVariable(Obj);
    Obj.tableRoom.schoolId = this.defaultValuesService.getSchoolID();
    Obj.tableRoom.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + Obj._tenantName + '/Room/updateRoom';
    return this.http.put<RoomAddView>(apiurl, Obj);
  }
  deleteRoom(Obj: RoomAddView){
    Obj = this.defaultValuesService.getAllMandatoryVariable(Obj);
    Obj.tableRoom.schoolId = this.defaultValuesService.getSchoolID();
    Obj.tableRoom.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + Obj._tenantName + '/Room/deleteRoom';
    return this.http.post<RoomAddView>(apiurl, Obj);
  }
  getAllRoom(obj: RoomListViewModel){
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName + '/Room/getAllRoom';
    return this.http.post<RoomListViewModel>(apiurl, obj);
  }
}
