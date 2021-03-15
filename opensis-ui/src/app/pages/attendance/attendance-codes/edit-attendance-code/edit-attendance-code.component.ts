import { Component, Inject, OnInit, Optional } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import icClose from '@iconify/icons-ic/twotone-close';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { AttendanceCodeService } from '../../../../services/attendance-code.service';
import { AttendanceCodeModel } from '../../../../models/attendanceCodeModel';
import { MatSnackBar } from '@angular/material/snack-bar';
import {AttendanceCodeEnum} from '../../../../enums/attendance-code.enum';

@Component({
  selector: 'vex-edit-attendance-code',
  templateUrl: './edit-attendance-code.component.html',
  styleUrls: ['./edit-attendance-code.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class EditAttendanceCodeComponent implements OnInit {
  icClose = icClose;
  form: FormGroup;
  editMode:boolean;
  editDetails;
  selectedAttendanceCategory:number=0;
  attendanceCodeModel:AttendanceCodeModel=new AttendanceCodeModel();
  attendanceCodeModalTitle="addAttendanceCode";
  attendanceCodeModalActionButton="submit";
  attendanceStateCode=AttendanceCodeEnum;
  constructor(private dialogRef: MatDialogRef<EditAttendanceCodeComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: any,
    private attendanceCodeService:AttendanceCodeService,
     private fb: FormBuilder,
     private snackbar: MatSnackBar) {
       if(data.editMode){
         this.editMode=data.editMode;
         this.editDetails=data.editDetails;
       }else{
        this.editMode=data.editMode;
        this.selectedAttendanceCategory=data.attendanceCategoryId;
       }
       this.form = this.fb.group({
        title:['',Validators.required],
        shortName:['',Validators.required],
        sortOrder:['',[Validators.required,Validators.min(1)]],
        allowEntryBy:["null"],
        defaultCode:[false],
        stateCode:["null"],
       });
      }

  ngOnInit(): void {
    if(this.editMode){
      this.attendanceCodeModalTitle="editAttendanceCode";
      this.attendanceCodeModalActionButton="update";
      let modifiedStateCode;
      if(this.editDetails.stateCode==null){
        modifiedStateCode="null"
      }else{
        modifiedStateCode=this.attendanceStateCode[this.editDetails.stateCode];
      }
      if(this.editDetails.allowEntryBy==null){
        this.editDetails.allowEntryBy="null"
      }
    this.form.patchValue({
      title:this.editDetails.title,
      shortName:this.editDetails.shortName,
      sortOrder:this.editDetails.sortOrder,
      allowEntryBy:this.editDetails.allowEntryBy,
      defaultCode:this.editDetails.defaultCode,
      stateCode:modifiedStateCode
    });
  }

  }

  submitAttenndanceCode(){
    this.form.markAllAsTouched();
    if(this.editMode){
      this.updateAttendanceCode();
    }else{
      this.addAttendanceCode();
    }
  }

  addAttendanceCode(){
    if(this.form.valid){
    this.attendanceCodeModel.attendanceCode.schoolId=+sessionStorage.getItem("selectedSchoolId");
    this.attendanceCodeModel.attendanceCode.attendanceCategoryId=this.selectedAttendanceCategory;
    this.attendanceCodeModel.attendanceCode.academicYear=+sessionStorage.getItem("academicyear");
    this.attendanceCodeModel.attendanceCode.title=this.form.value.title;
    this.attendanceCodeModel.attendanceCode.shortName=this.form.value.shortName;
    // this.attendanceCodeModel.attendanceCode.type=this.form.value.type;
    if(this.form.value.stateCode=="null"){
      this.attendanceCodeModel.attendanceCode.stateCode=null;
    }else{
    this.attendanceCodeModel.attendanceCode.stateCode=this.form.value.stateCode;
    }
    if(this.form.value.allowEntryBy=="null"){
      this.attendanceCodeModel.attendanceCode.allowEntryBy=null;
    }else{
      this.attendanceCodeModel.attendanceCode.allowEntryBy=this.form.value.allowEntryBy;
    }
    this.attendanceCodeModel.attendanceCode.defaultCode=this.form.value.defaultCode;
    this.attendanceCodeModel.attendanceCode.sortOrder=this.form.value.sortOrder;
    this.attendanceCodeService.addAttendanceCode(this.attendanceCodeModel).subscribe((res)=>{
      if (typeof (res) == 'undefined') {
        this.snackbar.open('Attendance Code is Failed to Submit!. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }else if (res._failure) {
        this.snackbar.open(res._message, '', {
          duration: 10000
        });
      } else {
        this.snackbar.open('' + res._message, '', {
          duration: 10000
        });
        this.dialogRef.close(true);
      }
    });
  }
  }

  updateAttendanceCode(){
    if(this.form.valid){
      this.attendanceCodeModel.attendanceCode.title=this.form.value.title;
      this.attendanceCodeModel.attendanceCode.shortName=this.form.value.shortName;
      this.attendanceCodeModel.attendanceCode.sortOrder=this.form.value.sortOrder;
        if(this.form.value.stateCode=="null"){
      this.attendanceCodeModel.attendanceCode.stateCode=null;
    }else{
    this.attendanceCodeModel.attendanceCode.stateCode=this.form.value.stateCode;
    }
    if(this.form.value.allowEntryBy=="null"){
      this.attendanceCodeModel.attendanceCode.allowEntryBy=null;
    }else{
      this.attendanceCodeModel.attendanceCode.allowEntryBy=this.form.value.allowEntryBy;
    }
      this.attendanceCodeModel.attendanceCode.defaultCode=this.form.value.defaultCode;
      

      this.attendanceCodeModel.attendanceCode.schoolId=this.editDetails.schoolId;
      this.attendanceCodeModel.attendanceCode.attendanceCode1=this.editDetails.attendanceCode1;
      this.attendanceCodeModel.attendanceCode.academicYear=this.editDetails.academicYear;
      this.attendanceCodeModel.attendanceCode.attendanceCategoryId=this.editDetails.attendanceCategoryId;
      this.attendanceCodeService.updateAttendanceCode(this.attendanceCodeModel).subscribe((res)=>{
        if (typeof (res) == 'undefined') {
          this.snackbar.open('Attendance Code is Failed to Update!. ' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }else if (res._failure) {
          this.snackbar.open(res._message, '', {
            duration: 10000
          });
        } else {
          this.snackbar.open('' + res._message, '', {
            duration: 10000
          });
          this.dialogRef.close(true);
        }
      })
    }
  }

}
