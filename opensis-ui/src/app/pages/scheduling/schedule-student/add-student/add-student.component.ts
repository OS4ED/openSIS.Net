import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import icClose from '@iconify/icons-ic/twotone-close';
import { StudentDetails } from '../../../../models/studentDetailsModel';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'vex-add-student',
  templateUrl: './add-student.component.html',
  styleUrls: ['./add-student.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class AddStudentComponent implements OnInit {

  icClose = icClose;
  studentDetails: StudentDetails[] = [
    {studentSelected: true, studentName: 'Danielle Boucher', studentId: 'ST4562', alternateId: 'Grade 9', gradeLevel: 'Mathematics', section: 'Section A', tbd: '-'},
    {studentSelected: false, studentName: 'Andrew Brown', studentId: 'ST756', alternateId: 'Grade 10', gradeLevel: 'Science', section: 'Section B', tbd: '-'},
    {studentSelected: true, studentName: 'Lian Fang', studentId: 'ST456', alternateId: 'Grade 9', gradeLevel: 'Mathematics', section: 'Section A', tbd: '-'}
  ];
  displayedColumns: string[] = ['studentSelected', 'studentName', 'studentId', 'alternateId', 'gradeLevel', 'section', 'tbd'];


  constructor(private dialogRef: MatDialogRef<AddStudentComponent>, public translateService:TranslateService) { 
    translateService.use('en');
  }

  ngOnInit(): void {
  }

}
