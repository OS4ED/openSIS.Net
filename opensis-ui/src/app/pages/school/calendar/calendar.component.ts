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

import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit, TemplateRef, ViewChild, ViewEncapsulation } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { CalendarEvent, CalendarEventAction, CalendarEventTimesChangedEvent, CalendarMonthViewBeforeRenderEvent, CalendarMonthViewDay, CalendarView, DAYS_OF_WEEK } from 'angular-calendar';
import { addDays, addHours, endOfDay, endOfMonth, endOfWeek, format, isSameDay, isSameMonth, startOfDay, startOfMonth, startOfWeek, subDays } from 'date-fns';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CalendarEditComponent } from './calendar-edit/calendar-edit.component';
import { AddCalendarComponent } from './add-calendar/add-calendar.component';
import { AddEventComponent } from './add-event/add-event.component';
import icAdd from '@iconify/icons-ic/add';
import icEdit from '@iconify/icons-ic/edit';
import icDelete from '@iconify/icons-ic/delete';
import icWarning from '@iconify/icons-ic/warning';
import icChevronLeft from '@iconify/icons-ic/twotone-chevron-left';
import icChevronRight from '@iconify/icons-ic/twotone-chevron-right';
import { FormControl } from '@angular/forms';
import { CalendarService } from '../../../services/calendar.service';
import { CalendarAddViewModel, CalendarListModel, CalendarModel } from '../../../models/calendar.model';
import { GetAllMembersList } from '../../../models/membership.model';
import { MembershipService } from '../../../services/membership.service';
import { CalendarEventService } from '../../../services/calendar-event.service';
import { CalendarEventAddViewModel, CalendarEventListViewModel, CalendarEventModel } from '../../../models/calendar-event.model';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmDialogComponent } from '../../shared-module/confirm-dialog/confirm-dialog.component';
import * as moment from 'moment';
import { LayoutService } from 'src/@vex/services/layout.service';
import { LoaderService } from '../../../services/loader.service';
import { RolePermissionListViewModel, RolePermissionViewModel } from '../../../models/roll-based-access.model';
import { RollBasedAccessService } from '../../../services/roll-based-access.service';
import { CryptoService } from '../../../services/Crypto.service';
import { DefaultValuesService } from '../../../common/default-values.service';
const colors: any = {
  blue: {
    primary: '#5c77ff',
    secondary: '#FFFFFF'
  },
  yellow: {
    primary: '#ffc107',
    secondary: '#FDF1BA'
  },
  red: {
    primary: '#f44336',
    secondary: '#FFFFFF'
  }
};

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'vex-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss'],
  encapsulation: ViewEncapsulation.None
})

export class CalendarComponent implements OnInit {
  @ViewChild('modalContent', { static: true }) modalContent: TemplateRef<any>;
  isMarkingPeriod: string;
  getCalendarList: CalendarListModel = new CalendarListModel();
  getAllMembersList: GetAllMembersList = new GetAllMembersList();
  getAllCalendarEventList: CalendarEventListViewModel = new CalendarEventListViewModel();
  calendarAddViewModel = new CalendarAddViewModel();
  calendarEventAddViewModel = new CalendarEventAddViewModel();
  showCalendarView: boolean = false;
  view: CalendarView = CalendarView.Month;
  calendars: CalendarModel[];
  activeDayIsOpen = true;
  weekendDays: number[];
  filterDays = [];
  CalendarView = CalendarView;
  viewDate: Date = new Date();
  selectedCalendar = new CalendarModel();
  icChevronLeft = icChevronLeft;
  icChevronRight = icChevronRight;
  icAdd = icAdd;
  icEdit = icEdit;
  icDelete = icDelete;
  icWarning = icWarning;
  events$: Observable<CalendarEvent<{ calendar: CalendarEventModel }>[]>;
  refresh: Subject<any> = new Subject();
  calendarFrom: FormControl;
  cssClass: string;
  loading: boolean;
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();
  constructor(private http: HttpClient,
              private dialog: MatDialog,
              private snackbar: MatSnackBar,
              public translate: TranslateService,
              private membershipService: MembershipService,
              private calendarEventService: CalendarEventService,
              private calendarService: CalendarService,
              private layoutService: LayoutService,
              public rollBasedAccessService: RollBasedAccessService,
              private loaderService: LoaderService,
              private cdr: ChangeDetectorRef,
              private cryptoService: CryptoService,
              private defaultValuesService: DefaultValuesService ) {
    this.translate.setDefaultLang('en');
    if (localStorage.getItem("collapseValue") !== null) {
      if (localStorage.getItem("collapseValue") === "false") {
        this.layoutService.expandSidenav();
      } else {
        this.layoutService.collapseSidenav();
      }
    } else {
      this.layoutService.expandSidenav();
    }
    this.loaderService.isLoading.subscribe((res) => {
      this.loading = res;
    });
    this.calendarEventService.currentEvent.subscribe(
      res => {
        if (res) {
          this.getAllCalendarEvent();
        }
      }
    )
  }

