import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { TranslateService } from "@ngx-translate/core";
import icSearch from "@iconify/icons-ic/search";

@Component({
  selector: "vex-missing-attendance",
  templateUrl: "./missing-attendance.component.html",
  styleUrls: ["./missing-attendance.component.scss"],
})
export class MissingAttendanceComponent implements OnInit {

  icSearch = icSearch;

  constructor(
    public translateService: TranslateService,
    private router: Router
  ) {
    translateService.use("en");
  }
  ngOnInit(): void {}
}
