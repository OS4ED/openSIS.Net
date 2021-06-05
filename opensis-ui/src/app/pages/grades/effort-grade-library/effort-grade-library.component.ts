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

import { Component, OnInit, Input } from '@angular/core';
import icAdd from '@iconify/icons-ic/baseline-add';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icDelete from '@iconify/icons-ic/twotone-delete';
import icSearch from '@iconify/icons-ic/search';
import icFilterList from '@iconify/icons-ic/filter-list';
import icMoreVertical from '@iconify/icons-ic/baseline-more-vert';
import icCancel from '@iconify/icons-ic/baseline-close';
import { Router} from '@angular/router';
import { fadeInUp400ms } from '../../../../@vex/animations/fade-in-up.animation';
import { stagger40ms } from '../../../../@vex/animations/stagger.animation';
import { TranslateService } from '@ngx-translate/core';
import { MatDialog } from '@angular/material/dialog';
import { EditEffortItemComponent } from './edit-effort-item/edit-effort-item.component';
import { GradesService } from '../../../services/grades.service';
import { EffortGradeLibraryCategoryAddViewModel, 
  EffortGradeLibraryCategoryItemAddViewModel, 
  EffortGradeLibraryCategoryListView, 
  EffortGradeLlibraryDragDropModel } from '../../../models/grades.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { ConfirmDialogComponent } from '../../shared-module/confirm-dialog/confirm-dialog.component';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { ExcelService } from '../../../services/excel.service';
import { ValidationService } from '../../shared/validation.service';
import { takeUntil } from 'rxjs/operators';
import { LoaderService } from '../../../services/loader.service';
import { Subject } from 'rxjs';
import { CryptoService } from '../../../services/Crypto.service';
import { RolePermissionListViewModel, RolePermissionViewModel } from '../../../models/roll-based-access.model';

@Component({
  selector: 'vex-effort-grade-library',
  templateUrl: './effort-grade-library.component.html',
  styleUrls: ['./effort-grade-library.component.scss'],
  animations: [
    fadeInUp400ms,
    stagger40ms
  ]
})
export class EffortGradeLibraryComponent implements OnInit {
  columns = [
    { label: 'Order', property: 'order', type: 'number', visible: true },
    { label: 'Title', property: 'effortItemTitle', type: 'text', visible: true },
    { label: 'Action', property: 'action', type: 'text', visible: true }
  ];

  PeriodsModelList;

  icAdd = icAdd;
  icEdit = icEdit;
  icDelete = icDelete;
  icSearch = icSearch;
  icFilterList = icFilterList;
  icMoreVertical = icMoreVertical;
  icCancel = icCancel;
  addCategory = false;
  loading:Boolean;
  effortGradeLibraryCategoryListView:EffortGradeLibraryCategoryListView=new EffortGradeLibraryCategoryListView();
  effortGradeLibraryCategoryAddViewModel:EffortGradeLibraryCategoryAddViewModel=new EffortGradeLibraryCategoryAddViewModel();
  effortGradeLibraryCategoryItemAddViewModel:EffortGradeLibraryCategoryItemAddViewModel=new EffortGradeLibraryCategoryItemAddViewModel();
  effortGradeLlibraryDragDropModel:EffortGradeLlibraryDragDropModel=new EffortGradeLlibraryDragDropModel();
  effortCategoriesList = [];
  currentEffortCategoryId;
  effortItemList:MatTableDataSource<any>;
  form:FormGroup
  buttonType: string;
  effortCategoryTitle: string;
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();
  destroySubject$: Subject<void> = new Subject();

  constructor(
    private router: Router,
    private dialog: MatDialog,
    public translateService:TranslateService,
    private snackbar: MatSnackBar,
    private gradesService:GradesService,
    private fb:FormBuilder,
    private loaderService: LoaderService,
    private excelService:ExcelService,
    private cryptoService: CryptoService
    ) {
    translateService.use('en');
    this.loaderService.isLoading.pipe(takeUntil(this.destroySubject$)).subscribe((val) => {
      this.loading = val;
    });
    this.form=fb.group({
      effortCategoryId:[0],
      categoryName:['',[ValidationService.noWhitespaceValidator]]
    });
  }

