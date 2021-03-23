import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AddCourseSectionComponent } from './add-course-section/add-course-section.component';
import icClose from '@iconify/icons-ic/twotone-close';
import { StudentDetails } from '../../../models/studentDetailsModel';
import { fadeInUp400ms } from '../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../@vex/animations/stagger.animation';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'vex-group-drop',
  templateUrl: './group-drop.component.html',
  styleUrls: ['./group-drop.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class GroupDropComponent implements OnInit {
  icClose = icClose;
  selectDropDate: string;
  courseSectionData ;
  showcourseSectionCount: boolean;
  studentDetails: StudentDetails[] = [
    {studentSelected: true, studentName: 'Danielle Boucher', studentId: '12', alternateId: 'STD0012', gradeLevel: 'Grade 9', section: 'Section A', phoneNumber: '3217984560', action: 'Active'},
    {studentSelected: false, studentName: 'Andrew Brown', studentId: '15', alternateId: 'STD0012', gradeLevel: 'Grade 10', section: 'Section B', phoneNumber: '4446534672', action: 'Active'},
    {studentSelected: true, studentName: 'Lian Fang', studentId: '35', alternateId: 'STD0035', gradeLevel: 'Grade 11', section: 'Section A', phoneNumber: '4560986424', action: 'Active'},
    {studentSelected: true, studentName: 'James Miller', studentId: '102', alternateId: 'STD00103', gradeLevel: 'Grade 8', section: 'Section A', phoneNumber: '3217984560', action: 'Active'},
    {studentSelected: false, studentName: 'olivia Smith', studentId: '67', alternateId: 'STD0052', gradeLevel: 'Grade 10', section: 'Section B', phoneNumber: '4446534672', action: 'Active'},
    {studentSelected: true, studentName: 'Amelia Jones', studentId: '24', alternateId: 'STD0015', gradeLevel: 'Grade 9', section: 'Section A', phoneNumber: '4560986424', action: 'Active'}
  ];
  displayedColumns: string[] = ['studentSelected', 'studentName', 'studentId', 'alternateId', 'gradeLevel', 'section', 'phoneNumber', 'action'];

  constructor(private dialog: MatDialog, public translateService:TranslateService) { 
    translateService.use('en');
   }

  ngOnInit(): void {
  }

  selectCourseSection(){
    this.dialog.open(AddCourseSectionComponent, {
      width: '900px'
    }).afterClosed().subscribe((data) => {
      this.courseSectionData = data;
      if (this.courseSectionData !== null) {
        this.showcourseSectionCount = true;
      }
    });
  }
}
