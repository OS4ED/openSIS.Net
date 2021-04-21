import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import icMoreVert from '@iconify/icons-ic/twotone-more-vert';
import icAdd from '@iconify/icons-ic/baseline-add';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icDelete from '@iconify/icons-ic/twotone-delete';
import icSearch from '@iconify/icons-ic/search';
import icFilterList from '@iconify/icons-ic/filter-list';
import icImport from '@iconify/icons-ic/twotone-unarchive';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router} from '@angular/router';
import { fadeInUp400ms } from '../../../../@vex/animations/fade-in-up.animation';
import { stagger40ms } from '../../../../@vex/animations/stagger.animation';
import { TranslateService } from '@ngx-translate/core';
import { GradesService } from '../../../services/grades.service';
import { EditHonorRollComponent } from './edit-honor-roll/edit-honor-roll.component';
import { GetHonorRollModel, HonorRollAddViewModel, HonorRollListModel } from '../../../models/grades.model';
import { MatTableDataSource } from '@angular/material/table';
import { ConfirmDialogComponent } from '../../shared-module/confirm-dialog/confirm-dialog.component';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { ExcelService } from '../../../services/excel.service';
import { LoaderService } from '../../../services/loader.service';
import { CryptoService } from 'src/app/services/Crypto.service';
import { RolePermissionListViewModel, RolePermissionViewModel } from 'src/app/models/roll-based-access.model';

@Component({
  selector: 'vex-honor-roll-setup',
  templateUrl: './honor-roll-setup.component.html',
  styleUrls: ['./honor-roll-setup.component.scss'],
  animations: [
    fadeInUp400ms,
    stagger40ms
  ]
})
export class HonorRollSetupComponent implements OnInit,AfterViewInit {

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator; 
  @ViewChild(MatSort) sort:MatSort;
  
  columns = [
    { label: 'Honor Roll', property: 'honorRoll', type: 'text', visible: true },
    { label: 'Break Off', property: 'breakoff', type: 'text', visible: true },
    { label: 'Actions', property: 'actions', type: 'text', visible: true }
  ];

  EffortGradeScaleModelList;
  honorROllModList:MatTableDataSource<any>;

