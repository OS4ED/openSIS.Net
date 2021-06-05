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
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CountryModel } from '../../../../app/models/country.model';
import { LanguageModel } from '../../../../app/models/language.model';
import { StudentListModel, StudentMasterSearchModel } from '../../../../app/models/student.model';
import { SearchFilterAddViewModel } from '../../../../app/models/search-filter.model';
import { StudentService } from '../../../../app/services/student.service';
import { CommonLOV } from '../../shared-module/lov/common-lov';
import { SectionService } from '../../../../app/services/section.service';
import { CommonService } from '../../../../app/services/common.service';
import { DefaultValuesService } from '../../../../app/common/default-values.service';
import { LoginService } from '../../../../app/services/login.service';
import { SharedFunction } from '../../shared/shared-function';
import { GetAllSectionModel } from '../../../../app/models/section.model';

@Component({
  selector: 'vex-search-student',
  templateUrl: './search-student.component.html',
  styleUrls: ['./search-student.component.scss']
})
export class SearchStudentComponent implements OnInit {
  @ViewChild(MatAccordion) accordion: MatAccordion;
  @Output() showHideAdvanceSearch = new EventEmitter<any>();
  @Output() searchList = new EventEmitter<any>();
  @Output() searchValue = new EventEmitter<any>();
  @Output() toggelValues = new EventEmitter<any>();
  @Input() filterJsonParams;
  @Input() incomingSearchValue;
  @Input() incomingToggleValues;
  countryModel: CountryModel = new CountryModel();
  languages: LanguageModel = new LanguageModel();
  @ViewChild('f') currentForm: NgForm;
  destroySubject$: Subject<void> = new Subject();
  studentMasterSearchModel: StudentMasterSearchModel = new StudentMasterSearchModel();
  getAllStudent: StudentListModel = new StudentListModel();
  searchFilterAddViewModel: SearchFilterAddViewModel = new SearchFilterAddViewModel();
  dobEndDate: string;
  dobStartDate: string;
  showSaveFilter= true;
  params = [];
  searchTitle :string= 'search';
  updateFilter: boolean = false;
  countryListArr = [];
  ethnicityList = [];
  raceList = [];
  genderList = [];
  suffixList = [];
  maritalStatusList = [];
  salutationList = [];
  sectionList = [];
  languageList;
  searchSchoolId: number = this.defaultValuesService.getSchoolID();
  inactiveStudents = false;
  constructor(
    private studentService: StudentService,
    private commonLOV: CommonLOV,
    private snackbar: MatSnackBar,
    private sectionService: SectionService,
    private commonService: CommonService,
    private defaultValuesService: DefaultValuesService,
    private loginService: LoginService,
    private commonFunction: SharedFunction,
  ) { }

  ngOnInit(): void {
    this.getAllStudent.pageSize = this.defaultValuesService.getPageSize() ? this.defaultValuesService.getPageSize() : 10;
    if (this.incomingSearchValue){
      this.inactiveStudents = this.incomingToggleValues.inactiveStudents;
      this.searchSchoolId = this.incomingToggleValues.searchSchoolId;
      this.studentMasterSearchModel = this.incomingSearchValue;
    }
    if (this.filterJsonParams !== null && this.filterJsonParams !== undefined) {
      this.updateFilter = true;
      this.searchTitle='searchAndUpdateFilter';
      let jsonResponse = JSON.parse(this.filterJsonParams.jsonList);
      for (let json of jsonResponse) {
        this.studentMasterSearchModel[json.columnName] = json.filterValue;
      }
    }

    this.initializeDropdownsInAddMode();
  }

  initializeDropdownsInAddMode() {
    this.callLOVs();
    this.getAllCountry();
    this.GetAllLanguage();
    this.getAllSection();
  }

  callLOVs() {
    this.commonLOV.getLovByName('Salutation').pipe(takeUntil(this.destroySubject$)).subscribe((res) => {
      this.salutationList = res;
    });
    this.commonLOV.getLovByName('Suffix').pipe(takeUntil(this.destroySubject$)).subscribe((res) => {
      this.suffixList = res;
    });
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
      if (typeof (data) == 'undefined') {
        this.countryListArr = [];
      }
      else {
        if (data._failure) {
          this.countryListArr = [];
        } else {
          this.countryListArr = data.tableCountry?.sort((a, b) => { return a.name < b.name ? -1 : 1; })

        }
      }
    })
  }

  GetAllLanguage() {
    this.loginService.getAllLanguage(this.languages).pipe(takeUntil(this.destroySubject$)).subscribe((res) => {
      if (typeof (res) == 'undefined') {
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
  includeInactiveStudents(event){
    if (event.checked){
      this.inactiveStudents = true;
    }
    else{
      this.inactiveStudents = false;
    }
  }

  getAllSection() {
    let section: GetAllSectionModel = new GetAllSectionModel();
    this.sectionService.GetAllSection(section).pipe(takeUntil(this.destroySubject$)).subscribe(data => {
      if (data._failure) {
      }
      else {
        this.sectionList = data.tableSectionsList;
      }

    });
  }


  submit() {
    this.params = [];
    for (let key in this.studentMasterSearchModel) {
      if (this.studentMasterSearchModel.hasOwnProperty(key))
      if (this.studentMasterSearchModel[key] !== null && this.studentMasterSearchModel[key] !== '' && this.studentMasterSearchModel[key] !== undefined) {
        if (key === 'dob') {
          this.params.push({ "columnName": key, "filterOption": 11, "filterValue": this.commonFunction.formatDateSaveWithoutTime(this.studentMasterSearchModel[key]) })
        }
        else {
          this.params.push({ "columnName": key, "filterOption": 11, "filterValue": this.studentMasterSearchModel[key] })
        }
      }
    }



    if (this.updateFilter) {
      this.showSaveFilter = false;
      this.searchFilterAddViewModel.searchFilter.filterId = this.filterJsonParams.filterId;
      this.searchFilterAddViewModel.searchFilter.module = 'Student';
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
    this.getAllStudent.schoolId = this.searchSchoolId;
    this.getAllStudent.includeInactive = this.inactiveStudents;
    this.getAllStudent.filterParams = this.params;
    this.getAllStudent.sortingModel = null;
    this.getAllStudent.dobStartDate = this.commonFunction.formatDateSaveWithoutTime(this.dobStartDate);
    this.getAllStudent.dobEndDate = this.commonFunction.formatDateSaveWithoutTime(this.dobEndDate);
    this.commonService.setSearchResult(this.params);
    this.studentService.GetAllStudentList(this.getAllStudent).subscribe(data => {
      if (data._failure) {
        this.searchList.emit([]);
        this.toggelValues.emit({inactiveStudents: this.inactiveStudents, searchSchoolId: this.searchSchoolId});
        this.searchValue.emit(this.currentForm.value);
        this.snackbar.open('' + data._message, '', {
          duration: 10000
        });

      } else {
        this.searchList.emit(data);
        this.toggelValues.emit({inactiveStudents: this.inactiveStudents, searchSchoolId: this.searchSchoolId});
        this.searchValue.emit(this.currentForm.value);
        this.showHideAdvanceSearch.emit({ showSaveFilter: this.showSaveFilter , hide: false});
      }
    });
  }

  resetData() {
    this.currentForm.reset();
    this.inactiveStudents = false;
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
