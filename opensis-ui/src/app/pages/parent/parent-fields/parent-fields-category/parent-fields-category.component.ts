import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import icClose from '@iconify/icons-ic/twotone-close';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CustomFieldService } from '../../../../services/custom-field.service';
import {FieldsCategoryAddView} from '../../../../models/fields-category.model';
import {FieldCategoryModuleEnum} from '../../../../enums/field-category-module.enum'

@Component({
  selector: 'vex-parent-fields-category',
  templateUrl: './parent-fields-category.component.html',
  styleUrls: ['./parent-fields-category.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class ParentFieldsCategoryComponent implements OnInit {
  icClose = icClose;
  form: FormGroup;
  FieldCategoryTitle: string;
  buttonType: string;
  fieldsCategoryAddView:FieldsCategoryAddView=new FieldsCategoryAddView()
  fieldCategoryModuleEnum=FieldCategoryModuleEnum

  constructor(
    private dialogRef: MatDialogRef<ParentFieldsCategoryComponent>, 
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data:any,
    private snackbar:MatSnackBar,
    private customFieldService:CustomFieldService
    ) { 
      this.form= fb.group({
        categoryId:[0],
        title:['',[Validators.required]],
        sortOrder:['',[Validators.required,Validators.min(1)]]
      });
      if(data==null){
        this.FieldCategoryTitle="addFieldCategory";
        this.buttonType="submit";
      }
      else{
        this.FieldCategoryTitle="editFieldCategory";
        this.buttonType="update";
        this.fieldsCategoryAddView.fieldsCategory=data
        this.form.controls.categoryId.patchValue(data.categoryId)
        this.form.controls.title.patchValue(data.title)
        this.form.controls.sortOrder.patchValue(data.sortOrder)
      }
    }

  ngOnInit(): void {
  }
  submit(){
    if(this.form.valid){
      if(this.form.controls.categoryId.value==0){
        this.fieldsCategoryAddView.fieldsCategory.title=this.form.controls.title.value;
        this.fieldsCategoryAddView.fieldsCategory.sortOrder=this.form.controls.sortOrder.value;
        this.fieldsCategoryAddView.fieldsCategory.module=this.fieldCategoryModuleEnum.Parent
        this.customFieldService.addFieldsCategory(this.fieldsCategoryAddView).subscribe(
          (res:FieldsCategoryAddView)=>{
            if(typeof(res)=='undefined'){
              this.snackbar.open('field category failed. ' + sessionStorage.getItem("httpError"), '', {
                duration: 10000
              });
            }
            else{
              if (res._failure) {
                this.snackbar.open('field category failed. ' + res._message, '', {
                  duration: 10000
                });
              } 
              else { 
                this.snackbar.open('field category Successful Created.', '', {
                  duration: 10000
                }); 
                this.dialogRef.close('submited');
              }
            }
          }
        )

      }
      else{
        this.fieldsCategoryAddView.fieldsCategory.categoryId=this.form.controls.categoryId.value;
        this.fieldsCategoryAddView.fieldsCategory.title=this.form.controls.title.value;
        this.fieldsCategoryAddView.fieldsCategory.sortOrder=this.form.controls.sortOrder.value;
        this.customFieldService.updateFieldsCategory(this.fieldsCategoryAddView).subscribe(
          (res: FieldsCategoryAddView) => {
            if(typeof(res)=='undefined'){
              this.snackbar.open('field category failed. ' + sessionStorage.getItem("httpError"), '', {
                duration: 10000
              });
            }
            else{
              if (res._failure) {
                this.snackbar.open('field category failed. ' + res._message, '', {
                  duration: 10000
                });
              }
              else { 
                this.snackbar.open('field category Successful Edited.', '', {
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
