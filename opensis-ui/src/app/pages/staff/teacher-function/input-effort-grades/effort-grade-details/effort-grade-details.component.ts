import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { map } from 'rxjs/operators';
import { LayoutService } from '../../../../../../@vex/services/layout.service';
import { DefaultValuesService } from '../../../../../common/default-values.service';
import { EffortGradeLibraryCategoryListView, GetAllEffortGradeScaleListModel } from '../../../../../models/grades.model';
import { GetMarkingPeriodTitleListModel } from '../../../../../models/marking-period.model';
import { StudentEffortGradeListModel, StudentEffortGradeMaster } from '../../../../../models/student-effort-grade.model';
import { ScheduleStudentForView, ScheduleStudentListViewModel } from '../../../../../models/student-schedule.model';
import { AllScheduledCourseSectionForStaffModel } from '../../../../../models/teacher-schedule.model';
import { CourseManagerService } from '../../../../../services/course-manager.service';
import { EffotrGradeService } from '../../../../../services/effort-grade.service';
import { FinalGradeService } from '../../../../../services/final-grade.service';
import { GradesService } from '../../../../../services/grades.service';
import { LoaderService } from '../../../../../services/loader.service';
import { MarkingPeriodService } from '../../../../../services/marking-period.service';
import { StudentScheduleService } from '../../../../../services/student-schedule.service';
import { TeacherScheduleService } from '../../../../../services/teacher-schedule.service';
import { fadeInRight400ms } from '../../../../../../@vex/animations/fade-in-right.animation';
import { fadeInUp400ms } from '../../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../../@vex/animations/stagger.animation';

@Component({
  selector: 'vex-effort-grade-details',
  templateUrl: './effort-grade-details.component.html',
  styleUrls: ['./effort-grade-details.component.scss'],
  animations: [
    fadeInRight400ms,
    stagger60ms,
    fadeInUp400ms
  ]
})
export class EffortGradeDetailsComponent implements OnInit {
  pageStatus = "Grade Details";
  staffDetails;
  showMessage="pleaseSelectCourseSectionAndMarkingPeriodForEffortGrade";
  showStudentList: boolean = true;
  courseSectionData;
  selectedStudent: number = 0;
  loading: boolean = false;
  totalCount: number = 0;
  courseSectionId: number;
  showEffort: boolean = false;
  markingPeriodList = [];
  effortCategoriesList = [];
  effortGradeDetailViewList = [];
  effortGradeScaleModelList = [];
  studentMasterList: ScheduleStudentForView[];
  getEffortGradeScaleList: GetAllEffortGradeScaleListModel = new GetAllEffortGradeScaleListModel();
  effortGradeLibraryCategoryListView: EffortGradeLibraryCategoryListView = new EffortGradeLibraryCategoryListView();
  allScheduledCourseSectionBasedOnTeacher: AllScheduledCourseSectionForStaffModel = new AllScheduledCourseSectionForStaffModel();
  studentEffortGradeListModel: StudentEffortGradeListModel = new StudentEffortGradeListModel();
  scheduleStudentListViewModel: ScheduleStudentListViewModel = new ScheduleStudentListViewModel();
  getMarkingPeriodTitleListModel: GetMarkingPeriodTitleListModel = new GetMarkingPeriodTitleListModel();

  constructor(public translateService: TranslateService,
    private finalGradeService: FinalGradeService,
    private teacherReassignmentService: TeacherScheduleService,
    private snackbar: MatSnackBar,
    private studentScheduleService: StudentScheduleService,
    private markingPeriodService: MarkingPeriodService,
    private router: Router,
    private defaultValuesService: DefaultValuesService,
    private loaderService: LoaderService,
    private gradesService: GradesService,
    private layoutService: LayoutService,
    private effotrGradeService: EffotrGradeService) {
    this.loaderService.isLoading.subscribe((val) => {
      this.loading = val;
    });
    translateService.use('en');
    this.staffDetails = this.finalGradeService.getStaffDetails();
    this.layoutService.collapseSidenav();
  }

