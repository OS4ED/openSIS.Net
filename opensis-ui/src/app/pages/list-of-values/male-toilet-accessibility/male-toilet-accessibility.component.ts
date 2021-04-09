import { Component, OnInit, Input, ViewChild } from '@angular/core';
import icMoreVert from '@iconify/icons-ic/twotone-more-vert';
import icAdd from '@iconify/icons-ic/baseline-add';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icDelete from '@iconify/icons-ic/twotone-delete';
import icSearch from '@iconify/icons-ic/search';
import icFilterList from '@iconify/icons-ic/filter-list';
import icImport from '@iconify/icons-ic/twotone-unarchive';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router} from '@angular/router';
import { fadeInUp400ms } from '../../../../@vex/animations/fade-in-up.animation';
import { stagger40ms } from '../../../../@vex/animations/stagger.animation';
import { TranslateService } from '@ngx-translate/core';
import { EditMaleToiletAccessibilityComponent } from './edit-male-toilet-accessibility/edit-male-toilet-accessibility.component';
import { LovAddView, LovList } from '../../../models/lovModel';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { ExcelService } from '../../../services/excel.service';
import { CommonService } from '../../../services/common.service';
import { SharedFunction } from '../../shared/shared-function';
import { ConfirmDialogComponent } from '../../shared-module/confirm-dialog/confirm-dialog.component';
import { LoaderService } from '../../../services/loader.service';
import { RolePermissionListViewModel, RolePermissionViewModel } from 'src/app/models/rollBasedAccessModel';
import { CryptoService } from '../../../services/Crypto.service';

@Component({
  selector: 'vex-male-toilet-accessibility',
  templateUrl: './male-toilet-accessibility.component.html',
  styleUrls: ['./male-toilet-accessibility.component.scss'],
  animations: [
    fadeInUp400ms,
    stagger40ms
  ]
})
export class MaleToiletAccessibilityComponent implements OnInit {
  @Input()
  columns = [
    { label: 'Accessibility Name', property: 'lovColumnValue', type: 'text', visible: true },
    { label: 'Created By', property: 'createdBy', type: 'text', visible: true },
    { label: 'Created Date', property: 'createdOn', type: 'text', visible: true },
    { label: 'Updated By', property: 'updatedBy', type: 'text', visible: true },
    { label: 'Updated Date', property: 'updatedOn', type: 'text', visible: true },
    { label: 'Actions', property: 'actions', type: 'text', visible: true }
  ];

  EffortGradeScaleModelList;

  icMoreVert = icMoreVert;
  icAdd = icAdd;
  icEdit = icEdit;
  icDelete = icDelete;
  icSearch = icSearch;
  icImport = icImport;
  icFilterList = icFilterList;
  loading:Boolean;
  searchKey:string;
  lovList:LovList= new LovList();
  lovAddView:LovAddView=new LovAddView();
  lovName="Male Toilet Accessibility";
  maleToiletAccessibilityList: MatTableDataSource<any>;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  listCount;
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();

  constructor(
    private router: Router,
    private dialog: MatDialog,
    public translateService:TranslateService,
    private snackbar: MatSnackBar,
    private excelService:ExcelService,
    private commonService:CommonService,
    private loaderService:LoaderService,
    public commonfunction:SharedFunction,
    private cryptoService: CryptoService
    ) {
    translateService.use('en');
    this.loaderService.isLoading.subscribe((val) => {
      this.loading = val;
    }); 
  }

  ngOnInit(): void {
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 12);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 28);
    const permissionSubCategory = permissionCategory.permissionSubcategory.find( x => x.permissionSubcategoryId === 39);
    this.editPermission = permissionSubCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionSubCategory.rolePermission[0].canDelete;
    this.addPermission = permissionSubCategory.rolePermission[0].canAdd;
    this.getAllDropdownValues();
  }

  getPageEvent(event){    
    // this.getAllSchool.pageNumber=event.pageIndex+1;
    // this.getAllSchool.pageSize=event.pageSize;
    // this.callAllSchool(this.getAllSchool);
  }

  goToAdd(){
    this.dialog.open(EditMaleToiletAccessibilityComponent, {
      data:{mod:0},
      width: '500px'
    }).afterClosed().subscribe((data)=>{
      if(data==='submited'){
        this.getAllDropdownValues()
      }
    })
  }
  goToEdit(element){
    this.dialog.open(EditMaleToiletAccessibilityComponent, {
      data:{mod:1,element},
      width: '500px'
    }).afterClosed().subscribe((data)=>{
      if(data==='submited'){
        this.getAllDropdownValues()
      }
    })
  }
  onSearchClear(){
    this.searchKey="";
    this.applyFilter();
  }
  applyFilter(){
    this.maleToiletAccessibilityList.filter = this.searchKey.trim().toLowerCase()
  }

  toggleColumnVisibility(column, event) {
    event.stopPropagation();
    event.stopImmediatePropagation();
    column.visible = !column.visible;
  }

  get visibleColumns() {
    return this.columns.filter(column => column.visible).map(column => column.property);
  }
  deleteMaleToiletAccessibilitydata(element){
    this.lovAddView.dropdownValue.id=element.id
    this.commonService.deleteDropdownValue(this.lovAddView).subscribe(
      (res:LovAddView)=>{
        if(typeof(res)=='undefined'){
          this.snackbar.open( sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else{
          if (res._failure) {
            this.snackbar.open( res._message, '', {
              duration: 10000
            });
          } 
          else { 
            this.snackbar.open( res._message, '', {
              duration: 10000
            });
            this.getAllDropdownValues()
          }
        }
      }
    )
  }
  
  confirmDelete(element){
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: {
          title: "Are you sure?",
          message: "You are about to delete "+element.lovColumnValue+"."}
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      if(dialogResult){
        this.deleteMaleToiletAccessibilitydata(element);
      }
   });
  }
  getAllDropdownValues(){
    this.lovList.lovName=this.lovName;
    this.commonService.getAllDropdownValues(this.lovList).subscribe(
      (res:LovList)=>{
        if(typeof(res)=='undefined'){
          this.snackbar.open('' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else{
          if (res._failure) {  
            if (res.dropdownList == null) {
              this.maleToiletAccessibilityList= new MatTableDataSource(null);
              this.listCount=this.maleToiletAccessibilityList.data;
              this.snackbar.open( res._message, '', {
                duration: 10000
              });
            } else {
              this.maleToiletAccessibilityList= new MatTableDataSource(null);
              this.listCount=this.maleToiletAccessibilityList.data;
            }
          } 
          else{
            this.maleToiletAccessibilityList=new MatTableDataSource(res.dropdownList) ;
            this.maleToiletAccessibilityList.sort=this.sort;    
            this.maleToiletAccessibilityList.paginator=this.paginator;
            this.listCount=this.maleToiletAccessibilityList.data.length;
          }
        }
      }
    );
  }
  exportToExcel(){
    if (this.maleToiletAccessibilityList.data?.length > 0) {
      let reportList = this.maleToiletAccessibilityList.data?.map((x) => {
        return {
          Title: x.lovColumnValue,
          "Created By": x.createdBy!==null ? x.createdBy: '-',
          "Create Date": this.commonfunction.transformDateWithTime(x.createdOn),
          "Updated By": x.updatedBy!==null ? x.updatedBy: '-',
          "Update Date":  this.commonfunction.transformDateWithTime(x.updatedOn)
        }
      });
      this.excelService.exportAsExcelFile(reportList,"Male_Toilet_Accessibility_List_")
    } else {
      this.snackbar.open('No records found. failed to export male toilet accessibility list', '', {
        duration: 5000
      });
    }
  }

}
