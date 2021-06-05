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

import { Component, OnDestroy, OnInit } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { MatSnackBar } from "@angular/material/snack-bar";
import { TranslateService } from "@ngx-translate/core";
import {
  CalendarEvent,
  CalendarView,
} from "angular-calendar";
import { Subject } from "rxjs";
import { takeUntil } from "rxjs/operators";
import {
  AddUpdateStudentAttendanceModelFor360,
  StudentAttendanceModelFor360,
} from "src/app/models/take-attendance-list.model";
import { ConfirmDialogComponent } from "src/app/pages/shared-module/confirm-dialog/confirm-dialog.component";
import { StudentAttendanceService } from "src/app/services/student-attendance.service";
import { DefaultValuesService } from "../../../../common/default-values.service";
import { days } from "../../../../common/static-data";
import {
  AttendanceWeekViewModel,
  ScheduledCourseSectionListForStudent360Model,
  WeekEventsModel,
} from "../../../../models/student-schedule.model";
import { StudentScheduleService } from "../../../../services/student-schedule.service";
import { StudentService } from "../../../../services/student.service";

@Component({
  selector: "vex-student-attendance",
  templateUrl: "./student-attendance.component.html",
  styleUrls: ["./student-attendance.component.scss"],
})
export class StudentAttendanceComponent implements OnInit, OnDestroy {
  viewDate: Date = new Date();
  view: CalendarView | string = "weekView";
  weekEvents: WeekEventsModel[] = [];
  CalendarView = CalendarView;
  events: CalendarEvent[] = [];

  scheduledCourseSectionList: ScheduledCourseSectionListForStudent360Model =
    new ScheduledCourseSectionListForStudent360Model();
  todayDate = new Date().toISOString().split("T")[0];
  currentWeek = [];
  days = days;
  initialRoutineDate: Date;
  endRoutingDate: Date;
  weeklyAttendanceView: AttendanceWeekViewModel = new AttendanceWeekViewModel();
  refresh: Subject<any> = new Subject();
  addUpdateStudentAttendanceModel360: AddUpdateStudentAttendanceModelFor360 =
    new AddUpdateStudentAttendanceModelFor360();
  destroySubject$: Subject<void> = new Subject();

  constructor(
    public translateService: TranslateService,
    private studentScheduledService: StudentScheduleService,
    private studentService: StudentService,
    private dialog: MatDialog,
    private studentAttendanceService: StudentAttendanceService,
    private defaultService: DefaultValuesService,
    private snackbar: MatSnackBar
  ) {
    translateService.use("en");
    this.renderAttendanceWeekCalendar();
  }

  ngOnInit(): void {
    this.getAllScheduledCourseSectionList();
  }

  changeCalendarView(calendarType) {
    this.view = calendarType;
    if (this.view === CalendarView.Month) {
      const currentDate = new Date(this.viewDate);
      const startDate = new Date(
        currentDate.getFullYear(),
        currentDate.getMonth() - 1,
        22
      );
      const endDate = new Date(
        currentDate.getFullYear(),
        currentDate.getMonth() + 1,
        8
      );
      if (!this.events.length) {
        this.getAllScheduledCourseSectionListForMonth(startDate, endDate);
      }
    }
  }

  getAllScheduledCourseSectionList() {
    this.scheduledCourseSectionList.durationStartDate = this.currentWeek[0]
      ?.toISOString()
      .split("T")[0];
    this.scheduledCourseSectionList.durationEndDate = this.currentWeek[6]
      ?.toISOString()
      .split("T")[0];
    this.scheduledCourseSectionList.studentId =
      this.studentService.getStudentId();
    this.studentScheduledService
      .scheduleCourseSectionListForStudent360(this.scheduledCourseSectionList)
      .pipe(takeUntil(this.destroySubject$))
      .subscribe((res) => {
        if (res) {
          if (res._failure) {
            if (res.scheduleCourseSectionForView?.length === 0) {
            } else {
              this.snackbar.open(res._message, "", {
                duration: 10000,
              });
            }
          } else {
            this.scheduledCourseSectionList = res;
            this.createDataSet();
          }
        } else {
          this.snackbar.open(sessionStorage.getItem("httpError"), "", {
            duration: 10000,
          });
        }
      });
  }

