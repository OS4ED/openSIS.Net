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

import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { fadeInRight400ms } from '../../../../../@vex/animations/fade-in-right.animation';
import { TranslateService } from '@ngx-translate/core';
import icAdd from '@iconify/icons-ic/baseline-add';
import icClear from '@iconify/icons-ic/baseline-clear';
import { SchoolCreate } from '../../../../enums/school-create.enum';
import { StaffService } from '../../../../services/staff.service';
import { StaffSchoolInfoListModel, StaffSchoolInfoModel } from '../../../../models/staff.model';
import { Subject } from '../../../../enums/temp-subjects-list.enum';
import { GetAllGradeLevelsModel } from '../../../../models/grade-level.model';
import { GradeLevelService } from '../../../../services/grade-level.service';
import { OnlySchoolListModel } from '../../../../models/get-all-school.model';
import { SchoolService } from '../../../../services/school.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import icEdit from '@iconify/icons-ic/edit';
import moment from 'moment';
import { ImageCropperService } from '../../../../services/image-cropper.service';
import { ModuleIdentifier } from '../../../../enums/module-identifier.enum';
import { SharedFunction } from '../../../shared/shared-function';
import { RolePermissionListViewModel, RolePermissionViewModel } from '../../../../models/roll-based-access.model';
import { RollBasedAccessService } from '../../../../services/roll-based-access.service';
import { GetAllMembersList } from '../../../../models/membership.model';
import { MembershipService } from '../../../../services/membership.service';
import { CourseManagerService } from '../../../../services/course-manager.service';
import { GetAllSubjectModel } from '../../../../models/course-manager.model';
import { DefaultValuesService } from '../../../../common/default-values.service';

@Component({
  selector: 'vex-staff-schoolinfo',
  templateUrl: './staff-schoolinfo.component.html',
  styleUrls: ['./staff-schoolinfo.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms,
    fadeInRight400ms
  ]
})
export class StaffSchoolinfoComponent implements OnInit {
  getSchoolList: OnlySchoolListModel = new OnlySchoolListModel();
  staffCreate = SchoolCreate;
  staffDetailsForViewAndEdit;
  staffCreateMode: SchoolCreate;
  @Output() checkUpdatedProfileName= new EventEmitter<string>()
  @ViewChild('f') currentForm: NgForm;
  divCount = [1];
  getAllGradeLevels: GetAllGradeLevelsModel = new GetAllGradeLevelsModel();
  staffSchoolInfoModel: StaffSchoolInfoModel = new StaffSchoolInfoModel();
  icAdd = icAdd;
  icClear = icClear;
  icEdit = icEdit;
  selectedSchoolId = [];
  otherGradeLevelTaught;
  otherSubjectTaught;
  cloneStaffModel;
  moduleIdentifier = ModuleIdentifier;

  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();
  getAllMembersList: GetAllMembersList = new GetAllMembersList();
  getAllSubjectModel: GetAllSubjectModel = new GetAllSubjectModel();

  constructor(public translateService: TranslateService,
    private snackbar: MatSnackBar,
    private staffService: StaffService,
    private gradeLevelService: GradeLevelService,
    public rollBasedAccessService: RollBasedAccessService,
    private schoolService: SchoolService,
    private imageCropperService: ImageCropperService,
    private commonFunction: SharedFunction,
    private membershipService: MembershipService,
    private defaultValuesService: DefaultValuesService, 
    private courseManagerService:CourseManagerService) {
    translateService.use('en');
  }

