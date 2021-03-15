import { Component, OnInit, Input,ViewChild } from '@angular/core';
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
import { EditMaleToiletTypeComponent } from './edit-male-toilet-type/edit-male-toilet-type.component';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { LoaderService } from './../../../services/loader.service';
import { ConfirmDialogComponent } from '../../shared-module/confirm-dialog/confirm-dialog.component';
import { LovList, LovAddView } from './../../../models/lovModel';
import { CommonService } from './../../../services/common.service';
import { MatPaginator } from '@angular/material/paginator';
import { ExcelService } from '../../../services/excel.service';
import { SharedFunction } from '../../shared/shared-function';

@Component({
  selector: 'vex-male-toilet-type',
  templateUrl: './male-toilet-type.component.html',
  styleUrls: ['./male-toilet-type.component.scss'],
  animations: [
    fadeInUp400ms,
    stagger40ms
  ]
})
export class MaleToiletTypeComponent implements OnInit {
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
  maleToiletTypeListForExcel =[];
  lovName="Male Toilet Type";
  maleToiletTypeList: MatTableDataSource<any>;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  listCount;

  constructor(
    private router: Router,
    private dialog: MatDialog,
    public translateService:TranslateService,
    private snackbar: MatSnackBar,
    private commonService:CommonService,
    private loaderService:LoaderService,
    private excelService:ExcelService,
    public commonfunction:SharedFunction
    ) {
    translateService.use('en');
    this.loaderService.isLoading.subscribe((val) => {
      this.loading = val;
    }); 
  }

  ngOnInit(): void {
    this.getAllMaleToiletType();
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
    this.maleToiletTypeList.filter = this.searchKey.trim().toLowerCase()
  }
  goToAdd(){
    this.dialog.open(EditMaleToiletTypeComponent, {
      width: '500px'
    }).afterClosed().subscribe((data)=>{
      if(data==='submited'){
        this.getAllMaleToiletType();
      }
    })
  }
  goToEdit(element){
    this.dialog.open(EditMaleToiletTypeComponent,{
      data: element,
      width:'500px'
    }).afterClosed().subscribe((data)=>{
      if(data==='submited'){
        this.getAllMaleToiletType()
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
  deleteMaleToiletTypedata(element){
    this.lovAddView.dropdownValue.id=element.id
    this.commonService.deleteDropdownValue(this.lovAddView).subscribe(
      (res:LovAddView)=>{
        if(typeof(res)=='undefined'){
          this.snackbar.open('Male Toilet Type Deletion failed. ' + sessionStorage.getItem("httpError"), '', {
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
            this.getAllMaleToiletType()
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
        this.deleteMaleToiletTypedata(element);
      }
   });
  }

  getAllMaleToiletType(){
    this.lovList.lovName=this.lovName;
    this.commonService.getAllDropdownValues(this.lovList).subscribe(
      (res:LovList)=>{
        if(typeof(res)=='undefined'){
          this.snackbar.open('Male Toilet Type List failed. ' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else{
          if (res._failure) {  
            if (res.dropdownList == null) {
              this.maleToiletTypeList= new MatTableDataSource(null);
              this.listCount=this.maleToiletTypeList.data;
              this.snackbar.open( res._message, '', {
                duration: 10000
              });
            } else {
              this.maleToiletTypeList= new MatTableDataSource(null);
              this.listCount=this.maleToiletTypeList.data;
            }
          } 
          else { 
            this.maleToiletTypeList=new MatTableDataSource(res.dropdownList) ;
            this.maleToiletTypeListForExcel= res.dropdownList;
            this.maleToiletTypeList.sort=this.sort; 
            this.maleToiletTypeList.paginator=this.paginator; 
            this.listCount=this.maleToiletTypeList.data.length;  
          }
        }
      }
    );
  }
  exportMaleToiletListToExcel(){
    if(this.maleToiletTypeListForExcel.length!=0){
      let maleToiletTypeList=this.maleToiletTypeListForExcel?.map((item)=>{
        return{
          Title: item.lovColumnValue,
          CreatedBy: item.createdBy!==null ? item.createdBy: '-',
          CreateDate: this.commonfunction.transformDateWithTime(item.createdOn),
          UpdatedBy: item.updatedBy!==null ? item.updatedBy: '-',
          UpdateDate:  this.commonfunction.transformDateWithTime(item.updatedOn)
        }
      });
      this.excelService.exportAsExcelFile(maleToiletTypeList,'Male_Toilet_Type_List_')
     }
     else{
    this.snackbar.open('No Records Found. Failed to Export Male Toilet Type List','', {
      duration: 5000
    });
  }
}
}
