import { Component, OnInit } from '@angular/core';
import { TranslateService } from "@ngx-translate/core";
import icEdit from "@iconify/icons-ic/edit";
import icHowtoReg from "@iconify/icons-ic/outline-how-to-reg";

@Component({
  selector: 'vex-course-overview',
  templateUrl: './course-overview.component.html',
  styleUrls: ['./course-overview.component.scss']
})
export class CourseOverviewComponent implements OnInit {

  icEdit = icEdit;
  icHowtoReg = icHowtoReg;
  showUrlInput = false;
  hideUrl = true;
  hidePassword = true;
  showPasswordInput = false;

  constructor(public translateService: TranslateService) {
    translateService.use("en");
  }

  ngOnInit(): void {}

  addUrl() {
    this.hideUrl = false;
    this.showUrlInput = true;
  }

  addPassword() {
    this.hidePassword = false;
    this.showPasswordInput = true;
  }
}