  ngOnInit(): void {
    this.staffService.staffCreatedMode.subscribe((res)=>{
      this.staffCreateMode = res;
    })
    this.staffService.staffDetailsForViewedAndEdited.subscribe((res)=>{
      this.staffDetailsForViewAndEdit = res;
    })

    this.permissionListViewModel = this.rollBasedAccessService.getPermission();
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 5);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 10);
    const permissionSubCategory = permissionCategory.permissionSubcategory.find(x => x.permissionSubcategoryId === 14);
    this.editPermission = permissionSubCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionSubCategory.rolePermission[0].canDelete;
    this.addPermission = permissionSubCategory.rolePermission[0].canAdd;

    if (this.staffCreateMode == this.staffCreate.EDIT) {
      this.staffCreateMode = this.staffCreate.ADD
    }

    if (this.staffCreateMode == this.staffCreate.ADD) {
      this.getAllGradeLevel();
      this.callAllSchool();
      this.getAllStaffSchoolInfo();
      this.getAllMembership();
      this.getAllSubjectList();
    } else if (this.staffCreateMode == this.staffCreate.VIEW) {
      this.staffService.changePageMode(this.staffCreateMode);
      this.getAllStaffSchoolInfo();
    }
  }

  getAllSubjectList(){   
    this.courseManagerService.GetAllSubjectList(this.getAllSubjectModel).subscribe(data => {          
      this.getAllSubjectModel.subjectList=data.subjectList;      
    });
  }

  getAllGradeLevel() {
    this.gradeLevelService.getAllGradeLevels(this.getAllGradeLevels).subscribe((res) => {
      this.getAllGradeLevels.tableGradelevelList = res.tableGradelevelList;
    })
  }

  callAllSchool() {
    this.schoolService.GetAllSchools(this.getSchoolList).subscribe((data) => {
      this.getSchoolList.schoolMaster = data.schoolMaster;
    });
  }

  onSchoolChange(schoolId, indexOfDynamicRow) {
    let index = this.getSchoolList.schoolMaster.findIndex((x) => {
      return x.schoolId === +schoolId;
    });
    this.staffSchoolInfoModel.staffSchoolInfoList[indexOfDynamicRow].schoolAttachedName = this.getSchoolList.schoolMaster[index].schoolName;
    this.selectedSchoolId[indexOfDynamicRow] = +schoolId
  }

  addMoreSchoolInfo() {
    this.staffSchoolInfoModel.staffSchoolInfoList.push(new StaffSchoolInfoListModel)
    this.divCount.push(2);
  }

  deleteSchoolInfo(index) {
    this.divCount.splice(index, 1);
    this.staffSchoolInfoModel.staffSchoolInfoList.splice(index, 1);
    this.selectedSchoolId.splice(index, 1);
  }

  submitSchoolInfo() {
    this.currentForm.form.markAllAsTouched();
    if (this.currentForm.form.valid) {
      this.updateSchoolInfo();
    }
  }

  compareDate(index) {
    let endDate = this.staffSchoolInfoModel.staffSchoolInfoList[index].endDate
    if (this.staffSchoolInfoModel.staffSchoolInfoList[index].startDate != null) {
      if (endDate == null || moment(endDate) > moment()) {
        return true;
      } else {
        return false;
      }
    } else {
      return false;
    }

  }

  getAllStaffSchoolInfo() {
    this.staffSchoolInfoModel.staffId = this.staffService.getStaffId();
    this.staffSchoolInfoModel.staffSchoolInfoList[0].staffId = this.staffService.getStaffId();
    this.staffService.viewStaffSchoolInfo(this.staffSchoolInfoModel).subscribe((res) => {
      if (typeof (res) == 'undefined') {
        this.snackbar.open('Staff School Info Failed to Fetch. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (res._failure) {
          this.snackbar.open( res._message, '', {
            duration: 10000
          });
        } else {
          this.staffSchoolInfoModel = res;
          this.cloneStaffModel = JSON.stringify(this.staffSchoolInfoModel)
          if (this.staffSchoolInfoModel.otherGradeLevelTaught != null || this.staffSchoolInfoModel.otherGradeLevelTaught != null) {
            this.otherGradeLevelTaught = this.staffSchoolInfoModel.otherGradeLevelTaught?.split(',');
            this.otherSubjectTaught = this.staffSchoolInfoModel.otherSubjectTaught?.split(',');
          }
          this.manipulateArray();
        }
      }
    })
  }

  findProfileForCurrentSchool(schoolInfo){
    let currentProfileIndex=schoolInfo.findIndex((item)=>{
      return item.schoolId==this.defaultValuesService.schoolID
    })
    // this.checkUpdatedProfileName.emit(schoolInfo[currentProfileIndex].profile)
    this.staffService.setCheckUpdatedProfileName(schoolInfo[currentProfileIndex].profile);
  }

  manipulateArray() {
    for (let i = 0; i < this.staffSchoolInfoModel.staffSchoolInfoList?.length; i++) {
      this.staffSchoolInfoModel.staffSchoolInfoList[i].schoolAttachedId = this.staffSchoolInfoModel.staffSchoolInfoList[i].schoolAttachedId.toString();
      delete this.staffSchoolInfoModel.staffSchoolInfoList[i].staffMaster;
      this.selectedSchoolId[i] = +this.staffSchoolInfoModel.staffSchoolInfoList[i].schoolAttachedId;
      let endDate = this.staffSchoolInfoModel.staffSchoolInfoList[i].endDate
      if (endDate != null && moment(endDate) < moment()) {
        Object.assign(this.staffSchoolInfoModel.staffSchoolInfoList[i], { hide: true })
        this.selectedSchoolId.splice(i, 1);
      } else {
        Object.assign(this.staffSchoolInfoModel.staffSchoolInfoList[i], { hide: false })
      }
    };
  }

  getAllMembership() {
    this.membershipService.getAllMembers(this.getAllMembersList).subscribe((res) => {
      if (typeof (res) == 'undefined') {
        this.snackbar.open('Membership List failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (res._failure) {
          if (res.getAllMemberList == null) {
            this.getAllMembersList.getAllMemberList = [];
            this.snackbar.open( res._message,'', {
              duration: 10000
            });
          } else {
            this.getAllMembersList.getAllMemberList = [];
          }
        }
        else {
          this.getAllMembersList.getAllMemberList = res.getAllMemberList.filter((item) => {
            return (item.profileType == 'School Administrator' || item.profileType == 'Admin Assistant'
              || item.profileType == 'Teacher' || item.profileType == 'Homeroom Teacher')
          });
        }
      }
    })
  }

  updateSchoolInfo() {
    this.staffSchoolInfoModel.staffId = this.staffService.getStaffId();
    this.staffSchoolInfoModel.joiningDate = this.commonFunction.formatDateSaveWithoutTime(this.staffSchoolInfoModel.joiningDate)
    if (this.otherGradeLevelTaught != undefined) {
      this.staffSchoolInfoModel.otherGradeLevelTaught = this.otherGradeLevelTaught.toString()
    }
    if (this.otherSubjectTaught != undefined) {
      this.staffSchoolInfoModel.otherSubjectTaught = this.otherSubjectTaught.toString()
    }
    this.staffSchoolInfoModel?.staffSchoolInfoList?.map((item) => {
      item.staffId = this.staffService.getStaffId();
      item.startDate = this.commonFunction.formatDateSaveWithoutTime(item.startDate);
      item.endDate = this.commonFunction.formatDateSaveWithoutTime(item.endDate)
    });
    this.staffService.updateStaffSchoolInfo(this.staffSchoolInfoModel).subscribe((res) => {
      if (typeof (res) == 'undefined') {
        this.snackbar.open('Staff School Info Update failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (res._failure) {
          this.snackbar.open( res._message, '', {
            duration: 10000
          });
        } else {
          this.snackbar.open(res._message, '', {
            duration: 10000
          });
          this.staffSchoolInfoModel = res;
          this.findProfileForCurrentSchool(res.staffSchoolInfoList);
          this.cloneStaffModel = JSON.stringify(this.staffSchoolInfoModel)
          if (this.staffCreateMode == this.staffCreate.EDIT || this.staffCreateMode == this.staffCreate.ADD) {
            this.staffCreateMode = this.staffCreate.VIEW
          }
          this.staffService.changePageMode(this.staffCreateMode);
          this.manipulateArray();
        }
      }
    })
  }

  editSchoolInfo() {
    if (this.staffSchoolInfoModel.staffSchoolInfoList != null) {
      for (let i = 0; i < this.staffSchoolInfoModel.staffSchoolInfoList?.length; i++) {
        this.divCount[i] = 2;
      }
    } else {
      this.staffSchoolInfoModel = new StaffSchoolInfoModel();
    }
    this.getAllGradeLevel();
    this.callAllSchool();
    this.getAllMembership();
    this.getAllSubjectList();
    this.staffCreateMode = this.staffCreate.EDIT;
    this.staffService.changePageMode(this.staffCreateMode);
  }
  cancelEdit() {
    if (this.staffSchoolInfoModel !== JSON.parse(this.cloneStaffModel)) {
      this.staffSchoolInfoModel = JSON.parse(this.cloneStaffModel);
      this.staffService.sendDetails(JSON.parse(this.cloneStaffModel));
    }
    this.staffCreateMode = this.staffCreate.VIEW;
    this.staffService.changePageMode(this.staffCreateMode);
    this.imageCropperService.cancelImage("staff");
  }
}

