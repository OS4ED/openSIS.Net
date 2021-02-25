import { Component, OnInit, ViewChild } from '@angular/core';
import icAdd from '@iconify/icons-ic/baseline-add';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icDelete from '@iconify/icons-ic/twotone-delete';
import icSearch from '@iconify/icons-ic/search';
import icFilterList from '@iconify/icons-ic/filter-list';
import { fadeInUp400ms } from '../../../../@vex/animations/fade-in-up.animation';
import { stagger40ms } from '../../../../@vex/animations/stagger.animation';
import { TranslateService } from '@ngx-translate/core';
import { MatDialog } from '@angular/material/dialog';
import { EditBlockComponent } from './edit-block/edit-block.component';
import { LayoutService } from '../../../../@vex/services/layout.service';
import { SchoolPeriodService } from '../../../services/school-period.service';
import { BlockAddViewModel, BlockListViewModel, BlockPeriodAddViewModel, BlockPeriodSortOrderViewModel } from '../../../models/schoolPeriodModel';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { ConfirmDialogComponent } from '../../shared-module/confirm-dialog/confirm-dialog.component';
import { MatSort } from '@angular/material/sort';
import { EditPeriodComponent } from './edit-period/edit-period.component';
import { LoaderService } from '../../../services/loader.service';
import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { ExcelService } from '../../../services/excel.service';
import * as moment from 'moment';

@Component({
  selector: 'vex-periods',
  templateUrl: './periods.component.html',
  styleUrls: ['./periods.component.scss'],
  animations: [
    fadeInUp400ms,
    stagger40ms
  ]
})
export class PeriodsComponent implements OnInit {

  icAdd = icAdd;
  icEdit = icEdit;
  icDelete = icDelete;
  icSearch = icSearch;
  icFilterList = icFilterList;
  blockPeriodList: MatTableDataSource<any>;
  @ViewChild(MatSort) sort: MatSort;
  blockCount:number;
  currentBlockName:string;
  blockListViewModel: BlockListViewModel = new BlockListViewModel();
  blockPeriodSortOrderViewModel: BlockPeriodSortOrderViewModel = new BlockPeriodSortOrderViewModel();
  blockPeriodAddViewModel: BlockPeriodAddViewModel = new BlockPeriodAddViewModel();
  blockAddViewModel: BlockAddViewModel = new BlockAddViewModel();
  currentBlockId: number = null;
  loading: boolean;
  columns = [
    { label: 'ID', property: 'periodId', type: 'number', visible: true },
    { label: 'Title', property: 'periodTitle', type: 'text', visible: true },
    { label: 'Short Name', property: 'periodShortName', type: 'text', visible: true },
    { label: 'Start Time', property: 'periodStartTime', type: 'text', visible: true },
    { label: 'End Time', property: 'periodEndTime', type: 'text', visible: true },
    { label: 'Length', property: 'length', type: 'number', visible: true },
    { label: 'Calculate Attendance', property: 'calculateAttendance', type: 'text', visible: true },
    { label: 'action', property: 'action', type: 'text', visible: true }
  ];
  searchKey: string;

  constructor(public translateService: TranslateService, private dialog: MatDialog,
    private snackbar: MatSnackBar,
    private layoutService: LayoutService,
    private schoolPeriodService: SchoolPeriodService,
    private excelService : ExcelService,
    private loaderService: LoaderService) {
    translateService.use('en');
    if (localStorage.getItem("collapseValue") !== null) {
      if (localStorage.getItem("collapseValue") === "false") {
        this.layoutService.expandSidenav();
      } else {
        this.layoutService.collapseSidenav();
      }
    } else {
      this.layoutService.expandSidenav();
    }
    this.loaderService.isLoading.subscribe((val) => {
      this.loading = val;
    });
  }

  ngOnInit(): void {
    this.getAllBlockList();
  }

