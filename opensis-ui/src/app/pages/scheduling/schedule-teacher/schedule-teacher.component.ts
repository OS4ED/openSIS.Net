import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AddTeacherComponent } from './add-teacher/add-teacher.component';
import { AddCourseSectionComponent } from './add-course-section/add-course-section.component';
import { TranslateService } from '@ngx-translate/core';
import { AddDaysScheduleComponent } from './add-days-schedule/add-days-schedule.component';
import { TeacherScheduleService } from '../../../services/teacher-schedule.service';
import { CourseSectionList, StaffScheduleView, StaffScheduleViewModel } from '../../../models/teacher-schedule.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { GetMarkingPeriodTitleListModel } from '../../../models/markingPeriodModel';
import { MarkingPeriodService } from '../../../services/marking-period.service';
import { LoaderService } from '../../../services/loader.service';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { stagger60ms } from '../../../../@vex/animations/stagger.animation';
import { fadeInUp400ms } from '../../../../@vex/animations/fade-in-up.animation';

@Component({
  selector: 'vex-schedule-teacher',
  templateUrl: './schedule-teacher.component.html',
  animations: [
    stagger60ms,
    fadeInUp400ms
  ], 
  styleUrls: ['./schedule-teacher.component.scss']
})
export class ScheduleTeacherComponent implements OnInit,OnDestroy {
  selectedTeachers=[]
  selectedCourseSection=[]
  staffScheduleView:StaffScheduleViewModel;
  staffScheduleListForView:StaffScheduleViewModel= new StaffScheduleViewModel();
  getMarkingPeriodTitleListModel: GetMarkingPeriodTitleListModel = new GetMarkingPeriodTitleListModel();
  isStartSchedulingPossible=false;
  weeks = [
    { name: 'Sun',id: 0 },
    { name: 'Mon',id: 1 },
    { name: 'Tue', id: 2 },
    { name: 'Wed',id: 3 },
    { name: 'Thu',id: 4 },
    { name: 'Fri', id: 5 },
    { name: 'Sat',id: 6 }
];
cloneStaffScheduleList:StaffScheduleViewModel;
cloneStaffScheduleListForCheckAvailibility:StaffScheduleViewModel;
staffSchedulingFinished=false;
checkAvailabilityLoader:boolean=false;
startSchedulingLoader:boolean=false;
globalLoader:boolean;
destroySubject$: Subject<void> = new Subject();
noConflictDetected:boolean=false;
checkAvailabilityFinished:boolean=false;
  constructor(private dialog: MatDialog, 
    private translateService:TranslateService,
    private staffScheduleService:TeacherScheduleService,
    private snackBar:MatSnackBar,
    private markingPeriodService: MarkingPeriodService,
    private loaderService: LoaderService) { 
    translateService.use('en');
    this.loaderService.isLoading.pipe(takeUntil(this.destroySubject$)).subscribe((val) => {
      this.globalLoader = val;
    });
  }

  ngOnInit(): void {
    this.getAllMarkingPeriodList();
  }

  selectTeacher(){
    this.dialog.open(AddTeacherComponent, {
      width: '900px'
    }).afterClosed().subscribe((res)=>{
      this.selectedTeachers=res?res:[];
      this.getTeacherScheduleView();
    });
  }

  selectCourseSection(){
    this.dialog.open(AddCourseSectionComponent, {
      data:{markingPeriods:this.getMarkingPeriodTitleListModel.getMarkingPeriodView},
      width: '900px'
    }).afterClosed().subscribe((res)=>{
      this.selectedCourseSection=res?res:[];
      this.getTeacherScheduleView();
    });
  }
  

