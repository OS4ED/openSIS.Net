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

import { Component, OnInit, ViewChild, Output, EventEmitter, OnDestroy, Input } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatAccordion } from '@angular/material/expansion';
import { StudentService } from '../../../../services/student.service';
import { filterParams, StudentListModel, StudentMasterSearchModel } from '../../../../models/student.model';
import { GetAllSectionModel } from '../../../../models/section.model';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { CommonLOV } from '../../../shared-module/lov/common-lov';
import { SectionService } from '../../../../services/section.service';
import { CommonService } from '../../../../services/common.service';
import { LoginService } from '../../../../services/login.service';
import { CountryModel } from '../../../../models/country.model';
import { LanguageModel } from '../../../../models/language.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { StaffService } from '../../../../services/staff.service';
import { GetAllStaffModel, StaffMasterSearchModel } from '../../../../models/staff.model';
import { GetAllMembersList } from '../../../../models/membership.model';
import { MembershipService } from '../../../../services/membership.service';
import { SearchFilterAddViewModel } from '../../../../models/search-filter.model';
import { DefaultValuesService } from '../../../../common/default-values.service';
import { SharedFunction } from '../../../shared/shared-function';
import moment from 'moment';

@Component({
  selector: 'vex-search-staff',
  templateUrl: './search-staff.component.html',
  styleUrls: ['./search-staff.component.scss']
})
export class SearchStaffComponent implements OnInit,OnDestroy {

  @ViewChild(MatAccordion) accordion: MatAccordion;
  @Output() showHideAdvanceSearch = new EventEmitter<any>();
  @Output() searchList = new EventEmitter<any>();
  @Input() filterJsonParams;
  @Output() searchValue = new EventEmitter<any>();
  @Input() incomingSearchValue;
  @Input() incomingToggleValues;
  @Output() toggelValues = new EventEmitter<any>();
  countryModel: CountryModel = new CountryModel();
  languages: LanguageModel = new LanguageModel();
  @ViewChild('f') currentForm: NgForm;
  destroySubject$: Subject<void> = new Subject();
  staffMasterSearchModel: StaffMasterSearchModel = new StaffMasterSearchModel();
  getAllStaff: GetAllStaffModel = new GetAllStaffModel();
  searchFilterAddViewModel: SearchFilterAddViewModel = new SearchFilterAddViewModel();
  dobEndDate : string;
  dobStartDate : string;
  searchTitle= 'search'
  showSaveFilter= true;
  updateFilter= false;
  params = [];
  countryListArr = [];
  ethnicityList = [];
  raceList = [];
  genderList = [];
  maritalStatusList = [];
  languageList;
  getAllMembersList: GetAllMembersList = new GetAllMembersList();
  searchSchoolId: number = this.defaultValuesService.getSchoolID();
  inactiveStaff = false;

  constructor(
    private commonLOV: CommonLOV,
    private snackbar: MatSnackBar,
    private sectionService: SectionService,
    private commonService: CommonService,
    private loginService: LoginService,
    private staffService: StaffService,
    private defaultValuesService: DefaultValuesService,
    private commonFunction: SharedFunction,
    private membershipService: MembershipService
  ) { }

  ngOnInit(): void {
    this.getAllStaff.pageSize = this.defaultValuesService.getPageSize() ? this.defaultValuesService.getPageSize() : 10;
    if (this.incomingSearchValue){
      this.inactiveStaff = this.incomingToggleValues.inactiveStaff;
      this.searchSchoolId = this.incomingToggleValues.searchSchoolId;
      this.staffMasterSearchModel = this.incomingSearchValue;
    }
    if (this.filterJsonParams !== null && this.filterJsonParams !== undefined) {
      this.updateFilter = true;
      this.searchTitle='searchAndUpdateFilter';
      let jsonResponse = JSON.parse(this.filterJsonParams.jsonList);
      for (let json of jsonResponse) {
        this.staffMasterSearchModel[json.columnName] = json.filterValue;
      }
    }

    this.initializeDropdownsInAddMode();
  }

  initializeDropdownsInAddMode() {
    this.callLOVs();
    this.getAllCountry();
    this.GetAllLanguage();
    this.getAllMembership();
  }

  callLOVs() {
    this.commonLOV.getLovByName('Gender').pipe(takeUntil(this.destroySubject$)).subscribe((res) => {
      this.genderList = res;
    });
    this.commonLOV.getLovByName('Race').pipe(takeUntil(this.destroySubject$)).subscribe((res) => {
      this.raceList = res;
    });
    this.commonLOV.getLovByName('Ethnicity').pipe(takeUntil(this.destroySubject$)).subscribe((res) => {
      this.ethnicityList = res;
    });
    this.commonLOV.getLovByName('Marital Status').pipe(takeUntil(this.destroySubject$)).subscribe((res) => {
      this.maritalStatusList = res;
    });
  }

  getAllCountry() {
    this.commonService.GetAllCountry(this.countryModel).pipe(takeUntil(this.destroySubject$)).subscribe(data => {
      if (typeof (data) === 'undefined') {
        this.countryListArr = [];
      }
      else {
        if (data._failure) {
          this.countryListArr = [];
        } else {
          this.countryListArr = data.tableCountry?.sort((a, b) => { return a.name < b.name ? -1 : 1; })

        }
      }
    });
  }

  GetAllLanguage() {
    this.loginService.getAllLanguage(this.languages).pipe(takeUntil(this.destroySubject$)).subscribe((res) => {
      if (typeof (res) === 'undefined') {
        this.languageList = [];
      }
      else {
        this.languageList = res.tableLanguage?.sort((a, b) => { return a.locale < b.locale ? -1 : 1; })

      }
    });
  }
  searchAllSchools(event){
    if (event.checked){
      this.searchSchoolId = 0;
    }
    else{
      this.searchSchoolId = this.defaultValuesService.getSchoolID();
    }
  }
  includeInactiveStaff(event){
    if (event.checked){
      this.inactiveStaff = true;
    }
    else{
      this.inactiveStaff = false;
    }
  }
  
