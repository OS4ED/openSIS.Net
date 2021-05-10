import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import icClose from '@iconify/icons-ic/twotone-close';

@Component({
  selector: 'vex-add-medical',
  templateUrl: './add-medical.component.html',
  styleUrls: ['./add-medical.component.scss']
})
export class AddMedicalComponent implements OnInit {

  icClose = icClose;

  constructor(public translateService: TranslateService) {
    translateService.use("en");
  }

  ngOnInit(): void {
  }

}
