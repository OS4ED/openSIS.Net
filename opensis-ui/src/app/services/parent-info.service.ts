import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { ViewParentInfoModel, GetAllParentModel, AddParentInfoModel, ParentInfoList, GetAllParentInfoModel, RemoveAssociateParent } from '../models/parent-info.model';
import { CryptoService } from './Crypto.service';
import { BehaviorSubject, Subject } from 'rxjs';
import { DefaultValuesService } from '../common/default-values.service';
@Injectable({
  providedIn: 'root'
})
export class ParentInfoService {

  apiUrl: string = environment.apiURL;
  private parentId;
  private parentDetails;
  userName = sessionStorage.getItem('user');

  constructor(private http: HttpClient, private cryptoService: CryptoService, private defaultValuesService: DefaultValuesService) { }


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
  modeToUpdate = this.pageMode.asObservable();

  changePageMode(mode: number) {
    this.pageMode.next(mode);
  }

  ViewParentListForStudent(parentInfo: ViewParentInfoModel) {
    parentInfo = this.defaultValuesService.getAllMandatoryVariable(parentInfo);
    let apiurl = this.apiUrl + parentInfo._tenantName + "/ParentInfo/ViewParentListForStudent";
    return this.http.post<ViewParentInfoModel>(apiurl, parentInfo);
  }
  viewParentInfo(parentInfo: AddParentInfoModel) {
    parentInfo = this.defaultValuesService.getAllMandatoryVariable(parentInfo);
    // parentInfo.parentAssociationship.schoolId = this.defaultValuesService.getSchoolID();
    // parentInfo.parentAssociationship.tenantId = this.defaultValuesService.getTenantID();
    parentInfo.parentInfo.schoolId = this.defaultValuesService.getSchoolID();
    parentInfo.parentInfo.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + parentInfo._tenantName + "/ParentInfo/viewParentInfo";
    return this.http.post<AddParentInfoModel>(apiurl, parentInfo);
  }

  updateParentInfo(parentInfo: AddParentInfoModel) {
    parentInfo = this.defaultValuesService.getAllMandatoryVariable(parentInfo);
    parentInfo.parentInfo.schoolId = this.defaultValuesService.getSchoolID();
    parentInfo.parentInfo.tenantId = this.defaultValuesService.getTenantID();
    parentInfo.parentInfo.parentPhoto = this.parentImage;
    parentInfo.parentInfo.parentAddress[0].tenantId = this.defaultValuesService.getTenantID();
    parentInfo.parentInfo.parentAddress[0].schoolId = this.defaultValuesService.getSchoolID();
    let apiurl = this.apiUrl + parentInfo._tenantName + "/ParentInfo/updateParentInfo";
    return this.http.put<AddParentInfoModel>(apiurl, parentInfo);
  }
  getAllParentInfo(Obj: GetAllParentModel) {
    Obj = this.defaultValuesService.getAllMandatoryVariable(Obj);
    let apiurl = this.apiUrl + Obj._tenantName + "/ParentInfo/getAllParentInfo";
    return this.http.post<GetAllParentModel>(apiurl, Obj);
  }
  addParentForStudent(obj: AddParentInfoModel) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.parentInfo.schoolId = this.defaultValuesService.getSchoolID();
    obj.parentInfo.tenantId = this.defaultValuesService.getTenantID();
    obj.passwordHash = this.cryptoService.encrypt(obj.passwordHash);
    obj.parentInfo.parentPhoto = this.parentImage;
    let apiurl = this.apiUrl + obj._tenantName + "/ParentInfo/addParentForStudent";
    return this.http.post<AddParentInfoModel>(apiurl, obj);
  }
  deleteParentInfo(obj: AddParentInfoModel) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.parentInfo.schoolId = this.defaultValuesService.getSchoolID();
    obj.parentInfo.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + obj._tenantName + "/ParentInfo/deleteParentInfo";
    return this.http.post<AddParentInfoModel>(apiurl, obj);
  }
  searchParentInfoForStudent(obj: ParentInfoList) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName + "/ParentInfo/searchParentInfoForStudent";
    return this.http.post<ParentInfoList>(apiurl, obj);
  }

  viewParentListForStudent(obj: GetAllParentInfoModel) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName + "/ParentInfo/viewParentListForStudent";
    return this.http.post<GetAllParentInfoModel>(apiurl, obj);
  }

  removeAssociatedParent(obj: RemoveAssociateParent) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.parentInfo.schoolId = this.defaultValuesService.getSchoolID();
    obj.parentInfo.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + obj._tenantName + "/ParentInfo/removeAssociatedParent";
    return this.http.post<RemoveAssociateParent>(apiurl, obj);
  }

  addUpdateParentPhoto(obj: AddParentInfoModel) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.parentInfo.schoolId = this.defaultValuesService.getSchoolID();
    obj.parentInfo.tenantId = this.defaultValuesService.getTenantID();
    obj.parentInfo.parentId = this.getParentId();
    obj.parentInfo.parentPhoto = this.parentImage;
    let apiurl = this.apiUrl + obj._tenantName + "/ParentInfo/addUpdateParentPhoto";
    return this.http.put<AddParentInfoModel>(apiurl, obj);
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
