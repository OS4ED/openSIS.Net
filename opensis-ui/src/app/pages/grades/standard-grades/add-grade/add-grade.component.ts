import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import icClose from '@iconify/icons-ic/twotone-close';

@Component({
  selector: 'vex-add-grade',
  templateUrl: './add-grade.component.html',
  styleUrls: ['./add-grade.component.scss']
})
export class AddGradeComponent implements OnInit {

  icClose = icClose;

  constructor(private dialogRef: MatDialogRef<AddGradeComponent>, public translateService:TranslateService) { 
    translateService.use('en');
  }

  ngOnInit(): void {
  }

}
