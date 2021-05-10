import { Component, OnInit } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import icCheckCircle from "@iconify/icons-ic/check-circle";

@Component({
  selector: "vex-recalculate-daily-attendance",
  templateUrl: "./recalculate-daily-attendance.component.html",
  styleUrls: ["./recalculate-daily-attendance.component.scss"],
})
export class RecalculateDailyAttendanceComponent implements OnInit {

  icCheckCircle = icCheckCircle;
  constructor(public translateService: TranslateService) {
    translateService.use("en");
  }

  ngOnInit(): void {}
}
