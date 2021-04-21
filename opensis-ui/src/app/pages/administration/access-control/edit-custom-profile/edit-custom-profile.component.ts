import { Component, OnInit,Inject  } from '@angular/core';
import { MatDialogRef,MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder,FormGroup, Validators } from '@angular/forms';
import icClose from '@iconify/icons-ic/twotone-close';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import {CountryAddModel} from '../../../../models/country.model';
import {CommonService} from '../../../../services/common.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AddMembershipModel, GetAllMembersList} from '../../../../models/membership.model';
import { MembershipService } from '../../../../services/membership.service';
@Component({
  selector: 'vex-edit-custom-profile',
  templateUrl: './edit-custom-profile.component.html',
  styleUrls: ['./edit-custom-profile.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class EditCustomProfileComponent implements OnInit {
  icClose = icClose;
  form: FormGroup;
  profileList = [];
  customProfileModalTitle:string;
  customProfileModalActionTitle:string;
  addMembershipModel:AddMembershipModel= new AddMembershipModel();
  constructor(private dialogRef: MatDialogRef<EditCustomProfileComponent>,
    @Inject(MAT_DIALOG_DATA) public data:any, 
    private fb: FormBuilder,
  private membershipService:MembershipService,
  private snackbar:MatSnackBar) { }
  getAllMembersList: GetAllMembersList = new GetAllMembersList();

  ngOnInit(): void {
    this.form=this.fb.group({
      title:['',Validators.required],
      userType:['',Validators.required],
      description:[''],
    })
    this.getAllMembership();
    if(this.data !== null){
      this.form.controls.title.patchValue(this.data.profile);
      this.form.controls.description.patchValue(this.data.description);
      this.form.controls.userType.patchValue(this.data.profileType);
      this.customProfileModalTitle ="updateCustomProfile";
      this.customProfileModalActionTitle="update";
    }else{
      this.customProfileModalTitle ="addCustomProfile";
      this.customProfileModalActionTitle="submit";
    }
    
  }

  submit(){
    if (this.form.valid) {   
      if(this.data !== null){
        this.addMembershipModel.membership.membershipId=this.data.membershipId;    
        this.addMembershipModel.membership.profile=this.form.controls.title.value;
        this.addMembershipModel.membership.description=this.form.controls.description.value;
        this.addMembershipModel.membership.profileType=this.form.controls.userType.value;
          
        this.membershipService.updateMembership(this.addMembershipModel).subscribe(data => {
          if (typeof (data) == 'undefined') {
            this.snackbar.open('Member Updation failed. ' + sessionStorage.getItem("httpError"), '', {
              duration: 10000
            });
          }
          else {
            if (data._failure) {
              this.snackbar.open('Member Updation failed. ' + data._message, '', {
                duration: 10000
              });
            } else {
              this.snackbar.open('Member Updation Successful.', '', {
                duration: 10000
              });              
              this.dialogRef.close(true);
            }
          }
  
        })     
      }else{
        this.addMembershipModel.membership.profile=this.form.controls.title.value;
        this.addMembershipModel.membership.description=this.form.controls.description.value;
        this.addMembershipModel.membership.profileType=this.form.controls.userType.value;
       
        this.membershipService.addMembership(this.addMembershipModel).subscribe(data => {
          if (typeof (data) == 'undefined') {
            this.snackbar.open('Member Submission failed. ' + sessionStorage.getItem("httpError"), '', {
              duration: 10000
            });
          }
          else {
            if (data._failure) {
              this.snackbar.open('Member Submission failed. ' + data._message, '', {
                duration: 10000
              });
            } else {
              this.snackbar.open('Member Submission Successful.', '', {
                duration: 10000
              });              
              this.dialogRef.close(true);
            }
          }
  
        })     
      }
       
      }
    }

    getAllMembership(){
      this.membershipService.getAllMembers(this.getAllMembersList).subscribe((res) => {
        if (typeof (res) == 'undefined') {
          this.snackbar.open('Membership List failed. ' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else {
          if (res._failure) {
            if (res.getAllMemberList) {
              if (res.getAllMemberList == null) {
                this.profileList=[];
                this.snackbar.open(res._message,'', {
                  duration: 10000
                });
              }
            } else{
              this.profileList=[];
            }
          }
          else { 
            this.profileList=res.getAllMemberList.filter((item)=>{
              return item.isSystem;
            });           
          }
        }
      })
    }

}
