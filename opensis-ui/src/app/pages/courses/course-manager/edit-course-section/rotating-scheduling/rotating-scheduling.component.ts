import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import icClose from '@iconify/icons-ic/twotone-close';
import { BlockListViewModel } from '../../../../../models/school-period.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SchoolPeriodService } from '../../../../../services/school-period.service';
import { RoomService } from '../../../../../services/room.service';
import { RoomListViewModel } from '../../../../../models/room.model';
import icPlusCircle from '@iconify/icons-ic/add-circle-outline';
import { BlockedSchedulingCourseSectionAddModel, OutputEmitDataFormat, CourseBlockSchedule, DeleteCourseSectionSchedule } from '../../../../../models/course-section.model';
import { map } from 'rxjs/operators';
import { CourseSectionService } from '../../../../../services/course-section.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'vex-rotating-scheduling',
  templateUrl: './rotating-scheduling.component.html',
  styleUrls: ['./rotating-scheduling.component.scss']
})
export class RotatingSchedulingComponent implements OnInit {
  icClose = icClose;
  icPlusCircle = icPlusCircle;

  blockListViewModel: BlockListViewModel = new BlockListViewModel();
  roomListViewModel: RoomListViewModel = new RoomListViewModel();
  blockScheduleAddModel: BlockedSchedulingCourseSectionAddModel = new BlockedSchedulingCourseSectionAddModel()
  selected = null;
  selectedBlocks = [];
  selectedPeriod = []
  divCount = [0];
  @ViewChild('form') currentForm: NgForm;
  @Input() detailsFromParentModal;
  @Output() blockScheduleData = new EventEmitter<OutputEmitDataFormat>()
  roomIdWithCapacity = [];
  constructor(private snackbar: MatSnackBar,
    private schoolPeriodService: SchoolPeriodService,
    private roomService: RoomService,
    private courseSectionService: CourseSectionService) {
    this.courseSectionService.currentUpdate.subscribe((res) => {
      if (res) {
        this.sendBlockScheduleDataToParent();
      }
    })
  }

  ngOnInit(): void {
    this.getAllBlockList();
    this.getAllRooms();
    if (!this.detailsFromParentModal.editMode) {
      this.blockScheduleAddModel.courseBlockScheduleList[0].courseId = this.detailsFromParentModal.courseDetails.courseId;
    }
  }

  onBlockDayChange(blockId, indexOfDynamicRow) {
    this.blockScheduleAddModel.courseBlockScheduleList[indexOfDynamicRow].periodId = null;

    let index = this.blockListViewModel.getBlockListForView.findIndex((x) => {
      return x.blockId === +blockId;
    });
    this.selectedBlocks[indexOfDynamicRow] = index;
  }

  onPeriodChange(periodId, indexOfDynamicRow) {
    let index = this.blockListViewModel.getBlockListForView[this.selectedBlocks[indexOfDynamicRow]]?.blockPeriod.findIndex((x) => {
      return x.periodId === +periodId
    })
    this.selectedPeriod[indexOfDynamicRow] = index;
  }

  addMoreRotatingScheduleRow() {
    this.blockScheduleAddModel.courseBlockScheduleList.push(new CourseBlockSchedule)
    this.divCount.push(2); // Why 2? We have to fill up the divCount, It could be anything.
  }

  deleteRow(indexOfDynamicRow) {
    this.divCount.splice(indexOfDynamicRow, 1);
    this.blockScheduleAddModel.courseBlockScheduleList.splice(indexOfDynamicRow, 1);
    this.selectedBlocks.splice(indexOfDynamicRow, 1);
    this.selectedPeriod.splice(indexOfDynamicRow, 1);
  }


