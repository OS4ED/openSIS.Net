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

import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, NgForm, FormGroup, Validators } from '@angular/forms';
import icClose from '@iconify/icons-ic/twotone-close';
import icBack from '@iconify/icons-ic/baseline-arrow-back';
import { fadeInUp400ms } from '../../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../../@vex/animations/stagger.animation';
import { MatDialog } from '@angular/material/dialog';
import { fadeInRight400ms } from 'src/@vex/animations/fade-in-right.animation';
import { StudentSiblingAssociation, StudentSiblingSearch } from '../../../../../models/student.model';
import { StudentService } from '../../../../../services/student.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AssociateStudent, AddParentInfoModel } from '../../../../../models/parent-info.model';
import { relationShip} from '../../../../../enums/studentAdd.enum';
import {MAT_DIALOG_DATA} from '@angular/material/dialog';
import { ParentInfoService } from '../../../../../services/parent-info.service';
import { GetAllGradeLevelsModel } from '../../../../../models/grade-level.model';
import { GradeLevelService } from '../../../../../services/grade-level.service';
import { LovList } from '../../../../../models/lov.model';
import { CommonService } from '../../../../../services/common.service';
import { DefaultValuesService } from '../../../../../common/default-values.service';
@Component({
  selector: 'vex-add-sibling',
  templateUrl: './add-sibling.component.html',
  styleUrls: ['./add-sibling.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms,
    fadeInRight400ms
  ]
})
export class AddSiblingComponent implements OnInit {
  icClose = icClose;
  icBack = icBack;
  form: FormGroup;
  associateStudents: NgForm;
  studentSiblingSearch: StudentSiblingSearch = new StudentSiblingSearch();
  getAllGradeLevelsModel: GetAllGradeLevelsModel = new GetAllGradeLevelsModel();
  studentSiblingAssociation: StudentSiblingAssociation = new StudentSiblingAssociation();
  hideSearchBoxAfterSearch = true;
  associatStudent: AssociateStudent = new AssociateStudent();
  associateMultipleStudents: NgForm;
  relationShipList = [];
  addParentInfoModel: AddParentInfoModel = new AddParentInfoModel();
  parentData;
  mode;
  gradeLevelArr;
  lovList: LovList = new LovList();
  constructor(private dialogRef: MatDialogRef<AddSiblingComponent>,
              private fb: FormBuilder,
              private studentService: StudentService,
              private snackbar: MatSnackBar,
              private parentInfoService: ParentInfoService,
              private gradeLevelService: GradeLevelService,
              private defaultValuesService: DefaultValuesService,
              private commonService: CommonService,
              @Inject(MAT_DIALOG_DATA) public val) { }

  ngOnInit(): void {
    this.getRelationship();
    this.getGradeLevel();
    this.form = this.fb.group(
      {
        firstGivenName: [null, Validators.required],
        lastFamilyName: [null, Validators.required],
        gradeLevel: [null],
        studentId: [null],
        dob: [null],
        searchAllSchool: [false]
      });


  }
  cancel(){
    this.dialogRef.close(false);
  }
  getRelationship(){

      this.lovList.lovName = 'Relationship';
      this.commonService.getAllDropdownValues(this.lovList).subscribe(
        (res: LovList) => {
          this.relationShipList = res.dropdownList;

        }
      );

  }
  getGradeLevel(){
    this.gradeLevelService.getAllGradeLevels(this.getAllGradeLevelsModel).subscribe((res) => {
      if (res){
        if (res._failure) {
          this.snackbar.open( res._message, '', {
            duration: 10000
          });
        } else {
          this.gradeLevelArr = res.tableGradelevelList;
        }
      }else{
        this.snackbar.open(this.defaultValuesService.translateKey('gradeLevelInformationfailed')
        + sessionStorage.getItem('httpError'), '', {
          duration: 10000
        });
      }
    });
  }

