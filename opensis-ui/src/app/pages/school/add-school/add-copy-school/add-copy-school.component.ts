import { Component, OnInit } from '@angular/core';
import icClose from '@iconify/icons-ic/twotone-close';
import { TranslateService } from '@ngx-translate/core';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'vex-add-copy-school',
  templateUrl: './add-copy-school.component.html',
  styleUrls: ['./add-copy-school.component.scss']
})
export class AddCopySchoolComponent implements OnInit {

  icClose = icClose;

  constructor(private dialog: MatDialog, public translateService:TranslateService) {
    translateService.use('en');
  }

  ngOnInit(): void {
  }

}
