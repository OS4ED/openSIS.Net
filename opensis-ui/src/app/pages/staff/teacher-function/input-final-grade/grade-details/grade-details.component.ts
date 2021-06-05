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

import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { FinalGradeService } from '../../../../../services/final-grade.service';
import { fadeInRight400ms } from '../../../../../../@vex/animations/fade-in-right.animation';
import { fadeInUp400ms } from '../../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../../@vex/animations/stagger.animation';
import { AllScheduledCourseSectionForStaffModel } from '../../../../../models/teacher-schedule.model';
import { TeacherScheduleService } from '../../../../../services/teacher-schedule.service';
import { map } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ScheduleStudentForView, ScheduleStudentListViewModel } from '../../../../../models/student-schedule.model';
import { StudentScheduleService } from '../../../../../services/student-schedule.service';
import { GetMarkingPeriodTitleListModel } from '../../../../../models/marking-period.model';
import { MarkingPeriodService } from '../../../../../services/marking-period.service';
import { Router } from '@angular/router';
import { AddUpdateStudentFinalGradeModel, StudentFinalGrade, StudentFinalGradeStandard } from '../../../../../models/student-final-grade.model';
import { ReportCardService } from '../../../../../services/report-card.service';
import { GetAllCourseCommentCategoryModel } from '../../../../../models/report-card.model';
import { DefaultValuesService } from '../../../../../common/default-values.service';
import { CourseManagerService } from 'src/app/services/course-manager.service';
import { CourseStandardForCourseViewModel, GetAllCourseListModel } from 'src/app/models/course-manager.model';
import { LoaderService } from 'src/app/services/loader.service';
import { GradesService } from 'src/app/services/grades.service';
import { GradeScaleListView } from 'src/app/models/grades.model';
import { LayoutService } from 'src/@vex/services/layout.service';
import { MatAutocomplete, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { Observable } from 'rxjs';
import { MatChipInputEvent } from '@angular/material/chips';

@Component({
  selector: 'vex-grade-details',
  templateUrl: './grade-details.component.html',
  styleUrls: ['./grade-details.component.scss'],
  animations: [
    fadeInRight400ms,
    stagger60ms,
    fadeInUp400ms
  ]
})
export class GradeDetailsComponent implements OnInit {
  visible = true;
  selectable = true;
  removable = true;
  separatorKeysCodes: number[] = [ENTER, COMMA];
  filteredFruits: Observable<string[]>;
  fruits: string[] = ['Lemon'];
  allFruits: string[] = ['Apple', 'Lemon', 'Lime', 'Orange', 'Strawberry'];
  @ViewChild('fruitInput') fruitInput: ElementRef<HTMLInputElement>;

  pageStatus = "Grade Details";
  showComment: boolean = false;
  staffDetails;
  showMessage = "pleaseSelectCourseSectionAndMarkingPeriodForInputGrade";
  courseSectionData;
  selectedStudent: number = 0;
  reportCardComments = [];
  courseList = [];
  courseStandardList = [];
  gradeScaleStandardList = [];
  gradeScaleList = [];
  loading: boolean = false;
  isPercentChecked: boolean = false;
  reportCardGrade = [
    { id: 1, value: 'A' },
    { id: 2, value: 'B' },
    { id: 3, value: 'C' },
    { id: 4, value: 'D' },
    { id: 5, value: 'F' }
  ];
  totalCount: number = 0;
  @ViewChild('auto') matAutocomplete: MatAutocomplete;
  courseSectionId: number;
  courseSectionDetails=[];
  markingPeriodList = [];
  studentFinalGradeComments;
  gradeScaleListView: GradeScaleListView = new GradeScaleListView();
  courseStandardForCourseViewModel: CourseStandardForCourseViewModel = new CourseStandardForCourseViewModel();
  reportCardCategoryWithComments: GetAllCourseCommentCategoryModel = new GetAllCourseCommentCategoryModel();
  getAllReportCardCommentsWithCategoryModel: GetAllCourseCommentCategoryModel = new GetAllCourseCommentCategoryModel();
  allScheduledCourseSectionBasedOnTeacher: AllScheduledCourseSectionForStaffModel = new AllScheduledCourseSectionForStaffModel();
  addUpdateStudentFinalGradeModel: AddUpdateStudentFinalGradeModel = new AddUpdateStudentFinalGradeModel();
  scheduleStudentListViewModel: ScheduleStudentListViewModel = new ScheduleStudentListViewModel();
  getMarkingPeriodTitleListModel: GetMarkingPeriodTitleListModel = new GetMarkingPeriodTitleListModel();
  isPercent: boolean = false;
  studentMasterList: ScheduleStudentForView[];
  constructor(public translateService: TranslateService,
    private finalGradeService: FinalGradeService,
    private teacherReassignmentService: TeacherScheduleService,
    private snackbar: MatSnackBar,
    private studentScheduleService: StudentScheduleService,
    private markingPeriodService: MarkingPeriodService,
    private router: Router,
    private gradesService: GradesService,
    private reportCardService: ReportCardService,
    private defaultValuesService: DefaultValuesService,
    private courseManager: CourseManagerService,
    private loaderService: LoaderService,
    private layoutService: LayoutService
  ) {
    this.loaderService.isLoading.subscribe((val) => {
      this.loading = val;
    });
    translateService.use('en');
    this.staffDetails = this.finalGradeService.getStaffDetails();
    this.layoutService.collapseSidenav();


  }

  ngOnInit(): void {
    this.addUpdateStudentFinalGradeModel.isPercent = true;
    this.isPercentChecked = true;
    this.allScheduledCourseSectionBasedOnTeacher.staffId = this.staffDetails.staffId;
    if (this.allScheduledCourseSectionBasedOnTeacher.staffId) {

    }
    else {
      this.router.navigate(['/school', 'staff', 'teacher-functions', 'input-final-grade']);
    }
    this.getAllMarkingPeriodList();
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    if (this.addUpdateStudentFinalGradeModel.studentFinalGradeList[this.selectedStudent].studentFinalGradeComments.findIndex(item => item.courseCommentId === event.option.value.courseCommentId) === -1) {
      this.addUpdateStudentFinalGradeModel.studentFinalGradeList[this.selectedStudent].studentFinalGradeComments.push(event.option.value);
    }
  }

  percentToGrade(index) {
    if (this.addUpdateStudentFinalGradeModel.studentFinalGradeList[index].percentMarks >= 90 && this.addUpdateStudentFinalGradeModel.studentFinalGradeList[index].percentMarks <= 100) {
      this.addUpdateStudentFinalGradeModel.studentFinalGradeList[index].gradeObtained = 'A';

    }
    else if (this.addUpdateStudentFinalGradeModel.studentFinalGradeList[index].percentMarks >= 80 && this.addUpdateStudentFinalGradeModel.studentFinalGradeList[index].percentMarks < 90) {
      this.addUpdateStudentFinalGradeModel.studentFinalGradeList[index].gradeObtained = 'B';

    }
    else if (this.addUpdateStudentFinalGradeModel.studentFinalGradeList[index].percentMarks >= 70 && this.addUpdateStudentFinalGradeModel.studentFinalGradeList[index].percentMarks < 80) {
      this.addUpdateStudentFinalGradeModel.studentFinalGradeList[index].gradeObtained = 'C';

    }
    else if (this.addUpdateStudentFinalGradeModel.studentFinalGradeList[index].percentMarks >= 60 && this.addUpdateStudentFinalGradeModel.studentFinalGradeList[index].percentMarks < 70) {
      this.addUpdateStudentFinalGradeModel.studentFinalGradeList[index].gradeObtained = 'D';

    }
    else if (this.addUpdateStudentFinalGradeModel.studentFinalGradeList[index].percentMarks >= 0 && this.addUpdateStudentFinalGradeModel.studentFinalGradeList[index].percentMarks < 60) {
      this.addUpdateStudentFinalGradeModel.studentFinalGradeList[index].gradeObtained = 'F';

    }
  }

  selectedGrade(grade, index) {
    this.addUpdateStudentFinalGradeModel.studentFinalGradeList[index].percentMarks = this.gradeScaleList.filter(x => x.title === grade)[0].breakoff;
  }

  onCheckboxChange(value) {
    if (value.currentTarget.checked) {
      this.addUpdateStudentFinalGradeModel.isPercent = false;
      this.isPercentChecked = true;
    }
    else {
      this.addUpdateStudentFinalGradeModel.isPercent = true;
      this.isPercentChecked = false;
    }


  }

  getAllMarkingPeriodList() {
    this.getMarkingPeriodTitleListModel.schoolId = +sessionStorage.getItem("selectedSchoolId");
    this.getMarkingPeriodTitleListModel.academicYear = +sessionStorage.getItem("academicyear");
    this.markingPeriodService.getAllMarkingPeriodList(this.getMarkingPeriodTitleListModel).subscribe(data => {
      if (data._failure) {
        this.getMarkingPeriodTitleListModel.getMarkingPeriodView = [];
      } else {
        this.getMarkingPeriodTitleListModel.getMarkingPeriodView = data.getMarkingPeriodView;
        this.getAllScheduledCourseSectionBasedOnTeacher();
      }
    });
  }

  getAllScheduledCourseSectionBasedOnTeacher() {
    this.allScheduledCourseSectionBasedOnTeacher.courseSectionViewList = null;
    this.teacherReassignmentService.getAllScheduledCourseSectionForStaff(this.allScheduledCourseSectionBasedOnTeacher).pipe(
      map((res) => {
        res._userName = sessionStorage.getItem('user');
        return res;
      })
    ).subscribe((res) => {
      if (res) {
        if (res._failure) {
          if (res.courseSectionViewList == null) {
            this.snackbar.open(res._message, '', {
              duration: 10000
            });
            this.allScheduledCourseSectionBasedOnTeacher.courseSectionViewList = [];
          } else {
            this.allScheduledCourseSectionBasedOnTeacher.courseSectionViewList = [];
          }
        } else {
          this.allScheduledCourseSectionBasedOnTeacher = res;
          this.allScheduledCourseSectionBasedOnTeacher.courseSectionViewList.filter(x => x.gradeScaleType !== 'Ungraded');

        }
      }
      else {
        this.snackbar.open('' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });

      }
    })
  }

  selectedMarkingPeriod(markingPeriodId) {
    this.addUpdateStudentFinalGradeModel.markingPeriodId = markingPeriodId;

  }

  inActiveStudent(value) {
  }

  selectedCourseSection(courseSection) {
    this.courseSectionId = courseSection;
    this.courseSectionDetails = this.allScheduledCourseSectionBasedOnTeacher.courseSectionViewList.filter(x => x.courseSectionId === +this.courseSectionId);
    this.courseSectionData = this.findMarkingPeriodTitleById(this.courseSectionDetails[0]);
    this.markingPeriodList = this.getMarkingPeriodTitleListModel.getMarkingPeriodView.filter(x => x.value === this.courseSectionData.markingPeriodId);
    this.addUpdateStudentFinalGradeModel.markingPeriodId = this.getMarkingPeriodTitleListModel.getMarkingPeriodView.filter(x => x.value === this.courseSectionData.markingPeriodId)[0].value;
    this.addUpdateStudentFinalGradeModel.schoolId = this.defaultValuesService.getSchoolID();
    this.addUpdateStudentFinalGradeModel.tenantId = this.defaultValuesService.getTenantID();
    this.addUpdateStudentFinalGradeModel.courseId = this.courseSectionDetails[0].courseId;
    this.addUpdateStudentFinalGradeModel.courseSectionId = this.courseSectionDetails[0].courseSectionId;
    this.addUpdateStudentFinalGradeModel.calendarId = this.courseSectionDetails[0].calendarId;
    this.finalGradeService.getAllStudentFinalGradeList(this.addUpdateStudentFinalGradeModel).subscribe((res) => {
      if (res) {
        if (res._failure) {
          this.addUpdateStudentFinalGradeModel.courseId = this.courseSectionDetails[0].courseId;
          this.addUpdateStudentFinalGradeModel.calendarId = this.courseSectionDetails[0].calendarId;
          this.getAllReportCardCommentsWithCategory(this.addUpdateStudentFinalGradeModel.courseId);
          this.getAllCourseStandard(this.addUpdateStudentFinalGradeModel.courseId);
          if (this.courseSectionDetails[0].gradeScaleType !== 'Numeric' || this.courseSectionDetails[0].gradeScaleType !== 'Teacher_Scale') {
            this.addUpdateStudentFinalGradeModel.isPercent = false;
            this.isPercentChecked = true;
            this.getAllGradeScaleList(this.courseSectionDetails[0].standardGradeScaleId).then(() => {
              this.searchScheduledStudentForGroupDrop(this.courseSectionDetails[0].courseSectionId);
            });
          }
          else {
            this.searchScheduledStudentForGroupDrop(this.courseSectionDetails[0].courseSectionId);
          }

        }
        else {
          this.getAllReportCardCommentsWithCategory(this.addUpdateStudentFinalGradeModel.courseId);
          this.getAllCourseStandard(this.addUpdateStudentFinalGradeModel.courseId);
          if (this.courseSectionDetails[0].gradeScaleType !== 'Numeric' || this.courseSectionDetails[0].gradeScaleType !== 'Teacher_Scale') {
            this.addUpdateStudentFinalGradeModel.isPercent = false;
            this.isPercentChecked = true;
            this.getAllGradeScaleList(this.courseSectionDetails[0].standardGradeScaleId);
          }
          this.scheduleStudentListViewModel.courseSectionId = this.courseSectionDetails[0].courseSectionId;
          this.scheduleStudentListViewModel.profilePhoto = true;
          this.scheduleStudentListViewModel.sortingModel = null;
          this.studentScheduleService.searchScheduledStudentForGroupDrop(this.scheduleStudentListViewModel).subscribe((res) => {
            if (res) {
              if (res._failure) {
                this.showMessage = 'noRecordFound';
              } else {
                this.studentMasterList = res.scheduleStudentForView;
                this.totalCount = this.studentMasterList.length;
                this.addComments(this.studentMasterList.length > 0 ? 0 : null);

                if (this.studentMasterList.length === 0) {
                  this.showMessage = 'noRecordFound';
                }
              }
            }
          });
          this.addUpdateStudentFinalGradeModel = res;
          this.addUpdateStudentFinalGradeModel.studentFinalGradeList.map((item, i) => {
            let commentArray = [];
            item.studentFinalGradeComments.map((subItem) => {
              const commentData = {
                courseCommentId: subItem.courseCommentCategory.courseCommentId,
                comments: subItem.courseCommentCategory.comments,
              }
              commentArray.push(commentData);
            });
            item.studentFinalGradeComments = commentArray;

            let standardArray = [];
            item.studentFinalGradeStandard.map((subItem) => {
              const standardData = {
                standardGradeScaleId: subItem.standardGradeScaleId,
                gradeObtained: subItem.gradeObtained,
              }
              standardArray.push(standardData);
            });
            item.studentFinalGradeStandard = standardArray;
          });
        }
      }
      else {
        this.snackbar.open('' + sessionStorage.getItem('httpError'), '', {
          duration: 10000
        });
      }


    });
  }

  findMarkingPeriodTitleById(courseDetails) {
    if (courseDetails.yrMarkingPeriodId) {
      courseDetails.markingPeriodId = '0_' + courseDetails.yrMarkingPeriodId;
    } else if (courseDetails.smstrMarkingPeriodId) {
      courseDetails.markingPeriodId = '1_' + courseDetails.smstrMarkingPeriodId;
    } else if (courseDetails.qtrMarkingPeriodId) {
      courseDetails.markingPeriodId = '2_' + courseDetails.qtrMarkingPeriodId;
    }
    else {
      courseDetails.markingPeriodId = this.getMarkingPeriodTitleListModel.getMarkingPeriodView[0].value;
    }
    return courseDetails;

  }


  getAllReportCardCommentsWithCategory(courseId) {
    this.reportCardComments = [];
    this.reportCardService.getAllCourseCommentCategory(this.getAllReportCardCommentsWithCategoryModel).subscribe((res) => {
      if (res) {
        if (res._failure) {
          if (res.courseCommentCategories === null) {
            this.snackbar.open(res._message, '', {
              duration: 10000
            });
            this.reportCardCategoryWithComments.courseCommentCategories = []
          } else {
            this.reportCardCategoryWithComments.courseCommentCategories = []
          }
        } else {

          this.reportCardCategoryWithComments = JSON.parse(JSON.stringify(res));
          let commentCategories = this.reportCardCategoryWithComments?.courseCommentCategories.filter(x => x.courseId === +courseId || x.applicableAllCourses);
          for (let commentCategory of commentCategories) {
            this.reportCardComments.push({ courseCommentId: commentCategory.courseCommentId, comments: commentCategory.comments });
          }

        }
      } else {
        this.snackbar.open('' + sessionStorage.getItem('httpError'), '', {
          duration: 10000
        });
      }
    });
  }


  getAllCourseStandard(courseId) {
    this.courseStandardForCourseViewModel.courseId = +courseId;
    this.courseManager.getAllCourseStandardForCourse(this.courseStandardForCourseViewModel).subscribe(data => {
      if (data._failure) {

      } else {

        this.courseStandardList = data.courseStandards;

      }
    });
  }
  addComments(id) {
    if (id !== null) {
      this.selectedStudent = id;
      this.showComment = true;
    }
  }

  compareObjects(o1: any, o2: any): boolean {
    return o1.courseCommentId === o2.courseCommentId && o1.comments === o2.comments;
  }

  selectedReportComment(comment) {
  }

  removeComment(reportCommentId) {
    this.addUpdateStudentFinalGradeModel.studentFinalGradeList[this.selectedStudent].studentFinalGradeComments = this.addUpdateStudentFinalGradeModel.studentFinalGradeList[this.selectedStudent].studentFinalGradeComments.filter(item => item.courseCommentId !== +reportCommentId);
  }

  getAllGradeScaleList(standardGradeScaleId) {
    return new Promise((resolve, reject) => {
      this.gradesService.getAllGradeScaleList(this.gradeScaleListView).subscribe(data => {
        if (data._failure) {
        } else {
          this.gradeScaleStandardList = data.gradeScaleList.filter(x => x.gradeScaleId === +standardGradeScaleId)[0]?.grade;
          this.gradeScaleList = data.gradeScaleList.filter(x => x.useAsStandardGradeScale === false)[0]?.grade;
          resolve('');
        }
      });
    })

  }

  searchScheduledStudentForGroupDrop(courseSectionId) {
    this.scheduleStudentListViewModel.sortingModel = null;
    this.scheduleStudentListViewModel.courseSectionId = courseSectionId;
    this.scheduleStudentListViewModel.profilePhoto = true;
    this.studentScheduleService.searchScheduledStudentForGroupDrop(this.scheduleStudentListViewModel).subscribe((res) => {
      if (res) {
        if (res._failure) {
          this.showMessage = 'noRecordFound';
        } else {
          this.studentMasterList = res.scheduleStudentForView;
          this.totalCount = this.studentMasterList.length;
          this.addComments(this.studentMasterList.length > 0 ? 0 : null);
          if (this.studentMasterList.length === 0) {
            this.showMessage = 'noRecordFound';
          }
          this.addUpdateStudentFinalGradeModel.studentFinalGradeList = [new StudentFinalGrade()];
          this.studentMasterList.map((item, i) => {
            this.initializeDefaultValues(item, i);
            this.addUpdateStudentFinalGradeModel.studentFinalGradeList.push(new StudentFinalGrade());
          });
          this.scheduleStudentListViewModel = new ScheduleStudentListViewModel();
          this.addUpdateStudentFinalGradeModel.studentFinalGradeList.pop();
        }
      }
      else {
        this.snackbar.open('' + sessionStorage.getItem('httpError'), '', {
          duration: 10000
        });
      }
    });
  }

  initializeDefaultValues(item, i) {
    for (let j = 0; j < this.gradeScaleStandardList?.length; j++) {
      this.addUpdateStudentFinalGradeModel.studentFinalGradeList[i].studentFinalGradeStandard[j] = [];
    }
    this.addUpdateStudentFinalGradeModel.studentFinalGradeList[i].studentId = item.studentId;
    this.addUpdateStudentFinalGradeModel.studentFinalGradeList[i].gradeId = item.gradeId;
    this.addUpdateStudentFinalGradeModel.studentFinalGradeList[i].gradeScaleId = item.gradeScaleId;
    this.addUpdateStudentFinalGradeModel.studentFinalGradeList[i].percentMarks = 0;
    this.addUpdateStudentFinalGradeModel.studentFinalGradeList[i].basedOnStandardGrade = true;
    this.addUpdateStudentFinalGradeModel.studentFinalGradeList[i].studentFinalGradeSrlno = 0;
    this.addUpdateStudentFinalGradeModel.studentFinalGradeList[i].gradeObtained = null;
    this.addUpdateStudentFinalGradeModel.studentFinalGradeList[i].teacherComment = null;
    this.addUpdateStudentFinalGradeModel.studentFinalGradeList[i].studentFinalGradeComments = [];

  }

  setValue(standardGradeScaleId, gradeObtained, i) {
    this.addUpdateStudentFinalGradeModel.studentFinalGradeList[this.selectedStudent].studentFinalGradeStandard[i] = { standardGradeScaleId: standardGradeScaleId, gradeObtained: gradeObtained };
  }

  submitFinalGrade() {
    this.addUpdateStudentFinalGradeModel.academicYear = this.defaultValuesService.getAcademicYear();
    this.addUpdateStudentFinalGradeModel.createdOrUpdatedBy = this.defaultValuesService.getUserName();
    this.finalGradeService.addUpdateStudentFinalGrade(this.addUpdateStudentFinalGradeModel).subscribe((data) => {
      if (data) {
        if (data._failure) {
          this.snackbar.open(data._message, '', {
            duration: 10000
          });
        } else {
          this.snackbar.open(data._message, '', {
            duration: 10000
          });
        }
      }
      else {
        this.snackbar.open('' + sessionStorage.getItem('httpError'), '', {
          duration: 10000
        });
      }
    });
  }

}
