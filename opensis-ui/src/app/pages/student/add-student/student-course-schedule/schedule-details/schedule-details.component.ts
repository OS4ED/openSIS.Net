import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import icClose from '@iconify/icons-ic/twotone-close';
import { ScheduleCourseSectionForViewModel } from '../../../../../models/student-schedule.model';

@Component({
  selector: 'vex-schedule-details',
  templateUrl: './schedule-details.component.html',
  styleUrls: ['./schedule-details.component.scss']
})
export class ScheduleDetailsComponent implements OnInit {

  icClose = icClose;
  courseSectionDetails: ScheduleCourseSectionForViewModel;
  constructor(private dialog: MatDialog, 
    public translateService:TranslateService,
    @Inject(MAT_DIALOG_DATA) public data:any) { 
      this.courseSectionDetails=data.courseSectionDetails;
    translateService.use('en');
  }

  ngOnInit(): void {
  }

}
