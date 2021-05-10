import { Component, OnInit } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import icSearch from "@iconify/icons-ic/search";
import { TranslateService } from "@ngx-translate/core";
import { AddCommentsComponent } from "./add-comments/add-comments.component";
import { StudentAttendanceCommentComponent } from "./student-attendance-comment/student-attendance-comment.component";

@Component({
  selector: "vex-administration",
  templateUrl: "./administration.component.html",
  styleUrls: ["./administration.component.scss"],
})
export class AdministrationComponent implements OnInit {
  icSearch = icSearch;

  constructor(
    private dialog: MatDialog,
    public translateService: TranslateService
  ) {
    translateService.use("en");
  }

  ngOnInit(): void {}

  addComments(){
    this.dialog.open(AddCommentsComponent, {
      width: '600px'
    })
  }

  openStudentAttendanceComment(){
    this.dialog.open(StudentAttendanceCommentComponent, {
      width: '900px'
    })
  }

}
