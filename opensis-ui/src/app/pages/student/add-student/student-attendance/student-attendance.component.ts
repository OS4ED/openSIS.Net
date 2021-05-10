import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { CalendarEvent, CalendarView } from 'angular-calendar';

@Component({
  selector: 'vex-student-attendance',
  templateUrl: './student-attendance.component.html',
  styleUrls: ['./student-attendance.component.scss']
})
export class StudentAttendanceComponent implements OnInit {

  viewDate: Date = new Date();
  view: CalendarView | string = 'weekView';
  events: CalendarEvent[] = [];
  CalendarView = CalendarView;

  constructor(public translateService:TranslateService) { 
    translateService.use('en');
  }

  changeCalendarView(calendarType){
    this.view = calendarType;
  }

  ngOnInit(): void {
  }

}
