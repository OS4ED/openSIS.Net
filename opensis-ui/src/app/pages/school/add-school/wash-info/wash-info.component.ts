import { Component, OnInit, EventEmitter, Output, Input, ViewChild, OnDestroy } from '@angular/core';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators, NgForm } from '@angular/forms';
import { WashInfoEnum } from '../../../../enums/wash-info.enum';
import { SchoolAddViewModel } from '../../../../models/school-master.model';
import { fadeInRight400ms } from '../../../../../@vex/animations/fade-in-right.animation';
import { TranslateService } from '@ngx-translate/core';
import { SchoolService } from '../../../../../app/services/school.service'
import { MatSnackBar } from '@angular/material/snack-bar';
import { LoaderService } from '../../../../../app/services/loader.service';
import { SharedFunction } from '../../../shared/shared-function';
import { ImageCropperService } from '../../../../services/image-cropper.service';
import icEdit from '@iconify/icons-ic/twotone-edit';
import { SchoolCreate } from '../../../../enums/school-create.enum';
import { LovList } from '../../../../models/lov.model';
import { CommonService } from '../../../../services/common.service';
import * as cloneDeep from 'lodash/cloneDeep';
import { CommonLOV } from '../../../shared-module/lov/common-lov';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { ModuleIdentifier } from '../../../../enums/module-identifier.enum';
import { RolePermissionListViewModel, RolePermissionViewModel } from '../../../../models/roll-based-access.model';
import { RollBasedAccessService } from '../../../../services/roll-based-access.service';
@Component({
  selector: 'vex-wash-info',
  templateUrl: './wash-info.component.html',
  styleUrls: ['./wash-info.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms,
    fadeInRight400ms
  ]
})
export class WashInfoComponent implements OnInit,OnDestroy {
  schoolCreate = SchoolCreate;
  moduleIdentifier = ModuleIdentifier;
  @Input() schoolCreateMode: SchoolCreate;
  icEdit = icEdit;
  @Input() schoolDetailsForViewAndEdit;
  @Input() categoryId;
  form: FormGroup;
  washinfo = WashInfoEnum;
  @ViewChild('f') currentForm: NgForm;
  f: NgForm;
  module = "School";
  schoolAddViewModel: SchoolAddViewModel = new SchoolAddViewModel();
  editPermission: boolean = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();
  loading: boolean;
  formActionButtonTitle = "submit";
  femaleToiletTypeList;
  maleToiletTypeList;
  commonToiletTypeList;
  femaleToiletAccessibilityList;
  maleToiletAccessibilityList;
  commonToiletAccessibilityList;
  lovList: LovList = new LovList();
  cloneSchool;
  destroySubject$: Subject<void> = new Subject();
  constructor(
    private schoolService: SchoolService,
    private snackbar: MatSnackBar,
    public translateService: TranslateService,
    private imageCropperService: ImageCropperService,
    private commonService: CommonService,
    public rollBasedAccessService: RollBasedAccessService,
    private commonLOV:CommonLOV) {
    translateService.use('en');

  }
  ngOnInit(): void {

    if (this.schoolCreateMode === this.schoolCreate.VIEW) {
      this.permissionListViewModel = this.rollBasedAccessService.getPermission();
      this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 2);
      const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 1);
      const permissionSubCategory = permissionCategory.permissionSubcategory.find(x => x.permissionSubcategoryId === 2);
      this.editPermission = permissionSubCategory.rolePermission[0].canEdit;
      this.schoolService.changePageMode(this.schoolCreateMode);
      this.imageCropperService.enableUpload({module: this.moduleIdentifier.SCHOOL, upload: true, mode: this.schoolCreate.VIEW});
      this.schoolAddViewModel = this.schoolDetailsForViewAndEdit;
      this.cloneSchool = JSON.stringify(this.schoolAddViewModel);
    } else {
      this.initializeDropdownValues();
      this.imageCropperService.enableUpload({module:this.moduleIdentifier.SCHOOL,upload:true,mode:this.schoolCreate.EDIT});
      this.schoolService.changePageMode(this.schoolCreateMode);
      this.schoolAddViewModel = this.schoolService.getSchoolDetails();
      this.cloneSchool = JSON.stringify(this.schoolAddViewModel);
    }
  }

  initializeDropdownValues() {
    this.commonLOV.getLovByName('Female Toilet Type').pipe(takeUntil(this.destroySubject$)).subscribe((res)=>{
      this.femaleToiletTypeList=res;
    });
    this.commonLOV.getLovByName('Male Toilet Type').pipe(takeUntil(this.destroySubject$)).subscribe((res)=>{
      this.maleToiletTypeList=res;
    });
    this.commonLOV.getLovByName('Common Toilet Type').pipe(takeUntil(this.destroySubject$)).subscribe((res)=>{
      this.commonToiletTypeList=res;
    });
    this.commonLOV.getLovByName('Female Toilet Accessibility').pipe(takeUntil(this.destroySubject$)).subscribe((res)=>{
      this.femaleToiletAccessibilityList=res;
    });
    this.commonLOV.getLovByName('Male Toilet Accessibility').pipe(takeUntil(this.destroySubject$)).subscribe((res)=>{
      this.maleToiletAccessibilityList=res;
    });
    this.commonLOV.getLovByName('Common Toilet Accessibility').pipe(takeUntil(this.destroySubject$)).subscribe((res)=>{
      this.commonToiletAccessibilityList=res;
    });
  }

  editWashInfo() {
    this.formActionButtonTitle = "update";
    this.initializeDropdownValues();
    this.imageCropperService.enableUpload({module:this.moduleIdentifier.SCHOOL,upload:true,mode:this.schoolCreate.EDIT});
    this.schoolCreateMode = this.schoolCreate.EDIT;
    this.schoolService.changePageMode(this.schoolCreateMode);
  }
  cancelEdit() {
    this.imageCropperService.enableUpload({module:this.moduleIdentifier.SCHOOL,upload:true,mode:this.schoolCreate.VIEW});
    this.imageCropperService.cancelImage("school");
    if(JSON.stringify(this.schoolAddViewModel)!==this.cloneSchool){
      this.schoolAddViewModel=JSON.parse(this.cloneSchool);
      this.schoolDetailsForViewAndEdit=this.schoolAddViewModel;
      this.schoolService.sendDetails(JSON.parse(this.cloneSchool));
    }
    this.schoolCreateMode = this.schoolCreate.VIEW;
    this.schoolService.changePageMode(this.schoolCreateMode);

  }

  submit() {
    this.currentForm.form.markAllAsTouched();
    if (this.currentForm.form.valid) {
      if (this.schoolAddViewModel.schoolMaster.fieldsCategory !== null) {
        this.modifyCustomFields();
      }
      this.schoolService.UpdateSchool(this.schoolAddViewModel).pipe(takeUntil(this.destroySubject$)).subscribe(data => {
        if (typeof (data) === 'undefined') {
          this.snackbar.open(`Wash Info Updation failed` + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else {
          if (data._failure) {
            this.snackbar.open(`Wash Info Updation failed` + data._message, '', {
              duration: 10000
            });
          } else {
            this.snackbar.open(`Wash Info Updation Successful`, '', {
              duration: 10000
            });
            this.schoolCreateMode = this.schoolCreate.VIEW;
            this.schoolService.setSchoolCloneImage(data.schoolMaster.schoolDetail[0].schoolLogo);
            data.schoolMaster.schoolDetail[0].schoolLogo=null;
            this.schoolDetailsForViewAndEdit=data;
            this.cloneSchool=JSON.stringify(this.schoolDetailsForViewAndEdit);
            this.schoolService.changePageMode(this.schoolCreateMode);
            this.imageCropperService.enableUpload({module:this.moduleIdentifier.SCHOOL,upload:true,mode:this.schoolCreate.VIEW});
          }
        }

      })
    }
  }

  modifyCustomFields(){
    this.schoolAddViewModel.selectedCategoryId = this.schoolAddViewModel.schoolMaster.fieldsCategory[this.categoryId].categoryId;
    for (const schoolCustomField of this.schoolAddViewModel.schoolMaster.fieldsCategory[this.categoryId].customFields) {
          if (schoolCustomField.type === 'Multiple SelectBox' && this.schoolService.getSchoolMultiselectValue() !== undefined) {
            schoolCustomField.customFieldsValue[0].customFieldValue = this.schoolService.getSchoolMultiselectValue().toString().replaceAll(',', '|');
          }
        }
  }

  ngOnDestroy() {
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }

}
