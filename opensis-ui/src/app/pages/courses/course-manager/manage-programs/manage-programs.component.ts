import { Component, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import icClose from '@iconify/icons-ic/twotone-close';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icDelete from '@iconify/icons-ic/twotone-delete';
import icAdd from '@iconify/icons-ic/twotone-add';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import {GetAllProgramModel,AddProgramModel,UpdateProgramModel,MassUpdateProgramModel,DeleteProgramModel,ProgramsModel} from '../../../../models/courseManagerModel';
import {CourseManagerService} from '../../../../services/course-manager.service';
import {MatSnackBar} from  '@angular/material/snack-bar';
import { FormBuilder,NgForm,FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmDialogComponent } from '../../../shared-module/confirm-dialog/confirm-dialog.component';

import { MatDialog } from '@angular/material/dialog';
@Component({
  selector: 'vex-manage-programs',
  templateUrl: './manage-programs.component.html',
  styleUrls: ['./manage-programs.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class ManageProgramsComponent implements OnInit {

  icClose = icClose;
  icEdit = icEdit;
  icDelete = icDelete;
  icAdd = icAdd;
  programList=[];
  form: FormGroup;
  f: NgForm;
  update: NgForm;
  editMode=false;
  editprogramId;
  programListArr=[];
  programNameArr =[];
  getAllProgramModel: GetAllProgramModel = new GetAllProgramModel();
  addProgramModel: AddProgramModel = new AddProgramModel();
  updateProgramModel: UpdateProgramModel = new UpdateProgramModel();
  massUpdateProgramModel: MassUpdateProgramModel = new MassUpdateProgramModel();
  deleteProgramModel: DeleteProgramModel = new DeleteProgramModel();
  hideinput = {};
  hideDiv={};
  constructor(
    private dialogRef: MatDialogRef<ManageProgramsComponent>,
    private courseManager:CourseManagerService,
    private snackbar: MatSnackBar,
    private fb: FormBuilder,
    public translateService:TranslateService,
    private dialog: MatDialog
   ) { 
      translateService.use('en');   
    }

  ngOnInit(): void {  
    this.getAllProgramList();
  }
  
  getAllProgramList(){   
    this.courseManager.GetAllProgramsList(this.getAllProgramModel).subscribe(data => {
      if(data._failure){
        if(data._message==="NO RECORD FOUND"){
          this.programList=[];
          this.snackbar.open('NO RECORD FOUND. ', '', {
            duration: 10000
          });        
        }
      }else{      
        this.programList=data.programList;
        this.programList.map((val,index)=>{
          this.updateProgramModel.programList.push(new ProgramsModel());  
          this.hideinput[index] = true; 
          this.hideDiv[index] = false;   
        })
         
      }
    });
  }

  


  confirmDelete(deleteDetails){
    
    // call our modal window
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: {
          title: "Are you sure?",
          message: "You are about to delete "+deleteDetails.programName+"."}
    });
    // listen to response
    dialogRef.afterClosed().subscribe(dialogResult => {
      // if user pressed yes dialogResult will be true, 
      // if user pressed no - it will be false
      if(dialogResult){
        this.deleteProgram(deleteDetails);
      }
   });
  }
  deleteProgram(deleteDetails){
  this.deleteProgramModel.programs.programId=deleteDetails.programId;    
  this.courseManager.DeletePrograms(this.deleteProgramModel).subscribe(data => {
    if (typeof (data) == 'undefined') {
      this.snackbar.open('Programs Deletion failed. ' + sessionStorage.getItem("httpError"), '', {
        duration: 10000
      });
    }
    else {
      if (data._failure) {
        this.snackbar.open('Programs Deletion failed. ' + data._message, 'LOL THANKS', {
          duration: 10000
        });
      } else {
     
        this.snackbar.open('Programs Deletion Successful.', '', {
          duration: 10000
        }).afterOpened().subscribe(data => {
          this.getAllProgramList();
        });
        
      }
    }

  })
}

updateProgram(element,index){ 
  let obj = {};
  obj["programId"] = element.programId;
  obj["programName"] = element.programName;  
  this.programListArr.push(obj);  
  this.programListArr.map((val)=>{  
    this.updateProgramModel.programList[index].programName = val.programName;
    this.updateProgramModel.programList[index].programId = val.programId;
    this.hideinput[index] = false;
    this.hideDiv[index] = true;
  })  
}


submit(){
  this.updateProgramModel.programList.map(val=>{
    let obj ={};
    if(val.hasOwnProperty("programName")){
      if(val.programId > 0){
        obj["programId"] = val.programId;
        obj["programName"] = val.programName;
        obj["tenantId"]= sessionStorage.getItem("tenantId");
        obj["schoolId"] = +sessionStorage.getItem("selectedSchoolId");    
        obj["createdBy"] = sessionStorage.getItem("email");       
        obj["updatedBy"]=  sessionStorage.getItem("email");       
        this.massUpdateProgramModel.programList.push(obj); 
      }
    }      
  })
 
  this.massUpdateProgramModel.programList.splice(0, 1); 
  if(this.addProgramModel.programList[0].hasOwnProperty("programName")){
    let obj1 ={};
    obj1["programId"] = 0
    obj1["programName"] = this.addProgramModel.programList[0].programName;
    obj1["tenantId"]= sessionStorage.getItem("tenantId");
    obj1["schoolId"] = +sessionStorage.getItem("selectedSchoolId");   
    obj1["createdBy"] = sessionStorage.getItem("email");       
    obj1["updatedBy"]=  sessionStorage.getItem("email");       
    this.massUpdateProgramModel.programList.push(obj1); 
  }
  if(this.massUpdateProgramModel.programList.length > 0){
    this.courseManager.AddEditPrograms(this.massUpdateProgramModel).subscribe(data => {     
      if(typeof(data)=='undefined'){
        this.snackbar.open('Program Submission failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else{
        if (data._failure) {
          this.snackbar.open('Program  Submission failed. ' + data._message, 'LOL THANKS', {
            duration: 10000
          });
        } 
        else{       
          
          this.snackbar.open('Program  Submission Successful.', '', {
            duration: 10000
          }).afterOpened().subscribe(data => {
            
              this.getAllProgramList();
            
            this.massUpdateProgramModel.programList=[{}];
            this.addProgramModel.programList= [new ProgramsModel()];
          });
          this.dialogRef.close(true);
        }        
      }      
    });
  } 
}   
}
