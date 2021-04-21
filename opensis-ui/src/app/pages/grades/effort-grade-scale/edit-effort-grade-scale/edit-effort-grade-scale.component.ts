import { Component, Inject, OnInit, Optional } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import icClose from '@iconify/icons-ic/twotone-close';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { EffortGradeScaleModel } from '../../../../models/grades.model';
import {GradesService} from '../../../../services/grades.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'vex-edit-effort-grade-scale',
  templateUrl: './edit-effort-grade-scale.component.html',
  styleUrls: ['./edit-effort-grade-scale.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class EditEffortGradeScaleComponent implements OnInit {
  icClose = icClose;
  form: FormGroup;
  editMode:boolean;
  effortGradeScaleModel:EffortGradeScaleModel=new EffortGradeScaleModel();
  modalDialogTitle="addNewEffortGradeScale";
  modalActionButton="submit";
  gradeScaleDetailsForEdit;
  constructor(private dialogRef: MatDialogRef<EditEffortGradeScaleComponent>,
    private fb: FormBuilder,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: any,
    private gradesService:GradesService,
    private snackbar: MatSnackBar
    ) {
      if(this.data.editMode){
        this.editMode = this.data.editMode;
        this.gradeScaleDetailsForEdit=this.data.gradeScaleDetails
        this.modalActionButton="update";
        this.modalDialogTitle="updateEffortGradeScale"
       }else{
        this.editMode = this.data.editMode;
       }
     }

  ngOnInit(): void {
    this.form = this.fb.group(
      {
        gradeScaleValue: ['',[Validators.required,Validators.min(1)]],
        gradeScaleComment: ['',Validators.required],
      });
    if(this.editMode){
      this.patchDataToForm();
    }
      
  }

  patchDataToForm(){
    this.form.patchValue({
      gradeScaleValue:this.gradeScaleDetailsForEdit.gradeScaleValue,
      gradeScaleComment:this.gradeScaleDetailsForEdit.gradeScaleComment,
    })
  }

  submit(){
    this.form.markAllAsTouched();
    if( this.form.invalid ){ return; }
     if(this.editMode){
       this.updateEffortGradeScale();
     }else{
       this.addEffortGradeScale();
     }
  }

  updateEffortGradeScale(){
    this.effortGradeScaleModel.effortGradeScale.effortGradeScaleId=this.gradeScaleDetailsForEdit.effortGradeScaleId;
    this.effortGradeScaleModel.effortGradeScale.gradeScaleValue=this.form.value.gradeScaleValue;
    this.effortGradeScaleModel.effortGradeScale.gradeScaleComment=this.form.value.gradeScaleComment;
    this.gradesService.updateEffortGradeScale(this.effortGradeScaleModel).subscribe((res)=>{
     if (typeof (res) == 'undefined') {
       this.snackbar.open('Failed to Update Effort Grade Scale ' + sessionStorage.getItem("httpError"), '', {
         duration: 10000
       });
     }else
     if (res._failure) {
       this.snackbar.open('Failed to Update Effort Grade Scale  ' + res._message, '', {
         duration: 10000
       });
     } else {
       this.snackbar.open('Effort Grade Scale Updated Successfully.', '', {
         duration: 10000
       });
       this.dialogRef.close(true);
     }
    })
  }

  addEffortGradeScale(){
    this.effortGradeScaleModel.effortGradeScale.gradeScaleValue=this.form.value.gradeScaleValue;
     this.effortGradeScaleModel.effortGradeScale.gradeScaleComment=this.form.value.gradeScaleComment;
     this.gradesService.addEffortGradeScale(this.effortGradeScaleModel).subscribe((res)=>{
      if (typeof (res) == 'undefined') {
        this.snackbar.open('Failed to Add Effort Grade Scale ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }else
      if (res._failure) {
        this.snackbar.open('Failed to Add Effort Grade Scale  ' + res._message, '', {
          duration: 10000
        });
      } else {
        this.snackbar.open('Effort Grade Scale Added Successfully.', '', {
          duration: 10000
        });
        this.dialogRef.close(true);
      }
     })
  }

}
