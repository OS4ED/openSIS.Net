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
import { TranslateService } from '@ngx-translate/core';
import { EditStudentFieldsComponent } from './edit-student-fields/edit-student-fields.component';
import { StudentFieldsCategoryComponent } from './student-fields-category/student-fields-category.component';
import { CustomFieldService } from '../../../services/custom-field.service';
import {CustomFieldAddView, CustomFieldDragDropModel, CustomFieldListViewModel} from '../../../models/custom-field.model';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { ConfirmDialogComponent } from '../../shared-module/confirm-dialog/confirm-dialog.component';
import { LoaderService } from '../../../services/loader.service';
import { FieldsCategoryListView, FieldsCategoryAddView } from '../../../models/fields-category.model';
import {FieldCategoryModuleEnum} from '../../../enums/field-category-module.enum';
import { CdkDragDrop} from '@angular/cdk/drag-drop';
import { CryptoService } from '../../../services/Crypto.service';
import { RolePermissionListViewModel, RolePermissionViewModel } from '../../../models/roll-based-access.model';
import { DefaultValuesService } from '../../../common/default-values.service';


@Component({
  selector: 'vex-student-fields',
  templateUrl: './student-fields.component.html',
  styleUrls: ['./student-fields.component.scss'],
  animations: [
    fadeInUp400ms,
    stagger40ms
  ]
})
export class StudentFieldsComponent implements OnInit {
  @Input()
  columns = [
    /* { label: '', property: 'type', type: 'text', visible: true }, */
    { label: 'Id', property: 'fieldId', type: 'text', visible: true },
    { label: 'Field Name', property: 'title', type: 'text', visible: true },
    { label: 'Field Type', property: 'type', type: 'text', visible: true },
    { label: 'In Used', property: 'hide', type: 'checkbox', visible: true },
    { label: 'Action', property: 'action', type: 'text', visible: true }
   ];

  StudentFieldsModelList;
  fieldsCategoryList;
  currentCategoryId = null;
  fieldCategoryModuleEnum = FieldCategoryModuleEnum;
  restrictedCategoryid = [5, 6, 8, 9, 10]; // All the catagory where Custom field cannot insert
  icMoreVert = icMoreVert;
  icAdd = icAdd;
  icEdit = icEdit;
  icDelete = icDelete;
  icSearch = icSearch;
  icFilterList = icFilterList;
  loading: boolean;
  searchKey: string;
  customFieldListViewModel: CustomFieldListViewModel = new CustomFieldListViewModel();
  customFieldAddView: CustomFieldAddView = new CustomFieldAddView();
  fieldsCategoryListView: FieldsCategoryListView = new FieldsCategoryListView();
  fieldsCategoryAddView: FieldsCategoryAddView = new FieldsCategoryAddView();
  customFieldDragDropModel: CustomFieldDragDropModel = new CustomFieldDragDropModel();
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();

  constructor(
    private router: Router,
    private snackbar: MatSnackBar,
    private dialog: MatDialog,
    public translateService: TranslateService,
    private customFieldservice: CustomFieldService,
    private loaderService: LoaderService,
    private defaultValuesService: DefaultValuesService,
    private cryptoService: CryptoService
    ) {
    translateService.use('en');
    this.loaderService.isLoading.subscribe((val) => {
      this.loading = val;
    });

  }

  customFieldList: MatTableDataSource<any>;
  @ViewChild(MatSort) sort: MatSort;
  onSearchClear(){
    this.searchKey = '';
    this.applyFilter();
  }
  applyFilter(){
    this.customFieldList.filter = this.searchKey.trim().toLowerCase();
  }

