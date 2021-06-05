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

import { Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { fadeInRight400ms } from '../../../../../@vex/animations/fade-in-right.animation';
import icAdd from '@iconify/icons-ic/baseline-add';
import icClear from '@iconify/icons-ic/baseline-clear';
import { SchoolCreate } from '../../../../enums/school-create.enum';
import { RollingOptionsEnum } from '../../../../enums/rolling-retention-option.enum';
import { CalendarListModel } from '../../../../models/calendar.model';
import { CalendarService } from '../../../../services/calendar.service';
import { StudentEnrollmentDetails, StudentEnrollmentModel, StudentEnrollmentSchoolListModel } from '../../../../models/student.model';
import { StudentService } from '../../../../services/student.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import icEdit from '@iconify/icons-ic/edit';
import icCheckbox from '@iconify/icons-ic/baseline-check-box';
import icCheckboxOutline from '@iconify/icons-ic/baseline-check-box-outline-blank';
import icPromoted from '@iconify/icons-ic/baseline-north';
import icExternal from '@iconify/icons-ic/baseline-undo';
import icRetained from '@iconify/icons-ic/baseline-replay';
import icHomeSchool from '@iconify/icons-ic/baseline-home';
import icExpand from '@iconify/icons-ic/baseline-arrow-drop-up';
import icCollapse from '@iconify/icons-ic/baseline-arrow-drop-down';
import icTrasnferIn from '@iconify/icons-ic/baseline-call-received';
import icTrasnferOut from '@iconify/icons-ic/baseline-call-made';
import icDrop from '@iconify/icons-ic/vertical-align-bottom';
import { EnrollmentCodeListView } from '../../../../models/enrollment-code.model';
import { Router } from '@angular/router';
import { SharedFunction} from '../../../shared/shared-function';
import { RolePermissionListViewModel, RolePermissionViewModel } from '../../../../models/roll-based-access.model';
import { CryptoService } from '../../../../services/Crypto.service';
import { GetAllSectionModel } from '../../../../models/section.model';
import { SectionService } from '../../../../services/section.service';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { DefaultValuesService } from '../../../../common/default-values.service';
@Component({
  selector: 'vex-student-enrollmentinfo',
  templateUrl: './student-enrollmentinfo.component.html',
  styleUrls: ['./student-enrollmentinfo.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms,
    fadeInRight400ms
  ],
})
export class StudentEnrollmentinfoComponent implements OnInit, OnDestroy {
  icAdd = icAdd;
  icClear = icClear;
  icEdit = icEdit;
  icCheckbox = icCheckbox;
  icCheckboxOutline = icCheckboxOutline;
  icPromoted = icPromoted;
  icExternal = icExternal;
  icHomeSchool = icHomeSchool;
  icExpand = icExpand;
  icCollapse = icCollapse;
  icRetained = icRetained;
  icTrasnferIn = icTrasnferIn;
  icTrasnferOut = icTrasnferOut;
  icDrop = icDrop;
  studentCreate = SchoolCreate;
  studentCreateMode: SchoolCreate;
  studentDetailsForViewAndEdit;
  rollingOptions = Object.keys(RollingOptionsEnum);
  exitCodes = [];
  calendarListModel: CalendarListModel = new CalendarListModel();
  studentEnrollmentModel: StudentEnrollmentModel = new StudentEnrollmentModel();
  schoolListWithGrades: StudentEnrollmentSchoolListModel = new StudentEnrollmentSchoolListModel();
  enrollmentCodeListView: EnrollmentCodeListView = new EnrollmentCodeListView();
  @ViewChild('form') currentForm: NgForm;

  divCount = [1];
  schoolListWithGradeLevelsAndEnrollCodes = [];
  selectedSchoolIndex = [];
  selectedTransferredSchoolIndex = [];
  calendarNameInViewMode = '-';
  filteredEnrollmentInfoInViewMode;
  expandEnrollmentHistory = true;
  selectedExitCodes = [];
  selectedEnrollmentCodes = [];
  disableEditDueToActiveExitCode = false;
  cloneEnrollmentForCancel;
  cloneOfCloneEnrollmentForCancel;
  cloneStudentEnrollment: StudentEnrollmentModel = new StudentEnrollmentModel();

  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();
  sectionList: GetAllSectionModel = new GetAllSectionModel();
  destroySubject$: Subject<void> = new Subject();
  sectionNameInViewMode = '-';
  studentActiveStatus = true;
  constructor(private calendarService: CalendarService,
              private studentService: StudentService,
              private snackbar: MatSnackBar,
              private cryptoService: CryptoService,
              private router: Router,
              private defaultValuesService: DefaultValuesService,
              private commonFunction: SharedFunction,
              private sectionService: SectionService,
              private defaultValueService: DefaultValuesService) {
  }

  ngOnInit(): void {
    this.studentService.studentCreatedMode.subscribe((res)=>{
      this.studentCreateMode = res;
 
    });
    this.studentService.studentDetailsForViewedAndEdited.subscribe((res)=>{
      this.studentDetailsForViewAndEdit = res;
      if(res?.fieldsCategoryList){
       if(this.studentCreateMode === this.studentCreate.VIEW) {
          // this.getAllSchoolListWithGradeLevels();
          this.getAllStudentEnrollments();
          this.studentService.changePageMode(this.studentCreateMode);
        }
      }
    });

    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 3);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 5);
    const permissionSubCategory = permissionCategory.permissionSubcategory.find( x => x.permissionSubcategoryId === 4);
    this.editPermission = permissionSubCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionSubCategory.rolePermission[0].canDelete;
    this.addPermission = permissionSubCategory.rolePermission[0].canAdd;

    if (this.studentCreateMode === this.studentCreate.EDIT) {
      this.studentCreateMode = this.studentCreate.ADD;
    }
    if (this.studentCreateMode === this.studentCreate.ADD) {
      // this.getAllCalendar();
      this.getAllSchoolListWithGradeLevelsAndEnrollCodes();
      this.getAllStudentEnrollments();
      this.studentService.changePageMode(this.studentCreateMode);
    }
    
  }

  cmpare(index) {
    return index;
  }

  onSchoolChange(schoolId, indexOfDynamicRow) {
    const index = this.schoolListWithGradeLevelsAndEnrollCodes.findIndex((x) => {
      return x.schoolId === +schoolId;
    });
    this.selectedSchoolIndex[indexOfDynamicRow] = index;

    this.cloneStudentEnrollment.studentEnrollments[indexOfDynamicRow].schoolName = this.schoolListWithGradeLevelsAndEnrollCodes[index].schoolName;
  }

  onTransferredSchoolChange(transferredSchoolId, indexOfDynamicRow) {
    const index = this.schoolListWithGradeLevelsAndEnrollCodes.findIndex((x) => {
      return x.schoolId === +transferredSchoolId;
    });
    this.selectedTransferredSchoolIndex[indexOfDynamicRow] = index;
    this.cloneStudentEnrollment.studentEnrollments[indexOfDynamicRow].schoolTransferred = this.schoolListWithGradeLevelsAndEnrollCodes[index].schoolName;

  }

  onEnrollmentCodeChange(enrollmentCode, indexOfDynamicRow) {

  }

  onExitCodeChange(value, indexOfDynamicRow) {
    this.schoolListWithGradeLevelsAndEnrollCodes[this.selectedSchoolIndex[indexOfDynamicRow]].studentEnrollmentCode?.map((item) => {
      if (item.enrollmentCode === +value) {
        this.selectedExitCodes[indexOfDynamicRow] = item.type;
      }
    })

  }

  addMoreEnrollments() {
    this.cloneStudentEnrollment.studentEnrollments.push(new StudentEnrollmentDetails)
    this.divCount.push(2); // Why 2? We have to fill up the divCount, It could be anything.
  }

  deleteDynamicRow(indexOfDynamicRow) {
    this.divCount.splice(indexOfDynamicRow, 1);
    this.cloneStudentEnrollment.studentEnrollments.splice(indexOfDynamicRow, 1);
    this.selectedSchoolIndex.splice(indexOfDynamicRow, 1);
    this.selectedTransferredSchoolIndex.splice(indexOfDynamicRow, 1);
  }

  getAllCalendar() {
    this.calendarService.getAllCalendar(this.calendarListModel).subscribe((res) => {
      const allCalendarsInCurrentSchool = res.calendarList;
      this.calendarListModel.calendarList = allCalendarsInCurrentSchool.filter((x) => {
        return (x.academicYear === +this.defaultValueService.getAcademicYear() && x.defaultCalender);
      });
      this.manipulateModelInEditMode();
    });

  }

  getAllSchoolListWithGradeLevelsAndEnrollCodes() {
    this.studentService.studentEnrollmentSchoolList(this.schoolListWithGrades).subscribe(res => {
      this.schoolListWithGradeLevelsAndEnrollCodes = res.schoolMaster;
      for (let i = 0; i < this.cloneStudentEnrollment.studentEnrollments.length; i++) {
        this.selectedSchoolIndex[i] = this.schoolListWithGradeLevelsAndEnrollCodes.findIndex((x) => {
          return x.schoolId === +this.cloneStudentEnrollment.studentEnrollments[i].schoolId;
        });
        this.selectedTransferredSchoolIndex[i] = this.schoolListWithGradeLevelsAndEnrollCodes.findIndex((x) => {
          return x.schoolId === +this.cloneStudentEnrollment.studentEnrollments[i].transferredSchoolId;
        });
      }
      this.findEnrollmentCodeIdByName();
      // this.findExitCodeIdByName();
    });

  }

  editEnrollmentInfo() {
    this.studentCreateMode = this.studentCreate.EDIT;
    this.getAllSchoolListWithGradeLevelsAndEnrollCodes();
    this.studentService.changePageMode(this.studentCreateMode);
  }

  cancelEdit() {
    // if(this.studentEnrollmentModel!==this.cloneEnrollmentForCancel){
    //   this.studentEnrollmentModel=JSON.parse(this.cloneEnrollmentForCancel);
    // }
    // if(this.cloneStudentEnrollment!==this.cloneOfCloneEnrollmentForCancel){
    //   this.cloneStudentEnrollment=JSON.parse(this.cloneOfCloneEnrollmentForCancel);
    // }
    this.studentCreateMode = this.studentCreate.VIEW;
    this.replaceEnrollmentCodeWithName();
    this.studentService.changePageMode(this.studentCreateMode);
  }
  replaceEnrollmentCodeWithName() {
    for (let i = 0; i < this.studentEnrollmentModel.studentEnrollmentListForView?.length; i++) {
      const index = this.schoolListWithGradeLevelsAndEnrollCodes.findIndex((x) => {
        return x.schoolId == +this.studentEnrollmentModel.studentEnrollmentListForView[i].schoolId;
      });
      for (let j = 0; j < this.schoolListWithGradeLevelsAndEnrollCodes[index].studentEnrollmentCode?.length; j++) {
        if (this.studentEnrollmentModel.studentEnrollmentListForView[i].enrollmentCode == this.schoolListWithGradeLevelsAndEnrollCodes[index].studentEnrollmentCode[j].enrollmentCode.toString()) {
          this.studentEnrollmentModel.studentEnrollmentListForView[i].enrollmentCode = this.schoolListWithGradeLevelsAndEnrollCodes[index].studentEnrollmentCode[j].title;
          break;
        }
      }
    }
  }


  toggleEnrollmentHistory() {
    this.expandEnrollmentHistory = !this.expandEnrollmentHistory;
  }


  getAllStudentEnrollments() {
    const details = this.studentService.getStudentDetails();
    if (details != null) {
      this.studentEnrollmentModel.studentGuid = details.studentMaster.studentGuid;
    } else {
      this.studentEnrollmentModel.studentGuid = this.studentDetailsForViewAndEdit.studentMaster.studentGuid;
    }
    this.studentEnrollmentModel.studentId = this.studentService.getStudentId();
    this.studentEnrollmentModel.tenantId = this.defaultValueService.getTenantID();
    this.studentEnrollmentModel.schoolId = this.defaultValueService.getSchoolID();
    this.studentEnrollmentModel.academicYear = this.defaultValueService.getAcademicYear()?.toString();
    if (this.studentCreateMode === this.studentCreate.VIEW) {
      this.studentEnrollmentModel.studentGuid = this.studentDetailsForViewAndEdit.studentMaster.studentGuid;
    }

    this.studentService.getAllStudentEnrollment(this.studentEnrollmentModel).subscribe(res => {
      if (res){
        if (res._failure) {
          this.snackbar.open( res._message, '', {
            duration: 10000
          });
        } else {
          this.studentEnrollmentModel = res;
          this.getAllSection();
          this.findSectionNameById();
          this.cloneStudentEnrollment.studentEnrollments = res.studentEnrollmentListForView.filter((item) => {
            return item.exitCode == null;
          });
          this.cloneEnrollmentForCancel = JSON.stringify(this.studentEnrollmentModel);
          this.cloneOfCloneEnrollmentForCancel = JSON.stringify(this.cloneStudentEnrollment);
          for (let i = 0; i < this.cloneStudentEnrollment.studentEnrollments?.length; i++) {
            this.divCount[i] = i;
          }
          this.getAllCalendar();
        }
      }
      else{
        this.snackbar.open(this.defaultValueService.translateKey('studentEnrollmentsfailedtofetch') + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
    });
  }


  findEnrollmentCodeIdByName() {
    for (let i = 0; i < this.studentEnrollmentModel.studentEnrollmentListForView?.length; i++) {
      const index = this.schoolListWithGradeLevelsAndEnrollCodes.findIndex((x) => {
        return x.schoolId == +this.studentEnrollmentModel.studentEnrollmentListForView[i].schoolId;
      });
      for (let j = 0; j < this.schoolListWithGradeLevelsAndEnrollCodes[index].studentEnrollmentCode?.length; j++) {
        if (this.studentEnrollmentModel.studentEnrollmentListForView[i].enrollmentCode == this.schoolListWithGradeLevelsAndEnrollCodes[index].studentEnrollmentCode[j].title) {
          this.studentEnrollmentModel.studentEnrollmentListForView[i].enrollmentCode = this.schoolListWithGradeLevelsAndEnrollCodes[index].studentEnrollmentCode[j].enrollmentCode.toString();
          break;
        }
      }

    }
  }

  findCalendarNameById() {
    if (this.calendarListModel.calendarList != null) {
      for (let i = 0; i < this.calendarListModel.calendarList?.length; i++) {
        if (this.studentEnrollmentModel.calenderId != null && this.calendarListModel.calendarList[i].calenderId.toString() == this.studentEnrollmentModel.calenderId.toString()) {
          this.calendarNameInViewMode = this.calendarListModel.calendarList[i].title;
        } else {
          this.calendarNameInViewMode = '-';
        }
      }
    }
  }

  manipulateModelInEditMode() {
    this.filteredEnrollmentInfoInViewMode = this.studentEnrollmentModel.studentEnrollmentListForView.filter((x) => {
      return x.exitCode == null;
    });
    this.filteredEnrollmentInfoInViewMode=this.filteredEnrollmentInfoInViewMode.reverse();
    if(this.filteredEnrollmentInfoInViewMode?.length==1){
      this.filteredEnrollmentInfoInViewMode[0]?.enrollmentType == 'Drop'?this.studentActiveStatus=false:''
    }
    if (this.studentEnrollmentModel.calenderId != null) {
      for (let i = 0; i < this.studentEnrollmentModel.studentEnrollmentListForView?.length; i++) {
        this.studentEnrollmentModel.calenderId = this.studentEnrollmentModel.calenderId.toString();
      }
    }
    for (let i = 0; i < this.cloneStudentEnrollment.studentEnrollments.length; i++) {
      this.selectedExitCodes[i] = null;
      this.cloneStudentEnrollment.studentEnrollments[i].schoolId = this.studentEnrollmentModel.studentEnrollmentListForView[i].schoolId?.toString();
      this.cloneStudentEnrollment.studentEnrollments[i].gradeId = this.studentEnrollmentModel.studentEnrollmentListForView[i].gradeId?.toString();
    }
    this.findCalendarNameById();
  }

  updateStudentEnrollment() {
    this.currentForm.form.markAllAsTouched();
    if (this.currentForm.form.invalid) {
      return;
    }
    delete this.studentEnrollmentModel.studentEnrollmentListForView;
    this.studentEnrollmentModel.studentEnrollments = this.cloneStudentEnrollment.studentEnrollments;
    const details = this.studentService.getStudentDetails();
    for (let i = 0; i < this.studentEnrollmentModel.studentEnrollments?.length; i++) {
      this.studentEnrollmentModel.studentEnrollments[i].studentId = this.studentService.getStudentId();
      this.studentEnrollmentModel.studentEnrollments[i].academicYear = +this.defaultValueService.getAcademicYear();
      this.studentEnrollmentModel.studentEnrollments[i].enrollmentDate = this.commonFunction.formatDateSaveWithoutTime(this.studentEnrollmentModel.studentEnrollments[i].enrollmentDate)
      this.studentEnrollmentModel.studentEnrollments[i].exitDate = this.commonFunction.formatDateSaveWithoutTime(this.studentEnrollmentModel.studentEnrollments[i].exitDate)
      if (details != null) {
        this.studentEnrollmentModel.studentEnrollments[i].studentGuid = details.studentMaster.studentGuid;
      } else {
        this.studentEnrollmentModel.studentEnrollments[i].studentGuid = this.studentDetailsForViewAndEdit.studentMaster.studentGuid;

      }
    }
    if (details != null) {
      this.studentEnrollmentModel.studentGuid = details.studentMaster.studentGuid;
    } else {
      this.studentEnrollmentModel.studentGuid = this.studentDetailsForViewAndEdit.studentMaster.studentGuid;
    }
    this.studentEnrollmentModel.estimatedGradDate = this.commonFunction.formatDateSaveWithoutTime(this.studentEnrollmentModel.estimatedGradDate)

    this.studentEnrollmentModel.academicYear = this.defaultValueService.getAcademicYear()?.toString();
    this.studentEnrollmentModel.schoolId = +this.defaultValueService.getSchoolID();
    this.studentEnrollmentModel._userName = this.defaultValueService.getUserName();

    this.studentService.updateStudentEnrollment(this.studentEnrollmentModel).subscribe((res) => {
      if (res){
        if (res._failure) {
          this.snackbar.open( res._message, '', {
            duration: 10000
          });
        } else {
          this.snackbar.open(res._message, '', {
            duration: 10000
          });
          if (res.rollingOption === RollingOptionsEnum['Do not enroll after this school year']){
            res.studentEnrollments.map((item) => {
                if (+item.exitCode === 2){
              this.router.navigate(['school/students']);
                }
            });
          }
          for (let i = 0; i < res.studentEnrollments?.length; i++) {
            if (res.studentEnrollments[i].enrollmentCode === 'Transferred In') {
              this.router.navigate(['school/students']);
            }
          }
          this.getAllStudentEnrollments();
          // this.studentEnrollmentModel=res;
          // this.replaceEnrollmentCodeWithName();
          this.studentCreateMode = this.studentCreate.VIEW;
          this.studentService.changePageMode(this.studentCreateMode);
        }
      }
      else{
        this.snackbar.open( this.defaultValueService.translateKey('enrollmentUpdatefailed') + sessionStorage.getItem('httpError'), '', {
          duration: 10000
        });
      }
    });
  }

  getAllSection() {
    if (!this.sectionList.isSectionAvailable){
      this.sectionList.isSectionAvailable = true;
      this.sectionService.GetAllSection(this.sectionList).pipe(takeUntil(this.destroySubject$)).subscribe(res => {
        if (res._failure) {
        }
        else {
          this.sectionList.tableSectionsList = res.tableSectionsList;
          if (this.studentCreateMode === this.studentCreate.VIEW) {
           this.findSectionNameById();
          }
        }
      });
    }
  }

  findSectionNameById(){
    this.sectionList?.tableSectionsList?.map((item) => {
      if (item.sectionId === +this.studentEnrollmentModel.sectionId) {
        this.sectionNameInViewMode = item.name;
      }
    })
  }

  onGradeLevelChange(indexOfDynamicRow){
    const indexOfGradeLevel = this.schoolListWithGradeLevelsAndEnrollCodes[this.selectedSchoolIndex[indexOfDynamicRow]]?.gradelevels.findIndex((item) => {
      return item.gradeId === +this.cloneStudentEnrollment.studentEnrollments[indexOfDynamicRow].gradeId;
    });
    this.cloneStudentEnrollment.studentEnrollments[indexOfDynamicRow].gradeLevelTitle = this.schoolListWithGradeLevelsAndEnrollCodes[this.selectedSchoolIndex[indexOfDynamicRow]]?.gradelevels[indexOfGradeLevel].title;
  }

  ngOnDestroy() {
    this.destroySubject$.next();
    this.destroySubject$.complete();

  }

}
