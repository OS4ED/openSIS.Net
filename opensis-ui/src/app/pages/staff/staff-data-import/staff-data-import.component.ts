import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'vex-staff-data-import',
  templateUrl: './staff-data-import.component.html',
  styleUrls: ['./staff-data-import.component.scss']
})
export class StaffDataImportComponent implements OnInit {
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
