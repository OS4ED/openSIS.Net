import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { fadeInUp400ms } from '../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../@vex/animations/stagger.animation';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icDelete from '@iconify/icons-ic/twotone-delete';
import icSearch from '@iconify/icons-ic/twotone-search';
import icAdd from '@iconify/icons-ic/twotone-add';
import icFilterList from '@iconify/icons-ic/twotone-filter-list';
import { EditRoomComponent } from '../rooms/edit-room/edit-room.component';
import { RoomDetailsComponent } from '../rooms/room-details/room-details.component';

import { RoomAddView,RoomListViewModel } from '../../../models/room.model'
import { RoomService } from '../../../services/room.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { TranslateService } from '@ngx-translate/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ConfirmDialogComponent } from '../../shared-module/confirm-dialog/confirm-dialog.component';
import { LoaderService } from '../../../services/loader.service';
import { LayoutService } from '../../../../@vex/services/layout.service';
import { ExcelService } from '../../../services/excel.service';
import { CryptoService } from '../../../services/Crypto.service';
import { RolePermissionListViewModel, RolePermissionViewModel } from '../../../models/roll-based-access.model';
@Component({
  selector: 'vex-rooms',
  templateUrl: './rooms.component.html',
  styleUrls: ['./rooms.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class RoomsComponent implements OnInit {

  icEdit = icEdit;
  icDelete = icDelete;
  icSearch = icSearch;
  icAdd = icAdd;
  icFilterList = icFilterList;
  roomaddviewmodel:RoomAddView= new RoomAddView();
  roomListViewModel:RoomListViewModel = new RoomListViewModel()
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();
  roomDetails: any;
  loading:boolean;
  searchKey:string;
  constructor(private dialog: MatDialog,
              private roomService: RoomService,
              private snackbar: MatSnackBar,
              private excelService: ExcelService,
              private translateService: TranslateService,
              private loaderService: LoaderService, private layoutService: LayoutService,
              private cryptoService: CryptoService) {
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
      this.loaderService.isLoading.subscribe((val) => {
        this.loading = val;
      }); 
     }
  roomModelList: MatTableDataSource<any>;
  @ViewChild(MatPaginator) paginator:MatPaginator
  @ViewChild(MatSort) sort: MatSort;
  columns = [
    { label: 'Title', property: 'title', type: 'text', visible: true },
    { label: 'Capacity', property: 'capacity', type: 'text', visible: true, cssClasses: ['font-medium'] },
    { label: 'Description', property: 'description', type: 'text', visible: true },
    { label: 'Sort Order', property: 'sortOrder', type: 'text', visible: true, cssClasses: ['text-secondary', 'font-medium'] },
    { label: 'isActive', property: 'isActive', type: 'text', visible: true, cssClasses: ['text-secondary', 'font-medium'] },
    { label: 'Action', property: 'action', type: 'text', visible: true },
  ];

  ngOnInit(): void {
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 12);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 22);
    const permissionSubCategory = permissionCategory.permissionSubcategory.find( x => x.permissionSubcategoryId === 20);
    this.editPermission = permissionSubCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionSubCategory.rolePermission[0].canDelete;
    this.addPermission = permissionSubCategory.rolePermission[0].canAdd;
    this.roomListViewModel.schoolId=+sessionStorage.getItem("selectedSchoolId")
    this.getAllRooms()
  }
  getAllRooms(){
    this.roomService.getAllRoom(this.roomListViewModel).subscribe(
      (res: RoomListViewModel) => {
        if(typeof(res) === 'undefined'){
          this.snackbar.open('' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else{
          if (res._failure) {
            if (res.tableroomList == null) {
              this.roomModelList = new MatTableDataSource([]) ;
              this.roomModelList.sort = this.sort;
              this.snackbar.open( res._message, '', {
                duration: 10000
              });
            }
            else {
              this.roomModelList = new MatTableDataSource([]) ;
              this.roomModelList.sort = this.sort;
            }
          } 
          else { 
            this.roomModelList=new MatTableDataSource(res.tableroomList) ;
            this.roomModelList.sort=this.sort;      
          }
        }
      })
  }

  openAddNew() {
    this.dialog.open(EditRoomComponent, {
      data: null,
      width: '600px'
    }).afterClosed().subscribe(data => {
        if(data==='submited'){
          this.getAllRooms();
        }
      });
  }

  openViewDetails(element) {
    this.dialog.open(RoomDetailsComponent, {
      data: {info:element},
      width: '600px'
    });
  }

  onSearchClear(){
    this.searchKey = '';
    this.applyFilter();
  }

  applyFilter(){
    this.roomModelList.filter = this.searchKey.trim().toLowerCase()
  }
  
  get visibleColumns() {
    return this.columns.filter(column => column.visible).map(column => column.property);
  }

  toggleColumnVisibility(column, event) {
    event.stopPropagation();
    event.stopImmediatePropagation();
    column.visible = !column.visible;
  }

  openEditdata(element){
    this.dialog.open(EditRoomComponent, {
      data: element,
        width: '600px'
    }).afterClosed().subscribe((data) => {
      if (data === 'submited'){
        this.getAllRooms();
      }
    });
  }
  deleteRoomdata(element){
    this.roomaddviewmodel.tableRoom = element;
    this.roomService.deleteRoom(this.roomaddviewmodel).subscribe(
      (res: RoomAddView) => {
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
          else {
            this.getAllRooms();
          }
        }
      }
    );
  }
  confirmDelete(element){
      const dialogRef = this.dialog.open(ConfirmDialogComponent, {
        maxWidth: '400px',
        data: {
            title: 'Are you sure?',
            message: 'You are about to delete ' + element.title + '.'}
      });
      dialogRef.afterClosed().subscribe(dialogResult => {
        if (dialogResult){
          this.deleteRoomdata(element);
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

  exportToExcel(){
    if (this.roomModelList.data?.length > 0) {
      const reportList = this.roomModelList.data?.map((x) => {
        return {
          [this.translateKey('title')]: x.title,
          [this.translateKey('capacity')]: x.capacity,
          [this.translateKey('description')]: x.description,
          [this.translateKey('sortOrder')]: x.sortOrder,
          [this.translateKey('active')]: x.isActive? this.translateKey('yes'):this.translateKey('no')
        };
      });
      this.excelService.exportAsExcelFile(reportList, 'Rooms_List_');
    } else {
      this.snackbar.open('No records found. failed to export Rooms List', '', {
        duration: 5000
      });
    }
  }
}
