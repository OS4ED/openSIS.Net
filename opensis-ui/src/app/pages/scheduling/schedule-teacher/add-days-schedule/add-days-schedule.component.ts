import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import icClose from '@iconify/icons-ic/twotone-close';
import { TranslateService } from '@ngx-translate/core';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';

@Component({
  selector: 'vex-add-days-schedule',
  templateUrl: './add-days-schedule.component.html',
  styleUrls: ['./add-days-schedule.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class AddDaysScheduleComponent implements OnInit {
  icClose = icClose;

  constructor(private dialogRef: MatDialogRef<AddDaysScheduleComponent>, public translateService:TranslateService) { 
    translateService.use('en');
  }

  ngOnInit(): void {
  }

}
