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

import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import icClose from '@iconify/icons-ic/twotone-close';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CustomFieldService } from '../../../../services/custom-field.service';
import {CustomFieldAddView} from '../../../../models/custom-field.model';
import { CustomFieldOptionsEnum } from '../../../../enums/custom-field-options.enum';
import { FieldCategoryModuleEnum } from '../../../../enums/field-category-module.enum'
import { ValidationService } from '../../../shared/validation.service';

@Component({
  selector: 'vex-edit-parent-fields',
  templateUrl: './edit-parent-fields.component.html',
  styleUrls: ['./edit-parent-fields.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class EditParentFieldsComponent implements OnInit {
  icClose = icClose;
  form: FormGroup;
  customFieldTitle;
  buttonType;
  fieldCategoryModule=FieldCategoryModuleEnum
  customFieldOptionsEnum=Object.keys(CustomFieldOptionsEnum)
  customFieldAddView:CustomFieldAddView= new CustomFieldAddView()
  formfieldcheck=['Dropdown','Editable Dropdown','Multiple SelectBox']
  currentCategoryid;

  constructor(
    private dialogRef: MatDialogRef<EditParentFieldsComponent>, 
    private fb: FormBuilder, 
    @Inject(MAT_DIALOG_DATA) public data:any,
    private snackbar:MatSnackBar,
    private customFieldService:CustomFieldService
    ) {
      this.form=fb.group({
        fieldId:[0],
        title:['',[Validators.required]],
        fieldType:[],
        selectOptions:[''],
        defaultSelection:[,[ValidationService.defaultSelectionValidator]],
        sortOrder:[,[Validators.required,Validators.min(1)]],
        required:[false],
        hide:[false],
        systemField:[false]
      })
      if(data.information==null){
        this.currentCategoryid=data.categoryID
        this.customFieldTitle="addCustomField";
        this.buttonType="submit";
      }
      else{
        this.buttonType="update";
        this.customFieldTitle="editCustomField";
        this.customFieldAddView.customFields=data.information
        this.form.controls.fieldId.patchValue(data.information.fieldId)
        this.form.controls.title.patchValue(data.information.title)
        this.form.controls.selectOptions.patchValue(data.information.selectOptions.replaceAll("|","\n"))
        this.form.controls.defaultSelection.patchValue(data.information.defaultSelection)
        this.form.controls.sortOrder.patchValue(data.information.sortOrder)
        this.form.controls.required.patchValue(data.information.required)
        this.form.controls.fieldType.patchValue(data.information.type)
        this.form.controls.hide.patchValue(data.information.hide)
        this.form.controls.systemField.patchValue(data.information.systemField)
      }
     }

  ngOnInit(): void {
  }
  submit(){
    if(this.form.valid){
      if(this.form.controls.fieldId.value==0){
        this.customFieldAddView.customFields.categoryId=this.currentCategoryid;
        this.customFieldAddView.customFields.fieldId=this.form.controls.fieldId.value;
        this.customFieldAddView.customFields.title=this.form.controls.title.value;
        this.customFieldAddView.customFields.selectOptions=this.form.controls.selectOptions.value.replaceAll("\n","|");
        this.customFieldAddView.customFields.defaultSelection=this.form.controls.defaultSelection.value;
        this.customFieldAddView.customFields.sortOrder=this.form.controls.sortOrder.value;
        this.customFieldAddView.customFields.required=this.form.controls.required.value;
        this.customFieldAddView.customFields.hide=this.form.controls.hide.value;
        this.customFieldAddView.customFields.systemField=this.form.controls.systemField.value;
        this.customFieldAddView.customFields.type=this.form.controls.fieldType.value;
        this.customFieldAddView.customFields.module=this.fieldCategoryModule.Parent;
        //this.customFieldAddView.customFields.type="Custom";
         this.customFieldService.addCustomField(this.customFieldAddView).subscribe(
          (res:CustomFieldAddView)=>{
            if(typeof(res)=='undefined'){
              this.snackbar.open('School field failed. ' + sessionStorage.getItem("httpError"), '', {
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
                this.snackbar.open('School field Successful Created.', '', {
                  duration: 10000
                }); 
                this.dialogRef.close('submited');
              }
            }
          }
        ); 
      }
      else{
        this.customFieldAddView.customFields.fieldId=this.form.controls.fieldId.value;
        this.customFieldAddView.customFields.title=this.form.controls.title.value;
        this.customFieldAddView.customFields.type=this.form.controls.fieldType.value;
        if((this.form.controls.fieldType.value===this.formfieldcheck[0])||(this.form.controls.fieldType.value===this.formfieldcheck[1])||(this.form.controls.fieldType.value===this.formfieldcheck[2])){
            this.customFieldAddView.customFields.selectOptions=this.form.controls.selectOptions.value.replaceAll("\n","|");
          }
          else{
            this.customFieldAddView.customFields.selectOptions="";
          }
        this.customFieldAddView.customFields.defaultSelection=this.form.controls.defaultSelection.value;
        this.customFieldAddView.customFields.sortOrder=this.form.controls.sortOrder.value;
        this.customFieldAddView.customFields.required=this.form.controls.required.value;
        this.customFieldAddView.customFields.hide=this.form.controls.hide.value;
        this.customFieldAddView.customFields.systemField=this.form.controls.systemField.value;
        this.customFieldService.updateCustomField(this.customFieldAddView).subscribe(
          (res: CustomFieldAddView)=>{
            if(typeof(res)=='undefined'){
              this.snackbar.open('School field failed. ' + sessionStorage.getItem("httpError"), '', {
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
