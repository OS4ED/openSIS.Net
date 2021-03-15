import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
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
import { fadeInRight400ms } from 'src/@vex/animations/fade-in-right.animation';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'vex-parentinfo',
  templateUrl: './parentinfo.component.html',
  styleUrls: ['./parentinfo.component.scss'],
  animations: [
    fadeInUp400ms,
    stagger40ms,
    fadeInRight400ms
  ]
})
export class ParentinfoComponent implements OnInit {
  columns = [
    { label: 'Parent Name', property: 'name', type: 'text', visible: true },
    { label: 'Profile', property: 'userProfile', type: 'text', visible: true },
    { label: 'Email Address', property: 'workEmail', type: 'text', visible: true },
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
  getAllParentModel: GetAllParentModel = new GetAllParentModel();
  parentFieldsModelList: MatTableDataSource<any>;
  allParentList = [];
  parentListForExcel = [];
  showAdvanceSearchPanel: boolean = false;
  constructor(
    private router: Router,
    private parentInfoService: ParentInfoService,
    private snackbar: MatSnackBar,
    private loaderService: LoaderService,
    public translateService: TranslateService,
    private studentService: StudentService,
    private layoutService: LayoutService,
    private excelService: ExcelService,
    private cdr: ChangeDetectorRef
  ) {
    if (localStorage.getItem("collapseValue") !== null) {
      if (localStorage.getItem("collapseValue") === "false") {
        this.layoutService.expandSidenav();
      } else {
        this.layoutService.collapseSidenav();
      }
    } else {
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
    this.parentFieldsModelList.filter = JSON.stringify(this.searchKey.trim());


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
              if(res.parentInfoForView === null){
                this.parentFieldsModelList = new MatTableDataSource([]);
                this.parentFieldsModelList.sort = this.sort;
                this.parentFieldsModelList.paginator = this.paginator;
                this.snackbar.open( res._message, '', {
                  duration: 10000
                });
              }
              else{
                this.parentFieldsModelList = new MatTableDataSource([]);
                this.parentFieldsModelList.sort = this.sort;
                this.parentFieldsModelList.paginator = this.paginator;
              }
              
          }
          else {
          
            let parentList=res.parentInfoForView.map((item,i)=>{
              item.studentFirstName=item.students.map((item)=>{
                return item.split('|')[0];
              });
              item.studentMiddleName=item.students.map((item)=>{
                return item.split('|')[1];
              });
              item.studentLastName=item.students.map((item)=>{
                return item.split('|')[2];
              });
              item.studentFullName=item.students.map((item)=>{
                return item.split('|')[0]+' '+item.split('|')[2];
              });
              return {
                parentId: item.parentId,
                name: item.firstname + ' ' + item.lastname,
                userProfile: item.userProfile==null?'':item.userProfile,
                workEmail: item.workEmail==null?'':item.workEmail,
                mobile: item.mobile==null?'':item.mobile,
                students: item.students,
                firstname:item.firstname==null?'':item.firstname,
                middlename:item.middlename==null?'':item.middlename,
                lastname:item.lastname==null?'':item.lastname,
                personalEmail:item.personalEmail==null?'':item.personalEmail,
                homePhone:item.homePhone==null?'':item.homePhone,
                workPhone:item.workPhone==null?'':item.workPhone,
                studentFirstName:item.studentFirstName,
                studentMiddleName:item.studentMiddleName,
                studentLastName:item.studentLastName,
                studentFullName:item.studentFullName,
                addressLineOne:item.addressLineOne==null?'':item.addressLineOne,
                addressLineTwo:item.addressLineTwo==null?'':item.addressLineTwo,
                country:item.country==null?'':item.country,
                state:item.state==null?'':item.state,
                city:item.city==null?'':item.city,
                zip:item.zip==null?'':item.zip
              };
            })
            this.allParentList = parentList;
            this.parentListForExcel = res.parentInfoForView;
            this.parentFieldsModelList = new MatTableDataSource(parentList);
            this.parentFieldsModelList.sort = this.sort;
            this.parentFieldsModelList.paginator = this.paginator;
            this.parentFieldsModelList.filterPredicate = this.createFilter();
          }
        }
      }
    )
  }

  exportParentListToExcel() {
    if (this.parentListForExcel.length != 0) {
      let parentList = this.parentListForExcel?.map((item) => {
        let students = item.students?.map((student) => {
          return student.split('|')[0];
        });
        return {
          ParentsName: item.firstname + ' ' + item.lastname,
          Profile: item.userProfile,
          EmailAddress: item.workEmail,
          MobilePhone: item.mobile,
          AssociatedStudents: students == undefined ? '' : students.toString()
        }
      });
      this.excelService.exportAsExcelFile(parentList, 'Parents_List_');
    } else {
      this.snackbar.open('No Records Found. Failed to Export Parent List', '', {
        duration: 5000
      });
    }
  }

  getSearchResult(res) {
    this.parentFieldsModelList.filter = JSON.stringify(res);
   
    if(this.parentFieldsModelList.filteredData.length>0){
      this.showAdvanceSearchPanel=false;
    }
  }

  createFilter(): (data: any, filter: string) => boolean {

    let filterFunction = function(data, filter): boolean {
      if(typeof JSON.parse(filter)!='string' ){
        let searchTerms = JSON.parse(filter);
        return (
          data.firstname.toLowerCase().indexOf(searchTerms.firstname) !== -1 
          &&
          data.middlename.toLowerCase()
            .indexOf(searchTerms.middlename) !== -1 
            &&
          data.lastname?.toLowerCase().indexOf(searchTerms.lastname) !== -1
           &&
           data.personalEmail?.toLowerCase().indexOf(searchTerms.personalEmail) !== -1
           &&
           data.workEmail?.toLowerCase().indexOf(searchTerms.workEmail) !== -1
           &&
           data.homePhone?.toLowerCase().indexOf(searchTerms.homePhone) !== -1
           &&
           data.mobile?.toLowerCase().indexOf(searchTerms.mobile) !== -1
           &&
           data.workPhone?.toLowerCase().indexOf(searchTerms.workPhone) !== -1
           &&
          data.studentFirstName.join(',')
            .toLowerCase()
            .indexOf(searchTerms.studentFirstName) !== -1
             &&
              data.studentMiddleName.join(',')
            .toLowerCase()
            .indexOf(searchTerms.studentMiddleName) !== -1 
            &&
              data.studentLastName.join(',')
            .toLowerCase()
            .indexOf(searchTerms.studentLastName) !== -1
            &&
            data.addressLineOne?.toLowerCase().indexOf(searchTerms.addressLineOne) !== -1
            &&
            data.addressLineTwo?.toLowerCase().indexOf(searchTerms.addressLineTwo) !== -1
            &&
            data.country?.toLowerCase().indexOf(searchTerms.country) !== -1
            &&
            data.state?.toLowerCase().indexOf(searchTerms.state) !== -1
            &&
            data.city?.toLowerCase().indexOf(searchTerms.city) !== -1
            &&
            data.zip?.toLowerCase().indexOf(searchTerms.zip) !== -1
        );
      }else{
        filter=JSON.parse(filter);
        return (
          data.name?.toLowerCase().includes(filter)
          ||
           data.userProfile?.toLowerCase().includes(filter)
           ||
           data.workEmail?.toLowerCase().includes(filter)
          ||
           data.mobile?.toLowerCase().includes(filter)
           ||
          data.studentFullName.join(',')
            .toLowerCase()
            .indexOf(filter) !== -1
        );
      }
      
    };

    return filterFunction;
  }

  showAdvanceSearch() {
    this.showAdvanceSearchPanel = true;
    this.cdr.detectChanges();
  }

  hideAdvanceSearch(event){
    this.showAdvanceSearchPanel = false;
  }


}
