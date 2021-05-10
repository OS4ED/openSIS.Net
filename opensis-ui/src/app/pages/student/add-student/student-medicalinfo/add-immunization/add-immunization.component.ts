import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import icClose from '@iconify/icons-ic/twotone-close';

@Component({
  selector: 'vex-add-immunization',
  templateUrl: './add-immunization.component.html',
  styleUrls: ['./add-immunization.component.scss']
})
export class AddImmunizationComponent implements OnInit {

  icClose = icClose;

  constructor(public translateService: TranslateService) {
    translateService.use("en");
  }

  ngOnInit(): void {
  }

}
