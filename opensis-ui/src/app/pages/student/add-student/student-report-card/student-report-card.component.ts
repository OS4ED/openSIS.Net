import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { AddCommentsComponent } from './add-comments/add-comments.component';
import { AddTeacherCommentsComponent } from './add-teacher-comments/add-teacher-comments.component';
import icPrint from '@iconify/icons-ic/twotone-print';

@Component({
  selector: 'vex-student-report-card',
  templateUrl: './student-report-card.component.html',
  styleUrls: ['./student-report-card.component.scss']
})
export class StudentReportCardComponent implements OnInit {

  icPrint = icPrint;

  constructor(private dialog: MatDialog, public translateService: TranslateService) {
    translateService.use("en");
  }

  ngOnInit(): void {
  }
  addComments(){
    this.dialog.open(AddCommentsComponent, {
      width: '500px'
    })
  }

  addTeacherComments(){
    this.dialog.open(AddTeacherCommentsComponent, {
      width: '500px'
    })
  }

}
