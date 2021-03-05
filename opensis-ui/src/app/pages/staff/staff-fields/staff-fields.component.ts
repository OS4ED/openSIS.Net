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
import { EditStaffFieldsComponent } from './edit-staff-fields/edit-staff-fields.component';
import { StaffFieldsCategoryComponent } from './staff-fields-category/staff-fields-category.component';
import { CustomFieldService } from '../../../services/custom-field.service';
import {CustomFieldAddView,CustomFieldDragDropModel, CustomFieldListViewModel} from '../../../models/customFieldModel';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { ConfirmDialogComponent } from '../../shared-module/confirm-dialog/confirm-dialog.component';
import { LoaderService } from '../../../services/loader.service';
import { FieldsCategoryListView,FieldsCategoryAddView } from '../../../models/fieldsCategoryModel';
import {FieldCategoryModuleEnum} from '../../../enums/field-category-module.enum';
import { CdkDragDrop, moveItemInArray, transferArrayItem} from '@angular/cdk/drag-drop';

@Component({
  selector: 'vex-staff-fields',
  templateUrl: './staff-fields.component.html',
  styleUrls: ['./staff-fields.component.scss'],
  animations: [
    fadeInUp400ms,
    stagger40ms
  ]
})
export class StaffFieldsComponent implements OnInit {
  @Input()
  columns = [
   /* { label: '', property: 'type', type: 'text', visible: true }, */
   { label: 'Id', property: 'fieldId', type: 'text', visible: true },
   { label: 'Field Name', property: 'title', type: 'text', visible: true },
   { label: 'Field Type', property: 'type', type: 'text', visible: true },
   { label: 'In Used', property: 'hide', type: 'checkbox', visible: true },
   { label: 'Action', property: 'action', type: 'text', visible: true }
  ];

  fieldsCategoryList;
  currentCategoryId=null;
  fieldCategoryModuleEnum=FieldCategoryModuleEnum
  icMoreVert = icMoreVert;
  icAdd = icAdd;
  icEdit = icEdit;
  icDelete = icDelete;
  icSearch = icSearch;
  icFilterList = icFilterList;
  loading:Boolean;
  searchKey:string;
  customFieldListViewModel:CustomFieldListViewModel= new CustomFieldListViewModel();
  customFieldAddView:CustomFieldAddView= new CustomFieldAddView()
  fieldsCategoryListView:FieldsCategoryListView= new FieldsCategoryListView();
  fieldsCategoryAddView:FieldsCategoryAddView= new FieldsCategoryAddView();
  customFieldDragDropModel:CustomFieldDragDropModel= new CustomFieldDragDropModel();

  constructor(
    private router: Router,
    private dialog: MatDialog,
    public translateService:TranslateService,
    private snackbar: MatSnackBar,
    private customFieldservice:CustomFieldService,
    private loaderService:LoaderService
    ) {
    translateService.use('en');
    this.loaderService.isLoading.subscribe((val) => {
      this.loading = val;
    });
  }
  customFieldList: MatTableDataSource<any>;
  @ViewChild(MatSort) sort: MatSort;
  onSearchClear(){
    this.searchKey="";
    this.applyFilter();
  }
  applyFilter(){
    this.customFieldList.filter = this.searchKey.trim().toLowerCase()
  }
  ngOnInit(): void {
    this.getAllCustomFieldCategory()
  }
  selectCategory(element){
    this.currentCategoryId=element.categoryId;
    this.customFieldList=new MatTableDataSource(element.customFields) ;
    this.customFieldList.sort=this.sort;
  }

   goToAdd(){   
    this.dialog.open(EditStaffFieldsComponent, {
      data: {categoryID:this.currentCategoryId},
      width: '600px'
    }).afterClosed().subscribe((data)=>{
      if(data==='submited'){
        this.getAllCustomFieldCategory();
      }
    });
   }

