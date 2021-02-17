import { Component, OnInit } from '@angular/core';
import { SearchStudentCourseSection } from '../../../../models/searchStudentCourseSectionModel';
import icClose from '@iconify/icons-ic/twotone-close';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'vex-add-course-section',
  templateUrl: './add-course-section.component.html',
  styleUrls: ['./add-course-section.component.scss']
})
export class AddCourseSectionComponent implements OnInit {

  icClose = icClose;
  teacherDetails: SearchStudentCourseSection[] = [
    {courseSelected: true, course: 'Geometry', courseSection: 'GEOM001', markingPeriod: 'Quarter 1', startDate: '01/01/2021', endDate: '03/31/2021', seats: 50, available: 30},
    {courseSelected: false, course: 'Geometry', courseSection: 'GEOM002', markingPeriod: 'Semester 1', startDate: '01/01/2021', endDate: '07/31/2021', seats: 50, available: 0},
    {courseSelected: true, course: 'Geometry', courseSection: 'GEOM003', markingPeriod: 'Custom', startDate: '02/15/2021', endDate: '06/10/2021', seats: 50, available: 25}
  ];
  displayedColumns: string[] = ['courseSelected', 'course', 'courseSection', 'markingPeriod', 'startDate', 'endDate', 'seats', 'available'];

  constructor(public translateService:TranslateService) {
    translateService.use('en');
  }

  ngOnInit(): void {
  }

}
