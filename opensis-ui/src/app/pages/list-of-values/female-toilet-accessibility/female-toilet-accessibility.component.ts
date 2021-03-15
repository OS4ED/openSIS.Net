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
import { EditFemaleToiletAccessibilityComponent } from './edit-female-toilet-accessibility/edit-female-toilet-accessibility.component';
import { CommonService } from '../../../services/common.service';
import { LovAddView, LovList } from '../../../models/lovModel';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { ExcelService } from '../../../services/excel.service';
import { SharedFunction } from '../../shared/shared-function';
import { ConfirmDialogComponent } from '../../shared-module/confirm-dialog/confirm-dialog.component';
import { LoaderService } from '../../../services/loader.service';

@Component({
  selector: 'vex-female-toilet-accessibility',
  templateUrl: './female-toilet-accessibility.component.html',
  styleUrls: ['./female-toilet-accessibility.component.scss'],
  animations: [
    fadeInUp400ms,
    stagger40ms
  ]
})
export class FemaleToiletAccessibilityComponent implements OnInit {
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
  lovName="Female Toilet Accessibility";
  femaleToiletAccessibilityList: MatTableDataSource<any>;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  listCount;

  constructor(
    private router: Router,
    private dialog: MatDialog,
    private snackbar: MatSnackBar,
    public translateService:TranslateService,
    private excelService:ExcelService,
    private commonService:CommonService,
    private loaderService:LoaderService,
    public commonfunction:SharedFunction
    ) {
    translateService.use('en');
    this.loaderService.isLoading.subscribe((val) => {
      this.loading = val;
    }); 
  }

  ngOnInit(): void {
    this.getAllDropdownValues();
  }

  getPageEvent(event){    
    // this.getAllSchool.pageNumber=event.pageIndex+1;
    // this.getAllSchool.pageSize=event.pageSize;
    // this.callAllSchool(this.getAllSchool);
  }

  goToAdd(){
    this.dialog.open(EditFemaleToiletAccessibilityComponent, {
      data:{mod:0},
      width: '500px'
    }).afterClosed().subscribe((data)=>{
      if(data==='submited'){
        this.getAllDropdownValues()
      }
    })
  }
  
  goToEdit(element){
    this.dialog.open(EditFemaleToiletAccessibilityComponent, {
      data:{mod:1,element},
      width: '500px'
    }).afterClosed().subscribe((data)=>{
      if(data==='submited'){
        this.getAllDropdownValues()
      }
    })
  }

  toggleColumnVisibility(column, event) {
    event.stopPropagation();
    event.stopImmediatePropagation();
    column.visible = !column.visible;
  }
  onSearchClear(){
    this.searchKey="";
    this.applyFilter();
  }
  applyFilter(){
    this.femaleToiletAccessibilityList.filter = this.searchKey.trim().toLowerCase()
  }
  get visibleColumns() {
    return this.columns.filter(column => column.visible).map(column => column.property);
  }
  deleteFemaleToiletAccessibilitydata(element){
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
            this.snackbar.open(  res._message, '', {
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
        this.deleteFemaleToiletAccessibilitydata(element);
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
              this.femaleToiletAccessibilityList= new MatTableDataSource(null);
              this.listCount=this.femaleToiletAccessibilityList.data;
              this.snackbar.open( res._message, '', {
                duration: 10000
              });
            } else {
              this.femaleToiletAccessibilityList= new MatTableDataSource(null);
              this.listCount=this.femaleToiletAccessibilityList.data;
            } 
          } 
          else{
            this.femaleToiletAccessibilityList=new MatTableDataSource(res.dropdownList) ;
            this.femaleToiletAccessibilityList.sort=this.sort;    
            this.femaleToiletAccessibilityList.paginator=this.paginator;
            this.listCount=this.femaleToiletAccessibilityList.data.length;
          }
        }
      }
    );
  }

  exportToExcel(){
    if (this.femaleToiletAccessibilityList.data?.length > 0) {
      let reportList = this.femaleToiletAccessibilityList.data?.map((x) => {
        return {
          Title: x.lovColumnValue,
          "Created By": x.createdBy!==null ? x.createdBy: '-',
          "Create Date": this.commonfunction.transformDateWithTime(x.createdOn),
          "Updated By": x.updatedBy!==null ? x.updatedBy: '-',
          "Update Date":  this.commonfunction.transformDateWithTime(x.updatedOn)
        }
      });
      this.excelService.exportAsExcelFile(reportList,"Female_Toilet_Accessibility_List_")
    } else {
      this.snackbar.open('No records found. failed to export female toilet accessibility list', '', {
        duration: 5000
      });
    }
  }


}
