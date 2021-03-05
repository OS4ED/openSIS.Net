import { Component, OnInit, ViewChild } from '@angular/core';
import icMoreVert from '@iconify/icons-ic/twotone-more-vert';
import icAdd from '@iconify/icons-ic/baseline-add';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icDelete from '@iconify/icons-ic/twotone-delete';
import icSearch from '@iconify/icons-ic/search';
import icFilterList from '@iconify/icons-ic/filter-list';
import icImpersonate from '@iconify/icons-ic/twotone-account-circle';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { fadeInUp400ms } from '../../../../@vex/animations/fade-in-up.animation';
import { stagger40ms } from '../../../../@vex/animations/stagger.animation';
import { TranslateService } from '@ngx-translate/core';
import { ParentInfoService } from '../../../services/parent-info.service';
import { GetAllParentModel } from "../../../models/parentInfoModel";
import { LoaderService } from '../../../services/loader.service';
import { MatTableDataSource } from '@angular/material/table';
import { StudentService } from '../../../services/student.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { LayoutService } from 'src/@vex/services/layout.service';
import { ExcelService } from '../../../services/excel.service';

@Component({
  selector: 'vex-parentinfo',
  templateUrl: './parentinfo.component.html',
  styleUrls: ['./parentinfo.component.scss'],
  animations: [
    fadeInUp400ms,
    stagger40ms
  ]
})
export class ParentinfoComponent implements OnInit {
  columns = [
    { label: 'Parent Name', property: 'name', type: 'text', visible: true },
    { label: 'Profile', property: 'profile', type: 'text', visible: true },
    { label: 'Email Address', property: 'email', type: 'text', visible: true },
    { label: 'Mobile Phone', property: 'mobile', type: 'number', visible: true },
    { label: 'Associated Students', property: 'students', type: 'text', visible: true },
    { label: 'Action', property: 'action', type: 'text', visible: true }
  ];
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort

  searchKey: string;

  icMoreVert = icMoreVert;
  icAdd = icAdd;
  icEdit = icEdit;
  icDelete = icDelete;
  icSearch = icSearch;
  icFilterList = icFilterList;
  icImpersonate = icImpersonate;
  loading: boolean;
  getAllParentModel: GetAllParentModel = new GetAllParentModel()
  parentFieldsModelList: MatTableDataSource<any>;
  allParentList=[];
  parentListForExcel=[];
  constructor(
    private router: Router,
    private parentInfoService: ParentInfoService,
    private snackbar: MatSnackBar,
    private loaderService: LoaderService,
    public translateService: TranslateService,
    private studentService: StudentService,
    private layoutService: LayoutService,
    private excelService:ExcelService
  ) {
    if(localStorage.getItem("collapseValue") !== null){
      if( localStorage.getItem("collapseValue") === "false"){
        this.layoutService.expandSidenav();
      }else{
        this.layoutService.collapseSidenav();
      } 
    }else{
      this.layoutService.expandSidenav();
    }
    translateService.use('en');
    this.loaderService.isLoading.subscribe((val) => {
      this.loading = val;
    });
    this.getAllparentList();


  }

  ngOnInit(): void {

  }

  goToStudentInformation(studentId) {
    this.studentService.setStudentId(studentId)
    this.router.navigateByUrl('/school/students/student-generalinfo');
  }

  toggleColumnVisibility(column, event) {
    event.stopPropagation();
    event.stopImmediatePropagation();
    column.visible = !column.visible;
  }

  get visibleColumns() {
    return this.columns.filter(column => column.visible).map(column => column.property);
  }

  viewGeneralInfo(parentInfo) {
    this.parentInfoService.setParentId(parentInfo.parentId);
    this.parentInfoService.setParentDetails(parentInfo)
    this.router.navigateByUrl('/school/parents/parent-generalinfo');
  }

  onSearchClear() {
    this.searchKey = "";
    this.applyFilter();
  }

  applyFilter() {
    this.parentFieldsModelList.filter = this.searchKey.trim().toLowerCase()


  }

  getAllparentList() {
    this.parentInfoService.getAllParentInfo(this.getAllParentModel).subscribe(
      (res: GetAllParentModel) => {
        if (typeof (res) == 'undefined') {
          this.snackbar.open('Parent list failed. ' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else {
          if (res._failure) {
            if (res._message === "NO RECORD FOUND") {
                this.parentFieldsModelList = new MatTableDataSource([]);
                this.parentFieldsModelList.sort = this.sort;
                this.parentFieldsModelList.paginator = this.paginator;  
            } else {
              this.snackbar.open('Parent list failed.' + res._message, '', {
                duration: 10000
              });
            }
          }
          else {
            let parentList = res.parentInfoForView?.map(function (item) {
              return {
                parentId: item.parentId,
                name: item.firstname + ' ' + item.lastname,
                profile: item.userProfile,
                email: item.workEmail,
                mobile: item.mobile,
                students: item.students
              };
            });
            this.allParentList=parentList;
            this.parentListForExcel=res.parentInfoForView;
            this.parentFieldsModelList = new MatTableDataSource(parentList);
            this.parentFieldsModelList.sort = this.sort;
            this.parentFieldsModelList.paginator = this.paginator;
          }
        }
      }
    )
  }

  exportParentListToExcel(){
    if(this.parentListForExcel.length!=0){
   let parentList=this.parentListForExcel?.map((item)=>{
     let students=item.students?.map((student)=>{
       return student.split('|')[0];
     });
     return{
                ParentsName: item.firstname + ' ' + item.lastname,
                Profile: item.userProfile,
                EmailAddress: item.workEmail,
                MobilePhone: item.mobile,
                AssociatedStudents: students==undefined?'':students.toString()
     }
   });
   this.excelService.exportAsExcelFile(parentList, 'Parents_List_');
  }else{
    this.snackbar.open('No Records Found. Failed to Export Parent List', '', {
      duration: 5000
    });
  }
}


}
