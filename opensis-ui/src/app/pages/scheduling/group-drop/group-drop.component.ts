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

import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AddCourseSectionComponent } from './add-course-section/add-course-section.component';
import icClose from '@iconify/icons-ic/twotone-close';
import { fadeInUp400ms } from '../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../@vex/animations/stagger.animation';
import { TranslateService } from '@ngx-translate/core';
import { StudentScheduleService } from '../../../services/student-schedule.service';
import { ScheduledStudentDropModel, ScheduleStudentForView, ScheduleStudentListViewModel } from '../../../models/student-schedule.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { SelectionModel } from '@angular/cdk/collections';
import { LoaderService } from '../../../services/loader.service';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { SharedFunction } from '../../shared/shared-function';
import { MatCheckbox } from '@angular/material/checkbox';

@Component({
  selector: 'vex-group-drop',
  templateUrl: './group-drop.component.html',
  styleUrls: ['./group-drop.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class GroupDropComponent implements OnInit, OnDestroy {
  icClose = icClose;
  selectDropDate: string;
  courseSectionData;
  startDate = new Date();
  listOfStudent = [];
  selectedStudent = [];
  totalCount: number = 0;
  pageNumber: number;
  pageSize: number;
  endDate;
  loading: boolean;
  dropSuccess: boolean = false;
  startDropping: boolean = false;
  studentNotFound: boolean = false;
  dropMessage: string;
  destroySubject$: Subject<void> = new Subject();
  scheduleStudentListViewModel: ScheduleStudentListViewModel = new ScheduleStudentListViewModel();
  scheduledStudentDropModel: ScheduledStudentDropModel = new ScheduledStudentDropModel();
  showcourseSectionCount: boolean;
  studentMasterList: ScheduleStudentForView[];
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild('masterCheckBox') masterCheckBox: MatCheckbox;
  studentDetails: MatTableDataSource<ScheduleStudentForView>;
  selection: SelectionModel<ScheduleStudentForView> = new SelectionModel<ScheduleStudentForView>(true, []);

  displayedColumns: string[] = ['studentSelected', 'studentName', 'studentId', 'alternateId', 'gradeLevel', 'section', 'phoneNumber', 'action'];

  constructor(private dialog: MatDialog, public translateService: TranslateService,
    private studentScheduleService: StudentScheduleService,
    private snackbar: MatSnackBar,
    private commonFunction: SharedFunction,
    private loaderService: LoaderService) {
    translateService.use('en');
    this.loaderService.isLoading.pipe(takeUntil(this.destroySubject$)).subscribe((val) => {
      this.loading = val;
    });

  }


  ngOnInit(): void {
  }


  selectCourseSection() {
    this.dialog.open(AddCourseSectionComponent, {
      width: '900px'
    }).afterClosed().subscribe((data) => {
      this.courseSectionData = data;
      if (this.courseSectionData !== '') {
        this.startDate = new Date();
        this.endDate = this.courseSectionData.durationEndDate;
        this.showcourseSectionCount = true;
        this.searchScheduledStudentForGroupDrop(this.courseSectionData.courseSectionId);
      }
      else {
        this.showcourseSectionCount = false;
      }
    });
  }

  someComplete(): boolean {
    let indetermine = false;
    for (let user of this.listOfStudent) {
      for (let selectedUser of this.selectedStudent) {
        if (user.StudentId == selectedUser.StudentId) {
          indetermine = true;
        }
      }
    }
    if (indetermine) {
      this.masterCheckBox.checked = this.listOfStudent.every((item) => {
        return item.checked;
      })
      if (this.masterCheckBox.checked) {
        return false;
      } else {
        return true;
      }
    }

  }

  setAll(event) {
    this.listOfStudent.forEach(user => {
      user.checked = event;
    });
    this.studentDetails = new MatTableDataSource(this.listOfStudent);
    this.decideCheckUncheck();
  }

  onChangeSelection(eventStatus: boolean, id) {
    for (let item of this.listOfStudent) {
      if (item.studentId == id) {
        item.checked = eventStatus;
        break;
      }
    }
    this.studentDetails = new MatTableDataSource(this.listOfStudent);
    this.masterCheckBox.checked = this.listOfStudent.every((item) => {
      return item.checked;
    });

    this.decideCheckUncheck();
  }

  decideCheckUncheck() {
    this.listOfStudent.map((item) => {
      let isIdIncludesInSelectedList = false;
      if (item.checked) {
        for (let selectedUser of this.selectedStudent) {
          if (item.studentId == selectedUser.studentId) {
            isIdIncludesInSelectedList = true;
            break;
          }
        }
        if (!isIdIncludesInSelectedList) {
          this.selectedStudent.push(item);
        }
      } else {
        for (let selectedUser of this.selectedStudent) {
          if (item.studentId == selectedUser.studentId) {
            this.selectedStudent = this.selectedStudent.filter((user) => user.studentId != item.studentId);
            break;
          }
        }
      }
      isIdIncludesInSelectedList = false;

    });
    this.selectedStudent = this.selectedStudent.filter((item) => item.checked);
  }
  getPageEvent(event) {
    this.scheduleStudentListViewModel.pageNumber = event.pageIndex + 1;
    this.scheduleStudentListViewModel._pageSize = event.pageSize;
    this.searchScheduledStudentForGroupDrop(this.courseSectionData.courseSectionId);
  }

  searchScheduledStudentForGroupDrop(courseSectionId) {
    this.selection = new SelectionModel<ScheduleStudentForView>(true, []);
    this.scheduleStudentListViewModel.sortingModel = null;
    this.scheduleStudentListViewModel.courseSectionId = courseSectionId
    this.studentScheduleService.searchScheduledStudentForGroupDrop(this.scheduleStudentListViewModel).subscribe((res) => {
      if (res._failure) {
        this.studentNotFound = true;
        this.dropMessage = res._message;
        this.studentDetails = new MatTableDataSource([]);
        this.totalCount = res.totalCount;
      } else {
        this.totalCount = res.totalCount;
        if (res.totalCount === 0 || res.totalCount === null) {
          this.studentNotFound = true;
          this.dropMessage = 'Student not found';
        }
        this.pageNumber = res.pageNumber;
        this.pageSize = res._pageSize;
        this.studentMasterList = res.scheduleStudentForView;
        this.studentDetails = new MatTableDataSource(this.studentMasterList);
        this.studentMasterList.forEach(user => {
          user.checked = false
        });
        let response = this.studentMasterList.map((item) => {
          this.selectedStudent.map((selectedUser) => {
            if (item.studentId == selectedUser.studentId) {
              item.checked = true;
              return item;
            }
          });
          return item;
        });
        this.listOfStudent = response;
        this.scheduleStudentListViewModel = new ScheduleStudentListViewModel();
      }
    });
  }

  dropGroupStudents() {
    let selectedStudents = this.selectedStudent.filter(item => item.action !== true);
    if (this.selectDropDate == undefined) {
      this.snackbar.open('Please select drop date', '', {
        duration: 5000
      });
      return;
    }
    else if (selectedStudents.length == 0) {
      this.snackbar.open('Please select any student', '', {
        duration: 5000
      });
      return;
    }
    else {
      this.startDropping = true;
      this.scheduledStudentDropModel.studentCoursesectionScheduleList = selectedStudents;
      this.scheduledStudentDropModel.effectiveDropDate = this.commonFunction.formatDateSaveWithoutTime(this.selectDropDate);
      this.scheduledStudentDropModel.courseSectionId = this.courseSectionData.courseSectionId;
      this.studentScheduleService.groupDropForScheduledStudent(this.scheduledStudentDropModel).subscribe((res) => {
        if (res._failure) {
          this.snackbar.open(res._message, '', {
            duration: 5000
          });
          this.dropMessage = res._message;
          this.studentNotFound = true;
        } else {
          this.dropMessage = res._message;
          this.dropSuccess = true;
          this.studentDetails = new MatTableDataSource([]);
          this.showcourseSectionCount = false;
          this.selectDropDate = null;
          this.totalCount = 0;
        }
      })
    }

  }

  ngOnDestroy(): void {
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }
}
