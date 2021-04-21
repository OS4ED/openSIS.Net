import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import icClose from '@iconify/icons-ic/twotone-close';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { MatSnackBar } from '@angular/material/snack-bar';
import { EnrollmentCodesService } from '../../../../services/enrollment-codes.service';
import {EnrollmentCodeAddView} from '../../../../models/enrollment-code.model';
import {EnrollmentCodeEnum } from '../../../../enums/enrollment_code.enum';

@Component({
  selector: 'vex-edit-enrollment-code',
  templateUrl: './edit-enrollment-code.component.html',
  styleUrls: ['./edit-enrollment-code.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class EditEnrollmentCodeComponent implements OnInit {
  icClose = icClose;
  form: FormGroup;
  enrollmentCodeTitle;
  buttonType;
  enrollmentCodeAddView:EnrollmentCodeAddView= new EnrollmentCodeAddView();
  enrollmentCodeEnum=Object.keys(EnrollmentCodeEnum)
  constructor(
    private dialogRef: MatDialogRef<EditEnrollmentCodeComponent>,
    @Inject(MAT_DIALOG_DATA) public data:any,
    private snackbar:MatSnackBar,
     private fb: FormBuilder,
     private enrollmentCodeService:EnrollmentCodesService
     ) {
    this.form= fb.group({
      enrollmentCode:[0],
      title:['',[Validators.required]],
      shortName:['',[Validators.required]],
      sortOrder:['',[Validators.required,Validators.min(1)]],
      type:['',[]]
    });
  
   }

  ngOnInit(): void {
    if(this.data==null){
      this.enrollmentCodeTitle="addEnrollmentCode";
      this.buttonType="SUBMIT";
    }
    else{
      this.enrollmentCodeTitle="editEnrollmentCode";
      this.buttonType="UPDATE";
      this.form.controls.enrollmentCode.patchValue(this.data.enrollmentCode)
      this.form.controls.title.patchValue(this.data.title)
      this.form.controls.shortName.patchValue(this.data.shortName)
      this.form.controls.sortOrder.patchValue(this.data.sortOrder)
      this.form.controls.type.patchValue(this.data.type)
    }
  }
  submit(){
    this.form.markAllAsTouched();
    if (this.form.valid) {
    if(this.form.controls.enrollmentCode.value===0){
      this.enrollmentCodeAddView.studentEnrollmentCode.title=this.form.controls.title.value
      this.enrollmentCodeAddView.studentEnrollmentCode.shortName=this.form.controls.shortName.value
      this.enrollmentCodeAddView.studentEnrollmentCode.sortOrder=this.form.controls.sortOrder.value
      this.enrollmentCodeAddView.studentEnrollmentCode.type=this.form.controls.type.value   
      this.enrollmentCodeService.addStudentEnrollmentCode(this.enrollmentCodeAddView).subscribe(
        (res:EnrollmentCodeAddView)=>{

          if(typeof(res)=='undefined'){
            this.snackbar.open('Enrollment code failed. ' + sessionStorage.getItem("httpError"), '', {
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
              this.snackbar.open( res._message, '', {
                duration: 10000
              }); 
              this.dialogRef.close('submited');
            }
          
          }
        }
      ) 
    }
    else{
      this.enrollmentCodeAddView.studentEnrollmentCode.enrollmentCode=this.form.controls.enrollmentCode.value
      this.enrollmentCodeAddView.studentEnrollmentCode.title=this.form.controls.title.value
      this.enrollmentCodeAddView.studentEnrollmentCode.shortName=this.form.controls.shortName.value
      this.enrollmentCodeAddView.studentEnrollmentCode.sortOrder=this.form.controls.sortOrder.value
      this.enrollmentCodeAddView.studentEnrollmentCode.type=this.form.controls.type.value
      this.enrollmentCodeService.updateStudentEnrollmentCode(this.enrollmentCodeAddView).subscribe(
        (res:EnrollmentCodeAddView)=>{
          if(typeof(res)=='undefined'){
            this.snackbar.open('Enrollment code failed. ' + sessionStorage.getItem("httpError"), '', {
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
              this.snackbar.open( res._message, '', {
                duration: 10000
              }); 
              this.dialogRef.close('submited');
            }
          
          }
        }
      )
    }
  }
  }
}
