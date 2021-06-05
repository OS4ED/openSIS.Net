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

import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable, Subject } from 'rxjs';
import * as moment from 'moment';
import { CalendarEvent, CalendarEventAction, CalendarEventTimesChangedEvent, CalendarMonthViewBeforeRenderEvent, CalendarMonthViewDay, CalendarView, DAYS_OF_WEEK } from 'angular-calendar';
import icClose from '@iconify/icons-ic/twotone-close';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icDelete from '@iconify/icons-ic/twotone-delete';
import icAdd from '@iconify/icons-ic/twotone-add';
import icList from '@iconify/icons-ic/twotone-list-alt';
import icInfo from '@iconify/icons-ic/info';
import icRemove from '@iconify/icons-ic/remove-circle';
import icBack from '@iconify/icons-ic/baseline-arrow-back';
import icExpand from '@iconify/icons-ic/outline-add-box';
import icCollapse from '@iconify/icons-ic/outline-indeterminate-check-box';
import icPlusCircle from '@iconify/icons-ic/add-circle-outline';
import icLeftArrow from '@iconify/icons-ic/baseline-west';
import icRightArrow from '@iconify/icons-ic/baseline-east';
import { Router } from '@angular/router';
import {
  startOfDay, endOfDay, subDays,
  addDays, endOfMonth,
  isSameDay, isSameMonth, addHours
} from 'date-fns';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { BlockedSchedulingCourseSectionAddModel, CalendarSchedulingCourseSectionAddModel, CourseCalendarSchedule, CourseSectionAddViewModel, DeleteCourseSectionSchedule, OutputEmitDataFormat } from '../../../../../models/course-section.model';
import { SchoolPeriodService } from '../../../../../services/school-period.service';
import { BlockListViewModel } from '../../../../../models/school-period.model';
import { RoomListViewModel } from '../../../../../models/room.model';
import { RoomService } from '../../../../../services/room.service';
import { CalendarAddViewModel, CalendarModel } from '../../../../../models/calendar.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CourseSectionService } from '../../../../../services/course-section.service';
import { SharedFunction } from '../../../../shared/shared-function';

