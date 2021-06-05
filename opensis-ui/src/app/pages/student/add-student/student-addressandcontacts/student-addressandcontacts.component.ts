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

import { Component, OnInit, Input , ViewChild} from '@angular/core';
import { NgForm, Validators } from '@angular/forms';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { fadeInRight400ms } from '../../../../../@vex/animations/fade-in-right.animation';
import { StudentService } from '../../../../services/student.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TranslateService } from '@ngx-translate/core';
import { CommonService } from '../../../../services/common.service';
import { StudentAddModel} from '../../../../models/student.model';
import { CountryModel } from '../../../../models/country.model';
import * as _moment from 'moment';
import { default as _rollupMoment } from 'moment';
import { SchoolCreate } from '../../../../enums/school-create.enum';
import icCheckBox from '@iconify/icons-ic/check-box';
import icCheckBoxOutlineBlank from '@iconify/icons-ic/check-box-outline-blank';
import icEdit from '@iconify/icons-ic/edit';
import { ImageCropperService } from '../../../../services/image-cropper.service';
import { MiscModel } from '../../../../models/misc-data-student.model';
import { ModuleIdentifier } from '../../../../enums/module-identifier.enum';
import { RolePermissionListViewModel, RolePermissionViewModel } from '../../../../models/roll-based-access.model';
import { CryptoService } from '../../../../services/Crypto.service';
import { DefaultValuesService } from '../../../../common/default-values.service';
@Component({
  selector: 'vex-student-addressandcontacts',
  templateUrl: './student-addressandcontacts.component.html',
  styleUrls: ['./student-addressandcontacts.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms,
    fadeInRight400ms
  ],
})
export class StudentAddressandcontactsComponent implements OnInit {
  @Input() studentDetailsForViewAndEdit;
  @ViewChild('f') currentForm: NgForm;
  @ViewChild('checkBox') checkBox;
  f: NgForm;
  nameOfMiscValuesForView: MiscModel = new MiscModel();
  icEdit = icEdit;
  icCheckBox = icCheckBox;
  icCheckBoxOutlineBlank = icCheckBoxOutlineBlank;
  countryListArr = [];
  countryName = '-';
  mailingAddressCountry = '-';
  countryModel: CountryModel = new CountryModel();
  data;
  studentCreate = SchoolCreate;
  @Input() studentCreateMode: SchoolCreate;
  studentAddModel: StudentAddModel = new StudentAddModel();
  languageList;
  checkBoxChecked = false;
  actionButtonTitle = 'submit';
  cloneStudentAddModel;
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();
  constructor(public translateService: TranslateService,
              private snackbar: MatSnackBar,
              private studentService: StudentService,
              private commonService: CommonService,
              private cryptoService: CryptoService,
              private defaultValuesService: DefaultValuesService,
              private imageCropperService: ImageCropperService) {
      translateService.use('en');
    }

