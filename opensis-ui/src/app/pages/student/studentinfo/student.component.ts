import { Component, Input, OnInit,Output,EventEmitter, ViewChild, OnDestroy, OnChanges, SimpleChanges } from '@angular/core';
import icMoreVert from '@iconify/icons-ic/twotone-more-vert';
import icAdd from '@iconify/icons-ic/baseline-add';
import icSearch from '@iconify/icons-ic/search';
import icFilterList from '@iconify/icons-ic/filter-list';
import { StudentService } from '../../../services/student.service';
import { StudentListModel} from '../../../models/studentModel';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router} from '@angular/router';
import { SelectionModel } from '@angular/cdk/collections';
import { fadeInUp400ms } from '../../../../@vex/animations/fade-in-up.animation';
import { fadeInRight400ms } from '../../../../@vex/animations/fade-in-right.animation';
import { stagger40ms } from '../../../../@vex/animations/stagger.animation';
import { MatPaginator} from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { LoaderService } from '../../../services/loader.service';
import { FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged, takeUntil } from 'rxjs/operators';
import { ImageCropperService } from '../../../services/image-cropper.service';
import { LayoutService } from 'src/@vex/services/layout.service';
import { ExcelService } from '../../../services/excel.service';
import icImpersonate from '@iconify/icons-ic/twotone-account-circle';
import { TranslateService } from '@ngx-translate/core';
import { Subject } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { SaveFilterComponent } from './save-filter/save-filter.component';
import { ModuleIdentifier } from '../../../enums/module-identifier.enum';
import { SchoolCreate } from '../../../enums/school-create.enum';

@Component({
  selector: 'vex-student',
  templateUrl: './student.component.html',
  styleUrls: ['./student.component.scss'],
  animations: [
    fadeInUp400ms,
    stagger40ms,
    fadeInRight400ms
  ]
})
export class StudentComponent implements OnInit,OnDestroy { 
  columns = [
    { label: 'Name', property: 'firstGivenName', type: 'text', visible: true },
    { label: 'Student ID', property: 'studentId', type: 'text', visible: true },
    { label: 'Alternate ID', property: 'alternateId', type: 'text', visible: true }, 
    { label: 'Grade Level', property: 'gradeLevelTitle', type: 'text', visible: true },    
    { label: 'Email', property: 'schoolEmail', type: 'text', visible: true },    
    { label: 'Telephone', property: 'homePhone', type: 'text', visible: true },
    { label: 'Action', property: 'action', type: 'text', visible: true }
  ];
  icImpersonate = icImpersonate;
  selection = new SelectionModel<any>(true, []);
  totalCount:number=0;
  pageNumber:number;
  pageSize:number;
  searchCtrl: FormControl;
  icMoreVert = icMoreVert;
  icAdd = icAdd;
  icSearch = icSearch;
  icFilterList = icFilterList;
  fapluscircle = "fa-plus-circle";
  tenant = "";
  loading:boolean;
  allStudentList=[];
  destroySubject$: Subject<void> = new Subject();
  getAllStudent: StudentListModel = new StudentListModel(); 
  StudentModelList: MatTableDataSource<any>;
  showAdvanceSearchPanel: boolean = false;
  
  moduleIdentifier=ModuleIdentifier;
  createMode=SchoolCreate;
  
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator; 
  @ViewChild(MatSort) sort:MatSort

  constructor(
    private studentService: StudentService,
    private snackbar: MatSnackBar,
    private router: Router,
    private loaderService:LoaderService,
    private imageCropperService:ImageCropperService,
    private layoutService: LayoutService,
    private excelService: ExcelService,
    public translateService: TranslateService,
    private dialog: MatDialog
  ) {
    translateService.use('en');
     this.getAllStudent.filterParams=null;
     if(localStorage.getItem("collapseValue") !== null){
      if( localStorage.getItem("collapseValue") === "false"){
        this.layoutService.expandSidenav();
      }else{
        this.layoutService.collapseSidenav();
      } 
    }else{
      this.layoutService.expandSidenav();
    }
     this.loaderService.isLoading.pipe(takeUntil(this.destroySubject$)).subscribe((val) => {
        this.loading = val;
      });
      this.callAllStudent();
    }

  ngOnInit(): void {
    this.searchCtrl = new FormControl();
  }

  getSearchResult(res){
    this.totalCount= res.totalCount;
    this.pageNumber = res.pageNumber;
    this.pageSize = res.pageSize;
    this.StudentModelList = new MatTableDataSource(res.studentMaster); 
    this.getAllStudent=new StudentListModel();
  }

