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

import { Component, Input, OnInit,Output,EventEmitter, ViewChild, OnDestroy } from '@angular/core';
import icMoreVert from '@iconify/icons-ic/twotone-more-vert';
import icAdd from '@iconify/icons-ic/baseline-add';
import icSearch from '@iconify/icons-ic/search';
import icFilterList from '@iconify/icons-ic/filter-list';
import { HttpClient } from 'selenium-webdriver/http';
import { SchoolService } from '../../../../services/school.service';
import { GetAllSchoolModel,AllSchoolListModel } from '../../../../models/get-all-school.model';

import { MatSnackBar } from '@angular/material/snack-bar';
import { Router} from '@angular/router';
import { SelectionModel } from '@angular/cdk/collections';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { MAT_FORM_FIELD_DEFAULT_OPTIONS, MatFormFieldDefaultOptions } from '@angular/material/form-field';
import { stagger40ms } from '../../../../../@vex/animations/stagger.animation';
import { MatSelectChange } from '@angular/material/select';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { LoaderService } from '../../../../services/loader.service';
import { FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged, takeUntil } from 'rxjs/operators';
import { SchoolAddViewModel } from '../../../../models/school-master.model';
import { ImageCropperService } from '../../../../services/image-cropper.service';
import { LayoutService } from 'src/@vex/services/layout.service';
import { ExcelService } from '../../../../services/excel.service';
import { Subject } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { RollBasedAccessService } from '../../../../services/roll-based-access.service';
import { PermissionGroup, RolePermissionListViewModel, RolePermissionViewModel } from 'src/app/models/roll-based-access.model';
import { CryptoService } from '../../../../services/Crypto.service';
import { DefaultValuesService } from '../../../../common/default-values.service';
import { CommonService } from '../../../../services/common.service';
@Component({
  selector: 'vex-school-details',
  templateUrl: './school-details.component.html',
  styleUrls: ['./school-details.component.scss'],
  animations: [
    fadeInUp400ms,
    stagger40ms
  ],
})
export class SchoolDetailsComponent implements OnInit,OnDestroy {
  columns = [
    { label: 'Name', property: 'schoolName', type: 'text', visible: true },
    { label: 'Address', property: 'streetAddress1', type: 'text', visible: true, cssClasses: ['font-medium'] },
    { label: 'Principal', property: 'nameOfPrincipal', type: 'text', visible: true },
    { label: 'Phone', property: 'telephone', type: 'text', visible: true, cssClasses: ['text-secondary', 'font-medium'] },
    { label: 'Status', property: 'status', type: 'text', visible: true }
  ];
  schoolAddViewModel: SchoolAddViewModel = new SchoolAddViewModel();
  selection = new SelectionModel<any>(true, []);
  totalCount:number=0;
  pageNumber:number;
  pageSize:number;
  searchCtrl: FormControl;
  addPermission: boolean= false;
  icMoreVert = icMoreVert;
  icAdd = icAdd;
  icSearch = icSearch;
  icFilterList = icFilterList;
  fapluscircle = "fa-plus-circle";
  permissionListViewModel:RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup:RolePermissionViewModel= new RolePermissionViewModel(); 
  tenant = "";
  loading:boolean;
  getAllSchool: GetAllSchoolModel = new GetAllSchoolModel();
  SchoolModelList: MatTableDataSource<any>;
  destroySubject$: Subject<void> = new Subject();
  showInactiveSchools:boolean=false;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator; 
  @ViewChild(MatSort) sort:MatSort

  constructor(private schoolService: SchoolService,
    private snackbar: MatSnackBar,
    private router: Router,
    private loaderService:LoaderService,
    private layoutService: LayoutService,
    private excelService:ExcelService,
    public translateService: TranslateService,
    public rollBasedAccessService: RollBasedAccessService,
    private defaultService: DefaultValuesService,
    private commonService:CommonService,
    private cryptoService: CryptoService
    ) 
    {
    this.getAllSchool.pageSize = this.defaultService.getPageSize() ? this.defaultService.getPageSize() : 10;
      translateService.use('en');
      if(localStorage.getItem("collapseValue") !== null){
        if( localStorage.getItem("collapseValue") === "false"){
          this.layoutService.expandSidenav();
        }else{
          this.layoutService.collapseSidenav();
        } 
      }else{
        this.layoutService.expandSidenav();
      }
      
      this.getAllSchool.filterParams=null;
      this.loaderService.isLoading.pipe(takeUntil(this.destroySubject$)).subscribe((val) => {
        this.loading = val;
      });
      this.callAllSchool();
}

