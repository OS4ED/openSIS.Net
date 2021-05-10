import { Component, OnInit, ViewEncapsulation } from "@angular/core";
import icEdit from "@iconify/icons-ic/twotone-edit";
import icDelete from "@iconify/icons-ic/twotone-delete";
import icMoreVert from "@iconify/icons-ic/more-vert";
import icInfo from "@iconify/icons-ic/twotone-info";
import icPendingActions from "@iconify/icons-ic/twotone-pending-actions";
import icCalendarToday from "@iconify/icons-ic/twotone-calendar-today";
import { MatDialog } from "@angular/material/dialog";
import { TranslateService } from "@ngx-translate/core";
import { AddAssignCourseComponent } from "./add-assign-course/add-assign-course.component";

import { CalendarEvent, CalendarView } from "angular-calendar";
import { StudentScheduleService } from "../../../../services/student-schedule.service";
import {
  RoutineView,
  RoutineViewEvent,
  RoutineViewModel,
  ScheduleCourseSectionForViewModel,
  ScheduleCoursesForStudent360Model,
  ScheduledStudentDropModel,
  StudentCoursesectionSchedule,
} from "../../../../models/student-schedule.model";
import { MatSnackBar } from "@angular/material/snack-bar";
import { StudentService } from "../../../../services/student.service";
import { GetMarkingPeriodTitleListModel } from "../../../../models/marking-period.model";
import { MarkingPeriodService } from "../../../../services/marking-period.service";
import { DefaultValuesService } from "../../../../common/default-values.service";
import { days, uniqueColors } from "../../../../common/static-data";
import { Transform24to12Pipe } from "../../../shared-module/user-define-pipe/transform-24to12.pipe";
import { SharedFunction } from "../../../shared/shared-function";
import { ConfirmDialogComponent } from "../../../shared-module/confirm-dialog/confirm-dialog.component";
import { AddDaysScheduleComponent } from "../../../scheduling/schedule-teacher/add-days-schedule/add-days-schedule.component";
import { ExcelService } from "../../../../services/excel.service";
import { format } from 'date-fns'
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
  icInfo = icInfo;
  icPendingActions = icPendingActions;
  icCalendarToday = icCalendarToday;

  uniqueColors = uniqueColors;
  view: CalendarView | string = CalendarView.Week;
  viewDate: Date = new Date();
  events: CalendarEvent[] = [];
  activeDayIsOpen: boolean = true;
  CalendarView = CalendarView;
  scheduleSwitch = true;
  scheduleCoursesForStudent360Model: ScheduleCoursesForStudent360Model = new ScheduleCoursesForStudent360Model();
  showDropCourse: boolean = false;
  getMarkingPeriodTitleListModel: GetMarkingPeriodTitleListModel = new GetMarkingPeriodTitleListModel();
  disableUpdateCourseSection = true;
  selectionForDropDateUpdate: ScheduleCourseSectionForViewModel[] = [new ScheduleCourseSectionForViewModel()];
  todayDate = new Date().toISOString().split('T')[0];

  routineViewBasedOn = "0" // 0 for Period base, 1 for Time base
  currentWeek = [];
  days = days;
  initialRoutineDate: Date;
  endRoutingDate: Date;
  routineViewWithEvent: RoutineViewModel = new RoutineViewModel();
  showWeekendsRoutingView = true;
  constructor(
    private dialog: MatDialog,
    public translateService: TranslateService,
    private studentScheduleService: StudentScheduleService,
    private snackbar: MatSnackBar,
    private studentService: StudentService,
    private markingPeriodService: MarkingPeriodService,
    private defaultService: DefaultValuesService,
    public commonFunction: SharedFunction,
    private excelService: ExcelService
  ) {
    translateService.use("en");
  }

  ngOnInit(): void {
    this.getAllMarkingPeriodList().then(() => {
      this.getAllScheduleCoursesForStudent360();
    });
    this.renderRoutineView();
  }

  onDropCourseChange() {
    this.getAllScheduleCoursesForStudent360();
  }

  getAllMarkingPeriodList() {
    return new Promise((resolve, rej) => {
      this.getMarkingPeriodTitleListModel.academicYear = this.defaultService.getAcademicYear();

      this.markingPeriodService
        .getAllMarkingPeriodList(this.getMarkingPeriodTitleListModel)
        .subscribe((data) => {
          if (data._failure) {
            this.getMarkingPeriodTitleListModel.getMarkingPeriodView = [];
          } else {
            this.getMarkingPeriodTitleListModel.getMarkingPeriodView =
              data.getMarkingPeriodView;
            resolve([]);
          }
        });
    })

  }

  getAllScheduleCoursesForStudent360() {
    this.events = [];
    this.uniqueColors = uniqueColors;
    let scheduleCoursesStudent360 = new ScheduleCoursesForStudent360Model();
    scheduleCoursesStudent360.isDropped = this.showDropCourse;
    scheduleCoursesStudent360.studentId = this.studentService.getStudentId();
    this.studentScheduleService
      .scheduleCoursesForStudent360(scheduleCoursesStudent360)
      .subscribe((res) => {
        if (res) {
          if (res._failure) {
            if (res.scheduleCourseSectionForView === null) {
              this.snackbar.open(res._message, "", {
                duration: 10000,
              });
              this.scheduleCoursesForStudent360Model.scheduleCourseSectionForView = [];
            } else {
              this.scheduleCoursesForStudent360Model.scheduleCourseSectionForView = [];
            }
          } else {
            this.scheduleCoursesForStudent360Model = res;
            this.selectionForDropDateUpdate
            = JSON.parse(JSON.stringify(this.scheduleCoursesForStudent360Model.scheduleCourseSectionForView));

            this.scheduleCoursesForStudent360Model.scheduleCourseSectionForView = this.findMarkingPeriodTitleById(
              res.scheduleCourseSectionForView
            );
            this.generateEvents();
          }
        } else {
          this.snackbar.open(sessionStorage.getItem("httpError"), "", {
            duration: 10000,
          });
        }
      });
  }

  selectAssignCourse() {
    this.dialog
      .open(AddAssignCourseComponent, {
        data: {
          markingPeriod: this.getMarkingPeriodTitleListModel
            .getMarkingPeriodView,
        },
        width: "900px",
      })
      .afterClosed()
      .subscribe((res) => {
        if (res) {
          this.getAllScheduleCoursesForStudent360();
        }
      });
  }

  scheduleDetails(courseSectionDetails) {
    this.dialog.open(AddDaysScheduleComponent, {
      data: { scheduleDetails: courseSectionDetails, fromTeacherSchedule: false },
      width: "600px",
    });
  }

  changeCalendarView(calendarType) {
    this.scheduleSwitch = false;
    this.view = calendarType;
  }

  listAll() {
    this.scheduleSwitch = true;
    this.scheduleCoursesForStudent360Model.scheduleCourseSectionForView 
    = this.scheduleCoursesForStudent360Model.scheduleCourseSectionForView.map(
      (item) => {
        item.takeInput = false;
        return item;
      }
    );
    this.disableUpdateCourseSection = true;
  }

  confirmDelete(courseDetails) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: {
        title: this.defaultService.translateKey("areYouSure"),
        message: this.defaultService.translateKey("youAreAboutToDelete") + ".",
      },
    });
    dialogRef.afterClosed().subscribe((dialogResult) => {
      if (dialogResult) {
        this.onScheduleCourseSectionDeletion(courseDetails);
      }
    });
  }

  onScheduleCourseSectionDeletion(courseDetails) {
    let deleteCourseSection: ScheduledStudentDropModel = new ScheduledStudentDropModel();
    deleteCourseSection.courseSectionId = courseDetails.courseSectionId;
    deleteCourseSection.studentId = this.studentService.getStudentId();
    this.studentScheduleService
      .dropScheduledCourseSectionForStudent360(deleteCourseSection)
      .subscribe((res) => {
        if (res) {
          if (res._failure) {
            if (res.studentCoursesectionScheduleList === null) {
              this.snackbar.open(res._message, "", {
                duration: 10000,
              });
            }
          } else {
            this.snackbar.open(res._message, "", {
              duration: 10000,
            });
            this.getAllScheduleCoursesForStudent360();
          }
        } else {
          this.snackbar.open(sessionStorage.getItem("httpError"), "", {
            duration: 10000,
          });
        }
      });
  }

  generateEvents() {
    this.scheduleCoursesForStudent360Model.scheduleCourseSectionForView.forEach(
      (item) => {
        if (item.isDropped) return;
        let uniqueColorClass;
        uniqueColorClass = this.generateUniqueColor(item);
        const effectiveDropDate = new Date(
          new Date(item.effectiveDropDate).setDate(
            new Date(item.effectiveDropDate).getDate() + 1
          )
        );
        for (
          let dt = new Date(item.enrolledDate);
          dt <= effectiveDropDate;
          dt.setDate(dt.getDate() + 1)
        ) {
          const formatedDate = new Date(dt);
          const dayName = days[formatedDate.getDay()];
          if (item.courseFixedSchedule) {
            this.renderFixedSchedule(item, dayName, formatedDate, uniqueColorClass);
          } else if (item.courseVariableScheduleList?.length > 0) {
            this.renderVariableSchedule(item, dayName, formatedDate, uniqueColorClass);
          } else if (item.courseCalendarScheduleList?.length > 0) {
            this.renderCalendarSchedule(item, dayName, formatedDate, uniqueColorClass);
          }
        }
      }
    );
    this.createDatasetForRoutine();
  }

  renderFixedSchedule(item, dayName, formatedDate, uniqueColorClass) {
    const startTime = item.courseFixedSchedule.blockPeriod.periodStartTime;
    const startDateTime = new Date(
      formatedDate.toISOString().split("T")[0] + " " + startTime
    );

    const endTime = item.courseFixedSchedule.blockPeriod.periodEndTime;
    const endDateTime = new Date(
      formatedDate.toISOString().split("T")[0] + " " + endTime
    );

    item.dayOfWeek?.split("|").forEach((day) => {
      if (dayName === day) {
        let teacherName = item?.staffMasterList?.length > 0 ? (" - " +
          item?.staffMasterList[0]?.firstGivenName +
          " " +
          item?.staffMasterList[0]?.lastFamilyName) : null;

        this.events.push({
          title:
            item.courseSectionName + '' + (teacherName ? teacherName : ''),
          start: startDateTime,
          end: endDateTime,
          color: { primary: "red", secondary: "white" },
          meta: {
            color: uniqueColorClass,
            teacherName,
            monthTitle:
              Transform24to12Pipe.prototype.transform(
                item.courseFixedSchedule.blockPeriod.periodStartTime
              ) +
              " - " +
              item.courseSectionName,
            scheduleDetails: item,
            periodDetails: item.courseFixedSchedule.blockPeriod
          },
        });
      }
    });
  }

  renderVariableSchedule(item, dayName, formatedDate, uniqueColorClass) {
    item.courseVariableScheduleList.forEach((variableSchedule) => {
      const startTime = variableSchedule.blockPeriod.periodStartTime;
      const startDateTime = new Date(
        formatedDate.toISOString().split("T")[0] + " " + startTime
      );

      const endTime = variableSchedule.blockPeriod.periodEndTime;
      const endDateTime = new Date(
        formatedDate.toISOString().split("T")[0] + " " + endTime
      );
      if (dayName === variableSchedule.day) {
        let teacherName = item?.staffMasterList?.length > 0 ? (" - " +
          item?.staffMasterList[0]?.firstGivenName +
          " " +
          item?.staffMasterList[0]?.lastFamilyName) : null;
        this.events.push({
          title:
            item.courseSectionName + '' + (teacherName ? teacherName : ''),
          start: startDateTime,
          end: endDateTime,
          color: { primary: "green", secondary: "white" },
          meta: {
            color: uniqueColorClass,
            teacherName,
            monthTitle:
              Transform24to12Pipe.prototype.transform(
                variableSchedule.blockPeriod.periodStartTime
              ) +
              " - " +
              item.courseSectionName,
            scheduleDetails: item,
            periodDetails: variableSchedule.blockPeriod
          },
        });
      }
    });
  }

  renderCalendarSchedule(item, dayName, formatedDate, uniqueColorClass) {
    item.courseCalendarScheduleList.forEach((calendarSchedule) => {
      const startTime = calendarSchedule.blockPeriod.periodStartTime;
      const startDateTime = new Date(
        formatedDate.toISOString().split("T")[0] + " " + startTime
      );

      const endTime = calendarSchedule.blockPeriod.periodEndTime;
      const endDateTime = new Date(
        formatedDate.toISOString().split("T")[0] + " " + endTime
      );

      const scheduledDate = new Date(
        this.commonFunction.serverToLocalDateAndTime(calendarSchedule.date)
      )
        .toISOString()
        .split("T")[0];
      const currentDate = new Date(formatedDate).toISOString().split("T")[0];
      if (currentDate === scheduledDate) {
        let teacherName = item?.staffMasterList?.length > 0 ? (" - " +
          item?.staffMasterList[0]?.firstGivenName +
          " " +
          item?.staffMasterList[0]?.lastFamilyName) : null;
        this.events.push({
          title:
            item.courseSectionName + '' + (teacherName ? teacherName : ''),
          start: startDateTime,
          end: endDateTime,
          color: { primary: "blue", secondary: "white" },
          meta: {
            color: uniqueColorClass,
            teacherName,
            monthTitle:
              Transform24to12Pipe.prototype.transform(
                calendarSchedule.blockPeriod.periodStartTime
              ) +
              " - " +
              item.courseSectionName,
            scheduleDetails: item,
            periodDetails: calendarSchedule.blockPeriod
          },
        });
      }
    });
  }

  generateUniqueColor(item) {
    let uniqueColorClass = {
      backgroundColor: null,
      textColor: null
    };

    const foundIndex = this.uniqueColors.findIndex((color) => color.id === item.courseSectionId);

    if (foundIndex !== -1) {
      uniqueColorClass.backgroundColor = this.uniqueColors[foundIndex].backgroundColor;
      uniqueColorClass.textColor = this.uniqueColors[foundIndex].textColor;
    } else {

      let foundUniqueColor = false
      for (let [i, color] of uniqueColors.entries()) {
        if (!color.used) {
          uniqueColorClass.backgroundColor = color.backgroundColor;
          uniqueColorClass.textColor = color.textColor;

          this.uniqueColors[i].used = true;
          this.uniqueColors[i].id = item.courseSectionId;

          foundUniqueColor = true;
          break;
        }
      }
      if (!foundUniqueColor) {
        uniqueColorClass.backgroundColor = this.uniqueColors[0].backgroundColor;
        uniqueColorClass.textColor = this.uniqueColors[0].textColor;
      }
    }
    return uniqueColorClass;
  }

  eventClicked(scheduleDetails): void {
    this.scheduleDetails(scheduleDetails);
  }

  takeInput(index) {
    this.scheduleCoursesForStudent360Model.scheduleCourseSectionForView[
      index
    ].takeInput = true;

    this.disableUpdateCourseSection = false;
  }

  updateDropDateOfScheduledCourses() {
    const updateScheduledStudents = new ScheduledStudentDropModel();
    updateScheduledStudents.studentCoursesectionScheduleList = this.createDatasetBeforeUpdate();
    updateScheduledStudents.studentId = this.studentService.getStudentId();
    updateScheduledStudents.updatedBy = this.defaultService.getUserName();
    this.studentScheduleService
      .groupDropForScheduledStudent(updateScheduledStudents)
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
            this.getAllScheduleCoursesForStudent360();
          }
        } else {
          this.snackbar.open(sessionStorage.getItem("httpError"), "", {
            duration: 10000,
          });
        }
      });
  }

  createDatasetBeforeUpdate() {
    let courseSections = [];
    this.scheduleCoursesForStudent360Model.scheduleCourseSectionForView.map(
      (item, i) => {
        if (item.takeInput) {
          this.selectionForDropDateUpdate[
            i
          ].effectiveDropDate = this.commonFunction.formatDateSaveWithoutTime(
            this.selectionForDropDateUpdate[i].effectiveDropDate
          );
          let courseSection: StudentCoursesectionSchedule = new StudentCoursesectionSchedule();
          courseSection.courseSectionId = this.selectionForDropDateUpdate[
            i
          ].courseSectionId;
          courseSection.effectiveDropDate = this.selectionForDropDateUpdate[
            i
          ].effectiveDropDate;
          courseSection.updatedOn = this.commonFunction.formatDateSaveWithoutTime(new Date());
          courseSections.push(courseSection);
        }
      }
    );
    return courseSections;
  }

  findMarkingPeriodTitleById(courseSectionList) {
    courseSectionList = courseSectionList.map((item) => {
      item.takeInput = false;
      if (item.yrMarkingPeriodId) {
        item.yrMarkingPeriodId = "0_" + item.yrMarkingPeriodId;
      } else if (item.smstrMarkingPeriodId) {
        item.smstrMarkingPeriodId = "1_" + item.smstrMarkingPeriodId;
      } else if (item.qtrMarkingPeriodId) {
        item.qtrMarkingPeriodId = "2_" + item.qtrMarkingPeriodId;
      }

      if (
        item.yrMarkingPeriodId ||
        item.smstrMarkingPeriodId ||
        item.qtrMarkingPeriodId
      ) {
        for (const markingPeriod of this.getMarkingPeriodTitleListModel
          .getMarkingPeriodView) {
          if (markingPeriod.value == item.yrMarkingPeriodId) {
            item.markingPeriodTitle = markingPeriod.text;
            item.mpStartDate = item.schoolYears.startDate;
            break;
          } else if (markingPeriod.value == item.smstrMarkingPeriodId) {
            item.markingPeriodTitle = markingPeriod.text;
            item.mpStartDate = item.semesters.startDate;
            break;
          } else if (markingPeriod.value == item.qtrMarkingPeriodId) {
            item.markingPeriodTitle = markingPeriod.text;
            item.mpStartDate = item.quarters.startDate;
            break;
          }
        }
      } else {
        item.markingPeriodTitle = "Custom";
        item.mpStartDate = item.courseSectionDurationStartDate;
      }
      return item;
    });
    return courseSectionList;
  }

  renderRoutineView() {
    const today = new Date(this.todayDate);
    const cloneToday = today;
    for (let i = 0; i < 7; i++) {
      if (today.getDay() === 0) {
        this.initialRoutineDate = today;
        break;
      } else {
        cloneToday.setDate(today.getDate() - 1)
        if (cloneToday.getDay() === 0) {
          this.initialRoutineDate = cloneToday;
          break;
        }
      }
    }

    const cloneInitialDate = new Date(this.initialRoutineDate);
    this.endRoutingDate = new Date(cloneInitialDate.setDate(today.getDate() + 6));

    for (const d = new Date(this.initialRoutineDate); d <= this.endRoutingDate; d.setDate(d.getDate() + 1)) {
      this.currentWeek.push(new Date(d));
    }


  }
  createDatasetForRoutine() {
    this.routineViewWithEvent.routineView = [];
    this.events.map((item) => {
      const foundIndex = this.routineViewWithEvent.routineView?.findIndex((routine) => {
        return (routine.blockId === item.meta.periodDetails.blockId && routine.periodId === item.meta.periodDetails.periodId)
      });
      if (foundIndex !== -1) {
        let event: RoutineViewEvent;
        event = {
          date: item.start.toISOString().split('T')[0],
          courseSectionName: item.meta.scheduleDetails.courseSectionName,
          staffName: item.meta.teacherName,
          color: item.meta.color.textColor
        };
        this.routineViewWithEvent.routineView[foundIndex].events.push(event)
      } else {
        let eachEvent: RoutineView = new RoutineView();
        eachEvent = {
          periodId: item.meta.periodDetails.periodId,
          blockId: item.meta.periodDetails.blockId,
          periodName: item.meta.periodDetails.periodTitle,
          periodStartTime: item.meta.periodDetails.periodStartTime,
          periodEndTime: item.meta.periodDetails.periodEndTime,
          events: [],
          filteredEvents: []
        };
        let event: RoutineViewEvent;
        event = {
          date: item.start.toISOString().split('T')[0],
          courseSectionName: item.meta.scheduleDetails.courseSectionName,
          staffName: item.meta.teacherName,
          color: item.meta.color.textColor
        };
        eachEvent.events.push(event);
        this.routineViewWithEvent.routineView.push(eachEvent);
      }
    });

    this.filterEventsForRoutineView();
  }

  filterEventsForRoutineView() {
    let count = 0;
    for (const dt = new Date(this.initialRoutineDate);
      dt <= this.endRoutingDate;
      dt.setDate(dt.getDate() + 1)) {

      this.routineViewWithEvent.routineView.map((period) => {
        let filtered = null;
        period.events.forEach((event) => {
          if (new Date(event.date).getTime() == new Date(dt.toISOString().split('T')[0]).getTime()) {
            filtered = event;
          }
        })
        period.filteredEvents[count] = filtered
      })
      count = count + 1;
    }
  }

  previousWeek() {
    let initialDate = new Date(this.initialRoutineDate);
    let count = 0;
    for (let d = new Date(initialDate.setDate(initialDate.getDate() - 7));
      d < this.initialRoutineDate;
      d.setDate(d.getDate() + 1)) {
      this.currentWeek[count] = new Date(d)
      count++
    }
    this.initialRoutineDate = this.currentWeek[0];
    this.endRoutingDate = this.currentWeek[this.currentWeek.length - 1];
    this.filterEventsForRoutineView();
  }

  nextWeek() {
    let endDate = new Date(this.endRoutingDate);
    for (let i = 0;
      i < 7;
      i++) {
      this.currentWeek[i] = new Date(endDate.setDate(endDate.getDate() + 1))
    }
    this.initialRoutineDate = this.currentWeek[0];
    this.endRoutingDate = this.currentWeek[this.currentWeek.length - 1];
    this.filterEventsForRoutineView();
  }

  translateKey(key) {
    let translateKey;
    this.translateService.get(key).subscribe((res: string) => {
      translateKey = res;
    });
    return translateKey;
  }

  exportScheduledCourses() {
    if (this.scheduleCoursesForStudent360Model.scheduleCourseSectionForView?.length > 0) {
      let scheduledCoursesList;
      scheduledCoursesList = this.scheduleCoursesForStudent360Model.scheduleCourseSectionForView?.map((item) => {
        let teacherNames = null
        item.staffMasterList?.map((teacher) => {
          teacherNames = teacherNames ? ',' : '' + teacher.firstGivenName + ' ' + teacher.lastFamilyName;
        });
        let enrolledDate = new Date(item.enrolledDate);
        let dropDate = new Date(item.effectiveDropDate)
        return {
          [this.translateKey('course')]: item.courseTitle,
          [this.translateKey('courseSection')]: item.courseSectionName,
          [this.translateKey('teachers')]: teacherNames ? teacherNames : '-',
          [this.translateKey('markingPeriod')]: item.markingPeriodTitle,
          [this.translateKey('enrolledDate')]: format(enrolledDate, 'MMM d, y'),
          [this.translateKey('endDropDate')]: format(dropDate, 'MMM d, y')
        };
      });

      this.excelService.exportAsExcelFile(scheduledCoursesList, 'Scheduled_Course_List_')
    } else {
      this.snackbar.open('No Records Found. Failed to Export Scheduled Courses', '', {
        duration: 5000
      });
    }
  }
}
