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

import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { TakeAttendanceList } from '../../../../models/take-attendance-list.model';
import { Router} from '@angular/router';
import { LayoutService } from '../../../../../@vex/services/layout.service';
import { debounceTime, distinctUntilChanged, takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { LoaderService } from '../../../../services/loader.service';
import { GetAllStaffModel, StaffMasterModel } from '../../../../models/staff.model';
import { StudentAttendanceService } from '../../../../services/student-attendance.service';
import { SearchFilter, SearchFilterListViewModel } from '../../../../models/search-filter.model';
import { CommonService } from '../../../../services/common.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { RolePermissionListViewModel, RolePermissionViewModel } from '../../../../models/roll-based-access.model';
import { CryptoService } from '../../../../services/Crypto.service';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { ExcelService } from '../../../../services/excel.service';
import { FormControl } from '@angular/forms';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger40ms } from '../../../../../@vex/animations/stagger.animation';
import { fadeInRight400ms } from '../../../../../@vex/animations/fade-in-right.animation';

@Component({
  selector: 'vex-take-attendance',
  templateUrl: './take-attendance.component.html',
  styleUrls: ['./take-attendance.component.scss'],
  animations: [
    fadeInUp400ms,
    stagger40ms,
    fadeInRight400ms
  ]
})
export class TakeAttendanceComponent implements OnInit, AfterViewInit {

  pageStatus = "Teacher Function";
  totalCount:number=0;
  pageSize:number;
  searchFilterListViewModel: SearchFilterListViewModel= new SearchFilterListViewModel();
  searchKeyword: string;
  staffList: MatTableDataSource<StaffMasterModel>;
  pageNumber: number;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  showSaveFilter:boolean;

  columns = [
    { label: 'Name', property: 'lastFamilyName', type: 'text', visible: true },
    { label: 'Staff ID', property: 'staffInternalId', type: 'text', visible: true },
    { label: 'Profile', property: 'profile', type: 'text', visible: true },
    { label: 'Job Title', property: 'jobTitle', type: 'text', visible: true },
    { label: 'School Email', property: 'schoolEmail', type: 'text', visible: true },
    { label: 'Mobile Phone', property: 'mobilePhone', type: 'number', visible: true },
    // { label: 'Actions', property: 'actions', type: 'text', visible: true }
  ];
  destroySubject$: Subject<boolean> = new Subject();
  loading: boolean;
  getAllStaff: GetAllStaffModel = new GetAllStaffModel();
  searchFilter: SearchFilter = new SearchFilter();
  editPermission: boolean;
  deletePermission: boolean;
  addPermission: boolean;
  filterJsonParams;
  showAdvanceSearchPanel: boolean;
  searchCtrl: FormControl;
  permissionCategoryForTeacherFunctions;
  inputTakeAttendancePermissions;

  constructor(
    public translateService:TranslateService,
    private router: Router,
    private layoutService: LayoutService,
    private loaderService: LoaderService,
    private studentAttendanceService: StudentAttendanceService,
    private commonService:CommonService,
    private snackbar: MatSnackBar,
    private cryptoService: CryptoService,
    private excelService: ExcelService,
    ) { 
    translateService.use('en');
      if(JSON.parse(localStorage.getItem("collapseValue"))){
        this.layoutService.collapseSidenav();
      }else{
      this.layoutService.expandSidenav();
      }

    this.getAllStaff.filterParams = null;
    this.loaderService.isLoading.pipe(takeUntil(this.destroySubject$)).subscribe((val) => {
      this.loading = val;
    });
  }

