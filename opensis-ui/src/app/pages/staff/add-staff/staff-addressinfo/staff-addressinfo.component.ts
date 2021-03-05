import { Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { fadeInRight400ms } from '../../../../../@vex/animations/fade-in-right.animation';
import { SchoolCreate } from '../../../../enums/school-create.enum';
import { CountryModel } from '../../../../models/countryModel';
import { StaffAddModel } from '../../../../models/staffModel';
import { LanguageModel } from '../../../../models/languageModel';
import icEdit from '@iconify/icons-ic/edit';
import { TranslateService } from '@ngx-translate/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { StaffService } from '../../../../services/staff.service';
import { CommonService } from '../../../../services/common.service';
import { LoginService } from '../../../../services/login.service';
import { SharedFunction } from '../../../../pages/shared/shared-function';
import { StaffRelation } from '../../../../enums/staff-relation.enum';
import { ImageCropperService } from '../../../../services/image-cropper.service';
import { LovList } from '../../../../models/lovModel';
import { MiscModel } from '../../../../models/misc-data-student.model';
import { ModuleIdentifier } from '../../../../enums/module-identifier.enum';
import { CommonLOV } from '../../../shared-module/lov/common-lov';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
@Component({
  selector: 'vex-staff-addressinfo',
  templateUrl: './staff-addressinfo.component.html',
  styleUrls: ['./staff-addressinfo.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms,
    fadeInRight400ms
  ]
})
export class StaffAddressinfoComponent implements OnInit, OnDestroy {
  staffCreate = SchoolCreate;
  moduleIdentifier=ModuleIdentifier;
  @Input() staffDetailsForViewAndEdit;
  @Input() categoryId;
  @ViewChild('f') currentForm: NgForm;
  @Input() staffCreateMode: SchoolCreate;
  nameOfMiscValuesForView:MiscModel = new MiscModel();
  countryModel: CountryModel = new CountryModel();
  staffAddModel: StaffAddModel = new StaffAddModel();
  languages: LanguageModel = new LanguageModel();
  mailingAddressSameToHome: boolean = false;
  countryListArr = [];
  relationshipList = [];
  lovListViewModel: LovList = new LovList();
  module = 'Staff';
  data;
  languageList;
  checkBoxChecked = false;
  icEdit = icEdit;
  actionButton="submit"
  cloneStaffAddModel;
  destroySubject$: Subject<void> = new Subject();
  constructor(public translateService: TranslateService,
    private snackbar: MatSnackBar,
    private staffService: StaffService,
    private commonService: CommonService,
    private imageCropperService: ImageCropperService,
    private commonLOV:CommonLOV) {
    translateService.use('en');
  }

  ngOnInit(): void {
    if (this.staffCreateMode == this.staffCreate.VIEW) {
      this.data = this.staffDetailsForViewAndEdit?.staffMaster;
      this.staffAddModel = this.staffDetailsForViewAndEdit;
      this.cloneStaffAddModel = JSON.stringify(this.staffAddModel);
      this.staffService.changePageMode(this.staffCreateMode);
      if (this.data.mailingAddressSameToHome) {
        this.mailingAddressSameToHome = true;
      } else {
        this.mailingAddressSameToHome = false;
      }
      this.getAllCountry();
    } else {
      this.staffService.changePageMode(this.staffCreateMode);
      this.staffAddModel = this.staffService.getStaffDetails();
      this.cloneStaffAddModel = JSON.stringify(this.staffAddModel);
    }
    this.callLOVs();
    this.getAllCountry();
  }

  editAddressContactInfo() {
    this.staffCreateMode = this.staffCreate.EDIT;
    this.staffService.changePageMode(this.staffCreateMode);
    this.actionButton="update";
    this.staffAddModel.staffMaster.mailingAddressCountry =+this.staffAddModel.staffMaster.mailingAddressCountry ;
    this.staffAddModel.staffMaster.homeAddressCountry = +this.staffAddModel.staffMaster.homeAddressCountry;

  }

  cancelEdit() {
    if(this.staffAddModel!==JSON.parse(this.cloneStaffAddModel)){
      this.staffAddModel=JSON.parse(this.cloneStaffAddModel);
      this.staffDetailsForViewAndEdit=JSON.parse(this.cloneStaffAddModel);
      this.staffService.sendDetails(JSON.parse(this.cloneStaffAddModel));
    }
    this.staffCreateMode = this.staffCreate.VIEW;
    this.staffService.changePageMode(this.staffCreateMode);
    this.imageCropperService.cancelImage("staff");

  }

  copyHomeAddress(check) {
    if (this.staffAddModel.staffMaster.mailingAddressSameToHome === false || this.staffAddModel.staffMaster.mailingAddressSameToHome === null) {
      if (this.staffAddModel.staffMaster.homeAddressLineOne !== undefined && this.staffAddModel.staffMaster.homeAddressCity !== undefined &&
        this.staffAddModel.staffMaster.homeAddressState !== undefined && this.staffAddModel.staffMaster.homeAddressZip !== undefined) {
        this.staffAddModel.staffMaster.mailingAddressLineOne = this.staffAddModel.staffMaster.homeAddressLineOne;
        this.staffAddModel.staffMaster.mailingAddressLineTwo = this.staffAddModel.staffMaster.homeAddressLineTwo;
        this.staffAddModel.staffMaster.mailingAddressCity = this.staffAddModel.staffMaster.homeAddressCity;
        this.staffAddModel.staffMaster.mailingAddressState = this.staffAddModel.staffMaster.homeAddressState;
        this.staffAddModel.staffMaster.mailingAddressZip = this.staffAddModel.staffMaster.homeAddressZip;
        this.staffAddModel.staffMaster.mailingAddressCountry = this.staffAddModel.staffMaster.homeAddressCountry;
      } else {
        this.checkBoxChecked = check ? true : false;
        this.snackbar.open('Please Provide All Mandatory Fields First', '', {
          duration: 10000
        });
      }

    } else {
      this.staffAddModel.staffMaster.mailingAddressLineOne = "";
      this.staffAddModel.staffMaster.mailingAddressLineTwo = "";
      this.staffAddModel.staffMaster.mailingAddressCity = "";
      this.staffAddModel.staffMaster.mailingAddressState = "";
      this.staffAddModel.staffMaster.mailingAddressZip = "";
      this.staffAddModel.staffMaster.mailingAddressCountry = null;
    }
  }

  callLOVs(){
    this.commonLOV.getLovByName('Relationship').pipe(takeUntil(this.destroySubject$)).subscribe((res)=>{
      this.relationshipList=res;  
    });
  }
  
  getAllCountry() {
    this.commonService.GetAllCountry(this.countryModel).subscribe(data => {
      if (typeof (data) == 'undefined') {
        this.countryListArr = [];
      }
      else {
        if (data._failure) {
          this.countryListArr = [];
        } else {
          this.countryListArr=data.tableCountry?.sort((a, b) => {return a.name < b.name ? -1 : 1;} )   
          if (this.staffCreateMode == this.staffCreate.VIEW) {
          this.findCountryNameById()
          }
        }
      }
    })
  }

  findCountryNameById(){
    this.countryListArr.map((val) => {
      var countryInNumber = +this.data.homeAddressCountry;
      var mailingAddressCountry = +this.data.mailingAddressCountry;
      if (val.id === countryInNumber) {
        this.nameOfMiscValuesForView.countryName = val.name;
      }
      if (val.id === mailingAddressCountry) {
        this.nameOfMiscValuesForView.mailingAddressCountry = val.name;
      }
    })
  }

  submitAddress() {
    if (this.staffAddModel.fieldsCategoryList !== null) {
      this.staffAddModel.selectedCategoryId = this.staffAddModel.fieldsCategoryList[this.categoryId].categoryId;
      
      for (let staffCustomField of this.staffAddModel.fieldsCategoryList[this.categoryId].customFields) {
        if (staffCustomField.type === "Multiple SelectBox" && this.staffService.getStaffMultiselectValue() !== undefined) {
          staffCustomField.customFieldsValue[0].customFieldValue = this.staffService.getStaffMultiselectValue().toString().replaceAll(",", "|");
        }
      }
    }
    this.staffAddModel._token = sessionStorage.getItem("token");
    this.staffAddModel._tenantName = sessionStorage.getItem("tenant");
    this.staffService.updateStaff(this.staffAddModel).subscribe(data => {
      if (typeof (data) == 'undefined') {
        this.snackbar.open('Staff Updation failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (data._failure) {
          this.snackbar.open('Staff Updation failed. ' + data._message, '', {
            duration: 10000
          });
        } else {
          this.snackbar.open('Staff Update Successful.', '', {
            duration: 10000
          });
          this.staffService.setStaffCloneImage(data.staffMaster.staffPhoto);
          data.staffMaster.staffPhoto=null;
          this.data = data.staffMaster;
          this.staffDetailsForViewAndEdit=data;
          this.cloneStaffAddModel=JSON.stringify(this.staffDetailsForViewAndEdit);
          this.staffCreateMode = this.staffCreate.VIEW;
          this.findCountryNameById();
          this.staffService.changePageMode(this.staffCreateMode);
          
        }
      }
    })
  }

  ngOnDestroy() {
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }



}
