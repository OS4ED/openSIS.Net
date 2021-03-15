import { LovList, LovAddView } from './../../../models/lovModel';
import { CommonService } from './../../../services/common.service';
import { Component, OnInit, Input, ViewChild } from '@angular/core';
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
import { EditSchoolLevelComponent } from './edit-school-level/edit-school-level.component';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { LoaderService } from './../../../services/loader.service';
import { ConfirmDialogComponent } from '../../shared-module/confirm-dialog/confirm-dialog.component';
import { MatPaginator } from '@angular/material/paginator';
import { ExcelService } from '../../../services/excel.service';
import { SharedFunction } from '../../shared/shared-function';

@Component({
  selector: 'vex-school-level',
  templateUrl: './school-level.component.html',
  styleUrls: ['./school-level.component.scss'],
  animations: [
    fadeInUp400ms,
    stagger40ms
  ]
})
export class SchoolLevelComponent implements OnInit {
  @Input()
  columns = [
    { label: 'Title', property: 'lovColumnValue', type: 'text', visible: true },
    { label: 'Created By', property: 'createdBy', type: 'text', visible: true },
    { label: 'Create Date', property: 'createdOn', type: 'text', visible: true },
    { label: 'Updated By', property: 'updatedBy', type: 'text', visible: true },
    { label: 'Update Date', property: 'updatedOn', type: 'text', visible: true },
    { label: 'Actions', property: 'actions', type: 'text', visible: true }
  ];


  icMoreVert = icMoreVert;
  icAdd = icAdd;
  icEdit = icEdit;
  icDelete = icDelete;
  icSearch = icSearch;
  icImpersonate = icImpersonate;
  icFilterList = icFilterList;
  loading:Boolean;
  searchKey:string;
  lovAddView:LovAddView= new LovAddView();
  lovList:LovList= new LovList();
  lovName="School Level";
  schoolLevelList: MatTableDataSource<any>;
  schoolLevelListForExcel=[];
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  listCount;

  constructor(
    private router: Router,
    private dialog: MatDialog,
    private snackbar: MatSnackBar,
    private commonService:CommonService,
    private loaderService:LoaderService,
    public translateService:TranslateService,
    private excelService:ExcelService,
    public commonfunction:SharedFunction
    ) {
    translateService.use('en');
    this.loaderService.isLoading.subscribe((val) => {
      this.loading = val;
    }); 
  }

  ngOnInit(): void {
    this.getAllSchoolLevel();
  }

  getPageEvent(event){    
    // this.getAllSchool.pageNumber=event.pageIndex+1;
    // this.getAllSchool.pageSize=event.pageSize;
    // this.callAllSchool(this.getAllSchool);
  }
  onSearchClear(){
    this.searchKey="";
    this.applyFilter();
  }
  applyFilter(){
    this.schoolLevelList.filter = this.searchKey.trim().toLowerCase()
  }
  goToAdd(){
    this.dialog.open(EditSchoolLevelComponent, {
      width: '500px'
    }).afterClosed().subscribe((data)=>{
      if(data==='submited'){
        this.getAllSchoolLevel()
      }
    })
  }
  goToEdit(element){
    this.dialog.open(EditSchoolLevelComponent,{
      data: element,
      width:'500px'
    }).afterClosed().subscribe((data)=>{
      if(data==='submited'){
        this.getAllSchoolLevel()
      }
    })
  }

  toggleColumnVisibility(column, event) {
    event.stopPropagation();
    event.stopImmediatePropagation();
    column.visible = !column.visible;
  }

  get visibleColumns() {
    return this.columns.filter(column => column.visible).map(column => column.property);
  }
  deleteSchoolLeveldata(element){
    this.lovAddView.dropdownValue.id=element.id
    this.commonService.deleteDropdownValue(this.lovAddView).subscribe(
      (res:LovAddView)=>{
        if(typeof(res)=='undefined'){
          this.snackbar.open('School Level Deletion failed. ' + sessionStorage.getItem("httpError"), '', {
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
            this.snackbar.open(''+ res._message , '', {
              duration: 10000
            });
            this.getAllSchoolLevel()
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
        this.deleteSchoolLeveldata(element);
      }
   });
  }

  getAllSchoolLevel(){
    this.lovList.lovName=this.lovName;
    this.commonService.getAllDropdownValues(this.lovList).subscribe(
      (res:LovList)=>{
        if(typeof(res)=='undefined'){
          this.snackbar.open('No Record Found For School Level.. ' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else{
          if (res._failure) {  
            if (res.dropdownList == null) {
              this.schoolLevelList= new MatTableDataSource(null);
              this.listCount=this.schoolLevelList.data;
              this.snackbar.open( res._message, '', {
                duration: 10000
              });
            } else {
              this.schoolLevelList= new MatTableDataSource(null);
              this.listCount=this.schoolLevelList.data;
            }
          } 
          else { 
            this.schoolLevelList=new MatTableDataSource(res.dropdownList) ;
            this.schoolLevelListForExcel = res.dropdownList;
            this.schoolLevelList.sort=this.sort;  
            this.schoolLevelList.paginator=this.paginator; 
            this.listCount=this.schoolLevelList.data.length;
          }
        }
      }
    );
  }

  exportSchoolLevelListToExcel(){
    if(this.schoolLevelListForExcel.length!=0){
      let schoolLevelList=this.schoolLevelListForExcel?.map((item)=>{
        return{
          Title: item.lovColumnValue,
          CreatedBy: item.createdBy!==null ? item.createdBy: '-',
          CreateDate: this.commonfunction.transformDateWithTime(item.createdOn),
          UpdatedBy: item.updatedBy!==null ? item.updatedBy: '-',
          UpdateDate:  this.commonfunction.transformDateWithTime(item.updatedOn)
        }
      });
      this.excelService.exportAsExcelFile(schoolLevelList,'School_Level_List_')
     }
     else{
    this.snackbar.open('No Records Found. Failed to Export School Level List','', {
      duration: 5000
    });
  }
}

}
