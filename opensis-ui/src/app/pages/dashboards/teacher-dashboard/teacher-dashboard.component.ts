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

import { Component, OnDestroy, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import icMissingAttendance from '@iconify/icons-ic/twotone-alarm-off';
import icAssessment from '@iconify/icons-ic/outline-assessment';
import icWarning from '@iconify/icons-ic/warning';
import { CalendarEvent, CalendarMonthViewBeforeRenderEvent, CalendarView } from 'angular-calendar';
import icHowToReg from '@iconify/icons-ic/outline-how-to-reg';
import { DasboardService } from '../../../services/dasboard.service';
import { DashboardViewModel, ScheduledCourseSectionViewModel } from '../../../models/dashboard.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable, Subject } from 'rxjs';
import { CalendarEventModel } from 'src/app/models/calendar-event.model';
import { CalendarModel } from 'src/app/models/calendar.model';
import { map, shareReplay, takeUntil, tap } from 'rxjs/operators';
import { SchoolService } from 'src/app/services/school.service';

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
export class TeacherDashboardComponent implements OnInit,OnDestroy {
  view: CalendarView = CalendarView.Month;
  viewDate: Date = new Date();
  events: CalendarEvent[] = [];
  events$: Observable<CalendarEvent<{ calendar: CalendarEventModel }>[]>;
  calendars: CalendarModel;
  activeDayIsOpen = true;
  weekendDays: number[];
  filterDays = [];

  icMissingAttendance = icMissingAttendance;
  icAssessment = icAssessment;
  icWarning = icWarning;
  panelOpenState = false;
  icHowToReg = icHowToReg;
  scheduledCourseSectionViewModel: ScheduledCourseSectionViewModel = new ScheduledCourseSectionViewModel();
  dashboardViewModel: DashboardViewModel = new DashboardViewModel();
  calendarTitle: string;
  noticeBody: string;
  refresh: Subject<any> = new Subject();
  destroySubject$: Subject<void> = new Subject();
  cssClass = 'bg-aqua';
  showCalendarView: boolean = false;
  noticeHide: boolean = true;
  periodTitle: string;
  roomTitle: string;
  periodStartTime: string;
  takeAttendance: boolean;
  classCount:number=0;
  noticeCount:number=0;
  weeks = [
    { name: 'Sunday', id: 0 },
    { name: 'Monday', id: 1 },
    { name: 'Tueday', id: 2 },
    { name: 'Wednesday', id: 3 },
    { name: 'Thursday', id: 4 },
    { name: 'Friday', id: 5 },
    { name: 'Saturday', id: 6 }
  ];

  constructor(public translateService: TranslateService,
    private snackbar: MatSnackBar,
    private schoolService: SchoolService,
    private dasboardService: DasboardService) {
    translateService.use("en");
  }

  ngOnInit(): void {
    this.schoolService.schoolListCalled.pipe(takeUntil(this.destroySubject$)).subscribe((res) => {

      if (res.academicYearChanged || res.academicYearLoaded) {
        this.getDashboardViewForStaff();
        this.getDashboardViewForCalendarView();
      }
    })

  }

  getDashboardViewForStaff() {
    this.scheduledCourseSectionViewModel.staffId = +sessionStorage.getItem('userId');
    this.dasboardService.getDashboardViewForStaff(this.scheduledCourseSectionViewModel).subscribe((res) => {
      if (res) {
        if (res._failure) {
          this.classCount=0;
          this.noticeCount=0;
        }
        else {
          this.scheduledCourseSectionViewModel = res;
          this.scheduledCourseSectionViewModel.courseSectionViewList = this.findMeetingDays(this.scheduledCourseSectionViewModel.courseSectionViewList);
          console.log(this.scheduledCourseSectionViewModel.courseSectionViewList);
          this.classCount= this.scheduledCourseSectionViewModel.courseSectionViewList.length;
          this.noticeCount=this.scheduledCourseSectionViewModel.noticeList.length;
        }
      }
      else {
        this.snackbar.open('Dashboard View failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
        this.classCount=0;
        this.noticeCount=0;
      }
    });
  }

  findMeetingDays(courseSectionList) {
    courseSectionList = courseSectionList.map((item) => {
      if (item.scheduleType === 'Fixed Schedule' || item.scheduleType=='Variable Schedule') {
        let days = item.meetingDays.split('|')
        days.map((day) => {
          for (let [i, weekDay] of this.weeks.entries()) {
            if (weekDay.name == day.trim()) {
              item.meetingDays = item.meetingDays + weekDay.id;
              break;
            }
          }
        })
      }
      return item;

    });
    return courseSectionList;
  }

  getAttendanceForPeriod(courseSection) {
    if (courseSection.scheduleType === "Fixed Schedule") {
      this.takeAttendance = courseSection.takeAttendanceForFixedSchedule ? true : false;
    }
    return this.takeAttendance;
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

  getPeriodTitle(courseSection) {
    if (courseSection.scheduleType === "Fixed Schedule") {
      this.periodTitle = courseSection?.courseFixedSchedule?.blockPeriod?.periodTitle;

    }

    return this.periodTitle;
  }

  todaysClasses(){
    this.scheduledCourseSectionViewModel.allCourse= false;
    this.getDashboardViewForStaff();
  }

  allClasses(){
    this.scheduledCourseSectionViewModel.allCourse= true;
    this.getDashboardViewForStaff();
  }

  getPeriodStartTime(courseSection) {
    if (courseSection.scheduleType === "Fixed Schedule") {
      this.periodStartTime = new Date("1900-01-01T" + courseSection?.courseFixedSchedule?.blockPeriod?.periodStartTime).toString();

    }
    return this.periodStartTime;
  }

  getRoomTitle(courseSection) {
    if (courseSection.scheduleType === "Fixed Schedule") {
      this.roomTitle = courseSection?.courseFixedSchedule?.rooms?.title;

    }
    return this.roomTitle;
  }

  getDashboardViewForCalendarView() {
    this.events$ = this.dasboardService.getDashboardViewForCalendarView(this.dashboardViewModel).pipe(shareReplay(), tap((res) => {
      if (res) {
        if (res._failure) {
        }
        else {
          this.calendars = res.schoolCalendar;
          this.showCalendarView = false;
          if (this.calendars !== null) {
            this.calendarTitle = res.schoolCalendar.title;
            this.showCalendarView = true;
            this.getDays(this.calendars.days);
          }
        }
      }
      else {
        this.snackbar.open('Dashboard View failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
    }),
      map(({ calendarEventList }: { calendarEventList: CalendarEventModel[] }) => {
        if (calendarEventList !== null) {
          let eventList = calendarEventList.map((calendar: CalendarEventModel) => {

            return {
              id: calendar.eventId,
              title: calendar.title,
              start: new Date(calendar.startDate),
              end: new Date(calendar.endDate),
              allDay: true,
              meta: {
                calendar,
              },
              draggable: true
            };
          });
          return eventList.sort((n1, n2) => {
            if (n1.start > n2.start) {
              return 1;
            }

            if (n1.start < n2.start) {
              return -1;
            }

            return 0;
          });
        }

      })
    );
  }

  selectCourseSection(courseSection) {
    let courseSectionId = courseSection.courseSectionId;
    localStorage.setItem("courseSectionId", courseSectionId);
    localStorage.setItem("selectedCourseSection",JSON.stringify(courseSection));
  }

  getDays(days: string) {
    const calendarDays = days;
    let allDays = [0, 1, 2, 3, 4, 5, 6];
    let splitDays = calendarDays.split('').map(x => +x);
    this.filterDays = allDays.filter(f => !splitDays.includes(f));
    this.weekendDays = this.filterDays;
    this.cssClass = 'bg-aqua';
    this.refresh.next();
  }

  ngOnDestroy(): void {
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }

}
