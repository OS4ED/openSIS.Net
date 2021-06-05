/***********************************************************************************
openSIS is a free student information system for public and non-public
schools from Open Solutions for Education, Inc.Website: www.os4ed.com.

Visit the openSIS product website at https://opensis.com to learn more.
If you have question regarding this software or the license, please contact
via the website.

The software is released under the terms of the GNU Affero General Public License as
published by the Free Software Foundation, version 3 of the License.
See https://www.gnu.org/licenses/agpl-3.0.en.html.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

Copyright (c) Open Solutions for Education, Inc.

All rights reserved.
***********************************************************************************/

import { Component, Inject, OnInit, ViewEncapsulation } from "@angular/core";
import { MAT_DIALOG_DATA } from "@angular/material/dialog";
import icClose from "@iconify/icons-ic/twotone-close";
import { TranslateService } from "@ngx-translate/core";
import { fadeInUp400ms } from "../../../../../@vex/animations/fade-in-up.animation";
import { stagger60ms } from "../../../../../@vex/animations/stagger.animation";
import {
  CalendarEvent,
  CalendarMonthViewBeforeRenderEvent,
  CalendarView,
} from "angular-calendar";
import { Subject } from "rxjs";
import { CourseSectionList } from "../../../../models/teacher-schedule.model";
import { weeks } from "../../../../common/static-data";
@Component({
  selector: "vex-add-days-schedule",
  templateUrl: "./add-days-schedule.component.html",
  styleUrls: ["./add-days-schedule.component.scss"],
  animations: [stagger60ms, fadeInUp400ms],
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
export class AddDaysScheduleComponent implements OnInit {
  icClose = icClose;
  scheduleDetails: CourseSectionList;
  memberName: string;
  constructor(
    @Inject(MAT_DIALOG_DATA) public data,
    public translateService: TranslateService
  ) {
    translateService.use("en");
    if (data.fromTeacherSchedule) {
      this.scheduleDetails = data.scheduleDetails;
    } else {
      this.scheduleDetails = data.scheduleDetails;
      this.scheduleDetails.courseVariableSchedule =
        data.scheduleDetails.courseVariableScheduleList;
      this.scheduleDetails.courseCalendarSchedule =
        data.scheduleDetails.courseCalendarScheduleList;
      this.scheduleDetails.courseBlockSchedule =
        data.scheduleDetails.courseBlockScheduleList;
      this.checkScheduleType();
    }
    this.memberName = sessionStorage.getItem('membershipName');
  }

  view: CalendarView = CalendarView.Month;
  viewDate: Date = new Date();
  events: CalendarEvent[] = [];
  cssClass: string;
  refresh: Subject<any> = new Subject();
  weekendDays = [];
  activeDayIsOpen: boolean = true;
  filterDays = [];
  color = [
    "bg-deep-orange",
    "bg-red",
    "bg-green",
    "bg-teal",
    "bg-cyan",
    "bg-deep-purple",
    "bg-pink",
    "bg-blue",
  ];
  calendarDayDetails = false;
  classPeriodName;
  classRoomName;
  classdate;
  classtakeAttendance;
  ngOnInit(): void {
    if (this.data.fromTeacherSchedule) {
      if (this.scheduleDetails.scheduleType == "Calendar Schedule") {
        let days = this.scheduleDetails.weekDays;
        if (days) {
          this.getDays(days);
        }
        this.renderCalendarPeriods();
        this.viewDate = new Date(this.scheduleDetails.durationStartDate);
      }
    } else {
      if (this.scheduleDetails.courseCalendarSchedule?.length > 0) {
        let weekDays: string;
        this.scheduleDetails.dayOfWeek?.split("|").map((item) => {
          weeks.map((day) => {
            if (day.name.toLowerCase() == item.toLowerCase()) {
              weekDays = weekDays + day.id;
            }
          });
        });

        let days = weekDays;
        if (days) {
          this.getDays(days);
        }
        this.renderCalendarPeriods();
        this.viewDate = new Date(this.scheduleDetails.mpStartDate);
      }
    }
  }

  checkScheduleType() {
    if (this.scheduleDetails.courseFixedSchedule) {
      this.scheduleDetails.scheduleType = "Fixed Schedule";
    } else if (this.scheduleDetails.courseVariableSchedule?.length > 0) {
      this.scheduleDetails.scheduleType = "Variable Schedule";
    } else if (this.scheduleDetails.courseCalendarSchedule?.length > 0) {
      this.scheduleDetails.scheduleType = "Calendar Schedule";
    } else if (this.scheduleDetails.courseBlockSchedule?.length > 0) {
      this.scheduleDetails.scheduleType = "Block Schedule";
    }
  }

  // render weekends
  getDays(days: string) {
    const calendarDays = days;
    let allDays = [0, 1, 2, 3, 4, 5, 6];
    let splitDays = calendarDays.split("").map((x) => +x);
    this.filterDays = allDays.filter((f) => !splitDays.includes(f));
    this.weekendDays = this.filterDays;
    this.cssClass = "bg-aqua";
    this.refresh.next();
  }

  renderCalendarPeriods() {
    this.events = [];
    for (let schedule of this.scheduleDetails?.courseCalendarSchedule) {
      let random = Math.floor(Math.random() * 7 + 0);
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
        meta: { scheduleDetails: schedule, randomColor: this.color[random] },
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

  viewEvent(event) {
    this.classPeriodName = event.title;
    this.classRoomName = event.meta.scheduleDetails.rooms.title;
    this.classdate = event.meta.scheduleDetails.date;
    this.classtakeAttendance = event.meta.scheduleDetails.takeAttendance
      ? "Yes"
      : "No";
    this.calendarDayDetails = true;
  }

  closeDetails() {
    this.calendarDayDetails = false;
  }
}