  search(){
    this.form.markAllAsTouched();
    if (this.form.valid){
      if (this.form.value.searchAllSchool){
        this.studentSiblingSearch.schoolId = null;
      }else{
        this.studentSiblingSearch.schoolId = this.defaultValuesService.getSchoolID();
      }
      if (this.studentSiblingSearch.studentInternalId === ''){
        this.studentSiblingSearch.studentInternalId = null;
      }
      this.studentService.siblingSearch(this.studentSiblingSearch).subscribe((res) => {
        if (res){
          if (res._failure) {
            this.snackbar.open( res._message, '', {
              duration: 10000
            });
          } else {
            if (this.val !== null){
              if (this.val.data){
                this.mode = 'Parent';
              }
            }
            this.hideSearchBoxAfterSearch = false;
            this.studentSiblingSearch.getStudentForView = res.getStudentForView;

          }
        }else{
          this.snackbar.open(this.defaultValuesService.translateKey('studentSearchfailed') + sessionStorage.getItem('httpError'), '', {
            duration: 10000
          });
        }
      });
    }
  }
  associateMultipleStudentsToParent(){
    let isCustodian = this.associatStudent.isCustodian;
    let contactRelationship = this.associatStudent.contactRelationship;
    if (contactRelationship === undefined){
     contactRelationship = '';
    }
    if (isCustodian === undefined){
     isCustodian = false;
    }
    const obj = {
     'isCustodian': isCustodian,
     'relationship': contactRelationship,
     'tenantId': this.defaultValuesService.getTenantID(),
     'schoolId': this.defaultValuesService.getSchoolID(),
     'parentId': this.val.data.parentId
     };
    return obj;
  }
  associateStudent(studentDetails){
   if (this.val !== null){
    if (this.val.data !== null ){
      if (this.studentSiblingSearch.getStudentForView?.length > 1){
        const obj = this.associateMultipleStudentsToParent();
        if (obj.relationship === ''){
          this.snackbar.open(this.defaultValuesService.translateKey('pleaseProvideRelationship'), '', {
            duration: 10000
          });
        }else
        {
          this.addParentInfoModel.parentAssociationship.studentId = studentDetails.studentId;
          this.addParentInfoModel.parentInfo.parentAddress[0].studentId = studentDetails.studentId;
          this.addParentInfoModel.parentAssociationship.parentId = this.val.data.parentInfo.parentId;
          this.addParentInfoModel.parentAssociationship.schoolId = studentDetails.schoolId;
          this.addParentInfoModel.parentInfo.parentAddress[0].parentId = this.val.data.parentInfo.parentId;
          this.addParentInfoModel.parentInfo.parentId = this.val.data.parentInfo.parentId;
          this.addParentInfoModel.parentAssociationship.relationship = obj.relationship;
          this.addParentInfoModel.parentAssociationship.isCustodian = obj.isCustodian;
          this.parentInfoService.addParentForStudent(this.addParentInfoModel).subscribe(data => {
            if (data){
              if (data._failure) {
                this.snackbar.open( data._message, '', {
                  duration: 10000
                });
              }
              else
              {
                this.snackbar.open( data._message, '', {
                  duration: 10000
                });
                this.dialogRef.close(true);
              }
            }
            else{
              this.snackbar.open(this.defaultValuesService.translateKey('parentInformationSubmissionfailed') + sessionStorage.getItem('httpError'), '', {
                duration: 10000
              });
            }
          });
        }
      }else{
        let isCustodian = this.associatStudent.isCustodian;
        let contactRelationship = this.associatStudent.contactRelationship;
        if (contactRelationship === undefined){
          contactRelationship = '';
         }
        if (isCustodian === undefined){
          isCustodian = false;
         }


        this.addParentInfoModel.parentAssociationship.relationship = contactRelationship;
        this.addParentInfoModel.parentAssociationship.isCustodian = isCustodian;
        this.addParentInfoModel.parentAssociationship.studentId = studentDetails.studentId;
        this.addParentInfoModel.parentAssociationship.schoolId = studentDetails.schoolId;
        this.addParentInfoModel.parentInfo.parentAddress[0].studentId = studentDetails.studentId;
        this.addParentInfoModel.parentAssociationship.parentId = this.val.data.parentInfo.parentId;
        this.addParentInfoModel.parentInfo.parentAddress[0].parentId = this.val.data.parentInfo.parentId;
        this.addParentInfoModel.parentInfo.parentId = this.val.data.parentInfo.parentId;

        this.parentInfoService.addParentForStudent(this.addParentInfoModel).subscribe(data => {
          if (data){
            if (data._failure) {
              this.snackbar.open( data._message, '', {
                duration: 10000
              });
            }
            else
            {
              // static comment added because same api is called for parent associate for student 
              this.snackbar.open("Student Added Successfully", '', {
              duration: 10000
              });
              this.dialogRef.close(true);
            }
          }
          else{
            this.snackbar.open(this.defaultValuesService.translateKey('parentInformationSubmissionfailed') + sessionStorage.getItem('httpError'), '', {
              duration: 10000
            });
          }
        });
      }
    }
  }else{
      this.studentSiblingAssociation.studentMaster.studentId = studentDetails.studentId;
      this.studentSiblingAssociation.studentMaster.schoolId = studentDetails.schoolId;
      this.studentSiblingAssociation.studentId = this.studentService.getStudentId();
      this.studentService.associationSibling(this.studentSiblingAssociation).subscribe((res) => {
        if (res){
          if (res._failure) {
            this.snackbar.open( res._message, '', {
              duration: 10000
            });
          } else {
            this.dialogRef.close(true);
            this.snackbar.open( res._message, '', {
              duration: 10000
            });
          }
        }
        else{
          this.snackbar.open(this.defaultValuesService.translateKey('associationFailed') + sessionStorage.getItem('httpError'), '', {
            duration: 10000
          });
        }
      });
    }

  }

  backToSearch(){
    if (this.hideSearchBoxAfterSearch){
      this.dialogRef.close();
    }else{
      this.hideSearchBoxAfterSearch = true;
      this.studentSiblingSearch.getStudentForView = null;
    }
  }
}
