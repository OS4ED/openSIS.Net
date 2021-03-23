import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AddStudentComponent } from './add-student/add-student.component';
import { AddCourseSectionComponent } from './add-course-section/add-course-section.component';
import { ScheduleReport } from '../../../models/scheduleReportModel';
import { TranslateService } from '@ngx-translate/core';
import { StudentScheduleService } from 'src/app/services/student-schedule.service';
import { StudentCourseSectionScheduleAddViewModel } from 'src/app/models/studentCourseSectionScheduleAddViewModel';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';

@Component({
  selector: 'vex-schedule-student',
  templateUrl: './schedule-student.component.html',
  styleUrls: ['./schedule-student.component.scss']
})
export class ScheduleStudentComponent implements OnInit, OnDestroy {
  studentList = [];
  courseSectionList = [];
  showStudentCount: boolean = false;
  showCourseSectionCount: boolean = false;
  destroySubject$: Subject<void> = new Subject();
  studentCourseSectionScheduleAddViewModel: StudentCourseSectionScheduleAddViewModel = new StudentCourseSectionScheduleAddViewModel();
 
  scheduleReport: ScheduleReport[] = [
    { studentName: 'Danielle Boucher', studentId: '456123', geom001: true, algb001: true, whs05: true, lsc03: true },
    { studentName: 'Andrew Brown', studentId: '543126', geom001: true, algb001: true, whs05: true, lsc03: false },
    { studentName: 'Lian Fang', studentId: '745632', geom001: true, algb001: false, whs05: true, lsc03: true },
    { studentName: 'Amelia Jones', studentId: '766123', geom001: true, algb001: true, whs05: true, lsc03: true },
    { studentName: 'James Millar', studentId: '673126', geom001: true, algb001: true, whs05: true, lsc03: false },
    { studentName: 'Lian Fang', studentId: '745632', geom001: true, algb001: true, whs05: true, lsc03: true },
    { studentName: 'Danielle Boucher', studentId: '456123', geom001: true, algb001: true, whs05: true, lsc03: true },
    { studentName: 'Andrew Brown', studentId: '543126', geom001: true, algb001: true, whs05: false, lsc03: false },
    { studentName: 'Lian Fang', studentId: '745632', geom001: true, algb001: true, whs05: true, lsc03: true },
    { studentName: 'Amelia Jones', studentId: '766123', geom001: true, algb001: true, whs05: true, lsc03: true },
    { studentName: 'James Millar', studentId: '673126', geom001: true, algb001: true, whs05: true, lsc03: false },
    { studentName: 'Lian Fang', studentId: '745632', geom001: true, algb001: true, whs05: true, lsc03: true },
    { studentName: 'Danielle Boucher', studentId: '456123', geom001: true, algb001: true, whs05: true, lsc03: true },
    { studentName: 'Andrew Brown', studentId: '543126', geom001: true, algb001: true, whs05: false, lsc03: false },
    { studentName: 'Lian Fang', studentId: '745632', geom001: true, algb001: true, whs05: true, lsc03: true },
    { studentName: 'Amelia Jones', studentId: '766123', geom001: true, algb001: true, whs05: true, lsc03: true },
    { studentName: 'James Millar', studentId: '673126', geom001: true, algb001: true, whs05: true, lsc03: false },
    { studentName: 'Lian Fang', studentId: '745632', geom001: true, algb001: false, whs05: true, lsc03: true }
  ];
  displayedColumns: string[] = ['studentName', 'studentId', 'geom001', 'algb001', 'whs05', 'lsc03'];
  constructor(private dialog: MatDialog, public translateService: TranslateService,
    private studentScheduleService: StudentScheduleService) {
    translateService.use('en');
  }

  ngOnInit(): void {
  }

  selectStudent() {
    this.dialog.open(AddStudentComponent, {
      width: '900px'
    }).afterClosed().subscribe((data) => {
      this.studentList = data;
      if (this.studentList.length > 0) {
        this.showStudentCount = true;
      }
    });
  }

  scheduleStudent() {
    this.studentCourseSectionScheduleAddViewModel.courseSectionList = this.courseSectionList;
    this.studentCourseSectionScheduleAddViewModel.studentMasterList = this.studentList;
    this.studentCourseSectionScheduleAddViewModel.createdBy = sessionStorage.getItem('user');
    this.studentScheduleService.addStudentCourseSectionSchedule(this.studentCourseSectionScheduleAddViewModel).pipe(takeUntil(this.destroySubject$)).subscribe(data => {
      if (data._failure) {

      }
      else {
        this.studentCourseSectionScheduleAddViewModel = data;
      }

    });


  }

  selectCourseSection() {
    this.dialog.open(AddCourseSectionComponent, {
      width: '900px'
    }).afterClosed().subscribe((data) => {
      this.courseSectionList = data;
      if (this.courseSectionList.length > 0) {
        this.showCourseSectionCount = true;
      }
    });
  }

  ngOnDestroy() {
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }
}
