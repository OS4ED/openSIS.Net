import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AddStudentComponent } from './add-student/add-student.component';
import { AddCourseSectionComponent } from './add-course-section/add-course-section.component';
import { ScheduleReport } from '../../../models/scheduleReportModel';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'vex-schedule-student',
  templateUrl: './schedule-student.component.html',
  styleUrls: ['./schedule-student.component.scss']
})
export class ScheduleStudentComponent implements OnInit {

  scheduleReport: ScheduleReport[] = [
    {studentName: 'Danielle Boucher', studentId: '456123', geom001: true, algb001: true, whs05: true, lsc03: true},
    {studentName: 'Andrew Brown', studentId: '543126', geom001: true, algb001: true, whs05: true, lsc03: false},
    {studentName: 'Lian Fang', studentId: '745632', geom001: true, algb001: false, whs05: true, lsc03: true},
    {studentName: 'Amelia Jones', studentId: '766123', geom001: true, algb001: true, whs05: true, lsc03: true},
    {studentName: 'James Millar', studentId: '673126', geom001: true, algb001: true, whs05: true, lsc03: false},
    {studentName: 'Lian Fang', studentId: '745632', geom001: true, algb001: true, whs05: true, lsc03: true},
    {studentName: 'Danielle Boucher', studentId: '456123', geom001: true, algb001: true, whs05: true, lsc03: true},
    {studentName: 'Andrew Brown', studentId: '543126', geom001: true, algb001: true, whs05: false, lsc03: false},
    {studentName: 'Lian Fang', studentId: '745632', geom001: true, algb001: true, whs05: true, lsc03: true},
    {studentName: 'Amelia Jones', studentId: '766123', geom001: true, algb001: true, whs05: true, lsc03: true},
    {studentName: 'James Millar', studentId: '673126', geom001: true, algb001: true, whs05: true, lsc03: false},
    {studentName: 'Lian Fang', studentId: '745632', geom001: true, algb001: true, whs05: true, lsc03: true},
    {studentName: 'Danielle Boucher', studentId: '456123', geom001: true, algb001: true, whs05: true, lsc03: true},
    {studentName: 'Andrew Brown', studentId: '543126', geom001: true, algb001: true, whs05: false, lsc03: false},
    {studentName: 'Lian Fang', studentId: '745632', geom001: true, algb001: true, whs05: true, lsc03: true},
    {studentName: 'Amelia Jones', studentId: '766123', geom001: true, algb001: true, whs05: true, lsc03: true},
    {studentName: 'James Millar', studentId: '673126', geom001: true, algb001: true, whs05: true, lsc03: false},
    {studentName: 'Lian Fang', studentId: '745632', geom001: true, algb001: false, whs05: true, lsc03: true}
  ];
  displayedColumns: string[] = ['studentName', 'studentId', 'geom001', 'algb001', 'whs05', 'lsc03'];
  constructor(private dialog: MatDialog, public translateService:TranslateService) {
    translateService.use('en');
   }

  ngOnInit(): void {
  }

  selectStudent(){
    this.dialog.open(AddStudentComponent, {
      width: '900px'
    });
  }

  selectCourseSection(){
    this.dialog.open(AddCourseSectionComponent, {
      width: '900px'
    });
  }

}