  getAllMembership() {
    this.membershipService.getAllMembers(this.getAllMembersList).subscribe((res) => {
      if (typeof (res) == 'undefined') {
        this.snackbar.open('Membership List failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (res._failure) {
          if (res.getAllMemberList == null) {
            this.getAllMembersList.getAllMemberList = [];
            this.snackbar.open( res._message,'', {
              duration: 10000
            });
          } else {
            this.getAllMembersList.getAllMemberList = [];
          }
        }
        else {
          this.getAllMembersList.getAllMemberList = res.getAllMemberList.filter((item) => {
            return (item.profileType == 'School Administrator' || item.profileType == 'Admin Assistant'
              || item.profileType == 'Teacher' || item.profileType == 'Homeroom Teacher')
          });
        }
      }
    })
  }


  submit() {
    this.params = [];
    for (let key in this.staffMasterSearchModel) {
      if (this.staffMasterSearchModel.hasOwnProperty(key))
      if (this.staffMasterSearchModel[key] !== null && this.staffMasterSearchModel[key] !== '' && this.staffMasterSearchModel[key] !== undefined) {
        if (key === 'joiningDate' || key === 'endDate' || key === 'dob') {
          this.params.push({ "columnName": key, "filterOption": 11, "filterValue": this.commonFunction.formatDateSaveWithoutTime(this.staffMasterSearchModel[key]) })
        }
        else {
          this.params.push({ "columnName": key, "filterOption": 11, "filterValue": this.staffMasterSearchModel[key] })
        }
      }
    }

    if (this.updateFilter) {
      this.showSaveFilter = false;
      this.searchFilterAddViewModel.searchFilter.filterId = this.filterJsonParams.filterId;
      this.searchFilterAddViewModel.searchFilter.module = 'Staff';
      this.searchFilterAddViewModel.searchFilter.jsonList = JSON.stringify(this.params);
      this.searchFilterAddViewModel.searchFilter.filterName = this.filterJsonParams.filterName;
      this.searchFilterAddViewModel.searchFilter.modifiedBy = this.defaultValuesService.getEmailId();
      this.commonService.updateSearchFilter(this.searchFilterAddViewModel).subscribe((res) => {
        if (typeof (res) === 'undefined') {
          this.snackbar.open('Search filter updated failed' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else {
          if (res._failure) {
            this.snackbar.open(res._message, '', {
              duration: 10000
            });
          }
          else {
            this.snackbar.open(res._message, '', {
              duration: 10000
            });
          }
        }
      }
      );
    }

    this.getAllStaff.filterParams = this.params;
    this.getAllStaff.sortingModel = null;
    this.getAllStaff.schoolId = this.searchSchoolId;
    this.getAllStaff.includeInactive = this.inactiveStaff;
    this.getAllStaff.dobStartDate = this.commonFunction.formatDateSaveWithoutTime(this.dobStartDate);
    this.getAllStaff.dobEndDate = this.commonFunction.formatDateSaveWithoutTime(this.dobEndDate);
    this.commonService.setSearchResult(this.params);
    this.staffService.getAllStaffList(this.getAllStaff).subscribe(res => {
      if (res._failure) {
        this.searchList.emit([]);
        this.searchValue.emit(this.currentForm.value);
        this.toggelValues.emit({inactiveStaff: this.inactiveStaff, searchSchoolId: this.searchSchoolId});
        this.snackbar.open(res._message, '', {
          duration: 10000
        });
      } else {
        let outStafflist = res;
        for (let staff of outStafflist.staffMaster){
          if (staff.staffSchoolInfo[0].endDate){
            let today = moment().format('DD-MM-YYYY').toString();
            let todayarr = today.split('-');
            let todayDate = +todayarr[0];
            let todayMonth = +todayarr[1];
            let todayYear = +todayarr[2];
            let endDate = moment(staff.staffSchoolInfo[0].endDate).format('DD-MM-YYYY').toString();
            let endDateArr = endDate.split('-');
            let endDateDate = +endDateArr[0];
            let endDateMonth = +endDateArr[1];
            let endDateYear = +endDateArr[2];
            if ( endDateYear === todayYear){
              if (endDateMonth === todayMonth){
                if (endDateDate >= todayDate){
                  staff.status = 'active';
                }
                else {
                  staff.status = 'inactive';
                }
              }
              else if (endDateMonth < todayMonth){
                staff.status = 'inactive';
              }
              else{
                staff.status = 'active';
              }
            }
            else if (endDateYear < todayYear){
              staff.status = 'inactive';
            }
            else{
              staff.status = 'active';
            }
          }
          else{
            staff.status = 'active';
          }
        }
        this.searchList.emit(outStafflist);
        this.searchValue.emit(this.currentForm.value);
        this.toggelValues.emit({inactiveStaff: this.inactiveStaff, searchSchoolId: this.searchSchoolId});
        this.showHideAdvanceSearch.emit({ showSaveFilter:this.showSaveFilter , hide: false});
      }
    });
  }

  resetData() {
    this.currentForm.reset();
    this.inactiveStaff = false;
    this.searchSchoolId = this.defaultValuesService.getSchoolID();
    this.submit();
  }

  hideAdvanceSearch() {
    this.showHideAdvanceSearch.emit({ showSaveFilter: null , hide: false});
  }

  ngOnDestroy() {
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }

}
