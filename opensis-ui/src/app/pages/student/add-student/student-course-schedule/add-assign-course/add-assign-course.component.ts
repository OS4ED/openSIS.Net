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

import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import icClose from '@iconify/icons-ic/twotone-close';
import { TranslateService } from '@ngx-translate/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AllCourseSectionView, GetAllCourseListModel, GetAllProgramModel, GetAllSubjectModel, SearchCourseForScheduleModel, SearchCourseSectionViewModel } from '../../../../../models/course-manager.model';
import { GetMarkingPeriodTitleListModel } from '../../../../../models/marking-period.model';
import { MatTableDataSource } from '@angular/material/table';
import { Subject } from 'rxjs';
import { SelectionModel } from '@angular/cdk/collections';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { LoaderService } from '../../../../../services/loader.service';
import { takeUntil } from 'rxjs/operators';
import { CourseManagerService } from '../../../../../services/course-manager.service';
import { MarkingPeriodService } from '../../../../../services/marking-period.service';
import { CourseSectionService } from '../../../../../services/course-section.service';
import { DefaultValuesService } from '../../../../../common/default-values.service';
import { StudentScheduleService } from '../../../../../services/student-schedule.service';
import { StudentCourseSectionScheduleAddViewModel } from '../../../../../models/student-schedule.model';
import { StudentService } from '../../../../../services/student.service';
import { StudentMasterModel } from '../../../../../models/student.model';


@Component({
  selector: 'vex-add-assign-course',
  templateUrl: './add-assign-course.component.html',
  styleUrls: ['./add-assign-course.component.scss']
})
export class AddAssignCourseComponent implements OnInit {
  icClose = icClose;

  getAllProgramModel: GetAllProgramModel = new GetAllProgramModel();
  getAllSubjectModel: GetAllSubjectModel = new GetAllSubjectModel();
  getAllCourseListModel: GetAllCourseListModel = new GetAllCourseListModel();
  getMarkingPeriodTitleListModel: GetMarkingPeriodTitleListModel = new GetMarkingPeriodTitleListModel();
  displayedColumns: string[] = ['staffSelected', 'course', 'courseSectionName', 'markingPeriod', 'startDate', 'endDate', 'seats', 'available'];
  courseSectionSearch: SearchCourseSectionViewModel = new SearchCourseSectionViewModel();
  courseSectionList: MatTableDataSource<any>;
  loading: boolean;
  destroySubject$: Subject<void> = new Subject();
  selection: SelectionModel<AllCourseSectionView> = new SelectionModel<AllCourseSectionView>(true, []);
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  searching: boolean = false;
  studentCourseSectionScheduleAddViewModel: StudentCourseSectionScheduleAddViewModel = new StudentCourseSectionScheduleAddViewModel();
  studentAssigningLoader: boolean = false;
  constructor(public translateService: TranslateService,
    private defaultService: DefaultValuesService,
    private courseManagerService: CourseManagerService,
    private snackbar: MatSnackBar,
    private courseSectionService: CourseSectionService,
    private loaderService: LoaderService,
    private studentScheduleService: StudentScheduleService,
    private dialogRef: MatDialogRef<AddAssignCourseComponent>,
    private studentService: StudentService,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    this.getMarkingPeriodTitleListModel.getMarkingPeriodView = data.markingPeriod;
    translateService.use('en');
    this.loaderService.isLoading.pipe(takeUntil(this.destroySubject$)).subscribe((val) => {
      this.loading = val;
    });
  }

  ngOnInit(): void {
    this.getAllCourse();
    this.getAllSubjectList();
    this.getAllProgramList();
  }


  getAllProgramList() {
    this.courseManagerService.GetAllProgramsList(this.getAllProgramModel).subscribe(data => {
      this.getAllProgramModel.programList = data.programList;
    });
  }
  getAllSubjectList() {
    this.courseManagerService.GetAllSubjectList(this.getAllSubjectModel).subscribe(data => {
      this.getAllSubjectModel.subjectList = data.subjectList;
    });
  }

  getAllCourse() {
    this.courseManagerService.GetAllCourseList(this.getAllCourseListModel).subscribe(data => {
      if (data._failure) {
        this.getAllCourseListModel.courseViewModelList = [];
      } else {
        this.getAllCourseListModel.courseViewModelList = data.courseViewModelList;
      }
    })
  }

  onSearchCriteriaChange() {
    this.searchCourseSection();
  }

  searchCourseSection() {
    this.searching = true;
    let searchCriteriaList: SearchCourseSectionViewModel = new SearchCourseSectionViewModel();
    searchCriteriaList = this.checkForBlankCriteria(searchCriteriaList);
    searchCriteriaList.courseId = this.courseSectionSearch.courseId;
    searchCriteriaList.forStudent = true;
    this.courseSectionService.searchCourseSectionForSchedule(searchCriteriaList).subscribe((res) => {
      if (res._failure) {
        if (res.allCourseSectionViewList === null) {
          this.courseSectionList = new MatTableDataSource([]);
          this.snackbar.open(res._message, '', {
            duration: 5000
          });
        } else {
          this.courseSectionList = new MatTableDataSource([]);
        }
      } else {
        res.allCourseSectionViewList = this.findMarkingPeriodTitleById(res.allCourseSectionViewList)
        this.courseSectionList = new MatTableDataSource(res.allCourseSectionViewList);
        this.courseSectionList.paginator = this.paginator;
      }
    })
  }

