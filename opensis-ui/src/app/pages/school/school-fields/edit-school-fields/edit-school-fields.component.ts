import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import icClose from '@iconify/icons-ic/twotone-close';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CustomFieldService } from '../../../../services/custom-field.service';
import {CustomFieldAddView} from '../../../../models/customFieldModel';
import { CustomFieldOptionsEnum } from '../../../../enums/custom-field-options.enum';
import { FieldCategoryModuleEnum } from '../../../../enums/field-category-module.enum'
import { ValidationService } from '../../../shared/validation.service';

@Component({
  selector: 'vex-edit-school-fields',
  templateUrl: './edit-school-fields.component.html',
  styleUrls: ['./edit-school-fields.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class EditSchoolFieldsComponent implements OnInit {
  icClose = icClose;
  form: FormGroup;
  customFieldTitle;
  buttonType;
  fieldCategoryModule = FieldCategoryModuleEnum;
  customFieldOptionsEnum = Object.keys(CustomFieldOptionsEnum);
  customFieldAddView: CustomFieldAddView = new CustomFieldAddView();
  formfieldcheck = ['Dropdown', 'Editable Dropdown', 'Multiple SelectBox'];
  currentCategoryid;
  constructor(
    private dialogRef: MatDialogRef<EditSchoolFieldsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fb: FormBuilder,
    private snackbar: MatSnackBar,
    private customFieldService: CustomFieldService
    ) {
      this.form = fb.group({
        fieldId: [0],
        title: ['', [ValidationService.noWhitespaceValidator]],
        fieldType: ['', [Validators.required]],
        selectOptions: [''],
        defaultSelection: [, [ValidationService.defaultSelectionValidator]],
        required: [false],
        hide: [false],
        systemField: [false]
      });
      if (data.information === null){
        this.currentCategoryid = data.categoryID;
        this.customFieldTitle = 'addCustomField';
        this.buttonType = 'submit';
      }
      else{
        this.buttonType = 'update';
        this.customFieldTitle = 'editCustomField';
        this.customFieldAddView.customFields = data.information;
        this.form.controls.fieldId.patchValue(data.information.fieldId);
        this.form.controls.title.patchValue(data.information.title);
        this.form.controls.selectOptions.patchValue(data.information.selectOptions.replaceAll('|', '\n'));
        this.form.controls.defaultSelection.patchValue(data.information.defaultSelection);
        this.form.controls.required.patchValue(data.information.required);
        this.form.controls.fieldType.patchValue(data.information.type);
        this.form.controls.hide.patchValue(data.information.hide);
        this.form.controls.systemField.patchValue(data.information.systemField);
      }
     }

  ngOnInit(): void {
  }
  submit(){
    if(this.form.valid){
      if(this.form.controls.fieldId.value==0){
        this.customFieldAddView.customFields.categoryId = this.currentCategoryid;
        this.customFieldAddView.customFields.fieldId = this.form.controls.fieldId.value;
        this.customFieldAddView.customFields.title = this.form.controls.title.value;
        this.customFieldAddView.customFields.selectOptions = this.form.controls.selectOptions.value.replaceAll("\n","|");
        this.customFieldAddView.customFields.defaultSelection = this.form.controls.defaultSelection.value;
        this.customFieldAddView.customFields.required = this.form.controls.required.value;
        this.customFieldAddView.customFields.hide = this.form.controls.hide.value;
        this.customFieldAddView.customFields.systemField = this.form.controls.systemField.value;
        this.customFieldAddView.customFields.type = this.form.controls.fieldType.value;
        this.customFieldAddView.customFields.module = this.fieldCategoryModule.School;
        this.customFieldService.addCustomField(this.customFieldAddView).subscribe(
          (res: CustomFieldAddView) => {
            if (typeof(res) === 'undefined'){
              this.snackbar.open('School field failed. ' + sessionStorage.getItem('httpError'), '', {
                duration: 10000
              });
            }
            else{
              if (res._failure) {
                this.snackbar.open( res._message, '', {
                  duration: 10000
                });
              }
              else {
                this.snackbar.open( res._message , '', {
                  duration: 10000
                });
                this.dialogRef.close('submited');
              }
            }
          }
        );
      }
      else{
        this.customFieldAddView.customFields.fieldId = this.form.controls.fieldId.value;
        this.customFieldAddView.customFields.title = this.form.controls.title.value;
        this.customFieldAddView.customFields.type = this.form.controls.fieldType.value;
        if ((this.form.controls.fieldType.value === this.formfieldcheck[0]) ||
        (this.form.controls.fieldType.value === this.formfieldcheck[1]) ||
         (this.form.controls.fieldType.value === this.formfieldcheck[2])){
            this.customFieldAddView.customFields.selectOptions = this.form.controls.selectOptions.value.replaceAll('\n', '|');
          }
          else{
            this.customFieldAddView.customFields.selectOptions = '';
          }
        this.customFieldAddView.customFields.defaultSelection = this.form.controls.defaultSelection.value;
        this.customFieldAddView.customFields.required = this.form.controls.required.value;
        this.customFieldAddView.customFields.hide = this.form.controls.hide.value;
        this.customFieldAddView.customFields.systemField = this.form.controls.systemField.value;
        this.customFieldService.updateCustomField(this.customFieldAddView).subscribe(
          (res: CustomFieldAddView) => {
            if (typeof(res) === 'undefined'){
              this.snackbar.open('School field failed. ' + sessionStorage.getItem('httpError'), '', {
                duration: 10000
              });
            }
            else{
              if (res._failure) {
                this.snackbar.open('School field failed. ' + res._message, '', {
                  duration: 10000
                });
              }
              else {
                this.snackbar.open('School field Successful Edited.', '', {
                  duration: 10000
                });
                this.dialogRef.close('submited');
              }
            }
          }
        );
      }
    }
  }
}
