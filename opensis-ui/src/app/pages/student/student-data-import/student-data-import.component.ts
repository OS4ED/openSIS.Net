import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'vex-student-data-import',
  templateUrl: './student-data-import.component.html',
  styleUrls: ['./student-data-import.component.scss']
})
export class StudentDataImportComponent implements OnInit {
  upload_file = 1;
  map_fields = 2;
  preview_map = 3;
  import_data = 4;
  view = this.upload_file;
  files: File[] = [];


  constructor(public translateService:TranslateService) { 
    translateService.use('en');
  }

  ngOnInit(): void {
  }

  changeView(viewType){
    this.view = viewType;
  }

	onSelect(event) {
		console.log(event);
		this.files.push(...event.addedFiles);
	}
  onRemove(event) {
		console.log(event);
		this.files.splice(this.files.indexOf(event), 1);
	}

}
