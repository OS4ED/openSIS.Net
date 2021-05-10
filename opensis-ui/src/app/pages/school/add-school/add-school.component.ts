import { Component, OnInit, OnDestroy, ChangeDetectionStrategy, ChangeDetectorRef, AfterViewInit } from '@angular/core';
import { fadeInRight400ms } from '../../../../@vex/animations/fade-in-right.animation';
import { ImageCropperService } from '../../../services/image-cropper.service';

import { SchoolAddViewModel } from '../../../models/school-master.model';
import { ActivatedRoute, Router } from '@angular/router';
import { SchoolService } from '../../../services/school.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SharedFunction } from '../../../pages/shared/shared-function';
import { CommonService } from '../../../services/common.service';
import { LayoutService } from '../../../../../src/@vex/services/layout.service';
import { CustomFieldAddView, CustomFieldListViewModel, CustomFieldModel } from '../../../models/custom-field.model';
import { CustomFieldService } from '../../../services/custom-field.service';
import { FieldsCategoryAddView, FieldsCategoryListView, FieldsCategoryModel } from '../../../models/fields-category.model';
import { LoaderService } from '../../../services/loader.service';
import { SchoolCreate } from '../../../enums/school-create.enum';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { ModuleIdentifier } from '../../../enums/module-identifier.enum';
import { RolePermissionListViewModel } from 'src/app/models/roll-based-access.model';
import { CryptoService } from 'src/app/services/Crypto.service';
import { stagger60ms } from '../../../../@vex/animations/stagger.animation';
import { fadeInUp400ms } from '../../../../@vex/animations/fade-in-up.animation';
import { MatDialog } from '@angular/material/dialog';
import { AddCopySchoolComponent } from './add-copy-school/add-copy-school.component';
import { SuccessCopySchoolComponent } from './success-copy-school/success-copy-school.component';
import icCleanHands from '@iconify/icons-ic/outline-clean-hands';
import icArticle from '@iconify/icons-ic/outline-article';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'vex-add-school',
  templateUrl: './add-school.component.html',
  styleUrls: ['./add-school.component.scss'],
  animations: [
    fadeInRight400ms,
    stagger60ms,
    fadeInUp400ms
  ]
})

export class AddSchoolComponent implements OnInit, OnDestroy {
  schoolCreate = SchoolCreate;
  schoolCreateMode: SchoolCreate = SchoolCreate.ADD;
  schoolTitle = 'Add School Information';
  pageStatus = 'Add School';
  responseImage: string;
  image = '';
  module = 'School';
  fieldsCategory: FieldsCategoryModel[];
  fieldsCategoryListView = new FieldsCategoryListView();
  schoolId: number = null;
  enableCropTool = false;
  indexOfCategory = 0;
  schoolAddViewModel: SchoolAddViewModel = new SchoolAddViewModel();
  customFieldModel: [CustomFieldModel];
  currentCategory;


  loading: boolean;
  destroySubject$: Subject<void> = new Subject();
  moduleIdentifier = ModuleIdentifier;
  icCleanHands = icCleanHands;
  icArticle = icArticle;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup;
  addPermission: boolean;


  constructor(private imageCropperService: ImageCropperService,
              private Activeroute: ActivatedRoute,
              private snackbar: MatSnackBar,
              private schoolService: SchoolService,
              private router: Router,
              private commonFunction: SharedFunction,
              private layoutService: LayoutService,
              private loaderService: LoaderService,
              private customFieldservice: CustomFieldService,
              private cryptoService: CryptoService,
              private dialog: MatDialog,
              private cdr: ChangeDetectorRef) {
    // !this.schoolService.getSchoolId() ? this.router.navigate(['/school', 'schoolinfo']) : null;
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.layoutService.collapseSidenav();
    this.imageCropperService.getUncroppedEvent().pipe(takeUntil(this.destroySubject$)).subscribe((res) => {
      this.schoolService.setSchoolImage(btoa(res.target.result));
    });
    this.schoolService.modeToUpdate.pipe(takeUntil(this.destroySubject$)).subscribe((res) => {
      if (res === this.schoolCreate.VIEW){
        this.pageStatus = 'View School';
      }else{
        this.pageStatus = 'Edit School';
      }
    });
    this.schoolService.categoryToSend.pipe(takeUntil(this.destroySubject$)).subscribe((res) => {
        this.currentCategory = this.currentCategory + 1;
    });
    this.loaderService.isLoading.pipe(takeUntil(this.destroySubject$)).subscribe((val) => {
      this.loading = val;
    });
    this.schoolService.getSchoolDetailsForGeneral.pipe(takeUntil(this.destroySubject$)).subscribe((res: SchoolAddViewModel) => {
      this.schoolAddViewModel = res;
    });
    this.getCurrentCategory();
  }

