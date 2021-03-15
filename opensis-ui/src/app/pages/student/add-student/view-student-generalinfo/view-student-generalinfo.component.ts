import { Component, Input, OnInit } from '@angular/core';
import { SchoolCreate } from '../../../../enums/school-create.enum';
import { StudentAddModel } from '../../../../models/studentModel';
import { SharedFunction } from '../../../shared/shared-function';
import { TranslateService } from '@ngx-translate/core';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';

@Component({
  selector: 'vex-view-student-generalinfo',
  templateUrl: './view-student-generalinfo.component.html',
  styleUrls: ['./view-student-generalinfo.component.scss'],
  animations: [
    stagger60ms
  ]
})
export class ViewStudentGeneralinfoComponent implements OnInit {


  studentCreate = SchoolCreate;
  @Input() studentCreateMode: SchoolCreate;
  @Input() categoryId;
  @Input() studentViewDetails: StudentAddModel;
  module = "Student";
  @Input() nameOfMiscValues
  constructor(private commonFunction: SharedFunction,
    public translateService: TranslateService
  ) {
    translateService.use('en');
   }

  ngOnInit(): void {
    
  }
  getAge(birthDate) {
    return this.commonFunction.getAge(birthDate);
  }

}
