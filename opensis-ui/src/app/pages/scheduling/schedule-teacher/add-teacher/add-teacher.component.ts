import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import icClose from '@iconify/icons-ic/twotone-close';
import { TeacherDetails } from '../../../../models/teacherDetailsModel';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'vex-add-teacher',
  templateUrl: './add-teacher.component.html',
  styleUrls: ['./add-teacher.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class AddTeacherComponent implements OnInit {

  icClose = icClose;
  teacherDetails: TeacherDetails[] = [
    {staffSelected: true, staffName: 'Danielle Boucher', staffId: 'ST4562', primaryGrade: 'Grade 9', primarySubject: 'Mathematics', homeroomTeacher: 'Yes'},
    {staffSelected: false, staffName: 'Andrew Brown', staffId: 'ST756', primaryGrade: 'Grade 10', primarySubject: 'Science', homeroomTeacher: 'No'},
    {staffSelected: true, staffName: 'Lian Fang', staffId: 'ST456', primaryGrade: 'Grade 9', primarySubject: 'Mathematics', homeroomTeacher: 'No'}
  ];
  displayedColumns: string[] = ['staffSelected', 'staffName', 'staffId', 'primaryGrade', 'primarySubject', 'homeroomTeacher'];


  constructor(private dialogRef: MatDialogRef<AddTeacherComponent>, public translateService:TranslateService) { 
    translateService.use('en');
  }

  ngOnInit(): void {
  }

}