  ngOnInit() {
    this.schoolCreateMode = this.schoolCreate.ADD;
    this.schoolService.sendDetails(this.schoolAddViewModel);
    this.schoolId = this.schoolService.getSchoolId();
    if (this.schoolId != null) {
      this.schoolCreateMode = this.schoolCreate.VIEW;
      this.getSchoolGeneralandWashInfoDetails();
      this.onViewMode();
    }else if (this.schoolCreateMode === this.schoolCreate.ADD) {     
      if(this.checkCanAddSchool()) {
        this.getAllFieldsCategory();
        this.imageCropperService.enableUpload({module: this.moduleIdentifier.SCHOOL, upload: true, mode: this.schoolCreate.ADD});
      } else{
        this.router.navigate(['/school','schoolinfo']);
      }
     
    }

  }

  checkCanAddSchool() {
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x=>x.permissionGroup.permissionGroupId == 2);
    let permissionCategory= this.permissionGroup.permissionGroup.permissionCategory.find(x=>x.permissionCategoryId == 1);
    let permissionSubCategory = permissionCategory.permissionSubcategory.find( x => x.permissionSubcategoryId === 1);
   return permissionSubCategory.rolePermission[0].canAdd;
  }

  ngAfterViewChecked() {
    this.cdr.detectChanges();
  }

  onViewMode() {
    this.pageStatus = 'View School';
  }
  getCurrentCategory(){
    this.permissionListViewModel.permissionList.map((category) => {
      if (category.permissionGroup.permissionGroupId === 2){
        category.permissionGroup.permissionCategory.map((subCategory) => {
          if (subCategory.permissionCategoryId === 1){
            for (const result of subCategory.permissionSubcategory){
              if (result.rolePermission[0].canView){
                this.currentCategory = result.permissionSubcategoryId;
                break;
              }
            }
          }
        });
      }
    });
    // this.currentCategory;
  }

  changeCategory(categoryDetails, index) {
    const schoolDetails = this.schoolService.getSchoolDetails();
    if (schoolDetails !== undefined || schoolDetails != null) {
      this.schoolCreateMode = this.schoolCreate.EDIT;
      this.currentCategory = categoryDetails.categoryId;
      this.indexOfCategory = index;
      this.schoolAddViewModel = schoolDetails;
    }

    if (this.schoolCreateMode === this.schoolCreate.VIEW) {
      this.currentCategory = categoryDetails.categoryId;
      this.indexOfCategory = index;
    }
  }

  getAllFieldsCategory() {
    this.fieldsCategoryListView.module = 'School';
    this.fieldsCategoryListView.schoolId = +sessionStorage.getItem('selectedSchoolId');
    this.customFieldservice.getAllFieldsCategory(this.fieldsCategoryListView).subscribe((res) => {
      if (typeof (res) === 'undefined') {
        this.snackbar.open('Custom Field list failed. ' + sessionStorage.getItem('httpError'), '', {
          duration: 10000
        });
      }
      else {
        if (res._failure) {
          this.snackbar.open('Custom Field list failed. ' + res._message, '', {
            duration: 10000
          });
        }
        else {
          this.fieldsCategory = res.fieldsCategoryList.filter(x => x.isSystemCategory === true);
        }
      }
    }
    );
  }

  checkViewPermission(category){
    category = category.filter((item) => {
     for (const permission of this.permissionListViewModel.permissionList[1].permissionGroup.permissionCategory[0].permissionSubcategory){
      if ( item.title.toLowerCase() === permission.permissionSubcategoryName.toLowerCase()){
        if (permission.rolePermission[0].canView === true){
          return item;
        }
     }
    }
    });
    this.currentCategory = category[0]?.categoryId;

    return category;
  }

  getSchoolGeneralandWashInfoDetails() {
    this.schoolAddViewModel.schoolMaster.schoolDetail[0].schoolId = this.schoolService.getSchoolId();
    this.schoolAddViewModel.schoolMaster.schoolId = this.schoolAddViewModel.schoolMaster.schoolDetail[0].schoolId;
    this.schoolService.ViewSchool(this.schoolAddViewModel).subscribe(data => {
      this.schoolAddViewModel = data;
      this.responseImage = this.schoolAddViewModel.schoolMaster.schoolDetail[0].schoolLogo;
      this.schoolAddViewModel.schoolMaster.schoolDetail[0].schoolLogo = null;
      this.schoolService.sendDetails(this.schoolAddViewModel);
      this.fieldsCategory = this.checkViewPermission(data.schoolMaster.fieldsCategory);
      this.schoolAddViewModel.schoolMaster.fieldsCategory = this.fieldsCategory;
      this.schoolTitle = this.schoolAddViewModel.schoolMaster.schoolName;
      this.schoolService.setSchoolImage(this.responseImage);
      this.schoolService.setSchoolCloneImage(this.responseImage);
    });
  }

  addCopySchool() {
    this.dialog.open(AddCopySchoolComponent, {
      width: '900px'
    });
  }

  successCopySchool() {
    this.dialog.open(SuccessCopySchoolComponent, {
      width: '500px'
    });
  }

  ngOnDestroy() {
    // this.schoolService.setSchoolDetails(null);
    this.schoolService.setSchoolImage(null);
    this.schoolService.setSchoolId(null);
    this.schoolService.setSchoolCloneImage(null);
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }

}