  changeCalendar(event) {
    this.getDays(event.days);
    this.calendarService.setCalendarId(event.calenderId);
    this.getAllCalendarEvent();
  }

  ngAfterViewChecked() {
    this.cdr.detectChanges();
  }

  //Show all members
  getAllMemberList() {
    this.membershipService.getAllMembers(this.getAllMembersList).subscribe(
      (res) => {
        if (typeof (res) == 'undefined') {
          this.snackbar.open('No Member Found. ' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else {
          if (res._failure) {
            this.snackbar.open('No Member Found. ' + res._message, '', {
              duration: 10000
            });
          }
          else {
            this.getAllMembersList = res;
          }
        }
      });
  }
  
  //Show all calendar
  getAllCalendar() {
    this.calendarService.getAllCalendar(this.getCalendarList).subscribe((data) => {
      this.calendars = data.calendarList;
      this.showCalendarView = false;
      if (this.calendars.length !== 0) {
        this.showCalendarView = true;
        const defaultCalender = this.calendars.find(element => element.defaultCalender === true);
        if (defaultCalender != null) {
          this.selectedCalendar = defaultCalender;
          this.calendarService.setCalendarId(this.selectedCalendar.calenderId);
          this.getDays(this.selectedCalendar.days);
          this.getAllCalendarEvent();
        }
        this.refresh.next();
      }

    });

  }

  // Rendar all events in calendar
  getAllCalendarEvent() {
    this.getAllCalendarEventList.calendarId = this.calendarService.getCalendarId();
    this.events$ = this.calendarEventService.getAllCalendarEvent(this.getAllCalendarEventList).pipe(
      map(({ calendarEventList }: { calendarEventList: CalendarEventModel[] }) => {
        return calendarEventList.map((calendar: CalendarEventModel) => {

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
      })
    );
    this.refresh.next();

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

  //for rendar weekends
  beforeMonthViewRender(renderEvent: CalendarMonthViewBeforeRenderEvent): void {
    renderEvent.body.forEach((day) => {
      const dayOfMonth = day.date.getDay();
      if (this.filterDays.includes(dayOfMonth)) {
        day.cssClass = this.cssClass;
      }
    });
  }

  ngOnInit(): void {
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 2);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 3);
    this.editPermission = permissionCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionCategory.rolePermission[0].canDelete;
    this.addPermission = permissionCategory.rolePermission[0].canAdd;

    this.isMarkingPeriod = sessionStorage.getItem('markingPeriod');
    if (this.isMarkingPeriod !== 'null') {
      this.getAllCalendar();
      this.getAllMemberList();
    }
  }

  // open event modal for view
  viewEvent(eventData) {
    this.dialog.open(AddEventComponent, {
      data: { allMembers: this.getAllMembersList, membercount: this.getAllMembersList.getAllMemberList.length, calendarEvent: eventData },
      width: '600px'
    }).afterClosed().subscribe(data => {
      if (data === 'submitedEvent') {
        this.getAllCalendarEvent();
      }
    });

  }
  private formatDate(date: Date): string {
    if (date === undefined || date === null) {
      return undefined;
    } else {
      return moment(date).format('YYYY-MM-DDTHH:mm:ss.SSS');
    }
  }

  //drag and drop event
  eventTimesChanged({ event, newStart, newEnd, }: CalendarEventTimesChangedEvent): void {
    event.start = newStart;
    event.end = newEnd;
    this.calendarEventAddViewModel.schoolCalendarEvent = event.meta.calendar;
    this.calendarEventAddViewModel.schoolCalendarEvent.startDate = this.formatDate(newEnd);
    this.calendarEventAddViewModel.schoolCalendarEvent.endDate = this.formatDate(newStart);
    this.calendarEventService.updateCalendarEvent(this.calendarEventAddViewModel).subscribe(data => {
      if (data._failure) {
        this.snackbar.open('Event dragging failed. ' + data._message, '', {
          duration: 10000
        });
      }
    });
    this.refresh.next();
  }

  //Open modal for add new calendar
  openAddNewCalendar() {
    this.dialog.open(AddCalendarComponent, {
      data: { allMembers: this.getAllMembersList, membercount: this.getAllMembersList.getAllMemberList.length, calendarListCount: this.calendars.length },
      width: '600px'
    }).afterClosed().subscribe(data => {
      if (data === 'submited') {
        this.getAllCalendar();
      }
    });
  }

  // Open calendar confirm modal
  deleteCalendarConfirm(event) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: {
        title: this.defaultValuesService.translateKey('areYouSure'),
        message: this.defaultValuesService.translateKey('youAreAboutToDelete') + event.title
      }
    });
    // listen to response
    dialogRef.afterClosed().subscribe(dialogResult => {
      // if user pressed yes dialogResult will be true, 
      // if user pressed no - it will be false
      if (dialogResult) {
        this.deleteCalendar(event.calenderId);
      }
    });
  }

