import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import icClose from '@iconify/icons-ic/twotone-close';
import { TranslateService } from '@ngx-translate/core';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import {
  CalendarEvent,
  CalendarMonthViewBeforeRenderEvent,
  CalendarView,
} from 'angular-calendar';
import { Subject } from 'rxjs';
import { CourseSectionList } from '../../../../models/teacher-schedule.model';
@Component({
  selector: 'vex-add-days-schedule',
  templateUrl: './add-days-schedule.component.html',
  styleUrls: ['./add-days-schedule.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ],  styles: [
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
export class AddDaysScheduleComponent implements OnInit {
  icClose = icClose;
  scheduleDetails:CourseSectionList;
  constructor(private dialogRef: MatDialogRef<AddDaysScheduleComponent>,
    @Inject(MAT_DIALOG_DATA) public data:CourseSectionList, public translateService:TranslateService) { 
    translateService.use('en');
      this.scheduleDetails=data;
  }

  view: CalendarView = CalendarView.Month;
  viewDate: Date = new Date();
  events: CalendarEvent[] = [];
  cssClass: string;
  refresh: Subject<any> = new Subject();
  weekendDays=[];
  activeDayIsOpen: boolean = true;
  filterDays = [];
  color=['bg-deep-orange','bg-red','bg-green','bg-teal','bg-cyan','bg-deep-purple','bg-pink','bg-blue'];   
  calendarDayDetails=false;
  classPeriodName;
  classRoomName;
  classdate;
  classtakeAttendance;
  ngOnInit(): void {
    if(this.scheduleDetails.scheduleType=='Calendar Schedule'){
      let days = this.scheduleDetails.weekDays;
      if (days) {
        this.getDays(days);
      }
      this.renderCalendarPeriods();
    }
  }

   //render weekends
   getDays(days: string) {
    const calendarDays = days;
    var allDays = [0, 1, 2, 3, 4, 5, 6];
    var splitDays = calendarDays.split('').map(x => +x);
    this.filterDays = allDays.filter(f => !splitDays.includes(f));
    this.weekendDays = this.filterDays;
    this.cssClass = 'bg-aqua';
    this.refresh.next();
  }

  renderCalendarPeriods() {
    this.events = [];
    for (let schedule of this.scheduleDetails.courseCalendarSchedule) {
     let random=Math.floor((Math.random() * 7) + 0);
      this.events.push({
        start: new Date(schedule.date),
        end: new Date(schedule.date),
        title: schedule.blockPeriod.periodTitle,
        color: null,
        actions: null,
        allDay: schedule.takeAttendance,
        resizable: {
          beforeStart: true,
          afterEnd: true,
        },
        draggable: false,
        meta: {scheduleDetails:schedule,randomColor:this.color[random]}
      });
      this.refresh.next();
    }
  }

   //for rendar weekends
   beforeMonthViewRender(renderEvent: CalendarMonthViewBeforeRenderEvent): void {
    renderEvent.body.forEach((day) => {
    const dayOfMonth = day.date.getDay();
    if (this.filterDays.includes(dayOfMonth)) {
      day.cssClass = this.cssClass;
    }
  });    
}

viewEvent(event){
  this.classPeriodName= event.title;
  this.classRoomName = event.meta.scheduleDetails.rooms.title;
  this.classdate= event.meta.scheduleDetails.date;
  this.classtakeAttendance= event.meta.scheduleDetails.takeAttendance ? 'Yes':'No';
  this.calendarDayDetails=true;
}

closeDetails(){
  this.calendarDayDetails=false;
}



  handleEvent(action: string, event: CalendarEvent): void {
    console.log(action, event);
  }

}
