/***********************************************************************************
openSIS is a free student information system for public and non-public
schools from Open Solutions for Education, Inc.Website: www.os4ed.com.

Visit the openSIS product website at https://opensis.com to learn more.
If you have question regarding this software or the license, please contact
via the website.

The software is released under the terms of the GNU Affero General Public License as
published by the Free Software Foundation, version 3 of the License.
See https://www.gnu.org/licenses/agpl-3.0.en.html.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

Copyright (c) Open Solutions for Education, Inc.

All rights reserved.
***********************************************************************************/

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
import {CustomFieldAddView,CustomFieldDragDropModel, CustomFieldListViewModel} from '../../../models/custom-field.model';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { ConfirmDialogComponent } from '../../shared-module/confirm-dialog/confirm-dialog.component';
import { LoaderService } from '../../../services/loader.service';
import { FieldsCategoryListView,FieldsCategoryAddView } from '../../../models/fields-category.model';
import {FieldCategoryModuleEnum} from '../../../enums/field-category-module.enum';
import { CdkDragDrop, moveItemInArray, transferArrayItem} from '@angular/cdk/drag-drop';
import { RolePermissionListViewModel, RolePermissionViewModel } from '../../../models/roll-based-access.model';
import { CryptoService } from '../../../services/Crypto.service';
import { SchoolService } from '../../../services/school.service';

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
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();
  
  constructor(
    private router: Router,
    private dialog: MatDialog,
    public translateService:TranslateService,
    private snackbar: MatSnackBar,
    private customFieldservice:CustomFieldService,
    private loaderService:LoaderService,
    private cryptoService: CryptoService,
    private schoolService:SchoolService
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
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 12);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 24);
    const permissionSubCategory = permissionCategory.permissionSubcategory.find( x => x.permissionSubcategoryId === 26);
    this.editPermission = permissionSubCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionSubCategory.rolePermission[0].canDelete;
    this.addPermission = permissionSubCategory.rolePermission[0].canAdd;
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
        this.schoolService.changeSchoolListStatus({schoolLoaded:false,schoolChanged:true,dataFromUserLogin:false,academicYearChanged:false,academicYearLoaded:false});
        this.getAllCustomFieldCategory();
      }
    });
   }

   goToAddCategory(){   
    this.dialog.open(StaffFieldsCategoryComponent, {
      width: '500px'
    }).afterClosed().subscribe((data)=>{
      if(data==='submited'){
        this.schoolService.changeSchoolListStatus({schoolLoaded:false,schoolChanged:true,dataFromUserLogin:false,academicYearChanged:false,academicYearLoaded:false});
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
        this.schoolService.changeSchoolListStatus({schoolLoaded:false,schoolChanged:true,dataFromUserLogin:false,academicYearChanged:false,academicYearLoaded:false});
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
        this.schoolService.changeSchoolListStatus({schoolLoaded:false,schoolChanged:true,dataFromUserLogin:false,academicYearChanged:false,academicYearLoaded:false});
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
            if (res.fieldsCategoryList == null) {
              this.fieldsCategoryList = [];
              this.snackbar.open( res._message, '', {
                duration: 10000
              });
            } else {
              this.fieldsCategoryList = []
            }
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
      this.schoolService.changeSchoolListStatus({schoolLoaded:false,schoolChanged:true,dataFromUserLogin:false,academicYearChanged:false,academicYearLoaded:false});
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
