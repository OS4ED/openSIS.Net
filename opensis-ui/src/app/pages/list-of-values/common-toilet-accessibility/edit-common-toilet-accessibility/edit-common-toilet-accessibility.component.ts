import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import icClose from '@iconify/icons-ic/twotone-close';
import { LovAddView } from '../../../../models/lov.model';
import { ValidationService } from '../../../shared/validation.service';
import { CommonService } from '../../../../services/common.service';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';

@Component({
  selector: 'vex-edit-common-toilet-accessibility',
  templateUrl: './edit-common-toilet-accessibility.component.html',
  styleUrls: ['./edit-common-toilet-accessibility.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class EditCommonToiletAccessibilityComponent implements OnInit {
  icClose = icClose;
  form:FormGroup
  editmod;
  commonToiletAccessibilityTitle: string;
  buttonType: string;
  lovAddView:LovAddView=new LovAddView();

  constructor(
    private dialogRef: MatDialogRef<EditCommonToiletAccessibilityComponent>,
    @Inject(MAT_DIALOG_DATA) public data:any,
    private snackbar:MatSnackBar,
    private commonService:CommonService,
    fb:FormBuilder
    ) { 
      this.editmod=data.mod
      this.form=fb.group({
        id:[0],
        lovName:["Common Toilet Accessibility"],
        lovColumnValue:['',[ValidationService.noWhitespaceValidator]],
      })
    }

  ngOnInit(): void {
    if(this.editmod===1){
      this.commonToiletAccessibilityTitle="editCommonToiletAccessibility";
      this.buttonType="update";
      this.patchDataToForm();
    }
    else{
      this.commonToiletAccessibilityTitle="addNewCommonToiletAccessibility";
      this.buttonType="submit";
    }
  }
  patchDataToForm() {
    this.form.controls.id.patchValue(this.data.element.id)
    this.form.controls.lovName.patchValue(this.data.element.lovName)
    this.form.controls.lovColumnValue.patchValue(this.data.element.lovColumnValue)
  }
  submit(){
    this.form.markAllAsTouched();
    if(this.form.valid){
      if(this.editmod==0){
        this.addCommonToiletAccessibility()
      }
      else{
        this.editCommonToiletAccessibility()
      }
    }
  }
  editCommonToiletAccessibility() {
    this.lovAddView.dropdownValue.id=this.form.controls.id.value;
    this.lovAddView.dropdownValue.lovName=this.form.controls.lovName.value;
    this.lovAddView.dropdownValue.lovColumnValue=this.form.controls.lovColumnValue.value;
    this.lovAddView.dropdownValue.updatedBy=sessionStorage.getItem("email");
    this.commonService.updateDropdownValue(this.lovAddView).subscribe(
      (res:LovAddView)=>{
        if(typeof(res)=='undefined'){
          this.snackbar.open( sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else{
          if (res._failure) {
            this.snackbar.open( res._message, '', {
              duration: 10000
            });
          } 
          else{
            this.snackbar.open( res._message, '', {
              duration: 10000
            });
            this.dialogRef.close('submited');
          }
        }
      }
    );
  }
  addCommonToiletAccessibility() {
    this.lovAddView.dropdownValue.lovColumnValue=this.form.controls.lovColumnValue.value;
    this.lovAddView.dropdownValue.lovName=this.form.controls.lovName.value;
    this.lovAddView.dropdownValue.createdBy=sessionStorage.getItem("email");
    this.commonService.addDropdownValue(this.lovAddView).subscribe(
      (res:LovAddView)=>{
        if(typeof(res)=='undefined'){
          this.snackbar.open( sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else{
          if (res._failure) {
            this.snackbar.open( res._message, '', {
              duration: 10000
            });
          } 
          else{
            this.snackbar.open( res._message, '', {
              duration: 10000
            });
            this.dialogRef.close('submited');
          }
        }
      }
    );
  }

}
