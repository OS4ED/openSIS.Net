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

import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import icSearch from "@iconify/icons-ic/search";
import { StudentAttendanceService } from "src/app/services/student-attendance.service";
import { ScheduledCourseSectionViewModel } from "src/app/models/dashboard.model";
import { GetAllStaffModel } from "src/app/models/staff.model";
import { MatSnackBar } from "@angular/material/snack-bar";
import { MatTableDataSource } from "@angular/material/table";
import { AllCourseSectionView } from "src/app/models/course-manager.model";
import { LoaderService } from "src/app/services/loader.service";
import { debounceTime, distinctUntilChanged, takeUntil } from "rxjs/operators";
import { Subject } from "rxjs";
import { Router } from "@angular/router";
import { fadeInUp400ms } from "src/@vex/animations/fade-in-up.animation";
import { stagger40ms } from "src/@vex/animations/stagger.animation";
import { fadeInRight400ms } from "src/@vex/animations/fade-in-right.animation";
import { MatPaginator } from "@angular/material/paginator";
import { FormControl } from "@angular/forms";

@Component({
  selector: "vex-missing-attendance-details",
  templateUrl: "./missing-attendance-details.component.html",
  styleUrls: ["./missing-attendance-details.component.scss"],
  animations: [
    fadeInUp400ms,
    stagger40ms,
    fadeInRight400ms
  ]
})
export class MissingAttendanceDetailsComponent implements OnInit ,AfterViewInit, OnDestroy{
  icSearch = icSearch;
  scheduledCourseSectionViewModel: ScheduledCourseSectionViewModel = new ScheduledCourseSectionViewModel();
  getAllStaffModel: GetAllStaffModel= new GetAllStaffModel();
  staffDetails;
  courseSectionViewList: MatTableDataSource<AllCourseSectionView>;
  loading: boolean;
  destroySubject$: Subject<void> = new Subject();
  totalCount: number = 0;
  pageSize: number;
  missingAttendanceDateList = [];
  searchKeyword: string;
  pageNumber: number;
  searchCtrl: FormControl;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  columns = [
    { label: 'date', property: 'date', type: 'text', visible: true },
    { label: 'staffName', property: 'staffName', type: 'text', visible: true },
    { label: 'courseSectionName', property: 'courseSectionName', type: 'text', visible: true },
    { label: 'period', property: 'period', type: 'text', visible: true },
    { label: 'action', property: 'action', type: 'text', visible: true },
    
  ];

  constructor(public translateService: TranslateService,
    private snackbar:MatSnackBar,
    private router:Router,
    private loaderService: LoaderService,
    private studentAttendanceService: StudentAttendanceService) {
    translateService.use("en");
    this.staffDetails = this.studentAttendanceService.getStaffDetails();
    this.loaderService.isLoading.pipe(takeUntil(this.destroySubject$)).subscribe((val) => {
      this.loading = val;
    });
  }

  ngOnInit(): void {
this.searchCtrl= new FormControl();
    this.getMissingAttendanceList();
  }


  ngAfterViewInit() {
    //  Sorting
     this.getAllStaffModel = new GetAllStaffModel();
    // this.sort.sortChange.subscribe((res) => {
    //   this.getAllStaffModel.pageNumber = this.pageNumber
    //   this.getAllStaffModel.pageSize = this.pageSize;
    //   this.getAllStaffModel.sortingModel.sortColumn = res.active;
    //   if (this.searchCtrl.value != null && this.searchCtrl.value != "") {
    //     let filterParams = [
    //       {
    //         columnName: null,
    //         filterValue: this.searchCtrl.value,
    //         filterOption: 4
    //       }
    //     ]
    //     Object.assign(this.getAllStaffModel, { filterParams: filterParams });
    //   }
    //   if (res.direction == "") {
    //     this.getAllStaffModel.sortingModel = null;
    //     this.getMissingAttendanceList();
    //     this.getAllStaffModel = new GetAllStaffModel();
    //     this.getAllStaffModel.sortingModel = null;
    //   } else {
    //     this.getAllStaffModel.sortingModel.sortDirection = res.direction;
    //     this.getMissingAttendanceList();
    //   }
    // });

    //Searching
    this.searchCtrl.valueChanges.pipe(debounceTime(500), distinctUntilChanged()).subscribe((term) => {
      if (term != '') {
        this.callWithFilterValue(term);
      } else {
        this.callWithoutFilterValue()
      }
    });
  }

