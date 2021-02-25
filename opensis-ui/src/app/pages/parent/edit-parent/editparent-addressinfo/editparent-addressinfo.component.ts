import { Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { fadeInRight400ms } from '../../../../../@vex/animations/fade-in-right.animation';
import { TranslateService } from '@ngx-translate/core';
import icAdd from '@iconify/icons-ic/baseline-add';
import icEdit from '@iconify/icons-ic/edit';
import icClear from '@iconify/icons-ic/baseline-clear';
import { SchoolCreate } from '../../../../enums/school-create.enum';
import { AddParentInfoModel } from '../../../../models/parentInfoModel';
import { CountryModel } from '../../../../models/countryModel';
import { CommonService } from '../../../../services/common.service';
import { ParentInfoService } from '../../../../services/parent-info.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ImageCropperService } from 'src/app/services/image-cropper.service';
import { ModuleIdentifier } from '../../../../enums/module-identifier.enum';

@Component({
  selector: 'vex-editparent-addressinfo',
  templateUrl: './editparent-addressinfo.component.html',
  styleUrls: ['./editparent-addressinfo.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms,
    fadeInRight400ms
  ]
})
export class EditparentAddressinfoComponent implements OnInit,OnDestroy {

  icAdd = icAdd;
  icClear = icClear;
  icEdit = icEdit;
  @Input() parentDetailsForViewAndEdit;
  @ViewChild('f') currentForm: NgForm;
  f: NgForm;
  parentCreate = SchoolCreate;
  @Input() parentCreateMode: SchoolCreate;
  addParentInfoModel: AddParentInfoModel = new AddParentInfoModel();
  countryModel: CountryModel = new CountryModel();
  countryListArr = [];
  countryName = '-';
  country = '-';
  data;
  parentInfo;
  moduleIdentifier = ModuleIdentifier;
  mapUrl: string;
  constructor(private fb: FormBuilder,
              private snackbar: MatSnackBar,
              public translateService: TranslateService,
              private commonService: CommonService,
              private parentInfoService: ParentInfoService,
              private imageCropperService: ImageCropperService) {
    translateService.use('en');

  }

  ngOnInit(): void {   
    this.imageCropperService.enableUpload({module:this.moduleIdentifier.PARENT,upload:true,mode:this.parentCreate.VIEW});
    this.parentCreateMode = this.parentCreate.VIEW;
    this.parentInfo = {};
    this.addParentInfoModel = this.parentDetailsForViewAndEdit;
    this.addParentInfoModel.parentInfo.parentAddress[0].country = +this.parentDetailsForViewAndEdit.parentInfo.parentAddress[0].country; 
    this.getAllCountry();
  }




  editAddressContactInfo() {
    this.parentCreateMode = this.parentCreate.EDIT;
    this.imageCropperService.enableUpload({module: this.moduleIdentifier.PARENT, upload: true, mode: this.parentCreate.EDIT});
  }

  getAllCountry() {
    this.commonService.GetAllCountry(this.countryModel).subscribe(data => {
      if (typeof (data) === 'undefined') {
        this.countryListArr = [];
      }
      else {
        if (data._failure) {
          this.countryListArr = [];
        } else {
          this.countryListArr=data.tableCountry?.sort((a, b) => {return a.name < b.name ? -1 : 1;} )   
          if (this.parentCreateMode == this.parentCreate.VIEW) {
            this.viewCountryName();
          }

          this.countryListArr.map((val) => {
            var country = + this.addParentInfoModel.parentInfo.parentAddress[0].country;
            if (val.id === country) {

              this.country = val.name;
            }

          })


        }
      }
    })
  }


  viewCountryName() {
    this.countryListArr.map((val) => {
      var countryInNumber = + this.addParentInfoModel.parentInfo.parentAddress[0].country;      
      if (val.id === countryInNumber) {
        this.countryName = val.name;

      }

    })
  }

  submit() {
    this.addParentInfoModel._token = sessionStorage.getItem("token");
    this.addParentInfoModel._tenantName = sessionStorage.getItem("tenant");
    this.parentInfoService.updateParentInfo(this.addParentInfoModel).subscribe(data => {
      if (typeof (data) == 'undefined') {
        this.snackbar.open('Address Updation failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (data._failure) {
          this.snackbar.open('Address Updation failed. ' + data._message, 'LOL THANKS', {
            duration: 10000
          });
        } else {
          this.snackbar.open('Address Update Successful.', '', {
            duration: 10000
          });
          this.viewCountryName();
          this.parentCreateMode = this.parentCreate.VIEW;
          this.parentInfoService.changePageMode(this.parentCreateMode);
          this.imageCropperService.enableUpload({module:this.moduleIdentifier.PARENT,upload:true,mode:this.parentCreate.VIEW});
        }
      }

    });
  }
  showOnGoogleMap(){
    const stAdd1 = this.addParentInfoModel.parentInfo.parentAddress[0].addressLineOne;
    const stAdd2 = this.addParentInfoModel.parentInfo.parentAddress[0].addressLineTwo;
    const city = this.addParentInfoModel.parentInfo.parentAddress[0].city;
    const country = this.countryName;
    const state = this.addParentInfoModel.parentInfo.parentAddress[0].state;
    const zip = this.addParentInfoModel.parentInfo.parentAddress[0].zip;
    if (stAdd1 && country && city && zip){
      this.mapUrl = `https://maps.google.com/?q=${stAdd1},${stAdd2},${city},${state},${zip},${country}`;
      window.open(this.mapUrl, '_blank');
    }else{
      this.snackbar.open('Invalid parent address', 'Ok', {
        duration: 5000
      });
    }
  }

  ngOnDestroy(){
  }
}
