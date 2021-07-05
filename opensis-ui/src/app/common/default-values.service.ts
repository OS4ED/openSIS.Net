import { Injectable, OnInit } from '@angular/core';
import { CommonField } from '../models/common-field.model';
import { TranslateService } from '@ngx-translate/core';
import { LoginService } from '../services/login.service';
import { Router } from '@angular/router';
import { CryptoService } from '../services/Crypto.service';

@Injectable({
  providedIn: 'root',
})
export class DefaultValuesService {
  commonModel: CommonField = new CommonField();
  TenantId: string = '';
  schoolID: number ;
  academicYear: number;
  markingPeriodStartDate: string;
  permissionListKeyName: string;
  constructor(
    public translateService: TranslateService,
    private cryptoService: CryptoService
    ) {
  }

  setDefaultTenant() {
    const url = window.location.href;

    let tenant = '';
    if (url.includes('localhost')) {
      sessionStorage.setItem('tenant', 'opensisv2');
      tenant = 'opensisv2';
    } else {
      let startIndex = url.indexOf('//');
      let endIndex = url.indexOf('.');
      let tenantName = url.substr(startIndex + 2, endIndex - (startIndex + 2));

        sessionStorage.setItem('tenant', tenantName);
        tenant = tenantName;
      
    }

    this.commonModel._tenantName = tenant;
  }

  getDefaultTenant() {
    if (
      this.commonModel._tenantName === '' ||
      this.commonModel._tenantName === null ||
      typeof this.commonModel._tenantName == 'undefined'
    ) {
      this.setDefaultTenant();
    }

    return this.commonModel._tenantName;
  }

  // setIP() {
  //   this.http.get<{ ip: string }>('https://jsonip.com').subscribe((data) => {
  //     this.commonModel.IpAddress = data.ip;
  //     sessionStorage.setItem('IpAddress', this.commonModel.IpAddress);
  //   });
  // }

  // getIP() {
  //   if (
  //     this.commonModel.IpAddress === '' ||
  //     this.commonModel.IpAddress === null ||
  //     typeof this.commonModel.IpAddress == 'undefined'
  //   ) {
  //     this.setIP();
  //   }

  //   return sessionStorage.getItem('IpAddress');
  // }

  // setHostName() {
  //   this.commonModel.HostName = '';
  //   sessionStorage.setItem('HostName', this.commonModel.HostName);
  // }

  // getHostName() {
  //   if (
  //     this.commonModel.HostName === '' ||
  //     this.commonModel.HostName === null ||
  //     typeof this.commonModel.HostName == 'undefined'
  //   ) {
  //     this.setHostName();
  //   }

  //   return this.commonModel.HostName;
  // }

  getUserName() {
    let user = sessionStorage.getItem('user');
    this.commonModel._userName = user ? user : "";
    return this.commonModel._userName;
  }
  getEmailId(){
    let email =sessionStorage.getItem('email');
    return email;
  }
  setToken(token: string) {
    return new Promise((resolve, reject)=>{
      sessionStorage.setItem('token', token);
      resolve('');
    })
  }

  getToken() {
    this.commonModel._token = sessionStorage.getItem('token');
    return this.commonModel._token;
  }

  setTenantID() {
    if (this.TenantId?.trim().length > 0) {
      this.TenantId = sessionStorage.getItem('tenantId');
    }
  }
  getTenantID() {
    return sessionStorage.getItem('tenantId');
  }

  setSchoolID(schoolId?: string) {

    if (schoolId) {
      sessionStorage.setItem("selectedSchoolId", schoolId);
      this.schoolID = +sessionStorage.getItem("selectedSchoolId");
      return;
    }

    if (this.schoolID === null || typeof (schoolId) === "undefined") {
      this.schoolID = +sessionStorage.getItem("selectedSchoolId");
    }
  }

  setAcademicYear(academicYear?: string) {

    if (academicYear) {
      sessionStorage.setItem("academicyear", academicYear);
      this.academicYear = +sessionStorage.getItem("academicyear");
      return;
    }

    if (this.academicYear === null || typeof (academicYear) === "undefined") {
      this.academicYear = +sessionStorage.getItem("academicyear");
    }
  }

  getAcademicYear() {
    this.setAcademicYear();
    return this.academicYear;
  }

  getSchoolID() {
    this.setSchoolID();
    return this.schoolID;
  }

  // setFinYear(finYear?: string) {
  //   this.commonModel.finYear = sessionStorage.getItem('FinancialYear');
  // }

  // getFinYear() {
  //   this.commonModel.finYear = sessionStorage.getItem('FinancialYear');
  //   return this.commonModel.finYear;
  // }

  getAllMandatoryVariable(obj: any) {
    
    obj._tenantName = this.getDefaultTenant();
    obj._userName = this.getUserName();
    obj._token = this.getToken();
    obj.tenantId = this.getTenantID();
    obj.schoolId = this.getSchoolID();
    return obj;
  }

  getTenent() {
    return sessionStorage.getItem('tenant');
  }

  getuserMembershipID() {
    return sessionStorage.getItem('userMembershipID');
  }

  getuserMembershipName() {
    return sessionStorage.getItem('membershipName');
  }

  setAll(token: string) {
    this.setDefaultTenant();
    
   
    this.setToken(token);
    this.setTenantID();
   
   
  }

  translateKey(key) {
    let trnaslateKey;
   this.translateService.get(key).subscribe((res: string) => {
       trnaslateKey = res;
    });
    return trnaslateKey;
  }

  setPageSize(data: number) {
    sessionStorage.setItem('pageSize', JSON.stringify(data));
  }

  getPageSize(): number {
   return JSON.parse(sessionStorage.getItem('pageSize'));
  }

  setMarkingPeriodStartDate(mpStartDate?: string) {
      sessionStorage.setItem("markingPeriodStartDate", mpStartDate);
      this.markingPeriodStartDate = mpStartDate;
  }

  getMarkingPeriodStartDate(){
    return this.markingPeriodStartDate;
  }

  createPermissionKeyName(){
    let membershipId=this.getuserMembershipID();
    this.permissionListKeyName=`permissions${membershipId}`;
    return membershipId;
  }

  setPermissionList(permissionList){
    let membershipId = this.createPermissionKeyName();
    localStorage.setItem(`permissions${membershipId}`,this.cryptoService.dataEncrypt(JSON.stringify(permissionList)))
  }

  getPermissionList(){
    this.createPermissionKeyName();
    if(localStorage.getItem(this.permissionListKeyName)){
      let permissionList = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem(this.permissionListKeyName)));
      return permissionList;
    }
    return null;
  }

  


}
 