  getPageEvent(event) {
    // if (this.sort.active != undefined && this.sort.direction != "") {
    //   this.getAllStaffModel.sortingModel.sortColumn = this.sort.active;
    //   this.getAllStaffModel.sortingModel.sortDirection = this.sort.direction;
    // }
    if (this.searchCtrl.value != null && this.searchCtrl.value != "") {
      let filterParams = [
        {
          columnName: null,
          filterValue: this.searchCtrl.value,
          filterOption: 3
        }
      ]
      Object.assign(this.getAllStaffModel, { filterParams: filterParams });
    }
    this.getAllStaffModel.pageNumber = event.pageIndex + 1;
    this.getAllStaffModel.pageSize = event.pageSize;
    this.getMissingAttendanceList();
  }


  callWithFilterValue(term) {
    let filterParams = [
      {
        columnName: null,
        filterValue: term,
        filterOption: 4
      }
    ]
    // if (this.sort.active != undefined && this.sort.direction != "") {
    //   this.getAllStaffModel.sortingModel.sortColumn = this.sort.active;
    //   this.getAllStaffModel.sortingModel.sortDirection = this.sort.direction;
    // }
    Object.assign(this.getAllStaffModel, { filterParams: filterParams });
    this.getAllStaffModel.pageNumber = 1;
    this.paginator.pageIndex = 0;
    this.getAllStaffModel.pageSize = this.pageSize;
    this.getMissingAttendanceList();
  }

  callWithoutFilterValue() {
    Object.assign(this.getAllStaffModel, { filterParams: null });
    this.getAllStaffModel.pageNumber = this.paginator.pageIndex + 1;
    this.getAllStaffModel.pageSize = this.pageSize;
    // if (this.sort.active != undefined && this.sort.direction != "") {
    //   this.getAllStaffModel.sortingModel.sortColumn = this.sort.active;
    //   this.getAllStaffModel.sortingModel.sortDirection = this.sort.direction;
    // }
    this.getMissingAttendanceList();
  }

  toggleColumnVisibility(column, event) {
    event.stopPropagation();
    event.stopImmediatePropagation();
    column.visible = !column.visible;
  }

  get visibleColumns() {
    return this.columns.filter(column => column.visible).map(column => column.property);
  }

  getMissingAttendanceList(){
    this.getAllStaffModel.sortingModel = null;
    this.getAllStaffModel.staffId= this.staffDetails.staffId;
    this.getAllStaffModel.dobStartDate = this.staffDetails.startDate;
    this.getAllStaffModel.dobEndDate = this.staffDetails.endDate;
    this.studentAttendanceService.missingAttendanceList(this.getAllStaffModel).subscribe((res:ScheduledCourseSectionViewModel) => {
      if (res) {
        if (res._failure) {
          if (res.courseSectionViewList == null) {
            this.snackbar.open(res._message, '', {
              duration: 10000
            });
            this.courseSectionViewList = new MatTableDataSource([]);
          } else {
            this.courseSectionViewList = new MatTableDataSource([]);
          }
        }
        else {
          this.totalCount = res.missingAttendanceCount;
            this.pageNumber = res.pageNumber;
            this.pageSize = res._pageSize;
          this.courseSectionViewList = new MatTableDataSource(res.courseSectionViewList);
        }
      }
      else {
        this.snackbar.open(sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
    });
  }

  takeAttendance(element){
    this.router.navigate(['/school', 'attendance', 'missing-attendance', 'take-attendance']);
    Object.assign(this.staffDetails, element);
    this.studentAttendanceService.setStaffDetails(this.staffDetails);
  }

  ngOnDestroy() {
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }

}
