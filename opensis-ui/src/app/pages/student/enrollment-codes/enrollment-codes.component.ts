import { Component, OnInit, Input, ViewChild } from '@angular/core';
import icMoreVert from '@iconify/icons-ic/twotone-more-vert';
import icAdd from '@iconify/icons-ic/baseline-add';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icDelete from '@iconify/icons-ic/twotone-delete';
import icSearch from '@iconify/icons-ic/search';
import icFilterList from '@iconify/icons-ic/filter-list';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router} from '@angular/router';
import { fadeInUp400ms } from '../../../../@vex/animations/fade-in-up.animation';
import { stagger40ms } from '../../../../@vex/animations/stagger.animation';
import { MatSort } from '@angular/material/sort';
import { LoaderService } from '../../../services/loader.service';
import { MatTableDataSource } from '@angular/material/table';
import { EditEnrollmentCodeComponent } from '../enrollment-codes/edit-enrollment-code/edit-enrollment-code.component';
import { EnrollmentCodesService } from '../../../services/enrollment-codes.service';
import { EnrollmentCodeAddView, EnrollmentCodeListView } from '../../../models/enrollment-code.model';
import { ConfirmDialogComponent } from '../../shared-module/confirm-dialog/confirm-dialog.component';
import { TranslateService } from '@ngx-translate/core';
import { ExcelService } from '../../../services/excel.service';
import { CryptoService } from '../../../services/Crypto.service';
import { RolePermissionListViewModel, RolePermissionViewModel } from '../../../models/roll-based-access.model';
import { DefaultValuesService } from '../../../common/default-values.service';

@Component({
  selector: 'vex-enrollment-codes',
  templateUrl: './enrollment-codes.component.html',
  styleUrls: ['./enrollment-codes.component.scss'],
  animations: [
    fadeInUp400ms,
    stagger40ms
  ]
})
export class EnrollmentCodesComponent implements OnInit {
  columns = [
    { label: 'Title',       property: 'title',      type: 'text', visible: true },
    { label: 'Short Name',  property: 'shortName',  type: 'text', visible: true },
    { label: 'Sort Order',  property: 'sortOrder',  type: 'text', visible: true },
    { label: 'Type',        property: 'type',       type: 'text', visible: true },
    { label: 'Action',      property: 'action',     type: 'text', visible: true }
  ];

  icMoreVert = icMoreVert;
  icAdd = icAdd;
  icEdit = icEdit;
  icDelete = icDelete;
  icSearch = icSearch;
  icFilterList = icFilterList;
  loading: boolean;
  searchKey: string;
  enrollmentCodelistView: EnrollmentCodeListView = new EnrollmentCodeListView();
  enrollmentAddView: EnrollmentCodeAddView = new EnrollmentCodeAddView();
  enrollmentListForExcel: EnrollmentCodeListView;
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();

  constructor(private router: Router,
              private dialog: MatDialog,
              private snackbar: MatSnackBar,
              private enrollmentCodeService: EnrollmentCodesService,
              private loaderService: LoaderService,
              private translateService: TranslateService,
              private excelService: ExcelService,
              private defaultValuesService: DefaultValuesService,
              private cryptoService: CryptoService
    ) {
      translateService.use('en');
      this.loaderService.isLoading.subscribe((val) => {
        this.loading = val;
      });

  }
  enrollmentList: MatTableDataSource<any>;
  @ViewChild(MatSort) sort: MatSort;

