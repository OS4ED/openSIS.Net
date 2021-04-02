import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router} from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icDelete from '@iconify/icons-ic/twotone-delete';

@Component({
  selector: 'vex-report-card-comments',
  templateUrl: './report-card-comments.component.html',
  styleUrls: ['./report-card-comments.component.scss']
})
export class ReportCardCommentsComponent implements OnInit {


  icEdit = icEdit;
  icDelete = icDelete;

  constructor(
    private router: Router,
    private dialog: MatDialog,
    public translateService:TranslateService) {
      translateService.use('en'); 
    }

  ngOnInit(): void {
  }

}
