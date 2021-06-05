/***********************************************************************************
openSIS is a free student information system for public and non-public
schools from Open Solutions for Education, Inc.Website: www.os4ed.com.

Visit the openSIS product website at https://opensis.com to learn more.
If you have question regarding this software or the license, please contact
via the website.

The software is released under the terms of the GNU Affero General Public License as
published by the Free Software Foundation, version 3 of the License.
See https://www.gnu.org/licenses/agpl-3.0.en.html.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

Copyright (c) Open Solutions for Education, Inc.

All rights reserved.
***********************************************************************************/

import { Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SharedFunction } from '../../../../src/app/pages/shared/shared-function';
import { FieldsCategoryListView, FieldsCategoryModel } from '../../models/fields-category.model';
import { CustomFieldService } from '../../../../src/app/services/custom-field.service';
import { CustomFieldListViewModel, CustomFieldModel } from '../../models/custom-field.model';
import icAdd from '@iconify/icons-ic/baseline-add';
import icEdit from '@iconify/icons-ic/twotone-edit';
import { NgForm } from '@angular/forms';
import { stagger60ms } from '../../../../src/@vex/animations/stagger.animation';
import { fadeInUp400ms } from '../../../../src/@vex/animations/fade-in-up.animation';
import { fadeInRight400ms } from '../../../../src/@vex/animations/fade-in-right.animation';
import { SchoolCreate } from '../../../../src/app/enums/school-create.enum';
import { SchoolAddViewModel } from '../../models/school-master.model';
import { SchoolService } from '../../../../src/app/services/school.service';
import { CustomFieldsValueModel } from '../../models/custom-fields-value.model';
import { StudentAddModel } from '../../models/student.model';
import { StudentService } from '../../../../src/app/services/student.service';
import { StaffAddModel } from '../../models/staff.model';
import { StaffService } from '../../services/staff.service';
import { CryptoService } from '../../services/Crypto.service';
import { RolePermissionListViewModel, RolePermissionViewModel } from '../../models/roll-based-access.model';
import { CommonService } from '../../services/common.service';
import { DefaultValuesService } from '../default-values.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'vex-custom-field',
  templateUrl: './custom-field.component.html',
  styleUrls: ['./custom-field.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms,
    fadeInRight400ms
  ]
})
export class CustomFieldComponent implements OnInit, OnDestroy {
  SchoolCreate = SchoolCreate;
  schoolCreateMode;
  studentCreateMode;
  staffCreateMode;
  categoryTitle;
  categoryId;
  schoolDetailsForViewAndEdit;
  module;
  icEdit = icEdit;
  icAdd = icAdd;
  viewInfo: boolean = true;
  editInfo: boolean = false;
  studentAddViewModel: StudentAddModel = new StudentAddModel();
  schoolAddViewModel: SchoolAddViewModel = new SchoolAddViewModel();
  staffAddViewModel: StaffAddModel = new StaffAddModel();
  @ViewChild('f') currentForm: NgForm;
  staffMultiSelectValue;
  studentMultiSelectValue;
  schoolMultiSelectValue
  editStudentPermission:boolean= false;
  editStaffPermission:boolean= false;
  editSchoolPermission:boolean= false;
  permissionListViewModel:RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup:RolePermissionViewModel= new RolePermissionViewModel();
  f: NgForm;
  formActionButtonTitle = "update";
  destroySubject$: Subject<void> = new Subject();

  constructor(
    private snackbar: MatSnackBar,
    private commonFunction: SharedFunction,
    private studentService: StudentService,
    private schoolService: SchoolService,
    private staffService: StaffService,
    private commonService:CommonService,
    private cryptoService: CryptoService,
    private defaultService: DefaultValuesService
  ) {
  }

