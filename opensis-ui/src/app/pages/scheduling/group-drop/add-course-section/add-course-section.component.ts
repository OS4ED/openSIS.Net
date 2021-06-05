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

import { Component, OnInit, ViewChild } from '@angular/core';
import { SearchStudentCourseSection } from '../../../../models/search-student-course-section.model';
import icClose from '@iconify/icons-ic/twotone-close';
import { TranslateService } from '@ngx-translate/core';
import { AllCourseSectionView, GetAllCourseListModel, GetAllProgramModel, GetAllSubjectModel, SearchCourseSectionViewModel } from '../../../../models/course-manager.model';
import { GetAllMarkingPeriodTitle, GetMarkingPeriodTitleListModel } from '../../../../models/marking-period.model';
import { Subject } from 'rxjs';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialogRef } from '@angular/material/dialog';
import { CourseManagerService } from '../../../../services/course-manager.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MarkingPeriodService } from '../../../../services/marking-period.service';
import { LoaderService } from '../../../../services/loader.service';
import { CourseSectionService } from '../../../../services/course-section.service';
import { takeUntil } from 'rxjs/operators';
import { NgForm } from '@angular/forms';
import { DefaultValuesService } from 'src/app/common/default-values.service';

@Component({
  selector: 'vex-add-course-section',
  templateUrl: './add-course-section.component.html',
  styleUrls: ['./add-course-section.component.scss']
})
export class AddCourseSectionComponent implements OnInit {

  getAllProgramModel: GetAllProgramModel = new GetAllProgramModel();
  getAllSubjectModel: GetAllSubjectModel = new GetAllSubjectModel();
  getAllCourseListModel: GetAllCourseListModel = new GetAllCourseListModel();
  getMarkingPeriodTitleListModel: GetMarkingPeriodTitleListModel = new GetMarkingPeriodTitleListModel();
  courseSectionSearch: SearchCourseSectionViewModel = new SearchCourseSectionViewModel();
  programList = [];
  subjectList = [];
  courseList = [];
  markingPeriodList = [];
  searchRecord: boolean = false;
  loading: boolean;
  destroySubject$: Subject<void> = new Subject();
  @ViewChild('f') currentForm: NgForm;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  courseDetails: MatTableDataSource<any>;
  icClose = icClose;
  displayedColumns: string[] = ['course', 'courseSection', 'markingPeriod', 'startDate', 'endDate', 'scheduledTeacher'];

  constructor(public translateService: TranslateService,
    private dialogRef: MatDialogRef<AddCourseSectionComponent>,
    private courseManagerService: CourseManagerService,
    private defaultService:DefaultValuesService,
    private snackbar: MatSnackBar,
    private markingPeriodService: MarkingPeriodService,
    private loaderService: LoaderService,
    private courseSectionService: CourseSectionService) {
    translateService.use('en');
    this.loaderService.isLoading.pipe(takeUntil(this.destroySubject$)).subscribe((val) => {
      this.loading = val;
    });
  }

  ngOnInit(): void {
    this.getAllCourse();
    this.getAllSubjectList();
    this.getAllProgramList();
    this.getAllMarkingPeriodList();
  }


  getAllProgramList() {
    this.courseManagerService.GetAllProgramsList(this.getAllProgramModel).subscribe(data => {
      this.programList = data.programList;
    });
  }
  getAllSubjectList() {
    this.courseManagerService.GetAllSubjectList(this.getAllSubjectModel).subscribe(data => {
      this.subjectList = data.subjectList;

    });
  }

  getAllMarkingPeriodList() {
    this.getMarkingPeriodTitleListModel.schoolId = +sessionStorage.getItem("selectedSchoolId");
    this.getMarkingPeriodTitleListModel.academicYear = +sessionStorage.getItem("academicyear");
    this.markingPeriodService.getAllMarkingPeriodList(this.getMarkingPeriodTitleListModel).subscribe(data => {
      if (data._failure) {
        this.getMarkingPeriodTitleListModel.getMarkingPeriodView = [];
      } else {
        this.getMarkingPeriodTitleListModel.getMarkingPeriodView = data.getMarkingPeriodView;
      }
    });
  }

  getAllCourse() {
    this.courseManagerService.GetAllCourseList(this.getAllCourseListModel).subscribe(data => {
      if (data._failure) {
        this.courseList = [];
      } else {
        this.courseList = data.courseViewModelList;

      }
    })
  }

  searchCourseSection() {
    this.searchRecord = true;
    if(this.courseSectionSearch.markingPeriodId){
      this.courseSectionSearch.markingPeriodStartDate=null;
    }else{
      let mpStartDate = this.defaultService.getMarkingPeriodStartDate();
      this.courseSectionSearch.markingPeriodStartDate=mpStartDate?mpStartDate.split('T')[0]:null;
    }
    this.courseSectionSearch.forStudent = true;
    this.courseSectionService.searchCourseSectionForSchedule(this.courseSectionSearch).subscribe((res) => {
      if (res._failure) {
        if (res.allCourseSectionViewList === null) {
          this.courseDetails = new MatTableDataSource([]);
          this.snackbar.open(res._message, '', {
            duration: 5000
          });
        } else {
          this.courseDetails = new MatTableDataSource([]);
        }
      } else {
        res.allCourseSectionViewList = this.findMarkingPeriodTitleById(res.allCourseSectionViewList);
        this.courseDetails = new MatTableDataSource(res.allCourseSectionViewList);
        this.courseDetails.paginator = this.paginator;
      }
    })
  }

  findMarkingPeriodTitleById(courseDetails) {

    courseDetails = courseDetails.map((item) => {
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
          } else if (markingPeriod.value == item.qtrMarkingPeriodId){
            item.markingPeriodTitle = markingPeriod.text;
            break;
          }
        }
      } else {
        item.markingPeriodTitle = 'Custom'
      }
      return item;
    });
    return courseDetails;
  }

  cellClicked(element) {
    this.dialogRef.close(element);
  }

}
