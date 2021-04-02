import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AddStudentComponent } from './add-student/add-student.component';
import { AddCourseSectionComponent } from './add-course-section/add-course-section.component';
import { TranslateService } from '@ngx-translate/core';
import { StudentScheduleService } from '../../../services/student-schedule.service';
import { StudentCourseSectionScheduleAddViewModel, StudentScheduleReportViewModel } from 'src/app/models/studentCourseSectionScheduleAddViewModel';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { LoaderService } from '../../../services/loader.service';
import { MatTableDataSource } from '@angular/material/table';
import { ExcelService } from '../../../services/excel.service';
import { SharedFunction } from '../../shared/shared-function';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'vex-schedule-student',
  templateUrl: './schedule-student.component.html',
  styleUrls: ['./schedule-student.component.scss']
})
export class ScheduleStudentComponent implements OnInit, OnDestroy {
  studentList = [];
  viewReport: boolean = false;
  showReportTable: boolean = false;
  courseSectionList = [];
  showStudentCount: boolean = false;
  showCourseSectionCount: boolean = false;
  destroySubject$: Subject<void> = new Subject();
  studentCourseSectionScheduleAddViewModel: StudentCourseSectionScheduleAddViewModel = new StudentCourseSectionScheduleAddViewModel();
  studentScheduleReportViewModel: StudentScheduleReportViewModel = new StudentScheduleReportViewModel();
  loading: boolean;
  showCard: boolean = false;
  scheduleReport: MatTableDataSource<any>;
  displayedColumns: string[];
  constructor(private dialog: MatDialog, public translateService: TranslateService,
    private studentScheduleService: StudentScheduleService,
    private loaderService: LoaderService,
    private excelService: ExcelService,
    private snackbar: MatSnackBar) {
    translateService.use('en');
    this.loaderService.isLoading.pipe(takeUntil(this.destroySubject$)).subscribe((val) => {
      this.loading = val;
    });
  }

  ngOnInit(): void {
  }

  viewScheduledReport() {
    this.studentScheduleService.studentScheduleReport(this.studentScheduleReportViewModel).subscribe(data => {
      if (data._failure) {

      }
      else {
        if (data.scheduleReport.length > 0) {
          this.showReportTable = true;
        }
        this.displayedColumns = Object.keys(data.scheduleReport[0])

        this.scheduleReport = new MatTableDataSource(data.scheduleReport);
      }

    });
  }

  selectStudent() {
    this.dialog.open(AddStudentComponent, {
      width: '900px'
    }).afterClosed().subscribe((data) => {
      this.studentList = data;
      if (this.studentList?.length > 0) {
        this.showStudentCount = true;
        this.viewReport = false;
        this.showCard = false;
      }
      else {
        this.showStudentCount = false;
        this.showCard = false;
        this.viewReport = false;
      }
    });
  }

  selectCourseSection() {
    this.dialog.open(AddCourseSectionComponent, {
      width: '900px'
    }).afterClosed().subscribe((data) => {
      this.courseSectionList = data;
      if (this.courseSectionList?.length > 0) {
        this.showCourseSectionCount = true;
        this.viewReport = false;
        this.showCard = false;
      }
      else {
        this.showCourseSectionCount = false;
        this.showCard = false;
        this.viewReport = false;
      }
    });
  }

  scheduleStudent() {
    this.showCard = true;
    this.studentCourseSectionScheduleAddViewModel.courseSectionList = this.courseSectionList;
    this.studentCourseSectionScheduleAddViewModel.studentMasterList = this.studentList;
    this.studentCourseSectionScheduleAddViewModel.createdBy = sessionStorage.getItem('user');
    this.studentScheduleService.addStudentCourseSectionSchedule(this.studentCourseSectionScheduleAddViewModel).pipe(takeUntil(this.destroySubject$)).subscribe(data => {
      if (data._failure) {
        this.snackbar.open('Failed to schedule student', '', {
          duration: 5000
        });
      }
      else {
        this.studentCourseSectionScheduleAddViewModel = data;
        this.studentCourseSectionScheduleAddViewModel.conflictMessage = data.conflictMessage;
        this.viewReport = true;
      }
    });
  }

  refreshAll() {
    this.studentList=[];
    this.courseSectionList=[];
    this.showStudentCount = false;
    this.showCourseSectionCount = false;
    this.showCard = false;
    this.viewReport = false;
    this.showReportTable= false;
  }

  viewExcelReport() {

    this.studentScheduleService.studentScheduleReport(this.studentScheduleReportViewModel).subscribe(data => {
      if (data._failure) {

      }
      else {
        let reportData = data.scheduleReport;
        for (let report of reportData) {
          for (var key in report) {
            if (report.hasOwnProperty(key)) {
              if (report[key].split('|')[0].includes('False')) {
                report[key] = report[key].split('|')[1].trim()
              } else if (report[key].split('|')[0].includes('True')) {
                report[key] = ''
              }
            }
          }
        }
        if (reportData.length > 0) {
          this.excelService.exportAsExcelFile(reportData, 'Schedule_Report_List_')
        }
        else {
          this.snackbar.open('No Records Found. Failed to Export Schedule Report', '', {
            duration: 5000
          });
        }
      }

    });
  }

  ngOnDestroy() {
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }
}
