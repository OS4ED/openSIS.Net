import { Component, OnInit } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { Router } from "@angular/router";
import { TranslateService } from "@ngx-translate/core";
import { StudentDetails } from "../../../models/student-details.model";
import icSearch from '@iconify/icons-ic/search';

@Component({
  selector: "vex-transcripts",
  templateUrl: "./transcripts.component.html",
  styleUrls: ["./transcripts.component.scss"],
})
export class TranscriptsComponent implements OnInit {

  icSearch = icSearch;
  constructor(
    private router: Router,
    private dialog: MatDialog,
    public translateService: TranslateService
  ) {
    translateService.use("en");
  }
  studentDetails: StudentDetails[] = [
    {
      studentSelected: false,
      studentName: "Danielle Boucher",
      studentId: "12",
      alternateId: "STD0012",
      gradeLevel: "Grade 11",
      section: "Section A",
      phone: 123456789,
    },
    {
      studentSelected: true,
      studentName: "Andrew Brown",
      studentId: "15",
      alternateId: "STD0015",
      gradeLevel: "Grade 11",
      section: "Section A",
      phone: 123456789,
    },
    {
      studentSelected: true,
      studentName: "Ella Brown",
      studentId: "35",
      alternateId: "STD0035",
      gradeLevel: "Grade 10",
      section: "Section B",
      phone: 653456789,
    },
    {
      studentSelected: false,
      studentName: "Lian Fang",
      studentId: "102",
      alternateId: "STD00102",
      gradeLevel: "Grade 11",
      section: "Section A",
      phone: 923456789,
    },
    {
      studentSelected: true,
      studentName: "Adriana Garcia",
      studentId: "67",
      alternateId: "STD0067",
      gradeLevel: "Grade 11",
      section: "Section A",
      phone: 123456789,
    },
    {
      studentSelected: false,
      studentName: "Olivia Jones",
      studentId: "52",
      alternateId: "STD0052",
      gradeLevel: "Grade 10",
      section: "Section A",
      phone: 123456789,
    },
    {
      studentSelected: true,
      studentName: "Amare Keita",
      studentId: "103",
      alternateId: "STD00103",
      gradeLevel: "Grade 11",
      section: "Section B",
      phone: 453456789,
    },
    {
      studentSelected: true,
      studentName: "Amber Keita",
      studentId: "24",
      alternateId: "STD0024",
      gradeLevel: "Grade 11",
      section: "Section A",
      phone: 123456789,
    },
    {
      studentSelected: false,
      studentName: "Alyssa Kimathi",
      studentId: "41",
      alternateId: "STD0041",
      gradeLevel: "Grade 10",
      section: "Section A",
      phone: 123456789,
    },
    {
      studentSelected: true,
      studentName: "Robert Millar",
      studentId: "15",
      alternateId: "STD0015",
      gradeLevel: "Grade 11",
      section: "Section A",
      phone: 123456789,
    },
  ];
  displayedColumns: string[] = [
    "studentSelected",
    "studentName",
    "studentId",
    "alternateId",
    "gradeLevel",
    "section",
    "phone",
  ];

  ngOnInit(): void {}
}
