import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import icSearch from "@iconify/icons-ic/search";

@Component({
  selector: 'vex-missing-attendance-list',
  templateUrl: './missing-attendance-list.component.html',
  styleUrls: ['./missing-attendance-list.component.scss']
})
export class MissingAttendanceListComponent implements OnInit {
  
  icSearch = icSearch;

  constructor(public translateService: TranslateService) {
    translateService.use("en");
  }

  ngOnInit(): void {
  }

}
