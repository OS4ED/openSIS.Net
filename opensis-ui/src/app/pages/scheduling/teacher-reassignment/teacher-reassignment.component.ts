import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { AddTeacherComponent } from './add-teacher/add-teacher.component';
import { AddCourseComponent } from './add-course/add-course.component';
import { StaffListModel } from '../../../models/staffModel';
import { CourseModel } from '../../../models/courseManagerModel';
import { TeacherScheduleService } from '../../../services/teacher-schedule.service';
import { AllScheduledCourseSectionForStaffModel, StaffScheduleView, StaffScheduleViewModel } from '../../../models/teacher-schedule.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { GetMarkingPeriodTitleListModel } from '../../../models/markingPeriodModel';
import { MarkingPeriodService } from '../../../services/marking-period.service';
import { CourseSectionService } from '../../../services/course-section.service';
import { ScheduledStaffForCourseSection } from '../../../models/courseSectionModel';
import { map } from 'rxjs/operators';

@Component({
  selector: 'vex-teacher-reassignment',
  templateUrl: './teacher-reassignment.component.html',
  styleUrls: ['./teacher-reassignment.component.scss']
})
export class TeacherReassignmentComponent implements OnInit {

  constructor(private dialog: MatDialog, 
    public translateService: TranslateService,
    private teacherReassignmentService:TeacherScheduleService,
    private snackbar:MatSnackBar,
    private markingPeriodService:MarkingPeriodService,
    private courseSectionService:CourseSectionService,
    private staffScheduleService:TeacherScheduleService) {
    translateService.use('en');
  }
  selectedCurrentTeacher: StaffListModel;
  selectedNewTeacher: StaffListModel;
  selectedCourse: CourseModel;
  allScheduledCourseSectionBasedOnTeacher:AllScheduledCourseSectionForStaffModel = new AllScheduledCourseSectionForStaffModel();
  checkAvailibilityBasedOnTeacher:StaffScheduleViewModel = new StaffScheduleViewModel();
  teacherReassignmentBasedOnTeacher=false;
  teacherReassignmentBasedOnCourse=false;
  checkAvailabilityLoader=false;
  checkAvailabilityFinished=false;
  noConflictDetected=false;
  isTeacherReassignPossible=false;
  isAllCourseSectionConflicted=false;
  teacherReassigningLoader=false;
  getMarkingPeriodTitleListModel: GetMarkingPeriodTitleListModel = new GetMarkingPeriodTitleListModel();
  weeks = [
    { name: 'Sun',id: 0 },
    { name: 'Mon',id: 1 },
    { name: 'Tue', id: 2 },
    { name: 'Wed',id: 3 },
    { name: 'Thu',id: 4 },
    { name: 'Fri', id: 5 },
    { name: 'Sat',id: 6 }
];
  isAllCoursesChecked=true;
  allScheduledTeacherBasedOnCourse:ScheduledStaffForCourseSection = new ScheduledStaffForCourseSection();
  checkAvailibilityBasedOnCourse:ScheduledStaffForCourseSection = new ScheduledStaffForCourseSection();

