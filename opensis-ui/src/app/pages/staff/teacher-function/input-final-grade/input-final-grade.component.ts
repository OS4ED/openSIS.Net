import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Router } from '@angular/router';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormControl } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { GetAllStaffModel, StaffMasterModel } from 'src/app/models/staff.model';
import { RolePermissionListViewModel } from 'src/app/models/roll-based-access.model';
import { CryptoService } from 'src/app/services/Crypto.service';
import { LoaderService } from 'src/app/services/loader.service';
import { StaffService } from 'src/app/services/staff.service';
import { FinalGradeService } from 'src/app/services/final-grade.service';
import { LayoutService } from 'src/@vex/services/layout.service';
import { fadeInRight400ms } from 'src/@vex/animations/fade-in-right.animation';
import { stagger60ms } from 'src/@vex/animations/stagger.animation';
import { fadeInUp400ms } from 'src/@vex/animations/fade-in-up.animation';

@Component({
  selector: 'vex-input-final-grade',
  templateUrl: './input-final-grade.component.html',
  styleUrls: ['./input-final-grade.component.scss'],
  animations: [
    fadeInRight400ms,
    stagger60ms,
    fadeInUp400ms
  ],
})
export class InputFinalGradeComponent implements OnInit {
  pageStatus = "Teacher Function";
  pageInit = 1;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort
  getAllStaff: GetAllStaffModel = new GetAllStaffModel();
  staffList: MatTableDataSource<StaffMasterModel>;

  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup;
  permissionCategoryForTeacherFunctions;
  loading: boolean;
  totalCount: number = 0;
  pageNumber: number;
  pageSize: number;
  searchCtrl: FormControl;
  displayedColumns: string[] = ['lastFamilyName', 'staffInternalId', 'profile', 'jobTitle', 'schoolEmail', 'mobilePhone'];
  inputFinalGradePermissions: any;

  constructor(
    public translateService: TranslateService,
    private router: Router,
    private snackbar: MatSnackBar,
    private cryptoService: CryptoService,
    private loaderService: LoaderService,
    private staffService: StaffService,
    private finalGradeService: FinalGradeService,
    private layoutService: LayoutService,
  ) {
    translateService.use('en');
    this.getAllStaff.filterParams = null;
    this.loaderService.isLoading.subscribe((val) => {
      this.loading = val;
    });

    if(localStorage.getItem("collapseValue") !== null){
      if( localStorage.getItem("collapseValue") === "false"){
        this.layoutService.expandSidenav();
      }else{
      } 
    }else{
      this.layoutService.expandSidenav();
    }
  }
  viewEffortGradeDetails(element) {
    const staffFullName = `${element.firstGivenName} ${element.middleName ? element.middleName + ' ' : ''}${element.lastFamilyName}`;
    this.finalGradeService.setStaffDetails({ staffId: element.staffId, staffFullName });
    this.router.navigate(['/school', 'staff', 'teacher-functions', 'input-final-grade', 'grade-details']);
    this.pageInit = 2;
  }

  ngOnInit(): void {
    this.callStaffList();
    this.searchCtrl = new FormControl();
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

  callStaffList() {
    if (this.getAllStaff.sortingModel?.sortColumn == "") {
      this.getAllStaff.sortingModel = null
    }
    this.staffService.getAllStaffList(this.getAllStaff).subscribe(res => {
      if (res._failure) {
        if (res.staffMaster == null) {
          this.snackbar.open(res._message, '', {
            duration: 10000
          });
          this.staffList = new MatTableDataSource([]);
        } else {
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
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 5);
    this.permissionCategoryForTeacherFunctions = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 11)
    this.inputFinalGradePermissions = this.permissionCategoryForTeacherFunctions.permissionSubcategory.find(x => x.permissionSubcategoryId === 50).rolePermission[0];
  }

  checkViewEnableOrNot(id): boolean {
    return this.permissionCategoryForTeacherFunctions.permissionSubcategory.find(x => x.permissionSubcategoryId === id).rolePermission[0].canView;
  }

}
