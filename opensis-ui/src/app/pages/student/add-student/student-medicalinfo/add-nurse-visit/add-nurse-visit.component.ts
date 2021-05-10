import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import icClose from '@iconify/icons-ic/twotone-close';
import icSchedule from '@iconify/icons-ic/twotone-schedule';

@Component({
  selector: 'vex-add-nurse-visit',
  templateUrl: './add-nurse-visit.component.html',
  styleUrls: ['./add-nurse-visit.component.scss']
})
export class AddNurseVisitComponent implements OnInit {

  icClose = icClose;
  icSchedule = icSchedule;

  constructor(public translateService: TranslateService) {
    translateService.use("en");
  }

  ngOnInit(): void {
  }

}