  ngOnInit(): void {
    this.allScheduledCourseSectionBasedOnTeacher.staffId = this.staffDetails.staffId;
    if (this.allScheduledCourseSectionBasedOnTeacher.staffId) {

    }
    else {
      this.router.navigate(['/school', 'staff', 'teacher-functions', 'input-effort-grade']);
    }
    this.getAllMarkingPeriodList();
    this.getAllEffortGradeLlibraryCategoryList();
    
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

        }
      }
      else {
        this.snackbar.open('' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });

      }
    })
  }

  selectedCourseSection(courseSection) {
    this.courseSectionId = courseSection;
    let courseSectionDetails = this.allScheduledCourseSectionBasedOnTeacher.courseSectionViewList.filter(x => x.courseSectionId === +this.courseSectionId);
    this.courseSectionData = this.findMarkingPeriodTitleById(courseSectionDetails[0]);
    this.markingPeriodList = this.getMarkingPeriodTitleListModel.getMarkingPeriodView.filter(x => x.value === this.courseSectionData.markingPeriodId);
    this.studentEffortGradeListModel.markingPeriodId = this.getMarkingPeriodTitleListModel.getMarkingPeriodView.filter(x => x.value === this.courseSectionData.markingPeriodId)[0].value;
    this.studentEffortGradeListModel.schoolId = this.defaultValuesService.getSchoolID();
    this.studentEffortGradeListModel.tenantId = this.defaultValuesService.getTenantID();
    this.studentEffortGradeListModel.courseId = courseSectionDetails[0].courseId;
    this.studentEffortGradeListModel.courseSectionId = courseSectionDetails[0].courseSectionId;
    this.studentEffortGradeListModel.calendarId = courseSectionDetails[0].calendarId;
    this.effotrGradeService.getAllStudentEffortGradeList(this.studentEffortGradeListModel).subscribe((res) => {
      if (res) {
        if (res._failure) {
          this.studentEffortGradeListModel.courseId = courseSectionDetails[0].courseId;
          this.studentEffortGradeListModel.calendarId = courseSectionDetails[0].calendarId;
          this.searchScheduledStudentForGroupDrop(courseSectionDetails[0].courseSectionId);
        }
        else {
          this.scheduleStudentListViewModel.courseSectionId = courseSectionDetails[0].courseSectionId;
          this.scheduleStudentListViewModel.profilePhoto = true;
          this.studentScheduleService.searchScheduledStudentForGroupDrop(this.scheduleStudentListViewModel).subscribe((res) => {
            if (res) {
              if (res._failure) {
                this.showMessage = 'noRecordFound';
              } else {
                this.studentMasterList = res.scheduleStudentForView;
                this.totalCount = this.studentMasterList.length;
                if (this.studentMasterList.length === 0) {
                  this.showMessage = 'noRecordFound';
                }
              }
            }
          });
          this.studentEffortGradeListModel = res;
        }
      }
      else {
        this.snackbar.open('' + sessionStorage.getItem('httpError'), '', {
          duration: 10000
        });
      }


    });
  }

  goToAddEffortGrade(){
    this.router.navigate(['/school', 'settings', 'grade-settings']);
    localStorage.setItem('pageId','Effort Grade Setup');
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

  searchScheduledStudentForGroupDrop(courseSectionId) {
    this.scheduleStudentListViewModel.courseSectionId = courseSectionId;
    this.scheduleStudentListViewModel.profilePhoto = true;
    this.studentScheduleService.searchScheduledStudentForGroupDrop(this.scheduleStudentListViewModel).subscribe((res) => {
      if (res) {
        if (res._failure) {
          this.showMessage = 'noRecordFound';
        } else {
          this.studentMasterList = res.scheduleStudentForView;
          this.totalCount = this.studentMasterList.length;
          if (this.studentMasterList.length === 0) {
            this.showMessage = 'noRecordFound';
          }
          this.studentEffortGradeListModel.studentEffortGradeList = [new StudentEffortGradeMaster()];
          this.studentMasterList.map((item, i) => {
            this.initializeDefaultValues(item, i);
            this.studentEffortGradeListModel.studentEffortGradeList.push(new StudentEffortGradeMaster());
          });
          this.scheduleStudentListViewModel = new ScheduleStudentListViewModel();
          this.studentEffortGradeListModel.studentEffortGradeList.pop();
        }
      }
      else {
        this.snackbar.open('' + sessionStorage.getItem('httpError'), '', {
          duration: 10000
        });
      }
    });
  }

  getAllEffortGradeLlibraryCategoryList() {
    this.gradesService.getAllEffortGradeLlibraryCategoryList(this.effortGradeLibraryCategoryListView).subscribe(
      (res: EffortGradeLibraryCategoryListView) => {
        if (typeof (res) == 'undefined') {
          this.snackbar.open('' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else {
          if (res._failure) {
            if (res.effortGradeLibraryCategoryList == null) {
              this.effortCategoriesList = null
              this.snackbar.open(res._message, '', {
                duration: 10000
              });
            } else {
              this.effortCategoriesList = null;
            }
          }
          else {
            this.effortCategoriesList = res.effortGradeLibraryCategoryList;
            this.getAllEffortGradeScale();
            for (let category of this.effortCategoriesList) {
              for (let item of category.effortGradeLibraryCategoryItem) {
                this.effortGradeDetailViewList.push({ categoryName: category.categoryName, effortCategoryId: item.effortCategoryId, effortItemTitle: item.effortItemTitle, effortItemId: item.effortItemId });
              }
            }
          }
        }
      }
    );
  }

  addEffortItems(id) {
    this.selectedStudent = id;
    this.showEffort = true;
  }

  selectCategory(item, index, value) {
    this.studentEffortGradeListModel.studentEffortGradeList[this.selectedStudent].studentEffortGradeDetail[index] = { effortCategoryId: item.effortCategoryId, effortItemId: item.effortItemId, effortGradeScaleId: value.value };
  }

  getAllEffortGradeScale() {
    if (this.getEffortGradeScaleList.sortingModel?.sortColumn == "") {
      this.getEffortGradeScaleList.sortingModel.sortColumn = "sortOrder"
      this.getEffortGradeScaleList.sortingModel.sortDirection = "asc"
    }

    this.gradesService.getAllEffortGradeScaleList(this.getEffortGradeScaleList).subscribe(data => {
      if (data) {
        if (data._failure) {
          if (data.effortGradeScaleList == null) {

            this.snackbar.open(data._message, '', {
              duration: 10000
            });
          } else {
            this.effortGradeScaleModelList = null;
          }
        } else {
          this.effortGradeScaleModelList = data.effortGradeScaleList;

          if(this.effortGradeScaleModelList.length>0 && this.effortCategoriesList.length>0){
              this.showStudentList= false;
          }
        }
      }
      else {
        this.snackbar.open(data._message, '', {
          duration: 10000
        });
      }
    });
  }


  initializeDefaultValues(item, i) {

    for (let category of this.effortGradeDetailViewList) {
      this.studentEffortGradeListModel.studentEffortGradeList[i].studentEffortGradeDetail.push({ effortCategoryId: null, effortItemId: null, effortGradeScaleId: null });
    }
    this.studentEffortGradeListModel.studentEffortGradeList[i].studentId = item.studentId;
    this.studentEffortGradeListModel.studentEffortGradeList[i].studentFinalGradeSrlno = 0;
    this.studentEffortGradeListModel.studentEffortGradeList[i].teacherComment = null;

  }

  submitEffortGrade() {
    this.effotrGradeService.addUpdateStudentEffortGrade(this.studentEffortGradeListModel).subscribe(data => {
      if (data) {

        if (data._failure) {
          if (data.studentEffortGradeList == null) {

            this.snackbar.open(data._message, '', {
              duration: 10000
            });
          } else {

          }
        } else {
          this.effortGradeScaleModelList = data.studentEffortGradeList;
          this.snackbar.open(data._message, '', {
            duration: 10000
          });
        }
      }
      else {
        this.snackbar.open(data._message, '', {
          duration: 10000
        });
      }

    });
  }

}
