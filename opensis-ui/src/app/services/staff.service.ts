import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Subject } from 'rxjs';
import { CryptoService } from './Crypto.service';
import { CheckStaffInternalIdViewModel, GetAllStaffModel, StaffAddModel,StaffCertificateModel,StaffCertificateListModel,StaffSchoolInfoModel } from '../models/staff.model';
import { DefaultValuesService } from '../common/default-values.service';
@Injectable({
  providedIn: 'root'
})
export class StaffService {
  apiUrl: string = environment.apiURL;
  private currentYear = new BehaviorSubject(false);
  currentY = this.currentYear.asObservable();
  userName = sessionStorage.getItem('user');
  constructor(private http: HttpClient,
    private defaultValuesService: DefaultValuesService,
    private cryptoService: CryptoService) { }

  private staffImage;
  setStaffImage(imageInBase64) {
    this.staffImage = imageInBase64;
  }

  private staffId: number;
  setStaffId(id: number) {
    this.staffId = id
  }
  getStaffId() {
    return this.staffId;
  }

  private staffMultiselectValue: any;
  setStaffMultiselectValue(value: any) {
    this.staffMultiselectValue = value;
  }
  getStaffMultiselectValue() {
    return this.staffMultiselectValue;
  }


  private category = new Subject;
  categoryToSend = this.category.asObservable();
  changeCategory(category: number) {
    this.category.next(category);
  }

  private staffDetails;
  setStaffDetails(data) {
    this.staffDetails = data;
  }
  getStaffDetails() {
    return this.staffDetails;
  }
  checkStaffInternalId(obj: CheckStaffInternalIdViewModel) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName + "/Staff/checkStaffInternalId";
    return this.http.post<CheckStaffInternalIdViewModel>(apiurl, obj)
  }

     // Update Mode in Staff
 public pageMode = new Subject;
 modeToUpdate=this.pageMode.asObservable();

 changePageMode(mode:number){
     this.pageMode.next(mode);
 }

  // to Update staff details in General for first time.
  private staffDetailsForGeneralInfo = new Subject;
  getStaffDetailsForGeneral = this.staffDetailsForGeneralInfo.asObservable();
  sendDetails(staffDetailsForGeneralInfo) {
    this.staffDetailsForGeneralInfo.next(staffDetailsForGeneralInfo);
  }

  // for cancel after staff photo added
  public cloneStaffImage
  setStaffCloneImage(image){
    this.cloneStaffImage = image;
  }
  getStaffCloneImage(){
    return this.cloneStaffImage;
  }


  addStaff(obj: StaffAddModel) {
    obj= this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.passwordHash = this.cryptoService.encrypt(obj.passwordHash);
    obj.staffMaster.staffPhoto=this.staffImage;
    obj.staffMaster.schoolId= this.defaultValuesService.getSchoolID();
    obj.staffMaster.tenantId= this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + obj._tenantName + "/Staff/addStaff";
    return this.http.post<StaffAddModel>(apiurl, obj)
  }

  updateStaff(obj: StaffAddModel) {
    obj.passwordHash = this.cryptoService.encrypt(obj.passwordHash);
    obj.staffMaster.staffPhoto=this.staffImage;
    obj.staffMaster.schoolId= this.defaultValuesService.getSchoolID();
    obj.staffMaster.tenantId= this.defaultValuesService.getTenantID();
    obj= this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName + "/Staff/updateStaff";
    return this.http.put<StaffAddModel>(apiurl, obj)
  }

  viewStaff(obj: StaffAddModel) {
    obj= this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.staffMaster.schoolId = this.defaultValuesService.getSchoolID();
    obj.staffMaster.tenantId = this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + obj._tenantName + "/Staff/viewStaff";
    return this.http.post<StaffAddModel>(apiurl, obj)
  }
  
  getAllStaffList(obj: GetAllStaffModel){
    obj= this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName + "/Staff/getAllStaffList";
    return this.http.post<GetAllStaffModel>(apiurl, obj)
  }

  // Staff Certificate Services 
  getAllStaffCertificateInfo(obj:StaffCertificateListModel){
    obj= this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName + "/Staff/getAllStaffCertificateInfo";
    return this.http.post<StaffCertificateListModel>(apiurl,obj);
  }

  addStaffCertificateInfo(obj:StaffCertificateModel){
    obj= this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.staffCertificateInfo.schoolId= this.defaultValuesService.getSchoolID();
    obj.staffCertificateInfo.tenantId= this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + obj._tenantName + "/Staff/addStaffCertificateInfo";
    return this.http.post<StaffCertificateModel>(apiurl,obj); 
  }
  deleteStaffCertificateInfo(obj:StaffCertificateModel){
    obj= this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.staffCertificateInfo.schoolId= this.defaultValuesService.getSchoolID();
    obj.staffCertificateInfo.tenantId= this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + obj._tenantName + "/Staff/deleteStaffCertificateInfo";
    return this.http.post<StaffCertificateModel>(apiurl,obj);
  }
  updateStaffCertificateInfo(obj:StaffCertificateModel){
    obj= this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.staffCertificateInfo.schoolId= this.defaultValuesService.getSchoolID();
    obj.staffCertificateInfo.tenantId= this.defaultValuesService.getTenantID();
    let apiurl = this.apiUrl + obj._tenantName + "/Staff/updateStaffCertificateInfo";
    return this.http.put<StaffCertificateModel>(apiurl,obj); 
  }

  addStaffSchoolInfo(obj:StaffSchoolInfoModel){
    obj= this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName + "/Staff/addStaffSchoolInfo";
    return this.http.post<StaffSchoolInfoModel>(apiurl, obj)
  }

  viewStaffSchoolInfo(obj:StaffSchoolInfoModel){
    obj= this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName + "/Staff/viewStaffSchoolInfo";
    return this.http.post<StaffSchoolInfoModel>(apiurl, obj)
  }

  updateStaffSchoolInfo(obj:StaffSchoolInfoModel){
    obj= this.defaultValuesService.getAllMandatoryVariable(obj);
    let apiurl = this.apiUrl + obj._tenantName + "/Staff/updateStaffSchoolInfo";
    return this.http.put<StaffSchoolInfoModel>(apiurl, obj)
  }

  addUpdateStaffPhoto(obj:StaffAddModel){
    obj= this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.staffMaster.staffId = this.getStaffId();
    obj.staffMaster.staffPhoto = this.staffImage;
    let apiurl = this.apiUrl + obj._tenantName + "/Staff/addUpdateStaffPhoto";
    return this.http.put<StaffAddModel>(apiurl, obj)
  }

}