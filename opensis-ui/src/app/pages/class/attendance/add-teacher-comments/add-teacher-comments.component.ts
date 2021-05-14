import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import icClose from '@iconify/icons-ic/twotone-close';

@Component({
  selector: 'vex-add-comments',
  templateUrl: './add-teacher-comments.component.html',
  styleUrls: ['./add-teacher-comments.component.scss']
})
export class AddTeacherCommentsComponent implements OnInit {

  icClose = icClose;

  constructor(private dialogRef: MatDialogRef<AddTeacherCommentsComponent>, public translateService:TranslateService) { 
    translateService.use('en');
  }

  ngOnInit(): void {
  }

}
