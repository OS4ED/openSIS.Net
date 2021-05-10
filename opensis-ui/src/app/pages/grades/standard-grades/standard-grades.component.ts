import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icDelete from '@iconify/icons-ic/twotone-delete';
import icAdd from '@iconify/icons-ic/baseline-add';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import icSearch from '@iconify/icons-ic/search';
import icFilterList from '@iconify/icons-ic/filter-list';
import { MatDialog } from '@angular/material/dialog';
import { AddGradeComponent } from './add-grade/add-grade.component';
import { ValidationService } from '../../shared/validation.service';
import { GradeScaleAddViewModel, GradeScaleListView, GradeDragDropModel, GradeAddViewModel } from 'src/app/models/grades.model';
import { GradesService } from 'src/app/services/grades.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ConfirmDialogComponent } from '../../shared-module/confirm-dialog/confirm-dialog.component';
import { MatTableDataSource } from '@angular/material/table';
import { LoaderService } from 'src/app/services/loader.service';
import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { fadeInUp400ms } from '../../../../@vex/animations/fade-in-up.animation';
import { stagger40ms } from '../../../../@vex/animations/stagger.animation';
import { MatSort } from '@angular/material/sort';
import { RolePermissionListViewModel, RolePermissionViewModel } from '../../../models/roll-based-access.model';
import { CryptoService } from '../../../services/Crypto.service';
import { ExcelService } from '../../../services/excel.service';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';

@Component({
  selector: 'vex-standard-grades',
  templateUrl: './standard-grades.component.html',
  styleUrls: ['./standard-grades.component.scss'],
  animations: [
    fadeInUp400ms,
    stagger40ms
  ]
})
export class StandardGradesComponent implements OnInit {

  @Input()
  columns = [
    { label: 'Order', property: 'id', type: 'number', visible: true },
    { label: 'Title', property: 'title', type: 'text', visible: true },
    { label: 'Description', property: 'comment', type: 'text', visible: true },
    { label: 'action', property: 'action', type: 'text', visible: true }
  ];

  icEdit = icEdit;
  icDelete = icDelete;
  icAdd = icAdd;
  effortCategoryTitle: string;
  addCategory = false;
  form: FormGroup;
  buttonType: string;
  icSearch = icSearch;
  icFilterList = icFilterList;
  gradeScaleAddViewModel: GradeScaleAddViewModel = new GradeScaleAddViewModel();
  gradeScaleListView: GradeScaleListView = new GradeScaleListView();
  gradeScaleList = [];
  currentGradeScaleId = null;
  gradeListValue = [];
  gradeList: MatTableDataSource<any>;
  @ViewChild(MatSort) sort: MatSort;
  totalCount;
  loading: Boolean;
  gradeDragDropModel: GradeDragDropModel = new GradeDragDropModel();
  gradeAddViewModel: GradeAddViewModel = new GradeAddViewModel();
  gradeScaleListForExcel = [];
  destroySubject$: Subject<void> = new Subject();
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();
  searchKey: string;
  editMode:boolean;

  constructor(public translateService: TranslateService,
    private dialog: MatDialog,
    private fb: FormBuilder,
    private gradesService: GradesService,
    private snackbar: MatSnackBar,
    private loaderService: LoaderService,
    private cryptoService: CryptoService,
    private excelService: ExcelService,
  ) {
    translateService.use('en');
    this.formInit();
    this.loaderService.isLoading.pipe(takeUntil(this.destroySubject$)).subscribe((val) => {
      this.loading = val;
    });
    
  }

