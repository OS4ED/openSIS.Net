/***********************************************************************************
openSIS is a free student information system for public and non-public
schools from Open Solutions for Education, Inc.Website: www.os4ed.com.

Visit the openSIS product website at https://opensis.com to learn more.
If you have question regarding this software or the license, please contact
via the website.

The software is released under the terms of the GNU Affero General Public License as
published by the Free Software Foundation, version 3 of the License.
See https://www.gnu.org/licenses/agpl-3.0.en.html.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

Copyright (c) Open Solutions for Education, Inc.

All rights reserved.
***********************************************************************************/

import { Component, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import icClose from '@iconify/icons-ic/twotone-close';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icDelete from '@iconify/icons-ic/twotone-delete';
import icAdd from '@iconify/icons-ic/twotone-add';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import {GetAllProgramModel,AddProgramModel,UpdateProgramModel,MassUpdateProgramModel,DeleteProgramModel,ProgramsModel} from '../../../../models/course-manager.model';
import {CourseManagerService} from '../../../../services/course-manager.service';
import {MatSnackBar} from '@angular/material/snack-bar';
import { FormBuilder, NgForm, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmDialogComponent } from '../../../shared-module/confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { RolePermissionListViewModel, RolePermissionViewModel } from '../../../../models/roll-based-access.model';
import { CryptoService } from '../../../../services/Crypto.service';
import { DefaultValuesService } from 'src/app/common/default-values.service';
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
  editPermission:boolean= false;
  permissionList= [];
  permissionListViewModel:RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup:RolePermissionViewModel= new RolePermissionViewModel();
  hideinput = {};
  hideDiv={};
  constructor(
    private dialogRef: MatDialogRef<ManageProgramsComponent>,
    private courseManager:CourseManagerService,
    private snackbar: MatSnackBar,
    private fb: FormBuilder,
    private defaultValuesService: DefaultValuesService,
    public translateService:TranslateService,
    private dialog: MatDialog,
    private cryptoService: CryptoService
   ) { 
      translateService.use('en');
    }

  ngOnInit(): void {  
    this.getAllProgramList();
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x=>x.permissionGroup.permissionGroupId == 6);
    let permissionCategory= this.permissionGroup.permissionGroup.permissionCategory.find(x=>x.permissionCategoryId == 12);
    this.editPermission = permissionCategory.rolePermission[0].canEdit;
     
  }
  
  getAllProgramList(){   
    this.courseManager.GetAllProgramsList(this.getAllProgramModel).subscribe(data => {
      if(data._failure){
        if(data._message==="NO RECORD FOUND"){
          this.programList=[];
          this.snackbar.open(data._message, '', {
            duration: 10000
          });        
        }
      }else{
        this.programList = data.programList;
        this.programList.map((val,index)=>{
          this.updateProgramModel.programList.push(new ProgramsModel());
          this.hideinput[index] = true;
          this.hideDiv[index] = false;
        });
      }
    });
  }

  confirmDelete(deleteDetails){
    // call our modal window
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: {
          title: this.defaultValuesService.translateKey('areYouSure'),
          message: this.defaultValuesService.translateKey('youAreAboutToDelete') +
          deleteDetails.programName + '.'}
    });
    // listen to response
    dialogRef.afterClosed().subscribe(dialogResult => {
      // if user pressed yes dialogResult will be true,
      // if user pressed no - it will be false
      if (dialogResult){
        this.deleteProgram(deleteDetails);
      }
   });
  }
  deleteProgram(deleteDetails){
  this.deleteProgramModel.programs.programId=deleteDetails.programId;    
  this.courseManager.DeletePrograms(this.deleteProgramModel).subscribe(data => {
    if (data){
      if (data._failure) {
        this.snackbar.open(data._message, '', {
          duration: 10000
        });
      } else {
        this.snackbar.open(data._message, '', {
          duration: 10000
        });
        this.dialogRef.close(true);
      }
    }else{
      this.snackbar.open('Programs Deletion failed. ' + sessionStorage.getItem("httpError"), '', {
        duration: 10000
      });
    }
  });
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
          obj["tenantId"] = this.defaultValuesService.getTenantID();
          obj["schoolId"] = this.defaultValuesService.getSchoolID();
          obj["createdBy"] = this.defaultValuesService.getEmailId();
          obj["updatedBy"] =  this.defaultValuesService.getEmailId();
          this.massUpdateProgramModel.programList.push(obj);
        }
      }
    })
  
    this.massUpdateProgramModel.programList.splice(0, 1); 
    if(this.addProgramModel.programList[0].hasOwnProperty("programName")){
      let obj1 ={};
      obj1["programId"] = 0
      obj1["programName"] = this.addProgramModel.programList[0].programName;
      obj1["tenantId"]= this.defaultValuesService.getTenantID();
      obj1["schoolId"] = this.defaultValuesService.getSchoolID();
      obj1["createdBy"] = this.defaultValuesService.getEmailId();
      obj1["updatedBy"]=  this.defaultValuesService.getEmailId();
      this.massUpdateProgramModel.programList.push(obj1);
    }
    if(this.massUpdateProgramModel.programList.length > 0){
      this.courseManager.AddEditPrograms(this.massUpdateProgramModel).subscribe(data => {
        if (data){
          if (data._failure) {
            this.snackbar.open(data._message, '', {
              duration: 10000
            });
          }
          else{
            this.snackbar.open(data._message, '', {
              duration: 10000
            }).afterOpened().subscribe(data => {
                this.getAllProgramList();
                this.massUpdateProgramModel.programList=[{}];
                this.addProgramModel.programList= [new ProgramsModel()];
            });
            this.dialogRef.close(true);
          }
        }
        else{
          this.snackbar.open('Program Submission failed. ' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
      });
    }
  }
}
