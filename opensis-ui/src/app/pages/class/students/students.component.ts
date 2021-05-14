import { Component, OnInit } from '@angular/core';
import { TranslateService } from "@ngx-translate/core";
import icSearch from "@iconify/icons-ic/search";

@Component({
  selector: 'vex-students',
  templateUrl: './students.component.html',
  styleUrls: ['./students.component.scss']
})
export class StudentsComponent implements OnInit {
  icSearch = icSearch;
  totalCount:number=0;
  pageNumber:number;
  pageSize:number;

  constructor( public translateService: TranslateService ) {
    translateService.use("en");
  }

  ngOnInit(): void {
  }

}