  deleteCalendar(id: number) {
    this.calendarAddViewModel.schoolCalendar.calenderId = id;
    this.calendarService.deleteCalendar(this.calendarAddViewModel).subscribe(
      (res) => {
        if (res._failure) {
          this.snackbar.open(res._message, '', {
            duration: 10000
          });
        } else {
          this.snackbar.open(res._message, '', {
            duration: 10000
          });
          this.getAllCalendar();
        }
      });

  };

  // Edit calendar which is selected in dropdown
  openEditCalendar(event) {
    this.dialog.open(AddCalendarComponent, {
      data: { allMembers: this.getAllMembersList, membercount: this.getAllMembersList.getAllMemberList.length, calendar: event },
      width: '600px'
    }).afterClosed().subscribe(data => {
      if (data === 'submited') {
        this.getAllCalendar();
      }
    });
  }

  // Open add new event by clicking calendar day
  openAddNewEvent(event) {
    if (this.addPermission){
      if (!event.isWeekend && event.inMonth) {
        this.dialog.open(AddEventComponent, {
          data: { allMembers: this.getAllMembersList, membercount: this.getAllMembersList.getAllMemberList.length, day: event },
          width: '600px'
        }).afterClosed().subscribe(data => {
          if (data === 'submitedEvent') {
            this.getAllCalendarEvent();
          }
        });
      }
      else {
        if (event.isWeekend) {
          this.snackbar.open(this.defaultValuesService.translateKey('cannotAddEventInWeekend'), '', {
            duration: 2000
          });
        }
        if (!event.isWeekend) {
          this.snackbar.open(this.defaultValuesService.translateKey('cannotAddEventInPreviousMonth'), '', {
            duration: 2000
          });
        }
      }
    }
    else{
      this.snackbar.open(this.defaultValuesService.translateKey('HaveNotAnyPermissionToAdd'), '', {
        duration: 2000
      });
    }
    
  }
}