  icMoreVert = icMoreVert;
  icAdd = icAdd;
  icEdit = icEdit;
  icDelete = icDelete;
  icSearch = icSearch;
  icImport = icImport;
  icFilterList = icFilterList;
  loading:Boolean;
  honorRollListModel:HonorRollListModel= new HonorRollListModel();
  honorRollAddViewModel:HonorRollAddViewModel=new HonorRollAddViewModel();
  getHonorRollModel:GetHonorRollModel=new GetHonorRollModel();
  pageNumber: number;
  pageSize: number;
  searchCtrl: FormControl;
  totalCount: any;
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel()
  constructor(
    private router: Router,
    private dialog: MatDialog,
    private gradesService:GradesService,
    private snackbar: MatSnackBar,
    private excelService:ExcelService,
    private loaderService:LoaderService,
    public translateService:TranslateService,
    private cryptoService: CryptoService) {
    translateService.use('en');
    this.loaderService.isLoading.subscribe((val) => {
      this.loading = val;
    }); 
    this.EffortGradeScaleModelList = [
      {honor_roll: 'Bronze', break_off: '80'},
      {honor_roll: 'Silver', break_off: '85'},
      {honor_roll: 'Gold', break_off: '90'},
      {honor_roll: 'Platinum', break_off: '96'}
    ]
  }
  ngAfterViewInit(): void {
    //  Sorting
    this.sort.sortChange.subscribe(
      (res)=>{
        this.honorRollListModel=new HonorRollListModel();
        this.honorRollListModel.pageNumber=this.pageNumber;
        this.honorRollListModel.pageSize=this.pageSize;
        this.honorRollListModel.sortingModel.sortColumn=res.active;
        if(this.searchCtrl.value!=null && this.searchCtrl.value!=""){
          let filterParams=[
            {
             columnName:null,
             filterValue:this.searchCtrl.value,
             filterOption:3
            }
          ]
           Object.assign(this.honorRollListModel,{filterParams: filterParams});
        }
        if(res.direction==""){
          this.honorRollListModel.sortingModel=null;
          this.getAllHonorRollList();
          this.honorRollListModel=new HonorRollListModel();
          this.honorRollListModel.sortingModel=null;
         }else{
          this.honorRollListModel.sortingModel.sortDirection=res.direction;
          this.getAllHonorRollList();
         }
      }
    );
    //searching
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
          this.honorRollListModel.sortingModel.sortColumn=this.sort.active;
          this.honorRollListModel.sortingModel.sortDirection=this.sort.direction;
        }
        Object.assign(this.honorRollListModel,{filterParams: filterParams});
        this.honorRollListModel.pageNumber=1;
        this.paginator.pageIndex=0;
        this.honorRollListModel.pageSize=this.pageSize;
        this.getAllHonorRollList();
      }
      else{
        Object.assign(this.honorRollListModel,{filterParams: null});
        this.honorRollListModel.pageNumber=this.paginator.pageIndex+1;
        this.honorRollListModel.pageSize=this.pageSize;
        if(this.sort.active!=undefined && this.sort.direction!=""){
          this.honorRollListModel.sortingModel.sortColumn=this.sort.active;
          this.honorRollListModel.sortingModel.sortDirection=this.sort.direction;
        }
        this.getAllHonorRollList();
      }
    })
  }

  ngOnInit(): void {
    this.searchCtrl=new FormControl();
    this.getAllHonorRollList();
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 12);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 26);
    const permissionSubCategory = permissionCategory.permissionSubcategory.find( x => x.permissionSubcategoryId === 31);
    this.editPermission = permissionSubCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionSubCategory.rolePermission[0].canDelete;
    this.addPermission = permissionSubCategory.rolePermission[0].canAdd;
  }

  getPageEvent(event){
    if(this.sort.active!=undefined && this.sort.direction!=""){
      this.honorRollListModel.sortingModel.sortColumn=this.sort.active;
      this.honorRollListModel.sortingModel.sortDirection=this.sort.direction;
    }
    if(this.searchCtrl.value!=null && this.searchCtrl.value!=""){
      let filterParams=[
        {
         columnName:null,
         filterValue:this.searchCtrl.value,
         filterOption:3
        }
      ]
     Object.assign(this.honorRollListModel,{filterParams: filterParams});
    }
    this.honorRollListModel.pageNumber=event.pageIndex+1;
    this.honorRollListModel.pageSize=event.pageSize;
    this.getAllHonorRollList();
  }

  goToAdd(){
    this.dialog.open(EditHonorRollComponent, {
      data:{mod:0},
      width: '500px'
    }).afterClosed().subscribe((data)=>{
      if(data==='submited'){
        this.getAllHonorRollList()
      }
    })
  }
  goToEdit(element){
    this.dialog.open(EditHonorRollComponent, {
      data:{mod:1,element},
      width: '500px'
    }).afterClosed().subscribe((data)=>{
      if(data==='submited'){
        this.getAllHonorRollList()
      }
    })
  }

  toggleColumnVisibility(column, event) {
    event.stopPropagation();
    event.stopImmediatePropagation();
    column.visible = !column.visible;
  }

  get visibleColumns() {
    return this.columns.filter(column => column.visible).map(column => column.property);
  }
  deletedata(element){
    this.honorRollAddViewModel.honorRolls.honorRollId=element.honorRollId
    this.gradesService.deleteHonorRoll(this.honorRollAddViewModel).subscribe(
      (res:HonorRollAddViewModel)=>{
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
            this.snackbar.open('' + res._message, '', {
              duration: 10000
            });
            this.getAllHonorRollList()
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
          message: "You are about to delete "+element.honorRoll+"."}
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      if(dialogResult){
        this.deletedata(element);
      }
   });
  }

  getAllHonorRollList(){
    if(this.honorRollListModel.sortingModel?.sortColumn==""){
      this.honorRollListModel.sortingModel=null
    }
    this.gradesService.getAllHonorRollList(this.honorRollListModel).subscribe(
      (res:HonorRollListModel)=>{
        if (typeof (res) == 'undefined') {
          this.snackbar.open('' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else{
          if (res._failure) {
            if (res.honorRollList == null) {
              this.honorROllModList= new MatTableDataSource([]);
              this.snackbar.open( res._message, '', {
                duration: 10000
              });
            } else {
              this.honorROllModList= new MatTableDataSource([]);
            }
          }
          else{
            this.totalCount=res.totalCount;
            this.pageNumber = res.pageNumber;
            this.pageSize = res.pageSize;
            this.honorROllModList= new MatTableDataSource(res.honorRollList);
            this.honorRollListModel=new HonorRollListModel();
          }
        }
      }
    );
  }
  onFilterChange(value: string) {
    if (!this.honorRollListModel) {
      return;
    }
    value = value.trim();
    value = value.toLowerCase();
    this.honorROllModList.filter = value;
  }

  translateKey(key) {
    let trnaslateKey;
    this.translateService.get(key).subscribe((res: string) => {
       trnaslateKey = res;
    });
    return trnaslateKey;
  }

  exportToExcel(){
    if (this.honorROllModList.data?.length > 0) {
      let reportList = this.honorROllModList.data?.map((x) => {
        return {
          [this.translateKey('honorRoll')]: x.honorRoll,
          [this.translateKey('breakoff')]: x.breakoff
        }
      });
      this.excelService.exportAsExcelFile(reportList,"Honor_Roll_List_")
    } else {
      this.snackbar.open('No records found. failed to export honor roll list', '', {
        duration: 5000
      });
    }
  }
}
