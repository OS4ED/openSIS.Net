import { Component, OnInit, ViewEncapsulation } from "@angular/core";
import icEdit from "@iconify/icons-ic/twotone-edit";
import icDelete from "@iconify/icons-ic/twotone-delete";
import icMoreVert from '@iconify/icons-ic/more-vert';
import { MatDialog } from "@angular/material/dialog";
import { TranslateService } from "@ngx-translate/core";
import { AddAssignCourseComponent } from "./add-assign-course/add-assign-course.component";
import {
  AfterViewInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  ElementRef,
  ViewChild,
} from '@angular/core';
import { CalendarEvent, CalendarView } from 'angular-calendar';
import { differenceInMinutes, startOfDay, startOfHour } from 'date-fns';
import { setHours, setMinutes } from 'date-fns';

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
  selector: "vex-student-course-schedule",
  templateUrl: "./student-course-schedule.component.html",
  styleUrls: ["./student-course-schedule.component.scss"],
  encapsulation: ViewEncapsulation.None,
})

export class StudentCourseScheduleComponent implements OnInit {

  icEdit = icEdit;
  icDelete = icDelete;
  icMoreVert = icMoreVert;
  view: CalendarView = CalendarView.Week;
  viewDate: Date = new Date();
  events: CalendarEvent[] = [];
  activeDayIsOpen: boolean = true;
  CalendarView = CalendarView;
  scheduleSwitch = true;

  dayevents: CalendarEvent[] = [
    {
      title: 'No event end date',
      start: setHours(setMinutes(new Date(), 0), 3),
      color: colors.blue,
    },
    {
      title: 'No event end date',
      start: setHours(setMinutes(new Date(), 0), 5),
      color: colors.yellow,
    },
  ];

  constructor(private dialog: MatDialog, public translateService:TranslateService) { 
    translateService.use('en');
  }

  ngOnInit(): void {}

  selectAssignCourse() {
    this.dialog.open(AddAssignCourseComponent, {
      width: '900px'
    });
  }

  changeCalendarView(calendarType){
    this.scheduleSwitch = false;
    this.view = calendarType;
  }

  listAll(){
    this.scheduleSwitch = true;
  }

}