   goToAddCategory(){   
    this.dialog.open(StaffFieldsCategoryComponent, {
      width: '500px'
    }).afterClosed().subscribe((data)=>{
      if(data==='submited'){
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
    this.customFieldAddView.customFields=element
    this.customFieldservice.deleteCustomField(this.customFieldAddView).subscribe(
      (res:CustomFieldAddView)=>{
        if(typeof(res)=='undefined'){
          this.snackbar.open('Custom Field failed. ' + sessionStorage.getItem("httpError"), '', {
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
            this.getAllCustomFieldCategory()
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
          message: "You are about to delete "+element.title+"."}
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      if(dialogResult){
        this.deleteCustomFieldata(element);
      }
   });
  }
  openEditdata(element){
    this.dialog.open(EditStaffFieldsComponent, {
      data: {information:element},
        width: '800px'
    }).afterClosed().subscribe((data)=>{
      if(data==='submited'){
        this.getAllCustomFieldCategory();
      }
    })
  }
  getAllCustomFieldCategory(){
    this.fieldsCategoryListView.module=this.fieldCategoryModuleEnum.Staff;
    this.customFieldservice.getAllFieldsCategory(this.fieldsCategoryListView).subscribe(
      (res:FieldsCategoryListView)=>{
        if(typeof(res)=='undefined'){
          this.snackbar.open('Field Category list failed. ' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else{
          if (res._failure) {
            this.snackbar.open( res._message, '', {
              duration: 10000
            });
          } 
          else{
            this.fieldsCategoryList= res.fieldsCategoryList 
            if(this.currentCategoryId==null){
              this.currentCategoryId=res.fieldsCategoryList[0].categoryId  
              this.customFieldList=new MatTableDataSource(res.fieldsCategoryList[0].customFields) ;
              this.customFieldList.sort=this.sort;
            }   
            else{
              let index = this.fieldsCategoryList.findIndex((x) => {
                return x.categoryId === this.currentCategoryId
              });
              this.customFieldList=new MatTableDataSource(res.fieldsCategoryList[index].customFields) ;
              this.customFieldList.sort=this.sort;
            }
          }
        }
      }
    );
  }
  editFieldCategory(element){
    this.dialog.open(StaffFieldsCategoryComponent, {
      data: element,
      width: '800px'
    }).afterClosed().subscribe((data)=>{
      if(data==='submited'){
        this.getAllCustomFieldCategory();
      }
    })
  }
  deleteFieldCategory(element){
    this.fieldsCategoryAddView.fieldsCategory = element;
    this.customFieldservice.deleteFieldsCategory(this.fieldsCategoryAddView).subscribe(
      (res: FieldsCategoryAddView) => {
        if (typeof(res) === 'undefined'){
          this.snackbar.open('Field Category delete failed. ' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else{
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
      }
    )
  }
  confirmDeleteFieldCategory(element){
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: {
          title: "Are you sure?",
          message: "You are about to delete "+element.title+"."}
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      if(dialogResult){
        this.deleteFieldCategory(element);
      }
   });
  }

  drop(event: CdkDragDrop<string[]>) {
    this.customFieldDragDropModel.categoryId=this.currentCategoryId
    this.customFieldDragDropModel.currentSortOrder=this.customFieldList.data[event.currentIndex].sortOrder
    this.customFieldDragDropModel.previousSortOrder=this.customFieldList.data[event.previousIndex].sortOrder
    this.customFieldservice.updateCustomFieldSortOrder(this.customFieldDragDropModel).subscribe(
      (res: CustomFieldDragDropModel)=>{
        if(typeof(res)=='undefined'){
          this.snackbar.open('Custom Field Drag short failed. ' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }else{
          if (res._failure) {
            this.snackbar.open(  res._message, '', {
              duration: 10000
            });
          } 
          else{
            this.getAllCustomFieldCategory()
          }
        }
      }
    ); 
  }
 
}
