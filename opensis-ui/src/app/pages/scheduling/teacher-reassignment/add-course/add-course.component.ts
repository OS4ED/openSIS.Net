import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import icClose from '@iconify/icons-ic/twotone-close';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'vex-add-course',
  templateUrl: './add-course.component.html',
  styleUrls: ['./add-course.component.scss']
})
export class AddCourseComponent implements OnInit {
  icClose = icClose;

  constructor(private dialogRef: MatDialogRef<AddCourseComponent>, public translateService:TranslateService) { 
    translateService.use('en');
  }

  ngOnInit(): void {
  }

}
