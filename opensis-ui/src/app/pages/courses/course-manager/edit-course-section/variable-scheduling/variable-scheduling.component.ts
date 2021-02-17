import { Component, OnInit,ViewChild,Input, OnChanges,EventEmitter,Output } from '@angular/core';
import icClose from '@iconify/icons-ic/twotone-close';
import icPlusCircle from '@iconify/icons-ic/add-circle-outline';
import { weekDay } from '../../../../../enums/day.enum';
import { SchoolPeriodService } from '../../../../../services/school-period.service';
import { BlockListViewModel } from '../../../../../models/schoolPeriodModel';
import { MatSnackBar } from '@angular/material/snack-bar';
import { RoomService } from '../../../../../services/room.service';
import { RoomListViewModel} from '../../../../../models/roomModel';
import {CourseVariableSchedule,OutputEmitDataFormat,CourseSectionAddViewModel } from '../../../../../models/courseSectionModel';
import { CourseSectionService } from '../../../../../services/course-section.service';
import { map } from 'rxjs/operators';
import { NgForm } from '@angular/forms';
@Component({
  selector: 'vex-variable-scheduling',
  templateUrl: './variable-scheduling.component.html',
  styleUrls: ['./variable-scheduling.component.scss']
})
export class VariableSchedulingComponent implements OnInit ,OnChanges {
  @Input() selectedCalendar;
  icClose = icClose;
  icPlusCircle = icPlusCircle;
  variableScheduleList=[];
  blockListViewModel: BlockListViewModel = new BlockListViewModel();
  roomListViewModel:RoomListViewModel = new RoomListViewModel();  
  courseSectionAddViewModel:CourseSectionAddViewModel= new CourseSectionAddViewModel();
  
  selected=null;
  selectedBlocks=[];
  selectedPeriod=[]
  divCount=[0];
  weekDaysList=[];
  filterDays;
  periodList=[];
  @ViewChild('form') currentForm: NgForm;
  @Input() detailsFromParentModal;
  @Output() variableScheduleData = new EventEmitter<OutputEmitDataFormat>()
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
    this.weekDaysList= Object.keys(weekDay).filter(k => typeof weekDay[k] === 'number')
    .map(label => ({ label, value: weekDay[label] }))  
    this.getAllBlockList();
    this.getAllRooms();
    if(this.detailsFromParentModal.editMode){
      for(let i=0;i<this.detailsFromParentModal.courseSectionDetails.courseVariableSchedule.length;i++){
          this.courseSectionAddViewModel.courseVariableScheduleList[i]=this.detailsFromParentModal.courseSectionDetails.courseVariableSchedule[i];
          
          this.weekDaysList.map(val=>{
            if(this.courseSectionAddViewModel.courseVariableScheduleList[i].day === val.label){
              this.courseSectionAddViewModel.courseVariableScheduleList[i].day = val.value;
            }
          })
          this.divCount[i]=i;
      }     
    }else{
      this.courseSectionAddViewModel.courseVariableScheduleList[0].courseId=this.detailsFromParentModal.courseDetails.courseId;
    }
   
  }
  ngOnChanges(changes: import("@angular/core").SimpleChanges): void { 
  
    if(this.selectedCalendar.days !== undefined){
      this.getDays(this.selectedCalendar.days)
    }    
  }
  getDays(days: string) {
    const calendarDays = days;
    var allDays = [0, 1, 2, 3, 4, 5, 6];
    var splitDays = calendarDays.split('').map(x => +x);
    this.filterDays = allDays.filter(f => !splitDays.includes(f));
    this.weekDaysList.map((val,i)=>{
      this.filterDays.map(data=>{       
        if(data == val.value){
          this.weekDaysList.splice(i, 1); 
        }
      })
    })    
  }
  
  onPeriodChange(periodId,indexOfDynamicRow){    
    let index=this.blockListViewModel.getBlockListForView[0]?.blockPeriod.findIndex((x)=>{
      return x.periodId===+periodId
    })    
    this.selectedPeriod[indexOfDynamicRow] = index;  
  }

  addMoreRotatingScheduleRow() {
    this.courseSectionAddViewModel.courseVariableScheduleList.push(new CourseVariableSchedule());
   
    this.divCount.push(2); // Why 2? We have to fill up the divCount, It could be anything.
  }

  deleteRow(indexOfDynamicRow){
    this.divCount.splice(indexOfDynamicRow, 1);
    this.courseSectionAddViewModel.courseVariableScheduleList.splice(indexOfDynamicRow, 1);
    this.selectedBlocks.splice(indexOfDynamicRow, 1);
    this.selectedPeriod.splice(indexOfDynamicRow, 1);
  }

  getAllBlockList() {
    this.schoolPeriodService.getAllBlockList(this.blockListViewModel).subscribe(data => {
      if(data._failure){
        if(data._message==="NO RECORD FOUND"){
          this.periodList=[];      
          this.snackbar.open('NO RECORD FOUND. ', '', {
            duration: 1000
          });        
        }
      }else{    
        this.blockListViewModel = data;     
        if(data.getBlockListForView.length >0){
          this.periodList = data.getBlockListForView[0].blockPeriod;
          
        }    
      }
    });
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
           
          }
        }
      })
  }

 

  sendBlockScheduleDataToParent(){
    
    this.currentForm.form.markAllAsTouched();
    if (this.currentForm.form.invalid) {
      return
    }
    var error = false;
    var arr = [];
    for(var i=0;i<this.courseSectionAddViewModel.courseVariableScheduleList.length;i++){   
      arr[0] = this.courseSectionAddViewModel.courseVariableScheduleList[i];      
      for(var j=i+1;j<this.courseSectionAddViewModel.courseVariableScheduleList.length;j++){
        if(arr[0].day == this.courseSectionAddViewModel.courseVariableScheduleList[j].day && arr[0].periodId == this.courseSectionAddViewModel.courseVariableScheduleList[j].periodId){
          error = true;
        }else{
          error = false;
        }
      }
    }
    this.variableScheduleData.emit({scheduleType:'variableSchedule',scheduleDetails:this.courseSectionAddViewModel.courseVariableScheduleList,error:error});
  }
  

}
