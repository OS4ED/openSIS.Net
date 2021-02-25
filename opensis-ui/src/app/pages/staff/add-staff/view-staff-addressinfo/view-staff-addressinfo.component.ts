import { Component, Input, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { StaffAddModel } from '../../../../models/staffModel';
import icCheckBox from '@iconify/icons-ic/check-box';
import icCheckBoxOutlineBlank from '@iconify/icons-ic/check-box-outline-blank';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'vex-view-staff-addressinfo',
  templateUrl: './view-staff-addressinfo.component.html',
  styleUrls: ['./view-staff-addressinfo.component.scss'],
  animations: [
    stagger60ms
  ]
})


export class ViewStaffAddressinfoComponent implements OnInit {
  @Input() staffCreateMode;
  @Input() categoryId;
  @Input() staffViewDetails: StaffAddModel;
  @Input() nameOfMiscValues;
  module = 'Staff';
  icCheckBox = icCheckBox;
  icCheckBoxOutlineBlank = icCheckBoxOutlineBlank;
  homeAddressMapUrl: string;
  mailingAddressMapUrl: string;
  constructor(
              public translateService: TranslateService,
              private snackbar: MatSnackBar) {
    translateService.use('en');
  }

  ngOnInit(): void {
  }
  showHomeAddressOnGoogleMap(){
    const stAdd1 = this.staffViewDetails.staffMaster.homeAddressLineOne;
    const stAdd2 = this.staffViewDetails.staffMaster.homeAddressLineTwo;
    const city = this.staffViewDetails.staffMaster.homeAddressCity;
    const country = this.nameOfMiscValues.countryName;
    const state = this.staffViewDetails.staffMaster.homeAddressState;
    const zip = this.staffViewDetails.staffMaster.homeAddressZip;
    if (stAdd1 && country && city && zip){
      this.homeAddressMapUrl = `https://maps.google.com/?q=${stAdd1},${stAdd2},${city},${state},${zip},${country}`;
      window.open(this.homeAddressMapUrl, '_blank');
    }else{
      this.snackbar.open('Invalid home address', 'Ok', {
        duration: 5000
      });
    }

  }
  showMailingAddressOnGoogleMap(){
    const stAdd1 = this.staffViewDetails.staffMaster.mailingAddressLineOne;
    const stAdd2 = this.staffViewDetails.staffMaster.mailingAddressLineTwo;
    const city = this.staffViewDetails.staffMaster.mailingAddressCity;
    const country = this.nameOfMiscValues.mailingAddressCountry;
    const state = this.staffViewDetails.staffMaster.mailingAddressState;
    const zip = this.staffViewDetails.staffMaster.mailingAddressZip;
    if (stAdd1 && country && city && zip){
      this.mailingAddressMapUrl = `https://maps.google.com/?q=${stAdd1},${stAdd2},${city},${state},${zip},${country}`;
      window.open(this.mailingAddressMapUrl, '_blank');
    }else{
      this.snackbar.open('Invalid mailing address', 'Ok', {
        duration: 5000
      });
    }
  }

}
