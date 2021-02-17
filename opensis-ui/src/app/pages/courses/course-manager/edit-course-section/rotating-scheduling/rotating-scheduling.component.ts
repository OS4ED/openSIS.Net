import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import icClose from '@iconify/icons-ic/twotone-close';
import { BlockListViewModel } from '../../../../../models/schoolPeriodModel';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SchoolPeriodService } from '../../../../../services/school-period.service';
import { RoomService } from '../../../../../services/room.service';
import { RoomListViewModel } from '../../../../../models/roomModel';
import icPlusCircle from '@iconify/icons-ic/add-circle-outline';
import { BlockedSchedulingCourseSectionAddModel,OutputEmitDataFormat, CourseBlockSchedule } from '../../../../../models/courseSectionModel';
import { map } from 'rxjs/operators';
import { CourseSectionService } from '../../../../../services/course-section.service';

@Component({
  selector: 'vex-rotating-scheduling',
  templateUrl: './rotating-scheduling.component.html',
  styleUrls: ['./rotating-scheduling.component.scss']
})
export class RotatingSchedulingComponent implements OnInit {
  icClose = icClose;
  icPlusCircle = icPlusCircle;

  blockListViewModel: BlockListViewModel = new BlockListViewModel();
  roomListViewModel:RoomListViewModel = new RoomListViewModel();
  blockScheduleAddModel:BlockedSchedulingCourseSectionAddModel= new BlockedSchedulingCourseSectionAddModel()
  selected=null;
  selectedBlocks=[];
  selectedPeriod=[]
  divCount=[0];
  @Input() detailsFromParentModal;
  @Output() blockScheduleData = new EventEmitter<OutputEmitDataFormat>()
  constructor(private snackbar: MatSnackBar,
    private schoolPeriodService: SchoolPeriodService,
    private roomService:RoomService,
    private courseSectionService:CourseSectionService) {
        this.courseSectionService.currentUpdate.subscribe((res)=>{
          if(res){
            this.sendBlockScheduleDataToParent();
          }
        })
     }

  ngOnInit(): void {
    this.getAllBlockList();
    this.getAllRooms();
    if(!this.detailsFromParentModal.editMode){
      this.blockScheduleAddModel.courseBlockScheduleList[0].courseId=this.detailsFromParentModal.courseDetails.courseId;
    }
  }
  onBlockDayChange(blockId, indexOfDynamicRow){
    let index = this.blockListViewModel.getBlockListForView.findIndex((x) => {
      return x.blockId === +blockId;
    });
    this.selectedBlocks[indexOfDynamicRow] = index;
  }

  onPeriodChange(periodId,indexOfDynamicRow){
    let index=this.blockListViewModel.getBlockListForView[this.selectedBlocks[indexOfDynamicRow]]?.blockPeriod.findIndex((x)=>{
      return x.periodId===+periodId
    })
    this.selectedPeriod[indexOfDynamicRow] = index;
  }

  addMoreRotatingScheduleRow() {
    this.blockScheduleAddModel.courseBlockScheduleList.push(new CourseBlockSchedule)
    this.divCount.push(2); // Why 2? We have to fill up the divCount, It could be anything.
  }

  deleteRow(indexOfDynamicRow){
    this.divCount.splice(indexOfDynamicRow, 1);
    this.blockScheduleAddModel.courseBlockScheduleList.splice(indexOfDynamicRow, 1);
    this.selectedBlocks.splice(indexOfDynamicRow, 1);
    this.selectedPeriod.splice(indexOfDynamicRow, 1);
  }

  getAllBlockList() {
    this.blockListViewModel.getBlockListForView = [];
    this.blockListViewModel.tenantId = sessionStorage.getItem('tenantId');
    this.blockListViewModel.schoolId = +sessionStorage.getItem('selectedSchoolId');
    this.schoolPeriodService.getAllBlockList(this.blockListViewModel).pipe(
     map((res)=>{
       res.getBlockListForView=res.getBlockListForView.filter((item)=>{
         return item.blockId!=1;
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
            // this.blockScheduleAddModel.courseBlockScheduleList[0].blockId=this.blockListViewModel.getBlockListForView[0]?.blockId;
            // this.blockScheduleAddModel.courseBlockScheduleList[0].periodId=this.blockListViewModel.getBlockListForView[0]?.blockPeriod[0]?.periodId;
          }
        }
      }
    );
  }

  getAllRooms(){
    this.roomListViewModel.schoolId=+sessionStorage.getItem("selectedSchoolId");
    this.roomService.getAllRoom(this.roomListViewModel).subscribe(
      (res:RoomListViewModel)=>{
        if(typeof(res)=='undefined'){
          this.snackbar.open('Room list failed. ' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else{
          if (res._failure) {     
            if (res._message === "NO RECORD FOUND") {
  
            } else {
              this.snackbar.open('Room List failed. ' + res._message, '', {
                duration: 10000
              });
            }
          } 
          else { 
            this.roomListViewModel=res;  
            // this.blockScheduleAddModel.courseBlockScheduleList[0].roomId=this.roomListViewModel.tableroomList[0].roomId;
          }
        }
      })
  }

  sendBlockScheduleDataToParent(){
    
    this.blockScheduleData.emit({scheduleType:'blockSchedule',scheduleDetails:this.blockScheduleAddModel.courseBlockScheduleList,error:false});
  }
  

}
