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
import {GetAllSubjectModel,AddSubjectModel,UpdateSubjectModel,SubjectModel,DeleteSubjectModel,MassUpdateSubjectModel} from '../../../../models/course-manager.model';
import {CourseManagerService} from '../../../../services/course-manager.service';
import {MatSnackBar} from '@angular/material/snack-bar';
import { FormBuilder, NgForm, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmDialogComponent } from '../../../shared-module/confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { CryptoService } from '../../../../services/Crypto.service';
import { RolePermissionListViewModel, RolePermissionViewModel } from '../../../../models/roll-based-access.model';
import { DefaultValuesService } from '../../../../common/default-values.service';
@Component({
  selector: 'vex-manage-subjects',
  templateUrl: './manage-subjects.component.html',
  styleUrls: ['./manage-subjects.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class ManageSubjectsComponent implements OnInit {

  icClose = icClose;
  icEdit = icEdit;
  icDelete = icDelete;
  icAdd = icAdd;
  subjectList=[];
  form: FormGroup;
  f: NgForm;
  update: NgForm;
  editMode=false;
  editSubjectId;
  subjectListArr=[];
  subjectNameArr =[];
  getAllSubjectModel: GetAllSubjectModel = new GetAllSubjectModel();
  addSubjectModel: AddSubjectModel = new AddSubjectModel();
  updateSubjectModel: UpdateSubjectModel = new UpdateSubjectModel();
  deleteSubjectModel:DeleteSubjectModel= new DeleteSubjectModel();
  massUpdateSubjectModel:MassUpdateSubjectModel= new MassUpdateSubjectModel();
  editPermission:boolean= false;
  permissionList= [];
  permissionListViewModel:RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup:RolePermissionViewModel= new RolePermissionViewModel();
  hideinput = {};
  hideDiv={};
  constructor(
    private dialogRef: MatDialogRef<ManageSubjectsComponent>,
    private courseManager: CourseManagerService,
    private snackbar: MatSnackBar,
    private fb: FormBuilder,
    public translateService: TranslateService,
    private defaultValuesService: DefaultValuesService,
    private dialog: MatDialog,
    private cryptoService: CryptoService) {
      translateService.use('en');
    }

  ngOnInit(): void {  
    this.getAllSubjectList();
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x=>x.permissionGroup.permissionGroupId == 6);
    let permissionCategory= this.permissionGroup.permissionGroup.permissionCategory.find(x=>x.permissionCategoryId == 12);
    this.editPermission = permissionCategory.rolePermission[0].canEdit;
      
  }
  
  getAllSubjectList(){   
    this.courseManager.GetAllSubjectList(this.getAllSubjectModel).subscribe(data => {
      if(data._failure){
        if(data._message==="NO RECORD FOUND"){
          this.subjectList=[];
          this.snackbar.open(data._message, '', {
            duration: 10000
          });
        }
      }else{
        this.subjectList = data.subjectList;
        this.subjectList.map((val, index) => {
          this.updateSubjectModel.subjectList.push(new SubjectModel());
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
          message: this.defaultValuesService.translateKey('youAreAboutToDelete') + deleteDetails.subjectName + '.'}
    });
    // listen to response
    dialogRef.afterClosed().subscribe(dialogResult => {
      // if user pressed yes dialogResult will be true, 
      // if user pressed no - it will be false
      if(dialogResult){
        this.deleteSubject(deleteDetails);
      }
   });
  }

  deleteSubject(deleteDetails){
  this.deleteSubjectModel.subject.subjectId = deleteDetails.subjectId;
  this.courseManager.DeleteSubject(this.deleteSubjectModel).subscribe(data => {
    if (data){
      if (data._failure) {
        this.snackbar.open( data._message, '', {
          duration: 10000
        });
      } else {
        this.snackbar.open(data._message, '', {
          duration: 10000
        });
        this.dialogRef.close(true);
      }
    }else{
      this.snackbar.open('Subject Deletion failed. ' + sessionStorage.getItem("httpError"), '', {
        duration: 10000
      });
    }
  });
}

updateSubject(element,index){
  
  let obj = {};
  obj["subjectId"] = element.subjectId;
  obj["subjectName"] = element.subjectName;
  this.subjectListArr.push(obj);  
  this.subjectListArr.map((val)=>{  
    this.updateSubjectModel.subjectList[index].subjectName = val.subjectName;
    this.updateSubjectModel.subjectList[index].subjectId = val.subjectId;
    this.hideinput[index] = false;
    this.hideDiv[index] = true;
  });
}


  submit(){
    this.updateSubjectModel.subjectList.map(val=>{
      let obj ={};
      if(val.hasOwnProperty("subjectName")){
        if(val.subjectId > 0){
          obj["subjectId"] = val.subjectId;
          obj["subjectName"] = val.subjectName;
          obj["tenantId"]= this.defaultValuesService.getTenantID();
          obj["schoolId"] = this.defaultValuesService.getSchoolID();
          obj["createdBy"] = this.defaultValuesService.getEmailId();
          obj["updatedBy"]= this.defaultValuesService.getEmailId();
          this.massUpdateSubjectModel.subjectList.push(obj);
        }
      }      
    })
   
    this.massUpdateSubjectModel.subjectList.splice(0, 1); 
    if(this.addSubjectModel.subjectList[0].hasOwnProperty("subjectName")){
      let obj1 ={};
      obj1["subjectId"] = 0
      obj1["subjectName"] = this.addSubjectModel.subjectList[0].subjectName;
      obj1["tenantId"]= sessionStorage.getItem("tenantId");
      obj1["schoolId"] = +sessionStorage.getItem("selectedSchoolId");  
      obj1["createdBy"] = sessionStorage.getItem("email");       
      obj1["updatedBy"]=  sessionStorage.getItem("email");       
      this.massUpdateSubjectModel.subjectList.push(obj1); 
    }
    if(this.massUpdateSubjectModel.subjectList.length > 0){
      this.courseManager.AddEditSubject(this.massUpdateSubjectModel).subscribe(data => {
        if (data){

          if (data._failure) {
            this.snackbar.open( data._message, '', {
              duration: 10000
            });
          } 
          else{
            this.snackbar.open(data._message, '', {
              duration: 10000
            }).afterOpened().subscribe(data => {
                this.getAllSubjectList();
                this.massUpdateSubjectModel.subjectList=[{}];
                this.addSubjectModel.subjectList= [new SubjectModel()];
            });
            this.dialogRef.close(true);
          }
        }else{
          this.snackbar.open('Subject Submission failed. ' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
      });
    }
  }
}
