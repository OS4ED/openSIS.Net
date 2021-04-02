import { Component, Input, OnInit } from '@angular/core';
import { SchoolCreate } from '../../../../enums/school-create.enum';
import icCheckBoxOutlineBlank from '@iconify/icons-ic/check-box-outline-blank';
import icCheckBox from '@iconify/icons-ic/check-box';
import { StudentAddModel } from '../../../../models/studentModel';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'vex-view-student-addressandcontacts',
  templateUrl: './view-student-addressandcontacts.component.html',
  styleUrls: ['./view-student-addressandcontacts.component.scss'],
  animations: [
    stagger60ms
  ],
})
export class ViewStudentAddressandcontactsComponent implements OnInit {
  icCheckBoxOutlineBlank = icCheckBoxOutlineBlank;
  icCheckBox = icCheckBox;

  @Input() studentCreateMode: SchoolCreate;
  @Input() studentViewDetails: StudentAddModel;
  @Input() nameOfMiscValues;
  constructor(private snackbar: MatSnackBar) { }

  showHomeAddressOnGoogleMap(){
    const stAdd1 = this.studentViewDetails.studentMaster.homeAddressLineOne;
    const stAdd2 = this.studentViewDetails.studentMaster.homeAddressLineTwo;
    const city = this.studentViewDetails.studentMaster.homeAddressCity;
    const country = this.nameOfMiscValues.countryName;
    const state = this.studentViewDetails.studentMaster.homeAddressState;
    const zip = this.studentViewDetails.studentMaster.homeAddressZip;
    let homeAddressMapUrl = `https://maps.google.com/?q=${stAdd1?stAdd1:''}${stAdd2?','+stAdd2:''}${city?','+city:''}${state?','+state:''}${zip?','+zip:''}${country?','+country:''}`;
    window.open(homeAddressMapUrl, '_blank');

  }
  showMailingAddressOnGoogleMap(){
    const stAdd1 = this.studentViewDetails.studentMaster.mailingAddressLineOne;
    const stAdd2 = this.studentViewDetails.studentMaster.mailingAddressLineTwo;
    const city = this.studentViewDetails.studentMaster.mailingAddressCity;
    const country = this.nameOfMiscValues.mailingAddressCountry;
    const state = this.studentViewDetails.studentMaster.mailingAddressState;
    const zip = this.studentViewDetails.studentMaster.mailingAddressZip;

    let mailingAddressMapUrl = `https://maps.google.com/?q=${stAdd1?stAdd1:''}${stAdd2?','+stAdd2:''}${city?','+city:''}${state?','+state:''}${zip?','+zip:''}${country?','+country:''}`;
    window.open(mailingAddressMapUrl, '_blank');

  }

  ngOnInit(): void {
    this.nameOfMiscValues.countryName = this.nameOfMiscValues.countryName === '-' ? null : this.nameOfMiscValues.countryName;
    this.nameOfMiscValues.mailingAddressCountry = this.nameOfMiscValues.mailingAddressCountry === '-' ? null : this.nameOfMiscValues.mailingAddressCountry;
  }

}
