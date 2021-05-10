import { Component, OnInit } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import icDownload from "@iconify/icons-fa-solid/download";

@Component({
  selector: "vex-student-transcript",
  templateUrl: "./student-transcript.component.html",
  styleUrls: ["./student-transcript.component.scss"],
})
export class StudentTranscriptComponent implements OnInit {
  icDownload = icDownload;
  downloadTranscript = false;
  openTranscript = true;

  constructor(public translateService: TranslateService) {
    translateService.use("en");
  }

  ngOnInit(): void {}
  openDownloadTranscript() {
    this.openTranscript = false;
    this.downloadTranscript = true;
  }

  closeTranscript() {
    this.downloadTranscript = false;
    this.openTranscript = true;
  }
}