  ngOnInit(): void {
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));

    this.module = this.commonService.getModuleName();
  // School Details Subscription


    this.schoolService.selectedCategoryTitle.subscribe((res)=>{
      if(res && this.module==='School'){
        this.categoryTitle = res;
      }
    });
    this.schoolService.categoryIdSelected.subscribe((res)=>{
      if(res>=0 && this.module==='School'){
        this.categoryId = res;
        this.checkCustomValue();
      }
    });
    this.schoolService.schoolCreatedMode.subscribe((res)=>{
      if(res>=0 && this.module==='School'){
        this.schoolCreateMode = res;
      }
    });
    this.schoolService.schoolDetailsForViewedAndEdited.subscribe((res)=>{
      if(res?.schoolMaster?.fieldsCategory && this.module==='School'){
        this.schoolDetailsForViewAndEdit = res;
        this.schoolAddViewModel = res;
        // this.checkCustomValue();
      }
    });

  
// Student Details Subscription
    this.studentService.selectedCatgoryTitle.subscribe((res)=>{
      if(res && this.module==='Student'){
        this.categoryTitle=res;
      }
    })
    this.studentService.categoryIdSelected.subscribe((res)=>{
      if(res>=0 && this.module==='Student'){
        this.categoryId = res;
      this.checkStudentCustomValue();
      }
    });
    this.studentService.studentCreatedMode.subscribe((res)=>{
      if(res>=0 && this.module==='Student'){
        this.studentCreateMode = res;
      }
    })
    this.studentService.studentDetailsForViewedAndEdited.subscribe((res)=>{
      if(res && this.module==='Student'){
        this.schoolDetailsForViewAndEdit = res;
      }
    })

    // Staff Details Subscription
    this.staffService.staffDetailsForViewedAndEdited.subscribe((res)=>{
      if(res && this.module==='Staff'){
        this.schoolDetailsForViewAndEdit = res;
      }
    })

    this.staffService.selectedCategoryTitle.subscribe((res)=>{
      if(res && this.module==='Staff'){
        this.categoryTitle=res;
      }
    })
    this.staffService.categoryIdSelected.subscribe((res)=>{
      if(res && this.module==='Staff'){
        this.categoryId = res;
        this.checkStaffCustomValue();
      }
    });
    this.staffService.staffCreatedMode.subscribe((res)=>{
      if(res>=0 && this.module==='Staff'){
        this.staffCreateMode = res;
      }
    })
   

    if (this.module === 'Student') {
      this.permissionGroup = this.permissionListViewModel?.permissionList.find(x=>x.permissionGroup.permissionGroupId == 3);
      let permissionCategory= this.permissionGroup.permissionGroup.permissionCategory.find(x=>x.permissionCategoryId == 5);
      let permissionSubCategory = permissionCategory.permissionSubcategory.find(x=>x.permissionSubcategoryName == this.categoryTitle);
      this.editStudentPermission = permissionSubCategory.rolePermission[0].canEdit;
      this.studentAddViewModel = this.schoolDetailsForViewAndEdit;
      this.checkStudentCustomValue();
    }
    else if (this.module === 'School') {
      if(this.schoolAddViewModel.schoolMaster.schoolId===this.defaultService.getSchoolID()){
        this.permissionGroup = this.permissionListViewModel?.permissionList.find(x=>x.permissionGroup.permissionGroupId == 2);
        let permissionCategory= this.permissionGroup.permissionGroup.permissionCategory.find(x=>x.permissionCategoryId == 1);
        let permissionSubCategory = permissionCategory.permissionSubcategory.find(x=>x.permissionSubcategoryName == this.categoryTitle);
        this.editSchoolPermission = permissionSubCategory.rolePermission[0].canEdit;
      }else{
         this.editSchoolPermission=true;
      }
      this.checkCustomValue();
    }
    else if (this.module === 'Staff') {
      this.permissionGroup = this.permissionListViewModel?.permissionList.find(x=>x.permissionGroup.permissionGroupId == 5);
      let permissionCategory= this.permissionGroup.permissionGroup.permissionCategory.find(x=>x.permissionCategoryId == 10);
      let permissionSubCategory = permissionCategory.permissionSubcategory.find(x=>x.permissionSubcategoryName == this.categoryTitle);
      this.editStaffPermission = permissionSubCategory.rolePermission[0].canEdit;
      this.staffAddViewModel = this.schoolDetailsForViewAndEdit;
      this.checkStaffCustomValue();
    }
  }

  submit() {
    this.currentForm.form.markAllAsTouched();
    if (this.currentForm.form.valid) {
      if (this.module === 'School') {
        this.updateSchool();
      }
      else if (this.module === 'Student') {
        this.updateStudent();
      }
      else if (this.module === 'Staff') {
        this.updateStaff();
      }
    }
  }



  checkStudentCustomValue() {
    if (this.studentAddViewModel !== undefined) {
      if (this.studentAddViewModel.fieldsCategoryList !== undefined) {
        for (let studentCustomField of this.studentAddViewModel.fieldsCategoryList[this.categoryId]?.customFields) {
          if (studentCustomField?.customFieldsValue.length == 0) {
            studentCustomField?.customFieldsValue.push(new CustomFieldsValueModel());
          }
          else {
            if (studentCustomField?.type === 'Multiple SelectBox') {
              this.studentMultiSelectValue = studentCustomField?.customFieldsValue[0].customFieldValue.split('|');

            }
          }
        }
      }
    }

  }

  checkStaffCustomValue() {
    if (this.staffAddViewModel !== undefined) {
      if (this.staffAddViewModel.fieldsCategoryList !== undefined) {

        for (let staffCustomField of this.staffAddViewModel?.fieldsCategoryList[this.categoryId]?.customFields) {
          if (staffCustomField?.customFieldsValue.length == 0) {

            staffCustomField?.customFieldsValue.push(new CustomFieldsValueModel());
          }
          else {
            if (staffCustomField?.type === 'Multiple SelectBox') {
              this.staffMultiSelectValue = staffCustomField?.customFieldsValue[0].customFieldValue.split('|');

            }
          }
        }

      }
    }
  }

  checkCustomValue() {
    if(this.schoolAddViewModel?.schoolMaster?.fieldsCategory?.length>0 && this.categoryId>=0){
      for (let customField of this.schoolAddViewModel?.schoolMaster?.fieldsCategory[this.categoryId]?.customFields) {
        if (customField.customFieldsValue.length == 0) {
          customField.customFieldsValue.push(new CustomFieldsValueModel());
        }else {
          if (customField.type === 'Multiple SelectBox') {
            this.schoolMultiSelectValue = customField.customFieldsValue[0].customFieldValue.split('|');

          }
        }
      }
    }
       
      
  }
  updateStudent() {
    this.studentAddViewModel.selectedCategoryId = this.studentAddViewModel.fieldsCategoryList[this.categoryId].categoryId;
    for (let studentCustomField of this.studentAddViewModel.fieldsCategoryList[this.categoryId].customFields) {
      if (studentCustomField.type === "Multiple SelectBox" && this.studentMultiSelectValue !== undefined) {
        studentCustomField.customFieldsValue[0].customFieldValue = this.studentMultiSelectValue.toString().replaceAll(",", "|");
      }
    }
    this.studentService.UpdateStudent(this.studentAddViewModel).subscribe(data => {
      if (typeof (data) == 'undefined') {
        this.snackbar.open(this.categoryTitle + ' Updation failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (data._failure) {
          this.snackbar.open(this.categoryTitle + ' Updation failed. ' + data._message, ' ', {
            duration: 10000
          });
        } else {
          this.snackbar.open(this.categoryTitle + ' Updated Successfully.', '', {
            duration: 10000
          });
          this.studentCreateMode = this.SchoolCreate.VIEW


        }
      }

    })
  }



  mapCustomFields() {
    this.studentAddViewModel.fieldsCategoryList[this.categoryId].customFields.map((item: any)=>{
      if(item.customFieldsValue.length === 0 ){
        item.customFieldsValue.push(new CustomFieldsValueModel())
      }
    })
  }

  mapStaffCustomFields(){
    this.staffAddViewModel.fieldsCategoryList[this.categoryId].customFields.map((item: any)=>{
      if(item.customFieldsValue.length === 0 ){
        item.customFieldsValue.push(new CustomFieldsValueModel())
      }
    })
  }

  updateSchool() {
    this.schoolAddViewModel.selectedCategoryId = this.schoolAddViewModel.schoolMaster.fieldsCategory[this.categoryId].categoryId;
    this.schoolAddViewModel.schoolMaster.city = this.schoolAddViewModel.schoolMaster.city.toString();
    this.schoolAddViewModel.schoolMaster.schoolDetail[0].dateSchoolOpened = this.commonFunction.formatDateSaveWithoutTime(this.schoolAddViewModel.schoolMaster.schoolDetail[0].dateSchoolOpened);
    this.schoolAddViewModel.schoolMaster.schoolDetail[0].dateSchoolClosed = this.commonFunction.formatDateSaveWithoutTime(this.schoolAddViewModel.schoolMaster.schoolDetail[0].dateSchoolClosed);
    for (let schoolCustomField of this.schoolAddViewModel.schoolMaster.fieldsCategory[this.categoryId].customFields) {
      if (schoolCustomField.type === "Multiple SelectBox" && this.schoolMultiSelectValue !== undefined) {
        schoolCustomField.customFieldsValue[0].customFieldValue = this.schoolMultiSelectValue.toString().replaceAll(",", "|");
      }
    }
    this.schoolService.UpdateSchool(this.schoolAddViewModel).subscribe(data => {
      if (typeof (data) == 'undefined') {
        this.snackbar.open(this.categoryTitle + ' ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (data._failure) {
          this.snackbar.open(this.categoryTitle + ' ' + + data._message, ' ', {
            duration: 10000
          });
        } else {

          this.snackbar.open(this.categoryTitle + " " + 'Updation Successful', '', {
            duration: 10000
          });
          this.schoolCreateMode = this.SchoolCreate.VIEW;
          this.schoolService.changeMessage(true);

        }
      }

    });
  }

  updateStaff() {
    this.staffAddViewModel.selectedCategoryId = this.staffAddViewModel.fieldsCategoryList[this.categoryId].categoryId;
    for (let staffCustomField of this.staffAddViewModel.fieldsCategoryList[this.categoryId].customFields) {
      if (staffCustomField.type === "Multiple SelectBox" && this.staffMultiSelectValue !== undefined) {
        staffCustomField.customFieldsValue[0].customFieldValue = this.staffMultiSelectValue.toString().replaceAll(",", "|");
      }
    }
    this.staffService.updateStaff(this.staffAddViewModel).subscribe(data => {
      if (typeof (data) == 'undefined') {
        this.snackbar.open(this.categoryTitle + ' ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (data._failure) {
          this.snackbar.open(this.categoryTitle + ' ' + + data._message, ' ', {
            duration: 10000
          });
        } else {
          this.snackbar.open(this.categoryTitle + " " + 'Updation Successful', '', {
            duration: 10000
          });
          this.staffCreateMode = this.SchoolCreate.VIEW;
        }
      }

    })
  }


  editOtherInfo() {
    this.schoolCreateMode = this.SchoolCreate.EDIT;
    this.studentCreateMode = this.SchoolCreate.EDIT;
    this.staffCreateMode = this.SchoolCreate.EDIT;
    this.studentService.changePageMode(this.studentCreateMode);
    this.staffService.changePageMode(this.staffCreateMode);
    this.schoolService.changePageMode(this.schoolCreateMode);
    this.formActionButtonTitle = 'update';
  }

  ngOnDestroy() {
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }
}


