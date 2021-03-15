import { Component, Inject, OnInit, Optional } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import icClose from '@iconify/icons-ic/twotone-close';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import {AddGradeLevelModel, GelAllGradeEquivalencyModel} from '../../../../models/gradeLevelModel';
import { GradeLevelService } from '../../../../services/grade-level.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AgeRangeList, EducationalStage } from '../../../../models/common.model';

@Component({
  selector: 'vex-edit-grade-levels',
  templateUrl: './edit-grade-levels.component.html',
  styleUrls: ['./edit-grade-levels.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class EditGradeLevelsComponent implements OnInit {
  icClose = icClose;
  form: FormGroup;
  addGradeLevelData:AddGradeLevelModel = new AddGradeLevelModel();
  updateGradeLevelData:AddGradeLevelModel = new AddGradeLevelModel();
  getGradeEquivalencyList:GelAllGradeEquivalencyModel = new GelAllGradeEquivalencyModel();
  ageRangeList:AgeRangeList = new AgeRangeList();
  educationStageList:EducationalStage = new EducationalStage();
  nextGradeLevelList:[];
  editMode:boolean;
  modalTitle="addGradeLevel";
  modalActionButtonTitle="submit";
  editDetails;
  nextGradeIdInString:string;
  constructor(private dialogRef: MatDialogRef<EditGradeLevelsComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: any,
     private fb: FormBuilder,
     private gradeLevelService:GradeLevelService,
     private snackbar: MatSnackBar) {
    }

  ngOnInit(): void {
      this.getGradeEquivalencyList=this.data.gradeLevelEquivalencyList;
      this.ageRangeList=this.data.ageRangeList;
      this.educationStageList=this.data.educationalStage;
      if(this.data.editMode){
      this.editMode = this.data.editMode;
      this.editDetails = this.data.editDetails;
      this.nextGradeLevelList=this.data.gradeLevels;
     }else{
       this.nextGradeLevelList=this.data.gradeLevels;
     }
    
      this.form = this.fb.group(
      {
        title: ['',Validators.required],
        shortName: ['',Validators.required],
        sortOrder: ['',[Validators.required,Validators.min(1)]],
        equivalencyId:["null"],
        ageRangeId:["null"],
        iscedCode:["null"],
        nextGradeId: ["null"],
      });
      
    if(this.editMode){
      this.modalTitle="editGradeLevel"
      this.modalActionButtonTitle="update";
      this.callGradeLevelView();
    }
  }

  callGradeLevelView(){
      this.form.patchValue({
        title:this.editDetails.title,
        shortName:this.editDetails.shortName,
        sortOrder:this.editDetails.sortOrder,
        equivalencyId:this.editDetails.equivalencyId?this.editDetails.equivalencyId.toString():'null',
        ageRangeId:this.editDetails.ageRangeId?this.editDetails.ageRangeId.toString():'null',
        iscedCode:this.editDetails.iscedCode?this.editDetails.iscedCode.toString():'null',
        nextGradeId:this.editDetails.nextGradeId?this.editDetails.nextGradeId.toString():'null'
      })
   
  }

  submit(){
    this.form.markAllAsTouched();
    if(this.editMode){
      this.updateGradeLevel();
    }else{
      if(this.form.valid){
        this.addGradeLevel();
      }
    }
  }

  getGradeEquivalency(){
    this.gradeLevelService.getAllGradeEquivalency(this.getGradeEquivalencyList).subscribe((res)=>{
      this.getGradeEquivalencyList=res;
    })
  }

  addGradeLevel(){
    this.addGradeLevelData.tblGradelevel.schoolId=+sessionStorage.getItem("selectedSchoolId");;
    this.addGradeLevelData.tblGradelevel.title = this.form.value.title;
    this.addGradeLevelData.tblGradelevel.shortName =this.form.value.shortName;
    this.addGradeLevelData.tblGradelevel.sortOrder=this.form.value.sortOrder;

    this.form.value.equivalencyId=='null'?
    this.addGradeLevelData.tblGradelevel.equivalencyId = null:this.addGradeLevelData.tblGradelevel.equivalencyId = this.form.value.equivalencyId

    this.form.value.ageRangeId=='null'?
    this.addGradeLevelData.tblGradelevel.ageRangeId = null:this.addGradeLevelData.tblGradelevel.ageRangeId = +this.form.value.ageRangeId

    this.form.value.iscedCode=='null'?
    this.addGradeLevelData.tblGradelevel.iscedCode = null:this.addGradeLevelData.tblGradelevel.iscedCode = +this.form.value.iscedCode
   
    this.form.value.nextGradeId=='null'?
    this.addGradeLevelData.tblGradelevel.nextGradeId = null:this.addGradeLevelData.tblGradelevel.nextGradeId = this.form.value.nextGradeId
   
  
    this.addGradeLevelData._token=sessionStorage.getItem("token");
    this.addGradeLevelData._tenantName=sessionStorage.getItem("tenant");
    this.gradeLevelService.addGradelevel(this.addGradeLevelData).subscribe((res)=>{
      if (typeof (res) == 'undefined') {
        this.snackbar.open('Failed to Add Grade Level ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }else
      if (res._failure) {
        this.snackbar.open( res._message, '', {
          duration: 10000
        });
      } else {
        this.snackbar.open(res._message, '', {
          duration: 10000
        });
        this.dialogRef.close(true);
      }
    })
  }

  updateGradeLevel(){
    this.updateGradeLevelData.tblGradelevel.schoolId = this.editDetails.schoolId;
    this.updateGradeLevelData.tblGradelevel.gradeId = this.editDetails.gradeId;
    this.updateGradeLevelData._tenantName=sessionStorage.getItem("tenant");
    this.updateGradeLevelData._token=sessionStorage.getItem("token");
    this.updateGradeLevelData.tblGradelevel.title=this.form.value.title;
    this.updateGradeLevelData.tblGradelevel.shortName=this.form.value.shortName;
    this.updateGradeLevelData.tblGradelevel.sortOrder=this.form.value.sortOrder;
    this.form.value.equivalencyId=='null'?
    this.updateGradeLevelData.tblGradelevel.equivalencyId = null:this.updateGradeLevelData.tblGradelevel.equivalencyId = this.form.value.equivalencyId;

    this.form.value.ageRangeId=='null'?
    this.updateGradeLevelData.tblGradelevel.ageRangeId = null:this.updateGradeLevelData.tblGradelevel.ageRangeId = +this.form.value.ageRangeId;

    this.form.value.iscedCode=='null'?
    this.updateGradeLevelData.tblGradelevel.iscedCode = null:this.updateGradeLevelData.tblGradelevel.iscedCode = +this.form.value.iscedCode;
   
    this.form.value.nextGradeId=='null'?
    this.updateGradeLevelData.tblGradelevel.nextGradeId = null:this.updateGradeLevelData.tblGradelevel.nextGradeId = this.form.value.nextGradeId;

    this.gradeLevelService.updateGradelevel(this.updateGradeLevelData).subscribe((res)=>{
      if (typeof (res) == 'undefined') {
        this.snackbar.open('Failed to Update Grade Level ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }else if(res._failure) {
        this.snackbar.open( res._message, '', {
          duration: 10000
        });
      } else {
        this.snackbar.open(res._message, '', {
          duration: 10000
        }
        );
        this.dialogRef.close(true);
      }
    })
  }

 

}
