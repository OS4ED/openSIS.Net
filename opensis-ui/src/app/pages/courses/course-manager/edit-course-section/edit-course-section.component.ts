import { Component, ComponentFactoryResolver, Inject, OnInit, Optional, Output, EventEmitter } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable, Subject } from 'rxjs';
import * as moment from 'moment';
import { CalendarEvent, CalendarEventAction, CalendarEventTimesChangedEvent, CalendarMonthViewBeforeRenderEvent, CalendarMonthViewDay, CalendarView, DAYS_OF_WEEK } from 'angular-calendar';
import icClose from '@iconify/icons-ic/twotone-close';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icDelete from '@iconify/icons-ic/twotone-delete';
import icAdd from '@iconify/icons-ic/twotone-add';
import icList from '@iconify/icons-ic/twotone-list-alt';
import icInfo from '@iconify/icons-ic/info';
import icRemove from '@iconify/icons-ic/remove-circle';
import icBack from '@iconify/icons-ic/baseline-arrow-back';
import icExpand from '@iconify/icons-ic/outline-add-box';
import icCollapse from '@iconify/icons-ic/outline-indeterminate-check-box';
import icPlusCircle from '@iconify/icons-ic/add-circle-outline';
import icLeftArrow from '@iconify/icons-ic/baseline-west';
import icRightArrow from '@iconify/icons-ic/baseline-east';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { Router } from '@angular/router';
import { CalendarService } from '../../../../services/calendar.service';
import { CalendarListModel, CalendarModel } from '../../../../models/calendar.model';
import { GradesService } from '../../../../services/grades.service';
import { GradeScaleListView, GetAllSchoolSpecificListModel } from '../../../../models/grades.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BlockedSchedulingCourseSectionAddModel, CourseCalendarSchedule, CourseSection, OutputEmitDataFormat, CourseVariableSchedule, CourseSectionAddViewModel, FixedSchedulingCourseSectionAddModel, VariableSchedulingCourseSectionAddModel, CourseVariableScheduleListModel, GetAllCourseStandardForCourseSectionModel } from '../../../../models/course-section.model';

import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { AttendanceCodeService } from '../../../../services/attendance-code.service';
import { GetAllAttendanceCategoriesListModel } from '../../../../models/attendance-code.model';
import { GetAllMarkingPeriodTitle, GetMarkingPeriodTitleListModel } from '../../../../models/marking-period.model';
import { MarkingPeriodService } from '../../../../services/marking-period.service';
import { CourseSectionService } from '../../../../services/course-section.service';
import { SharedFunction } from '../../../shared/shared-function';
import { isNull } from 'util';

