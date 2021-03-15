import { Component, OnInit ,Inject,ViewChild,Input, OnDestroy} from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, NgForm, Validators } from '@angular/forms';
import icClose from '@iconify/icons-ic/twotone-close';
import icBack from '@iconify/icons-ic/baseline-arrow-back';
import { fadeInUp400ms } from '../../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../../@vex/animations/stagger.animation';
import { TranslateService } from '@ngx-translate/core';
import { ParentInfoService } from '../../../../../services/parent-info.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import {MAT_DIALOG_DATA} from '@angular/material/dialog';
import { AddParentInfoModel,ParentInfoList,AssociateStudent } from '../../../../../models/parentInfoModel';
import { salutation,suffix ,relationShip,userProfile,Custody} from '../../../../../enums/studentAdd.enum';
import { CountryModel } from '../../../../../models/countryModel';
import { CommonService } from '../../../../../services/common.service';
import { SharedFunction } from '../../../../shared/shared-function';
import { LovList } from '../../../../../models/lovModel';
import { CommonLOV } from '../../../../shared-module/lov/common-lov';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';

@Component({
  selector: 'vex-edit-contact',
  templateUrl: './edit-contact.component.html',
  styleUrls: ['./edit-contact.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]

})
export class EditContactComponent implements OnInit,OnDestroy {
  @ViewChild('f') currentForm: NgForm;
  f: NgForm;
  search: NgForm;
  associateStudents: NgForm;
  associateMultipleStudents:NgForm;
  icClose = icClose;
  icBack = icBack; 
  addParentInfoModel: AddParentInfoModel = new AddParentInfoModel(); 
  parentInfoList:ParentInfoList=new ParentInfoList();
  associateStudent:AssociateStudent=new AssociateStudent();
  lovListViewModel: LovList = new LovList();
  contactModalTitle="addContact";
  contactModalActionTitle="submit";
  isEdit=false;
  userProfileEnum = Object.keys(userProfile);
  custodyEnum = Custody;
  mode;
  viewData:any;
  countryModel: CountryModel = new CountryModel();
  countryListArr=[]; 
  countryName="-";
  mailingAddressCountry="-";
  val;
  isPortalUser=false;
  sameAsStudentAddress:boolean=true;
  disableAddressFlag;
  disableNewAddressFlag;
  singleParentInfo;
  multipleParentInfo=[];
  suffixList = [];
  salutationList = [];
  relationshipList = [];
  editMode=false;
  isCustodyCheck=false;
  disablePassword=false;
  studentDetailsForViewAndEditDataDetails;
  parentAddress=[];
  destroySubject$: Subject<void> = new Subject();

  constructor(
      private dialogRef: MatDialogRef<EditContactComponent>,
      private fb: FormBuilder, 
      public translateService:TranslateService,
      private parentInfoService:ParentInfoService,
      private snackbar: MatSnackBar,
      private router:Router,
      private commonLOV:CommonLOV,
      private commonService:CommonService,
      private commonFunction:SharedFunction,
      @Inject(MAT_DIALOG_DATA) public data
    ) 
    { }

  ngOnInit(): void {  
   
    this.studentDetailsForViewAndEditDataDetails=this.data.studentDetailsForViewAndEditData; 
    this.getAllCountry(); 
    this.callLOVs();
    
      if(this.data.mode === "view"){       
       this.mode = "view";
       this.viewData = this.data.parentInfo;  
       
       if(this.viewData.middlename === null){
        this.viewData.middlename = "";
       }
       if(this.viewData.salutation === null){
        this.viewData.salutation = "";
       }
       
      }else{
        if(this.data.mode === "add"){
        this.disableAddressFlag =true;
        this.disableNewAddressFlag =true;
        this.val="Yes";     
        this.addParentInfoModel.parentAssociationship.contactType = this.data.contactType;    
        this.addParentInfoModel.parentInfo.parentAddress[0].studentAddressSame = true;   
        }else{ 
             
          this.addParentInfoModel.parentInfo.parentAddress[0].addressLineOne = this.data.parentInfo.parentAddress.addressLineOne;  
          this.addParentInfoModel.parentInfo.parentAddress[0].addressLineTwo = this.data.parentInfo.parentAddress.addressLineTwo;
          this.addParentInfoModel.parentInfo.parentAddress[0].country = +this.data.parentInfo.parentAddress.country;
          this.addParentInfoModel.parentInfo.parentAddress[0].state = this.data.parentInfo.parentAddress.state;
          this.addParentInfoModel.parentInfo.parentAddress[0].city = this.data.parentInfo.parentAddress.city;
          this.addParentInfoModel.parentInfo.parentAddress[0].zip = this.data.parentInfo.parentAddress.zip;  
          this.addParentInfoModel.parentInfo.salutation = this.data.parentInfo.salutation; 
          this.addParentInfoModel.parentInfo.firstname = this.data.parentInfo.firstname; 
          this.addParentInfoModel.parentInfo.middlename = this.data.parentInfo.middlename; 
          this.addParentInfoModel.parentInfo.lastname = this.data.parentInfo.lastname; 
          this.addParentInfoModel.parentInfo.suffix = this.data.parentInfo.suffix; 
          this.addParentInfoModel.parentAssociationship.relationship = this.data.parentInfo.relationship; 
          this.addParentInfoModel.parentAssociationship.isCustodian = this.data.parentInfo.isCustodian; 
          this.addParentInfoModel.parentInfo.mobile = this.data.parentInfo.mobile; 
          this.addParentInfoModel.parentInfo.workPhone = this.data.parentInfo.workPhone; 
          this.addParentInfoModel.parentInfo.homePhone = this.data.parentInfo.homePhone;
          this.addParentInfoModel.parentInfo.personalEmail = this.data.parentInfo.personalEmail; 
          this.addParentInfoModel.parentInfo.workEmail = this.data.parentInfo.workEmail; 
          this.addParentInfoModel.parentInfo.parentAddress[0].studentAddressSame = this.data.parentInfo.parentAddress.studentAddressSame;
          this.addParentInfoModel.parentInfo.isPortalUser = this.data.parentInfo.isPortalUser; 
          this.addParentInfoModel.parentInfo.loginEmail = this.data.parentInfo.loginEmail; 
          this.addParentInfoModel.passwordHash = this.data.passwordHash;

          this.editMode=true;
          this.disablePassword = true;    
             
          if(this.data.parentInfo.parentAddress.studentAddressSame === true){
            this.val="Yes";
            this.sameAsStudentAddress=true;
            
          }else{
            this.val="No";
            this.sameAsStudentAddress=false;
          }
          if(this.addParentInfoModel.parentInfo.isPortalUser === true){
            this.isPortalUser = true;
            this.addParentInfoModel.parentInfo.isPortalUser = true; 
          }else{
            this.isPortalUser = false;
            this.addParentInfoModel.parentInfo.isPortalUser = false; 
          }   
          if(this.addParentInfoModel.parentAssociationship.isCustodian === false){
           
            this.disableAddressFlag = true;
          }   
          this.addParentInfoModel.parentAssociationship.contactType = this.data.parentInfo.contactType;       
        }
        this.mode = "add";     
      }   
      
      this.addParentInfoModel.parentInfo.userProfile = "Parent";
  }
  disableAddress(event){
    if(event.value === true){
      this.disableAddressFlag = false;
      this.disableNewAddressFlag=false;
      //console.log(this.addParentInfoModel.parentAssociationship.isCustodian)
    }else{
      this.disableAddressFlag = true;
      this.disableNewAddressFlag = false;
      this.val="No";     
      this.sameAsStudentAddress = false;
      this.addParentInfoModel.parentInfo.parentAddress[0].studentAddressSame = false;
      //console.log(this.addParentInfoModel.parentAssociationship.isCustodian)
    }
  }
  associateStudentToParent(){    
    let isCustodian=this.associateStudent.isCustodian;
    let contactRelationship=this.associateStudent.contactRelationship;
    if(contactRelationship === undefined){
      contactRelationship = "";
     }   
     if(isCustodian === undefined){
      isCustodian = false
     }
  
    this.addParentInfoModel.parentAssociationship.isCustodian = isCustodian;
    this.addParentInfoModel.parentAssociationship.relationship = contactRelationship; 
    this.addParentInfoModel.parentAssociationship.tenantId =  sessionStorage.getItem("tenantId");
    this.addParentInfoModel.parentAssociationship.schoolId = +sessionStorage.getItem("selectedSchoolId");  
    this.addParentInfoModel.parentAssociationship.studentId = this.data.studentDetailsForViewAndEditData.studentMaster.studentId;  
    this.addParentInfoModel.parentAssociationship.parentId = this.singleParentInfo.parentId;
    this.addParentInfoModel.parentAssociationship.contactType=this.data.contactType;
    delete this.singleParentInfo.getStudentForView;  
    delete this.singleParentInfo.isCustodian;
    delete this.singleParentInfo.relationship;
    delete this.singleParentInfo.studentId; 
    this.addParentInfoModel.parentInfo= this.singleParentInfo;   
    
    this.submit();
  }

  callLOVs(){
    this.commonLOV.getLovByName('Relationship').pipe(takeUntil(this.destroySubject$)).subscribe((res)=>{
      this.relationshipList=res;  
    });
    this.commonLOV.getLovByName('Salutation').pipe(takeUntil(this.destroySubject$)).subscribe((res)=>{
      this.salutationList=res;  
    });
    this.commonLOV.getLovByName('Suffix').pipe(takeUntil(this.destroySubject$)).subscribe((res)=>{
      this.suffixList=res;  
    });
  }

  associateMultipleStudentsToParent(){
   var isCustodian=this.associateStudent.isCustodian;
   var contactRelationship=this.associateStudent.contactRelationship;  
   if(contactRelationship === undefined){
    contactRelationship = "";
   }   
   if(isCustodian === undefined){
    isCustodian = false
   }
   let obj = {
    'isCustodian':isCustodian,
    'relationship':contactRelationship,
    "tenantId": sessionStorage.getItem("tenantId"),
    "schoolId": +sessionStorage.getItem("selectedSchoolId"),
    "studentId": this.data.studentDetailsForViewAndEditData.studentMaster.studentId,   
    "parentId":0,
    "associationship":false,
    "lastUpdated":"",
    "contactType":this.data.contactType,
    "updatedBy":sessionStorage.getItem("email")
    }   
   return obj;
  }
  getIndexOfParentInfo(data){   
    let obj = this.associateMultipleStudentsToParent();   
    if(obj.relationship === ""){
      this.snackbar.open('Please provide Relationship','', {
        duration: 10000
      });
    }else{
      obj.parentId = data.parentId;
      this.addParentInfoModel.parentAssociationship = obj; 
      delete data.studentId;
      delete data.relationShip;
      delete data.isCustodian;
      delete data.getStudentForView;
      this.addParentInfoModel.parentInfo = data;     
      this.submit();
    }
    
  }
  copyEmailAddress(emailType){
    if(emailType === "personal"){
      if(this.currentForm.form.controls.personalEmail.value !== ""){
        this.addParentInfoModel.parentInfo.loginEmail =this.currentForm.form.controls.personalEmail.value;
      }      
    }else{
      if(this.currentForm.form.controls.workEmail.value !== ""){
        this.addParentInfoModel.parentInfo.loginEmail =this.currentForm.form.controls.workEmail.value;
      }
    }
  }
  
  getAllCountry(){  
    this.commonService.GetAllCountry(this.countryModel).subscribe(data => {
      if (typeof (data) == 'undefined') {
        this.countryListArr = [];
      }
      else {
        if (data._failure) {
          this.countryListArr = [];
        } else {
          this.countryListArr=data.tableCountry?.sort((a, b) => {return a.name < b.name ? -1 : 1;} ) 
          if(this.data.mode === "edit"){
            this.countryListArr.map((val) => {            
              if(this.data.parentInfo.parentAddress.country == val.name){
                this.addParentInfoModel.parentInfo.parentAddress[0].country = val.id;
                
              }                
            })  
          } 
          
          if(this.mode === "view"){
            this.countryListArr.map((val) => {
              var countryInNumber = +this.viewData.parentAddress.country;            
                if(val.id === countryInNumber){
                  this.viewData.parentAddress.country= val.name;
                }               
              })     
          } 
         
             
        }
      }

    })
  }
  closeDialog(){
    this.dialogRef.close(false);
  }
  searchExistingContact(){
    this.mode="search";
  }  
  backToAdd(){
    this.mode="add";
  }
  backToSearch(){
    this.mode="search";
  }
  radioChange(event){
    if(event.value === "Yes"){
      this.sameAsStudentAddress = true;
      this.addParentInfoModel.parentInfo.parentAddress[0].studentAddressSame = true;    
    }else{
      this.sameAsStudentAddress = false;
      this.addParentInfoModel.parentInfo.parentAddress[0].studentAddressSame = false; 
      this.addParentInfoModel.parentInfo.parentAddress[0].addressLineOne =null;  
      this.addParentInfoModel.parentInfo.parentAddress[0].addressLineTwo = null;
      this.addParentInfoModel.parentInfo.parentAddress[0].country = null;
      this.addParentInfoModel.parentInfo.parentAddress[0].state = null;
      this.addParentInfoModel.parentInfo.parentAddress[0].city = null;
      this.addParentInfoModel.parentInfo.parentAddress[0].zip = null;    
    }    
  }
  custodyCheck(event){
    if(event.checked === true){
      this.isCustodyCheck = true;
     
    }else{
      this.isCustodyCheck = true;
     
    }
  }
  portalUserCheck(event){
    if(event.checked === true){
      this.isPortalUser = true;
      this.addParentInfoModel.parentInfo.isPortalUser = true; 
    }else{
      this.isPortalUser = false;
      this.addParentInfoModel.parentInfo.isPortalUser = false; 
    }
  }
  submit() {
    if(this.mode!=="singleResult" && this.mode!=="multipleResult"){
      this.addParentInfoModel.parentAssociationship.studentId = this.studentDetailsForViewAndEditDataDetails.studentMaster.studentId;
      this.addParentInfoModel.parentInfo.parentAddress[0].studentId = this.studentDetailsForViewAndEditDataDetails.studentMaster.studentId;
    }    
    if(this.editMode === true){     
      this.addParentInfoModel.parentAssociationship.parentId = this.data.parentInfo.parentId;
      this.addParentInfoModel.parentInfo.parentAddress[0].parentId = this.data.parentInfo.parentId;
      this.addParentInfoModel.parentInfo.parentId = this.data.parentInfo.parentId;
      this.parentInfoService.updateParentInfo(this.addParentInfoModel).subscribe(data => {
        if (typeof (data) == 'undefined') 
        {
          this.snackbar.open('Parent Information Updation failed. ' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else 
        {
          if (data._failure) {
            this.snackbar.open( data._message, '', {
              duration: 10000
            });
          }
          else 
          {
            this.snackbar.open(data._message, '', {
            duration: 10000
            });
            this.dialogRef.close(true);            
          }
        }
      })     
    }else{  
    
      this.parentInfoService.addParentForStudent(this.addParentInfoModel).subscribe(data => {
        if (typeof (data) == 'undefined') 
        {
          this.snackbar.open('Parent Information Submission failed. ' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else 
        {
          if (data._failure) {
            this.snackbar.open(data._message, '', {
              duration: 10000
            });
          }
          else 
          {
            this.snackbar.open(data._message, '', {
            duration: 10000
            });
            
            this.dialogRef.close(true); 
            
                       
          }
        }
      })     
    }      
  }
    
    searchParent(){      
      if(this.parentInfoList.firstname === null && this.parentInfoList.lastname === null && this.parentInfoList.mobile === null &&
        this.parentInfoList.email === null && this.parentInfoList.streetAddress === null && this.parentInfoList.city === null &&
        this.parentInfoList.state === null  && this.parentInfoList.zip === null     
        ){
          this.snackbar.open('Please provide atleast one search field', '', {
            duration: 10000
            });
        }else{
          this.parentInfoList.studentId = this.data.studentDetailsForViewAndEditData.studentMaster.studentId;
          this.parentInfoService.searchParentInfoForStudent(this.parentInfoList).subscribe(data => {
            if (typeof (data) == 'undefined') 
            {
              this.snackbar.open('Search Parent Information failed. ' + sessionStorage.getItem("httpError"), '', {
                duration: 10000
              });
            }
            else 
            {
              if (data._failure) {
                this.snackbar.open( data._message, '', {
                  duration: 10000
                });
              }
              else 
              {       
                if(data.parentInfo.length > 1){
                  this.mode="multipleResult";
                  this.multipleParentInfo =data.parentInfo ;
                 
                }else if(data.parentInfo.length == 0){
                  this.snackbar.open('No Record Found', '', {
                    duration: 10000
                    });
                  this.mode="search";
                  this.singleParentInfo={};              
                } else{
                  this.mode="singleResult";                  
                  data.parentInfo.map((val,index) => {
                     this.singleParentInfo = val;     
                  }) 
                }         
              }
            }
          })     
        }
    }

    ngOnDestroy(){
      this.destroySubject$.next();
      this.destroySubject$.complete();
    }
}
