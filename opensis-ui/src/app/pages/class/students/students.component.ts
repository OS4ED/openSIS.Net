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

import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import icSearch from '@iconify/icons-ic/search';
import { ScheduleStudentListViewModel } from 'src/app/models/student-schedule.model';
import { StudentScheduleService } from 'src/app/services/student-schedule.service';
import { MatTableDataSource } from '@angular/material/table';

import { fadeInUp400ms } from '../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../@vex/animations/stagger.animation';
import { LoaderService } from 'src/app/services/loader.service';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { FormControl } from '@angular/forms';
import { DefaultValuesService } from '../../../common/default-values.service';
import { ExcelService } from '../../../services/excel.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { debounceTime, distinctUntilChanged, takeUntil, filter } from 'rxjs/operators';

@Component({
  selector: 'vex-students',
  templateUrl: './students.component.html',
  styleUrls: ['./students.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class StudentsComponent implements OnInit, AfterViewInit {
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  constructor(
    public translateService: TranslateService,
    private loaderService: LoaderService,
    private excelService: ExcelService,
    private snackbar: MatSnackBar,
    private defaultValuesService: DefaultValuesService,
    private studentScheduleService: StudentScheduleService ) {
    translateService.use('en');
    this.loaderService.isLoading.subscribe((val) => {
      this.loading = val;
    });
  }


  icSearch = icSearch;
  totalCount = 0;
  pageNumber: number;
  pageSize: number;
  loading: boolean;
  searchCtrl: FormControl;
  getAllStudent: ScheduleStudentListViewModel = new ScheduleStudentListViewModel();
  allStudentlist: MatTableDataSource<any> = new MatTableDataSource<any>();
  columns = [
    { label: 'Student Name',  property: 'studentName',  type: 'text', visible: true },
    { label: 'Student Id',    property: 'studentInternalId',    type: 'text', visible: true },
    { label: 'alternateID',   property: 'alternateId',  type: 'text', visible: true },
    { label: 'gradeLevel',    property: 'gradeLevel',   type: 'text', visible: true },
    { label: 'section',       property: 'section',      type: 'text', visible: true },
    { label: 'phone',         property: 'phoneNumber',        type: 'text', visible: true }
  ];

  ngOnInit(): void {
    this.searchCtrl = new FormControl();
    this.searchScheduledStudentForGroupDrop();
  }
  ngAfterViewInit() {
    this.getAllStudent = new ScheduleStudentListViewModel();
    this.sort.sortChange.subscribe(
      (res) => {
        this.getAllStudent = new ScheduleStudentListViewModel();
        this.getAllStudent.pageNumber = this.pageNumber;
        this.getAllStudent.pageSize = this.pageSize;
        this.getAllStudent.sortingModel.sortColumn = res.active;
        if (this.searchCtrl.value != null && this.searchCtrl.value !== ''){
          const filterParams = [
            {
             columnName: null,
             filterValue: this.searchCtrl.value,
             filterOption: 1
            }
          ];
          Object.assign(this.getAllStudent, {filterParams});
        }
        if (res.direction === ''){
          this.getAllStudent.sortingModel = null;
          this.searchScheduledStudentForGroupDrop();
          this.getAllStudent = new ScheduleStudentListViewModel();
          this.getAllStudent.sortingModel = null;
        }
        else{
          this.getAllStudent.sortingModel.sortDirection = res.direction;
          this.searchScheduledStudentForGroupDrop();
        }
      }
    );
    //  Searching
    this.searchCtrl.valueChanges.pipe(debounceTime(500), distinctUntilChanged()).subscribe((term) => {
      if (term !== '') {
        this.callWithFilterValue(term);
      } else {
        this.callWithoutFilterValue();
      }
    });
  }
  callWithFilterValue(term) {
    const searchValue: string = term.toString();
    const filterParams = [
      {
        columnName: null,
        filterValue: searchValue.trim(),
        filterOption: 1
      }
    ];
    if (this.sort.active !== undefined && this.sort.direction !== '') {
      this.getAllStudent.sortingModel.sortColumn = this.sort.active;
      this.getAllStudent.sortingModel.sortDirection = this.sort.direction;
    }
    Object.assign(this.getAllStudent, { filterParams });
    this.getAllStudent.pageNumber = 1;
    this.paginator.pageIndex = 0;
    this.getAllStudent.pageSize = this.pageSize;
    this.searchScheduledStudentForGroupDrop();
  }

  callWithoutFilterValue() {
    Object.assign(this.getAllStudent, { filterParams: null });
    this.getAllStudent.pageNumber = this.paginator.pageIndex + 1;
    this.getAllStudent.pageSize = this.pageSize;
    if (this.sort.active !== undefined && this.sort.direction !== '') {
      this.getAllStudent.sortingModel.sortColumn = this.sort.active;
      this.getAllStudent.sortingModel.sortDirection = this.sort.direction;
    }
    this.searchScheduledStudentForGroupDrop();
  }

  searchScheduledStudentForGroupDrop(){
    this.getAllStudent.courseSectionId = +localStorage.getItem('courseSectionId');
    if (this.getAllStudent.sortingModel?.sortColumn === ''){
      this.getAllStudent.sortingModel = null;
    }
    this.studentScheduleService.searchScheduledStudentForGroupDrop(this.getAllStudent).subscribe(
      (res: ScheduleStudentListViewModel) => {
        if (res){
          if (res._failure){
            this.allStudentlist = new MatTableDataSource([]);
            this.totalCount = null;
          }else{
            this.totalCount = res.totalCount;
            this.pageNumber = res.pageNumber;
            this.pageSize = res._pageSize;
            this.allStudentlist = new MatTableDataSource(res.scheduleStudentForView);
          }
        }
        else {
        }
      }
    );
  }
  toggleColumnVisibility(column, event: Event) {
    event.stopPropagation();
    event.stopImmediatePropagation();
    column.visible = !column.visible;
  }

  get visibleColumns() {
    return this.columns.filter(column => column.visible).map(column => column.property);
  }
  getPageEvent(event){
    if (this.sort.active !== undefined && this.sort.direction !== '') {
      this.getAllStudent.sortingModel.sortColumn = this.sort.active;
      this.getAllStudent.sortingModel.sortDirection = this.sort.direction;
    }
    if (this.searchCtrl.value != null && this.searchCtrl.value.trim() !== '') {
      const filterParams = [
        {
          columnName: null,
          filterValue: this.searchCtrl.value,
          filterOption: 1
        }
      ];
      Object.assign(this.getAllStudent, { filterParams });
    }
    this.getAllStudent.pageNumber = event.pageIndex + 1;
    this.getAllStudent.pageSize = event.pageSize;
    this.searchScheduledStudentForGroupDrop();
  }
  exportToExcel(){
    if (this.allStudentlist.data?.length > 0) {
      const reportList = this.allStudentlist.data?.map((x) => {
        return {
          [this.defaultValuesService.translateKey('studentName')]: x.firstGivenName + ' ' + x.lastFamilyName,
          [this.defaultValuesService.translateKey('studentId')]: x.studentId,
          [this.defaultValuesService.translateKey('alternateId')]: x.alternateId,
          [this.defaultValuesService.translateKey('gradeLevel')]: x.gradeLevel,
          [this.defaultValuesService.translateKey('section')]: x.section,
          [this.defaultValuesService.translateKey('phoneNumber')]: x.phoneNumber
        };
      });
      this.excelService.exportAsExcelFile(reportList, 'Student_List_');
    } else {
      this.snackbar.open('No records found. failed to export Student List', '', {
        duration: 5000
      });
    }
  }

}
