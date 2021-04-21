import { Component, OnInit } from '@angular/core';
import { fadeInRight400ms } from '../../../../../@vex/animations/fade-in-right.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { TranslateService } from '@ngx-translate/core';
import { TakeAttendanceList } from '../../../../models/take-attendance-list.model';
import { Router} from '@angular/router';

@Component({
  selector: 'vex-input-effort-grades',
  templateUrl: './input-effort-grades.component.html',
  styleUrls: ['./input-effort-grades.component.scss'],
  animations: [
    fadeInRight400ms,
    stagger60ms,
    fadeInUp400ms
  ]
})
export class InputEffortGradesComponent implements OnInit {

  pageStatus = "Teacher Function";
  totalCount:number=0;
  pageSize:number;
  pageInit = 1;

  takeAttendanceList: TakeAttendanceList[] = [
    {name: 'Danielle Boucher', staffID: 1, openSisProfile: 'Teacher', jobTitle: 'Head Teacher', schoolEmail: 'danielle.boucher@example.com', mobilePhone: 123456789},
    {name: 'Andrew Brown', staffID: 2, openSisProfile: 'Teacher', jobTitle: 'Asst. Teacher', schoolEmail: 'andrew.brown@example.com', mobilePhone: 123456789},
    {name: 'Ella Brown', staffID: 3, openSisProfile: 'Teacher', jobTitle: 'Asst. Teacher', schoolEmail: 'ella_.brown@example.com', mobilePhone: 123456789},
    {name: 'Lian Fang', staffID: 4, openSisProfile: 'Teacher', jobTitle: 'Asst. Teacher', schoolEmail: 'lian.fang@example.com', mobilePhone: 123456789},
    {name: 'Adriana Garcia', staffID: 5, openSisProfile: 'Teacher', jobTitle: 'Asst. Teacher', schoolEmail: 'adriana.garcia@example.com', mobilePhone: 123456789},
    {name: 'Olivia Jones', staffID: 6, openSisProfile: 'Teacher', jobTitle: 'Asst. Teacher', schoolEmail: 'olivia.jones@example.com', mobilePhone: 123456789},
    {name: 'Amare Keita', staffID: 7, openSisProfile: 'Teacher', jobTitle: 'Asst. Teacher', schoolEmail: 'amare.keita@example.com', mobilePhone: 123456789},
    {name: 'Amber Keita', staffID: 8, openSisProfile: 'Teacher', jobTitle: 'Asst. Teacher', schoolEmail: 'amber.keita@example.com', mobilePhone: 123456789},
    {name: 'Alyssa Kimathi', staffID: 9, openSisProfile: 'Teacher', jobTitle: 'Asst. Teacher', schoolEmail: 'alyssa.kimathi@example.com', mobilePhone: 123456789},
    {name: 'Robert Millar', staffID: 10, openSisProfile: 'Teacher', jobTitle: 'Asst. Teacher', schoolEmail: 'robert.millar@example.com', mobilePhone: 123456789},
  ];
  displayedColumns: string[] = ['name', 'staffID', 'openSisProfile', 'jobTitle', 'schoolEmail', 'mobilePhone'];

  constructor(public translateService:TranslateService, private router: Router) { 
    translateService.use('en');
  }

  viewEffortGradeDetails() {
    // this.router.navigate(["school/teacher-functions/input-effort-grades/grade-details"]); 
    this.pageInit = 2;
  }

  ngOnInit(): void {
  }

}
