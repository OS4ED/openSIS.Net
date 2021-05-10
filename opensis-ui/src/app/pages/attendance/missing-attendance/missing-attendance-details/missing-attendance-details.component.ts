import { Component, OnInit } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import icSearch from "@iconify/icons-ic/search";

@Component({
  selector: "vex-missing-attendance-details",
  templateUrl: "./missing-attendance-details.component.html",
  styleUrls: ["./missing-attendance-details.component.scss"],
})
export class MissingAttendanceDetailsComponent implements OnInit {
  icSearch = icSearch;

  constructor(public translateService: TranslateService) {
    translateService.use("en");
  }

  ngOnInit(): void {}
}