  selectBlock(element) {
    this.currentBlockId = element.blockId;
    let periodList = element.blockPeriod?.map(function (item) {
      return {
        blockId: item.blockId,
        periodId: item.periodId,
        periodTitle: item.periodTitle,
        periodShortName: item.periodShortName,
        periodStartTime: new Date("1900-01-01T" + item.periodStartTime),
        periodEndTime:  new Date("1900-01-01T" + item.periodEndTime),
        calculateAttendance: item.calculateAttendance,
        sortOrder:item.periodSortOrder,
        length: Math.round((new Date("1900-01-01T" + item.periodEndTime).getTime() - new Date("1900-01-01T" + item.periodStartTime).getTime()) / 60000)
      };
    });
    this.blockPeriodList = new MatTableDataSource(periodList);
    this.blockPeriodList.sort = this.sort;
  }
  getAllBlockList() {
    this.blockListViewModel.getBlockListForView = [];
    this.blockListViewModel.tenantId = sessionStorage.getItem('tenantId');
    this.blockListViewModel.schoolId = +sessionStorage.getItem('selectedSchoolId');
    this.schoolPeriodService.getAllBlockList(this.blockListViewModel).subscribe(
      (res: BlockListViewModel) => {
        if (typeof (res) == 'undefined') {
          this.snackbar.open('Block/Rotation Days list failed. ' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else {
          if (res._failure) {
            this.snackbar.open('Block/Rotation Days list failed. ' + res._message, '', {
              duration: 10000
            });
          }
          else {
            this.blockListViewModel = res;
            this.blockCount= res.getBlockListForView.length;
            if (this.currentBlockId == null) {
              this.currentBlockId = res.getBlockListForView[0].blockId;
              this.currentBlockName = res.getBlockListForView[0].blockTitle;
              let periodList= this.itemInPeriodList(0 , res);
              this.blockPeriodList = new MatTableDataSource(periodList);
              this.blockPeriodList.sort = this.sort;
            }
            else {
              let index = this.blockListViewModel.getBlockListForView.findIndex((x) => {
                return x.blockId === this.currentBlockId
              });
              let periodList= this.itemInPeriodList(index, res);
              this.blockPeriodList = new MatTableDataSource(periodList);
              this.blockPeriodList.sort = this.sort;
            }
          }
        }
      }
    );
  }

  itemInPeriodList(index=0,response){
    let periodList = response.getBlockListForView[index].blockPeriod?.map(function (item) {
      return {
        blockId: item.blockId,
        periodId: item.periodId,
        periodTitle: item.periodTitle,
        periodShortName: item.periodShortName,
        periodStartTime: new Date("1900-01-01T" + item.periodStartTime),
        periodEndTime:  new Date("1900-01-01T" + item.periodEndTime),
        calculateAttendance: item.calculateAttendance,
        sortOrder:item.periodSortOrder,
        length: Math.round((new Date("1900-01-01T" + item.periodEndTime).getTime() - new Date("1900-01-01T" + item.periodStartTime).getTime()) / 60000)
      };
    });
    return periodList;
  }

  excelPeriodList(index=0,response){
    
    let periodList = response.getBlockListForView[index].blockPeriod?.map(function (item) {

      return {
        Title: item.periodTitle,
        ShortName: item.periodShortName,
        StartTime: moment(new Date("1900-01-01T" + item.periodStartTime), ["YYYY-MM-DD hh:mm:ss"]).format("hh:mm A"),
        EndTime:  moment(new Date("1900-01-01T" + item.periodEndTime), ["YYYY-MM-DD hh:mm:ss"]).format("hh:mm A"),
        Length: Math.round((new Date("1900-01-01T" + item.periodEndTime).getTime() - new Date("1900-01-01T" + item.periodStartTime).getTime()) / 60000),
        CalculateAttendance: item.calculateAttendance?'Yes':'No'
      };
    });
    return periodList;
  }


  editBlock(element) {
    this.dialog.open(EditBlockComponent, {
      data: element,
      width: '500px'
    }).afterClosed().subscribe((data) => {
      if (data === 'submited') {
        this.getAllBlockList();
      }
    })
  }
  deleteBlock(element) {
    this.blockAddViewModel.block = element
    this.schoolPeriodService.deleteBlock(this.blockAddViewModel).subscribe(
      (res: BlockAddViewModel) => {
        if (typeof (res) == 'undefined') {
          this.snackbar.open('Block/Rotation Days deletion failed. ' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else {
          if (res._failure) {
            this.snackbar.open('Block/Rotation Days deletion failed. ' + res._message, '', {
              duration: 10000
            });
          }
          else {
            this.snackbar.open('' + res._message, '', {
              duration: 10000
            });
            this.currentBlockId= null;
            this.getAllBlockList();
          }
        }
      }
    )
  }
  confirmDeleteBlock(element) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: {
        title: "Are you sure?",
        message: "You are about to delete " + element.blockTitle + "."
      }
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult) {
        this.deleteBlock(element);
      }
    });
  }


  goToAddBlock() {
    this.dialog.open(EditBlockComponent, {
      width: '500px'
    }).afterClosed().subscribe((data) => {
      if (data.mode === 'submited') {
        this.currentBlockId = data.currentBlockId;
        this.getAllBlockList();
      }
    });
  }


