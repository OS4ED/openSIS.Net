import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import icClose from '@iconify/icons-ic/twotone-close';

@Component({
  selector: 'vex-add-alert',
  templateUrl: './add-alert.component.html',
  styleUrls: ['./add-alert.component.scss']
})
export class AddAlertComponent implements OnInit {

  icClose = icClose;

  constructor(public translateService: TranslateService) {
    translateService.use("en");
  }

  ngOnInit(): void {
  }

}
