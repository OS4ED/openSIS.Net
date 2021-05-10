import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import icClose from '@iconify/icons-ic/twotone-close';

@Component({
  selector: 'vex-add-comments',
  templateUrl: './add-comments.component.html',
  styleUrls: ['./add-comments.component.scss']
})
export class AddCommentsComponent implements OnInit {

  icClose = icClose;

  constructor( public translateService: TranslateService) {
    translateService.use("en");
  }

  ngOnInit(): void {
  }

}