@Component({
  selector: 'vex-edit-course-section',
  templateUrl: './edit-course-section.component.html',
  styleUrls: ['./edit-course-section.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class EditCourseSectionComponent implements OnInit {

  icClose = icClose;
  icEdit = icEdit;
  icDelete = icDelete;
  icAdd = icAdd;
  icList = icList;
  icInfo = icInfo;
  icRemove = icRemove;
  icBack = icBack;
  icExpand = icExpand;
  icCollapse = icCollapse;
  icPlusCircle = icPlusCircle;
  icLeftArrow = icLeftArrow;
  icRightArrow = icRightArrow;
  scheduleType = '0';
  durationType = '0';
  addCalendarDay = 0;
  selectedDate;
  attendanceCategoryList = [];
  form: FormGroup;
  calendarListModel: CalendarListModel = new CalendarListModel();
  gradeScaleListView: GradeScaleListView = new GradeScaleListView();
  courseSection: CourseSection = new CourseSection();
  courseSectionAddViewModel: CourseSectionAddViewModel = new CourseSectionAddViewModel();
  getMarkingPeriodTitleListModel: GetMarkingPeriodTitleListModel = new GetMarkingPeriodTitleListModel();
  getAllAttendanceCategoriesListModel: GetAllAttendanceCategoriesListModel = new GetAllAttendanceCategoriesListModel();
  view: CalendarView = CalendarView.Month;
  calendarError: boolean = false;
  calendarList = [];
  selectedCalendar = new CalendarModel();
  selectedMarkingPeriod;
  gradeScaleList = [];
  viewDate: Date = new Date();
  events: CalendarEvent[] = [];
  standardGradeScaleList = [];
  gradeScaleStandardList = [];
  markingPeriodList = [];
  durationDateRange ={startDate:null,endDate:null};
  calendarId;
  courseSectionModalTitle = "addNewCourseSection";
  courseSectionActionButtonTitle = "submit";
  startDate;
  endDate;
  showCalendarDates;
  seatChangeFlag: boolean;
  staticGradeScaleValue=[
    {
      gradeScaleId:'Ungraded',
      gradescaleName:'Not Graded'
    },
    {
      gradeScaleId:'Numeric',
      gradescaleName:'Numeric'
    },
    {
      gradeScaleId:'Teacher_Scale',
      gradescaleName:'Teacher own Grade Scale'
    }
  ]
  constructor(
    private dialogRef: MatDialogRef<EditCourseSectionComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: any,
    private router: Router,
    private calendarService: CalendarService,
    private snackbar: MatSnackBar,
    private gradesService: GradesService,
    private fb: FormBuilder,
    private courseSectionService: CourseSectionService,
    private attendanceCodeService: AttendanceCodeService,
    private markingPeriodService: MarkingPeriodService) {
    if (this.data.editMode) {
      this.setScheduleTypeInEditMode()
    }

  }
  ngOnInit(): void {
    this.form = this.fb.group(
      {
        courseSectionName: ['', Validators.required],
        calendarId: ['', Validators.required],
        isActive: [true],
        gradeScaleId: ['', Validators.required],
        creditHours: [this.data.courseDetails?.creditHours],
        seats: [''],
        attendanceCategoryId: [''],
        allowStudentConflict:[false],
        allowTeacherConflict:[false],
        isWeightedCourse: [false],
        affectsClassRank: [false],
        affectsHonorRoll: [false],
        onlineClassRoom: [false],
        useStandards: [false],
        standardGradeScaleId: [null],
        onlineClassroomUrl: [null],
        onlineClassroomPassword: [null],
        markingPeriodId: [''],
        durationType: [],
        durationStartDate: [''],
        durationEndDate: ['']
      });
    this.data.form = this.form;
    if (this.form.value.calendarId === "") {

      this.showCalendarDates = true;
    } else {
      this.showCalendarDates = false;
    }
    if (this.data.editMode) {
      this.courseSectionModalTitle = "updateCourseSection";
      this.courseSectionActionButtonTitle = "update";
      this.patchFormValue();
    }
    this.getAllCalendarList();
    this.getAllGradeScaleList();
    this.getAllAttendanceCategoryList();
    this.getAllMarkingPeriodList();
  }

  patchFormValue() {
    this.durationType = this.data.courseSectionDetails.courseSection.durationBasedOnPeriod ? '1' : '2'
    if(this.data.courseSectionDetails.courseSection.gradeScaleId!=( null && undefined)){
      this.form.controls.gradeScaleId.patchValue(this.data.courseSectionDetails.courseSection.gradeScaleId);
    }
    else{
      this.form.controls.gradeScaleId.patchValue(this.data.courseSectionDetails.courseSection.gradeScaleType);
    }
    this.form.patchValue({
      isActive: this.data.courseSectionDetails.courseSection.isActive,
      courseSectionName: this.data.courseSectionDetails.courseSection.courseSectionName,
      calendarId: this.data.courseSectionDetails.courseSection.calendarId,
      creditHours: this.data.courseSectionDetails.courseSection.creditHours,
      attendanceCategoryId: this.data.courseSectionDetails.courseSection.attendanceCategoryId,
      seats: this.data.courseSectionDetails.courseSection.seats,
      allowStudentConflict: this.data.courseSectionDetails.courseSection.allowStudentConflict,
      allowTeacherConflict: this.data.courseSectionDetails.courseSection.allowTeacherConflict,
      isWeightedCourse: this.data.courseSectionDetails.courseSection.isWeightedCourse,
      affectsClassRank: this.data.courseSectionDetails.courseSection.affectsClassRank,
      affectsHonorRoll: this.data.courseSectionDetails.courseSection.affectsHonorRoll,
      onlineClassRoom: this.data.courseSectionDetails.courseSection.onlineClassRoom,
      onlineClassroomUrl: this.data.courseSectionDetails.courseSection.onlineClassRoom ?
        this.data.courseSectionDetails.courseSection.onlineClassroomUrl : null,
      onlineClassroomPassword: this.data.courseSectionDetails.courseSection.onlineClassRoom ?
        this.data.courseSectionDetails.courseSection.onlineClassroomPassword : null,
      useStandards: this.data.courseSectionDetails.courseSection.useStandards,
      durationType: this.data.courseSectionDetails.courseSection.durationBasedOnPeriod ? '1' : '2',
      markingPeriodId: this.data.courseSectionDetails?.markingPeriod,
      durationStartDate: this.data.courseSectionDetails.courseSection?.durationStartDate,
      durationEndDate: this.data.courseSectionDetails.courseSection?.durationEndDate,
      standardGradeScaleId: this.data.courseSectionDetails.courseSection?.standardGradeScaleId
    });
  }

  showOnlineClassRoom(event) {
    if (event.checked) {
      this.form.setControl('onlineClassroomUrl', this.fb.control('', [Validators.required]));
    }
    else {
      this.form.removeControl('onlineClassroomUrl');
    }
  }

  toggleUsStandards(event) {
    if (event.checked) {
      this.form.setControl('standardGradeScaleId', this.fb.control('', Validators.required));
    } else {
      this.form.removeControl('standardGradeScaleId');
    }
  }

  setScheduleTypeInEditMode() {
    let scheduleName = this.data?.courseSectionDetails?.courseSection.scheduleType
    if (scheduleName === 'Fixed Schedule') {
      this.scheduleType = '1';
    } else if (scheduleName == 'Variable Schedule') {
      this.scheduleType = '2';
    } else if (scheduleName == 'Calendar Schedule') {
      this.scheduleType = '3';
    } else {
      this.scheduleType = '4';
    }
  }

  getAllCalendarList() {
    this.calendarService.getAllCalendar(this.calendarListModel).subscribe(data => {
      if (data._failure) {
        if (data._message === "NO RECORD FOUND") {
          this.calendarList = [];
          this.snackbar.open('NO RECORD FOUND. ', '', {
            duration: 1000
          });
        }
      } else {
        this.calendarList = data.calendarList;
        if (this.data.editMode) {
          this.changeCalendar(this.data.courseSectionDetails.courseSection.calendarId);
          this.changeMarkingPeriod(this.form.value.markingPeriodId);
          this.durationChange(null, null);
        }
      }
    });
  }

  changeCalendar(calendarId) {
    let index = this.calendarList.findIndex((item) => {
      return item.calenderId == +calendarId
    })
    this.selectedCalendar = this.calendarList[index];

    if (this.selectedCalendar !== undefined) {
      let startDateArr = this.selectedCalendar?.startDate?.split("T");
      this.startDate = startDateArr[0];
    }
    if (this.selectedCalendar !== undefined) {
      let endDateArr = this.selectedCalendar?.endDate?.split("T");
      this.endDate = endDateArr[0];
    }
    this.showCalendarDates = false;

  }

  changeMarkingPeriod(markingPeriodId) {
    let index = this.markingPeriodList.findIndex((item) => {
      return item.value == markingPeriodId
    })
    this.selectedMarkingPeriod = this.markingPeriodList[index];
  }

  durationChange(value, date) {
    let startdurationDate = this.form.value.durationStartDate;
    let endDurationDate = this.form.value.durationEndDate;
    if (date === 'durationStartDate'){
      startdurationDate = value;
    }
    else if(date === 'durationEndDate'){
      endDurationDate = value;
    }

    this.durationDateRange= {startDate: startdurationDate ,endDate: endDurationDate};
    
  }

  getAllGradeScaleList() {
    this.gradesService.getAllGradeScaleList(this.gradeScaleListView).subscribe(data => {
      if (data._failure) {
        if (data._message === "NO RECORD FOUND") {
          this.gradeScaleList = [];
          this.snackbar.open('NO RECORD FOUND. ', '', {
            duration: 1000
          });
        }
      } else {
        this.gradeScaleList = data.gradeScaleList;

        this.gradeScaleStandardList = data.gradeScaleList.filter(x => x.useAsStandardGradeScale);
      }
    });
  }



  getAllAttendanceCategoryList() {
    this.attendanceCodeService.getAllAttendanceCodeCategories(this.getAllAttendanceCategoriesListModel).subscribe(data => {
      if (data._failure) {

        if (data._message === "NO RECORD FOUND") {
          this.attendanceCategoryList = [];
          this.snackbar.open('NO RECORD FOUND. ', '', {
            duration: 1000
          });
        }
      } else {

        this.attendanceCategoryList = data.attendanceCodeCategoriesList;
      }
    });
  }


  getAllMarkingPeriodList() {
    this.getMarkingPeriodTitleListModel.academicYear = +sessionStorage.getItem("academicyear");
    this.markingPeriodService.getAllMarkingPeriodList(this.getMarkingPeriodTitleListModel).subscribe(data => {
      if (data._failure) {
        if (data._message === "NO RECORD FOUND") {
          this.markingPeriodList = [];
          this.snackbar.open('NO RECORD FOUND. ', '', {
            duration: 1000
          });
        }
      } else {
        this.markingPeriodList = data.getMarkingPeriodView;
        if (this.data.editMode) {
          this.changeMarkingPeriod(this.form.value.markingPeriodId);
          this.durationChange(null, null);
        }
      }
    });
  }


  setScheduleType(mrChange) {
    this.scheduleType = mrChange.value;
    switch (this.scheduleType) {
      case "1": {
        this.router.navigateByUrl('/school/course-manager/fixed-scheduling')

        break;
      }
      case "2": {
        this.router.navigateByUrl('/school/course-manager/variable-scheduling')
        break;
      }
      case "3": {
        this.router.navigateByUrl('/school/course-manager/calendar-days')
        break;
      }
      case "4": {
        this.router.navigateByUrl('/school/course-manager/rotating-scheduling')
        break;
      }
    }

    this.addCalendarDay = 0;
  }

  checkForSubmit() {
    if (this.scheduleType == '0') {
      this.snackbar.open('Please select a schedule ', '', {
        duration: 10000
      })
    }
    if (this.durationType == '0') {
      this.durationType = null
    }
    else {
      this.courseSectionService.sendCurrentData(true);
    }
  }
  collectValuesFromForm() {
    this.courseSection.courseId = this.data.courseDetails.courseId;
    this.courseSection.courseSectionName = this.form.value.courseSectionName;
    this.courseSection.calendarId = this.form.value.calendarId;
    if( typeof this.form.value.gradeScaleId=='number'){
      this.courseSection.gradeScaleId = this.form.value.gradeScaleId;
      this.courseSection.gradeScaleType = null;
    }else {
      this.courseSection.gradeScaleType = this.form.value.gradeScaleId;
      this.courseSection.gradeScaleId = null;
    }
    this.courseSection.creditHours = this.form.value.creditHours;
    this.courseSection.isActive = this.form.value.isActive;
    this.courseSection.attendanceCategoryId = this.form.value.attendanceCategoryId;
    this.courseSection.seats = this.form.value.seats;
    this.courseSection.allowStudentConflict = this.form.value.allowStudentConflict;
    this.courseSection.allowTeacherConflict = this.form.value.allowTeacherConflict;
    this.courseSection.isWeightedCourse = this.form.value.isWeightedCourse;
    this.courseSection.affectsClassRank = this.form.value.affectsClassRank;
    this.courseSection.affectsHonorRoll = this.form.value.affectsHonorRoll;
    this.courseSection.onlineClassRoom = this.form.value.onlineClassRoom;
    this.courseSection.useStandards = this.form.value.useStandards;
    this.courseSection.standardGradeScaleId = this.form.value.standardGradeScaleId;
    this.courseSection.onlineClassroomUrl = this.form.value.onlineClassroomUrl;
    this.courseSection.onlineClassroomPassword = this.form.value.onlineClassroomPassword;
    if(this.form.value.durationType=='1'){
      this.markingPeriodList.find(
        (x)=>{
          if(x.value === this.form.value.markingPeriodId?.toString())
          {
            this.courseSection.durationStartDate=x.startDate;
            this.courseSection.durationEndDate=x.endDate;
          }
        })
    }else{
      this.courseSection.durationStartDate = this.form.value.durationStartDate;
      this.courseSection.durationEndDate = this.form.value.durationEndDate;
    }
    
    this.courseSectionAddViewModel.markingPeriod = '';
    this.courseSectionAddViewModel.markingPeriodId = this.form.value.markingPeriodId?.toString();
    this.courseSection.durationBasedOnPeriod = this.form.value.durationType == '1' ? true : false;
    this.courseSectionAddViewModel.courseSection = this.courseSection;

  }
  submit() {
    if (this.form.valid) {
      if (this.data.editMode) {
        this.updateCourseSection();
      } else {
        this.addCourseSection()
      }
    }
  }
  addCourseSection() {
    this.collectValuesFromForm();
    this.courseSectionAddViewModel.courseSection.createdBy = sessionStorage.getItem("email")
    this.courseSectionService.addCourseSection(this.courseSectionAddViewModel).subscribe(data => {
      if (typeof (data) == 'undefined') {
        this.snackbar.open('Course Section Save failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 5000
        });
      }
      else {
        if (data._failure) {
          this.snackbar.open(data._message, '', {
            duration: 5000
          });
        } else {
          this.snackbar.open(data._message, '', {
            duration: 5000
          })
          this.dialogRef.close(true);
        }
      }
    });
  }
  updateCourseSection() {
    this.collectValuesFromForm();
    this.courseSectionAddViewModel.courseSection.updatedBy = sessionStorage.getItem("email")
    this.courseSection.courseSectionId = this.data.courseSectionDetails.courseSection.courseSectionId;
    this.courseSectionService.updateCourseSection(this.courseSectionAddViewModel).subscribe(data => {
      if (typeof (data) == 'undefined') {
        this.snackbar.open('Course Section Update failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 5000
        });
      }
      else {
        if (data._failure) {
          this.snackbar.open(data._message, '', {
            duration: 5000
          });
        } else {
          this.snackbar.open(data._message, '', {
            duration: 5000
          })
          this.dialogRef.close(true);
        }
      }
    });
  }

  openAddNewEvent(event) {
    this.addCalendarDay = 1;
    this.selectedDate = event.date;
  }
  setDuration(mrChange) {
    this.durationType = mrChange.value;

    if (this.durationType === '1') {
      this.selectedMarkingPeriod = undefined;
      this.durationDateRange ={startDate:null,endDate:null};
      this.courseSection.durationBasedOnPeriod = true;
      this.form.setControl('markingPeriodId', this.fb.control('', [Validators.required]));
      this.form.removeControl('durationStartDate');
      this.form.removeControl('durationEndDate');
    }
    else {
      this.selectedMarkingPeriod = undefined;
      this.durationDateRange ={startDate:null,endDate:null};
      this.form.removeControl('markingPeriodId');
      this.form.setControl('durationStartDate', this.fb.control('', [Validators.required]));
      this.form.setControl('durationEndDate', this.fb.control('', [Validators.required]));
      this.courseSection.durationBasedOnPeriod = false;
    }
  }

  cancelAddClass() {
    this.addCalendarDay = 0;
  }

  manipulateBlockScheduleBeforeSubmit(details: OutputEmitDataFormat) {
    if(details.scheduleDetails?.length==0){
      this.snackbar.open('You must have to select at least one day. ','', {
        duration: 3000
      });
      return
    }
    if (!details.error) {
      this.courseSection.scheduleType = details.scheduleType;
      this.courseSectionAddViewModel.courseBlockScheduleList = details.scheduleDetails;
      this.courseSectionAddViewModel.courseBlockScheduleList = this.courseSectionAddViewModel.courseBlockScheduleList?.map((item) => {
        // item.gradeScaleId = this.form.value.gradeScaleId;
        if (this.data.editMode) {
          item.updatedBy = sessionStorage.getItem("email");
        } else {
          item.createdBy = sessionStorage.getItem("email");
        }
        return item;
      });
      this.submit();
    }

  } 
  manipulateVariableScheduleBeforeSubmit(details:OutputEmitDataFormat){   
    if(details.scheduleDetails?.length==0){
      this.snackbar.open('You must have to select at least one day. ','', {
        duration: 3000
      });
      return
    }
    if(!details.error){    
      this.courseSectionAddViewModel.courseVariableScheduleList=details.scheduleDetails;       
      this.courseSectionAddViewModel.courseSection = this.courseSection;
      this.courseSection.scheduleType = details.scheduleType;    
      this.courseSectionAddViewModel.courseVariableScheduleList = this.courseSectionAddViewModel.courseVariableScheduleList;
      this.submit();
    }
  }

  manipulateFixedScheduleBeforeSubmit(details: OutputEmitDataFormat) {
    if (!details.error) {
      if (this.data.editMode) {
        details.scheduleDetails.updatedBy = sessionStorage.getItem("email");
      } else {
        details.scheduleDetails.createdBy = sessionStorage.getItem("email");
      }
      this.courseSection.scheduleType = details.scheduleType;
      this.courseSection.attendanceTaken = details.scheduleDetails.attendanceTaken;
      this.courseSection.meetingDays = details.scheduleDetails.meetingDays;
      this.courseSectionAddViewModel.courseFixedSchedule = details.scheduleDetails;
      this.courseSectionAddViewModel.courseFixedSchedule.schoolId = +sessionStorage.getItem("selectedSchoolId");
      this.courseSectionAddViewModel.courseFixedSchedule.courseId = +this.data.courseDetails.courseId;
      this.submit();
    }
  }

  manipulateCalendarScheduleBeforeSubmit(details) {
    if (!details.error) {
      this.courseSection.scheduleType = details.scheduleType;
      this.courseSectionAddViewModel.courseCalendarScheduleList = details.scheduleDetails;
      for (let schedule of this.courseSectionAddViewModel.courseCalendarScheduleList) {

        for (let room of details.roomList) {

          if (this.form.controls.seats.value !== "" && room.roomId === +schedule.roomId) {
            if (+this.form.controls.seats.value > +room.capacity) {
              this.calendarError = true;
              break;
            }
          }

        }
        if (this.calendarError) {
          break;
        }
      }
      if (!this.calendarError) {
        this.submit();
      }
    }
  }
}
