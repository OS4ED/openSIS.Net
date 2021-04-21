import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { fadeInRight400ms } from '../../../../@vex/animations/fade-in-right.animation';
import { fadeInUp400ms } from '../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../@vex/animations/stagger.animation';
import { InputEffortGradesComponent } from './input-effort-grades/input-effort-grades.component';
import { InputFinalGradeComponent } from './input-final-grade/input-final-grade.component';
import { TakeAttendanceComponent } from './take-attendance/take-attendance.component';

@Component({
  selector: 'vex-teacher-function',
  templateUrl: './teacher-function.component.html',
  styleUrls: ['./teacher-function.component.scss'],
  animations: [
    fadeInRight400ms,
    stagger60ms,
    fadeInUp400ms
  ]
})

export class TeacherFunctionComponent implements OnInit {

  //currentCategory: number = 1;
  gradeComponent: any;
  gradeComponentListCat = [InputFinalGradeComponent, InputEffortGradesComponent];
  takeAttendance = [TakeAttendanceComponent]

  constructor(public translateService:TranslateService, private router: Router) { 
    translateService.use('en');
  }

  ngOnInit(): void {
    this.gradeComponent = this.gradeComponentListCat[0];
  }

  changeCategory(step:number = 0){
    this.gradeComponent = this.gradeComponentListCat[step];
  }
}