  exportPeriodListToExcel(){
    this.blockListViewModel.getBlockListForView = [];
    this.blockListViewModel.tenantId = sessionStorage.getItem('tenantId');
    this.blockListViewModel.schoolId = +sessionStorage.getItem('selectedSchoolId');
    this.schoolPeriodService.getAllBlockList(this.blockListViewModel).subscribe(
      (res: BlockListViewModel) => {
        if (typeof (res) == 'undefined') {
          this.snackbar.open('Period list failed. ' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else {
          if (res._failure) {
            this.snackbar.open('Period list failed. ' + res._message, '', {
              duration: 10000
            });
          }
          else {
            this.blockListViewModel = res;
            if (this.currentBlockId == null) {
              this.currentBlockId = res.getBlockListForView[0].blockId;
              let periodList= this.excelPeriodList(0, res);
              if(periodList.length!=0){
                this.excelService.exportAsExcelFile(periodList,'Period_List_')
              }
              else{
                this.snackbar.open('No Records Found. Failed to Export Period List','', {
                  duration: 5000
                });
              }
              
            }
            else {
              let index = this.blockListViewModel.getBlockListForView.findIndex((x) => {
                return x.blockId === this.currentBlockId
              });
              let periodList= this.excelPeriodList(index, res);
              if(periodList.length!=0){
                this.excelService.exportAsExcelFile(periodList,'Period_List_')
              }
              else{
                this.snackbar.open('No Records Found. Failed to Export Period List','', {
                  duration: 5000
                });
              }
              
            }
          }
        }
      }
    );

  }

  getPageEvent(event) {
    // this.getAllSchool.pageNumber=event.pageIndex+1;
    // this.getAllSchool.pageSize=event.pageSize;
    // this.callAllSchool(this.getAllSchool);
  }

  toggleColumnVisibility(column, event) {
    event.stopPropagation();
    event.stopImmediatePropagation();
    column.visible = !column.visible;
  }

  onSearchClear() {
    this.searchKey = "";
    this.applyFilter();
  }
  applyFilter() {
    this.blockPeriodList.filter = this.searchKey.trim().toLowerCase();
  }

  dropPeriodList(event: CdkDragDrop<string[]>) {
    this.blockPeriodSortOrderViewModel.blockId = this.currentBlockId;
    this.blockPeriodSortOrderViewModel.currentSortOrder = this.blockPeriodList.data[event.currentIndex].sortOrder
    this.blockPeriodSortOrderViewModel.previousSortOrder = this.blockPeriodList.data[event.previousIndex].sortOrder
    this.schoolPeriodService.updateBlockPeriodSortOrder(this.blockPeriodSortOrderViewModel).subscribe(
      (res: BlockPeriodSortOrderViewModel) => {
        if (typeof (res) == 'undefined') {
          this.snackbar.open('Period Drag failed. ' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        } else {
          if (res._failure) {
            this.snackbar.open('Period Drag failed. ' + res._message, '', {
              duration: 10000
            });
          }
          else {
            this.getAllBlockList();
          }
        }
      }
    );
  }


  goToAddPeriod() {
    this.dialog.open(EditPeriodComponent, {
      data: { blockId: this.currentBlockId },
      width: '500px'
    }).afterClosed().subscribe((data) => {
      if (data === 'submited') {
        this.getAllBlockList();
      }
    });
  }

  editPeriod(element) {
    this.dialog.open(EditPeriodComponent, {
      data: { periodData: element },
      width: '500px'
    }).afterClosed().subscribe((data) => {
      if (data === 'submited') {
        this.getAllBlockList();
      }
    });
  }

  confirmDeletePeriod(element) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: {
        title: "Are you sure?",
        message: "You are about to delete " + element.periodTitle + "."
      }
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult) {
        this.deletePeriod(element);
      }
    });
  }

  deletePeriod(element) {
    this.blockPeriodAddViewModel.blockPeriod = element;
    this.blockPeriodAddViewModel.blockPeriod.schoolId = +sessionStorage.getItem('selectedSchoolId');
    this.blockPeriodAddViewModel.blockPeriod.tenantId = sessionStorage.getItem('tenantId');
    this.schoolPeriodService.deleteBlockPeriod(this.blockPeriodAddViewModel).subscribe(
      (res: BlockPeriodAddViewModel) => {
        if (typeof (res) == 'undefined') {
          this.snackbar.open('Period deletion failed. ' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else {
          if (res._failure) {
            this.snackbar.open('Period deletion failed. ' + res._message, '', {
              duration: 10000
            });
          }
          else {
            this.snackbar.open('' + res._message, '', {
              duration: 10000
            });
            this.getAllBlockList()
          }
        }
      }
    )
  }

  get visibleColumns() {
    return this.columns.filter(column => column.visible).map(column => column.property);
  }

}