  getAllScheduledCourseSectionListForMonth(startDate, endDate) {
    let scheduledCourseSectionListForMonth: ScheduledCourseSectionListForStudent360Model =
      new ScheduledCourseSectionListForStudent360Model();
    scheduledCourseSectionListForMonth.durationStartDate = startDate;
    scheduledCourseSectionListForMonth.durationEndDate = endDate;
    scheduledCourseSectionListForMonth.studentId =
      this.studentService.getStudentId();
    this.studentScheduledService
      .scheduleCourseSectionListForStudent360(
        scheduledCourseSectionListForMonth
      ).pipe(takeUntil(this.destroySubject$))
      .subscribe((res) => {
        if (res) {
          if (res._failure) {
            if (res.scheduleCourseSectionForView?.length === 0) {
            } else {
              this.snackbar.open(res._message, "", {
                duration: 10000,
              });
            }
          } else {
            scheduledCourseSectionListForMonth = res;
            this.extractAttendanceListForMonthView(res);
          }
        } else {
          this.snackbar.open(sessionStorage.getItem("httpError"), "", {
            duration: 10000,
          });
        }
      });
  }

  extractAttendanceListForMonthView(
    res: ScheduledCourseSectionListForStudent360Model
  ) {
    this.events = [];
    res.scheduleCourseSectionForView?.map((item: any) => {
      item.studentAttendanceList?.map((attendance) => {
        this.events.push({
          title: "",
          start: new Date(attendance.attendanceDate),
          meta: {
            bgColor: this.findColor(item, attendance),
          },
        });
      });
    });
    this.refresh.next();
  }

  findColor(item, attendance) {
    let stateCode;
    item?.attendanceCodeCategories.attendanceCode?.map((listItem) => {
      if (
        listItem.attendanceCategoryId === attendance.attendanceCategoryId &&
        listItem.attendanceCode1 === attendance.attendanceCode
      ) {
        stateCode = listItem.stateCode;
      }
    });
    let colorName;
    if (stateCode) {
      stateCode === "Present"
        ? (colorName = "bg-green")
        : stateCode === "Absent"
          ? (colorName = "bg-red")
          : (colorName = "bg-yellow");
    }
    return colorName;
  }

  changeMonthView(direction) {
    const currentDate = new Date(this.viewDate);
    let startDate;
    let endDate;
    if (direction === "prev") {
      startDate = new Date(
        currentDate.getFullYear(),
        currentDate.getMonth() - 1,
        22
      );
      endDate = new Date(
        currentDate.getFullYear(),
        currentDate.getMonth() + 1,
        8
      );
    } else if (direction === "next") {
      startDate = new Date(
        currentDate.getFullYear(),
        currentDate.getMonth(),
        22
      );
      endDate = new Date(
        currentDate.getFullYear(),
        currentDate.getMonth() + 2,
        8
      );
    }
    this.getAllScheduledCourseSectionListForMonth(startDate, endDate);
  }

  createDataSet() {
    this.weekEvents = [];
    this.scheduledCourseSectionList.scheduleCourseSectionForView.map(
      (item: any) => {
        if (item.courseFixedSchedule) {
          this.createDatasetForFixedSchedule(item);
        } else if (item.courseVariableScheduleList?.length > 0) {
          this.createDatasetForVariableSchedule(item);
        } else if (item.courseCalendarScheduleList?.length > 0) {
          this.createDatasetForCalendarSchedule(item);
        }
      }
    );
    this.filterWeekEventsBasedOnPeriod();
  }

  createDatasetForFixedSchedule(item) {
    if (!item.attendanceTaken) {
      return;
    }
    item.dayOfWeek?.split("|").map((day) => {
      this.weekEvents.push({
        courseId: item.courseId,
        courseSectionId: item.courseSectionId,
        takeAttendance: item.attendanceTaken,
        blockId: item.courseFixedSchedule.blockPeriod.blockId,
        periodId: item.courseFixedSchedule.blockPeriod.periodId,
        periodTitle: item.courseFixedSchedule.blockPeriod.periodTitle,
        day: this.getDayInNumberFromDayName(day),
        takenAttendanceList: item.studentAttendanceList,
        attendanceList: item.attendanceCodeCategories,
      });
    });
  }

