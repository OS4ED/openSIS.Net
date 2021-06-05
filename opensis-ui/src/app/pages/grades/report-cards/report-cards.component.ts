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

import { Component, OnInit, ViewChild } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { Router } from "@angular/router";
import { TranslateService } from "@ngx-translate/core";
import icSearch from '@iconify/icons-ic/search';
import { StudentDetails } from '../../../models/student-details.model';
import { StudentListModel } from "../../../../app/models/student.model";
import { StudentService } from "../../../../app/services/student.service";
import { MatTableDataSource } from "@angular/material/table";
import { MatSnackBar } from "@angular/material/snack-bar";
import { MarkingPeriodService } from "../../../../app/services/marking-period.service";
import { GetMarkingPeriodTitleListModel } from "src/app/models/marking-period.model";
import { AddReportCardPdf } from "../../../../app/models/report-card.model";
import { ReportCardService } from "../../../../app/services/report-card.service";
import { MatCheckbox } from "@angular/material/checkbox";
import { SearchFilter, SearchFilterAddViewModel, SearchFilterListViewModel } from "src/app/models/search-filter.model";
import { CommonService } from "../../../../app/services/common.service";
import { ConfirmDialogComponent } from "../../shared-module/confirm-dialog/confirm-dialog.component";
import { fadeInUp400ms } from "../../../../@vex/animations/fade-in-up.animation";
import { stagger40ms } from "../../../../@vex/animations/stagger.animation";
import { fadeInRight400ms } from "../../../../@vex/animations/fade-in-right.animation";
import { FormControl } from "@angular/forms";
import { MatSort } from "@angular/material/sort";
import { MatPaginator } from "@angular/material/paginator";
import { LoaderService } from "../../../../app/services/loader.service";
import { debounceTime, distinctUntilChanged } from "rxjs/operators";
import { DefaultValuesService } from "../../../../app/common/default-values.service";

@Component({
  selector: "vex-report-cards",
  templateUrl: "./report-cards.component.html",
  styleUrls: ["./report-cards.component.scss"],
  animations: [
    fadeInUp400ms,
    stagger40ms,
    fadeInRight400ms
  ]
})
export class ReportCardsComponent implements OnInit {

  icSearch = icSearch;
  getAllStudent: StudentListModel = new StudentListModel();
  totalCount:number=0;
  StudentModelList: MatTableDataSource<any>;
  pageNumber:number;
  pageSize:number;
  getMarkingPeriodTitleListModel: GetMarkingPeriodTitleListModel = new GetMarkingPeriodTitleListModel();
  markingPeriodList = [];
  markingPeriods;
  addReportCardPdf: AddReportCardPdf =  new AddReportCardPdf();
  pdfData;
  listOfStudent = [];
  @ViewChild('masterCheckBox') private masterCheckBox: MatCheckbox;
  showAdvanceSearchPanel: boolean = false;
  filterJsonParams: any;
  searchFilter: any;
  showSaveFilter: boolean;
  showLoadFilter: boolean;
  searchCount: any;
  searchFilterAddViewModel: SearchFilterAddViewModel = new SearchFilterAddViewModel();
  searchFilterListViewModel: SearchFilterListViewModel = new SearchFilterListViewModel();
  toggleValues: any;
  searchValue: any;
  searchCtrl = new FormControl();
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator; 
  markingPeriodError: boolean;
  loading: boolean;
  displayedColumns: string[] = ['studentSelected', 'studentName', 'studentId', 'alternateId', 'gradeLevelTitle', 'section','homePhone'];

  constructor(
    private router: Router,
    private dialog: MatDialog,
    public translateService: TranslateService,
    private studentService: StudentService,
    private snackbar: MatSnackBar,
    private markingPeriodService: MarkingPeriodService,
    private reportCardService: ReportCardService,
    private commonService: CommonService,
    private loaderService: LoaderService,
    private defaultValuesService: DefaultValuesService,
  ) {
    translateService.use("en");
    this.markingPeriods = [];
  }