  ngOnInit(): void {
    this.getAllEffortGradeLlibraryCategoryList();
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 12);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 26);
    const permissionSubCategory = permissionCategory.permissionSubcategory.find( x => x.permissionSubcategoryId === 29);
    this.editPermission = permissionSubCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionSubCategory.rolePermission[0].canDelete;
    this.addPermission = permissionSubCategory.rolePermission[0].canAdd;
  }
  selectEffortCategory(element){
    this.currentEffortCategoryId=element.effortCategoryId;
    this.effortItemList=new MatTableDataSource(element.effortGradeLibraryCategoryItem) ;
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

  goToAddEffortItem() {
    this.dialog.open(EditEffortItemComponent, {
      data: {effortCategoryId:this.currentEffortCategoryId},
      width: '500px'
    }).afterClosed().subscribe((data)=>{
      if(data==='submited'){
        this.getAllEffortGradeLlibraryCategoryList();
      }
    });
  }
  goToEditEffortItem(element) {
    this.dialog.open(EditEffortItemComponent, {
      data: {information:element},
      width: '500px'
    }).afterClosed().subscribe((data)=>{
      if(data==='submited'){
        this.getAllEffortGradeLlibraryCategoryList();
      }
    });
  }

  goToAddCategory() {
    
    this.effortCategoryTitle="addNewEffortCategory";
    this.addCategory = true;
    this.buttonType="submit";
  }
  goToEditCategory(element) {
    // });
    this.effortCategoryTitle="editEffortCategory";
    this.buttonType="update";
    this.form.controls.effortCategoryId.patchValue(element.effortCategoryId);
    this.form.controls.categoryName.patchValue(element.categoryName);
    this.addCategory = true;
    
  }

  closeAddCategory() {
    this.addCategory = false;
  }

  get visibleColumns() {
    return this.columns.filter(column => column.visible).map(column => column.property);
  }
  deleteEffortCategory(element){
    this.effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory=element
    this.gradesService.deleteEffortGradeLibraryCategory(this.effortGradeLibraryCategoryAddViewModel).subscribe(
      (res:EffortGradeLibraryCategoryAddViewModel)=>{
        if(typeof(res)=='undefined'){
          this.snackbar.open('' + sessionStorage.getItem("httpError"), '', {
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
            if (element.effortCategoryId === this.currentEffortCategoryId){
              this.currentEffortCategoryId = null;
            }
            this.getAllEffortGradeLlibraryCategoryList();
          }
        }
      }
    );
  }
  confirmDeleteEffortCategory(element){
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: {
          title: "Are you sure?",
          message: "You are about to delete "+element.categoryName+"."}
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      if(dialogResult){
        this.deleteEffortCategory(element);
      }
   });
  }
  deleteEffortItems(element) {
    this.effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem=element
    this.gradesService.deleteEffortGradeLibraryCategoryItem(this.effortGradeLibraryCategoryItemAddViewModel).subscribe(
      (res:EffortGradeLibraryCategoryItemAddViewModel)=>{
        if(typeof(res)=='undefined'){
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
            this.getAllEffortGradeLlibraryCategoryList()
          }
        }
      }
    );

  }
  confirmDeleteEffortItems(element){
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: {
          title: "Are you sure?",
          message: "You are about to delete "+element.effortItemTitle+"."}
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      if(dialogResult){
        this.deleteEffortItems(element);
      }
   });
  }
  getAllEffortGradeLlibraryCategoryList(){
    this.gradesService.getAllEffortGradeLlibraryCategoryList(this.effortGradeLibraryCategoryListView).subscribe(
      (res:EffortGradeLibraryCategoryListView)=>{
        if(typeof(res)=='undefined'){
          this.snackbar.open('' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else{
          if (res._failure) {
            if (res.effortGradeLibraryCategoryList == null) {
              this.effortCategoriesList=null
              this.snackbar.open( res._message, '', {
                duration: 10000
              });
            } else {
              this.effortCategoriesList=null;
            }
          } 
          else{
            this.effortCategoriesList= res.effortGradeLibraryCategoryList;
            if(this.currentEffortCategoryId==null){
              this.currentEffortCategoryId=res.effortGradeLibraryCategoryList[0]?.effortCategoryId
              this.effortItemList=new MatTableDataSource(res.effortGradeLibraryCategoryList[0]?.effortGradeLibraryCategoryItem) ;
            }
            else{
              let index = this.effortCategoriesList.findIndex((x) => {
                return x.effortCategoryId === this.currentEffortCategoryId
              });
              this.effortItemList=new MatTableDataSource(res.effortGradeLibraryCategoryList[index].effortGradeLibraryCategoryItem) ;
            }
          }
        }
      }
    );
  }
  cancelSubmit(){
    this.form.reset();
  }
  submit(){
    this.form.markAllAsTouched();
    if(this.form.valid){
      if(this.effortCategoryTitle=="addNewEffortCategory"){
        this.effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory.effortCategoryId=0
        this.effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory.categoryName=this.form.controls.categoryName.value
        this.gradesService.addEffortGradeLibraryCategory(this.effortGradeLibraryCategoryAddViewModel).subscribe(
          (res:EffortGradeLibraryCategoryAddViewModel)=>{
            if(typeof(res)=='undefined'){
              this.snackbar.open('' + sessionStorage.getItem("httpError"), '', {
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
                this.snackbar.open(''+res._message, '', {
                  duration: 10000
                }); 
                this.getAllEffortGradeLlibraryCategoryList();
                this.addCategory=false;
                this.form.reset();
              }
            }
          }
        );
      }
      else{
        this.effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory.effortCategoryId=this.form.controls.effortCategoryId.value
        this.effortGradeLibraryCategoryAddViewModel.effortGradeLibraryCategory.categoryName=this.form.controls.categoryName.value
        this.gradesService.updateEffortGradeLibraryCategory(this.effortGradeLibraryCategoryAddViewModel).subscribe(
          (res:EffortGradeLibraryCategoryAddViewModel)=>{
   if(typeof(res)=='undefined'){
              this.snackbar.open('' + sessionStorage.getItem("httpError"), '', {
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
                this.snackbar.open(''+res._message, '', {
                  duration: 10000
                }); 
                this.getAllEffortGradeLlibraryCategoryList();
                this.addCategory=false;
                this.form.reset();
              }
            }
          }
        );
      }
    }
  }
  dropEffortCategory(event: CdkDragDrop<string[]>){
    this.effortGradeLlibraryDragDropModel.currentSortOrder=this.effortCategoriesList[event.currentIndex].sortOrder
    this.effortGradeLlibraryDragDropModel.previousSortOrder=this.effortCategoriesList[event.previousIndex].sortOrder
    this.effortGradeLlibraryDragDropModel.effortCategoryId=0;
    
    this.gradesService.updateEffortGradeLlibraryCategorySortOrder(this.effortGradeLlibraryDragDropModel).subscribe(
      (res:EffortGradeLlibraryDragDropModel)=>{
        if(typeof(res)=='undefined'){
          this.snackbar.open('' + sessionStorage.getItem("httpError"), '', {
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
            this.getAllEffortGradeLlibraryCategoryList()
          } 
        }
      }
    );
  }
  dropEffortItem(event: CdkDragDrop<string[]>){
    this.effortGradeLlibraryDragDropModel.currentSortOrder=this.effortItemList.data[event.currentIndex].sortOrder
    this.effortGradeLlibraryDragDropModel.previousSortOrder=this.effortItemList.data[event.previousIndex].sortOrder
    this.effortGradeLlibraryDragDropModel.effortCategoryId=this.currentEffortCategoryId
    
    this.gradesService.updateEffortGradeLlibraryCategorySortOrder(this.effortGradeLlibraryDragDropModel).subscribe(
      (res:EffortGradeLlibraryDragDropModel)=>{
        if(typeof(res)=='undefined'){
          this.snackbar.open('' + sessionStorage.getItem("httpError"), '', {
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
            this.getAllEffortGradeLlibraryCategoryList()
          } 
        }
      }
    );


  }

  translateKey(key) {
    let trnaslateKey;
    this.translateService.get(key).subscribe((res: string) => {
       trnaslateKey = res;
    });
    return trnaslateKey;
  }

  exportToExcel(){
    if (this.effortItemList.data?.length > 0) {
      let reportList = this.effortItemList.data?.map((x) => {
        return {
          [this.translateKey('title')]: x.effortItemTitle
        }
      });
      this.excelService.exportAsExcelFile(reportList,"Effort_Item_List_")
    } else {
      this.snackbar.open('No records found. failed to export effort item list', '', {
        duration: 5000
      });
    }
  }

  ngOnDestroy(){
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }
}