  ngOnInit(): void {
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 12);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 23);
    const permissionSubCategory = permissionCategory.permissionSubcategory.find( x => x.permissionSubcategoryId === 24);
    this.editPermission = permissionSubCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionSubCategory.rolePermission[0].canDelete;
    this.addPermission = permissionSubCategory.rolePermission[0].canAdd;
    this.getAllCustomFieldCategory();
  }
  selectCategory(element){
    this.currentCategoryId = element.categoryId;
    this.customFieldList = new MatTableDataSource(element.customFields) ;
    this.customFieldList.sort = this.sort;
  }

   goToAdd(){
    this.dialog.open(EditStudentFieldsComponent, {
      data: {categoryID: this.currentCategoryId},
      width: '600px'
    }).afterClosed().subscribe((data) => {
      if (data === 'submited'){
        this.getAllCustomFieldCategory();
      }
    });
   }
   goToAddCategory(){
    this.dialog.open(StudentFieldsCategoryComponent, {
      width: '500px'
    }).afterClosed().subscribe((data) => {
      if (data === 'submited'){
        this.getAllCustomFieldCategory();
      }
    });
   }


  getPageEvent(event){
    // this.getAllSchool.pageNumber=event.pageIndex+1;
    // this.getAllSchool.pageSize=event.pageSize;
    // this.callAllSchool(this.getAllSchool);
  }

  toggleColumnVisibility(column, event) {
    event.stopPropagation();
    event.stopImmediatePropagation();
    column.visible = !column.visible;
  }

  get visibleColumns() {
    return this.columns.filter(column => column.visible).map(column => column.property);
  }

  deleteCustomFieldata(element){
    this.customFieldAddView.customFields = element;
    this.customFieldservice.deleteCustomField(this.customFieldAddView).subscribe(
      (res: CustomFieldAddView) => {
        if (res){
          if (res._failure) {
            this.snackbar.open(res._message, '', {
              duration: 10000
            });
          }
          else {
            this.getAllCustomFieldCategory();
          }
        }
        else{
          this.snackbar.open(this.defaultValuesService.translateKey('customFieldFailed') + sessionStorage.getItem('httpError'), '', {
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
        this.deleteCustomFieldata(element);
      }
   });
  }
  openEditdata(element){
    this.dialog.open(EditStudentFieldsComponent, {
      data: {information: element},
        width: '800px'
    }).afterClosed().subscribe((data) => {
      if (data === 'submited'){
        this.getAllCustomFieldCategory();
      }
    });
  }
  getAllCustomFieldCategory(){
    this.fieldsCategoryListView.module = this.fieldCategoryModuleEnum.Student;
    this.customFieldservice.getAllFieldsCategory(this.fieldsCategoryListView).subscribe(
      (res: FieldsCategoryListView) => {
        if (res){
          if (res._failure) {
            if (res.fieldsCategoryList == null) {
              this.fieldsCategoryList = [];
              this.snackbar.open( res._message, '', {
                duration: 10000
              });
            } else {
              this.fieldsCategoryList = [];
            }
          }
          else{
            this.fieldsCategoryList = res.fieldsCategoryList;
            if (this.currentCategoryId == null){
              this.currentCategoryId = res.fieldsCategoryList[0].categoryId;
              this.customFieldList = new MatTableDataSource(res.fieldsCategoryList[0].customFields) ;
              this.customFieldList.sort = this.sort;
            }
            else{
              const index = this.fieldsCategoryList.findIndex((x) => {
                return x.categoryId === this.currentCategoryId;
              });
              this.customFieldList = new MatTableDataSource(res.fieldsCategoryList[index].customFields) ;
              this.customFieldList.sort = this.sort;
            }
          }
        }
        else{
          this.snackbar.open(this.defaultValuesService.translateKey('fieldCategoryListFailed') + sessionStorage.getItem('httpError'), '', {
            duration: 10000
          });
        }
      }
    );
  }
  editFieldCategory(element){
    this.dialog.open(StudentFieldsCategoryComponent, {
      data: element,
      width: '800px'
    }).afterClosed().subscribe((data) => {
      if (data === 'submited'){
        this.getAllCustomFieldCategory();
      }
    });
  }
  deleteFieldCategory(element){
    this.fieldsCategoryAddView.fieldsCategory = element;
    this.customFieldservice.deleteFieldsCategory(this.fieldsCategoryAddView).subscribe(
      (res: FieldsCategoryAddView) => {
        if (res){
          if (res._failure) {
            this.snackbar.open( res._message, '', {
              duration: 10000
            });
          }
          else{
            this.snackbar.open( res._message, '', {
              duration: 10000
            });
            if (element.categoryId === this.currentCategoryId){
              this.currentCategoryId = null;
            }
            this.getAllCustomFieldCategory();
          }
        }
        else{
          this.snackbar.open(this.defaultValuesService.translateKey('fieldCategoryDeleteFailed') 
          + sessionStorage.getItem('httpError'), '', {
            duration: 10000
          });
        }
      }
    );
  }
  confirmDeleteFieldCategory(element){
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: '400px',
      data: {
          title: this.defaultValuesService.translateKey('areYouSure'),
          message: this.defaultValuesService.translateKey('youAreAboutToDelete') + element.title + '.'}
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult){
        this.deleteFieldCategory(element);
      }
   });
  }

  drop(event: CdkDragDrop<string[]>) {
    this.customFieldDragDropModel.categoryId = this.currentCategoryId;
    this.customFieldDragDropModel.currentSortOrder = this.customFieldList.data[event.currentIndex].sortOrder;
    this.customFieldDragDropModel.previousSortOrder = this.customFieldList.data[event.previousIndex].sortOrder;
    this.customFieldservice.updateCustomFieldSortOrder(this.customFieldDragDropModel).subscribe(
      (res: CustomFieldDragDropModel) => {
        if (res){
          if (res._failure) {
            this.snackbar.open( res._message, '', {
              duration: 10000
            });
          }
          else{
            this.getAllCustomFieldCategory();
          }
        }
        else{
          this.snackbar.open(this.defaultValuesService.translateKey('customFieldDragShortFailed')
          + sessionStorage.getItem('httpError'), '', {
            duration: 10000
          });
        }
      }
    );
  }

}
