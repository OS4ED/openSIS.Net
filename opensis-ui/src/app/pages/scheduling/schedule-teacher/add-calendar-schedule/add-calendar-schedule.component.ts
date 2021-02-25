import { Component, ChangeDetectionStrategy, Input, OnChanges, OnInit, SimpleChanges, ViewEncapsulation, } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import icClose from '@iconify/icons-ic/twotone-close';
import { TranslateService } from '@ngx-translate/core';
import {
  isSameMonth,
  isSameDay,
  startOfMonth,
  endOfMonth,
  startOfWeek,
  endOfWeek,
  startOfDay,
  endOfDay,
  format,
} from 'date-fns';
import {
  CalendarEvent,
  CalendarEventAction,
  CalendarEventTimesChangedEvent,
  CalendarMonthViewBeforeRenderEvent,
  CalendarView,
} from 'angular-calendar';

import { Subject } from 'rxjs';


const colors: any = {
  red: {
    primary: '#ad2121',
    secondary: '#FAE3E3',
  },
  blue: {
    primary: '#1e90ff',
    secondary: '#D1E8FF',
  },
  yellow: {
    primary: '#e3bc08',
    secondary: '#FDF1BA',
  },
};

@Component({
  selector: 'vex-add-calendar-schedule',
  templateUrl: './add-calendar-schedule.component.html',
  styleUrls: ['./add-calendar-schedule.component.scss'],
  styles: [
    `
     .cal-month-view .bg-aqua,
      .cal-week-view .cal-day-columns .bg-aqua,
      .cal-day-view .bg-aqua {
        background-color: #ffdee4 !important;
      }
    `,
  ],
  encapsulation: ViewEncapsulation.None
})
export class AddCalendarScheduleComponent implements OnInit {
  icClose = icClose;
  view: CalendarView = CalendarView.Month;
  viewDate: Date = new Date();
  events: CalendarEvent[] = [];
  cssClass: string;
  refresh: Subject<any> = new Subject();
  weekendDays=[];
  activeDayIsOpen: boolean = true;
  filterDays = [];

  constructor(private dialogRef: MatDialogRef<AddCalendarScheduleComponent>, public translateService:TranslateService) { 
    translateService.use('en');
  }

  // ngOnChanges(changes: SimpleChanges): void {
  //   this.selectedCalendar;
  //   // if (this.selectedCalendar.days !== null && this.selectedCalendar.days !== undefined) {
  //   //   this.showError = false;
  //   //   this.getDays(this.selectedCalendar.days);
  //   //   this.refresh.next();
  //   // }

  // }
  ngOnInit(): void {
  }


  handleEvent(action: string, event: CalendarEvent): void {
    console.log(action, event);
  }
}
