import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { StudentReenrollList } from '../../../models/studentReenrollListModel';
import { Router} from '@angular/router';
import { fadeInUp400ms } from '../../../../@vex/animations/fade-in-up.animation';
import { fadeInRight400ms } from '../../../../@vex/animations/fade-in-right.animation';
import { stagger40ms } from '../../../../@vex/animations/stagger.animation';

@Component({
  selector: 'vex-student-re-enroll',
  templateUrl: './student-re-enroll.component.html',
  styleUrls: ['./student-re-enroll.component.scss'],
  animations: [
    fadeInUp400ms,
    stagger40ms,
    fadeInRight400ms
  ]
})
export class StudentReEnrollComponent implements OnInit {

  totalCount:number=0;
  pageSize:number;
  showAdvanceSearchPanel: boolean = false;

  studentReenrollList: StudentReenrollList[] = [
    {studentCheck: true, studentName: 'Danny Anderson', studentId: 1, lastGradeLevel: 'Teacher', mobilePhone: 123456789, personalEmail: 'danielle.boucher@example.com', enrollmentDate: 'Mar 10, 2019', exitDate: 'Mar 10, 2019', exitCode: 'Transferred Out'},
    {studentCheck: true, studentName: 'Justin Aponte', studentId: 2, lastGradeLevel: 'Teacher', mobilePhone: 123456789, personalEmail: 'andrew.brown@example.com', enrollmentDate: 'Feb 10, 2018', exitDate: 'Feb 10, 2018', exitCode: 'Dropped Out'},
    {studentCheck: false, studentName: 'Ella Brown', studentId: 3, lastGradeLevel: 'Teacher', mobilePhone: 123456789, personalEmail: 'ella_.brown@example.com', enrollmentDate: 'Nov 10, 2019', exitDate: 'Nov 10, 2019', exitCode: 'Dropped Out'},
    {studentCheck: false, studentName: 'Lian Fang', studentId: 4, lastGradeLevel: 'Teacher', mobilePhone: 123456789, personalEmail: 'lian.fang@example.com', enrollmentDate: 'June 07, 2018', exitDate: 'June 07, 2018', exitCode: 'Transferred Out'},
    {studentCheck: true, studentName: 'Julie Davis', studentId: 5, lastGradeLevel: 'Teacher', mobilePhone: 123456789, personalEmail: 'adriana.garcia@example.com', enrollmentDate: 'Mar 10, 2018', exitDate: 'Mar 10, 2018', exitCode: 'Transferred Out'},
    {studentCheck: false, studentName: 'Olivia Jones', studentId: 6, lastGradeLevel: 'Teacher', mobilePhone: 123456789, personalEmail: 'olivia.jones@example.com', enrollmentDate: 'Nov 06, 2019', exitDate: 'Nov 06, 2019', exitCode: 'Dropped Out'},
    {studentCheck: false, studentName: 'Amare Keita', studentId: 7, lastGradeLevel: 'Teacher', mobilePhone: 123456789, personalEmail: 'amare.keita@example.com', enrollmentDate: 'Mar 10, 2019', exitDate: 'Mar 10, 2021', exitCode: 'Transferred Out'},
    {studentCheck: false, studentName: 'Amber Keita', studentId: 8, lastGradeLevel: 'Teacher', mobilePhone: 123456789, personalEmail: 'amber.keita@example.com', enrollmentDate: 'Nov 06, 2019', exitDate: 'Nov 06, 2019', exitCode: 'Dropped Out'},
    {studentCheck: false, studentName: 'Alyssa Kimathi', studentId: 9, lastGradeLevel: 'Teacher', mobilePhone: 123456789, personalEmail: 'alyssa.kimathi@example.com', enrollmentDate: 'Mar 10, 2019', exitDate: 'Mar 10, 2021', exitCode: 'Transferred Out'},
    {studentCheck: true, studentName: 'Colin Parker', studentId: 10, lastGradeLevel: 'Teacher', mobilePhone: 123456789, personalEmail: 'robert.millar@example.com', enrollmentDate: 'Nov 06, 2019', exitDate: 'Nov 06, 2019', exitCode: 'Dropped Out'},
  ];
  displayedColumns: string[] = ['studentCheck','studentName', 'studentId', 'lastGradeLevel', 'mobilePhone', 'personalEmail', 'enrollmentDate', 'exitDate', 'exitCode'];

  constructor(public translateService:TranslateService, private router: Router) { 
    translateService.use('en');
  }

  ngOnInit(): void {
  }
  
  showAdvanceSearch() {
    this.showAdvanceSearchPanel = true;
  }

  hideAdvanceSearch(event){
    this.showAdvanceSearchPanel = false;
    
  }

}