  getTeacherScheduleView(){

    if(this.selectedTeachers.length>0 && this.selectedCourseSection.length>0){
      this.staffSchedulingFinished=false;
      this.isStartSchedulingPossible=false;
      this.checkAvailabilityFinished=false;
      this.checkAvailabilityLoader=false;
     this.collectValuesFromTeacherAndCourseSection() 
    this.staffScheduleService.staffScheduleViewForCourseSection(this.staffScheduleView).subscribe((res)=>{
      if (typeof (res) == 'undefined') {
        this.staffScheduleListForView.staffScheduleViewList = [];
      }
      else {
        if (res._failure) {
          if(res.staffScheduleViewList==null){
            this.snackBar.open( res._message, '', {
              duration: 5000
            });
           this.staffScheduleListForView.staffScheduleViewList = [];
          }else{
           this.staffScheduleListForView.staffScheduleViewList = [];
          }
        } else {
          this.staffScheduleListForView=JSON.parse(JSON.stringify(res));
          this.staffScheduleListForView.staffScheduleViewList=this.staffScheduleListForView.staffScheduleViewList.map((staffDetails)=>{
            staffDetails.oneOrMoreCourseSectionChecked=true;
            staffDetails.allCourseSectionChecked=true;
             staffDetails.courseSectionViewList=this.findMarkingPeriodTitleById(staffDetails.courseSectionViewList);
             return staffDetails;
          });
          this.staffScheduleListForView.staffScheduleViewList=this.staffScheduleListForView.staffScheduleViewList.map((item)=>{
            item.allCourseSectionChecked=item.courseSectionViewList.every((courseSection)=>{
              return courseSection.checked;
            });
            return item;
          })
          this.cloneStaffScheduleListForCheckAvailibility=JSON.parse(JSON.stringify(this.staffScheduleListForView));
        }
      }
    })
    }
 
  }



  collectValuesFromTeacherAndCourseSection(){
    let staffScheduleList:StaffScheduleView[]=[];
    this.selectedTeachers?.map((item,i)=>{
      staffScheduleList.push(new StaffScheduleView)
      staffScheduleList[i].staffId=item.staffId

    });

    let courseSectionList:CourseSectionList[]=[];
    this.selectedCourseSection?.map((item,i)=>{
      courseSectionList.push(new CourseSectionList)
      courseSectionList[i].courseSectionId=item.courseSectionId;
    });

    this.staffScheduleView=new StaffScheduleViewModel;
    this.staffScheduleView.staffScheduleViewList=staffScheduleList;
    this.staffScheduleView.courseSectionViewList=courseSectionList;
  }

  findMarkingPeriodTitleById(courseSectionList){

    courseSectionList=courseSectionList.map((item)=>{
      if(item.yrMarkingPeriodId){
        item.yrMarkingPeriodId='0_'+item.yrMarkingPeriodId;
      }else if(item.smstrMarkingPeriodId){
        item.smstrMarkingPeriodId='1_'+item.smstrMarkingPeriodId;
      }else if(item.qtrMarkingPeriodId){
        item.qtrMarkingPeriodId='2_'+item.qtrMarkingPeriodId;
      }
      
      if(item.yrMarkingPeriodId || item.smstrMarkingPeriodId || item.qtrMarkingPeriodId){
        for(let markingPeriod of this.getMarkingPeriodTitleListModel.getMarkingPeriodView){
          if(markingPeriod.value==item.yrMarkingPeriodId){
            item.markingPeriodTitle=markingPeriod.text;
            break;
          }else if(markingPeriod.value==item.smstrMarkingPeriodId){
            item.markingPeriodTitle=markingPeriod.text;
            break;
          }else if(markingPeriod.value==item.qtrMarkingPeriodId){
            item.markingPeriodTitle=markingPeriod.text;
            break;
          }
        }
      }else{
        item.markingPeriodTitle='Custom'
      }
      item.checked=true;
      if(item.scheduleType=='Variable Schedule'){
        let days=item.meetingDays.split('|')
        days.map((day)=>{
          for(let [i,weekDay] of this.weeks.entries()){
            if(weekDay.name==day.trim()){
              item.meetingDays=item.meetingDays+weekDay.id;
              break;
            }
          }
        })
      }
      return item;
    });
    return courseSectionList;
  }

  getAllMarkingPeriodList() {
    this.getMarkingPeriodTitleListModel.schoolId = +sessionStorage.getItem("selectedSchoolId");
    this.getMarkingPeriodTitleListModel.academicYear = +sessionStorage.getItem("academicyear");
    this.markingPeriodService.getAllMarkingPeriodList(this.getMarkingPeriodTitleListModel).subscribe(data => {
      if (data._failure) {
        this.getMarkingPeriodTitleListModel.getMarkingPeriodView = [];
      } else {
        this.getMarkingPeriodTitleListModel.getMarkingPeriodView = data.getMarkingPeriodView;
      }
    });
  }

