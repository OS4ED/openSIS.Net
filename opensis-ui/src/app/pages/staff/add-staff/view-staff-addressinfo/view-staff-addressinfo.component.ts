import { Component, Input, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { StaffAddModel } from '../../../../models/staff.model';
import icCheckBox from '@iconify/icons-ic/check-box';
import icCheckBoxOutlineBlank from '@iconify/icons-ic/check-box-outline-blank';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { MatSnackBar } from '@angular/material/snack-bar';
import { StaffService } from 'src/app/services/staff.service';

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
              private snackbar: MatSnackBar,
              private staffService: StaffService) {
    translateService.use('en');
  }

  ngOnInit(): void {

 

    this.nameOfMiscValues.countryName=this.nameOfMiscValues.countryName=='-'?'':this.nameOfMiscValues.countryName;
    this.nameOfMiscValues.mailingAddressCountry=this.nameOfMiscValues.mailingAddressCountry=='-'?'':this.nameOfMiscValues.mailingAddressCountry;

  }
  showHomeAddressOnGoogleMap(){
    let stAdd1 = this.staffViewDetails.staffMaster.homeAddressLineOne;
    let stAdd2 = this.staffViewDetails.staffMaster.homeAddressLineTwo;
    let city = this.staffViewDetails.staffMaster.homeAddressCity;
    let country = this.nameOfMiscValues.countryName;
    let state = this.staffViewDetails.staffMaster.homeAddressState;
    let zip = this.staffViewDetails.staffMaster.homeAddressZip;

      this.homeAddressMapUrl = `https://maps.google.com/?q=${stAdd1?stAdd1:''}${stAdd2?','+stAdd2:''}${city?','+city:''}${state?','+state:''}${country?','+country:''}${zip?','+zip:''}`;
      window.open(this.homeAddressMapUrl, '_blank');
    
  }
  showMailingAddressOnGoogleMap(){
    let stAdd1 = this.staffViewDetails.staffMaster.mailingAddressLineOne;
    let stAdd2 = this.staffViewDetails.staffMaster.mailingAddressLineTwo;
    let city = this.staffViewDetails.staffMaster.mailingAddressCity;
    let country = this.nameOfMiscValues.mailingAddressCountry;
    let state = this.staffViewDetails.staffMaster.mailingAddressState;
    let zip = this.staffViewDetails.staffMaster.mailingAddressZip;
      this.mailingAddressMapUrl = `https://maps.google.com/?q=${stAdd1?stAdd1:''}${stAdd2?','+stAdd2:''}${city?','+city:''}${state?','+state:''}${country?','+country:''}${zip?','+zip:''}`;
      window.open(this.mailingAddressMapUrl, '_blank');
    
  }

  openUrl(href: string) {
    if (!href?.trim()) { return; }

    if (href.includes('@')) {
      href = 'mailto:' + href;
      window.open(href);
      return;
    } else
      if (!href.includes('http:') && !href.includes('https:')) {
        href = 'http://' + href;
      }
    window.open(href);
  }

}
