import { Component, OnInit,Inject } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { FormBuilder,FormGroup, Validators } from '@angular/forms';
import icClose from '@iconify/icons-ic/twotone-close';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { SectionAddModel } from '../../../../models/section.model';
import { SectionService } from '../../../../services/section.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { GetAllSectionModel } from '../../../../models/section.model';
import { Router } from '@angular/router';
import {MAT_DIALOG_DATA} from '@angular/material/dialog';
@Component({
  selector: 'vex-edit-section',
  templateUrl: './edit-section.component.html',
  styleUrls: ['./edit-section.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class EditSectionComponent implements OnInit {

  icClose = icClose;
  form: FormGroup;
  sectionAddModel: SectionAddModel = new SectionAddModel();
  getAllSection: GetAllSectionModel = new GetAllSectionModel();
  sectionModalTitle="addSection";
  ssectionModalActionTitle="submit";
  isEdit=false;
  constructor(private dialogRef: MatDialogRef<EditSectionComponent>, private fb: FormBuilder,
    private sectionService:SectionService,
    private snackbar: MatSnackBar,
    private router:Router,
    @Inject(MAT_DIALOG_DATA) public data) { }

  ngOnInit(): void {
    this.form = this.fb.group(
      {
        title: ['', Validators.required],
        sortOrder: ['',[Validators.required,Validators.min(1)]],
       
      });

      if (this.data && (Object.keys(this.data).length !== 0 || Object.keys(this.data).length > 0) ){
        this.sectionModalTitle="editSection";
        this.ssectionModalActionTitle="update"       
        this.sectionAddModel.tableSections.name=this.data.editDetails.name;
        this.sectionAddModel.tableSections.sortOrder=this.data.editDetails.sortOrder;
      }
  }
  get f() {
    return this.form.controls;
  }

  closeDialog(){
    this.dialogRef.close(false);
  }
  
  submit() {
    
    if (this.form.valid) {
        this.sectionAddModel.tableSections.schoolId=+sessionStorage.getItem("selectedSchoolId");
      if (this.data && (Object.keys(this.data).length !== 0 || Object.keys(this.data).length > 0) ){

        this.sectionAddModel.tableSections.sectionId = this.data.editDetails.sectionId;
        this.sectionService.UpdateSection(this.sectionAddModel).subscribe(data => {
          if (typeof (data) == 'undefined') {
            this.snackbar.open('Section Updation failed. ' + sessionStorage.getItem("httpError"), '', {
              duration: 10000
            });
          }
          else {
            if (data._failure) {
              this.snackbar.open('Section Updation failed. ' + data._message, '', {
                duration: 10000
              });
            } else {
              
              this.snackbar.open('Section Updation Successful.', '', {
                duration: 10000
              });
              
              this.dialogRef.close(true);            
            }
          }
  
        })
      }else{
        this.sectionService.SaveSection(this.sectionAddModel).subscribe(data => {
          if (typeof (data) == 'undefined') {
            this.snackbar.open('Section Submission failed. ' + sessionStorage.getItem("httpError"), '', {
              duration: 10000
            });
          }
          else {
            if (data._failure) {
              this.snackbar.open('Section Submission failed. ' + data._message, '', {
                duration: 10000
              });
            } else {
              this.snackbar.open('Section Submission Successful.', '', {
                duration: 10000
              });              
              this.dialogRef.close(true);
            }
          }
  
        })

      }
     

      }
    }
    

}