  ngOnInit(): void {
    this.getAllGradeScale();
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 12);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 26);
    const permissionSubCategory = permissionCategory.permissionSubcategory.find(x => x.permissionSubcategoryId === 28);
    this.editPermission = permissionSubCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionSubCategory.rolePermission[0].canDelete;
    this.addPermission = permissionSubCategory.rolePermission[0].canAdd;
  }

  formInit() {
    this.form = this.fb.group({
      gradeScaleId: [0],
      gradeScaleName: ['', [ValidationService.noWhitespaceValidator]],
    })
  }

  applyFilter() {
    this.gradeList.filter = this.searchKey.trim().toLowerCase();
  }

  onSearchClear() {
    this.searchKey = '';
    this.applyFilter();
  }

  selectGradeScale(element) {
    this.currentGradeScaleId = element.gradeScaleId;
    this.gradeList = new MatTableDataSource(element.grade);
    this.totalCount = element.grade.length;
  }

  submit() {
    this.form.markAllAsTouched();
    if( this.form.invalid ){ return; }

    if(this.editMode){
      this.updateGradeScale();
    }else{
      this.addGradeScale();
    }

  }

  addGradeScale(){
    this.gradeScaleAddViewModel.gradeScale.gradeScaleId = this.form.controls.gradeScaleId.value;
        this.gradeScaleAddViewModel.gradeScale.gradeScaleName = this.form.controls.gradeScaleName.value;
        this.gradeScaleAddViewModel.gradeScale.useAsStandardGradeScale = true;
        this.gradesService.addGradeScale(this.gradeScaleAddViewModel).subscribe(
          (res: GradeScaleAddViewModel) => {
            if (typeof (res) == 'undefined') {
              this.snackbar.open('' + sessionStorage.getItem("httpError"), '', {
                duration: 10000
              });
            }
            else {
              if (res._failure) {
                this.snackbar.open('' + res._message, '', {
                  duration: 10000
                });
              }
              else {
                this.snackbar.open('' + res._message, '', {
                  duration: 10000
                });
                this.getAllGradeScale();
                this.closeAddCategory();
              }
            }
          }
        );
  }

  updateGradeScale(){
    this.gradeScaleAddViewModel.gradeScale.gradeScaleId = this.form.controls.gradeScaleId.value;
    this.gradeScaleAddViewModel.gradeScale.gradeScaleName = this.form.controls.gradeScaleName.value;
    this.gradeScaleAddViewModel.gradeScale.useAsStandardGradeScale = true;

    this.gradesService.updateGradeScale(this.gradeScaleAddViewModel).subscribe(
      (res: GradeScaleAddViewModel) => {
        if (typeof (res) == 'undefined') {
          this.snackbar.open('' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else {
          if (res._failure) {
            this.snackbar.open('' + res._message, '', {
              duration: 10000
            });
          }
          else {
            this.snackbar.open('' + res._message, '', {
              duration: 10000
            });
            this.getAllGradeScale();
            this.closeAddCategory();
          }
        }
      }
    );
  }
  getAllGradeScale() {
    this.gradesService.getAllGradeScaleList(this.gradeScaleListView).subscribe(
      (res: GradeScaleListView) => {
        if (typeof (res) === 'undefined') {
          this.snackbar.open('' + sessionStorage.getItem('httpError'), '', {
            duration: 10000
          });
        }
        else {
          if (res._failure) {
            if (res.gradeScaleList == null) {
              this.gradeScaleList = [];
              this.snackbar.open(res._message, '', {
                duration: 10000
              });
            } else {
              this.gradeScaleList = []
            }

          }
          else {
            this.gradeScaleList = res.gradeScaleList.filter(x => x.useAsStandardGradeScale === true);
            let index = 0;
            if (this.gradeScaleList.length > 0) {
              if (this.currentGradeScaleId == null) {
                this.currentGradeScaleId = this.gradeScaleList[index]?.gradeScaleId;
              }
              else {
                index = this.gradeScaleList.findIndex((x) => {
                  return x.gradeScaleId === this.currentGradeScaleId;
                });
              }
              this.gradeListValue = this.gradeScaleList[index].grade.map((item) => {
                return ({
                  gradeScaleId: item.gradeScaleId,
                  gradeId: item.gradeId,
                  title: item.title,
                  comment: item.comment
                })
              })
            }
            this.totalCount = this.gradeListValue.length
            this.gradeList = new MatTableDataSource(this.gradeListValue);
            this.gradeList.filterPredicate = this.createFilter();
            this.gradeScaleListForExcel = this.gradeScaleList[index]?.grade;
          }
        }
      }
    );
  }

  createFilter(): (data: any, filter: string) => boolean {
    let filterFunction = (data, filter): boolean => {
      return (
        data.title.toLowerCase().includes(filter) || data.comment.toLowerCase().includes(filter)
      );
    };
    return filterFunction;
  }

  drop(event: CdkDragDrop<string[]>) {
    this.gradeDragDropModel.gradeScaleId = this.currentGradeScaleId;
    this.gradeDragDropModel.currentSortOrder = this.gradeScaleListForExcel[event.currentIndex].sortOrder;
    this.gradeDragDropModel.previousSortOrder = this.gradeScaleListForExcel[event.previousIndex].sortOrder;

    this.gradesService.updateGradeSortOrder(this.gradeDragDropModel).subscribe(
      (res: GradeDragDropModel) => {
        if (typeof (res) === 'undefined') {
          this.snackbar.open('' + sessionStorage.getItem('httpError'), '', {
            duration: 10000
          });
        } else {
          if (res._failure) {
            this.snackbar.open('' + res._message, '', {
              duration: 10000
            });
          }
          else {
            this.getAllGradeScale()
          }
        }
      }
    );
  }

  get visibleColumns() {
    return this.columns.filter(column => column.visible).map(column => column.property);
  }

  editGradeScale(element) {
    this.effortCategoryTitle = "updateGradeScale";
    this.form.patchValue(element)
    this.addCategory = true;
    this.buttonType = "update";
    this.editMode=true;
  }

  goToAddCategory() {
    this.editMode=false;
    this.effortCategoryTitle = "addNewGradeScale";
    this.addCategory = true;
    this.buttonType = "submit";
  }

  deleteGradeScale(element) {
    this.gradeScaleAddViewModel.gradeScale.gradeScaleId = element.gradeScaleId
    this.gradesService.deleteGradeScale(this.gradeScaleAddViewModel).subscribe(
      (res: GradeScaleAddViewModel) => {
        if (typeof (res) === 'undefined') {
          this.snackbar.open('' + sessionStorage.getItem('httpError'), '', {
            duration: 10000
          });
        }
        else {
          if (res._failure) {
            this.snackbar.open('' + res._message, '', {
              duration: 10000
            });
          }
          else {
            this.snackbar.open('' + res._message, '', {
              duration: 10000
            });
            if (element.gradeScaleId === this.currentGradeScaleId) {
              this.currentGradeScaleId = null;
            }
            this.getAllGradeScale();
          }
        }
      }
    );
  }

  confirmDeleteGradeScale(element) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: '400px',
      data: {
        title: 'Are you sure?',
        message: 'You are about to delete ' + element.gradeScaleName + '.'
      }
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult) {
        this.deleteGradeScale(element);
      }
    });
  }

  deleteGrade(element) {
    this.gradeAddViewModel.grade.gradeId = element.gradeId
    this.gradesService.deleteGrade(this.gradeAddViewModel).subscribe(
      (res: GradeAddViewModel) => {
        if (typeof (res) === 'undefined') {
          this.snackbar.open('' + sessionStorage.getItem('httpError'), '', {
            duration: 10000
          });
        }
        else {
          if (res._failure) {
            this.snackbar.open('' + res._message, '', {
              duration: 10000
            });
          }
          else {
            this.snackbar.open('' + res._message, '', {
              duration: 10000
            });
            this.getAllGradeScale();
          }
        }
      }
    );
  }

  confirmDeleteGrade(element) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: '400px',
      data: {
        title: 'Are you sure?',
        message: 'You are about to delete report card grade ' + element.title + '.'
      }
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult) {
        this.deleteGrade(element);
      }
    });
  }

  closeAddCategory() {
    this.addCategory = false;
    this.formInit();
  }

  addGrade() {
    this.dialog.open(AddGradeComponent, {
      data: { gradeScaleId: this.currentGradeScaleId },
      width: '500px'
    }).afterClosed().subscribe((data) => {
      if (data) {
        this.getAllGradeScale();
      }
    });
  }

  editGrade(element) {
    this.dialog.open(AddGradeComponent, {
      data: { information: element },
      width: '500px'
    }).afterClosed().subscribe((data) => {
      if (data) {
        this.getAllGradeScale();
      }
    })
  }

  toggleColumnVisibility(column, event) {
    event.stopPropagation();
    event.stopImmediatePropagation();
    column.visible = !column.visible;
  }

  translateKey(key) {
    let trnaslateKey;
    this.translateService.get(key).subscribe((res: string) => {
       trnaslateKey = res;
    });
    return trnaslateKey;
  }

  exportToExcel() {
    this.gradeScaleList.map((data) => {
      if (data.gradeScaleId === this.currentGradeScaleId) {
        const standardGradeList = data.grade;
        if (standardGradeList.length > 0) {
          const reportList = standardGradeList.map((x) => {
            return {
              [this.translateKey('title')]: x.title,
              [this.translateKey('description')]: x.comment
            };
          });
          this.excelService.exportAsExcelFile(reportList, 'Standard_Grade_List_');
        } else {
          this.snackbar.open('No records found. failed to export standard grade list', '', {
            duration: 5000
          });
        }
      }
    });
  }

  ngOnDestroy() {
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }

}