  ngAfterViewInit() {
    //  Sorting
    this.getAllStudent=new StudentListModel();
    this.sort.sortChange.subscribe((res) => {
      this.getAllStudent.pageNumber=this.pageNumber
      this.getAllStudent.pageSize=this.pageSize;
      this.getAllStudent.sortingModel.sortColumn=res.active;
      if(this.searchCtrl.value!=null && this.searchCtrl.value!=""){
        let filterParams=[
          {
            columnName:null,
            filterValue:this.searchCtrl.value,
            filterOption:3
          }
        ]
        Object.assign(this.getAllStudent,{filterParams: filterParams});
      }
      if(res.direction==""){
        this.getAllStudent.sortingModel=null;
        this.callAllStudent();
        this.getAllStudent=new StudentListModel();
        this.getAllStudent.sortingModel=null;
      }else{
        this.getAllStudent.sortingModel.sortDirection=res.direction;
        this.callAllStudent();
      }
    });
      //  Searching
    this.searchCtrl.valueChanges.pipe(debounceTime(500),distinctUntilChanged()).subscribe((term)=>{
      if(term!='')
      {
          let filterParams=[
          {
            columnName:null,
            filterValue:term,
            filterOption:3
          }
        ]
        if(this.sort.active!=undefined && this.sort.direction!=""){
          this.getAllStudent.sortingModel.sortColumn=this.sort.active;
          this.getAllStudent.sortingModel.sortDirection=this.sort.direction;
        }
        Object.assign(this.getAllStudent,{filterParams: filterParams});
        this.getAllStudent.pageNumber=1;
        this.paginator.pageIndex=0;
        this.getAllStudent.pageSize=this.pageSize;
        this.callAllStudent();
        }
        else
        {
        Object.assign(this.getAllStudent,{filterParams: null});
          this.getAllStudent.pageNumber=this.paginator.pageIndex+1;
          this.getAllStudent.pageSize=this.pageSize;
          if(this.sort.active!=undefined && this.sort.direction!=""){
            this.getAllStudent.sortingModel.sortColumn=this.sort.active;
            this.getAllStudent.sortingModel.sortDirection=this.sort.direction;
          }
          this.callAllStudent();
        }
      })
    }

  goToAdd(){   
    this.router.navigate(["school/students/student-generalinfo"]);
    this.imageCropperService.enableUpload({module:this.moduleIdentifier.STUDENT,upload:true,mode:this.createMode.ADD});
  }

  saveFilter(){
    this.dialog.open(SaveFilterComponent, {
      width: '500px'
    })
  }

  viewStudentDetails(id){  
    this.imageCropperService.enableUpload({module:this.moduleIdentifier.STUDENT,upload:true,mode:this.createMode.VIEW});
    this.studentService.setStudentId(id)
    this.router.navigate(["school/students/student-generalinfo"]); 
  }

  getPageEvent(event){
    if(this.sort.active!=undefined && this.sort.direction!=""){
      this.getAllStudent.sortingModel.sortColumn=this.sort.active;
      this.getAllStudent.sortingModel.sortDirection=this.sort.direction;
    }
    if(this.searchCtrl.value!=null && this.searchCtrl.value!=""){
      let filterParams=[
        {
         columnName:null,
         filterValue:this.searchCtrl.value,
         filterOption:3
        }
      ]
     Object.assign(this.getAllStudent,{filterParams: filterParams});
    }
    this.getAllStudent.pageNumber=event.pageIndex+1;
    this.getAllStudent.pageSize=event.pageSize;
    this.callAllStudent();
  }

  callAllStudent(){
    if(this.getAllStudent.sortingModel?.sortColumn==""){
      this.getAllStudent.sortingModel=null
    }
    this.studentService.GetAllStudentList(this.getAllStudent).subscribe(data => {
      if(data._failure){
        if(data._message==="NO RECORD FOUND"){
            this.StudentModelList = new MatTableDataSource([]);   
        } else{
          this.snackbar.open('Student List failed. ' + data._message, 'LOL THANKS', {
            duration: 10000
          });
        }
      }else{
        this.totalCount= data.totalCount;
        this.pageNumber = data.pageNumber;
        this.pageSize = data._pageSize;
        this.StudentModelList = new MatTableDataSource(data.studentMaster);      
        this.getAllStudent=new StudentListModel();     
      }
    });
  }

  exportStudentListToExcel(){
    let getAllStudent: StudentListModel = new StudentListModel();
    getAllStudent.pageNumber=0;
    getAllStudent.pageSize=0;
    getAllStudent.sortingModel=null;
      this.studentService.GetAllStudentList(getAllStudent).subscribe(res => {
        if(res._failure){
          this.snackbar.open('Failed to Export School List.'+ res._message, 'LOL THANKS', {
          duration: 10000
          });
        }else{
          if(res.studentMaster.length>0){
            let studentList;
            studentList=res.studentMaster?.map((x)=>{
             let middleName=x.middleName==null?' ':' '+x.middleName+' ';
               return {
                Name: x.firstGivenName+middleName+x.lastFamilyName,
                'Student Id': x.studentInternalId,
                'Alternative Id': x.alternateId,
                'Grade Level':x.studentEnrollment[0]?.gradeLevelTitle,
                Email:x.schoolEmail,
                Telephone: x.mobilePhone
              }
            });
            this.excelService.exportAsExcelFile(studentList,'Students_List_')
          }else{
            this.snackbar.open('No Records Found. Failed to Export Students List','LOL THANKS', {
              duration: 5000
            });
          }
        }
      });
  }

  showAdvanceSearch() {
    this.showAdvanceSearchPanel = true;
  }

  hideAdvanceSearch(event){
    this.showAdvanceSearchPanel = false;
  }


  get visibleColumns() {
    return this.columns.filter(column => column.visible).map(column => column.property);
  }

  onFilterChange(value: string) {
    if (!this.StudentModelList) {
      return;
    }
    value = value.trim();
    value = value.toLowerCase();
    this.StudentModelList.filter = value;
  }

  toggleColumnVisibility(column, event) {
    event.stopPropagation();
    event.stopImmediatePropagation();
    column.visible = !column.visible;
  }

  ngOnDestroy(){
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }

}