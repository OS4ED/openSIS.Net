import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { ViewParentInfoModel, GetAllParentModel, AddParentInfoModel ,ParentInfoList,GetAllParentInfoModel,RemoveAssociateParent} from '../models/parentInfoModel';
import { CryptoService } from './Crypto.service';
import { BehaviorSubject, Subject } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class ParentInfoService {

  apiUrl: string = environment.apiURL;
  private parentId;
  private parentDetails;
  
  constructor(private http: HttpClient, private cryptoService: CryptoService) { }

  
  setParentId(id: number) {
    this.parentId = id
  }
  getParentId() {
    return this.parentId;
  }
  setParentDetails(data) {
    this.parentDetails = data
  }
  getParentDetails() {
    return this.parentDetails;
  }

// Update Mode in Parent
 private pageMode = new Subject;
 modeToUpdate=this.pageMode.asObservable();

 changePageMode(mode:number){
     this.pageMode.next(mode);
 } 

  ViewParentListForStudent(parentInfo: ViewParentInfoModel) {
    let apiurl = this.apiUrl + parentInfo._tenantName + "/ParentInfo/ViewParentListForStudent";
    return this.http.post<ViewParentInfoModel>(apiurl, parentInfo)
  }
  viewParentInfo(parentInfo: AddParentInfoModel) {
    let apiurl = this.apiUrl + parentInfo._tenantName + "/ParentInfo/viewParentInfo";
    return this.http.post<AddParentInfoModel>(apiurl, parentInfo)
  }

  updateParentInfo(parentInfo: AddParentInfoModel) {
    parentInfo.parentInfo.parentPhoto = this.parentImage;
    let apiurl = this.apiUrl + parentInfo._tenantName + "/ParentInfo/updateParentInfo";
    return this.http.put<AddParentInfoModel>(apiurl, parentInfo)
  }
  getAllParentInfo(Obj: GetAllParentModel) {
    let apiurl = this.apiUrl + Obj._tenantName + "/ParentInfo/getAllParentInfo";
    return this.http.post<GetAllParentModel>(apiurl, Obj)
  }
  addParentForStudent(obj: AddParentInfoModel){
    obj.passwordHash = this.cryptoService.encrypt(obj.passwordHash);
    obj.parentInfo.parentPhoto = this.parentImage;
    let apiurl = this.apiUrl + obj._tenantName+ "/ParentInfo/addParentForStudent";   
    return this.http.post<AddParentInfoModel>(apiurl,obj)
  }
  deleteParentInfo(obj: AddParentInfoModel){
    let apiurl = this.apiUrl + obj._tenantName+ "/ParentInfo/deleteParentInfo";   
    return this.http.post<AddParentInfoModel>(apiurl,obj)
  }
  searchParentInfoForStudent(obj: ParentInfoList){
    let apiurl = this.apiUrl + obj._tenantName+ "/ParentInfo/searchParentInfoForStudent";   
    return this.http.post<ParentInfoList>(apiurl,obj)
  }

  viewParentListForStudent(obj: GetAllParentInfoModel){
    let apiurl = this.apiUrl + obj._tenantName+ "/ParentInfo/viewParentListForStudent";   
    return this.http.post<GetAllParentInfoModel>(apiurl,obj)
  }

  removeAssociatedParent(obj: RemoveAssociateParent){
    let apiurl = this.apiUrl + obj._tenantName+ "/ParentInfo/removeAssociatedParent";   
    return this.http.post<RemoveAssociateParent>(apiurl,obj)
  }

  addUpdateParentPhoto(obj: AddParentInfoModel){
    obj.parentInfo.parentId = this.getParentId();
    obj.parentInfo.parentPhoto = this.parentImage;
    let apiurl = this.apiUrl + obj._tenantName+ "/ParentInfo/addUpdateParentPhoto";   
    return this.http.put<AddParentInfoModel>(apiurl,obj)
  }


// to Update staff details in General for first time.
private parentDetailsForGeneralInfo = new Subject;
getParentDetailsForGeneral = this.parentDetailsForGeneralInfo.asObservable();
sendDetails(parentDetailsForGeneralInfo) {
  this.parentDetailsForGeneralInfo.next(parentDetailsForGeneralInfo);
}

private parentImage;
setParentImage(imageInBase64) {
  this.parentImage = imageInBase64;
}


  
  
  
  

  


}