  createDatasetForVariableSchedule(item) {
    item.courseVariableScheduleList?.map((day) => {
      if (day.takeAttendance) {
        this.weekEvents.push({
          courseId: item.courseId,
          courseSectionId: item.courseSectionId,
          takeAttendance: day.takeAttendance,
          blockId: day.blockPeriod.blockId,
          periodId: day.blockPeriod.periodId,
          periodTitle: day.blockPeriod.periodTitle,
          day: this.getDayInNumberFromDayName(day.day),
          takenAttendanceList: item.studentAttendanceList,
          attendanceList: item.attendanceCodeCategories,
        });
      }
    });
  }

  createDatasetForCalendarSchedule(item) {
    item.courseCalendarScheduleList?.map((day) => {
      if (day.takeAttendance) {
        const dayFromDate = new Date(day.date).getDay();
        this.weekEvents.push({
          courseId: item.courseId,
          courseSectionId: item.courseSectionId,
          takeAttendance: day.takeAttendance,
          blockId: day.blockPeriod.blockId,
          periodId: day.blockPeriod.periodId,
          periodTitle: day.blockPeriod.periodTitle,
          takenAttendanceList: item.studentAttendanceList,
          day: dayFromDate,
          attendanceList: item.attendanceCodeCategories,
        });
      }
    });
  }

  filterWeekEventsBasedOnPeriod() {
    this.weeklyAttendanceView.attendanceWeekView = [];
    this.weekEvents?.map((item) => {
      let isSameBlockPeriodFound = false;
      for (const eachDay of this.weeklyAttendanceView.attendanceWeekView) {
        if (
          eachDay.blockId === item.blockId &&
          eachDay.periodId === item.periodId
        ) {
          eachDay.days = eachDay.days
            ? eachDay.days + "|" + item.day
            : item.day.toString();
          isSameBlockPeriodFound = true;
          break;
        }
      }
      if (!isSameBlockPeriodFound) {
        this.weeklyAttendanceView.attendanceWeekView.push({
          blockId: item.blockId,
          periodId: item.periodId,
          attendanceTaken: false,
          periodTitle: item.periodTitle,
          courseId: item.courseId,
          courseSectionId: item.courseSectionId,
          days: item.day.toString(),
          cloneTakenAttendanceDays: this.findTakenDays(
            item.takenAttendanceList
          ),
          takenAttendanceDays: this.generateDayFromDate(
            item.takenAttendanceList
          ),
          takenAttendanceList: item.takenAttendanceList,
          attendanceList: item.attendanceList,
        });
      }
    });
  }

  findTakenDays(takenDaysList) {
    const takenDays = [null, null, null, null, null, null, null];
    takenDaysList?.map((day) => {
      takenDays[new Date(day.attendanceDate).getDay()] = day.attendanceCode;
    });
    return takenDays;
  }

  getDayInNumberFromDayName(dayName: string) {
    let index = null;
    days.map((day, i) => {
      if (day.toLowerCase() === dayName.toLowerCase()) {
        index = i;
      }
    });
    return index;
  }

  generateDayFromDate(takenAttendanceList) {
    let days;
    takenAttendanceList?.map((takenDay) => {
      const day = new Date(takenDay.attendanceDate).getDay();
      days = days ? days + "|" + day : day.toString();
    });
    return days;
  }

  renderAttendanceWeekCalendar() {
    const today = new Date(this.todayDate);
    const cloneToday = today;
    for (let i = 0; i < 7; i++) {
      if (today.getDay() === 0) {
        this.initialRoutineDate = today;
        break;
      } else {
        cloneToday.setDate(today.getDate() - 1);
        if (cloneToday.getDay() === 0) {
          this.initialRoutineDate = cloneToday;
          break;
        }
      }
    }

    const cloneInitialDate = new Date(this.initialRoutineDate);
    this.endRoutingDate = new Date(
      cloneInitialDate.setDate(today.getDate() + 6)
    );

    for (
      const d = new Date(this.initialRoutineDate);
      d <= this.endRoutingDate;
      d.setDate(d.getDate() + 1)
    ) {
      this.currentWeek.push(new Date(d));
    }
  }