@Component({
  selector: 'vex-calendar-days',
  templateUrl: './calendar-days.component.html',
  styleUrls: ['./calendar-days.component.scss'],
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
export class CalendarDaysComponent implements OnInit, OnChanges {
  icClose = icClose;
  icEdit = icEdit;
  icDelete = icDelete;
  icAdd = icAdd;
  icList = icList;
  icInfo = icInfo;
  icRemove = icRemove;
  icBack = icBack;
  icExpand = icExpand;
  icCollapse = icCollapse;
  icPlusCircle = icPlusCircle;
  icLeftArrow = icLeftArrow;
  icRightArrow = icRightArrow;
  useStandardGrades = false;
  isChecked = false;
  scheduleType = '1';
  durationType = '1';
  @Input() selectedCalendar;
  @ViewChild('f') currentForm: NgForm;
  addCalendarDay = 0;
  selectedDate;
  blockListViewModel: BlockListViewModel = new BlockListViewModel();
  roomListViewModel: RoomListViewModel = new RoomListViewModel();
  calendarAddViewModel: CalendarAddViewModel = new CalendarAddViewModel();
  calendarSchedulingModel: CalendarSchedulingCourseSectionAddModel = new CalendarSchedulingCourseSectionAddModel();
  weekendDays: number[];
  changes = new Subject<SimpleChanges>();
  filterDays = [];
  periodList = [];
  periodTitle = [];
  roomModelList = [];
  submitTitle = 'submit';
  addClassTitle = 'addClass';
  startDate: string;
  endDate: string;
  refresh: Subject<any> = new Subject();
  blockedSchedulingCourseSectionAddModel: BlockedSchedulingCourseSectionAddModel = new BlockedSchedulingCourseSectionAddModel();
  @Input() courseCalendarScheduleList;
  @Input() detailsFromParentModal;
  @Input() selectedMarkingPeriod;
  @Input() durationDateRange;
  @Output() calendarScheduleData = new EventEmitter<OutputEmitDataFormat>()
  courseCalendarSchedule: CourseCalendarSchedule = new CourseCalendarSchedule()
  view: CalendarView = CalendarView.Month;
  viewDate: Date = new Date();
  classEditMode: boolean = false;
  events: CalendarEvent[] = [];
  classDetails;
  showClassDetails: boolean = false;
  editClassDetails: boolean = false;
  cssClass: string;
  classRoomName: string;
  classPeriodName: string;
  classtakeAttendance: string;
  classdate: string;
  color = ['bg-deep-orange', 'bg-red', 'bg-green', 'bg-teal', 'bg-cyan', 'bg-deep-purple', 'bg-pink', 'bg-blue'];
  showError: boolean = false;
  showMarkingPeriodError: boolean = false;
  constructor(
    private fb: FormBuilder,
    private schoolPeriodService: SchoolPeriodService,
    private roomService: RoomService,
    private courseSectionService: CourseSectionService,
    private commonFunction: SharedFunction,
    private snackbar: MatSnackBar,
  ) {
    this.courseSectionService.currentUpdate.subscribe((res) => {
      if (res) {

        this.sendCalendarScheduleDataToParent();
      }
    })
  }




  ngOnChanges(changes: SimpleChanges): void {
    this.checkDurationChanges();
    this.checkCalendarChanges();
  }


  checkDurationChanges() {
      if(this.selectedMarkingPeriod){
        this.startDate = this.selectedMarkingPeriod.startDate;
        this.endDate = this.selectedMarkingPeriod.endDate;
        this.showMarkingPeriodError = false;
        this.viewDate = new Date(this.startDate);
      }
      else if (this.durationDateRange.startDate && this.durationDateRange.endDate) {
        this.startDate = this.durationDateRange.startDate;
        this.endDate = this.durationDateRange.endDate;
        this.showMarkingPeriodError = false;
        this.viewDate = new Date(this.startDate);
      }
      else {
        this.showMarkingPeriodError = true;
      }

  }
  ngOnInit(): void {
    this.courseCalendarSchedule.takeAttendance = false;
    this.checkCalendarChanges();
    this.checkDurationChanges();
    if (this.detailsFromParentModal.editMode) {
      this.patchFormValue();
    }

    this.getAllPeriodList();

    this.getAllRooms();
  }


  checkCalendarChanges() {
    if (this.selectedCalendar == undefined || this.selectedCalendar?.days == undefined) {
      this.showError = true;
    }
    else {
      this.showError = false;
      this.getDays(this.selectedCalendar.days);
      this.refresh.next()
    }
  }

  patchFormValue() {
    this.calendarSchedulingModel.courseCalendarScheduleList = this.detailsFromParentModal.courseSectionDetails.courseCalendarSchedule;
    for (let schedule of this.calendarSchedulingModel.courseCalendarScheduleList) {

      this.courseCalendarSchedule.blockId = schedule.blockId;
      this.courseCalendarSchedule.courseId = schedule.courseId;
      this.courseCalendarSchedule.courseSectionId = schedule.courseSectionId;
      this.courseCalendarSchedule.periodId = schedule.periodId;
      this.courseCalendarSchedule.roomId = schedule.roomId;
      this.courseCalendarSchedule.serial = schedule.serial;
      this.courseCalendarSchedule.takeAttendance = schedule.takeAttendance;
      let random = Math.floor((Math.random() * 7) + 0);
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
        draggable: true,
        meta: { scheduleDetails: schedule, randomColor: this.color[random] }
      });
      this.refresh.next();
    }
  }

  openAddNewEvent(event) {
    this.courseCalendarSchedule = new CourseCalendarSchedule();
    if (event.isWeekend) {
      this.snackbar.open('Cannot add class. ', '', {
        duration: 1000
      });
    }
    else {
      document.getElementById('calendarDays').scrollIntoView();
      this.addClassTitle = 'addClass';
      this.classEditMode = false;
      this.showClassDetails = false;
      this.editClassDetails = true;
      this.addCalendarDay = 1;
      this.selectedDate = event.date;
      this.courseCalendarSchedule.date = event.date;
    }

  }
  viewEvent(event) {
    document.getElementById('calendarDays').scrollIntoView();
    this.addClassTitle = 'editClass';
    this.showClassDetails = true;
    this.editClassDetails = false;
    this.classDetails = event;
    this.classPeriodName = event.title;
    this.classRoomName = event.meta.scheduleDetails.rooms.title;
    this.classdate = event.meta.scheduleDetails.date;
    this.classtakeAttendance = event.meta.scheduleDetails.takeAttendance ? 'Yes' : 'No';
    this.addCalendarDay = 1;

  }
  deleteEvent(event) {
    this.addCalendarDay = 0;
    if(event.meta.scheduleDetails.serial >0 ){
      let classIndex = this.calendarSchedulingModel.courseCalendarScheduleList.findIndex(x => x.serial == event.meta.scheduleDetails.serial);
      this.calendarSchedulingModel.courseCalendarScheduleList.splice(classIndex, 1);
      let eventIndex = this.events.findIndex(x => x.meta.scheduleDetails.serial == event.meta.scheduleDetails.serial);
      this.events.splice(eventIndex, 1);
      this.refresh.next();
    }
    else{
      let classIndex = this.calendarSchedulingModel.courseCalendarScheduleList.findIndex(x => x.serial == event.meta.scheduleDetails.serial);
      this.calendarSchedulingModel.courseCalendarScheduleList.splice(classIndex, 1);
      let eventIndex = this.events.findIndex(x => x.meta.scheduleDetails.serial == event.meta.scheduleDetails.serial);
      this.events.splice(eventIndex, 1);
      this.refresh.next();
    }
    
  }

  closeDetails() {
    this.addCalendarDay = 0;
  }
  editEvent(event) {
    this.showClassDetails = false;
    this.editClassDetails = true;
    this.classEditMode = true;
    this.submitTitle = 'update';
    this.addCalendarDay = 1;
    this.courseCalendarSchedule = event.meta.scheduleDetails;

  }

  onPeriodChange(event) {
    let index = this.blockListViewModel.getBlockListForView[0].blockPeriod.findIndex(item => item.periodId == +event);
    this.periodTitle.push(this.blockListViewModel.getBlockListForView[0].blockPeriod[index].periodTitle);
  }
  onRoomChange(event) {
    this.roomModelList.map((x) => {
      if (x.roomId === +event) {
        if (this.detailsFromParentModal.form.controls.seats.value !== "") {
          if (this.detailsFromParentModal.form.controls.seats.value > x.capacity) {
            this.currentForm.form.controls.roomId.setErrors({ 'nomatch': true });
          }
        } else {
          this.currentForm.form.controls.roomId.setErrors({ 'nomatch': true });
        }
      }
    })
  }


  setDuration(mrChange) {
    this.durationType = mrChange.value;
  }
  classSubmit() {
    let random = Math.floor((Math.random() * 7) + 0);
    this.currentForm.form.markAllAsTouched();
    this.courseCalendarSchedule.blockId = this.blockListViewModel.getBlockListForView[0].blockId;
    this.courseCalendarSchedule.date = this.commonFunction.formatDateSaveWithoutTime(this.courseCalendarSchedule.date);
    if (this.classEditMode) {

      if (this.courseCalendarSchedule.serial === 0) {
        let eventIndex = this.events.findIndex(x => x.meta.scheduleDetails.serial == this.courseCalendarSchedule.serial);
        this.events.splice(eventIndex, 1);
      }
      else {
        let classIndex = this.calendarSchedulingModel.courseCalendarScheduleList.findIndex(x => x.serial == this.courseCalendarSchedule.serial);
        this.calendarSchedulingModel.courseCalendarScheduleList.splice(classIndex, 1);
        this.calendarSchedulingModel.courseCalendarScheduleList.push(this.courseCalendarSchedule);
        let eventIndex = this.events.findIndex(x => x.meta.scheduleDetails.serial == this.courseCalendarSchedule.serial);
        this.events.splice(eventIndex, 1);
      }

    }
    else {
      let roomIndex = this.roomModelList.findIndex(x => x.roomId == this.courseCalendarSchedule.roomId);
      let roomTitle = this.roomModelList[roomIndex].title;
      this.courseCalendarSchedule.rooms.title = roomTitle;
      this.courseCalendarSchedule.serial = 0;
      this.calendarSchedulingModel.courseCalendarScheduleList.push(this.courseCalendarSchedule);
    }


    if (this.currentForm.form.valid) {

      if (this.periodTitle.length === 0) {
        let index = this.blockListViewModel.getBlockListForView[0].blockPeriod.findIndex(item => item.periodId == this.courseCalendarSchedule.periodId);
        this.periodTitle.push(this.blockListViewModel.getBlockListForView[0].blockPeriod[index].periodTitle);
      }
      this.events.push({
        start: new Date(this.courseCalendarSchedule.date),
        end: new Date(this.courseCalendarSchedule.date),
        title: this.periodTitle[this.periodTitle.length - 1],
        color: null,
        actions: null,
        allDay: this.courseCalendarSchedule.takeAttendance,
        resizable: {
          beforeStart: true,
          afterEnd: true,
        },
        draggable: true,
        meta: { scheduleDetails: this.courseCalendarSchedule, randomColor: this.color[random] }
      });
      this.refresh.next();
      this.addCalendarDay = 0;
    }

  }

  sendCalendarScheduleDataToParent() {
    if (this.calendarSchedulingModel.courseCalendarScheduleList.length > 0) {
      this.calendarScheduleData.emit({ scheduleType: 'calendarschedule', roomList: this.roomModelList, scheduleDetails: this.calendarSchedulingModel.courseCalendarScheduleList, error: false });
    } else {

      this.calendarScheduleData.emit({ scheduleType: 'calendarschedule', roomList: null, scheduleDetails: null, error: true });
      this.snackbar.open('Please add minimum one class', '', {
        duration: 10000
      });
    }
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
      if (day.date >= new Date(this.startDate) && day.date <= new Date(this.endDate)) {
        const dayOfMonth = day.date.getDay();
        if (this.filterDays.includes(dayOfMonth)) {
          day.cssClass = this.cssClass;
        }
      }
      else {
        day.isWeekend = true;
        day.cssClass = this.cssClass;
      }

    });
  }

  getAllPeriodList() {
    this.schoolPeriodService.getAllBlockList(this.blockListViewModel).subscribe(data => {
      if (data._failure) {
        if (data._message === "NO RECORD FOUND") {
          this.periodList = [];
        }
      } else {
        this.blockListViewModel = data;
        if (data.getBlockListForView.length > 0) {
          this.periodList = data.getBlockListForView[0].blockPeriod;
        }

      }
    });
  }

  getAllRooms() {
    this.roomService.getAllRoom(this.roomListViewModel).subscribe(
      (res: RoomListViewModel) => {
        if (typeof (res) == 'undefined') {
        }
        else {
          if (res._failure) {
          }
          else {
            this.roomModelList = res.tableroomList;
          }
        }
      })
  }

  private formatDate(date: Date): string {
    if (date === undefined || date === null) {
      return undefined;
    } else {
      return moment(date).format('YYYY-MM-DDTHH:mm:ss.SSS');
    }
  }



  cancelAddClass() {
    this.addCalendarDay = 0;
  }

}