  ngOnInit(): void {
    this.getAllStudent.pageSize = this.defaultValuesService.getPageSize() ? this.defaultValuesService.getPageSize() : 10;

    this.getAllMarkingPeriodList();
    this.getAllStudentList();

    this.loaderService.isLoading.subscribe((val) => {
      this.loading = val;
    });

    this.searchCtrl.valueChanges.pipe(debounceTime(500),distinctUntilChanged()).subscribe((term)=>{
      if(term.trim().length > 0)
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
          this.getAllStudentList();
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
        this.getAllStudentList();
        }
      })

  }

  getAllStudentList(){
    if(this.getAllStudent.sortingModel?.sortColumn==""){
      this.getAllStudent.sortingModel = null;
    }
    this.studentService.GetAllStudentList(this.getAllStudent).subscribe(data => {
      if (data._failure){
        if (data.studentMaster === null){
          this.totalCount = null;
          this.listOfStudent = [];
          this.StudentModelList = new MatTableDataSource(this.listOfStudent);
          this.snackbar.open( data._message, '', {
              duration: 10000
            });
        } else{
          this.listOfStudent = [];
          this.StudentModelList = new MatTableDataSource(this.listOfStudent);
          this.totalCount = null;
        }
      }else{
        this.totalCount = data.totalCount;
        this.pageNumber = data.pageNumber;
        this.pageSize = data._pageSize;
        this.listOfStudent = data.studentMaster;
        this.StudentModelList = new MatTableDataSource(this.listOfStudent);
        this.getAllStudent = new StudentListModel();
      }
    });
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
    this.defaultValuesService.setPageSize(event.pageSize);
    this.getAllStudentList();
  }


  resetStudentList(){
    this.searchCount = null;
    this.searchValue = null;
    this.getAllStudentList();
  }

  markingPeriodChecked(event, markingPeriod) {
    if(event.checked) {
      this.markingPeriods.push(markingPeriod.value);
    } else {
      this.markingPeriods.splice(this.markingPeriods.findIndex(x => x === markingPeriod.value), 1);
    }
    this.markingPeriodError = this.markingPeriods.length > 0 ? false : true;
  }

  selectedStudent(studentId, event) {
    if(event.checked) {
    this.addReportCardPdf.studentsReportCardViewModelList.push({studentId});
    this.listOfStudent.map((item)=>{
        if(item.studentId === studentId){
          item.checked = true;
        }
    });
    } else {
      this.addReportCardPdf.studentsReportCardViewModelList.splice(this.addReportCardPdf.studentsReportCardViewModelList.findIndex(x => x.studentId === studentId), 1);
    }
    this.masterCheckBox.checked=this.listOfStudent.every((item)=>{
      return item.checked;
    });
  }

  getSearchResult(res){
    if (res.totalCount){
      this.searchCount = res.totalCount;
      this.totalCount = res.totalCount;
    }
    else{
      this.searchCount = 0;
      this.totalCount = 0;
    }
    this.showSaveFilter = true;
    this.pageNumber = res.pageNumber;
    this.pageSize = res._pageSize;
    this.StudentModelList = new MatTableDataSource(res.studentMaster); 
    this.getAllStudent = new StudentListModel();
  }

  getAllSearchFilter(){
    this.searchFilterListViewModel.module='Student';
    this.commonService.getAllSearchFilter(this.searchFilterListViewModel).subscribe((res) => {
      if (typeof (res) === 'undefined') {
        this.snackbar.open('Filter list failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (res._failure) {
          this.searchFilterListViewModel.searchFilterList=[]
        }
        else {
          this.searchFilterListViewModel= res;
          let filterData=this.searchFilterListViewModel.searchFilterList.filter(x=> x.filterId == this.searchFilter.filterId);
          if(filterData.length >0){
            this.searchFilter.jsonList= filterData[0].jsonList;
          }
          if(this.filterJsonParams == null){
            this.searchFilter = this.searchFilterListViewModel.searchFilterList[this.searchFilterListViewModel.searchFilterList.length-1];
          }
        }
      }
    }
    );
  }

  hideAdvanceSearch(event){
    this.showSaveFilter = event.showSaveFilter;
    this.showAdvanceSearchPanel = false;
    if(event.showSaveFilter == false){
      this.getAllSearchFilter();
    }
  }

  getSearchInput(event){
    this.searchValue = event;
  }

  editFilter(){
    this.showAdvanceSearchPanel = true;
    this.filterJsonParams = this.searchFilter;
    this.showSaveFilter = false;
    this.showLoadFilter=false;
  }

  deleteFilter(){
   
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: '400px',
      data: {
          title: 'Are you sure?',
          message: 'You are about to delete ' + this.searchFilter.filterName + '.'}
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult){
        this.deleteFilterdata(this.searchFilter);
      }
   });
  }

  deleteFilterdata(filterData){
    this.searchFilterAddViewModel.searchFilter = filterData;
    this.commonService.deleteSearchFilter(this.searchFilterAddViewModel).subscribe(
      (res: SearchFilterAddViewModel) => {
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
            this.getAllSearchFilter();
            this.getAllStudent.filterParams= null;
            this.getAllStudentList();
            this.searchFilter = new SearchFilter();
            this.showLoadFilter=true;
          }
        }
      }
    );
  }

  getToggleValues(event){
    this.toggleValues = event;
    if (event.inactiveStudents === true){
      // this.columns[6].visible = true;
    }
    else if (event.inactiveStudents === false){
      // this.columns[6].visible = false;
    }
  }

  someComplete():boolean{
    let indetermine=false;
    for(let user of this.listOfStudent){
      for(let selectedUser of this.addReportCardPdf.studentsReportCardViewModelList){
        if(user.studentId === selectedUser.studentId){
          indetermine=true;
        }
      }
    }
    if(indetermine){
      this.masterCheckBox.checked=this.listOfStudent.every((item)=>{
        return item.checked;
      })
      if(this.masterCheckBox.checked){
        return false;
      }else{
        return true;
      }
    }
  }
  
    decideCheckUncheck(){
    this.listOfStudent.map((item)=>{
      let isIdIncludesInSelectedList=false;
      if(item.checked){
        for(let selectedUser of this.addReportCardPdf.studentsReportCardViewModelList){
          if(item.studentId==selectedUser.studentId){
            isIdIncludesInSelectedList=true;
            break;
           }
        }
        if(!isIdIncludesInSelectedList){
          this.addReportCardPdf.studentsReportCardViewModelList.push({studentId: item.studentId});
        }
      }else{
        for(let selectedUser of this.addReportCardPdf.studentsReportCardViewModelList){
          if(item.studentId==selectedUser.studentId){
            this.addReportCardPdf.studentsReportCardViewModelList =this.addReportCardPdf.studentsReportCardViewModelList.filter((user)=>user.studentId!=item.studentId);
            break;
           }
        }
      }
      isIdIncludesInSelectedList=false;
      
    });
  }

  setAll(event){
    this.listOfStudent.map(user => {
      user.checked = event;
      if(event) {
        this.addReportCardPdf.studentsReportCardViewModelList.push({studentId: user.studentId});
      } else {
        this.addReportCardPdf.studentsReportCardViewModelList = [];
      }
    });
    this.StudentModelList = new MatTableDataSource(this.listOfStudent);
  }

  doCheck(studentId) {
    return this.addReportCardPdf.studentsReportCardViewModelList.findIndex(x => x.studentId === studentId) === -1 ? false : true;
  }

  getAllMarkingPeriodList() {
    this.getMarkingPeriodTitleListModel.academicYear = +sessionStorage.getItem("academicyear");
    this.addReportCardPdf.academicYear = +sessionStorage.getItem("academicyear");

    this.markingPeriodService.getAllMarkingPeriodList(this.getMarkingPeriodTitleListModel).subscribe((res)=>{
      if (res._failure) {
          this.markingPeriodList = [];
          this.snackbar.open(res._message, '', {
            duration: 1000
          });
      } else {
        this.markingPeriodList = res.getMarkingPeriodView;
      }
    })
  }

  addAndGenerateReportCard() {
    return new Promise((resolve, reject)=>{
    this.addReportCardPdf.markingPeriods = this.markingPeriods.toString();

    this.reportCardService.addReportCard(this.addReportCardPdf).subscribe((res)=>{
      if (res._failure) {
        this.snackbar.open(res._message, '', {
          duration: 1000
        });
    } else {
      resolve('');
      // this.markingPeriods = [];
      // this.snackbar.open(res._message, '', {
      //   duration: 1000
      // });
    }
    })
  });
  }

  generateReportCard() {
    if(this.markingPeriods.length > 0) {
    this.addAndGenerateReportCard().then(()=>{
    this.reportCardService.generateReportCard(this.addReportCardPdf).subscribe((res)=>{
      if (res._failure) {
        this.snackbar.open(res._message, '', {
          duration: 1000
        });
    } else {
      // this.addReportCardPdf =  new AddReportCardPdf();
     this.pdfData = res;
    }
    });
  });
} else {
  this.markingPeriodError = true;
}
  }
}
