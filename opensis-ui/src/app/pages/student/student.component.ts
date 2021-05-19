import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { fadeInRight400ms } from 'src/@vex/animations/fade-in-right.animation';
import { fadeInUp400ms } from 'src/@vex/animations/fade-in-up.animation';
import { stagger40ms } from 'src/@vex/animations/stagger.animation';
import icMoreVert from '@iconify/icons-ic/twotone-more-vert';
import icAdd from '@iconify/icons-ic/baseline-add';
import icSearch from '@iconify/icons-ic/search';
import icFilterList from '@iconify/icons-ic/filter-list';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router} from '@angular/router';
import { SelectionModel } from '@angular/cdk/collections';
import { MatPaginator} from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormControl, NgForm } from '@angular/forms';
import { debounceTime, distinctUntilChanged, takeUntil } from 'rxjs/operators';
import { LayoutService } from 'src/@vex/services/layout.service';
import icImpersonate from '@iconify/icons-ic/twotone-account-circle';
import { TranslateService } from '@ngx-translate/core';
import { Subject } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { RolePermissionListViewModel, RolePermissionViewModel } from 'src/app/models/roll-based-access.model';
import { RollBasedAccessService } from 'src/app/services/roll-based-access.service';
import { CryptoService } from 'src/app/services/Crypto.service';
import { SearchFilter, SearchFilterAddViewModel, SearchFilterListViewModel } from 'src/app/models/search-filter.model';
import { ModuleIdentifier } from 'src/app/enums/module-identifier.enum';
import { SchoolCreate } from 'src/app/enums/school-create.enum';
import { LoaderService } from 'src/app/services/loader.service';
import { ImageCropperService } from 'src/app/services/image-cropper.service';
import { ExcelService } from 'src/app/services/excel.service';
import { CommonService } from 'src/app/services/common.service';
import { StudentListModel } from 'src/app/models/student.model';
import { StudentService } from 'src/app/services/student.service';
import { ConfirmDialogComponent } from '../shared-module/confirm-dialog/confirm-dialog.component';
import { SaveFilterComponent } from './save-filter/save-filter.component';

@Component({
  selector: 'vex-student',
  templateUrl: './student.component.html',
  styleUrls: ['./student.component.scss'],
  animations: [
    fadeInUp400ms,
    stagger40ms,
    fadeInRight400ms
  ]
})
export class StudentComponent implements OnInit, OnDestroy {
  columns = [
    { label: 'Name', property: 'firstGivenName', type: 'text', visible: true },
    { label: 'Student ID', property: 'studentId', type: 'text', visible: true },
    { label: 'Alternate ID', property: 'alternateId', type: 'text', visible: true },
    { label: 'Grade Level', property: 'gradeLevelTitle', type: 'text', visible: true },
    { label: 'Email', property: 'schoolEmail', type: 'text', visible: true },
    { label: 'Telephone', property: 'homePhone', type: 'text', visible: true },
    { label: 'Status', property: 'status', type: 'text', visible: false },
    { label: 'Action', property: 'action', type: 'text', visible: true }
  ];
  icImpersonate = icImpersonate;
  selection = new SelectionModel<any>(true, []);
  totalCount:number=0;
  pageNumber:number;
  pageSize:number;
  searchCtrl: FormControl;
  icMoreVert = icMoreVert;
  icAdd = icAdd;
  icSearch = icSearch;
  icFilterList = icFilterList;
  fapluscircle = "fa-plus-circle";
  tenant = "";
  filterValue:number;
  searchFilter: SearchFilter= new SearchFilter();
  filterJsonParams;
  loading:boolean;
  showSaveFilter:boolean = false;
  allStudentList=[];
  destroySubject$: Subject<void> = new Subject();
  getAllStudent: StudentListModel = new StudentListModel();
  searchFilterAddViewModel: SearchFilterAddViewModel = new SearchFilterAddViewModel();
  searchFilterListViewModel: SearchFilterListViewModel= new SearchFilterListViewModel();
  StudentModelList: MatTableDataSource<any>;
  showAdvanceSearchPanel: boolean = false;
  moduleIdentifier=ModuleIdentifier;
  createMode=SchoolCreate;
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  searchValue: any = null;
  toggleValues: any = null;
  searchCount: number = null;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator; 
  @ViewChild(MatSort) sort: MatSort;
  showLoadFilter = true;
  constructor(
              private studentService: StudentService,
              private snackbar: MatSnackBar,
              private router: Router,
              private loaderService:LoaderService,
              private imageCropperService:ImageCropperService,
              private layoutService: LayoutService,
              private excelService: ExcelService,
              private cryptoService: CryptoService,
              public translateService: TranslateService,
              public rollBasedAccessService: RollBasedAccessService,
              private dialog: MatDialog,
              private commonService: CommonService
  ) {
    translateService.use('en');
    this.getAllStudent.filterParams=null;
    if(localStorage.getItem("collapseValue") !== null){
      if( localStorage.getItem("collapseValue") === "false"){
        this.layoutService.expandSidenav();
      }else{
        this.layoutService.collapseSidenav();
      } 
    }else{
      this.layoutService.expandSidenav();
    }
    this.loaderService.isLoading.pipe(takeUntil(this.destroySubject$)).subscribe((val) => {
        this.loading = val;
      });
    this.callAllStudent();
    }