  getAllBlockList() {
    this.blockListViewModel.getBlockListForView = [];
    this.schoolPeriodService.getAllBlockList(this.blockListViewModel).pipe(
      map((res) => {
        res.getBlockListForView = res.getBlockListForView.filter((item) => {
          return item.blockId != 1;
        })
        return res;
      })
    ).subscribe(
      (res: BlockListViewModel) => {
        if (typeof (res) == 'undefined') {
          this.snackbar.open('Block/Rotation Days list failed. ' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else {
          if (res._failure) {
            this.snackbar.open('Block/Rotation Days list failed. ' + res._message, '', {
              duration: 10000
            });
          }
          else {
            this.blockListViewModel = res;
            if (this.detailsFromParentModal.editMode) {
              this.manipulateBlockAndPeriodInEditMode()
            }
          }
        }
      }
    );
  }

  manipulateBlockAndPeriodInEditMode() {
    for (let [i, val] of this.detailsFromParentModal.courseSectionDetails.courseBlockSchedule.entries()) {
      this.blockScheduleAddModel.courseBlockScheduleList[i] = this.detailsFromParentModal.courseSectionDetails.courseBlockSchedule[i];
      this.blockScheduleAddModel.courseBlockScheduleList[i].updatedBy = sessionStorage.getItem("email")
      this.divCount[i] = i;
      let index = this.blockListViewModel.getBlockListForView.findIndex((x) => {
        return x.blockId === +this.detailsFromParentModal.courseSectionDetails.courseBlockSchedule[i].blockId;
      });
      this.selectedBlocks[i] = index;

      this.blockListViewModel.getBlockListForView?.map((item, j) => {
        let periodIndex = this.blockListViewModel.getBlockListForView[j].blockPeriod.findIndex((x) => {

          return x.periodId == +this.detailsFromParentModal.courseSectionDetails.courseBlockSchedule[i].periodId;
        });
        if (periodIndex != -1) {
          this.selectedPeriod[i] = periodIndex;
        }
      })

      // for(let j=0;j<this.blockListViewModel.getBlockListForView.length;j++){
      //   let periodIndex = this.blockListViewModel.getBlockListForView[j].blockPeriod.findIndex((x) => {

      //     return x.periodId == +this.detailsFromParentModal.courseSectionDetails.courseBlockSchedule[i].periodId;
      //   });
      //   if(periodIndex!=-1){
      //     this.selectedPeriod[i] = periodIndex;
      //   }
      // }
    }


  }
  getAllRooms() {
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

  sendBlockScheduleDataToParent() {
    this.currentForm.form.markAllAsTouched()
    let invalidSeatCapacity = false;
    invalidSeatCapacity = this.blockScheduleAddModel.courseBlockScheduleList.some((item, i) => {
      if (this.detailsFromParentModal.form.value.seats > this.roomIdWithCapacity[item.roomId]) {
        return true;
      } else {
        return false;
      }
    })
    if (this.currentForm.form.valid && !invalidSeatCapacity) {
      this.checkDuplicateRow();
    } else {
      this.blockScheduleData.emit({ scheduleType: 'blockSchedule', roomList: null, scheduleDetails: null, error: true });
    }
  }

  checkDuplicateRow() {
    let Ids = [];
    for (let [i, val] of this.blockScheduleAddModel.courseBlockScheduleList.entries()) {
      Ids[i] = this.blockScheduleAddModel.courseBlockScheduleList[i].blockId?.toString()
        + this.blockScheduleAddModel.courseBlockScheduleList[i].periodId
        + this.blockScheduleAddModel.courseBlockScheduleList[i].roomId
    }
    let checkDuplicate = Ids.sort().some((item, i) => {
      if (item == Ids[i + 1]) {
        this.snackbar.open('Cannot Save Duplicate Block Schedule ', '', {
          duration: 10000
        });
        return true;
      } else {
        return false;
      }
    })
    if (checkDuplicate) {
      this.blockScheduleData.emit({ scheduleType: 'blockSchedule', roomList: null, scheduleDetails: null, error: true });
    } else {
      this.blockScheduleData.emit({ scheduleType: 'blockSchedule', roomList: null, scheduleDetails: this.blockScheduleAddModel.courseBlockScheduleList, error: false });
    }
  }



}
