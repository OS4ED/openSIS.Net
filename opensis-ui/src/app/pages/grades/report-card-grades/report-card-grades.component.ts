import { Component, OnInit, Input, ViewChild } from '@angular/core';
import icAdd from '@iconify/icons-ic/baseline-add';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icDelete from '@iconify/icons-ic/twotone-delete';
import icSearch from '@iconify/icons-ic/search';
import icFilterList from '@iconify/icons-ic/filter-list';
import { Router} from '@angular/router';
import { fadeInUp400ms } from '../../../../@vex/animations/fade-in-up.animation';
import { stagger40ms } from '../../../../@vex/animations/stagger.animation';
import { TranslateService } from '@ngx-translate/core';
import { MatDialog } from '@angular/material/dialog';
import { EditGradeScaleComponent } from './edit-grade-scale/edit-grade-scale.component';
import { EditReportCardGradeComponent } from './edit-report-card-grade/edit-report-card-grade.component';
import { GradeScaleListView, GradeScaleAddViewModel,GradeAddViewModel, GradeDragDropModel } from '../../../models/grades.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { ConfirmDialogComponent } from '../../shared-module/confirm-dialog/confirm-dialog.component';
import { MatSort } from '@angular/material/sort';
import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { GradesService } from '../../../services/grades.service';
import { ExcelService } from '../../../services/excel.service';
import { LoaderService } from '../../../services/loader.service';
import { CryptoService } from '../../../services/Crypto.service';
import { RolePermissionListViewModel, RolePermissionViewModel } from '../../../models/rollBasedAccessModel';

@Component({
  selector: 'vex-report-card-grades',
  templateUrl: './report-card-grades.component.html',
  styleUrls: ['./report-card-grades.component.scss'],
  animations: [
    fadeInUp400ms,
    stagger40ms
  ]
})
export class ReportCardGradesComponent implements OnInit {

  @Input()
  columns = [
    { label: 'ID', property: 'id', type: 'number', visible: true },
    { label: 'Title', property: 'title', type: 'text', visible: true },
    { label: 'Breakoff', property: 'breakoff', type: 'text', visible: true },
    { label: 'Weighted GP Value', property: 'weightedGpValue', type: 'text', visible: true },
    { label: 'unweighted GP Value', property: 'unweightedGpValue', type: 'text', visible: true },
    { label: 'Comment', property: 'comment', type: 'text', visible: false },
    { label: 'action', property: 'action', type: 'text', visible: true }
  ];

  ReportCardGradesModelList;

  icAdd = icAdd;
  icEdit = icEdit;
  icDelete = icDelete;
  icSearch = icSearch;
  icFilterList = icFilterList;
  loading:Boolean;
  @ViewChild(MatSort) sort: MatSort;
  gradeScaleList=[];
  searchKey:string;
  currentGradeScaleId =null;
  gradeScaleAddViewModel:GradeScaleAddViewModel =new GradeScaleAddViewModel();
  gradeAddViewModel:GradeAddViewModel =new GradeAddViewModel();
  gradeDragDropModel:GradeDragDropModel =new GradeDragDropModel()
  gradeScaleListView:GradeScaleListView =new GradeScaleListView();
  gradeList: MatTableDataSource<any>;
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel()
  gradeScaleListForExcel =[];
  gradeListValue;
  totalCount;

  constructor(
    private router: Router,
    private dialog: MatDialog,
    public translateService:TranslateService,
    private snackbar: MatSnackBar,
    private gradesService:GradesService,
    private loaderService:LoaderService,
    private excelService:ExcelService,
    private cryptoService: CryptoService
    ) {
    translateService.use('en');
    this.loaderService.isLoading.subscribe((val) => {
      this.loading = val;
    }); 
  }