  confirmWeekChange(direction: string) {
    if (this.addUpdateStudentAttendanceModel360.studentAttendance?.length === 0) {
      this.changeWeek(direction);
      return;
    }
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: {
        title: this.defaultService.translateKey("areYouSure"),
        message: this.defaultService.translateKey("youWillLoseTheSelectedAttendanceDoYouWantToContinue"),
        fromAttendance: true
      },
    });
    dialogRef.afterClosed().subscribe((dialogResult) => {
      if (dialogResult) {
        this.addUpdateStudentAttendanceModel360.studentAttendance = [];
        this.changeWeek(direction);
      }
    });
  }

  changeWeek(direction) {
    if (direction === 'prev') {
      this.previousWeek();
    } else if (direction === 'next') {
      this.nextWeek();
    }
  }

  previousWeek() {
    const initialDate = new Date(this.initialRoutineDate);
    let count = 0;
    for (
      const d = new Date(initialDate.setDate(initialDate.getDate() - 7));
      d < this.initialRoutineDate;
      d.setDate(d.getDate() + 1)
    ) {
      this.currentWeek[count] = new Date(d);
      count++;
    }
    this.initialRoutineDate = this.currentWeek[0];
    this.endRoutingDate = this.currentWeek[this.currentWeek.length - 1];
    this.getAllScheduledCourseSectionList();
  }

  nextWeek() {
    const endDate = new Date(this.endRoutingDate);
    for (let i = 0; i < 7; i++) {
      this.currentWeek[i] = new Date(endDate.setDate(endDate.getDate() + 1));
    }
    this.initialRoutineDate = this.currentWeek[0];
    this.endRoutingDate = this.currentWeek[this.currentWeek.length - 1];
    this.getAllScheduledCourseSectionList();
  }

  onAttendanceChange(attendanceCode, periodDetails, day) {
    let attendanceCategoryId;
    let staffId;

    for (const takenDay of periodDetails.takenAttendanceList) {
      if (new Date(takenDay.attendanceDate).getDay() == day) {
        staffId = takenDay.staffId;
        break;
      }
    }
    periodDetails?.attendanceList.attendanceCode?.map((item) => {
      if (!attendanceCategoryId) {
        if (item.attendanceCode1 === +attendanceCode) {
          attendanceCategoryId = item.attendanceCategoryId;
        }
      }
    });
    const index =
      this.addUpdateStudentAttendanceModel360.studentAttendance.findIndex(
        (item) => {
          return (
            item.courseId === periodDetails.courseId &&
            item.courseSectionId === periodDetails.courseSectionId &&
            item.blockId === periodDetails.blockId &&
            item.periodId === periodDetails.periodId
          );
        }
      );
    if (index !== -1) {
      this.addUpdateStudentAttendanceModel360.studentAttendance[index]
        = this.studentAttendanceDataSetForUpdate(periodDetails, day, attendanceCategoryId, attendanceCode, staffId);
    } else {
      this.addUpdateStudentAttendanceModel360.studentAttendance
      .push(this.studentAttendanceDataSetForUpdate(periodDetails, day, attendanceCategoryId, attendanceCode, staffId));
    }

  }

  studentAttendanceDataSetForUpdate(periodDetails, day, attendanceCategoryId, attendanceCode, staffId) {
    const studentAttendanceFor360: StudentAttendanceModelFor360 = {
      studentId: this.studentService.getStudentId(),
      schoolId: this.defaultService.getSchoolID(),
      courseId: periodDetails.courseId,
      courseSectionId: periodDetails.courseSectionId,
      attendanceCategoryId,
      attendanceCode,
      staffId,
      attendanceDate: new Date(this.currentWeek[day])
        .toISOString()
        .split("T")[0],
      blockId: periodDetails.blockId,
      periodId: periodDetails.periodId,
      updatedBy: this.defaultService.getUserName(),
    }
    return studentAttendanceFor360;
  }


  onAttendanceUpdate() {
    this.addUpdateStudentAttendanceModel360.studentId =
      this.studentService.getStudentId();
    if (
      this.addUpdateStudentAttendanceModel360.studentAttendance?.length === 0
    ) {
      this.snackbar.open("Nothing to Update", "Ok", {
        duration: 3000,
      });
      return;
    }
    this.studentAttendanceService
      .addUpdateStudentAttendanceForStudent360(
        this.addUpdateStudentAttendanceModel360
      ).pipe(takeUntil(this.destroySubject$))
      .subscribe((res) => {
        if (res) {
          if (res._failure) {
            this.snackbar.open(res._message, "", {
              duration: 10000,
            });
          } else {
            this.snackbar.open(res._message, "", {
              duration: 10000,
            });
            this.addUpdateStudentAttendanceModel360.studentAttendance = [];
          }
        } else {
          this.snackbar.open(sessionStorage.getItem("httpError"), "", {
            duration: 10000,
          });
        }
      });
  }



  ngOnDestroy() {
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }
}
