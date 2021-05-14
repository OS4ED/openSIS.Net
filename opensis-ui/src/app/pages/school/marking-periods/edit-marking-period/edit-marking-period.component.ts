import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import icClose from '@iconify/icons-ic/twotone-close';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { MarkingPeriodAddModel, SemesterAddModel , QuarterAddModel, ProgressPeriodAddModel} from '../../../../models/marking-period.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MarkingPeriodService } from '../../../../services/marking-period.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import {MAT_DIALOG_DATA} from '@angular/material/dialog';
import { MY_FORMATS } from '../../../shared/format-datepicker';
import * as _moment from 'moment';
import { default as _rollupMoment } from 'moment';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { SharedFunction } from '../../../shared/shared-function';
import { ValidationService } from '../../../shared/validation.service';
import { SchoolService } from '../../../../services/school.service';
import { MarkingPeriodListModel} from '../../../../models/marking-period.model';
const moment = _moment;
@Component({
  selector: 'vex-edit-marking-period',
  templateUrl: './edit-marking-period.component.html',
  styleUrls: ['./edit-marking-period.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ],
  providers: [
    { provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE] },
    { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
  ]
})
export class EditMarkingPeriodComponent implements OnInit {

  icClose = icClose;
  markingPeriodLevel;
  form: FormGroup;
  isEdit;
  doesGrades = false;
  markingPeriodAddModel: MarkingPeriodAddModel = new MarkingPeriodAddModel();
  semesterAddModel: SemesterAddModel = new SemesterAddModel();
  quarterAddModel: QuarterAddModel = new QuarterAddModel();
  progressPeriodAddModel: ProgressPeriodAddModel = new ProgressPeriodAddModel();
  markingPeriodListModel: MarkingPeriodListModel = new MarkingPeriodListModel();
  parentStartDate;
  parentEndDate;
  minDate;
  maxDate;
  list;
  obj = {
    'doesComments': false,
    'doesExam': false,
    'doesGrades': false,
    'endDate': '',
    'postEndDate': '',
    'postStartDate': '',
    'shortName': '',
    'startDate': '',
    'title': ''
  };
  sentArray = [];


  constructor(private dialogRef: MatDialogRef<EditMarkingPeriodComponent>,
              private fb: FormBuilder,
              private markingPeriodService: MarkingPeriodService,
              private snackbar: MatSnackBar,
              private commonFunction: SharedFunction,
              private schoolService: SchoolService,
              @Inject(MAT_DIALOG_DATA) public data) {

     }

  ngOnInit(): void {

    this.form = this.fb.group(
      {
        title: ['', Validators.required],
        shortName: ['', Validators.required],
        startDate: ['', Validators.required],
        endDate: ['', Validators.required],
        postStartDate: [''],
        postEndDate: [''],
        doesGrades: [''],
        doesExam: [''],
        doesComments: ['']
      });

    if (this.data && (Object.keys(this.data).length !== 0 || Object.keys(this.data).length > 0) ){

        if (this.data.isAdd === true && this.data.isEdit === false){
          this.isEdit = false;
          this.parentStartDate = this.commonFunction.formatDateInEditMode(this.data.details.startDate);
          this.parentEndDate = this.commonFunction.formatDateInEditMode(this.data.details.endDate);
          if (this.data.details.isParent){
            this.markingPeriodLevel = 'Year';
            this.assignFieldsValue('semesterAddModel', 'tableSemesters', 'yearId', '', '', 'markingPeriodId');

          }else{
            if (this.data.details.yearId > 0){
              this.markingPeriodLevel = 'Semester';
              this.assignFieldsValue('quarterAddModel', 'tableQuarter', 'semesterId', '', '', 'markingPeriodId');

            }else{
              this.markingPeriodLevel = 'Quarter';
              this.assignFieldsValue('progressPeriodAddModel', 'tableProgressPeriods', 'quarterId', '', '', 'markingPeriodId');

            }
          }
        }else{
          this.isEdit = true;
          if (this.data.editDetails.doesGrades){
            this.doesGrades = true;
          }

          if (this.data.editDetails.yearId > 0){

            if (this.data.fullData.length > 0){
              this.data.fullData.forEach((value, index) => {
                if (value.markingPeriodId === this.data.editDetails.yearId){
                  this.parentStartDate = this.commonFunction.formatDateInEditMode(value.startDate);
                  this.parentEndDate = this.commonFunction.formatDateInEditMode(value.endDate);
                }
              });
            }


            this.markingPeriodLevel = 'Year';
            this.assignFieldsValue('semesterAddModel', 'tableSemesters', 'doesGrades', 'markingPeriodAddModel', 'tableSchoolYears', '');
            this.assignFieldsValue('semesterAddModel', 'tableSemesters', 'markingPeriodId', '', '', '');
            this.assignFieldsValue('semesterAddModel', 'tableSemesters', 'yearId', '', '', '');

          }else if (this.data.editDetails.semesterId > 0){

            if (this.data.fullData.length > 0){
              this.data.fullData.forEach((value, index) => {
                value.children.forEach((val) => {
                  if (val.markingPeriodId === this.data.editDetails.semesterId){
                    this.parentStartDate = this.commonFunction.formatDateInEditMode(val.startDate);
                    this.parentEndDate = this.commonFunction.formatDateInEditMode(val.endDate);
                  }
                });

              });
            }
            this.markingPeriodLevel = 'Semester';
            this.assignFieldsValue('quarterAddModel', 'tableQuarter', 'doesGrades', 'markingPeriodAddModel', 'tableSchoolYears', '');
            this.assignFieldsValue('quarterAddModel', 'tableQuarter', 'markingPeriodId', '', '', '');
            this.assignFieldsValue('quarterAddModel', 'tableQuarter', 'semesterId', '', '', '');
          }else if (this.data.editDetails.quarterId > 0){

            if (this.data.fullData.length > 0){
              this.data.fullData.forEach((value, index) => {
                value.children.forEach((val) => {
                  val.children.forEach((i) => {
                    if (i.markingPeriodId === this.data.editDetails.quarterId){
                      this.parentStartDate = this.commonFunction.formatDateInEditMode(i.startDate);
                      this.parentEndDate = this.commonFunction.formatDateInEditMode(i.endDate);
                    }
                  });
                });

              });
            }
            this.markingPeriodLevel = 'Quarter';
            this.assignFieldsValue('progressPeriodAddModel', 'tableProgressPeriods', 'doesGrades', 'markingPeriodAddModel', 'tableSchoolYears', '');
            this.assignFieldsValue('progressPeriodAddModel', 'tableProgressPeriods', 'markingPeriodId', '', '', '');
            this.assignFieldsValue('progressPeriodAddModel', 'tableProgressPeriods', 'quarterId', '', '', '');
          }
          let arrList = Object.keys(this.data.editDetails);
          for (let i of arrList) {
            this.assignFieldsValue('markingPeriodAddModel', 'tableSchoolYears', i, '', '', '');
          }
          this.markingPeriodAddModel.tableSchoolYears.startDate = this.commonFunction.formatDateInEditMode(this.markingPeriodAddModel.tableSchoolYears.startDate);
          this.markingPeriodAddModel.tableSchoolYears.endDate = this.commonFunction.formatDateInEditMode(this.markingPeriodAddModel.tableSchoolYears.endDate);
          this.markingPeriodAddModel.tableSchoolYears.postStartDate = this.commonFunction.formatDateInEditMode(this.markingPeriodAddModel.tableSchoolYears.postStartDate);
          this.markingPeriodAddModel.tableSchoolYears.postEndDate = this.commonFunction.formatDateInEditMode(this.markingPeriodAddModel.tableSchoolYears.postEndDate);
        }
      }
    this.minDate = this.parentStartDate;
    this.maxDate =  this.parentEndDate;
  }



  gradeDateCompare(){
    let gradeOpeningDate = this.form.controls.postStartDate.value;
    let gradeClosingDate = this.form.controls.postEndDate.value;
    if (ValidationService.compareValidation(gradeOpeningDate, gradeClosingDate) === false) {
      this.form.controls.postEndDate.setErrors({ compareGradeError: true });
    }
  }

  checkStartDate(){
    let dateSchoolOpened = new Date(sessionStorage.getItem('schoolOpened'));
    return dateSchoolOpened;
  }

  checkGrade(data){

    if (data === false || data === undefined || data === null){
      this.doesGrades = true;
    }else{
      this.doesGrades = false;
      this.markingPeriodAddModel.tableSchoolYears.postStartDate = null;
      this.markingPeriodAddModel.tableSchoolYears.postEndDate = null;
    }
  }

  assignFieldsValue(toModel, toTable, toField, fromModel, fromTable, fromField)
  {
    if (fromField !== ''){
      this[toModel][toTable][toField] = this.data.details[fromField];
    }else{
      if (fromModel !== '' && fromTable !== ''){

        this[toModel][toTable][toField] = this[fromModel][fromTable][toField];
      }else{

        this[toModel][toTable][toField] = this.data.editDetails[toField];
      }
    }
  }

 submit(){

  if (this.form.valid) {
    if (this.isEdit ){
      if (this.markingPeriodLevel === 'Year'){
        let arrList = Object.keys(this.obj);
        for (let i of arrList) {
          this.assignFieldsValue('semesterAddModel', 'tableSemesters', i, 'markingPeriodAddModel', 'tableSchoolYears', '');
        }
        this.semesterAddModel.tableSemesters.academicYear = +sessionStorage.getItem('academicyear');
        this.semesterAddModel.tableSemesters.startDate = this.commonFunction.formatDateSaveWithoutTime(this.semesterAddModel.tableSemesters.startDate);
        this.semesterAddModel.tableSemesters.endDate = this.commonFunction.formatDateSaveWithoutTime(this.semesterAddModel.tableSemesters.endDate);
        this.semesterAddModel.tableSemesters.postStartDate = this.commonFunction.formatDateSaveWithoutTime(this.semesterAddModel.tableSemesters.postStartDate);
        this.semesterAddModel.tableSemesters.postEndDate = this.commonFunction.formatDateSaveWithoutTime(this.semesterAddModel.tableSemesters.postEndDate);
        this.markingPeriodService.UpdateSemester(this.semesterAddModel).subscribe(data => {
          if (data) {
            if (data._failure) {
              this.snackbar.open( data._message, '', {
                duration: 10000
              });
            } else {

              this.snackbar.open(data._message, '', {
                duration: 10000
              });
              this.markingPeriodService.getCurrentYear(true);
              this.dialogRef.close(true);
            }
          }
          else {
            this.snackbar.open('School Semester Updation failed. ' + sessionStorage.getItem('httpError'), '', {
              duration: 10000
            });
          }

        });
      }else if (this.markingPeriodLevel === 'Semester'){
        let arrList = Object.keys(this.obj);
        for (let i of arrList) {
          this.assignFieldsValue('quarterAddModel', 'tableQuarter', i, 'markingPeriodAddModel', 'tableSchoolYears', '');
        }
        this.quarterAddModel.tableQuarter.academicYear = +sessionStorage.getItem('academicyear');
        this.quarterAddModel.tableQuarter.startDate = this.commonFunction.formatDateSaveWithoutTime(this.quarterAddModel.tableQuarter.startDate);
        this.quarterAddModel.tableQuarter.endDate = this.commonFunction.formatDateSaveWithoutTime(this.quarterAddModel.tableQuarter.endDate);
        this.quarterAddModel.tableQuarter.postStartDate = this.commonFunction.formatDateSaveWithoutTime(this.quarterAddModel.tableQuarter.postStartDate);
        this.quarterAddModel.tableQuarter.postEndDate = this.commonFunction.formatDateSaveWithoutTime(this.quarterAddModel.tableQuarter.postEndDate);
        this.markingPeriodService.UpdateQuarter(this.quarterAddModel).subscribe(data => {
          if (data) {
            if (data._failure) {
              this.snackbar.open( data._message, '', {
                duration: 10000
              });
            } else {

              this.snackbar.open(data._message, '', {
                duration: 10000
              });
              this.markingPeriodService.getCurrentYear(true);
              this.dialogRef.close(true);
            }
          }
          else {
            this.snackbar.open('School Quarter Updation failed. ' + sessionStorage.getItem('httpError'), '', {
              duration: 10000
            });
          }

        });
      }else if (this.markingPeriodLevel === 'Quarter'){
        let arrList = Object.keys(this.obj);
        for (let i of arrList) {
          this.assignFieldsValue('progressPeriodAddModel', 'tableProgressPeriods', i, 'markingPeriodAddModel', 'tableSchoolYears', '');
        }
        this.progressPeriodAddModel.tableProgressPeriods.academicYear = +sessionStorage.getItem('academicyear');
        this.progressPeriodAddModel.tableProgressPeriods.startDate = this.commonFunction.formatDateSaveWithoutTime(this.progressPeriodAddModel.tableProgressPeriods.startDate);
        this.progressPeriodAddModel.tableProgressPeriods.endDate = this.commonFunction.formatDateSaveWithoutTime(this.progressPeriodAddModel.tableProgressPeriods.endDate);
        this.progressPeriodAddModel.tableProgressPeriods.postStartDate = this.commonFunction.formatDateSaveWithoutTime(this.progressPeriodAddModel.tableProgressPeriods.postStartDate);
        this.progressPeriodAddModel.tableProgressPeriods.postEndDate = this.commonFunction.formatDateSaveWithoutTime(this.progressPeriodAddModel.tableProgressPeriods.postEndDate);
        this.markingPeriodService.UpdateProgressPeriod(this.progressPeriodAddModel).subscribe(data => {
          if (data) {
            if (data._failure) {
              this.snackbar.open( data._message, '', {
                duration: 10000
              });
            } else {

              this.snackbar.open(data._message, '', {
                duration: 10000
              });
              this.markingPeriodService.getCurrentYear(true);
              this.dialogRef.close(true);
            }
          }
          else {
            this.snackbar.open('School Progress Period Updation failed. ' + sessionStorage.getItem('httpError'), '', {
              duration: 10000
            });
          }

        });
      }else{
        this.markingPeriodAddModel.tableSchoolYears.startDate = this.commonFunction.formatDateSaveWithoutTime(this.markingPeriodAddModel.tableSchoolYears.startDate);
        this.markingPeriodAddModel.tableSchoolYears.academicYear = +sessionStorage.getItem('academicyear');
        this.markingPeriodAddModel.tableSchoolYears.endDate = this.commonFunction.formatDateSaveWithoutTime(this.markingPeriodAddModel.tableSchoolYears.endDate);
        this.markingPeriodAddModel.tableSchoolYears.postStartDate = this.commonFunction.formatDateSaveWithoutTime(this.markingPeriodAddModel.tableSchoolYears.postStartDate);
        this.markingPeriodAddModel.tableSchoolYears.postEndDate = this.commonFunction.formatDateSaveWithoutTime(this.markingPeriodAddModel.tableSchoolYears.postEndDate);
        this.markingPeriodService.UpdateSchoolYear(this.markingPeriodAddModel).subscribe(data => {
        if (data) {
          if (data._failure) {
            this.snackbar.open(data._message, '', {
              duration: 10000
            });
          } else {

            this.snackbar.open(data._message, '', {
              duration: 10000
            });
            this.markingPeriodService.getCurrentYear(true);
            this.dialogRef.close(true);
          }
        }
        else {
          this.snackbar.open('School Year Updation failed. ' + sessionStorage.getItem('httpError'), '', {
            duration: 10000
          });
        }
      });
    }

    }else{
      if (this.markingPeriodLevel === 'Year'){
        let arrList = Object.keys(this.obj);
        for (let i of arrList) {
          this.assignFieldsValue('semesterAddModel', 'tableSemesters', i, 'markingPeriodAddModel', 'tableSchoolYears', '');
        }
        this.semesterAddModel.tableSemesters.academicYear = +sessionStorage.getItem('academicyear');
        this.semesterAddModel.tableSemesters.startDate = this.commonFunction.formatDateSaveWithoutTime(this.semesterAddModel.tableSemesters.startDate);
        this.semesterAddModel.tableSemesters.endDate = this.commonFunction.formatDateSaveWithoutTime(this.semesterAddModel.tableSemesters.endDate);
        this.semesterAddModel.tableSemesters.postStartDate = this.commonFunction.formatDateSaveWithoutTime(this.semesterAddModel.tableSemesters.postStartDate);
        this.semesterAddModel.tableSemesters.postEndDate = this.commonFunction.formatDateSaveWithoutTime(this.semesterAddModel.tableSemesters.postEndDate);
        this.markingPeriodService.AddSemester(this.semesterAddModel).subscribe(data => {
          if (data) {
            if (data._failure) {
              this.snackbar.open( data._message, '', {
                duration: 10000
              });
            } else {

              this.snackbar.open(data._message, '', {
                duration: 10000
              });
              this.sentArray = [true, sessionStorage.getItem('academicyear')];
              this.markingPeriodService.getCurrentYear(true);
              this.dialogRef.close(this.sentArray);
            }
          }
          else {
            this.snackbar.open('School Semester Submission failed. ' + sessionStorage.getItem('httpError'), '', {
              duration: 10000
            });
          }

        });
      }else if (this.markingPeriodLevel === 'Semester'){
        let arrList = Object.keys(this.obj);
        for (let i of arrList) {
          this.assignFieldsValue('quarterAddModel', 'tableQuarter', i, 'markingPeriodAddModel', 'tableSchoolYears', '');
        }
        this.quarterAddModel.tableQuarter.academicYear = +sessionStorage.getItem('academicyear');
        this.quarterAddModel.tableQuarter.startDate = this.commonFunction.formatDateSaveWithoutTime(this.quarterAddModel.tableQuarter.startDate);
        this.quarterAddModel.tableQuarter.endDate = this.commonFunction.formatDateSaveWithoutTime(this.quarterAddModel.tableQuarter.endDate);
        this.quarterAddModel.tableQuarter.postStartDate = this.commonFunction.formatDateSaveWithoutTime(this.quarterAddModel.tableQuarter.postStartDate);
        this.quarterAddModel.tableQuarter.postEndDate = this.commonFunction.formatDateSaveWithoutTime(this.quarterAddModel.tableQuarter.postEndDate);
        this.markingPeriodService.AddQuarter(this.quarterAddModel).subscribe(data => {
          if (data) {
            if (data._failure) {
              this.snackbar.open( data._message, '', {
                duration: 10000
              });
            } else {

              this.snackbar.open(data._message, '', {
                duration: 10000
              });
              this.sentArray = [true, sessionStorage.getItem('academicyear')];
              this.markingPeriodService.getCurrentYear(true);
              this.dialogRef.close(this.sentArray);
            }
          }
          else {
            this.snackbar.open('School Quarter Submission failed. ' + sessionStorage.getItem('httpError'), '', {
              duration: 10000
            });
          }

        });
      }else if (this.markingPeriodLevel === 'Quarter'){
        let arrList = Object.keys(this.obj);
        for (let i of arrList) {
          this.assignFieldsValue('progressPeriodAddModel', 'tableProgressPeriods', i, 'markingPeriodAddModel', 'tableSchoolYears', '');
        }
        this.progressPeriodAddModel.tableProgressPeriods.academicYear = +sessionStorage.getItem('academicyear');
        this.progressPeriodAddModel.tableProgressPeriods.startDate = this.commonFunction.formatDateSaveWithoutTime(this.progressPeriodAddModel.tableProgressPeriods.startDate);
        this.progressPeriodAddModel.tableProgressPeriods.endDate = this.commonFunction.formatDateSaveWithoutTime(this.progressPeriodAddModel.tableProgressPeriods.endDate);
        this.progressPeriodAddModel.tableProgressPeriods.postStartDate = this.commonFunction.formatDateSaveWithoutTime(this.progressPeriodAddModel.tableProgressPeriods.postStartDate);
        this.progressPeriodAddModel.tableProgressPeriods.postEndDate = this.commonFunction.formatDateSaveWithoutTime(this.progressPeriodAddModel.tableProgressPeriods.postEndDate);
        this.markingPeriodService.AddProgressPeriod(this.progressPeriodAddModel).subscribe(data => {
          if (data) {
            if (data._failure) {
              this.snackbar.open( data._message, '', {
                duration: 10000
              });
            } else {
              this.snackbar.open(data._message, '', {
                duration: 10000
              });
              this.sentArray = [true, sessionStorage.getItem('academicyear')];
              this.markingPeriodService.getCurrentYear(true);
              this.dialogRef.close(this.sentArray);
            }
          }
          else {
            this.snackbar.open('School Progress Period Submission failed. ' + sessionStorage.getItem('httpError'), '', {
              duration: 10000
            });
          }

        });
      }else{
        this.markingPeriodAddModel.tableSchoolYears.startDate = this.commonFunction.formatDateSaveWithoutTime(this.markingPeriodAddModel.tableSchoolYears.startDate);
        this.markingPeriodAddModel.tableSchoolYears.academicYear = +sessionStorage.getItem('academicyear');
        this.markingPeriodAddModel.tableSchoolYears.endDate = this.commonFunction.formatDateSaveWithoutTime(this.markingPeriodAddModel.tableSchoolYears.endDate);
        this.markingPeriodAddModel.tableSchoolYears.postStartDate = this.commonFunction.formatDateSaveWithoutTime(this.markingPeriodAddModel.tableSchoolYears.postStartDate);
        this.markingPeriodAddModel.tableSchoolYears.postEndDate = this.commonFunction.formatDateSaveWithoutTime(this.markingPeriodAddModel.tableSchoolYears.postEndDate);
        this.markingPeriodService.AddSchoolYear(this.markingPeriodAddModel).subscribe(data => {
          if (data) {
            if (data._failure) {
              this.snackbar.open( data._message, '', {
                duration: 10000
              });
            } else {

              this.snackbar.open(data._message, '', {
                duration: 10000
              });
              sessionStorage.setItem('academicyear', this.markingPeriodAddModel.tableSchoolYears.startDate.substr(0, 4));
              this.markingPeriodService.getCurrentYear(true);
              this.sentArray = [true, sessionStorage.getItem('academicyear')];
              this.dialogRef.close(this.sentArray);
            }
          }
          else {
            this.snackbar.open('School Year Submission failed. ' + sessionStorage.getItem('httpError'), '', {
              duration: 10000
            });
          }

        });
      }

    }
  }

 }
}
