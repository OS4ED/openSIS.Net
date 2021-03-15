import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { AddTeacherComponent } from './add-teacher/add-teacher.component';
import { AddCourseComponent } from './add-course/add-course.component';

@Component({
  selector: 'vex-teacher-reassignment',
  templateUrl: './teacher-reassignment.component.html',
  styleUrls: ['./teacher-reassignment.component.scss']
})
export class TeacherReassignmentComponent implements OnInit {

  constructor(private dialog: MatDialog, public translateService:TranslateService) { 
    translateService.use('en');
  }

  ngOnInit(): void {
  }

  selectTeacher(){
    this.dialog.open(AddTeacherComponent, {
      width: '900px'
    });
  }
  selectCourse(){
    this.dialog.open(AddCourseComponent, {
      width: '900px'
    });
  }

}
