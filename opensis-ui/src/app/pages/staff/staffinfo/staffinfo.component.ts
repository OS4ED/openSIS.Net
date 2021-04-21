import { Component, OnInit, ViewChild, AfterViewInit} from '@angular/core';
import icMoreVert from '@iconify/icons-ic/twotone-more-vert';
import icAdd from '@iconify/icons-ic/baseline-add';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icDelete from '@iconify/icons-ic/twotone-delete';
import icSearch from '@iconify/icons-ic/search';
import icFilterList from '@iconify/icons-ic/filter-list';
import icImpersonate from '@iconify/icons-ic/twotone-account-circle';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { fadeInUp400ms } from '../../../../@vex/animations/fade-in-up.animation';
import { stagger40ms } from '../../../../@vex/animations/stagger.animation';
import { TranslateService } from '@ngx-translate/core';
import { StaffService } from '../../../services/staff.service';
import { LoaderService } from '../../../services/loader.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged, takeUntil } from 'rxjs/operators';
import { GetAllStaffModel,StaffListModel, StaffMasterModel } from '../../../models/staff.model';
import { ImageCropperService } from '../../../services/image-cropper.service';
import { LayoutService } from 'src/@vex/services/layout.service';
import { ExcelService } from '../../../services/excel.service';
import { Subject } from 'rxjs';
import { ModuleIdentifier } from '../../../enums/module-identifier.enum';
import { SchoolCreate } from '../../../enums/school-create.enum';
import { fadeInRight400ms } from 'src/@vex/animations/fade-in-right.animation';
import { RolePermissionListViewModel, RolePermissionViewModel } from '../../../models/roll-based-access.model';
import { RollBasedAccessService } from '../../../services/roll-based-access.service';
import { SaveFilterComponent } from './save-filter/save-filter.component';
import { MatDialog } from '@angular/material/dialog';
import { SearchFilter, SearchFilterAddViewModel, SearchFilterListViewModel } from '../../../models/search-filter.model';
import { CommonService } from '../../../services/common.service';
import { ConfirmDialogComponent } from '../../shared-module/confirm-dialog/confirm-dialog.component';
import { CryptoService } from '../../../services/Crypto.service';

@Component({
  selector: 'vex-staffinfo',
  templateUrl: './staffinfo.component.html',
  styleUrls: ['./staffinfo.component.scss'],
  animations: [
    fadeInUp400ms,
    stagger40ms,
    fadeInRight400ms
  ]
})
export class StaffinfoComponent implements OnInit, AfterViewInit{
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort

  getAllStaff: GetAllStaffModel = new GetAllStaffModel();
  staffList: MatTableDataSource<StaffMasterModel>;
  showAdvanceSearchPanel: boolean = false;
  columns = [
    { label: 'Name', property: 'lastFamilyName', type: 'text', visible: true },
    { label: 'Staff ID', property: 'staffInternalId', type: 'text', visible: true },
    { label: 'Profile', property: 'profile', type: 'text', visible: true },
    { label: 'Job Title', property: 'jobTitle', type: 'text', visible: true },
    { label: 'School Email', property: 'schoolEmail', type: 'text', visible: true },
    { label: 'Mobile Phone', property: 'mobilePhone', type: 'number', visible: true },
    { label: 'Actions', property: 'actions', type: 'text', visible: true }
  ];

  icMoreVert = icMoreVert;
  icAdd = icAdd;
  icEdit = icEdit;
  icDelete = icDelete;
  icSearch = icSearch;
  icImpersonate = icImpersonate;
  icFilterList = icFilterList;

