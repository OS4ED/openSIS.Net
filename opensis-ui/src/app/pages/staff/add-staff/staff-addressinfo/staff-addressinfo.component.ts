/***********************************************************************************
openSIS is a free student information system for public and non-public
schools from Open Solutions for Education, Inc.Website: www.os4ed.com.

Visit the openSIS product website at https://opensis.com to learn more.
If you have question regarding this software or the license, please contact
via the website.

The software is released under the terms of the GNU Affero General Public License as
published by the Free Software Foundation, version 3 of the License.
See https://www.gnu.org/licenses/agpl-3.0.en.html.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

Copyright (c) Open Solutions for Education, Inc.

All rights reserved.
***********************************************************************************/

import { Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { fadeInRight400ms } from '../../../../../@vex/animations/fade-in-right.animation';
import { SchoolCreate } from '../../../../enums/school-create.enum';
import { CountryModel } from '../../../../models/country.model';
import { StaffAddModel } from '../../../../models/staff.model';
import { LanguageModel } from '../../../../models/language.model';
import icEdit from '@iconify/icons-ic/edit';
import { TranslateService } from '@ngx-translate/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { StaffService } from '../../../../services/staff.service';
import { CommonService } from '../../../../services/common.service';
import { LoginService } from '../../../../services/login.service';
import { SharedFunction } from '../../../../pages/shared/shared-function';
import { StaffRelation } from '../../../../enums/staff-relation.enum';
import { ImageCropperService } from '../../../../services/image-cropper.service';
import { LovList } from '../../../../models/lov.model';
import { MiscModel } from '../../../../models/misc-data-student.model';
import { ModuleIdentifier } from '../../../../enums/module-identifier.enum';
import { CommonLOV } from '../../../shared-module/lov/common-lov';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { RolePermissionListViewModel, RolePermissionViewModel } from '../../../../models/roll-based-access.model';
import { RollBasedAccessService } from '../../../../services/roll-based-access.service';
@Component({
  selector: 'vex-staff-addressinfo',
  templateUrl: './staff-addressinfo.component.html',
  styleUrls: ['./staff-addressinfo.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms,
    fadeInRight400ms
  ]
})
export class StaffAddressinfoComponent implements OnInit, OnDestroy {
  staffCreate = SchoolCreate;
  moduleIdentifier=ModuleIdentifier;
  staffDetailsForViewAndEdit;
  categoryId;
  @ViewChild('f') currentForm: NgForm;
  @Input() staffCreateMode: SchoolCreate;
  nameOfMiscValuesForView: MiscModel = new MiscModel();
  countryModel: CountryModel = new CountryModel();
  staffAddModel: StaffAddModel = new StaffAddModel();
  languages: LanguageModel = new LanguageModel();
  mailingAddressSameToHome: boolean = false;
  countryListArr = [];
  relationshipList = [];
  lovListViewModel: LovList = new LovList();
  module = 'Staff';
  data;
  languageList;
  checkBoxChecked = false;
  icEdit = icEdit;
  actionButton = 'submit';
  cloneStaffAddModel;
  destroySubject$: Subject<void> = new Subject();
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();
  constructor(public translateService: TranslateService,
              private snackbar: MatSnackBar,
              private staffService: StaffService,
              private commonService: CommonService,
              private imageCropperService: ImageCropperService,
              public rollBasedAccessService: RollBasedAccessService,
              private commonLOV: CommonLOV) {
    translateService.use('en');
  }

  ngOnInit(): void {

    this.staffService.staffCreatedMode.subscribe((res)=>{
      this.staffCreateMode = res;
    })
    this.staffService.staffDetailsForViewedAndEdited.subscribe((res)=>{
      this.staffDetailsForViewAndEdit = res;
    })

    this.staffService.categoryIdSelected.subscribe((res)=>{
      this.categoryId = res;
    })

    this.permissionListViewModel = this.rollBasedAccessService.getPermission();
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 5);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 10);
    const permissionSubCategory = permissionCategory.permissionSubcategory.find( x => x.permissionSubcategoryId === 15);
    this.editPermission = permissionSubCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionSubCategory.rolePermission[0].canDelete;
    this.addPermission = permissionSubCategory.rolePermission[0].canAdd;

    if (this.staffCreateMode === this.staffCreate.VIEW) {
      this.data = this.staffDetailsForViewAndEdit?.staffMaster;
      this.staffAddModel = this.staffDetailsForViewAndEdit;
      this.cloneStaffAddModel = JSON.stringify(this.staffAddModel);
      this.staffService.changePageMode(this.staffCreateMode);
      if (this.data.mailingAddressSameToHome) {
        this.mailingAddressSameToHome = true;
      } else {
        this.mailingAddressSameToHome = false;
      }
      this.getAllCountry();
    } else {
      this.staffService.changePageMode(this.staffCreateMode);
      this.staffAddModel = this.staffService.getStaffDetails();
      this.cloneStaffAddModel = JSON.stringify(this.staffAddModel);
    }
    this.callLOVs();
    this.getAllCountry();
  }

  editAddressContactInfo() {
    this.staffCreateMode = this.staffCreate.EDIT;
    this.staffService.changePageMode(this.staffCreateMode);
    this.actionButton = 'update';
    this.staffAddModel.staffMaster.mailingAddressCountry =+this.staffAddModel.staffMaster.mailingAddressCountry ;
    this.staffAddModel.staffMaster.homeAddressCountry = +this.staffAddModel.staffMaster.homeAddressCountry;

  }

  cancelEdit() {
    if(this.staffAddModel!==JSON.parse(this.cloneStaffAddModel)){
      this.staffAddModel=JSON.parse(this.cloneStaffAddModel);
      this.staffDetailsForViewAndEdit=JSON.parse(this.cloneStaffAddModel);
      this.staffService.sendDetails(JSON.parse(this.cloneStaffAddModel));
    }
    this.staffCreateMode = this.staffCreate.VIEW;
    this.staffService.changePageMode(this.staffCreateMode);
    this.imageCropperService.cancelImage("staff");

  }

  copyHomeAddress(check) {
    if (this.staffAddModel.staffMaster.mailingAddressSameToHome === false || this.staffAddModel.staffMaster.mailingAddressSameToHome === null) {
      if (this.staffAddModel.staffMaster.homeAddressLineOne !== undefined && this.staffAddModel.staffMaster.homeAddressCity !== undefined &&
        this.staffAddModel.staffMaster.homeAddressState !== undefined && this.staffAddModel.staffMaster.homeAddressZip !== undefined) {
        this.staffAddModel.staffMaster.mailingAddressLineOne = this.staffAddModel.staffMaster.homeAddressLineOne;
        this.staffAddModel.staffMaster.mailingAddressLineTwo = this.staffAddModel.staffMaster.homeAddressLineTwo;
        this.staffAddModel.staffMaster.mailingAddressCity = this.staffAddModel.staffMaster.homeAddressCity;
        this.staffAddModel.staffMaster.mailingAddressState = this.staffAddModel.staffMaster.homeAddressState;
        this.staffAddModel.staffMaster.mailingAddressZip = this.staffAddModel.staffMaster.homeAddressZip;
        this.staffAddModel.staffMaster.mailingAddressCountry = this.staffAddModel.staffMaster.homeAddressCountry;
      } else {
        this.checkBoxChecked = check ? true : false;
        this.snackbar.open('Please Provide All Mandatory Fields First', '', {
          duration: 10000
        });
      }

    } else {
      this.staffAddModel.staffMaster.mailingAddressLineOne = "";
      this.staffAddModel.staffMaster.mailingAddressLineTwo = "";
      this.staffAddModel.staffMaster.mailingAddressCity = "";
      this.staffAddModel.staffMaster.mailingAddressState = "";
      this.staffAddModel.staffMaster.mailingAddressZip = "";
      this.staffAddModel.staffMaster.mailingAddressCountry = null;
    }
  }

  callLOVs(){
    this.commonLOV.getLovByName('Relationship').pipe(takeUntil(this.destroySubject$)).subscribe((res)=>{
      this.relationshipList=res;  
    });
  }
  
  getAllCountry() {
    if (!this.countryModel.isCountryAvailable){
      this.countryModel.isCountryAvailable = true;
    this.commonService.GetAllCountry(this.countryModel).subscribe(data => {
      if (typeof (data) == 'undefined') {
        this.countryListArr = [];
      }
      else {
        if (data._failure) {
          this.countryListArr = [];
        } else {
          this.countryListArr=data.tableCountry?.sort((a, b) => {return a.name < b.name ? -1 : 1;} )   
          if (this.staffCreateMode == this.staffCreate.VIEW) {
          this.findCountryNameById()
          }
        }
      }
    });
  }
  }

  findCountryNameById(){
    this.countryListArr.map((val) => {
      let countryInNumber = +this.data.homeAddressCountry;
      let mailingAddressCountry = +this.data.mailingAddressCountry;
      if (val.id === countryInNumber) {
        this.nameOfMiscValuesForView.countryName = val.name;
      }
      if (val.id === mailingAddressCountry) {
        this.nameOfMiscValuesForView.mailingAddressCountry = val.name;
      }
    })
  }

  submitAddress() {
    if (this.staffAddModel.fieldsCategoryList !== null) {
      this.staffAddModel.selectedCategoryId = this.staffAddModel.fieldsCategoryList[this.categoryId].categoryId;
      
      for (let staffCustomField of this.staffAddModel.fieldsCategoryList[this.categoryId].customFields) {
        if (staffCustomField.type === "Multiple SelectBox" && this.staffService.getStaffMultiselectValue() !== undefined) {
          staffCustomField.customFieldsValue[0].customFieldValue = this.staffService.getStaffMultiselectValue().toString().replaceAll(",", "|");
        }
      }
    }
    this.staffService.updateStaff(this.staffAddModel).subscribe(data => {
      if (typeof (data) == 'undefined') {
        this.snackbar.open('Staff Updation failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (data._failure) {
          this.snackbar.open( data._message, '', {
            duration: 10000
          });
        } else {
          this.snackbar.open(data._message, '', {
            duration: 10000
          });
          this.staffService.setStaffCloneImage(data.staffMaster.staffPhoto);
          data.staffMaster.staffPhoto=null;
          this.data = data.staffMaster;
          this.staffDetailsForViewAndEdit=data;
          this.cloneStaffAddModel=JSON.stringify(this.staffDetailsForViewAndEdit);
          this.staffCreateMode = this.staffCreate.VIEW;
          this.findCountryNameById();
          this.staffService.changePageMode(this.staffCreateMode);
          
        }
      }
    })
  }

  ngOnDestroy() {
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }



}
