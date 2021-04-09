import { Component, OnInit, Input ,ViewChild} from '@angular/core';
import icMoreVert from '@iconify/icons-ic/twotone-more-vert';
import icAdd from '@iconify/icons-ic/baseline-add';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icDelete from '@iconify/icons-ic/twotone-delete';
import icSearch from '@iconify/icons-ic/search';
import icFilterList from '@iconify/icons-ic/filter-list';
import icImpersonate from '@iconify/icons-ic/twotone-account-circle';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router} from '@angular/router';
import { fadeInUp400ms } from '../../../../@vex/animations/fade-in-up.animation';
import { stagger40ms } from '../../../../@vex/animations/stagger.animation';
import { TranslateService } from '@ngx-translate/core';
import { EditLanguageComponent } from './edit-language/edit-language.component';
import {LanguageModel} from '../../../models/languageModel';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { LoaderService } from '../../../services/loader.service';
import { CommonService } from '../../../services/common.service';
import { MatPaginator } from '@angular/material/paginator';
import {LanguageAddModel} from '../../../models/languageModel';
import { ConfirmDialogComponent } from '../../shared-module/confirm-dialog/confirm-dialog.component';
import { ExcelService } from '../../../services/excel.service';
import { SharedFunction } from '../../shared/shared-function';
import { RolePermissionListViewModel, RolePermissionViewModel } from 'src/app/models/rollBasedAccessModel';
import { CryptoService } from '../../../services/Crypto.service';

@Component({
  selector: 'vex-language',
  templateUrl: './language.component.html',
  styleUrls: ['./language.component.scss'],
  animations: [
    fadeInUp400ms,
    stagger40ms
  ]
})
export class LanguageComponent implements OnInit {
  @Input()
  columns = [
    { label: 'Title', property: 'locale', type: 'text', visible: true },
    { label: 'Short Code', property: 'languageCode', type: 'text', visible: true },
    { label: 'Created By', property: 'createdBy', type: 'text', visible: true },
    { label: 'Create Date', property: 'createdOn', type: 'text', visible: true },
    { label: 'Updated By', property: 'updatedBy', type: 'text', visible: true },
    { label: 'Update Date', property: 'updatedOn', type: 'text', visible: true },
    { label: 'Actions', property: 'actions', type: 'text', visible: true }
  ];

  SchoolClassificationModelList;

  icMoreVert = icMoreVert;
  icAdd = icAdd;
  icEdit = icEdit;
  icDelete = icDelete;
  icSearch = icSearch;
  icImpersonate = icImpersonate;
  icFilterList = icFilterList; 
  loading;  
  totalCount:Number;pageNumber:Number;pageSize:Number;
  languageModel: LanguageModel = new LanguageModel();  
  languageAddModel = new LanguageAddModel();
  languageModelList: MatTableDataSource<any>;
  languageListForExcel = [];
  searchKey:string;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();

  constructor(private router: Router,
    private dialog: MatDialog,
    public translateService:TranslateService,
    public loaderService:LoaderService,
    public commonService:CommonService,
    public snackbar:MatSnackBar,
    private excelService:ExcelService,
    public commonfunction:SharedFunction,
    private cryptoService: CryptoService
    ) {
    translateService.use('en');
    this.loaderService.isLoading.subscribe((val) => {
      this.loading = val;
    });
    this.getLanguageList();
  }

  ngOnInit(): void { 
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 12);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 28);
    const permissionSubCategory = permissionCategory.permissionSubcategory.find( x => x.permissionSubcategoryId === 44);
    this.editPermission = permissionSubCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionSubCategory.rolePermission[0].canDelete;
    this.addPermission = permissionSubCategory.rolePermission[0].canAdd;
   }

  deleteLanguageData(element){
    this.languageAddModel._tenantName = sessionStorage.getItem("tenant");
    this.languageAddModel._userName = sessionStorage.getItem("user");
    this.languageAddModel._token = sessionStorage.getItem("token");
    this.languageAddModel.language.langId = element;
    this.commonService.DeleteLanguage(this.languageAddModel).subscribe(
      (res)=>{
        if(typeof(res)=='undefined'){
          this.snackbar.open('Language Deletion failed. ' + sessionStorage.getItem("httpError"), '', {
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
            this.snackbar.open('' + res._message, '', {
              duration: 10000
            });
            this.getLanguageList()
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
          message: "You are about to delete "+element.locale+"."}
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      if(dialogResult){
        this.deleteLanguageData(element.langId);
      }
   });
  }
  getLanguageList(){
    this.languageModel._tenantName = sessionStorage.getItem("tenant");  
    this.languageModel._userName = sessionStorage.getItem("user");
    this.commonService.GetAllLanguage(this.languageModel).subscribe(data => {
      if (typeof (data) == 'undefined') {
        this.snackbar.open('Language list failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }else{
        if (data._failure) {
          if(data.tableLanguage==null){
            this.languageModelList = new MatTableDataSource(null);
            this.languageModelList.sort=this.sort;
            this.languageModelList.paginator = this.paginator;  
            this.snackbar.open( data._message, '', {
              duration: 10000
            });
          }
          else{
            this.languageModelList = new MatTableDataSource(null);
            this.languageModelList.sort=this.sort;
            this.languageModelList.paginator = this.paginator;  
          }
        }else{ 
          this.languageModelList = new MatTableDataSource(data.tableLanguage);
          this.languageListForExcel= data.tableLanguage;
          this.languageModelList.sort=this.sort; 
          this.languageModelList.paginator = this.paginator;         
        } 
      }
    });
  }

  exportLanguageListToExcel(){
    if(this.languageListForExcel.length!=0){
      let languageList=this.languageListForExcel?.map((item)=>{
        return{
          Title: item.locale,
          ShortName: item.languageCode,
          CreatedBy: item.createdBy!==null ? item.createdBy: '-',
          CreateDate: this.commonfunction.transformDateWithTime(item.createdOn),
          UpdatedBy: item.updatedBy!==null ? item.updatedBy: '-',
          UpdateDate:  this.commonfunction.transformDateWithTime(item.updatedOn)
        }
      });
      this.excelService.exportAsExcelFile(languageList,'Language_List_')
     }
     else{
    this.snackbar.open('No Records Found. Failed to Export Language List','', {
      duration: 5000
    });
  }
}

  goToAdd(){
    this.dialog.open(EditLanguageComponent, {
      
      width: '500px'
    }).afterClosed().subscribe((data) => {
      if(data){
        this.getLanguageList();
      }            
    });   
  }

  goToEdit(editDetails){
    this.dialog.open(EditLanguageComponent, {
      data: editDetails,
      width: '500px'
    }).afterClosed().subscribe((data) => {
      if(data){
        this.getLanguageList();
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

  onSearchClear(){
    this.searchKey="";
    this.applyFilter();
  }

  applyFilter(){
    this.languageModelList.filter = this.searchKey.trim().toLowerCase()
  }

}