  ngOnInit(): void {
    this.getAllGradeScale(0);
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 12);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 26);
    const permissionSubCategory = permissionCategory.permissionSubcategory.find( x => x.permissionSubcategoryId === 30);
    this.editPermission = permissionSubCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionSubCategory.rolePermission[0].canDelete;
    this.addPermission = permissionSubCategory.rolePermission[0].canAdd;

    

  }

  getPageEvent(event){    
    // this.getAllSchool.pageNumber=event.pageIndex+1;
    // this.getAllSchool.pageSize=event.pageSize;
    // this.callAllSchool(this.getAllSchool);
  }
  applyFilter(){
    this.gradeList.filter = this.searchKey.trim().toLowerCase()
  }
  onSearchClear(){
    this.searchKey = '';
    this.applyFilter();
  }

  goToAddGrade() {
    this.dialog.open(EditReportCardGradeComponent, {
      data: {gradeScaleId:this.currentGradeScaleId},
      width: '500px'
    }).afterClosed().subscribe((data) => {
      if(data === 'submited'){
        this.getAllGradeScale(this.currentGradeScaleId);
      }
    });
  }
  editGrade(element){
    this.dialog.open(EditReportCardGradeComponent,{ 
      data: {information:element},
      width: '500px'
    }).afterClosed().subscribe((data) => {
      if(data === 'submited'){
        this.getAllGradeScale(this.currentGradeScaleId);
      }
    })
  }

  toggleColumnVisibility(column, event) {
    event.stopPropagation();
    event.stopImmediatePropagation();
    column.visible = !column.visible;
  }

  goToAddBlock() {
    this.dialog.open(EditGradeScaleComponent, {
      width: '500px'
    }).afterClosed().subscribe((data) => {
      if(data === 'submited'){
        this.getAllGradeScale(this.currentGradeScaleId);
      }
    });
  }
  editGradeScale(element){
    this.dialog.open(EditGradeScaleComponent,{ 
      data: element,
      width: '500px'
    }).afterClosed().subscribe((data) => {
      if(data === 'submited'){
        this.getAllGradeScale(element.gradeScaleId);
      }
    })
  }

  get visibleColumns() {
    return this.columns.filter(column => column.visible).map(column => column.property);
  }
  selectGradeScale(element){
    this.currentGradeScaleId = element.gradeScaleId;
    this.gradeList = new MatTableDataSource(element.grade) ;
  }
  deleteGradeScale(element){
    this.gradeScaleAddViewModel.gradeScale.gradeScaleId=element.gradeScaleId
    this.gradesService.deleteGradeScale(this.gradeScaleAddViewModel).subscribe(
     (res: GradeScaleAddViewModel) => {
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
        else{
          this.snackbar.open('' + res._message, '', {
            duration: 10000
          });
          if (element.gradeScaleId === this.currentGradeScaleId){
            this.currentGradeScaleId = null;
          }
          this.getAllGradeScale(element.gradeScaleId);
        }
      }
     }
   );
  }
  confirmDeleteGradeScale(element){
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: '400px',
      data: {
          title: 'Are you sure?',
          message: 'You are about to delete ' + element.gradeScaleName + '.'}
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      if(dialogResult){
        this.deleteGradeScale(element);
      }
   });
  }

  deleteGrade(element){
    this.gradeAddViewModel.grade.gradeId=element.gradeId
    this.gradesService.deleteGrade(this.gradeAddViewModel).subscribe(
     (res: GradeAddViewModel) => {
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
        else{
          this.snackbar.open('' + res._message, '', {
            duration: 10000
          });
          this.getAllGradeScale(element.gradeScaleId);
        }
      }
     }
   );
  }
  confirmDeleteGrade(element){
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: '400px',
      data: {
          title: 'Are you sure?',
          message: 'You are about to delete report card grade ' + element.title + '.'}
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult){
        this.deleteGrade(element);
      }
   });
  }

  getAllGradeScale( selectedGradeScaleId : number){
    this.gradesService.getAllGradeScaleList(this.gradeScaleListView).subscribe(
      (res: GradeScaleListView) => {
        if (typeof(res) === 'undefined'){
          this.snackbar.open('' + sessionStorage.getItem('httpError'), '', {
            duration: 10000
          });
        }
        else{
          if (res._failure) {
            if (res.gradeScaleList == null) {
              this.gradeScaleList = [];
              this.snackbar.open( res._message, '', {
                duration: 10000
              });
            } else {
              this.gradeScaleList = []
            }
            
          }
          else{
            this.gradeScaleList = res.gradeScaleList;
            let index =0;
            if (this.currentGradeScaleId == null){
              this.currentGradeScaleId = res.gradeScaleList[index].gradeScaleId;
            } 
            else{
              index = this.gradeScaleList.findIndex((x) => {
                return x.gradeScaleId === this.currentGradeScaleId;
              });
            } 
            this.gradeListValue=res.gradeScaleList[index].grade.map((item)=>{
              return ({
                gradeScaleId:item.gradeScaleId,
                gradeId:item.gradeId,      
                title: item.title,
                breakoff: item.breakoff,
                weightedGpValue: item.weightedGpValue,
                unweightedGpValue: item.unweightedGpValue,
                comment:item.comment
              })
            })
            this.totalCount=this.gradeListValue.length
            this.gradeList = new MatTableDataSource(this.gradeListValue) ;
            this.gradeScaleListForExcel = res.gradeScaleList[index].grade;
          }
        }
      }
    );
  }
  drop(event: CdkDragDrop<string[]>) {
    this.gradeDragDropModel.gradeScaleId = this.currentGradeScaleId;
    this.gradeDragDropModel.currentSortOrder = this.gradeList.data[event.currentIndex].sortOrder;
    this.gradeDragDropModel.previousSortOrder = this.gradeList.data[event.previousIndex].sortOrder;

    this.gradesService.updateGradeSortOrder(this.gradeDragDropModel).subscribe(
      (res: GradeDragDropModel) => {
        if (typeof(res) === 'undefined'){
          this.snackbar.open('' + sessionStorage.getItem('httpError'), '', {
            duration: 10000
          });
        }else{
          if (res._failure) {
            this.snackbar.open('' + res._message, '', {
              duration: 10000
            });
          } 
          else{
            this.getAllGradeScale(this.currentGradeScaleId )
          }
        }
      }
    );
  }
  excelReportCardList(index= 0, response){
    const reportCardGradeList = response.getBlockListForView[index].blockPeriod?.map( (item) => {

      return {
        Title: item.tite,
        Breakoff: item.breakoff,
        'Weighted Gp Value': item.weightedGpValue,
        'Unweighted Gp Value': item.unweightedGpValue,
        Comment:item.comment
      };
    });
    return reportCardGradeList;
  }

  exportToExcel(){
    this.gradeScaleList.map((data) => {
      if (data.gradeScaleId === this.currentGradeScaleId){
        const reportCardGradeList = data.grade;
        if (reportCardGradeList.length > 0) {
          const reportList = reportCardGradeList.map((x) => {
            return {
              Title: x.title,
              Breakoff: x.breakoff,
              'Weighted Gp Value': x.weightedGpValue,
              'Unweighted Gp Value': x.unweightedGpValue,
              comment:x.comment
            };
          });
          this.excelService.exportAsExcelFile(reportList, 'Report_Card_Grade_List_');
        } else {
          this.snackbar.open('No records found. failed to export report card grade list', '', {
            duration: 5000
          });
        }
      }
    });
  }
}
