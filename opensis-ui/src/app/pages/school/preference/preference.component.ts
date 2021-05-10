import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'vex-preference',
  templateUrl: './preference.component.html',
  styleUrls: ['./preference.component.scss']
})
export class PreferenceComponent implements OnInit {

  constructor(public translateService: TranslateService
    ) {
      translateService.use("en");

    }

  ngOnInit(): void {
  }

}
