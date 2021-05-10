import { Component, OnInit, ChangeDetectorRef, ViewChild, Input, ComponentFactoryResolver, ElementRef, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { fadeInRight400ms } from '../../../../../@vex/animations/fade-in-right.animation';
import { TranslateService } from '@ngx-translate/core';
import icAdd from '@iconify/icons-ic/baseline-add';
import icClear from '@iconify/icons-ic/baseline-clear';
import icEdit from '@iconify/icons-ic/edit';
import icVisibility from '@iconify/icons-ic/twotone-visibility';
import icVisibilityOff from '@iconify/icons-ic/twotone-visibility-off';
import { SchoolCreate } from '../../../../enums/school-create.enum';
import { CheckStaffInternalIdViewModel, StaffAddModel } from '../../../../models/staff.model';
import { LanguageModel } from '../../../../models/language.model';
import { ethnicity, gender, maritalStatus, race, salutation, suffix } from '../../../../enums/studentAdd.enum';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CommonService } from '../../../../services/common.service';
import { LoginService } from '../../../../services/login.service';
import { SharedFunction } from '../../../../pages/shared/shared-function';
import { CountryModel } from '../../../../models/country.model';
import { StaffService } from '../../../../services/staff.service';
import { CheckUserEmailAddressViewModel } from '../../../../models/user.model';
import { debounceTime, distinctUntilChanged, takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { WashInfoEnum } from '../../../../enums/wash-info.enum';
import { ImageCropperService } from '../../../../services/image-cropper.service';
import { LovList } from '../../../../models/lov.model';
import * as cloneDeep from 'lodash/cloneDeep';
import { MiscModel } from '../../../../models/misc-data-student.model';
import { CommonLOV } from '../../../shared-module/lov/common-lov';
import { ModuleIdentifier } from '../../../../enums/module-identifier.enum';
import { RolePermissionListViewModel, RolePermissionViewModel } from '../../../../models/roll-based-access.model';
import { RollBasedAccessService } from '../../../../services/roll-based-access.service';
import { CryptoService } from '../../../../services/Crypto.service';
import { Router } from '@angular/router';
@Component({
  selector: 'vex-staff-generalinfo',
  templateUrl: './staff-generalinfo.component.html',
  styleUrls: ['./staff-generalinfo.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms,
    fadeInRight400ms
  ]
})
export class StaffGeneralinfoComponent implements OnInit {
  moduleIdentifier = ModuleIdentifier;
  staffCreate = SchoolCreate;
  @Input() staffDetailsForViewAndEdit;
  @Input() categoryId;
  @Output() dataAfterSavingGeneralInfo = new EventEmitter<any>();
  @ViewChild('f') currentForm: NgForm;
  @Input() staffCreateMode: SchoolCreate;
  nameOfMiscValuesForView:MiscModel=new MiscModel();
  countryModel: CountryModel = new CountryModel();
  staffAddModel: StaffAddModel = new StaffAddModel();
  languages: LanguageModel = new LanguageModel();
  lovListViewModel: LovList = new LovList()
  checkStaffInternalIdViewModel: CheckStaffInternalIdViewModel = new CheckStaffInternalIdViewModel();
  checkUserEmailAddressViewModel: CheckUserEmailAddressViewModel = new CheckUserEmailAddressViewModel();
  destroySubject$: Subject<void> = new Subject();
  languageList = [];
  ethnicityList = [];
  raceList = [];
  genderList = [];
  suffixList = [];
  salutationList = [];
  maritalStatusList = [];
  disablity = WashInfoEnum;
  staffPortalAccess: string;
  countryListArr = [];
  icAdd = icAdd;
  icEdit = icEdit;
  icClear = icClear;
  icVisibility = icVisibility;
  icVisibilityOff = icVisibilityOff;
  inputType = 'password';
  module = 'Staff';
  visible = false;
  staffInternalId = '';
  data: any;
  hidePasswordAccess: boolean = false;
  hideAccess: boolean = false;
  fieldDisabled: boolean = false;
  saveAndNext = 'saveAndNext';
  pageStatus: string;
  staffPortalId: string;
  isUser :boolean= false;
  isStaffInternalId :boolean= false;
  showDisabilityDescription: boolean = false;
  internalId:FormControl;
  loginEmail:FormControl;
  cloneStaffAddModel;
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();
  constructor(private fb: FormBuilder,
              private el: ElementRef,
              public translateService: TranslateService,
              private snackbar: MatSnackBar,
              private commonService: CommonService,
              private loginService: LoginService,
              private cryptoService: CryptoService,
              private commonFunction: SharedFunction,
              private staffService: StaffService,
              private router:Router,
              public rollBasedAccessService: RollBasedAccessService,
              private cd: ChangeDetectorRef,
              private imageCropperService: ImageCropperService,
              private commonLOV: CommonLOV) {
    translateService.use('en');

    this.staffService.getStaffDetailsForGeneral.pipe(takeUntil(this.destroySubject$)).subscribe((res: StaffAddModel) => {
      this.staffAddModel = res;
      this.data = this.staffAddModel?.staffMaster;
      this.cloneStaffAddModel=JSON.stringify(this.staffAddModel);
      this.staffInternalId = this.data.staffInternalId;
      this.staffPortalId = this.data.loginEmailAddress;
      if(this.staffAddModel.staffMaster?.staffId){
        this.accessPortal();
        this.GetAllLanguage();
        this.getAllCountry();
      }

    })

  }

  ngOnInit(): void {

    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 5);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 10);
    const permissionSubCategory = permissionCategory.permissionSubcategory.find( x => x.permissionSubcategoryId === 13);
    this.editPermission = permissionSubCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionSubCategory.rolePermission[0].canDelete;
    this.addPermission = permissionSubCategory.rolePermission[0].canAdd;

    this.internalId = new FormControl('', Validators.required);
    this.loginEmail = new FormControl('', Validators.required);
    if (this.staffCreateMode == this.staffCreate.ADD) {
      if(this.addPermission===false){
        this.router.navigate(['/'])
      }
      this.initializeDropdowns();
    } else if (this.staffCreateMode == this.staffCreate.VIEW) {
      this.staffService.changePageMode(this.staffCreateMode);
      this.accessPortal();
      this.getAllCountry();
      this.GetAllLanguage();
      this.staffAddModel = this.staffDetailsForViewAndEdit;
      this.data = this.staffDetailsForViewAndEdit?.staffMaster;
      this.cloneStaffAddModel=JSON.stringify(this.staffAddModel);
    } else if (this.staffCreateMode == this.staffCreate.EDIT && (this.staffDetailsForViewAndEdit != undefined || this.staffDetailsForViewAndEdit != null)) {
      this.staffAddModel = this.staffDetailsForViewAndEdit;
      this.cloneStaffAddModel=JSON.stringify(this.staffAddModel);
      this.initializeDropdowns();
      this.staffService.changePageMode(this.staffCreateMode);
      this.saveAndNext = 'update';
      if (this.staffAddModel.staffMaster.loginEmailAddress !== null) {
        this.hideAccess = true;
        this.fieldDisabled = true;

      }
    }
  }

  initializeDropdowns(){
     this.getAllCountry();
      this.GetAllLanguage();
      this.callLOVs();
  }

  ngAfterViewInit(){
    this.staffInternalId = this.data?.staffInternalId;
    if(this.staffAddModel.staffMaster.loginEmailAddress ==null){
      this.staffPortalId = this.staffAddModel.staffMaster.loginEmailAddress;
    }
    else{
      this.staffPortalId = this.data?.loginEmailAddress;
    }
    // For Checking Internal Id
    this.internalId.valueChanges.pipe(debounceTime(500),distinctUntilChanged()).subscribe((term)=>{
      if(term!=''){
        if (this.staffInternalId === term) {
          this.internalId.setErrors(null);
        }
        else {
          this.isStaffInternalId= true;
          this.checkStaffInternalIdViewModel.staffInternalId = term;
          this.staffService.checkStaffInternalId(this.checkStaffInternalIdViewModel).subscribe(data => {
            if (data.isValidInternalId) {
              this.internalId.setErrors(null);
              this.isStaffInternalId= false;
            }
            else {
              this.internalId.markAsTouched();
              this.internalId.setErrors({ 'nomatch': true });
              this.isStaffInternalId= false;
            }
          });
        }
      }else{
        this.internalId.markAsTouched();
        this.isStaffInternalId= false;
      }
    });

    // For Checking Login Email Id
    this.loginEmail.valueChanges
    .pipe(debounceTime(600), distinctUntilChanged())
    .subscribe(term => {
      if (term != '') {
        if (this.staffPortalId === term) {
          this.loginEmail.setErrors(null);
        }
        else {
          this.isUser= true;
          this.checkUserEmailAddressViewModel.emailAddress = term;
          this.loginService.checkUserLoginEmail(this.checkUserEmailAddressViewModel).subscribe(data => {
            if (data.isValidEmailAddress) {
              this.loginEmail.setErrors(null);
              this.isUser= false;
            }
            else {
              this.loginEmail.markAsTouched();
              this.loginEmail.setErrors({ 'nomatch': true });
              this.isUser= false;
            }
          });
        }
      } else {
        this.loginEmail.markAsTouched();
        this.isUser= false;
      }
    });
  }

  callLOVs(){
    this.commonLOV.getLovByName('Salutation').pipe(takeUntil(this.destroySubject$)).subscribe((res)=>{
      this.salutationList=res;  
    });
    this.commonLOV.getLovByName('Suffix').pipe(takeUntil(this.destroySubject$)).subscribe((res)=>{
      this.suffixList=res;  
    });
    this.commonLOV.getLovByName('Gender').pipe(takeUntil(this.destroySubject$)).subscribe((res)=>{
      this.genderList=res;  
    });
    this.commonLOV.getLovByName('Race').pipe(takeUntil(this.destroySubject$)).subscribe((res)=>{
      this.raceList=res;  
    });
    this.commonLOV.getLovByName('Ethnicity').pipe(takeUntil(this.destroySubject$)).subscribe((res)=>{
      this.ethnicityList=res;  
    });
    this.commonLOV.getLovByName('Marital Status').pipe(takeUntil(this.destroySubject$)).subscribe((res)=>{
      this.maritalStatusList=res;  
    });
  }

  getAllCountry() {
    if (!this.countryModel.isCountryAvailable){
      this.countryModel.isCountryAvailable = true;
    this.commonService.GetAllCountry(this.countryModel).pipe(takeUntil(this.destroySubject$)).subscribe(data => {
      if (typeof (data) == 'undefined') {
        this.countryListArr = [];
      }
      else {
        if (data._failure) {
          this.countryListArr = [];
        } else {
          this.countryListArr=data.tableCountry?.sort((a, b) => {return a.name < b.name ? -1 : 1;} )   
          if (this.staffCreateMode == this.staffCreate.VIEW) {
              this.findNationalityNameById();
          }
        }
      }
    })
  }
  }

  findNationalityNameById(){
    this.countryListArr.map((val) => {
      let countryInNumber = +this.data.countryOfBirth;
      let nationality = +this.data.nationality;
      if (val.id === countryInNumber) {
        this.nameOfMiscValuesForView.countryName = val.name;
      }
      if (val.id === nationality) {
        this.nameOfMiscValuesForView.nationality = val.name;
      }
    })
  }

  GetAllLanguage() {
    if (!this.languages.isLanguageAvailable){
      this.languages.isLanguageAvailable = true;
    this.loginService.getAllLanguage(this.languages).pipe(takeUntil(this.destroySubject$)).subscribe((res) => {
      if (typeof (res) == 'undefined') {
        this.languageList = [];
      }
      else {
        this.languageList=res.tableLanguage?.sort((a, b) => {return a.locale < b.locale ? -1 : 1;} )   
        if (this.staffCreateMode == this.staffCreate.VIEW) {
          this.findNameById();
        }
      }

    })
  }
  }

  findNameById(){
    this.languageList.map((val) => {
      let firstLanguageId = + this.data.firstLanguage;
      let secondLanguageId = + this.data.secondLanguage;
      let thirdLanguageId = + this.data.thirdLanguage;
      if (val.langId === firstLanguageId) {
        this.nameOfMiscValuesForView.firstLanguage = val.locale;
      }
      if (val.langId === secondLanguageId) {
        this.nameOfMiscValuesForView.secondLanguage = val.locale;
      }
      if (val.langId === thirdLanguageId) {
        this.nameOfMiscValuesForView.thirdLanguage = val.locale;
      }
    })
  }

  
  accessPortal() {
    if (this.staffPortalId !== null && this.staffPortalId !== undefined) {
      this.hideAccess = true;
      this.fieldDisabled = true;
      this.hidePasswordAccess = false;

    } else {
      this.hideAccess = false;
      this.fieldDisabled = false;
      this.hidePasswordAccess = false;

    }
  }
  generate() {
    this.inputType = 'text';
    this.visible = true;
    this.currentForm.controls.passwordHash.setValue(this.commonFunction.autoGeneratePassword());
  }

  isPortalAccess(event) {
    if (event.checked) {
      if (this.staffCreateMode == this.staffCreate.ADD) {
        this.hidePasswordAccess = true;
      }
      else {
        if (this.staffPortalId !== null && this.staffPortalId !== undefined) {
          this.hidePasswordAccess = false;
        }
        else {
          this.hidePasswordAccess = true;
        }
      }
      this.hideAccess = true;
      this.staffAddModel.staffMaster.portalAccess = true;
    }
    else {
      this.hideAccess = false;
      this.hidePasswordAccess = false;
      this.staffAddModel.staffMaster.portalAccess = false;
    }
  }



  editGeneralInfo() {
    this.staffCreateMode = this.staffCreate.EDIT
    this.staffService.changePageMode(this.staffCreateMode);
    this.saveAndNext = 'update';
    if (this.staffAddModel.staffMaster.loginEmailAddress !== null) {
      this.hideAccess = true;
      this.fieldDisabled = true;

    }
    if (this.staffAddModel.staffMaster.physicalDisability) {
      this.showDisabilityDescription = true;
    }
    this.callLOVs();
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
    this.data = this.staffAddModel.staffMaster;
    this.findNationalityNameById()
    this.GetAllLanguage();
  }


  checkDisability(event) {
    if (event.value == true) {
      this.showDisabilityDescription = true;
    }
    else {
      this.showDisabilityDescription = false;
    }
  }

  submitStaff() {
    this.currentForm.form.markAllAsTouched();
    if (this.currentForm.controls.passwordHash !== undefined) {
      this.staffAddModel.passwordHash = this.currentForm.controls.passwordHash.value;
    }

    if (this.currentForm.form.valid) {
      if (this.staffAddModel.fieldsCategoryList !== null) {
        this.staffAddModel.selectedCategoryId = this.staffAddModel.fieldsCategoryList[this.categoryId].categoryId;
        
        for (let staffCustomField of this.staffAddModel.fieldsCategoryList[this.categoryId].customFields) {
          if (staffCustomField.type === "Multiple SelectBox" && this.staffService.getStaffMultiselectValue() !== undefined) {
            staffCustomField.customFieldsValue[0].customFieldValue = this.staffService.getStaffMultiselectValue().toString().replaceAll(",", "|");
          }
        }
      }
      if (this.staffCreateMode == this.staffCreate.EDIT) {
        this.updateStaff();
      } else {
        this.addStaff();
      }
    }
  }

  addStaff() {
    if(this.internalId.invalid){
      this.invalidScroll();
      return
    }
    this.staffAddModel.staffMaster.dob = this.commonFunction.formatDateSaveWithoutTime(this.staffAddModel.staffMaster.dob);
    this.staffService.addStaff(this.staffAddModel).pipe(takeUntil(this.destroySubject$)).subscribe(data => {
      if (typeof (data) == 'undefined') {
        this.snackbar.open('Staff Save failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (data._failure) {
          this.snackbar.open( data._message, '', {
            duration: 10000
          });
        } else {
          this.snackbar.open( data._message, '', {
            duration: 10000
          });
          this.staffService.setStaffId(data.staffMaster.staffId);
          this.staffService.setStaffCloneImage(data.staffMaster.staffPhoto);
          this.staffService.changeCategory(13);
          this.staffService.setStaffDetails(data);
          this.dataAfterSavingGeneralInfo.emit(data);
          this.imageCropperService.enableUpload({module:this.moduleIdentifier.STAFF,upload:true,mode:this.staffCreate.EDIT});
        }
      }
    })
  }

  updateStaff() {
    if(this.internalId.invalid){
      this.invalidScroll();
      return
    }
    if (this.staffAddModel.fieldsCategoryList !== null) {
      this.staffAddModel.selectedCategoryId = this.staffAddModel.fieldsCategoryList[this.categoryId].categoryId;
    }
    this.staffAddModel.staffMaster.dob = this.commonFunction.formatDateSaveWithoutTime(this.staffAddModel.staffMaster.dob);
    this.staffService.updateStaff(this.staffAddModel).pipe(takeUntil(this.destroySubject$)).subscribe(data => {
      if (typeof (data) == 'undefined') {
        this.snackbar.open('Staff Update failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (data._failure) {
          this.snackbar.open(data._message, '', {
            duration: 10000
          });
        } else {
          this.snackbar.open( data._message, '', {
            duration: 10000
          });
          this.staffService.setStaffCloneImage(data.staffMaster.staffPhoto);
          data.staffMaster.staffPhoto=null;
          this.dataAfterSavingGeneralInfo.emit(data);
          this.staffAddModel.staffMaster = data.staffMaster;
          this.staffDetailsForViewAndEdit=data;
          this.cloneStaffAddModel=JSON.stringify(this.staffAddModel);
          this.findNationalityNameById();
          this.findNameById();
          this.staffCreateMode = this.staffCreate.VIEW
          if (this.staffAddModel.staffMaster.loginEmailAddress !== null) {
            this.hidePasswordAccess = false;
          }
          this.staffService.changePageMode(this.staffCreateMode);
        }
      }

    })
  }

  toggleVisibility() {
    if (this.visible) {
      this.inputType = 'password';
      this.visible = false;
      this.cd.markForCheck();
    } else {
      this.inputType = 'text';
      this.visible = true;
      this.cd.markForCheck();
    }
  }
  
  invalidScroll(){
    const firstInvalidControl: HTMLElement = this.el.nativeElement.querySelector(
      'input.ng-invalid'
    );
      firstInvalidControl.scrollIntoView({ behavior: 'smooth',block: 'center' });
  }

  ngOnDestroy() {
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }

}