  masterCheckToggle(event,index){
    this.staffScheduleListForView.staffScheduleViewList[index].courseSectionViewList.forEach((item)=>{
      item.checked=event;
    });

    this.cloneStaffScheduleListForCheckAvailibility.staffScheduleViewList[index].courseSectionViewList.forEach((item)=>{
      item.checked=event;
    });

  }

  singleSelection(event,indexOfCourseSection,indexOfStaffList){
    this.staffScheduleListForView.staffScheduleViewList[indexOfStaffList].courseSectionViewList[indexOfCourseSection].checked=event;
      this.staffScheduleListForView.staffScheduleViewList[indexOfStaffList].allCourseSectionChecked=this.staffScheduleListForView.staffScheduleViewList[indexOfStaffList].courseSectionViewList.every((courseSection)=>{
        return courseSection.checked;
      });

      this.cloneStaffScheduleListForCheckAvailibility.staffScheduleViewList[indexOfStaffList].courseSectionViewList[indexOfCourseSection].checked=event;
      this.cloneStaffScheduleListForCheckAvailibility.staffScheduleViewList[indexOfStaffList].allCourseSectionChecked=this.cloneStaffScheduleListForCheckAvailibility.staffScheduleViewList[indexOfStaffList].courseSectionViewList.every((courseSection)=>{
        return courseSection.checked;
      });
  }



  checkAvailability() {
    this.checkAvailabilityLoader=true;
    this.cloneStaffScheduleList=JSON.parse(JSON.stringify(this.cloneStaffScheduleListForCheckAvailibility));
    this.onlySendCheckedCourseSections();
    this.staffScheduleService.checkAvailabilityStaffCourseSectionSchedule(this.cloneStaffScheduleList).subscribe((res) => {
      if (typeof (res) == 'undefined') {
        this.cloneStaffScheduleList.staffScheduleViewList = [];
      }
      else {
        if (res._failure) {
          if (res.staffScheduleViewList == null) {
            this.snackBar.open(res._message, '', {
              duration: 5000
            });
          } else {
            this.noConflictDetected=false;
            this.cloneStaffScheduleList=res;
            this.checkForConflicts();
            this.checkIfStartSchedulingPossibleOrNot();
          }
        } else {
          this.noConflictDetected=true;
          this.cloneStaffScheduleList=res;
          this.checkForConflicts();
          this.checkIfStartSchedulingPossibleOrNot();
        }
      }
      this.checkAvailabilityFinished=true
      this.checkAvailabilityLoader=false;

      
    });
  }



  onlySendCheckedCourseSections(){
    this.cloneStaffScheduleList.staffScheduleViewList.map((staffDetails)=>{
      let oneOrMoreCourseSectionChecked=true;
      staffDetails.courseSectionViewList.map((item)=>{
        let falseCount=0;
        if(!item.checked){
          falseCount=falseCount+1;
        }
        if(falseCount==staffDetails.courseSectionViewList.length){
          oneOrMoreCourseSectionChecked=false;
        }
      });
      if(!oneOrMoreCourseSectionChecked){
        staffDetails.oneOrMoreCourseSectionChecked=false;
      }
    });

    this.cloneStaffScheduleList.staffScheduleViewList=this.cloneStaffScheduleList.staffScheduleViewList.filter((item)=>{
      return item.oneOrMoreCourseSectionChecked;
    })

    this.cloneStaffScheduleList.staffScheduleViewList=this.cloneStaffScheduleList.staffScheduleViewList.map((staffDetails)=>{
      staffDetails.courseSectionViewList=staffDetails.courseSectionViewList.filter((courseSection)=>{
        return courseSection.checked;
      });
      return staffDetails;
    });

    this.cloneStaffScheduleList.staffScheduleViewList=this.cloneStaffScheduleList.staffScheduleViewList.map((staffDetails)=>{
       staffDetails.courseSectionViewList=this.removeIdentifierFromMarkingPeriods(staffDetails.courseSectionViewList);
       return staffDetails;
    });
  } 

  uncheckTheConflicted(){
    this.cloneStaffScheduleListForCheckAvailibility.staffScheduleViewList=this.cloneStaffScheduleListForCheckAvailibility.staffScheduleViewList.map((staffDetails)=>{
      staffDetails.courseSectionViewList=staffDetails.courseSectionViewList.filter((courseSection)=>{
         return !courseSection.conflictCourseSection
      });
      return staffDetails;
    });
    this.cloneStaffScheduleListForCheckAvailibility.staffScheduleViewList=this.cloneStaffScheduleListForCheckAvailibility.staffScheduleViewList.filter((staffDetails)=>{
      return staffDetails.courseSectionViewList.length>0
    })
  }