  ngOnInit(): void {
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 3);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 5);
    let permissionSubCategory = permissionCategory.permissionSubcategory.find( x => x.permissionSubcategoryId === 3);
    this.editPermission = permissionSubCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionSubCategory.rolePermission[0].canDelete;
    this.addPermission = permissionSubCategory.rolePermission[0].canAdd;
    let viewPermission= permissionCategory.rolePermission[0].canView;
    if(!viewPermission){
      this.router.navigate(['/']);
     }
    this.searchCtrl = new FormControl();
    this.getAllSearchFilter();
  }

  getSearchResult(res){
    if (res.totalCount){
      this.searchCount = res.totalCount;
      this.totalCount = res.totalCount;
    }
    else{
      this.searchCount = 0;
      this.totalCount = 0;
    }
    this.showSaveFilter = true;
    this.pageNumber = res.pageNumber;
    this.pageSize = res.pageSize;
    this.StudentModelList = new MatTableDataSource(res.studentMaster); 
    this.getAllStudent = new StudentListModel();
  }
  getToggleValues(event){
    this.toggleValues = event;
    if (event.inactiveStudents === true){
      this.columns[6].visible = true;
    }
    else if (event.inactiveStudents === false){
      this.columns[6].visible = false;
    }
  }
  getSearchInput(event){
    this.searchValue = event;
  }
  resetStudentList(){
    this.searchCount = null;
    this.searchValue = null;
    this.callAllStudent();
  }

  ngAfterViewInit() {
    //  Sorting
    this.getAllStudent=new StudentListModel();
    this.sort.sortChange.subscribe((res) => {
      this.getAllStudent.pageNumber=this.pageNumber
      this.getAllStudent.pageSize=this.pageSize;
      this.getAllStudent.sortingModel.sortColumn=res.active;
      if(this.searchCtrl.value!=null && this.searchCtrl.value!=""){
        let filterParams=[
          {
            columnName:null,
            filterValue:this.searchCtrl.value,
            filterOption:3
          }
        ]
        Object.assign(this.getAllStudent,{filterParams: filterParams});
      }
      if(res.direction==""){
        this.getAllStudent.sortingModel=null;
        this.callAllStudent();
        this.getAllStudent=new StudentListModel();
        this.getAllStudent.sortingModel=null;
      }else{
        this.getAllStudent.sortingModel.sortDirection=res.direction;
        this.callAllStudent();
      }
    });
      //  Searching
    this.searchCtrl.valueChanges.pipe(debounceTime(500),distinctUntilChanged()).subscribe((term)=>{
      if(term!='')
      {
          let filterParams=[
          {
            columnName:null,
            filterValue:term,
            filterOption:3
          }
        ]
          if(this.sort.active!=undefined && this.sort.direction!=""){
          this.getAllStudent.sortingModel.sortColumn=this.sort.active;
          this.getAllStudent.sortingModel.sortDirection=this.sort.direction;
        }
          Object.assign(this.getAllStudent,{filterParams: filterParams});
          this.getAllStudent.pageNumber=1;
          this.paginator.pageIndex=0;
          this.getAllStudent.pageSize=this.pageSize;
          this.callAllStudent();
        }
        else
        {
        Object.assign(this.getAllStudent,{filterParams: null});
        this.getAllStudent.pageNumber=this.paginator.pageIndex+1;
        this.getAllStudent.pageSize=this.pageSize;
        if(this.sort.active!=undefined && this.sort.direction!=""){
            this.getAllStudent.sortingModel.sortColumn=this.sort.active;
            this.getAllStudent.sortingModel.sortDirection=this.sort.direction;
          }
        this.callAllStudent();
        }
      })
    }

  goToAdd(){
    this.router.navigate(["school/students/student-generalinfo"]).then(()=>{
    this.imageCropperService.enableUpload({module:this.moduleIdentifier.STUDENT,upload:true,mode:this.createMode.ADD});
    });
  }

  navigateToSetting(){
    localStorage.setItem('pageId','Student Bulk Data Import')
    this.router.navigate(["school/settings/student-settings"]);
  }

  saveFilter(){
    this.dialog.open(SaveFilterComponent, {
      width: '500px',
      data: null
    }).afterClosed().subscribe(data => {
      if(data==='submited'){
        this.showSaveFilter=false;
        this.showLoadFilter=false;
        this.getAllSearchFilter();
      }
    });
  }

  getAllSearchFilter(){
    this.searchFilterListViewModel.module='Student';
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
            this.getAllSearchFilter();
            this.getAllStudent.filterParams= null;
            this.callAllStudent();
            this.searchFilter = new SearchFilter();
            this.showLoadFilter=true;
          }
        }
      }
    );
  }

  searchByFilterName(filter){
    this.searchFilter= filter;
    this.showLoadFilter=false;
    this.showSaveFilter=false;
    this.getAllStudent.filterParams = JSON.parse(filter.jsonList);
    this.getAllStudent.sortingModel = null;
    this.studentService.GetAllStudentList(this.getAllStudent).subscribe(data => {
      if(data._failure){
        if(data.studentMaster===null){
            this.StudentModelList = new MatTableDataSource([]);  
            this.snackbar.open(data._message, '', {
              duration: 10000
            }); 
        } else{
          this.StudentModelList = new MatTableDataSource([]);
        }
      }else{
        this.totalCount= data.totalCount;
        this.pageNumber = data.pageNumber;
        this.pageSize = data._pageSize;
        this.StudentModelList = new MatTableDataSource(data.studentMaster);      
        this.getAllStudent=new StudentListModel(); 
      }
    });
  }


  viewStudentDetails(id){  
    this.imageCropperService.enableUpload({module:this.moduleIdentifier.STUDENT,upload:true,mode:this.createMode.VIEW});
    this.studentService.setStudentId(id);
    this.checkViewPermission();
    // this.router.navigate(["school/students/student-generalinfo"]); 
  }

  checkViewPermission(){
    let categoryId;
    let categoryName;
    
     for (const permission of this.permissionListViewModel.permissionList[2].permissionGroup.permissionCategory[0].permissionSubcategory){
      if(permission.rolePermission[0].canView){
       categoryId=permission.permissionSubcategoryId;
       categoryName=permission.title;
       break;
      }
    }
    if(categoryId){
      this.checkCurrentCategoryAndRoute(categoryId,categoryName);
    }

  }

  checkCurrentCategoryAndRoute(categoryId,categoryName) {
    this.studentService.setCategoryTitle(categoryName);
    this.studentService.setCategoryId(0);
    this.commonService.setModuleName('Student');
    if(categoryId === 3) {
      this.router.navigate(['/school', 'students', 'student-generalinfo']);
    } else if(categoryId === 4 ) {
      this.router.navigate(['/school', 'students', 'student-enrollmentinfo']);
    } else if(categoryId === 5 ) {
        this.router.navigate(['/school', 'students', 'student-address-contact']);
    } else if(categoryId === 6 ) {
      this.router.navigate(['/school', 'students', 'student-familyinfo']);
    } else if(categoryId === 7 ) {
      this.router.navigate(['/school', 'students', 'student-medicalinfo']);
    }
     else if(categoryId === 8 ) {
      this.router.navigate(['/school', 'students', 'student-comments']);
    } else if(categoryId === 9 ) {
      this.router.navigate(['/school', 'students', 'student-documents']);
    } else if(categoryId === 100 ) {
      this.router.navigate(['/school', 'students', 'student-course-schedule']);
    }  else if(categoryId === 101 ) {
      this.router.navigate(['/school', 'students', 'student-attendance']);
    }  else if(categoryId === 102 ) {
      this.router.navigate(['/school', 'students', 'student-transcript']);
    }  else if(categoryId === 103 ) {
      this.router.navigate(['/school', 'students', 'student-report-card']);
    }
    else if(categoryId > 9 ) {
      this.router.navigate(['/school', 'students', 'custom', categoryName.trim().toLowerCase().split(' ').join('-')]);
    }

  }

  getPageEvent(event){
    if(this.sort.active!=undefined && this.sort.direction!=""){
      this.getAllStudent.sortingModel.sortColumn=this.sort.active;
      this.getAllStudent.sortingModel.sortDirection=this.sort.direction;
    }
    if(this.searchCtrl.value!=null && this.searchCtrl.value!=""){
      let filterParams=[
        {
         columnName:null,
         filterValue:this.searchCtrl.value,
         filterOption:3
        }
      ]
      Object.assign(this.getAllStudent,{filterParams: filterParams});
    }
    this.getAllStudent.pageNumber=event.pageIndex+1;
    this.getAllStudent.pageSize=event.pageSize;
    this.callAllStudent();
  }

  callAllStudent(){
    if(this.getAllStudent.sortingModel?.sortColumn==""){
      this.getAllStudent.sortingModel = null;
    }
    this.studentService.GetAllStudentList(this.getAllStudent).subscribe(data => {
      if (data._failure){
        if (data.studentMaster === null){
          this.totalCount = null;
          this.StudentModelList = new MatTableDataSource([]);
          this.snackbar.open( data._message, '', {
              duration: 10000
            });
        } else{
          this.StudentModelList = new MatTableDataSource([]);
          this.totalCount = null;
        }
      }else{
        this.totalCount = data.totalCount;
        this.pageNumber = data.pageNumber;
        this.pageSize = data._pageSize;
        this.StudentModelList = new MatTableDataSource(data.studentMaster);
        this.getAllStudent = new StudentListModel();
      }
    });
  }

  exportStudentListToExcel(){
    const getAllStudent: StudentListModel = new StudentListModel();
    getAllStudent.pageNumber = 0;
    getAllStudent.pageSize = 0;
    getAllStudent.sortingModel = null;
    this.studentService.GetAllStudentList(getAllStudent).subscribe(res => {
        if (res._failure){
          this.snackbar.open('Failed to Export School List.'+ res._message, '', {
          duration: 10000
          });
        }else{
          if (res.studentMaster.length > 0){
            let studentList;
            studentList = res.studentMaster?.map((x)=>{
             const middleName = x.middleName == null?' ':' '+x.middleName+' ';
             return {
                Name: x.firstGivenName + middleName + x.lastFamilyName,
                'Student ID': x.studentInternalId,
                'Alternate ID': x.alternateId,
                'Grade Level': x.studentEnrollment[0]?.gradeLevelTitle,
                Email: x.schoolEmail,
                Telephone: x.mobilePhone
              };
            });
            this.excelService.exportAsExcelFile(studentList,'Students_List_')
          }else{
            this.snackbar.open('No Records Found. Failed to Export Students List','', {
              duration: 5000
            });
          }
        }
      });
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


  get visibleColumns() {
    return this.columns.filter(column => column.visible).map(column => column.property);
  }

  onFilterChange(value: string) {
    if (!this.StudentModelList) {
      return;
    }
    value = value.trim();
    value = value.toLowerCase();
    this.StudentModelList.filter = value;
  }

  toggleColumnVisibility(column, event) {
    event.stopPropagation();
    event.stopImmediatePropagation();
    column.visible = !column.visible;
  }

  ngOnDestroy(){
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }

}