import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import icMissingAttendance from '@iconify/icons-ic/twotone-alarm-off';
import icAssessment from '@iconify/icons-ic/outline-assessment';
import icWarning from '@iconify/icons-ic/warning';
import { CalendarEvent, CalendarView } from 'angular-calendar';
import icHowToReg from '@iconify/icons-ic/outline-how-to-reg';

@Component({
  selector: 'vex-teacher-dashboard',
  templateUrl: './teacher-dashboard.component.html',
  styleUrls: ['./teacher-dashboard.component.scss'],
  styles: [
    `
     .cal-month-view .bg-aqua,
      .cal-week-view .cal-day-columns .bg-aqua,
      .cal-day-view .bg-aqua {
        background-color: #ffdee4 !important;
      }
    `,
  ],
})
export class TeacherDashboardComponent implements OnInit {

  icMissingAttendance = icMissingAttendance;
  icAssessment = icAssessment;
  icWarning = icWarning;
  panelOpenState = false;
  view: CalendarView = CalendarView.Month;
  viewDate: Date = new Date();
  events: CalendarEvent[] = [];
  icHowToReg = icHowToReg;

  constructor(public translateService: TranslateService) {
    translateService.use("en");
  }

  ngOnInit(): void {
  }

}