  ngOnInit(): void {
    this.studentService.studentCreatedMode.subscribe((res)=>{
      this.studentCreateMode = res;
    });
    this.studentService.studentDetailsForViewedAndEdited.subscribe((res)=>{
      this.studentDetailsForViewAndEdit = res;
    });
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 3);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 5);
    const permissionSubCategory = permissionCategory.permissionSubcategory.find( x => x.permissionSubcategoryId === 5);
    this.editPermission = permissionSubCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionSubCategory.rolePermission[0].canDelete;
    this.addPermission = permissionSubCategory.rolePermission[0].canAdd;
    this.getAllCountry();
    if (this.studentCreateMode === this.studentCreate.VIEW){
      this.studentService.changePageMode(this.studentCreateMode);
      this.data = this.studentDetailsForViewAndEdit?.studentMaster;
      this.studentAddModel = this.studentDetailsForViewAndEdit;
      this.cloneStudentAddModel = JSON.stringify(this.studentAddModel);
    }else{
      this.studentService.changePageMode(this.studentCreateMode);
      this.studentAddModel = this.studentService.getStudentDetails();
      this.cloneStudentAddModel = JSON.stringify(this.studentAddModel);
      this.data = this.studentAddModel?.studentMaster;
    }
  }

  editAddressContactInfo(){
    this.studentCreateMode = this.studentCreate.EDIT;
    this.studentService.changePageMode(this.studentCreateMode);
    this.actionButtonTitle = 'update';
    this.getAllCountry();
    this.studentAddModel.studentMaster.homeAddressCountry = +this.studentAddModel.studentMaster.homeAddressCountry;
    this.studentAddModel.studentMaster.mailingAddressCountry = +this.studentAddModel.studentMaster.mailingAddressCountry;
  }

  cancelEdit(){
    if (JSON.stringify(this.studentAddModel) !== this.cloneStudentAddModel){
      this.studentAddModel = JSON.parse(this.cloneStudentAddModel);
      this.studentDetailsForViewAndEdit = JSON.parse(this.cloneStudentAddModel);
      this.studentService.sendDetails(JSON.parse(this.cloneStudentAddModel));
    }
    this.findCountryNameById();
    this.studentCreateMode = this.studentCreate.VIEW;
    this.studentService.changePageMode(this.studentCreateMode);
    this.data = this.studentAddModel.studentMaster;
    this.imageCropperService.cancelImage('student');
  }

  copyHomeAddress(check){
    if(this.studentAddModel.studentMaster.mailingAddressSameToHome === false || this.studentAddModel.studentMaster.mailingAddressSameToHome === null){
      if(this.studentAddModel.studentMaster.homeAddressLineOne !== undefined && this.studentAddModel.studentMaster.homeAddressCity !== undefined &&
        this.studentAddModel.studentMaster.homeAddressState !== undefined && this.studentAddModel.studentMaster.homeAddressZip !== undefined ){
      this.studentAddModel.studentMaster.mailingAddressLineOne = this.studentAddModel.studentMaster.homeAddressLineOne;
      this.studentAddModel.studentMaster.mailingAddressLineTwo = this.studentAddModel.studentMaster.homeAddressLineTwo;
      this.studentAddModel.studentMaster.mailingAddressCity = this.studentAddModel.studentMaster.homeAddressCity;
      this.studentAddModel.studentMaster.mailingAddressState = this.studentAddModel.studentMaster.homeAddressState;
      this.studentAddModel.studentMaster.mailingAddressZip = this.studentAddModel.studentMaster.homeAddressZip;
      this.studentAddModel.studentMaster.mailingAddressCountry = +this.studentAddModel.studentMaster.homeAddressCountry;

    }else{
      this.checkBoxChecked = check ? true : false;
      this.snackbar.open('Please Provide All Mandatory Fields First', '', {
        duration: 10000
      });
    }

    }else{
      this.studentAddModel.studentMaster.mailingAddressLineOne = '';
      this.studentAddModel.studentMaster.mailingAddressLineTwo = '';
      this.studentAddModel.studentMaster.mailingAddressCity = '';
      this.studentAddModel.studentMaster.mailingAddressState = '';
      this.studentAddModel.studentMaster.mailingAddressZip = '';
      this.studentAddModel.studentMaster.mailingAddressCountry = null;
    }
  }
  getAllCountry(){
    this.commonService.GetAllCountry(this.countryModel).subscribe(data => {
      if (data){
        if (data._failure) {
          this.countryListArr = [];
        } else {
          this.countryListArr = data.tableCountry?.sort((a, b) => a.name < b.name ? -1 : 1 );
          if (this.studentCreateMode === this.studentCreate.VIEW) {
          this.findCountryNameById();
         }
        }
      }else{
        this.countryListArr = [];
      }
    });
  }

  findCountryNameById(){
    this.countryListArr.map((val) => {
      const countryInNumber = +this.data.homeAddressCountry;
      const mailingAddressCountry = +this.data.mailingAddressCountry;
      if (val.id === countryInNumber){
          this.nameOfMiscValuesForView.countryName = val.name;
      }
      if (val.id === mailingAddressCountry){
          this.nameOfMiscValuesForView.mailingAddressCountry = val.name;
      }
    });
  }

  checkBoxCheckInEditMode(){
    if (this.checkBox?.checked){
      this.studentAddModel.studentMaster.mailingAddressLineOne = this.studentAddModel.studentMaster.homeAddressLineOne;
      this.studentAddModel.studentMaster.mailingAddressLineTwo = this.studentAddModel.studentMaster.homeAddressLineTwo;
      this.studentAddModel.studentMaster.mailingAddressCity = this.studentAddModel.studentMaster.homeAddressCity;
      this.studentAddModel.studentMaster.mailingAddressState = this.studentAddModel.studentMaster.homeAddressState;
      this.studentAddModel.studentMaster.mailingAddressZip = this.studentAddModel.studentMaster.homeAddressZip;
      this.studentAddModel.studentMaster.mailingAddressCountry = +this.studentAddModel.studentMaster.homeAddressCountry;
    }
  }
  submit(){
    this.checkBoxCheckInEditMode();
    this.studentService.UpdateStudent(this.studentAddModel).subscribe(data => {
      if (data){
        if (data._failure) {
          this.snackbar.open( data._message, '', {
            duration: 10000
          });
        } else {
          this.snackbar.open(data._message, '', {
            duration: 10000
          });
          this.studentService.setStudentCloneImage(data.studentMaster.studentPhoto);
          data.studentMaster.studentPhoto = null;
          this.data = data.studentMaster;
          this.studentAddModel = data;
          this.cloneStudentAddModel = JSON.stringify(data);
          this.studentDetailsForViewAndEdit = data;
          this.findCountryNameById();
          this.studentCreateMode = this.studentCreate.VIEW;
          this.studentService.changePageMode(this.studentCreateMode);
        }
      }
      else{
        this.snackbar.open(this.defaultValuesService.translateKey('studentUpdationfailed') + sessionStorage.getItem('httpError'), '', {
          duration: 10000
        });
      }
    });
  }

}