  checkForConflicts(){
    for(let conflictedStaff of this.cloneStaffScheduleList.staffScheduleViewList){
      for(let viewStaff of this.staffScheduleListForView.staffScheduleViewList){
        if(conflictedStaff.staffId==viewStaff.staffId){
          viewStaff.conflictStaff=conflictedStaff.conflictStaff;
          
          for(let conflictedCourseSection of conflictedStaff.courseSectionViewList){
            for(let viewCourseSection of viewStaff.courseSectionViewList){
              if(conflictedCourseSection.courseSectionId==viewCourseSection.courseSectionId){
                viewCourseSection.conflictCourseSection=conflictedCourseSection.conflictCourseSection;
                conflictedCourseSection.checked=viewCourseSection.checked;
              }
            }
          }
        }
      }
    }

  }

  checkIfStartSchedulingPossibleOrNot(){
    let startSchedulingPossible=false;
    this.cloneStaffScheduleList.staffScheduleViewList.map((staffDetails)=>{
      staffDetails.courseSectionViewList.map((courseSection)=>{
          if(!courseSection.conflictCourseSection){
            startSchedulingPossible=true
          }
      })
    });

    if(startSchedulingPossible){
      this.isStartSchedulingPossible=true;
    }else{
      this.isStartSchedulingPossible=false;
    }
  }
  
  startScheduling(){
    this.startSchedulingLoader=true;
  let staffScheduleList = JSON.parse(JSON.stringify(this.cloneStaffScheduleList));
  staffScheduleList=this.removeConflictAndUncheckedRowsIfAny(staffScheduleList);
    this.staffScheduleService.addStaffCourseSectionSchedule(staffScheduleList).subscribe((res)=>{
      if (typeof (res) == 'undefined') {
        staffScheduleList.staffScheduleViewList = [];
      }
      else {
        if (res._failure) {
          if(res.staffScheduleViewList==null){
            this.snackBar.open( res._message, '', {
              duration: 5000
            });

          }else{
            this.snackBar.open( res._message, '', {
              duration: 5000
            });
          }
        } else {
          this.snackBar.open( res._message, '', {
            duration: 5000
          });
          this.staffSchedulingFinished=true;
          this.selectedTeachers=[]
          this.selectedCourseSection=[]
        }
      }
    this.startSchedulingLoader=false;
    })
  }

  removeConflictAndUncheckedRowsIfAny(staffScheduleList){
    let staffScheduleView=JSON.parse(JSON.stringify(this.staffScheduleListForView));
    staffScheduleList.staffScheduleViewList=staffScheduleView.staffScheduleViewList.map((staffDetails)=>{
      staffDetails.courseSectionViewList=this.removeIdentifierFromMarkingPeriods(staffDetails.courseSectionViewList);
      staffDetails.courseSectionViewList=staffDetails.courseSectionViewList.filter((courseSection)=>{
        return (courseSection.checked && !courseSection.conflictCourseSection)
      });
      return staffDetails;
    });
    staffScheduleList.staffScheduleViewList=staffScheduleList.staffScheduleViewList.filter((item)=>{
      return item.courseSectionViewList.length>0
    });
 
    return staffScheduleList;
  }

  removeIdentifierFromMarkingPeriods(courseSectionList){
    let courseSection=courseSectionList.map((item)=>{
      if(item.yrMarkingPeriodId){
         item.yrMarkingPeriodId=item.yrMarkingPeriodId.split('_')[1];
      }else if(item.smstrMarkingPeriodId){
       item.smstrMarkingPeriodId=item.smstrMarkingPeriodId.split('_')[1];
      }else if(item.qtrMarkingPeriodId){
        item.qtrMarkingPeriodId=item.qtrMarkingPeriodId.split('_')[1];
      }
      return item;
  })
      return courseSection;
  }


  viewCorrespondingSchedule(courseSectionDetails){
    this.dialog.open(AddDaysScheduleComponent, {
      width: '600px',
      data:courseSectionDetails
    });
  }


  ngOnDestroy(): void {
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }

}
