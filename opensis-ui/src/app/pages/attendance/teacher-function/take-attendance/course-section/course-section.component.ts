import { Component, OnInit, ViewEncapsulation } from "@angular/core";
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
  subDays,
  addDays,
} from "date-fns";
import {
  CalendarEvent,
  CalendarEventAction,
  CalendarEventTimesChangedEvent,
  CalendarMonthViewBeforeRenderEvent,
  CalendarView,
} from "angular-calendar";
import { Router} from '@angular/router';
import { Subject } from "rxjs";

@Component({
  selector: "vex-course-section",
  templateUrl: "./course-section.component.html",
  styleUrls: ["./course-section.component.scss"],
  styles: [
    `
      .cal-month-view .bg-aqua,
      .cal-week-view .cal-day-columns .bg-aqua,
      .cal-day-view .bg-aqua {
        background-color: #ffdee4 !important;
      }
    `,
  ],
  encapsulation: ViewEncapsulation.None,
})
export class CourseSectionComponent implements OnInit {
  view: CalendarView = CalendarView.Month;
  viewDate: Date = new Date();
  // events: CalendarEvent[] = [];
  cssClass: string;
  refresh: Subject<any> = new Subject();
  pageStatus = "Teacher Function";

  constructor( private router: Router) {}

  ngOnInit(): void {}
  events: CalendarEvent[] = [
    {
      start: subDays(startOfDay(new Date()), 1),
      end: addDays(new Date(), 1),
      title: "GEOM001 - At 3.30 PM",
    },
    {
      start: subDays(endOfMonth(new Date()), 3),
      end: addDays(endOfMonth(new Date()), 3),
      title: "GEOM005 - At 3.30 PM",
      allDay: true,
    },
  ];
  eventClicked(event: CalendarEvent, index: number): void {
    this.router.navigate(["school/take-attendance/period-attendance/"]);
  }
}
