import { Component, OnInit } from '@angular/core';
import icClose from '@iconify/icons-ic/twotone-close';
import { TranslateService } from '@ngx-translate/core';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'vex-success-copy-school',
  templateUrl: './success-copy-school.component.html',
  styleUrls: ['./success-copy-school.component.scss']
})
export class SuccessCopySchoolComponent implements OnInit {

  icClose = icClose;

  constructor(private dialog: MatDialog, public translateService:TranslateService) {
    translateService.use('en');
  }
  ngOnInit(): void {
  }

}