  loading: boolean;
  totalCount: number = 0;
  pageNumber: number;
  pageSize: number;
  searchCtrl: FormControl;
  destroySubject$: Subject<void> = new Subject();
  moduleIdentifier=ModuleIdentifier;
  createMode=SchoolCreate;
  
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();
  showSaveFilter:boolean=false;
  searchFilterAddViewModel: SearchFilterAddViewModel = new SearchFilterAddViewModel();
  searchFilterListViewModel: SearchFilterListViewModel= new SearchFilterListViewModel();
  searchFilter: SearchFilter= new SearchFilter();
  filterJsonParams;
  showLoadFilter=true;
  constructor(private snackbar: MatSnackBar,
    private router: Router,
    private loaderService: LoaderService,
    public translateService: TranslateService,
    private staffService: StaffService,
    private imageCropperService:ImageCropperService,
    public rollBasedAccessService: RollBasedAccessService,
    private layoutService: LayoutService,
    private excelService:ExcelService,
    private dialog: MatDialog,
    private commonService:CommonService,
    private cryptoService: CryptoService) {
    translateService.use('en');
    if(localStorage.getItem("collapseValue") !== null){
      if( localStorage.getItem("collapseValue") === "false"){
        this.layoutService.expandSidenav();
      }else{
        this.layoutService.collapseSidenav();
      } 
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
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 10);
    let permissionSubCategory = permissionCategory.permissionSubcategory.find( x => x.permissionSubcategoryId === 13);
    this.editPermission = permissionSubCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionSubCategory.rolePermission[0].canDelete;
    this.addPermission = permissionSubCategory.rolePermission[0].canAdd;
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

  viewStaffDetails(id) {
    this.imageCropperService.enableUpload({module:this.moduleIdentifier.STAFF,upload:true,mode:this.createMode.VIEW});
    this.staffService.setStaffId(id);
    this.router.navigate(["school/staff/add-staff"]); 
  }

  goToAdd() {
    this.staffService.setStaffId(null);
    this.router.navigate(["school/staff/add-staff"]);
    this.imageCropperService.enableUpload({module:this.moduleIdentifier.STAFF,upload:true,mode:this.createMode.ADD});

  }

  callStaffList() {
    if (this.getAllStaff.sortingModel?.sortColumn == "") {
      this.getAllStaff.sortingModel = null
    }
    this.staffService.getAllStaffList(this.getAllStaff).subscribe(res => {
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

  exportStaffListToExcel(){
    let getAllStaff: GetAllStaffModel = new GetAllStaffModel();
    getAllStaff.pageNumber=0;
    getAllStaff.pageSize=0;
    getAllStaff.sortingModel=null;
      this.staffService.getAllStaffList(getAllStaff).subscribe(res => {
        if(res._failure){
          this.snackbar.open('Failed to Export Staff List.'+ res._message, '', {
          duration: 10000
          });
        }else{
          if(res.staffMaster.length>0){
            let staffList = res.staffMaster?.map((x:StaffMasterModel)=>{
              let middleName=x.middleName==null?' ':' '+x.middleName+' ';
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

  get visibleColumns() {
    return this.columns.filter(column => column.visible).map(column => column.property);
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

  getSearchResult(res){
    this.showSaveFilter=true;
    this.totalCount= res.totalCount;
    this.pageNumber = res.pageNumber;
    this.pageSize = res.pageSize;
    this.staffList = new MatTableDataSource(res.staffMaster);
    this.getAllStaff = new GetAllStaffModel();
  }

  openSaveFilter(){
    this.dialog.open(SaveFilterComponent, {
      width: '500px',
      data: null
    }).afterClosed().subscribe(res => {
      if(res){
       this.showSaveFilter=false;
       this.showLoadFilter=false;
        this.getAllSearchFilter();
      }
    });
  }

  getAllSearchFilter(){
    this.searchFilterListViewModel.module='Staff';
    this.commonService.getAllSearchFilter(this.searchFilterListViewModel).subscribe((res) => {
      if (typeof (res) === 'undefined') {
        this.snackbar.open('Filter list failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (res._failure) {
          this.searchFilterListViewModel.searchFilterList=[]
        }
        else {
          this.searchFilterListViewModel= res;
          let filterData=this.searchFilterListViewModel.searchFilterList.filter(x=> x.filterId == this.searchFilter.filterId);
          if(filterData.length >0){
            this.searchFilter.jsonList= filterData[0].jsonList;
          }
          if(this.filterJsonParams == null){
            this.searchFilter = this.searchFilterListViewModel.searchFilterList[this.searchFilterListViewModel.searchFilterList.length-1];
          }
        }
      }
    }
    );
  }

  editFilter(){
    this.showAdvanceSearchPanel = true;
    this.filterJsonParams = this.searchFilter;
    this.showSaveFilter = false;
    this.showLoadFilter=false;
  }

  deleteFilter(){
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: '400px',
      data: {
          title: 'Are you sure?',
          message: 'You are about to delete ' + this.searchFilter.filterName + '.'}
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult){
        this.deleteFilterdata(this.searchFilter);
      }
   });
  }

  deleteFilterdata(filterData){
    this.searchFilterAddViewModel.searchFilter = filterData;
    this.commonService.deleteSearchFilter(this.searchFilterAddViewModel).subscribe(
      (res: SearchFilterAddViewModel) => {
        if (typeof(res) === 'undefined'){
          this.snackbar.open('' + sessionStorage.getItem('httpError'), '', {
            duration: 10000
          });
        }
        else{
          if (res._failure) {
            this.snackbar.open('' + res._message, '', {
              duration: 10000
            });
          }
          else {
            this.searchFilter = new SearchFilter();
            this.showLoadFilter=true;
            this.getAllStaff.filterParams= null;
            this.getAllSearchFilter();
            this.callStaffList();
          }
        }
      }
    );
  }

  searchByFilterName(filter){
    this.searchFilter= filter;
    this.showLoadFilter=false;
    this.showSaveFilter=false;
    this.getAllStaff.filterParams = JSON.parse(filter.jsonList);
    this.getAllStaff.sortingModel = null;
    this.staffService.getAllStaffList(this.getAllStaff).subscribe(data => {
      if(data._failure){
        if(data.staffMaster===null){
          this.staffList = new MatTableDataSource([]);   
          this.snackbar.open( data._message, '', {
            duration: 10000
          });
        } else{
          this.staffList = new MatTableDataSource([]);
        }
      }else{
        this.totalCount= data.totalCount;
        this.pageNumber = data.pageNumber;
        this.pageSize = data._pageSize;
        this.staffList = new MatTableDataSource(data.staffMaster);      
        this.getAllStaff = new GetAllStaffModel();    
      }
    });
  }

  ngOnDestroy(){
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }

}
