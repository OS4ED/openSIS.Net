import { Component, Inject, OnInit, Optional } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import icClose from '@iconify/icons-ic/twotone-close';
import { fadeInUp400ms } from '../../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../../@vex/animations/stagger.animation';
import { ParentInfoService } from '../../../../../services/parent-info.service';
import { AddParentInfoModel } from '../../../../../models/parentInfoModel';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CountryModel } from '../../../../../models/countryModel';
import { CommonService } from '../../../../../services/common.service';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { SchoolAddViewModel } from '../../../../../models/schoolMasterModel';
@Component({
  selector: 'vex-view-sibling',
  templateUrl: './view-sibling.component.html',
  styleUrls: ['./view-sibling.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class ViewSiblingComponent implements OnInit {
  icClose = icClose;
  address: string;
  schoolName;
  getStudentForView = []; 
  gradeLevelTitle;
  countryName;
  mapUrl: string;
  destroySubject$: Subject<void> = new Subject();
  getCountryModel: CountryModel = new CountryModel();
  addParentInfoModel: AddParentInfoModel = new AddParentInfoModel();
  schoolAddViewModel: SchoolAddViewModel = new SchoolAddViewModel();
  constructor(private dialogRef: MatDialogRef<ViewSiblingComponent>,
              private snackbar: MatSnackBar,
              private parentInfoService: ParentInfoService,
              private commonService: CommonService,
              @Optional() @Inject(MAT_DIALOG_DATA) public data: any) {}


  ngOnInit(): void {
    this.getAllCountry();
    if (this.data.flag === 'Parent'){      
      this.data.siblingDetails = this.data.studentDetails;
      this.gradeLevelTitle = this.data.studentDetails.gradeLevelTitle;
      this.schoolName = this.data.studentDetails.schoolName;
        if (this.data.studentDetails.address !== ''){
          this.address = this.data.studentDetails.address;
        }else{
          this.address = null;
        }   
    }else{
      this.schoolName = this.data.siblingDetails.schoolMaster.schoolName;
      this.gradeLevelTitle = this.data.siblingDetails.studentEnrollment[0].gradeLevelTitle;
      if (this.data.siblingDetails.homeAddressLineOne !== null){
        this.address = this.data.siblingDetails.homeAddressLineOne;
        if (this.data.siblingDetails.homeAddressLineTwo !== null){
          this.address = this.address + ' ' + this.data.siblingDetails.homeAddressLineTwo;
          if (this.data.siblingDetails.homeAddressCity !== null){
            this.address = this.address + ' ' + this.data.siblingDetails.homeAddressCity;
            if (this.data.siblingDetails.homeAddressState !== null) {
              this.address = this.address + ' ' + this.data.siblingDetails.homeAddressState;
              if (this.data.siblingDetails.homeAddressZip !== null) {
                this.address = this.address + ' ' + this.data.siblingDetails.homeAddressZip;
              }
            }
          }
        }
      }
      else {
        this.address = null;
      }
    }
  }
  getAllCountry() {
    this.commonService.GetAllCountry(this.getCountryModel).pipe(takeUntil(this.destroySubject$)).subscribe(data => {
      if (typeof (data) === 'undefined') {
      }
      else {
        if (data._failure) {
        } else {
          this.findCountryNameByIdOnViewMode(data.tableCountry);
        }
      }
    });
  }

  findCountryNameByIdOnViewMode(countryListArr) {
    const index = countryListArr.findIndex((x) => {
      return x.id === +this.data.siblingDetails.homeAddressCountry ;
    });
    if (countryListArr[index]?.name !== undefined){
      this.countryName = countryListArr[index]?.name;
    }
    else{
      this.countryName = '';
    }
  }

  showOnGoogleMap(){
    if (this.address !== null){
      const address = this.address;
      this.mapUrl = `https://maps.google.com/?q=${address}`;
      window.open(this.mapUrl, '_blank');
    }else{
      this.snackbar.open('Invalid  Address', 'Ok', {
        duration: 5000
      });
    }
  }

}