  ngOnInit(): void {
    this.searchCtrl = new FormControl();
    this.permissionListViewModel= JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x=>x.permissionGroup.permissionGroupId == 2);
    let permissionCategory= this.permissionGroup.permissionGroup.permissionCategory.find(x=>x.permissionCategoryId == 1);
    let permissionSubCategory = permissionCategory.permissionSubcategory.find( x => x.permissionSubcategoryId === 1);
    this.addPermission = permissionSubCategory.rolePermission[0].canAdd;
     let viewPermission= permissionCategory.rolePermission[0].canView;
     if(!viewPermission){
      this.router.navigate(['/']);
     }
  }
  ngAfterViewInit() {
    //  Sorting
    this.getAllSchool=new GetAllSchoolModel();
    this.sort.sortChange.subscribe((res) => {
      this.getAllSchool.pageNumber=this.pageNumber
      this.getAllSchool.pageSize=this.pageSize;
      this.getAllSchool.sortingModel.sortColumn=res.active;
      if(this.searchCtrl.value!=null && this.searchCtrl.value!=""){
        let filterParams=[
          {
           columnName:null,
           filterValue:this.searchCtrl.value,
           filterOption:3
          }
        ]
         Object.assign(this.getAllSchool,{filterParams: filterParams});
      }
      if(res.direction==""){
       this.getAllSchool.sortingModel=null;
      this.callAllSchool();
      this.getAllSchool=new GetAllSchoolModel();
      this.getAllSchool.sortingModel=null;
      }else{
      this.getAllSchool.sortingModel.sortDirection=res.direction;
    this.callAllSchool();
      }
    });

    //  Searching
    this.searchCtrl.valueChanges.pipe(debounceTime(500),distinctUntilChanged()).subscribe((term)=>{
      if(term!=''){
     let filterParams=[
       {
        columnName:null,
        filterValue:term,
        filterOption:3
       }
     ]
     if(this.sort.active!=undefined && this.sort.direction!=""){
      this.getAllSchool.sortingModel.sortColumn=this.sort.active;
      this.getAllSchool.sortingModel.sortDirection=this.sort.direction;
    }
     Object.assign(this.getAllSchool,{filterParams: filterParams});
     this.getAllSchool.pageNumber=1;
     this.paginator.pageIndex=0;
     this.getAllSchool.pageSize=this.pageSize;
     this.callAllSchool();
    }else{
      Object.assign(this.getAllSchool,{filterParams: null});
      this.getAllSchool.pageNumber=this.paginator.pageIndex+1;
     this.getAllSchool.pageSize=this.pageSize;
      if(this.sort.active!=undefined && this.sort.direction!=""){
        this.getAllSchool.sortingModel.sortColumn=this.sort.active;
        this.getAllSchool.sortingModel.sortDirection=this.sort.direction;
      }
     this.callAllSchool();

    }
      })
  }

  goToAdd(){
    this.schoolService.setSchoolId(null);
    this.router.navigate(['/school', 'schoolinfo', 'generalinfo']);
  }  

  includeInactiveSchools(event){
    if(this.sort.active!=undefined && this.sort.direction!=""){
      this.getAllSchool.sortingModel.sortColumn=this.sort.active;
      this.getAllSchool.sortingModel.sortDirection=this.sort.direction;
    }
    if(this.searchCtrl.value!=null && this.searchCtrl.value!=""){
      let filterParams=[
        {
         columnName:null,
         filterValue:this.searchCtrl.value,
         filterOption:3
        }
      ]
     Object.assign(this.getAllSchool,{filterParams: filterParams});
    }
    this.getAllSchool.pageNumber=1;
    this.paginator.pageIndex=0;
    this.getAllSchool.pageSize=this.pageSize;
    this.callAllSchool();
  }

  getPageEvent(event){
    if(this.sort.active!=undefined && this.sort.direction!=""){
      this.getAllSchool.sortingModel.sortColumn=this.sort.active;
      this.getAllSchool.sortingModel.sortDirection=this.sort.direction;
    }
    if(this.searchCtrl.value!=null && this.searchCtrl.value!=""){
      let filterParams=[
        {
         columnName:null,
         filterValue:this.searchCtrl.value,
         filterOption:3
        }
      ]
     Object.assign(this.getAllSchool,{filterParams: filterParams});
    }
    this.getAllSchool.pageNumber=event.pageIndex+1;
    this.getAllSchool.pageSize=event.pageSize;
    this.defaultService.setPageSize(event.pageSize);
    this.callAllSchool();
  }

  checkViewPermission(){
    let categoryId;
    let categoryName;
     for (const permission of this.permissionListViewModel.permissionList[1].permissionGroup.permissionCategory[0].permissionSubcategory){
      if(permission.rolePermission[0].canView){
       categoryId=permission.permissionSubcategoryId;
       categoryName=permission.title;
       break;
      }
    }
    if(categoryId){
      this.checkCurrentCategoryAndRoute(categoryId,categoryName);
    }

  }

  checkCurrentCategoryAndRoute(categoryId,categoryName) {
    this.schoolService.setCategoryTitle(categoryName);
    this.schoolService.setCategoryId(0);
    this.commonService.setModuleName('School');
    if(categoryId === 1) {
      this.router.navigate(['/school', 'schoolinfo', 'generalinfo']);
    } 
    else if(categoryId === 2) {
      this.router.navigate(['/school', 'schoolinfo', 'washinfo']);
    }else if(categoryId>2){
      this.router.navigate(['/school', 'schoolinfo', 'custom', categoryName.toLowerCase().split(' ').join('-') ]);
    }
  
  }
  
  viewGeneralInfo(id:number){    
    this.schoolService.setSchoolId(id);
    if(id===this.defaultService.getSchoolID()){
      this.checkViewPermission()
    }else{
      this.router.navigate(['/school', 'schoolinfo', 'generalinfo']);
    }
  }
  callAllSchool(){
    if(this.getAllSchool.sortingModel?.sortColumn==""){
      this.getAllSchool.sortingModel=null
    }
    this.getAllSchool.includeInactive=this.showInactiveSchools;
    this.schoolService.GetAllSchoolList(this.getAllSchool).subscribe(data => {
      if(data._failure){
        this.snackbar.open(data._message, '', {
        duration: 10000
        });
      }else{
        this.totalCount=data.totalCount;
        this.pageNumber = data.pageNumber;
        this.pageSize = data._pageSize;

        this.SchoolModelList = new MatTableDataSource(data.schoolMaster);
        this.getAllSchool=new GetAllSchoolModel();
      }
    });
  }

  get visibleColumns() {
    return this.columns.filter(column => column.visible).map(column => column.property);
  }

  onFilterChange(value: string) {
    if (!this.SchoolModelList) {
      return;
    }
    value = value.trim();
    value = value.toLowerCase();
    this.SchoolModelList.filter = value;
  }

  toggleColumnVisibility(column, event) {
    event.stopPropagation();
    event.stopImmediatePropagation();
    column.visible = !column.visible;
  }

   exportSchoolListToExcel(){
    let getAllSchool: GetAllSchoolModel = new GetAllSchoolModel();
    getAllSchool.pageNumber=0;
    getAllSchool.pageSize=0;
    getAllSchool.sortingModel=null;
    getAllSchool.includeInactive=this.showInactiveSchools;
      this.schoolService.GetAllSchoolList(getAllSchool).subscribe(res => {
        if(res._failure){
          this.snackbar.open('Failed to Export School List.'+ res._message, '', {
          duration: 10000
          });
        }else{
          if(res.schoolMaster.length>0){
            let schoolList = res.schoolMaster?.map((x)=>{
              return {
                Name:x.schoolName,
                Address: x.streetAddress1 +','+ x.streetAddress2 +','+ x.city +','+ x.state +','+ x.country,
                Principal:x.schoolDetail[0].nameOfPrincipal,
                Phone:x.schoolDetail[0].telephone,
                Status:x.schoolDetail[0].status?'Active':'Inactive'
              }
            });
            this.excelService.exportAsExcelFile(schoolList,'Schools_List_')
          }else{
            this.snackbar.open('No Records Found. Failed to Export School List','', {
              duration: 5000
            });
          }
        }
      });
    
   }

  ngOnDestroy(){
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }

}
