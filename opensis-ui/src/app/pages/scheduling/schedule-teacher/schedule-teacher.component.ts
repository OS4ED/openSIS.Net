import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AddTeacherComponent } from './add-teacher/add-teacher.component';
import { AddCourseSectionComponent } from './add-course-section/add-course-section.component';
import { TranslateService } from '@ngx-translate/core';
import { AddVariableScheduleComponent } from './add-variable-schedule/add-variable-schedule.component';
import { AddDaysScheduleComponent } from './add-days-schedule/add-days-schedule.component';
import { AddCalendarScheduleComponent } from './add-calendar-schedule/add-calendar-schedule.component';

@Component({
  selector: 'vex-schedule-teacher',
  templateUrl: './schedule-teacher.component.html',
  styleUrls: ['./schedule-teacher.component.scss']
})
export class ScheduleTeacherComponent implements OnInit {

  constructor(private dialog: MatDialog, public translateService:TranslateService) { 
    translateService.use('en');
  }

  ngOnInit(): void {
  }

  selectTeacher(){
    this.dialog.open(AddTeacherComponent, {
      width: '900px'
    });
  }

  selectCourseSection(){
    this.dialog.open(AddCourseSectionComponent, {
      width: '900px'
    });
  }

  selectCalendar(){
    this.dialog.open(AddCalendarScheduleComponent, {
      width: '700px'
    });
  }

  selectDays(){
    this.dialog.open(AddDaysScheduleComponent, {
      width: '600px'
    });
  }

  selectVariableSchedule() {
    this.dialog.open(AddVariableScheduleComponent, {
      width: '600px'
    });
  }

}
