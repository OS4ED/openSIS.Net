import { Component, OnInit, Input, ViewChild, ChangeDetectorRef, OnDestroy, EventEmitter, Output, AfterViewInit, ElementRef } from '@angular/core';
import { FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { fadeInRight400ms } from '../../../../../@vex/animations/fade-in-right.animation';
import { StudentService } from '../../../../services/student.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TranslateService } from '@ngx-translate/core';
import { CommonService } from '../../../../services/common.service';
import { CheckStudentInternalIdViewModel, StudentAddModel } from '../../../../models/student.model';
import { CountryModel } from '../../../../models/country.model';
import { LanguageModel } from '../../../../models/language.model';
import { LoginService } from '../../../../services/login.service';
import * as _moment from 'moment';
import { default as _rollupMoment } from 'moment';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { MY_FORMATS } from '../../../shared/format-datepicker';
import { SharedFunction } from '../../../shared/shared-function';
import { SchoolCreate } from '../../../../enums/school-create.enum';
import icEdit from '@iconify/icons-ic/edit';
import { Subject } from 'rxjs/internal/Subject';
import { auditTime, debounceTime, distinctUntilChanged, takeUntil, shareReplay } from 'rxjs/operators';
import icVisibility from '@iconify/icons-ic/twotone-visibility';
import icVisibilityOff from '@iconify/icons-ic/twotone-visibility-off';
import { SectionService } from '../../../../services/section.service';
import { GetAllSectionModel, TableSectionList } from '../../../../models/section.model';
import { CheckUserEmailAddressViewModel } from '../../../../models/user.model';
import { ImageCropperService } from '../../../../services/image-cropper.service';
import { LovList } from '../../../../models/lov.model';
import {MiscModel} from '../../../../models/misc-data-student.model';
import { CommonLOV } from '../../../shared-module/lov/common-lov';
import {ModuleIdentifier} from '../../../../enums/module-identifier.enum'
import { CryptoService } from '../../../../services/Crypto.service';
import { RolePermissionListViewModel, RolePermissionViewModel } from '../../../../models/roll-based-access.model';
import { Router } from '@angular/router';
@Component({
  selector: 'vex-student-generalinfo',
  templateUrl: './student-generalinfo.component.html',
  styleUrls: ['./student-generalinfo.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms,
    fadeInRight400ms
  ],
  providers: [
    { provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE] },
    { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
  ],
})
export class StudentGeneralinfoComponent implements OnInit, AfterViewInit, OnDestroy {
  icEdit = icEdit;
  icVisibility = icVisibility;
  icVisibilityOff = icVisibilityOff;

  studentCreate = SchoolCreate;
  @Input() studentCreateMode: SchoolCreate;
  @Input() studentDetailsForViewAndEdit;
  @Input() categoryId;
  @ViewChild('f') currentForm: NgForm;

  data;
  moduleIdentifier=ModuleIdentifier;
  nameOfMiscValuesForView:MiscModel=new MiscModel; //This Object contains Section Name, Nationality, Country, languages for View Mode.
  countryListArr = [];
  ethnicityList = [];
  raceList = [];
  genderList = [];
  suffixList = [];
  salutationList = [];
  maritalStatusList = [];
  countryModel: CountryModel = new CountryModel();
  destroySubject$: Subject<void> = new Subject();
  form: FormGroup;
  studentAddModel: StudentAddModel = new StudentAddModel();
  checkStudentInternalIdViewModel: CheckStudentInternalIdViewModel = new CheckStudentInternalIdViewModel();
  checkUserEmailAddressViewModel: CheckUserEmailAddressViewModel = new CheckUserEmailAddressViewModel();
  sectionList: GetAllSectionModel = new GetAllSectionModel();
  languages: LanguageModel = new LanguageModel();
  lovListViewModel: LovList = new LovList()
  module = 'Student';
  saveAndNext = 'saveAndNext';
  pageStatus: string;
  languageList;
  inputType = 'password';
  studentInternalId = '';
  studentPortalId = '';
  visible = false;
  pass: string = "";
  isUser :boolean= false;
  isStudentInternalId :boolean= false;
  hidePasswordAccess: boolean = false;
  hideAccess: boolean = false;
  fieldDisabled: boolean = false;
  internalId: FormControl;
  loginEmail:FormControl;
  cloneStudentModel;
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();

  @Output() dataAfterSavingGeneralInfo = new EventEmitter<any>();
  constructor(
    private el: ElementRef,
    public translateService: TranslateService,
    private snackbar: MatSnackBar,
    private studentService: StudentService,
    private commonService: CommonService,
    private loginService: LoginService,
    private commonFunction: SharedFunction,
    private sectionService: SectionService,
    private cd: ChangeDetectorRef,
    private router:Router,
    private imageCropperService: ImageCropperService,
    private cryptoService: CryptoService,
    private commonLOV:CommonLOV) {
    translateService.use('en');
    this.studentService.getStudentDetailsForGeneral.pipe(takeUntil(this.destroySubject$)).subscribe((res: StudentAddModel) => {
      this.studentAddModel = res;
      this.studentAddModel.loginEmail = this.studentAddModel.studentMaster.studentPortalId;
      this.data = this.studentAddModel?.studentMaster;
      this.cloneStudentModel=JSON.stringify(this.studentAddModel);
      this.studentInternalId = this.data.studentInternalId;
      this.studentPortalId = this.data.studentPortalId;
      if(this.studentAddModel.studentMaster?.studentId){
        this.accessPortal();
        this.GetAllLanguage();
        this.initializeDropdowns()
      }
    })
  }

  ngOnInit(): void {

    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 3);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 5);
    const permissionSubCategory = permissionCategory.permissionSubcategory.find( x => x.permissionSubcategoryId === 3);
    this.editPermission = permissionSubCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionSubCategory.rolePermission[0].canDelete;
    this.addPermission = permissionSubCategory.rolePermission[0].canAdd;
    this.internalId = new FormControl('',Validators.required);
    this.loginEmail=new FormControl('',Validators.required);
    if (this.studentCreateMode == this.studentCreate.ADD) {
      if(this.addPermission===false){
        this.router.navigate(['/'])
      }
      this.initializeDropdownsInAddMode();
    } else if (this.studentCreateMode == this.studentCreate.VIEW) {
      this.studentService.changePageMode(this.studentCreateMode);
      this.studentAddModel = this.studentDetailsForViewAndEdit;
      this.data = this.studentDetailsForViewAndEdit?.studentMaster;
      if(this.studentAddModel.studentMaster.studentPortalId == null){
        this.data.studentPortalId = this.studentAddModel.loginEmail;
      }
      this.accessPortal();
      this.cloneStudentModel=JSON.stringify(this.studentAddModel);

        this.GetAllLanguage();
        this.getAllCountry();
        // this.callLOVs();
      

    } else if (this.studentCreateMode == this.studentCreate.EDIT && (this.studentDetailsForViewAndEdit != undefined || this.studentDetailsForViewAndEdit != null)) {
      this.studentAddModel = this.studentDetailsForViewAndEdit;
      this.cloneStudentModel=JSON.stringify(this.studentAddModel);
      this.data=this.studentAddModel.studentMaster;
      if(this.studentAddModel.studentMaster.studentPortalId ==null){
        this.data.studentPortalId = this.studentAddModel.loginEmail;
      }
      else{
        this.studentPortalId = this.studentAddModel.studentMaster.studentPortalId;
      }
      
      this.studentService.changePageMode(this.studentCreateMode);
      this.accessPortal();
      this.initializeDropdownsInAddMode();
      this.saveAndNext = 'update';
      if (this.studentAddModel.studentMaster.studentPortalId !== null) {
        this.hideAccess = true;
        this.fieldDisabled = true;
      }
    }
  }

  initializeDropdowns(){
    this.getAllCountry();
  }

  initializeDropdownsInAddMode(){
    this.callLOVs();
    this.getAllCountry();
    this.GetAllLanguage();
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

  ngAfterViewInit() {
    this.studentInternalId = this.data?.studentInternalId;
    if(this.studentAddModel.studentMaster.studentPortalId ==null){
      this.studentPortalId = this.studentAddModel.loginEmail;
    }
    else{
      this.studentPortalId = this.data?.studentPortalId;
    }
    // For Checking Internal Id
    this.internalId.valueChanges.pipe(debounceTime(500), distinctUntilChanged()).subscribe((term) => {
      if (term != '') {
        if (this.studentInternalId === term) {
          this.internalId.setErrors(null);
        }
        else {
          this.isStudentInternalId= true;
          this.checkStudentInternalIdViewModel.studentInternalId = term;
          this.studentService.checkStudentInternalId(this.checkStudentInternalIdViewModel).subscribe(data => {
            if (data.isValidInternalId) {
              this.internalId.setErrors(null);
              this.isStudentInternalId= false;
            }
            else {
              this.internalId.markAsTouched();
              this.internalId.setErrors({ 'nomatch': true });
              this.isStudentInternalId= false;
            }
          });
        }
      } else {
        this.internalId.markAsTouched();
        this.isStudentInternalId= false;
      }
    });

    this.loginEmail.valueChanges
    .pipe(debounceTime(600), distinctUntilChanged())
    .subscribe(term => {
      if (term != '') {
        if (this.studentPortalId === term) {
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

  accessPortal() {
    if (this.data?.studentPortalId !== null && this.data?.studentPortalId !== undefined) {
      this.hideAccess = true;
      this.fieldDisabled = true;
      this.hidePasswordAccess = false;
    } else {
      this.hideAccess = false;
      this.fieldDisabled = false;
      this.hidePasswordAccess = false;
    }
  }

 

  

  isPortalAccess(event) {
    if (event.checked) {
      if (this.studentCreateMode == this.studentCreate.ADD) {
        this.hidePasswordAccess = true;
      }
      else {
        if (this.data.studentPortalId !== null) {
          this.hidePasswordAccess = false;
        }
        else {
          this.hidePasswordAccess = true;
        }
      }
      this.hideAccess = true;
      this.studentAddModel.portalAccess = true;
    }
    else {
      this.hideAccess = false;
      this.hidePasswordAccess = false;
      this.studentAddModel.portalAccess = false;
    }
  }


  getAllCountry() {
    if(!this.countryModel.isCountryAvailable){
      this.countryModel.isCountryAvailable=true;
      this.commonService.GetAllCountry(this.countryModel).pipe(takeUntil(this.destroySubject$)).subscribe(data => {
        if (typeof (data) == 'undefined') {
          this.countryListArr = [];
        }
        else {
          if (data._failure) {
            this.countryListArr = [];
          } else {
            this.countryListArr=data.tableCountry?.sort((a, b) => {return a.name < b.name ? -1 : 1;} )   
            if (this.studentCreateMode == this.studentCreate.VIEW) {
             this.findCountryNationalityById();
            }
          }
        }
      })
    }
    
  }

  findCountryNationalityById(){
    this.countryListArr.map((val) => {
      let countryInNumber = +this.data.countryOfBirth;
      let nationality = +this.data.nationality;
      if (val.id === countryInNumber) {
        this.nameOfMiscValuesForView.countryName = val.name;
      }
      if (val.id === nationality) {
        this.nameOfMiscValuesForView.nationality = val.name;
      }
    });
  }
  editGeneralInfo() {
    this.studentCreateMode = this.studentCreate.EDIT
    this.callLOVs();
    this.studentService.changePageMode(this.studentCreateMode);
    this.saveAndNext = 'update';
  }
  
  cancelEdit() {
    if(JSON.stringify(this.studentAddModel)!==this.cloneStudentModel){
      this.studentAddModel=JSON.parse(this.cloneStudentModel);
      this.studentDetailsForViewAndEdit=JSON.parse(this.cloneStudentModel);
      this.studentService.sendDetails(JSON.parse(this.cloneStudentModel));
    }
    this.findCountryNationalityById();
    this.findLanguagesById();
    this.studentCreateMode = this.studentCreate.VIEW
    this.imageCropperService.cancelImage("student");
    this.studentService.changePageMode(this.studentCreateMode);
    this.data = this.studentAddModel.studentMaster;       
  }

  GetAllLanguage() {
    if(!this.languages.isLanguageAvailable){
    this.languages.isLanguageAvailable=true;
    this.languages._tenantName = sessionStorage.getItem("tenant");
    this.loginService.getAllLanguage(this.languages).pipe(takeUntil(this.destroySubject$)).subscribe((res) => {
      if (typeof (res) == 'undefined') {
        this.languageList = [];
      }
      else {
        this.languageList=res.tableLanguage?.sort((a, b) => {return a.locale < b.locale ? -1 : 1;} )   
        if (this.studentCreateMode == this.studentCreate.VIEW) {
         this.findLanguagesById()
        }
      }
    })
  }
  }

  findLanguagesById(){
    this.languageList.map((val) => {
      let firstLanguageId = + this.data.firstLanguageId;
      let secondLanguageId = + this.data.secondLanguageId;
      let thirdLanguageId = + this.data.thirdLanguageId;
      
      if (val.langId === firstLanguageId) {
        this.nameOfMiscValuesForView.firstLanguage = val.locale;
      }
      if (val.langId === secondLanguageId) {
        this.nameOfMiscValuesForView.secondLanguage = val.locale;
      }
      if (val.langId === thirdLanguageId) {
        this.nameOfMiscValuesForView.thirdLanguage = val.locale;
      }
    });
  }


  generate() {
    this.inputType = 'text';
    this.visible = true;
    this.currentForm.controls.passwordHash.setValue(this.commonFunction.autoGeneratePassword());
  }

  submit() {
    this.currentForm.form.markAllAsTouched();
    if (this.currentForm.controls.passwordHash !== undefined) {
      this.studentAddModel.passwordHash = this.currentForm.controls.passwordHash.value;
    }

    if (this.currentForm.form.valid) {

      if (this.studentAddModel.fieldsCategoryList !== null) {
        this.studentAddModel.selectedCategoryId = this.studentAddModel.fieldsCategoryList[this.categoryId].categoryId;
        for (let studentCustomField of this.studentAddModel.fieldsCategoryList[this.categoryId].customFields) {
          if (studentCustomField.type === "Multiple SelectBox" && this.studentService.getStudentMultiselectValue() !== undefined) {
            studentCustomField.customFieldsValue[0].customFieldValue = this.studentService.getStudentMultiselectValue().toString().replaceAll(",", "|");
          }
        }
      }
      if (this.studentCreateMode == this.studentCreate.EDIT) {
        this.updateStudent();
      } else {
        this.addStudent();
      }
    }
  }

  updateStudent() {
    if (this.internalId.invalid) {
      this.invalidScroll();
      return
    }
    this.studentService.UpdateStudent(this.studentAddModel).pipe(takeUntil(this.destroySubject$)).subscribe(data => {
      if (typeof (data) == 'undefined') {
        this.snackbar.open('Student Update failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (data._failure) {
          this.snackbar.open( data._message, '', {
            duration: 10000
          });
        } else {
          this.snackbar.open(data._message, '', {
            duration: 10000
          });
          this.studentService.setStudentCloneImage(data.studentMaster.studentPhoto);
          data.studentMaster.studentPhoto=null;
          this.data = data.studentMaster;
          this.cloneStudentModel=JSON.stringify(data);
          this.findCountryNationalityById();
          this.findLanguagesById();
          this.studentDetailsForViewAndEdit=data;
          this.dataAfterSavingGeneralInfo.emit(data);
          this.studentCreateMode = this.studentCreate.VIEW
          this.studentService.changePageMode(this.studentCreateMode);
        }
      }
    });
  }

  addStudent() {
    if (this.internalId.invalid) {
      this.invalidScroll();
      return
    }
    this.studentAddModel.studentMaster.dob = this.commonFunction.formatDateSaveWithoutTime(this.studentAddModel.studentMaster.dob);
    this.studentService.AddStudent(this.studentAddModel).pipe(takeUntil(this.destroySubject$)).subscribe(data => {
      if (typeof (data) == 'undefined') {
        this.snackbar.open('Student Save failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (data._failure) {
          this.snackbar.open( data._message, '', {
            duration: 10000
          });
        } else {
          this.snackbar.open(data._message, '', {
            duration: 10000
          });
          this.studentService.setStudentId(data.studentMaster.studentId);
          this.studentService.setStudentCloneImage(data.studentMaster.studentPhoto);
          this.studentService.changeCategory(4);
          this.studentService.setStudentDetails(data);
          this.dataAfterSavingGeneralInfo.emit(data);
          this.imageCropperService.enableUpload({module:this.moduleIdentifier.STUDENT,upload:true,mode:this.studentCreate.EDIT});
        }
      }

    })
  }

  invalidScroll() {
    const firstInvalidControl: HTMLElement = this.el.nativeElement.querySelector(
      'input.ng-invalid'
    );
    firstInvalidControl.scrollIntoView({ behavior: 'smooth', block: 'center' });
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

  ngOnDestroy() {
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }


}
