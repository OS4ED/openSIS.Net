import { Component, OnInit, ViewChild, Input, OnChanges, EventEmitter, Output, SimpleChanges, ChangeDetectionStrategy, ChangeDetectorRef, AfterViewChecked } from '@angular/core';
import icClose from '@iconify/icons-ic/twotone-close';
import icPlusCircle from '@iconify/icons-ic/add-circle-outline';
import { weekDay } from '../../../../../enums/day.enum';
import { SchoolPeriodService } from '../../../../../services/school-period.service';
import { BlockListViewModel } from '../../../../../models/schoolPeriodModel';
import { MatSnackBar } from '@angular/material/snack-bar';
import { RoomService } from '../../../../../services/room.service';
import { RoomListViewModel } from '../../../../../models/roomModel';
import { CourseVariableSchedule, OutputEmitDataFormat, CourseSectionAddViewModel, DeleteCourseSectionSchedule } from '../../../../../models/courseSectionModel';
import { CourseSectionService } from '../../../../../services/course-section.service';
import { map } from 'rxjs/operators';
import { NgForm } from '@angular/forms';
@Component({
  selector: 'vex-variable-scheduling',
  templateUrl: './variable-scheduling.component.html',
  styleUrls: ['./variable-scheduling.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class VariableSchedulingComponent implements OnInit, OnChanges, AfterViewChecked {
  @Input() selectedCalendar;
  @Input() seatChangeFlag;
  icClose = icClose;
  icPlusCircle = icPlusCircle;
  variableScheduleList = [];
  blockListViewModel: BlockListViewModel = new BlockListViewModel();
  roomListViewModel: RoomListViewModel = new RoomListViewModel();
  courseSectionAddViewModel: CourseSectionAddViewModel = new CourseSectionAddViewModel();

  selected = null;
  selectedBlocks = [];
  selectedPeriod = []
  divCount = [0];
  weekDaysList = [];
  filterDays;
  periodList = [];
  selectedRooms = [];
  roomIdWithCapacity = [];
  @ViewChild('form') currentForm: NgForm;
  @Input() detailsFromParentModal;
  @Output() variableScheduleData = new EventEmitter<OutputEmitDataFormat>();

  constructor(private snackbar: MatSnackBar,
    private schoolPeriodService: SchoolPeriodService,
    private roomService: RoomService,
    private courseSectionService: CourseSectionService,
    private cdr: ChangeDetectorRef) {
    this.courseSectionService.currentUpdate.subscribe((res) => {
      if (res) {
        this.sendVariableScheduleDataToParent();
      }
    })
  }

  ngOnInit(): void {
    this.getAllBlockList();
    this.getAllRooms();
    this.weekDaysList = Object.keys(weekDay).filter(k => typeof weekDay[k] === 'number')
      .map(label => ({ label, value: weekDay[label] }))



    if (this.detailsFromParentModal.editMode) {
      for (let i = 0; i < this.detailsFromParentModal.courseSectionDetails.courseVariableSchedule.length; i++) {
        this.courseSectionAddViewModel.courseVariableScheduleList[i] = this.detailsFromParentModal.courseSectionDetails.courseVariableSchedule[i];

        this.weekDaysList.map(val => {
          if (this.courseSectionAddViewModel.courseVariableScheduleList[i].day === val.label) {
            this.courseSectionAddViewModel.courseVariableScheduleList[i].day = val.value;
          }
        })

        this.divCount[i] = i;
      }


    } else {
      this.courseSectionAddViewModel.courseVariableScheduleList[0].courseId = this.detailsFromParentModal.courseDetails.courseId;
    }

  }
  ngOnChanges(changes: SimpleChanges): void {
    if (this.selectedCalendar.days !== undefined) {
      this.getDays(this.selectedCalendar.days)
    }

  }

  ngAfterViewChecked() {
    this.cdr.detectChanges();
  }

  getDays(days: string) {
    const calendarDays = days;
    var allDays = [0, 1, 2, 3, 4, 5, 6];
    var splitDays = calendarDays?.split('').map(x => +x);
    this.filterDays = allDays.filter(f => !splitDays.includes(f));
    this.weekDaysList.map((val, i) => {
      this.filterDays.map(data => {
        if (data == val.value) {
          this.weekDaysList.splice(i, 1);
        }
      })
    })
  }

  onPeriodChange(periodId, indexOfDynamicRow) {
    let index = this.blockListViewModel.getBlockListForView[0]?.blockPeriod.findIndex((x) => {
      return x.periodId === +periodId
    })
    this.selectedPeriod[indexOfDynamicRow] = index;
  }

  addMoreRotatingScheduleRow() {
    this.courseSectionAddViewModel.courseVariableScheduleList.push(new CourseVariableSchedule());
    this.divCount.push(2); // Why 2? We have to fill up the divCount, It could be anything.
  }

  deleteRow(indexOfDynamicRow) {
    if (this.courseSectionAddViewModel.courseVariableScheduleList[indexOfDynamicRow]?.serial > 0) {
      this.deleteCourseSchedule(indexOfDynamicRow);
    }
    this.divCount.splice(indexOfDynamicRow, 1);
    this.courseSectionAddViewModel.courseVariableScheduleList.splice(indexOfDynamicRow, 1);
    this.selectedBlocks.splice(indexOfDynamicRow, 1);
    this.selectedPeriod.splice(indexOfDynamicRow, 1);   
  }

  deleteCourseSchedule(index) {
    let deleteVariableSchedule=new DeleteCourseSectionSchedule()
    deleteVariableSchedule.scheduleType='variableSchedule';
    deleteVariableSchedule.serial=this.courseSectionAddViewModel.courseVariableScheduleList[index]?.serial;
    deleteVariableSchedule.courseId=this.courseSectionAddViewModel.courseVariableScheduleList[index]?.courseId;
    deleteVariableSchedule.courseSectionId=this.courseSectionAddViewModel.courseVariableScheduleList[index]?.courseSectionId;

    this.courseSectionService.deleteSchedule(deleteVariableSchedule).subscribe((res) => {
      if (typeof (res) == 'undefined') {
        this.snackbar.open('Varibale Schedule Deletion failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 5000
        });
      }
      else {
        if (res._failure) {
            this.snackbar.open(res._message, '', {
              duration: 5000
            });
        }
        else {
          this.snackbar.open(res._message, '', {
            duration: 5000
          });
        }
      }
    })
  }
  getAllBlockList() {
    this.schoolPeriodService.getAllBlockList(this.blockListViewModel).subscribe(data => {
      if (data._failure) {
        if (data._message === "NO RECORD FOUND") {
          this.periodList = [];
          this.snackbar.open('NO RECORD FOUND. ', '', {
            duration: 1000
          });
        }
      } else {
        this.blockListViewModel = data;
        if (data.getBlockListForView.length > 0) {
          this.periodList = data.getBlockListForView[0].blockPeriod;
        }
        if (this.detailsFromParentModal.editMode) {
          for (let [i, val] of this.courseSectionAddViewModel.courseVariableScheduleList.entries()) {
            this.blockListViewModel.getBlockListForView?.map((item, j) => {
              let periodIndex = this.blockListViewModel.getBlockListForView[j].blockPeriod.findIndex((x) => {
                return x.periodId == +this.courseSectionAddViewModel.courseVariableScheduleList[i].periodId;
              });
              if (periodIndex != -1) {
                this.selectedPeriod[i] = periodIndex;
              }
            });
          }
        }
      }
    });
  }

  getAllRooms() {
    this.roomListViewModel.schoolId = +sessionStorage.getItem("selectedSchoolId");
    this.roomService.getAllRoom(this.roomListViewModel).subscribe(
      (res: RoomListViewModel) => {
        if (typeof (res) == 'undefined') {
          this.snackbar.open('Room list failed. ' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else {
          if (res._failure) {
            if (res._message === "NO RECORD FOUND") {

            } else {
              this.snackbar.open('Room List failed. ' + res._message, '', {
                duration: 10000
              });
            }
          }
          else {
            this.roomListViewModel = res;
            this.roomListViewModel.tableroomList.map((item, index) => {
              this.roomIdWithCapacity[item.roomId] = item.capacity;
            })

          }
        }
      })
  }




  sendVariableScheduleDataToParent() {
    this.currentForm.form.markAllAsTouched()
    let invalidSeatCapacity = false;
    invalidSeatCapacity = this.courseSectionAddViewModel.courseVariableScheduleList.some((item, i) => {
      if (this.detailsFromParentModal.form.value.seats > this.roomIdWithCapacity[item.roomId]) {
        return true;
      } else {
        return false;
      }
    })
    if (this.currentForm.form.valid && !invalidSeatCapacity) {
      this.checkDuplicateRow();
    } else {
      this.variableScheduleData.emit({ scheduleType: 'variableSchedule', roomList: null, scheduleDetails: this.courseSectionAddViewModel.courseVariableScheduleList, error: true });
    }
  }
  checkDuplicateRow() {
    this.currentForm.form.markAllAsTouched();
    if (this.currentForm.form.invalid) {
      return
    }
    let Ids = [];
    for (let [i, val] of this.courseSectionAddViewModel.courseVariableScheduleList.entries()) {
      Ids[i] = +this.courseSectionAddViewModel.courseVariableScheduleList[i].day
        + this.courseSectionAddViewModel.courseVariableScheduleList[i].periodId
        + this.courseSectionAddViewModel.courseVariableScheduleList[i].roomId
    }
    let checkDuplicate = Ids.sort().some((item, i) => {
      if (item == Ids[i + 1]) {
        this.snackbar.open('Cannot Save Duplicate Variable Schedule ', '', {
          duration: 5000
        });
        return true;
      } else {
        return false;
      }
    })
    for (var i = 0; i < this.courseSectionAddViewModel.courseVariableScheduleList.length; i++) {
      let blockId = this.periodList[0].blockId;
      this.courseSectionAddViewModel.courseVariableScheduleList[i].blockId = blockId
    }

    if (checkDuplicate) {
      this.variableScheduleData.emit({ scheduleType: 'variableSchedule', roomList: null, scheduleDetails: this.courseSectionAddViewModel.courseVariableScheduleList, error: true });
    } else {
      this.variableScheduleData.emit({ scheduleType: 'variableSchedule', roomList: null, scheduleDetails: this.courseSectionAddViewModel.courseVariableScheduleList, error: false });
    }
  }
}
