import { Component, OnInit } from "@angular/core";
import icClose from "@iconify/icons-ic/twotone-close";
import { TranslateService } from "@ngx-translate/core";

@Component({
  selector: "vex-student-attendance-comment",
  templateUrl: "./student-attendance-comment.component.html",
  styleUrls: ["./student-attendance-comment.component.scss"],
})
export class StudentAttendanceCommentComponent implements OnInit {
  icClose = icClose;
  category:number;

  constructor(public translateService: TranslateService) {
    translateService.use("en");
  }

  ngOnInit(): void {}

  openExpandable(step:number) {
    this.category = step;
  }
}