  checkForBlankCriteria(searchCriteriaList: SearchCourseSectionViewModel) {
    if (!this.courseSectionSearch.courseSubject) {
      searchCriteriaList.courseSubject = null;
    } else {
      searchCriteriaList.courseSubject = this.courseSectionSearch.courseSubject;
    }

    if (!this.courseSectionSearch.courseProgram) {
      searchCriteriaList.courseProgram = null;
    } else {
      searchCriteriaList.courseProgram = this.courseSectionSearch.courseProgram;
    }

    if (!this.courseSectionSearch.markingPeriodId) {
      let mpStartDate = this.defaultService.getMarkingPeriodStartDate();
      searchCriteriaList.markingPeriodId = null;
      searchCriteriaList.markingPeriodStartDate=mpStartDate?mpStartDate.split('T')[0]:null;
    } else {
      searchCriteriaList.markingPeriodId = this.courseSectionSearch.markingPeriodId;
      searchCriteriaList.markingPeriodStartDate=null;
    }

    return searchCriteriaList;
  }



  findMarkingPeriodTitleById(courseSectionList) {

    courseSectionList = courseSectionList.map((item) => {
      if (item.yrMarkingPeriodId) {
        item.yrMarkingPeriodId = '0_' + item.yrMarkingPeriodId;
      } else if (item.smstrMarkingPeriodId) {
        item.smstrMarkingPeriodId = '1_' + item.smstrMarkingPeriodId;
      } else if (item.qtrMarkingPeriodId) {
        item.qtrMarkingPeriodId = '2_' + item.qtrMarkingPeriodId;
      }

      if (item.yrMarkingPeriodId || item.smstrMarkingPeriodId || item.qtrMarkingPeriodId) {
        for (let markingPeriod of this.getMarkingPeriodTitleListModel.getMarkingPeriodView) {
          if (markingPeriod.value == item.yrMarkingPeriodId) {
            item.markingPeriodTitle = markingPeriod.text;
            break;
          } else if (markingPeriod.value == item.smstrMarkingPeriodId) {
            item.markingPeriodTitle = markingPeriod.text;
            break;
          } else if (markingPeriod.value == item.qtrMarkingPeriodId) {
            item.markingPeriodTitle = markingPeriod.text;
            break;
          }
        }
      } else {
        item.markingPeriodTitle = 'Custom'
      }
      return item;
    });
    return courseSectionList;
  }

  /** Whether the number of selected elements matches the total number of rows. */
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    return numSelected === this.courseSectionList.paginator.pageSize;
  }

  checked(row: any) {
    this.selection.select(row);

    let found = this.selection.selected.find(x => x.courseSectionId == row.courseSectionId);

    if (found) {
      return true;
    }
  }

  unChecked(row: any) {

    let found = this.selection.selected.find(x => x.courseSectionId == row.courseSectionId);
    this.selection.deselect(found);

    if (found) {
      return false;
    }

  }

  isChecked(row: any) {
    let found = this.selection.selected.find(x => x.courseSectionId == row.courseSectionId);

    if (found) {
      return true;
    }
  }



  selectRows() {
    for (let index = 0; index < this.courseSectionList.paginator.pageSize; index++) {
      if (this.courseSectionList.data[index]) {
        this.selection.select(this.courseSectionList.data[index]);
      }
      // this.selectionAmount = this.selection.selected.length;
    }
  }

  removeSelection(courseSection) {
    this.selection.deselect(courseSection)
  }

  assignStudent() {
    if (this.selection.selected.length === 0) {
      this.snackbar.open('Please select at least one course section', "", {
        duration: 10000,
      });
      return;
    }
    this.studentAssigningLoader = true;
    let courseSectionList = [];
    courseSectionList = [...this.selection.selected]
    courseSectionList = this.normalizeMarkingPeriodId(courseSectionList)
    let studentDetails: StudentMasterModel[] = [new StudentMasterModel];
    studentDetails[0].studentId = this.studentService.getStudentId();
    studentDetails[0].firstGivenName = this.studentService.getStudentName().firstGivenName;
    studentDetails[0].lastFamilyName = this.studentService.getStudentName().lastFamilyName;

    this.studentCourseSectionScheduleAddViewModel.courseSectionList = courseSectionList;
    this.studentCourseSectionScheduleAddViewModel.studentMasterList = studentDetails;
    this.studentCourseSectionScheduleAddViewModel.createdBy = this.defaultService.getEmailId();
    this.studentScheduleService.addStudentCourseSectionSchedule(this.studentCourseSectionScheduleAddViewModel).pipe(takeUntil(this.destroySubject$)).subscribe(res => {
      if (res) {
        if (res._conflictFailure) {
          this.snackbar.open(res.conflictMessage, "", {
            duration: 10000,
          });
        } else {
          this.snackbar.open(res._message, "", {
            duration: 10000,
          });
          this.dialogRef.close(true);
        }
      } else {
        this.snackbar.open(sessionStorage.getItem("httpError"), "", {
          duration: 10000,
        }
        );
      }
      this.studentAssigningLoader = false;
    });
  }

  normalizeMarkingPeriodId(courseSectionList) {
    courseSectionList = courseSectionList.map((item) => {
      if (item.yrMarkingPeriodId) {
        let yrMarkingPeriodId = item.yrMarkingPeriodId.toString().split('_');
        item.yrMarkingPeriodId = parseInt(yrMarkingPeriodId[1]);
      } else if (item.smstrMarkingPeriodId) {
        let smstrMarkingPeriodId = item.smstrMarkingPeriodId.toString().split('_');
        item.smstrMarkingPeriodId = parseInt(smstrMarkingPeriodId[1]);
      } else if (item.qtrMarkingPeriodId) {
        let qtrMarkingPeriodId = item.qtrMarkingPeriodId.toString().split('_');
        item.qtrMarkingPeriodId = parseInt(qtrMarkingPeriodId[1]);
      }
      return item;
    });
    return courseSectionList;
  }

  ngOnDestroy() {
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }
}
