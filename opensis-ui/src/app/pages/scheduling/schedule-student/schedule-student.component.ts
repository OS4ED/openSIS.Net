import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AddStudentComponent } from './add-student/add-student.component';
import { AddCourseSectionComponent } from './add-course-section/add-course-section.component';

@Component({
  selector: 'vex-schedule-student',
  templateUrl: './schedule-student.component.html',
  styleUrls: ['./schedule-student.component.scss']
})
export class ScheduleStudentComponent implements OnInit {
  constructor(private dialog: MatDialog) {
    
   }

  ngOnInit(): void {
  }

  selectStudent(){
    this.dialog.open(AddStudentComponent, {
      width: '900px'
    });
  }

  selectCourseSection(){
    this.dialog.open(AddCourseSectionComponent, {
      width: '900px'
    });
  }

}
