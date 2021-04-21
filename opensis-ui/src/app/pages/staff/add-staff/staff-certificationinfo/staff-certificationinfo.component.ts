import {StaffCertificateListModel,StaffCertificateModel } from '../../../../models/staff.model';
import { StaffService } from '../../../../services/staff.service';
import { Component, OnInit, Input, ViewChild } from '@angular/core';
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
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger40ms } from '../../../../../@vex/animations/stagger.animation';
import { TranslateService } from '@ngx-translate/core';
import { AddCertificateComponent } from './add-certificate/add-certificate.component';
import { CertificateDetailsComponent } from './certificate-details/certificate-details.component';
import { MatTableDataSource } from '@angular/material/table';
import { ConfirmDialogComponent } from '../../../shared-module/confirm-dialog/confirm-dialog.component';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { RolePermissionListViewModel, RolePermissionViewModel } from 'src/app/models/roll-based-access.model';
import { RollBasedAccessService } from 'src/app/services/roll-based-access.service';

@Component({
  selector: 'vex-staff-certificationinfo',
  templateUrl: './staff-certificationinfo.component.html',
  styleUrls: ['./staff-certificationinfo.component.scss'],
  animations: [
    fadeInUp400ms,
    stagger40ms
  ]
})
export class StaffCertificationinfoComponent implements OnInit {
  @Input()
  columns = [
    { label: 'Certification Name', property: 'certificationName', type: 'text', visible: true },
    { label: 'Short Name', property: 'shortName', type: 'text', visible: true },
    { label: 'Primary Certification Indicator', property: 'primaryCertification', type: 'text', visible: true },
    { label: 'Certification Date', property: 'certificationDate', type: 'text', visible: true },
    { label: 'Certification Expiry Date', property: 'certificationExpiryDate', type: 'text', visible: true },
    { label: 'Actions', property: 'actions', type: 'text', visible: true }
  ];


  icMoreVert = icMoreVert;
  icAdd = icAdd;
  icEdit = icEdit;
  icDelete = icDelete;
  icSearch = icSearch;
  icImpersonate = icImpersonate;
  icFilterList = icFilterList;
  loading: boolean;
  staffCertificateListModel: StaffCertificateListModel = new StaffCertificateListModel();
  staffCertificateModel: StaffCertificateModel = new StaffCertificateModel();
  staffid;
  staffCertificateList: MatTableDataSource<any>;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  searchKey: string;
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();
  constructor(private router: Router,
              private dialog: MatDialog,
              public translateService: TranslateService,
              private snackbar: MatSnackBar,
              public rollBasedAccessService: RollBasedAccessService,
              private staffService: StaffService
              ) {
    translateService.use('en');
  }

  ngOnInit(): void {
    this.permissionListViewModel = this.rollBasedAccessService.getPermission();
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 5);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 10);
    const permissionSubCategory = permissionCategory.permissionSubcategory.find( x => x.permissionSubcategoryId === 16);
    this.editPermission = permissionSubCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionSubCategory.rolePermission[0].canDelete;
    this.addPermission = permissionSubCategory.rolePermission[0].canAdd;
    this.staffid = this.staffService.getStaffId();
    this.getAllStaffCertificateInfo();
  }

  getPageEvent(event){    
    // this.getAllSchool.pageNumber=event.pageIndex+1;
    // this.getAllSchool.pageSize=event.pageSize;
    // this.callAllSchool(this.getAllSchool);
  }
  onSearchClear(){
    this.searchKey="";
    this.applyFilter();
  }
  applyFilter(){
    this.staffCertificateList.filter = this.searchKey.trim().toLowerCase()
  }
  openAddNew() {
    this.dialog.open(AddCertificateComponent, {
      data: null,
      width: '600px'
    }).afterClosed().subscribe((data)=>{
      if(data==='submited'){
        this.getAllStaffCertificateInfo();
      }
    });
  }
  openEditdata(element) {
    
    this.dialog.open(AddCertificateComponent, {
      data: element,
      width: '600px'
    }).afterClosed().subscribe((data)=>{
      if(data==='submited'){
        this.getAllStaffCertificateInfo();
      }
    })
  }
  openViewDetails(element) {
    this.dialog.open(CertificateDetailsComponent, {
      data: {info:element},
      width: '600px'
    })
  }

  toggleColumnVisibility(column, event) {
    event.stopPropagation();
    event.stopImmediatePropagation();
    column.visible = !column.visible;
  }
  getAllStaffCertificateInfo(){
    this.staffCertificateListModel.staffId=this.staffService.getStaffId();
    this.staffService.getAllStaffCertificateInfo(this.staffCertificateListModel).subscribe(
      (res:StaffCertificateListModel)=>{
        if(typeof(res)=='undefined'){
          this.snackbar.open('Staff Certificate List failed. ' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else{
          if (res._failure) {  
              if(res.staffCertificateInfoList==null){
                this.staffCertificateList=new MatTableDataSource([]) ;
                this.staffCertificateList.sort=this.sort;
                this.snackbar.open(res._message, '', {
                  duration: 10000
                });
              }
              else{
                this.staffCertificateList=new MatTableDataSource(res.staffCertificateInfoList) ;
                this.staffCertificateList.sort=this.sort; 
              }
            
          } 
          else { 
            this.staffCertificateList=new MatTableDataSource(res.staffCertificateInfoList) ;
            this.staffCertificateList.sort=this.sort;    
          }
        }
      }
    );
  }

  get visibleColumns() {
    return this.columns.filter(column => column.visible).map(column => column.property);
  }
  deleteStaffCertificatedata(element){
    this.staffCertificateModel.staffCertificateInfo=element
    this.staffService.deleteStaffCertificateInfo(this.staffCertificateModel).subscribe(
      (res:StaffCertificateModel)=>{
        if(typeof(res)=='undefined'){
          this.snackbar.open('Staff Certificate Delete failed. ' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else{
          if (res._failure) {
            this.snackbar.open(res._message, '', {
              duration: 10000
            });
          } 
          else { 
            this.getAllStaffCertificateInfo()
          }
        }
      }
    )
  }
  confirmDelete(element){
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: '400px',
      data: {
          title: 'Are you sure?',
          message: 'You are about to delete ' + element.certificationName + '.'}
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult){
        this.deleteStaffCertificatedata(element);
      }
   });
}

}
