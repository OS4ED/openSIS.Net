import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SharedFunction } from '../../../../src/app/pages/shared/shared-function';
import { FieldsCategoryListView, FieldsCategoryModel } from '../../../../src/app/models/fieldsCategoryModel';
import { CustomFieldService } from '../../../../src/app/services/custom-field.service';
import { CustomFieldListViewModel, CustomFieldModel } from '../../../../src/app/models/customFieldModel';
import icAdd from '@iconify/icons-ic/baseline-add';
import icEdit from '@iconify/icons-ic/twotone-edit';
import { NgForm } from '@angular/forms';
import { stagger60ms } from '../../../../src/@vex/animations/stagger.animation';
import { fadeInUp400ms } from '../../../../src/@vex/animations/fade-in-up.animation';
import { fadeInRight400ms } from '../../../../src/@vex/animations/fade-in-right.animation';
import { SchoolCreate } from '../../../../src/app/enums/school-create.enum';
import { SchoolAddViewModel } from '../../../../src/app/models/schoolMasterModel';
import { SchoolService } from '../../../../src/app/services/school.service';
import { CustomFieldsValueModel } from '../../../../src/app/models/customFieldsValueModel';
import { Router } from '@angular/router';
import { StudentAddModel } from '../../../../src/app/models/studentModel';
import { StudentService } from '../../../../src/app/services/student.service';
import { StaffAddModel } from '../../models/staffModel';
import { StaffService } from '../../services/staff.service';

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
export class CustomFieldComponent implements OnInit {
  SchoolCreate = SchoolCreate;
  @Input() schoolCreateMode;
  @Input() studentCreateMode;
  @Input() staffCreateMode;
  @Input() categoryTitle;
  @Input() categoryId;
  @Input() schoolDetailsForViewAndEdit;
  @Input() module;
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
  f: NgForm;
  formActionButtonTitle = "update";
  constructor(
    private snackbar: MatSnackBar,
    private commonFunction: SharedFunction,
    private studentService: StudentService,
    private schoolService: SchoolService,
    private staffService: StaffService,
    private router: Router,
  ) {
    if (this.module === 'School') {
      this.schoolService.getSchoolDetailsForGeneral.subscribe((res: SchoolAddViewModel) => {
        this.schoolAddViewModel = res;
        this.checkCustomValue();
      });
    }
  }

  ngOnInit(): void {
    if (this.module === 'Student') {
      this.studentAddViewModel = this.schoolDetailsForViewAndEdit;
      this.checkStudentCustomValue();

    }
    else if (this.module === 'School') {
      this.checkNgOnInitCustomValue();
    }
    else if (this.module === 'Staff') {
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

  checkNgOnInitCustomValue() {

    if (this.schoolDetailsForViewAndEdit !== undefined) {
      let catId = this.categoryId;
      if (this.schoolDetailsForViewAndEdit.schoolMaster.fieldsCategory !== undefined) {
        this.schoolAddViewModel = this.schoolDetailsForViewAndEdit;
        for (let customField of this.schoolAddViewModel.schoolMaster.fieldsCategory[this.categoryId].customFields) {
          if (customField.customFieldsValue.length == 0) {
            customField.customFieldsValue.push(new CustomFieldsValueModel());
          }
          else {
            if (customField.type === 'Multiple SelectBox') {
              this.schoolMultiSelectValue = customField.customFieldsValue[0].customFieldValue.split('|');

            }
          }
        }
      }

    }
    
  }


  checkStudentCustomValue() {
    if (this.studentAddViewModel !== undefined) {
      let catId = this.categoryId;
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
      let catId = this.categoryId;
      if (this.staffAddViewModel.fieldsCategoryList !== undefined) {

        for (let staffCustomField of this.staffAddViewModel.fieldsCategoryList[this.categoryId]?.customFields) {
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

    if (this.schoolAddViewModel !== undefined) {
      let catId = this.categoryId;
      if (this.schoolAddViewModel.schoolMaster.fieldsCategory !== undefined) {

        for (let customField of this.schoolAddViewModel.schoolMaster.fieldsCategory[this.categoryId].customFields) {
          if (customField.customFieldsValue.length == 0) {

            customField.customFieldsValue.push(new CustomFieldsValueModel());
          }
        }

      }
    }

  }
  updateStudent() {
    this.studentAddViewModel.selectedCategoryId = this.studentAddViewModel.fieldsCategoryList[this.categoryId].categoryId;
    this.studentAddViewModel._tenantName = sessionStorage.getItem("tenant");
    this.studentAddViewModel._token = sessionStorage.getItem("token");
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
    this.staffAddViewModel._token = sessionStorage.getItem("token");
    this.staffAddViewModel._tenantName = sessionStorage.getItem("tenant");
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
}
