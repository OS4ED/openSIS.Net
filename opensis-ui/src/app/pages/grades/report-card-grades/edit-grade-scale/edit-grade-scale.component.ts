import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import icClose from '@iconify/icons-ic/twotone-close';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { TranslateService } from '@ngx-translate/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { GradeScaleAddViewModel } from '../../../../models/grades.model';
import { GradesService } from 'src/app/services/grades.service';
import { ValidationService } from 'src/app/pages/shared/validation.service';

@Component({
  selector: 'vex-edit-grade-scale',
  templateUrl: './edit-grade-scale.component.html',
  styleUrls: ['./edit-grade-scale.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class EditGradeScaleComponent implements OnInit {
  icClose = icClose;
  form: FormGroup;
  gradeScaleTitle: string;
  buttonType: string;
  gradeScaleAddViewModel:GradeScaleAddViewModel=new GradeScaleAddViewModel();

  constructor(
    private dialogRef: MatDialogRef<EditGradeScaleComponent>,
    private fb: FormBuilder, 
    @Inject(MAT_DIALOG_DATA) public data:any,
    private snackbar:MatSnackBar,
    private gradesService:GradesService
     ) {
       this.form=fb.group(
         {
          gradeScaleId:[0],
          gradeScaleName:['',[Validators.required,ValidationService.noWhitespaceValidator]],
          gradeScaleValue:['',[Validators.required,ValidationService.noWhitespaceValidator]],
          gradeScaleComment:[],
          calculateGpa:[false],
          useAsStandardGradeScale:[false]

         }
       )
       if(data==null){
        this.gradeScaleTitle="addGradeScale";
        this.buttonType="submit";
      }
      else{
        this.buttonType="update";
        this.gradeScaleTitle="editGradeScale";
        this.form.controls.gradeScaleId.patchValue(this.data.gradeScaleId)
        this.form.controls.gradeScaleName.patchValue(this.data.gradeScaleName)
        this.data.useAsStandardGradeScale?this.onStandardGradeScaleChange(this.data.useAsStandardGradeScale):this.form.controls.gradeScaleValue.patchValue(this.data.gradeScaleValue)
        this.form.controls.gradeScaleComment.patchValue(this.data.gradeScaleComment)
        this.form.controls.calculateGpa.patchValue(this.data.calculateGpa)
        this.form.controls.useAsStandardGradeScale.patchValue(this.data.useAsStandardGradeScale)
      }
  }

  ngOnInit(): void {
  }

  onStandardGradeScaleChange(event){
    if(event){
    this.form.removeControl('gradeScaleValue'); 
    }else{
     this.form.setControl('gradeScaleValue', this.fb.control('', [Validators.required,ValidationService.noWhitespaceValidator])); 
    }

  }

  

  submit(){
    this.form.markAllAsTouched();
    if(this.form.valid){
      if(this.form.controls.gradeScaleId.value==0){
        this.gradeScaleAddViewModel.gradeScale.gradeScaleId=this.form.controls.gradeScaleId.value;
        this.gradeScaleAddViewModel.gradeScale.gradeScaleName=this.form.controls.gradeScaleName.value;
        this.gradeScaleAddViewModel.gradeScale.gradeScaleValue=this.form.controls?.gradeScaleValue?.value;
        this.gradeScaleAddViewModel.gradeScale.gradeScaleComment;
        this.gradeScaleAddViewModel.gradeScale.calculateGpa=this.form.controls.calculateGpa.value;
        this.gradeScaleAddViewModel.gradeScale.useAsStandardGradeScale=this.form.controls.useAsStandardGradeScale.value;
        this.gradesService.addGradeScale(this.gradeScaleAddViewModel).subscribe(
          (res:GradeScaleAddViewModel)=>{
            if(typeof(res)=='undefined'){
              this.snackbar.open('' + sessionStorage.getItem("httpError"), '', {
                duration: 10000
              });
            }
            else{
              if (res._failure) {
                this.snackbar.open('' + res._message, '', {
                  duration: 10000
                });
              } 
              else { 
                this.snackbar.open(''+res._message, '', {
                  duration: 10000
                }); 
                this.dialogRef.close('submited');
              }
            }
          }
        );
      }
      else{
        
        this.gradeScaleAddViewModel.gradeScale.gradeScaleId=this.form.controls.gradeScaleId.value;
        this.gradeScaleAddViewModel.gradeScale.gradeScaleName=this.form.controls.gradeScaleName.value;
        this.gradeScaleAddViewModel.gradeScale.gradeScaleValue=this.form.controls?.gradeScaleValue?.value;
        this.gradeScaleAddViewModel.gradeScale.gradeScaleComment;
        this.gradeScaleAddViewModel.gradeScale.calculateGpa=this.form.controls.calculateGpa.value;
        this.gradeScaleAddViewModel.gradeScale.useAsStandardGradeScale=this.form.controls.useAsStandardGradeScale.value;
        
        this.gradesService.updateGradeScale(this.gradeScaleAddViewModel).subscribe(
          (res:GradeScaleAddViewModel)=>{
            if(typeof(res)=='undefined'){
              this.snackbar.open('' + sessionStorage.getItem("httpError"), '', {
                duration: 10000
              });
            }  
            else{
              if (res._failure) {
                this.snackbar.open(''+res._message, '', {
                  duration: 10000
                });
              } 
              else { 
                this.snackbar.open(''+res._message, '', {
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
