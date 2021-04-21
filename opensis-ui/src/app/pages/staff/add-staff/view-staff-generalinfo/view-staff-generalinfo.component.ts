import { Component, Input, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { StaffAddModel } from '../../../../models/staff.model';
import { SharedFunction } from '../../../shared/shared-function';

@Component({
  selector: 'vex-view-staff-generalinfo',
  templateUrl: './view-staff-generalinfo.component.html',
  styleUrls: ['./view-staff-generalinfo.component.scss'],
  animations: [
    stagger60ms
  ]
})
export class ViewStaffGeneralinfoComponent implements OnInit {
  @Input() staffCreateMode;
  @Input() categoryId;
  @Input() staffViewDetails: StaffAddModel;
  @Input() nameOfMiscValues;
  module = 'Staff';
  staffPortalAccess: string;
  constructor(public translateService: TranslateService,
              private commonFunction: SharedFunction
              ) {
    translateService.use('en');
  }

  ngOnInit(): void {
    this.viewPortalAccess();
  }

  viewPortalAccess() {
    if (this.staffViewDetails.staffMaster.portalAccess == false || this.staffViewDetails.staffMaster.portalAccess == null) {
      this.staffPortalAccess = 'No';
    }
    else {
      this.staffPortalAccess = 'Yes';
    }
  }

  getAge(birthDate) {
    return this.commonFunction.getAge(birthDate);
  }

}
