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

import { Component, OnInit, Input,ViewChild } from '@angular/core';
import icMoreVert from '@iconify/icons-ic/twotone-more-vert';
import icAdd from '@iconify/icons-ic/baseline-add';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icDelete from '@iconify/icons-ic/twotone-delete';
import icSearch from '@iconify/icons-ic/search';
import icFilterList from '@iconify/icons-ic/filter-list';
import icImpersonate from '@iconify/icons-ic/twotone-account-circle';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router} from '@angular/router';
import { fadeInUp400ms } from '../../../../@vex/animations/fade-in-up.animation';
import { stagger40ms } from '../../../../@vex/animations/stagger.animation';
import { TranslateService } from '@ngx-translate/core';
import { CountryModel } from '../../../models/country.model';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { LoaderService } from '../../../services/loader.service';
import { CommonService } from '../../../services/common.service';
import { EditCountryComponent } from './edit-country/edit-country.component';
import { MatPaginator } from '@angular/material/paginator';
import { CountryAddModel } from '../../../models/country.model';
import { ConfirmDialogComponent } from '../../shared-module/confirm-dialog/confirm-dialog.component';
import { ExcelService } from '../../../services/excel.service';
import { SharedFunction } from '../../shared/shared-function';
import { RolePermissionListViewModel, RolePermissionViewModel } from 'src/app/models/roll-based-access.model';
import { CryptoService } from '../../../services/Crypto.service';

@Component({
  selector: 'vex-countries',
  templateUrl: './countries.component.html',
  styleUrls: ['./countries.component.scss'],
  animations: [
    fadeInUp400ms,
    stagger40ms
  ]
})
export class CountriesComponent implements OnInit {
  @Input()
  columns = [
    { label: 'Title', property: 'name', type: 'text', visible: true },
    { label: 'Short Name', property: 'countryCode', type: 'text', visible: true },
    { label: 'Created By', property: 'createdBy', type: 'text', visible: true },
    { label: 'Create Date', property: 'createdOn', type: 'text', visible: true },
    { label: 'Updated By', property: 'updatedBy', type: 'text', visible: true },
    { label: 'Update Date', property: 'updatedOn', type: 'text', visible: true },
    { label: 'Actions', property: 'actions', type: 'text', visible: true }
  ];  
  icMoreVert = icMoreVert;
  icAdd = icAdd;
  icEdit = icEdit;
  icDelete = icDelete;
  icSearch = icSearch;
  icImpersonate = icImpersonate;
  icFilterList = icFilterList; 
  loading;  
  countryAddModel = new CountryAddModel();
  totalCount:Number;pageNumber:Number;pageSize:Number;
  getCountryModel: CountryModel = new CountryModel();  
  CountryModelList: MatTableDataSource<any>;
  countryListForExcel=[];
  searchKey:string;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();

  constructor(private router: Router,
    private dialog: MatDialog,
    public translateService:TranslateService,
    public loaderService:LoaderService,
    public commonService:CommonService,
    public snackbar:MatSnackBar,
    private excelService:ExcelService,
    public commonfunction:SharedFunction,
    private cryptoService: CryptoService
    ) {
    translateService.use('en');
    this.loaderService.isLoading.subscribe((val) => {
      this.loading = val;
    });
    this.getCountryList();
  }

  ngOnInit(): void {
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 12);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 28);
    const permissionSubCategory = permissionCategory.permissionSubcategory.find( x => x.permissionSubcategoryId === 35);
    this.editPermission = permissionSubCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionSubCategory.rolePermission[0].canDelete;
    this.addPermission = permissionSubCategory.rolePermission[0].canAdd;
  }

  getCountryList(){    
    this.commonService.GetAllCountry(this.getCountryModel).subscribe(data => {
      if (typeof (data) == 'undefined') {
        this.snackbar.open('Country list failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }else{
        if (data._failure) {
          if (data._message === "NO RECORD FOUND") {
            this.CountryModelList = new MatTableDataSource(data.tableCountry);
            this.CountryModelList.sort=this.sort;      
            this.CountryModelList.paginator = this.paginator;   
          } else {
            this.CountryModelList = new MatTableDataSource([]);
            this.CountryModelList.sort=null;      
            this.CountryModelList.paginator = null;
            this.snackbar.open('' + data._message, '', {
              duration: 10000
            });
          }
        }else{
          this.CountryModelList = new MatTableDataSource(data.tableCountry);
          this.countryListForExcel= data.tableCountry;
          this.CountryModelList.sort=this.sort;      
          this.CountryModelList.paginator = this.paginator;  
        }
      }
    });
  }

  translateKey(key) {
    let trnaslateKey;
   this.translateService.get(key).subscribe((res: string) => {
       trnaslateKey = res;
    });
    return trnaslateKey;
  }

  exportCountryListToExcel(){
    if(this.countryListForExcel.length!=0){
      let countryList=this.countryListForExcel?.map((item)=>{
        return{
          [this.translateKey('title')]: item.name,
          [this.translateKey('shortName')]: item.countryCode,
          [this.translateKey('createdBy')]: item.createdBy ? item.createdBy: '-',
          [this.translateKey('createDate')]: this.commonfunction.transformDateWithTime(item.createdOn),
          [this.translateKey('updatedBy')]: item.updatedBy ? item.updatedBy: '-',
          [this.translateKey('updateDate')]:  this.commonfunction.transformDateWithTime(item.updatedOn)
        }
      });
      this.excelService.exportAsExcelFile(countryList,'Country_List_')
     }
     else{
    this.snackbar.open('No Records Found. Failed to Export Country List','', {
      duration: 5000
    });
  }
}

  goToAdd(){
    this.dialog.open(EditCountryComponent, {      
      width: '500px'
    }).afterClosed().subscribe((data) => {
      if(data){
        this.getCountryList();
      }            
    });   
  }

  deleteCountryData(element){
    this.countryAddModel.country.id = element;
    this.commonService.DeleteCountry(this.countryAddModel).subscribe(
      (res)=>{
        if(typeof(res)=='undefined'){
          this.snackbar.open('Country Deletion failed. ' + sessionStorage.getItem("httpError"), '', {
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
            this.snackbar.open('' + res._message, '', {
              duration: 10000
            });
            this.getCountryList()
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
          message: "You are about to delete "+element.name+"."}
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      if(dialogResult){
        this.deleteCountryData(element.id);
      }
   });
  }
  goToEdit(editDetails){
    this.dialog.open(EditCountryComponent, {
      data: editDetails,  
      width: '500px'
    }).afterClosed().subscribe((data) => {
      if(data){
        this.getCountryList();
      }            
    });   
  }

  toggleColumnVisibility(column, event) {
    event.stopPropagation();
    event.stopImmediatePropagation();
    column.visible = !column.visible;
  }

  get visibleColumns() {
    return this.columns.filter(column => column.visible).map(column => column.property);
  }

  onSearchClear(){
    this.searchKey="";
    this.applyFilter();
  }

  applyFilter(){
    this.CountryModelList.filter = this.searchKey.trim().toLowerCase()
  }

}
