import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import icClose from '@iconify/icons-ic/twotone-close';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'vex-add-variable-schedule',
  templateUrl: './add-variable-schedule.component.html',
  styleUrls: ['./add-variable-schedule.component.scss']
})
export class AddVariableScheduleComponent implements OnInit {
  icClose = icClose;

  constructor(private dialogRef: MatDialogRef<AddVariableScheduleComponent>, public translateService:TranslateService) { 
    translateService.use('en');
  }

  ngOnInit(): void {
  }

}
