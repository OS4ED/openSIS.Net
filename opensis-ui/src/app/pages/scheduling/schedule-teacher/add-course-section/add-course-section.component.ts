import { Component, OnInit } from '@angular/core';
import { SearchCourseSection } from '../../../../models/courseSectionModel';
import icClose from '@iconify/icons-ic/twotone-close';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'vex-add-course-section',
  templateUrl: './add-course-section.component.html',
  styleUrls: ['./add-course-section.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class AddCourseSectionComponent implements OnInit {

  icClose = icClose;
  teacherDetails: SearchCourseSection[] = [
    {staffSelected: true, course: 'Geometry', courseSectionName: 'GEOM001', markingPeriod: 'Quarter 1', startDate: '01/01/2021', endDate: '03/31/2021', scheduledTeacher: false},
    {staffSelected: false, course: 'Geometry', courseSectionName: 'GEOM002', markingPeriod: 'Semester 1', startDate: '01/01/2021', endDate: '07/31/2021', scheduledTeacher: true},
    {staffSelected: true, course: 'Geometry', courseSectionName: 'GEOM003', markingPeriod: 'Custom', startDate: '02/15/2021', endDate: '06/10/2021', scheduledTeacher: false}
  ];
  displayedColumns: string[] = ['staffSelected', 'course', 'courseSectionName', 'markingPeriod', 'startDate', 'endDate', 'scheduledTeacher'];

  constructor(public translateService:TranslateService) {
    translateService.use('en');
  }

  ngOnInit(): void {
  }

}
