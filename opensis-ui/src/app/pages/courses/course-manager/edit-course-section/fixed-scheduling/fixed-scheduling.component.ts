import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { RoomListViewModel } from '../../../../../models/roomModel';
import { BlockListViewModel } from '../../../../../models/schoolPeriodModel';
import { RoomService } from '../../../../../services/room.service';
import { SchoolPeriodService } from '../../../../../services/school-period.service';
import { FixedSchedulingCourseSectionAddModel, OutputEmitDataFormat } from '../../../../../models/courseSectionModel';
import { CourseSectionService } from '../../../../../services/course-section.service';

@Component({
  selector: 'vex-fixed-scheduling',
  templateUrl: './fixed-scheduling.component.html',
  styleUrls: ['./fixed-scheduling.component.scss']
})
export class FixedSchedulingComponent implements OnInit,OnChanges {
  @ViewChild('f') currentForm: NgForm;
  roomListViewModel:RoomListViewModel=new RoomListViewModel();
  fixedSchedulingModel: FixedSchedulingCourseSectionAddModel = new FixedSchedulingCourseSectionAddModel();
  @Input() calendar;
  @Input() detailsFromParentModal;
  @Output() fixedScheduleData = new EventEmitter<OutputEmitDataFormat>()
  activeDays=[];
  periodList=[];
  weekArray: number[] = [];
  activeDaysError=false;
  weeks = [
    { name: 'sunday', id: 0 },
    { name: 'monday', id: 1 },
    { name: 'tuesday', id: 2 },
    { name: 'wednesday', id: 3 },
    { name: 'thursday', id: 4 },
    { name: 'friday', id: 5 },
    { name: 'saturday', id: 6 }
  ];
  constructor(
    private roomService:RoomService,
    private schoolPeriodService:SchoolPeriodService,
    private snackbar: MatSnackBar,
    private courseSectionService:CourseSectionService) {
      this.courseSectionService.currentUpdate.subscribe((res)=>{
        if(res){
          this.currentForm.form.markAllAsTouched();
          this.sendFixedScheduleDataToParent();
        }
      })
     }

  ngOnChanges(): void {
    
    if(this.calendar==undefined || this.calendar?.days==undefined){
      this.weekArray =[];
    }
    else{
      this.weekArray = this.calendar.days.split('').map(x => +x);
      let namesOfDayById=[];
      this.weekArray?.map((item)=>{
        this.weeks.map((week)=>{
          if(item==week.id){
            namesOfDayById.push(week.name);
          }
        })
      });
      this.weekArray=namesOfDayById;
    }
  }

  ngOnInit(): void {
    if(this.detailsFromParentModal.editMode){
      this.patchFormValue();
    }
    this.getAllRooms();
    this.getAllBlockList();
  }

  patchFormValue(){
    this.fixedSchedulingModel.courseFixedSchedule.roomId=this.detailsFromParentModal.courseSectionDetails.courseFixedSchedule.roomId;
    this.fixedSchedulingModel.courseFixedSchedule.periodId=this.detailsFromParentModal.courseSectionDetails.courseFixedSchedule.periodId;
    this.fixedSchedulingModel.courseFixedSchedule.attendanceTaken=this.detailsFromParentModal.courseSectionDetails.courseSection.attendanceTaken;
    this.fixedSchedulingModel.courseFixedSchedule.courseSectionId=this.detailsFromParentModal.courseSectionDetails.courseSection.courseSectionId;
    this.fixedSchedulingModel.courseFixedSchedule.serial=this.detailsFromParentModal.courseSectionDetails.courseFixedSchedule.serial;
    this.fixedSchedulingModel.courseFixedSchedule.gradeScaleId=this.detailsFromParentModal.courseSectionDetails.courseSection.gradeScaleId;
    this.fixedSchedulingModel.courseFixedSchedule.tenantId=sessionStorage.getItem('tenantId')
    this.fixedSchedulingModel.courseFixedSchedule.updatedBy=sessionStorage.getItem("email");
    this.activeDays = this.detailsFromParentModal.courseSectionDetails.courseSection.meetingDays.split('|');
      }
 
  getAllRooms(){
    this.roomService.getAllRoom(this.roomListViewModel).subscribe(
      (res:RoomListViewModel)=>{
        if(typeof(res)=='undefined'){
          this.snackbar.open('' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else{
          if (res._failure) {    
            this.snackbar.open(res._message, '', {
              duration: 10000
            }); 
          } 
          else{
            this.roomListViewModel=res;
          }
        }
      }
    )
  }

  getAllBlockList() {
    let blockListViewModel:BlockListViewModel= new BlockListViewModel();
    blockListViewModel.tenantId = sessionStorage.getItem('tenantId');
    blockListViewModel.schoolId = +sessionStorage.getItem('selectedSchoolId');
    this.schoolPeriodService.getAllBlockList(blockListViewModel).subscribe(
      (res: BlockListViewModel) => {
        if(typeof(res)=='undefined'){
          this.snackbar.open('' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else{
          if (res._failure) {    
            this.snackbar.open(res._message, '', {
              duration: 10000
            }); 
          } 
          else{
            this.periodList=res.getBlockListForView[0]?.blockPeriod
            this.fixedSchedulingModel.courseFixedSchedule.blockId=res.getBlockListForView[0]?.blockId;
          }
        }
        
      }
    );
  }

  
  selectDays(event:Event, name) {

    event.preventDefault();
    if(this.weekArray.includes(name)){
      var index = this.activeDays.indexOf(name);
      if(index>-1){
        this.activeDays.splice(index,1);
      }
      else{
        this.activeDays.push(name);
      }
    }
  }

  sendFixedScheduleDataToParent(){
    if(this.currentForm.form.valid && this.activeDays.length>0){
      this.fixedSchedulingModel.courseFixedSchedule.meetingDays=this.activeDays?.join('|');
      this.fixedScheduleData.emit({scheduleType:'fixedschedule',roomList: null,scheduleDetails:this.fixedSchedulingModel.courseFixedSchedule,error:false});
    }else{
    this.activeDaysError=true;
    this.fixedScheduleData.emit({scheduleType:'fixedschedule', roomList: null, scheduleDetails:null,error:true});
    }

  }

}

