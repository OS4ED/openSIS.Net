import { Component, Input, OnInit } from '@angular/core';
import { SchoolCreate } from '../../../../enums/school-create.enum';
import { SchoolAddViewModel } from '../../../../models/school-master.model';

@Component({
  selector: 'vex-view-wash-info',
  templateUrl: './view-wash-info.component.html',
  styleUrls: ['./view-wash-info.component.scss']
})
export class ViewWashInfoComponent implements OnInit {
  @Input() schoolCreateMode: SchoolCreate;
  @Input() categoryId;
  @Input() schoolViewDetails: SchoolAddViewModel;
  module = "School";
  constructor() { }

  ngOnInit(): void {
  }

}
