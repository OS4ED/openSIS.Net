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
import { CalendarAddViewModel, CalendarListModel, CalendarModel } from '../../../models/calendarModel';
import { GetAllMembersList } from '../../../models/membershipModel';
import { MembershipService } from '../../../services/membership.service';
import { CalendarEventService } from '../../../services/calendar-event.service';
import { CalendarEventAddViewModel, CalendarEventListViewModel, CalendarEventModel } from '../../../models/calendarEventModel';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmDialogComponent } from '../../shared-module/confirm-dialog/confirm-dialog.component';
import * as moment from 'moment';
import { LayoutService } from 'src/@vex/services/layout.service';
import { LoaderService } from '../../../services/loader.service';
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
  constructor(private http: HttpClient,
    private dialog: MatDialog,
    private snackbar: MatSnackBar,
    public translate: TranslateService,
    private membershipService: MembershipService,
    private calendarEventService: CalendarEventService,
    private calendarService: CalendarService,
    private layoutService: LayoutService,
    private loaderService: LoaderService,
    private cdr: ChangeDetectorRef,) {
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
    var allDays = [0, 1, 2, 3, 4, 5, 6];
    var splitDays = calendarDays.split('').map(x => +x);
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
    this.isMarkingPeriod = sessionStorage.getItem("markingPeriod");
    if (this.isMarkingPeriod != "null") {
      this.getAllCalendar();
      this.getAllMemberList();
    }
  }

  //open event modal for view
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
        title: "Are you sure?",
        message: "You are about to delete " + event.title
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
          this.snackbar.open('Calendar Deletion failed. ' + res._message, '', {
            duration: 10000
          });
        } else {
          this.snackbar.open('Calendar Deleted Successfully. ', '', {
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
        this.snackbar.open('Cannot add event in weekend', '', {
          duration: 2000
        });
      }
      if (!event.isWeekend) {
        this.snackbar.open('Cannot add event in previous month', '', {
          duration: 2000
        });
      }

    }
  }
}
