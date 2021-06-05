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

import { Component, Input, OnInit, Output, ViewChild } from '@angular/core';
import { NgForm, ControlContainer } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SchoolAddViewModel } from '../../models/school-master.model';
import { SchoolCreate } from '../../../../src/app/enums/school-create.enum';
import { CustomFieldListViewModel, CustomFieldModel } from '../../models/custom-field.model';
import { FieldsCategoryListView } from '../../models/fields-category.model';
import { SharedFunction } from '../../../../src/app/pages/shared/shared-function';
import { CustomFieldService } from '../../../../src/app/services/custom-field.service';
import { SchoolService } from '../../../../src/app/services/school.service';
import { CustomFieldsValueModel } from '../../models/custom-fields-value.model';
import { stagger60ms } from '../../../../src/@vex/animations/stagger.animation';
import { StudentAddModel } from '../../models/student.model';
import { StudentService } from '../../../../src/app/services/student.service';
import { StaffAddModel } from '../../models/staff.model';
import { StaffService } from '../../services/staff.service';

@Component({
  selector: 'vex-custom-field-without-form',
  templateUrl: './custom-field-without-form.component.html',
  styleUrls: ['./custom-field-without-form.component.scss'],
  animations: [stagger60ms],
  viewProviders: [{ provide: ControlContainer, useExisting: NgForm }]
})
export class CustomFieldWithoutFormComponent implements OnInit {
  schoolCreate = SchoolCreate;
  @Input() schoolDetailsForViewAndEdit;
  @Input() categoryId;
  editInfo: boolean = false;
  schoolAddViewModel: SchoolAddViewModel = new SchoolAddViewModel();
  studentAddModel: StudentAddModel = new StudentAddModel();
  staffAddModel: StaffAddModel = new StaffAddModel();
  @Input() schoolCreateMode;
  @Input() studentCreateMode;
  @Input() staffCreateMode;
  @Input() module;
  staffMultiSelectValue;
  studentMultiSelectValue;
  staffCustomFields=[];
  schoolCustomFields=[];
  studentCustomFields=[];
  schoolMultiSelectValue
  customFieldListViewModel = new CustomFieldListViewModel();
  headerTitle: string = "Other Information";
  @ViewChild('f') currentForm: NgForm;
  f: NgForm;
  fieldsCategoryListView = new FieldsCategoryListView();
  constructor(
    private snackbar: MatSnackBar,
    private commonFunction: SharedFunction,
    private customFieldservice: CustomFieldService,
    private schoolService: SchoolService,
    private studentService: StudentService,
    private staffService : StaffService
  ) {
    this.schoolService.getSchoolDetailsForGeneral.subscribe((res: SchoolAddViewModel) => {
      if(res){
        this.schoolAddViewModel = res;
        this.checkCustomValue();
      }
    });
    this.studentService.getStudentDetailsForGeneral.subscribe((res: StudentAddModel) => {
      if(res){
        this.studentAddModel = res;
        if(res?.fieldsCategoryList?.length>0){
          this.checkStudentCustomValue();
        }
      }
    })
    this.staffService.getStaffDetailsForGeneral.subscribe((res: StaffAddModel) => {
      if(res){
        this.staffAddModel = res;
        if(res?.fieldsCategoryList?.length>0){
          this.checkStaffCustomValue();
        }
      }
    })
  }

  ngOnInit(): void {
    if (this.module === 'Student') {
      this.studentAddModel = this.schoolDetailsForViewAndEdit;
      if(this.schoolDetailsForViewAndEdit?.fieldsCategoryList?.length>0){
        this.checkStudentCustomValue();
      }
    }
    else if (this.module === 'School') {
      this.checkNgOnInitCustomValue();
    }
    else if(this.module === 'Staff') {
      this.staffAddModel = this.schoolDetailsForViewAndEdit;
      if(this.schoolDetailsForViewAndEdit?.fieldsCategoryList?.length>0){
        this.checkStaffCustomValue();
      }
    }
  }

  modelChanged(selectValue) {
    if (this.module === 'Staff') {
      this.staffService.setStaffMultiselectValue(selectValue);
    }
    else if (this.module === 'Student') {
      this.studentService.setStudentMultiselectValue(selectValue);
    }
    else if (this.module === 'School') {
      this.schoolService.setSchoolMultiselectValue(selectValue);
    }

}

  checkStudentCustomValue() {
    if (this.studentAddModel?.fieldsCategoryList?.length>0) {
        this.studentCustomFields = this.studentAddModel?.fieldsCategoryList[this.categoryId]?.customFields.filter(x=> !x.systemField);
        if(this.studentCustomFields?.length>0){

        for (let studentCustomField of this.studentCustomFields) {
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

  checkStaffCustomValue(){
      if (this.staffAddModel?.fieldsCategoryList?.length>0) {
        this.staffCustomFields = this.staffAddModel?.fieldsCategoryList[this.categoryId]?.customFields.filter(x=> !x.systemField);
        if(this.staffCustomFields?.length>0 ){

        for (let staffCustomField of this.staffCustomFields) {
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

  checkNgOnInitCustomValue() {

    if (this.schoolDetailsForViewAndEdit !== undefined) {
      if (this.schoolDetailsForViewAndEdit.schoolMaster.fieldsCategory !== undefined) {
        this.schoolAddViewModel = this.schoolDetailsForViewAndEdit;
        if (this.schoolAddViewModel.schoolMaster.fieldsCategory[this.categoryId]?.customFields !== undefined) {

          this.schoolCustomFields= this.schoolAddViewModel.schoolMaster.fieldsCategory[this.categoryId].customFields.filter(x => !x.systemField)
          for (let schoolCustomField of this.schoolCustomFields) {
            if (schoolCustomField.customFieldsValue.length == 0) {

              schoolCustomField.customFieldsValue.push(new CustomFieldsValueModel());

            }
            else {
              if (schoolCustomField?.type === 'Multiple SelectBox') {
                this.schoolMultiSelectValue = schoolCustomField?.customFieldsValue[0].customFieldValue.split('|');
  
              }
            }
          }
        }
      }
    }
  }

  checkCustomValue() {
    if (this.schoolAddViewModel !== undefined) {
      if (this.schoolAddViewModel.schoolMaster.fieldsCategory !== undefined && this.schoolAddViewModel.schoolMaster.fieldsCategory !== null) {

        if (this.schoolAddViewModel.schoolMaster.fieldsCategory[this.categoryId]?.customFields !== undefined) {

          this.schoolCustomFields= this.schoolAddViewModel.schoolMaster.fieldsCategory[this.categoryId].customFields.filter(x => !x.systemField)
          for (let schoolCustomField of this.schoolCustomFields) {
            if (schoolCustomField.customFieldsValue.length == 0) {

              schoolCustomField.customFieldsValue.push(new CustomFieldsValueModel());
            }
            else {
              if (schoolCustomField?.type === 'Multiple SelectBox') {
                this.schoolMultiSelectValue = schoolCustomField?.customFieldsValue[0].customFieldValue.split('|');
  
              }
            }
          }
        }
      }
    }
  }

}