  disableMasterCheckboxBasedOnTeacherConflict=false;
  teacherReassigning=false;
  ngOnInit(): void {
    this.getAllMarkingPeriodList();
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


  selectTeacher(teacherType) {
  // Im assuming currentTeacher as 0 and New Teacher as 1.
    this.noConflictDetected=false;
    this.isTeacherReassignPossible=false;
    this.selectedNewTeacher=null;
    this.teacherReassigning=false;
    if(!this.teacherReassignmentBasedOnCourse){
      this.selectedCourse = null;
    }
    this.isAllCourseSectionConflicted=false;
    this.checkAvailabilityFinished=false;
    if(teacherType!=1){
      this.teacherReassignmentBasedOnCourse=false;
      this.teacherReassignmentBasedOnTeacher=false;
      this.selectedCourse=null;
    }
   
    this.dialog.open(AddTeacherComponent, {
      width: '900px'
    }).afterClosed().subscribe((res) => {
      if (teacherType === 0) {
        this.isAllCoursesChecked=true;
        this.disableMasterCheckboxBasedOnTeacherConflict=false;
        this.selectedCurrentTeacher = res;
        this.allScheduledCourseSectionBasedOnTeacher.staffId=this.selectedCurrentTeacher.staffId;
        this.getAllScheduledCourseSectionBasedOnTeacher();
      } else {
        this.selectedNewTeacher = res;
        this.checkAllCoursesBasedOnTeacher();
      }
    });
  }

  selectCourse() {
    this.noConflictDetected=false;
    this.selectedCurrentTeacher = null;
    this.teacherReassignmentBasedOnCourse=false;
    this.teacherReassignmentBasedOnTeacher=false;
    this.isTeacherReassignPossible=false;
    this.isAllCourseSectionConflicted=false;
    this.checkAvailabilityFinished=false;
    this.selectedNewTeacher=null;
    this.teacherReassigning=false;
    this.dialog.open(AddCourseComponent, {
      width: '900px'
    }).afterClosed().subscribe((res) => {
      this.selectedCourse = res;

      this.getScheduledTeachersBasedOnCourse();
    });
  }

  getAllScheduledCourseSectionBasedOnTeacher(){
    this.allScheduledCourseSectionBasedOnTeacher.courseSectionViewList=null;
    this.teacherReassignmentService.getAllScheduledCourseSectionForStaff(this.allScheduledCourseSectionBasedOnTeacher).pipe(
      map((res)=>{
        res._userName=sessionStorage.getItem('user');
        return res;
      })
      ).subscribe((res)=>{
      if (typeof (res) == 'undefined') {       
        this.snackbar.open('Scheduled Course Section Failed ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (res._failure) { 
            if(res.courseSectionViewList==null){
              this.snackbar.open(res._message, '', {
                duration: 10000
              });
              this.allScheduledCourseSectionBasedOnTeacher.courseSectionViewList=[]
            }else{
              this.allScheduledCourseSectionBasedOnTeacher.courseSectionViewList=[]
            }
        } else {
          this.allScheduledCourseSectionBasedOnTeacher=res;
          this.allScheduledCourseSectionBasedOnTeacher.courseSectionViewList=this.findMarkingPeriodTitleById(this.allScheduledCourseSectionBasedOnTeacher.courseSectionViewList);
          this.teacherReassignmentBasedOnTeacher=true;
          this.teacherReassignmentBasedOnCourse=false;
        }
      }
    })
  }


  getScheduledTeachersBasedOnCourse(){
    this.allScheduledTeacherBasedOnCourse.courseSectionsList=[]
    this.allScheduledTeacherBasedOnCourse.courseId=this.selectedCourse.courseId;
    this.courseSectionService.getAllStaffScheduleInCourseSection(this.allScheduledTeacherBasedOnCourse).pipe(
      map((res)=>{
        res.courseSectionsList=res.courseSectionsList.filter((item)=>{
          return item.staffCoursesectionSchedule.length>0
        })
        return res;
      })
    ).subscribe((res)=>{
      if (typeof (res) == 'undefined') {       
        this.snackbar.open('Course Sections Failed ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (res._failure) { 
            if(res.courseSectionsList==null){
              this.snackbar.open(res._message, '', {
                duration: 10000
              });
              this.allScheduledTeacherBasedOnCourse.courseSectionsList=[]
            }else{
              this.allScheduledTeacherBasedOnCourse.courseSectionsList=[]
            }
        } else {
          this.allScheduledTeacherBasedOnCourse.courseSectionsList=res.courseSectionsList;
          this.moveLanguageNames();
          this.findMarkingPeriodTitle();
          this.teacherReassignmentBasedOnCourse=true;
          this.teacherReassignmentBasedOnTeacher=false;
        }
      }
    })
  }

  moveLanguageNames(){
    this.allScheduledTeacherBasedOnCourse.courseSectionsList.map((courseSection)=>{
      courseSection.staffCoursesectionSchedule?.forEach((staff)=>{
          if(staff.staffMaster.firstLanguage){
            staff.staffMaster.firstLanguageName=staff.staffMaster.firstLanguageNavigation.locale;
          }
          if(staff.staffMaster.secondLanguage){
            staff.staffMaster.secondLanguageName=staff.staffMaster.secondLanguageNavigation.locale;
          }
          if(staff.staffMaster.thirdLanguage){
            staff.staffMaster.thirdLanguageName=staff.staffMaster.thirdLanguageNavigation.locale;
          }
      })
    })
  }

  checkAvailability(){
    if(this.teacherReassignmentBasedOnTeacher){
      this.checkAvailabilityBasedOnTeacher();
    }else if(this.teacherReassignmentBasedOnCourse){
      this.checkAvailabilityBasedOnCourses();
    }
  }

  checkAvailabilityBasedOnTeacher() {
    this.checkAvailibilityBasedOnTeacher.existingStaff=this.allScheduledCourseSectionBasedOnTeacher.staffId;
    this.checkAvailibilityBasedOnTeacher.staffScheduleViewList=[new StaffScheduleView()]
    this.checkAvailibilityBasedOnTeacher.staffScheduleViewList[0].staffId=this.selectedNewTeacher.staffId;
    this.checkAvailibilityBasedOnTeacher.staffScheduleViewList[0].courseSectionViewList=this.onlySendCheckedCourse(this.allScheduledCourseSectionBasedOnTeacher.courseSectionViewList);
    this.checkAvailibilityBasedOnTeacher.staffScheduleViewList[0].courseSectionViewList=this.removeIdentifierFromMarkingPeriods(this.checkAvailibilityBasedOnTeacher.staffScheduleViewList[0].courseSectionViewList);
    this.checkAvailibilityBasedOnTeacher._failure=false;
    if(this.checkAvailibilityBasedOnTeacher.staffScheduleViewList[0].courseSectionViewList.length==0){
      this.snackbar.open('Select Course Section for Schedule Checking', '', {
        duration: 5000
      });
      return;
    }
    this.checkAvailabilityLoader=true;
    this.staffScheduleService.checkAvailabilityStaffCourseSectionSchedule(this.checkAvailibilityBasedOnTeacher).subscribe((res) => {
      if (typeof (res) == 'undefined') {
        this.checkAvailibilityBasedOnTeacher.staffScheduleViewList = [];
      }
      else {
        if (res._failure) {
          if (res.staffScheduleViewList == null) {
            this.snackbar.open(res._message, '', {
              duration: 5000
            });
          } else {
            this.noConflictDetected=false;
            this.checkAvailibilityBasedOnTeacher=res;
            this.checkForConflicts(this.checkAvailibilityBasedOnTeacher.staffScheduleViewList[0].courseSectionViewList);
            if(this.checkAvailibilityBasedOnTeacher.staffScheduleViewList[0].conflictStaff){
              this.isAllCoursesChecked=false;
              this.disableMasterCheckboxBasedOnTeacherConflict=true;
            }
            this.checkIfReassignPossibleOrNot();
          }
        } else {
          this.noConflictDetected=true;
          this.checkAvailibilityBasedOnTeacher=res;
          this.checkForConflicts(this.checkAvailibilityBasedOnTeacher.staffScheduleViewList[0].courseSectionViewList);
          this.checkIfReassignPossibleOrNot();
        }
      }
      this.checkAvailabilityFinished=true
      this.checkAvailabilityLoader=false;
    });
  }

  onlySendCheckedCourse(courseSectionList){
    courseSectionList=courseSectionList.filter(item=>item.checked);

    return courseSectionList;
  }

  checkForConflicts(courseSectionViewList){

    for(let courseSectionInView of this.allScheduledCourseSectionBasedOnTeacher.courseSectionViewList){
        for(let conflictCourseSection of courseSectionViewList){
          if(courseSectionInView.courseSectionId===conflictCourseSection.courseSectionId){
            courseSectionInView.checked=!conflictCourseSection.conflictCourseSection;
            courseSectionInView.conflictCourseSection=conflictCourseSection.conflictCourseSection
          }
        }
    }
    this.isAllCourseSectionConflicted=this.allScheduledCourseSectionBasedOnTeacher.courseSectionViewList.every((item=>item.conflictCourseSection))
  }

  findMarkingPeriodTitle(){
    this.allScheduledTeacherBasedOnCourse.courseSectionsList?.map((item)=>{
        if(item.quarters){
          item.mpTitle=item.quarters.title;
        }else if(item.schoolYears){
          item.mpTitle=item.schoolYears.title;
        }else if(item.semesters){
          item.mpTitle=item.semesters.title;
        }else{
          item.mpTitle='Custom'
        }
    })
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

  checkAllCoursesBasedOnTeacher(){
    this.allScheduledCourseSectionBasedOnTeacher?.courseSectionViewList?.forEach((item,i)=>{
      this.singleSelectionBasedOnTeacher(true,i)
      item.conflictCourseSection=false;
      this.disableMasterCheckboxBasedOnTeacherConflict=false;
    })
  }

  checkIfReassignPossibleOrNot(){
    this.isTeacherReassignPossible=this.allScheduledCourseSectionBasedOnTeacher.courseSectionViewList.some((item=>!item.conflictCourseSection))

  }


  reassignTeacherScheduleBasedOnTeacher(){
    let reassignTeacherSchedule:StaffScheduleViewModel = new StaffScheduleViewModel();
    reassignTeacherSchedule=JSON.parse(JSON.stringify(this.checkAvailibilityBasedOnTeacher))
    reassignTeacherSchedule.staffScheduleViewList[0].courseSectionViewList=this.allScheduledCourseSectionBasedOnTeacher.courseSectionViewList.filter((item)=>{
      return item.checked;
    })

    this.teacherReassigning = true;
    this.teacherReassigningLoader=true;

    this.staffScheduleService.AddStaffCourseSectionReSchedule(reassignTeacherSchedule).subscribe((res)=>{
      if (typeof (res) == 'undefined') {
        reassignTeacherSchedule.staffScheduleViewList = [];
      }
      else {
        if (res._failure) {
            this.teacherReassigning = false;

          if (res.staffScheduleViewList == null) {
            this.snackbar.open(res._message, '', {
              duration: 5000
            });
          } else {
            this.snackbar.open(res._message, '', {
              duration: 5000
            });
          }
        } else {
          this.resetAll();
        }

      }
    this.teacherReassigningLoader=false;
    })
  }


  singleSelectionBasedOnTeacher(event,indexOfCourseSection){
      this.allScheduledCourseSectionBasedOnTeacher.courseSectionViewList[indexOfCourseSection].checked=event;
      this.isAllCoursesChecked=this.allScheduledCourseSectionBasedOnTeacher.courseSectionViewList.every((course)=>{
          return course.checked;
        });
  }
  masterCheckToggleBasedOnTeacher(event){
    this.allScheduledCourseSectionBasedOnTeacher.courseSectionViewList.forEach((item)=>{
      item.checked=event;
    });
    this.isAllCoursesChecked=event;
  }

  checkAvailabilityBasedOnCourses(){
    this.checkAvailibilityBasedOnCourse.courseSectionsList = JSON.parse(JSON.stringify(this.allScheduledTeacherBasedOnCourse.courseSectionsList));
    this.checkAvailibilityBasedOnCourse.reScheduleStaffId = this.selectedNewTeacher.staffId;
    this.onlySendCheckedStaffBasedOnCourse();
    let noStaffSelected=this.checkAvailibilityBasedOnCourse.courseSectionsList.every(item=>item.staffCoursesectionSchedule.length==0);
    if(noStaffSelected){
      this.snackbar.open('Please select at least one teacher', '', {
        duration: 5000
      });
      return 
    }
    console.log(this.checkAvailibilityBasedOnCourse);
    this.staffScheduleService.checkAvailabilityStaffCourseSectionReSchedule(this.checkAvailibilityBasedOnCourse).subscribe((res)=>{
      if (typeof (res) == 'undefined') {
        this.checkAvailibilityBasedOnCourse.courseSectionsList = [];
      }
      else {
        if (res._failure) {
          if (res.courseSectionsList == null) {
            this.snackbar.open(res._message, '', {
              duration: 5000
            });
          } else {
            this.noConflictDetected=false;
            this.checkAvailibilityBasedOnCourse=res;
          this.manipulateView();
            this.checkForConflictsBasedOnCourse();
          console.log(this.checkAvailibilityBasedOnCourse);
          }
        } else {
          this.noConflictDetected=true;
          this.checkAvailibilityBasedOnCourse=res;
          this.manipulateView();
          this.checkForConflictsBasedOnCourse();
          console.log(this.checkAvailibilityBasedOnCourse);
        }
      }
    })
  }

  manipulateView(){
      for(let [courseSectionIndex,courseSection] of this.allScheduledTeacherBasedOnCourse.courseSectionsList.entries()){
          if(courseSection.staffCoursesectionSchedule.length>0){
            for(let staff of courseSection.staffCoursesectionSchedule){
              if(staff.checked){
                let matchedStaffIndex=this.checkAvailibilityBasedOnCourse.courseSectionsList[courseSectionIndex].staffCoursesectionSchedule.findIndex(item=>item.staffId==+staff.staffId);
                this.checkAvailibilityBasedOnCourse.courseSectionsList[courseSectionIndex].staffCoursesectionSchedule[matchedStaffIndex].checked=true;
              }
            }
            
          }
      }
  }
  checkForConflictsBasedOnCourse(){
    if(this.checkAvailibilityBasedOnCourse.conflictIndexNo?.trim()){
      let conflictedIndex=[]
      conflictedIndex=this.checkAvailibilityBasedOnCourse.conflictIndexNo.split(',');
      for(let [courseSectionIndex,conflictCourseSection] of conflictedIndex){
        for(let staff of this.checkAvailibilityBasedOnCourse.courseSectionsList[+conflictCourseSection].staffCoursesectionSchedule){
            if(staff.checked){
              let matchedStaffIndex=this.allScheduledTeacherBasedOnCourse.courseSectionsList[courseSectionIndex].staffCoursesectionSchedule.findIndex(item=>item.staffId==+staff.staffId);
                this.allScheduledTeacherBasedOnCourse.courseSectionsList[courseSectionIndex].staffCoursesectionSchedule[matchedStaffIndex].conflict=true;
              staff.conflict=true;
              break;
            }
        }
        
      }
    }
  }

  onlySendCheckedStaffBasedOnCourse(){
    this.checkAvailibilityBasedOnCourse.courseSectionsList=this.checkAvailibilityBasedOnCourse.courseSectionsList.map((staffList)=>{
      staffList.staffCoursesectionSchedule=staffList.staffCoursesectionSchedule.filter(staff=>staff.checked);
      return staffList;
    })
  }

  singleSelectionBasedOnCourse(event,courseSectionIndex,checkedStaffIndex){
      this.allScheduledTeacherBasedOnCourse.courseSectionsList[courseSectionIndex].staffCoursesectionSchedule[checkedStaffIndex].checked=true;

      for(let [i,staff] of this.allScheduledTeacherBasedOnCourse.courseSectionsList[courseSectionIndex].staffCoursesectionSchedule.entries()){
        if(i!=checkedStaffIndex){
          staff.checked=false
        }
      }
  }


  resetAll(){
    this.noConflictDetected=false;
    this.selectedCurrentTeacher = null;
    this.teacherReassignmentBasedOnCourse=false;
    this.teacherReassignmentBasedOnTeacher=false;
    this.isTeacherReassignPossible=false;
    this.isAllCourseSectionConflicted=false;
    this.checkAvailabilityFinished=false;
    this.selectedCourse = null;
    this.disableMasterCheckboxBasedOnTeacherConflict=false;
  }

}
