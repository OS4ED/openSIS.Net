import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import icEmail from '@iconify/icons-ic/twotone-email';
import { ScheduledStaffForCourseSection } from '../../../../../models/courseSectionModel';
import { CourseSectionService } from '../../../../../services/course-section.service';

@Component({
  selector: 'vex-scheduled-teachers',
  templateUrl: './scheduled-teachers.component.html',
  styleUrls: ['./scheduled-teachers.component.scss']
})
export class ScheduledTeachersComponent implements OnInit {
  icEmail = icEmail;
  @Input() courseSectionDetails;
  constructor(private courseSectionService:CourseSectionService,
    private snackbar:MatSnackBar) { } 
  scheduledTeacher:ScheduledStaffForCourseSection = new ScheduledStaffForCourseSection();
  
  ngOnChanges(changes:SimpleChanges){
        this.getScheduledTeachers();
  }
  ngOnInit(): void {
  }

  getScheduledTeachers(){
    this.scheduledTeacher.courseId=this.courseSectionDetails.courseSection.courseId;
    this.scheduledTeacher.courseSectionId=this.courseSectionDetails.courseSection.courseSectionId;

    this.courseSectionService.getAllStaffScheduleInCourseSection(this.scheduledTeacher).subscribe((res)=>{
      if (typeof (res) == 'undefined') {       
        this.snackbar.open('Teacher Schedule Failed ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (res._failure) { 
            if(res.courseSectionsList==null){
              this.snackbar.open(res._message, '', {
                duration: 10000
              });
              this.scheduledTeacher.courseSectionsList=[]
            }else{
              this.scheduledTeacher.courseSectionsList=[]
            }
        } else {
          this.scheduledTeacher.courseSectionsList=res.courseSectionsList;
          this.scheduledTeacher.courseSectionsList[0].staffCoursesectionSchedule.forEach((item)=>{
            item.staffMaster.staffEmail=item.staffMaster.loginEmailAddress?item.staffMaster.loginEmailAddress
                            :item.staffMaster.personalEmail?item.staffMaster.personalEmail
                            :item.staffMaster.schoolEmail?item.staffMaster.schoolEmail:null
          })
        }
      }
    })
  }

}