  ngOnInit(): void {
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 12);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 23);
    const permissionSubCategory = permissionCategory.permissionSubcategory.find( x => x.permissionSubcategoryId === 25);
    this.editPermission = permissionSubCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionSubCategory.rolePermission[0].canDelete;
    this.addPermission = permissionSubCategory.rolePermission[0].canAdd;
    this.getAllStudentEnrollmentCode();
  }
  getAllStudentEnrollmentCode(){
    this.enrollmentCodeService.getAllStudentEnrollmentCode(this.enrollmentCodelistView).subscribe(
      (res: EnrollmentCodeListView) => {
        if (res){
          if (res._failure) {
            if (res.studentEnrollmentCodeList == null) {
              this.enrollmentList = new MatTableDataSource([]) ;
              this.enrollmentList.sort = this.sort;
              this.snackbar.open( res._message, '', {
                duration: 10000
              });
            }
            else {
              this.enrollmentList = new MatTableDataSource([]) ;
              this.enrollmentList.sort = this.sort;
            }
          }
          else {
            this.enrollmentList = new MatTableDataSource(res.studentEnrollmentCodeList) ;
            this.enrollmentListForExcel = res;
            this.enrollmentList.sort = this.sort;
          }
        }
        else{
          this.snackbar.open(this.defaultValuesService.translateKey('enrollmentCodeListFailed') + sessionStorage.getItem('httpError'), '', {
            duration: 10000
          });
        }
      }
    );
  }

  exportEnrollmentCodesToExcel(){
    if (this.enrollmentListForExcel.studentEnrollmentCodeList.length > 0){
      const enrollmentList = this.enrollmentListForExcel.studentEnrollmentCodeList?.map((item) => {
        return{
                  [(this.defaultValuesService.translateKey('title')).toUpperCase()]: item.title,
                  [(this.defaultValuesService.translateKey('shortName')).toUpperCase()]: item.shortName,
                  [(this.defaultValuesService.translateKey('sortOrder')).toUpperCase()]: item.sortOrder,
                  [(this.defaultValuesService.translateKey('type')).toUpperCase()]: item.type ? item.type : '-'
        };
      });
      this.excelService.exportAsExcelFile(enrollmentList, 'Enrollment_List_');
     }else{
       this.snackbar.open(this.defaultValuesService.translateKey('noRecordsFoundFailedtoExportEnrollmentList'), '', {
         duration: 5000
       });
     }
  }

  goToAdd(){
    this.dialog.open(EditEnrollmentCodeComponent, {
      width: '600px'
    }).afterClosed().subscribe(
      result => {
        if (result === 'submited'){
          this.getAllStudentEnrollmentCode();
        }
      }
    );
  }

  getPageEvent(event){
  }

  toggleColumnVisibility(column, event) {
    event.stopPropagation();
    event.stopImmediatePropagation();
    column.visible = !column.visible;
  }

  get visibleColumns() {
    return this.columns.filter(column => column.visible).map(column => column.property);
  }
  openEditdata(element){
    this.dialog.open(EditEnrollmentCodeComponent, {
      data: element,
        width: '600px'
    }).afterClosed().subscribe(
      result => {
        if (result === 'submited'){
          this.getAllStudentEnrollmentCode();
        }
      }
    );
  }
  deleteEnrollmentCode(element){
    this.enrollmentAddView.studentEnrollmentCode = element;
    this.enrollmentCodeService.deleteStudentEnrollmentCode(this.enrollmentAddView).subscribe(
      (res: EnrollmentCodeAddView) => {
        if (res){
          if (res._failure) {
            this.snackbar.open(res._message, '', {
              duration: 10000
            });
          }
          else {
            this.snackbar.open(res._message, '', {
              duration: 10000
            });
            this.getAllStudentEnrollmentCode();
          }
        }
        else{
          this.snackbar.open(this.defaultValuesService.translateKey('enrollmentCodeDeleteFailed')
          + sessionStorage.getItem('httpError'), '', {
            duration: 10000
          });
        }
      }
    );
  }
  confirmDelete(element){
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: '400px',
      data: {
          title: this.defaultValuesService.translateKey('areYouSure'),
          message: this.defaultValuesService.translateKey('youAreAboutToDelete') + element.title + '.'}
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult){
        this.deleteEnrollmentCode(element);
      }
   });
}

onSearchClear(){
  this.searchKey = '';
  this.applyFilter();
}
applyFilter(){
  this.enrollmentList.filter = this.searchKey.trim().toLowerCase();
}

}