  ngOnInit(): void {
    this.callStaffList();
    this.getAllSearchFilter();
    this.searchCtrl = new FormControl();

    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 5);
    this.permissionCategoryForTeacherFunctions = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 11);
    this.inputTakeAttendancePermissions = this.permissionCategoryForTeacherFunctions.permissionSubcategory.find(x=> x.permissionSubcategoryId === 54).rolePermission[0];

  }

  ngAfterViewInit() {
     //  Sorting
     this.getAllStaff = new GetAllStaffModel();
     this.sort.sortChange.subscribe((res) => {
       this.getAllStaff.pageNumber = this.pageNumber
       this.getAllStaff.pageSize = this.pageSize;
       this.getAllStaff.sortingModel.sortColumn = res.active;
       if (this.searchCtrl.value != null && this.searchCtrl.value != "") {
         let filterParams = [
           {
             columnName: null,
             filterValue: this.searchCtrl.value,
             filterOption: 4
           }
         ]
         Object.assign(this.getAllStaff, { filterParams: filterParams });
       }
       if (res.direction == "") {
         this.getAllStaff.sortingModel = null;
         this.callStaffList();
         this.getAllStaff = new GetAllStaffModel();
         this.getAllStaff.sortingModel = null;
       } else {
         this.getAllStaff.sortingModel.sortDirection = res.direction;
         this.callStaffList();
       }
     });
 
     //  Searching
     this.searchCtrl.valueChanges.pipe(debounceTime(500), distinctUntilChanged()).subscribe((term) => {
       if (term != '') {
         this.callWithFilterValue(term);
       } else {
         this.callWithoutFilterValue()
       }
     });
  }

  callWithFilterValue(term) {
    let filterParams = [
      {
        columnName: null,
        filterValue: term,
        filterOption: 4
      }
    ]
    if (this.sort.active != undefined && this.sort.direction != "") {
      this.getAllStaff.sortingModel.sortColumn = this.sort.active;
      this.getAllStaff.sortingModel.sortDirection = this.sort.direction;
    }
    Object.assign(this.getAllStaff, { filterParams: filterParams });
    this.getAllStaff.pageNumber = 1;
    this.paginator.pageIndex = 0;
    this.getAllStaff.pageSize = this.pageSize;
    this.callStaffList();
  }

  getSearchResult(res){
    this.showSaveFilter=true;
    this.totalCount= res.totalCount;
    this.pageNumber = res.pageNumber;
    this.pageSize = res.pageSize;
    this.staffList = new MatTableDataSource(res.staffMaster);
    this.getAllStaff = new GetAllStaffModel();
  }

  callWithoutFilterValue() {
    Object.assign(this.getAllStaff, { filterParams: null });
    this.getAllStaff.pageNumber = this.paginator.pageIndex + 1;
    this.getAllStaff.pageSize = this.pageSize;
    if (this.sort.active != undefined && this.sort.direction != "") {
      this.getAllStaff.sortingModel.sortColumn = this.sort.active;
      this.getAllStaff.sortingModel.sortDirection = this.sort.direction;
    }
    this.callStaffList();
  }

  viewCourseSectionDetails(element) {
    const staffFullName = `${element.firstGivenName} ${element.middleName ? element.middleName + ' ' : ''}${ element.lastFamilyName}`;
    this.studentAttendanceService.setStaffDetails({staffId: element.staffId, staffFullName});
    this.router.navigate(['/school', 'staff', 'teacher-functions', 'take-attendance', 'course-section']);
  }


  getPageEvent(event) {
    if (this.sort.active != undefined && this.sort.direction != "") {
      this.getAllStaff.sortingModel.sortColumn = this.sort.active;
      this.getAllStaff.sortingModel.sortDirection = this.sort.direction;
    }
    if (this.searchCtrl.value != null && this.searchCtrl.value != "") {
      let filterParams = [
        {
          columnName: null,
          filterValue: this.searchCtrl.value,
          filterOption: 3
        }
      ]
      Object.assign(this.getAllStaff, { filterParams: filterParams });
    }
    this.getAllStaff.pageNumber = event.pageIndex + 1;
    this.getAllStaff.pageSize = event.pageSize;
    this.callStaffList();
  }

  getAllSearchFilter(){
    this.searchFilterListViewModel.module='Staff';
    this.searchFilterListViewModel.schoolId = JSON.parse(sessionStorage.getItem('selectedSchoolId'));
    this.searchFilterListViewModel.tenantId = sessionStorage.getItem("tenantId");
    this.commonService.getAllSearchFilter(this.searchFilterListViewModel).subscribe((res) => {
      if (res) {
        if (res._failure) {
          this.searchFilterListViewModel.searchFilterList=[]
        }
        else {
          this.searchFilterListViewModel= res;
          let filterData=this.searchFilterListViewModel.searchFilterList.filter(x=> x.filterId == this.searchFilter.filterId);
          if(filterData.length > 0) {
            this.searchFilter.jsonList= filterData[0].jsonList;
          }
          if(this.filterJsonParams == null){
            this.searchFilter = this.searchFilterListViewModel.searchFilterList[this.searchFilterListViewModel.searchFilterList.length-1];
          }
        }
      } else {
        this.snackbar.open('Filter list failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
    }
    );
  }

  callStaffList() {
    if (this.getAllStaff.sortingModel?.sortColumn == "") {
      this.getAllStaff.sortingModel = null
    }
    this.studentAttendanceService.getAllStaffList(this.getAllStaff).subscribe(res => {
      if (res._failure) {
        if(res.staffMaster==null){
          this.snackbar.open(res._message, '', {
            duration: 10000
          });
        this.staffList = new MatTableDataSource([]);
        }else{
          this.staffList = new MatTableDataSource([]);
        }
       
      } else {
        this.totalCount = res.totalCount;
        this.pageNumber = res.pageNumber;
        this.pageSize = res._pageSize;
        this.staffList = new MatTableDataSource(res.staffMaster);
        this.getAllStaff = new GetAllStaffModel();
      }
    });
  }


  get visibleColumns() {
    return this.columns.filter(column => column.visible).map(column => column.property);
  }

  exportStaffListToExcel(){
    let getAllStaff: GetAllStaffModel = new GetAllStaffModel();
    getAllStaff.pageNumber=0;
    getAllStaff.pageSize=0;
    getAllStaff.sortingModel=null;
      this.studentAttendanceService.getAllStaffList(getAllStaff).subscribe(res => {
        if(res._failure){
          this.snackbar.open('Failed to Export Staff List.'+ res._message, '', {
          duration: 10000
          });
        }else{
          if(res.staffMaster.length>0){
            let staffList = res.staffMaster?.map((x:StaffMasterModel)=>{
              let middleName=x.middleName ? ' '+x.middleName+' ' : ' ';
              return {
               Name: x.firstGivenName+middleName+x.lastFamilyName,
               'Staff ID': x.staffInternalId,
               Profile: x.profile,
               'Job Title': x.jobTitle,
               'School Email':x.schoolEmail,
               'Mobile Phone':x.mobilePhone
             }
            });
            this.excelService.exportAsExcelFile(staffList,'Staffs_List_')
          }else{
            this.snackbar.open('No Records Found. Failed to Export Staff List','', {
              duration: 5000
            });
          }
        }
      });
    
  }

  toggleColumnVisibility(column, event) {
    event.stopPropagation();
    event.stopImmediatePropagation();
    column.visible = !column.visible;
  }

  showAdvanceSearch() {
    this.showAdvanceSearchPanel = true;
    this.filterJsonParams = null;
  }

  hideAdvanceSearch(event){
    this.showSaveFilter = event.showSaveFilter;
    this.showAdvanceSearchPanel = false;
    if(event.showSaveFilter == false){
      this.getAllSearchFilter();
    }
  }

}
