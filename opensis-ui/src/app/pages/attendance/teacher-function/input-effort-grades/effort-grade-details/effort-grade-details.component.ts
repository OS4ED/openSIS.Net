import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { fadeInRight400ms } from '../../../../../../@vex/animations/fade-in-right.animation';
import { fadeInUp400ms } from '../../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../../@vex/animations/stagger.animation';

@Component({
  selector: 'vex-effort-grade-details',
  templateUrl: './effort-grade-details.component.html',
  styleUrls: ['./effort-grade-details.component.scss'],
  animations: [
    fadeInRight400ms,
    stagger60ms,
    fadeInUp400ms
  ]
})
export class EffortGradeDetailsComponent implements OnInit {

  constructor(public translateService:TranslateService) { 
    translateService.use('en');
  }

  ngOnInit(): void {
  }

}
