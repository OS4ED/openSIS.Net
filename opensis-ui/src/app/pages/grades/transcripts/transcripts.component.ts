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

import { AfterViewInit, Component, ElementRef, OnDestroy, OnInit, ViewChild } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import icSearch from '@iconify/icons-ic/search';
import { GetAllGradeLevelsModel } from "../../../models/grade-level.model";
import { GradeLevelService } from "../../../services/grade-level.service";
import { MatSnackBar } from "@angular/material/snack-bar";
import { StudentListModel } from '../../../models/student.model';
import { StudentService } from "../../../services/student.service";
import { MatTableDataSource } from "@angular/material/table";
import { FormControl } from "@angular/forms";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { debounceTime, distinctUntilChanged, switchMap, takeUntil } from "rxjs/operators";
import { stagger40ms } from '../../../../@vex/animations/stagger.animation';
import { LoaderService } from '../../../services/loader.service';
import { forkJoin, Subject } from "rxjs";
import { fadeInUp400ms } from "../../../../@vex/animations/fade-in-up.animation";
import { fadeInRight400ms } from "../../../../@vex/animations/fade-in-right.animation";
import { MatCheckbox } from "@angular/material/checkbox";
import { StudentTranscript } from "../../../models/student-transcript.model";
import { StudentTranscriptService } from "../../../services/student-transcript.service";
import { map } from 'rxjs/operators';

@Component({
  selector: "vex-transcripts",
  templateUrl: "./transcripts.component.html",
  styleUrls: ["./transcripts.component.scss"],
  animations: [
    fadeInUp400ms,
    stagger40ms,
    fadeInRight400ms
  ]
})
export class TranscriptsComponent implements OnInit, OnDestroy {
  icSearch = icSearch;
  columns = [
    { label: '', property: 'selectedStudent', type: 'text', visible: true },
    { label: 'Name', property: 'firstGivenName', type: 'text', visible: true },
    { label: 'Student ID', property: 'studentInternalId', type: 'text', visible: true },
    { label: 'Alternate ID', property: 'alternateId', type: 'text', visible: true },
    { label: 'Grade Level', property: 'gradeLevelTitle', type: 'text', visible: true },
    { label: 'Section', property: 'sectionId', type: 'text', visible: true },
    { label: 'Telephone', property: 'homePhone', type: 'text', visible: true },
    { label: 'Status', property: 'status', type: 'text', visible: false },
  ];

  loading: boolean;
  destroySubject$: Subject<void> = new Subject();
  constructor(
    public translateService: TranslateService,
    private gradeLevelService: GradeLevelService,
    private snackbar: MatSnackBar,
    private studentService: StudentService,
    private loaderService: LoaderService,
    private transcriptService: StudentTranscriptService,
    private el: ElementRef
  ) {
    translateService.use("en");
    this.getAllStudent.filterParams = null;
    this.loaderService.isLoading.pipe(takeUntil(this.destroySubject$)).subscribe((val) => {
      this.loading = val;
    });
  }
  totalCount = 0;
  pageNumber: number;
  pageSize: number;
  searchValue;
  searchCount;
  searchCtrl: FormControl;
  getAllGradeLevels: GetAllGradeLevelsModel = new GetAllGradeLevelsModel();
  getAllStudent: StudentListModel = new StudentListModel();
  studentList: MatTableDataSource<any>;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('masterCheckBox') masterCheckBox: MatCheckbox;
  showAdvanceSearchPanel: boolean = false;
  toggleValues;
  listOfStudents = [];
  selectedStudents = []
  selectedGradeLevels = [];
  studentTranscipt = new StudentTranscript();
  gradeLevelError: boolean;
  pdfByteArrayForTranscript: string;
  pdfGenerateLoader: boolean = false;
  ngOnInit(): void {
    this.searchCtrl = new FormControl();
    this.callAllStudent();
    this.getAllGradeLevel();
  }

  get visibleColumns() {
    return this.columns.filter(column => column.visible).map(column => column.property);
  }

  ngAfterViewInit() {

    //  Sorting
    this.getAllStudent = new StudentListModel();
    this.sort.sortChange.subscribe((res) => {
      this.getAllStudent.pageNumber = this.pageNumber
      this.getAllStudent.pageSize = this.pageSize;
      this.getAllStudent.sortingModel.sortColumn = res.active;
      if (this.searchCtrl.value) {
        let filterParams = [
          {
            columnName: null,
            filterValue: this.searchCtrl.value,
            filterOption: 3
          }
        ]
        Object.assign(this.getAllStudent, { filterParams: filterParams });
      }
      if (res.direction == "") {
        this.getAllStudent.sortingModel = null;
        this.callAllStudent();
        this.getAllStudent = new StudentListModel();
        this.getAllStudent.sortingModel = null;
      } else {
        this.getAllStudent.sortingModel.sortDirection = res.direction;
        this.callAllStudent();
      }
    });
    //  Searching
    this.searchCtrl.valueChanges.pipe(debounceTime(500), distinctUntilChanged()).subscribe((term) => {
      if (term) {
        this.callWithSearchParams(term)
      }
      else {
        this.callWithoutSearchParams()
      }
    })
  }

  callWithSearchParams(term) {
    let filterParams = [
      {
        columnName: null,
        filterValue: term,
        filterOption: 3
      }
    ]
    if (this.sort.active && this.sort.direction) {
      this.getAllStudent.sortingModel.sortColumn = this.sort.active;
      this.getAllStudent.sortingModel.sortDirection = this.sort.direction;
    }
    Object.assign(this.getAllStudent, { filterParams: filterParams });
    this.getAllStudent.pageNumber = 1;
    this.paginator.pageIndex = 0;
    this.getAllStudent.pageSize = this.pageSize;
    this.callAllStudent();
  }
  callWithoutSearchParams() {
    Object.assign(this.getAllStudent, { filterParams: null });
    this.getAllStudent.pageNumber = this.paginator.pageIndex + 1;
    this.getAllStudent.pageSize = this.pageSize;
    if (this.sort.active != undefined && this.sort.direction != "") {
      this.getAllStudent.sortingModel.sortColumn = this.sort.active;
      this.getAllStudent.sortingModel.sortDirection = this.sort.direction;
    }
    this.callAllStudent();
  }

  resetStudentList() {
    this.searchCount = null;
    this.searchValue = null;
    this.callAllStudent();
  }

  onGradeLevelChange(event, gradeId) {
    if (event.checked) {
      this.selectedGradeLevels.push(gradeId);
    } else {
      this.selectedGradeLevels = this.selectedGradeLevels.filter(item => item !== gradeId);
    }
    this.selectedGradeLevels?.length > 0 ? this.gradeLevelError = false : this.gradeLevelError = true;

  }

  getPageEvent(event) {
    if (this.sort.active && this.sort.direction) {
      this.getAllStudent.sortingModel.sortColumn = this.sort.active;
      this.getAllStudent.sortingModel.sortDirection = this.sort.direction;
    }
    if (this.searchCtrl.value) {
      let filterParams = [
        {
          columnName: null,
          filterValue: this.searchCtrl.value,
          filterOption: 3
        }
      ]
      Object.assign(this.getAllStudent, { filterParams: filterParams });
    }
    this.getAllStudent.pageNumber = event.pageIndex + 1;
    this.getAllStudent.pageSize = event.pageSize;
    this.callAllStudent();
  }

  callAllStudent() {
    if (this.getAllStudent.sortingModel?.sortColumn == "") {
      this.getAllStudent.sortingModel = null;
    }
    this.studentService.GetAllStudentList(this.getAllStudent).subscribe(res => {
      if (res._failure) {
        if (res.studentMaster === null) {
          this.totalCount = null;
          this.studentList = new MatTableDataSource([]);
          this.snackbar.open(res._message, '', {
            duration: 10000
          });
        } else {
          this.studentList = new MatTableDataSource([]);
          this.totalCount = null;
        }
      } else {
        this.totalCount = res.totalCount;
        this.pageNumber = res.pageNumber;
        this.pageSize = res._pageSize;
        res.studentMaster.forEach((student) => {
          student.checked = false;
        });
        this.listOfStudents = res.studentMaster.map((item) => {
          this.selectedStudents.map((selectedUser) => {
            if (item.studentId == selectedUser.studentId) {
              item.checked = true;
              return item;
            }
          });
          return item;
        });

        this.masterCheckBox.checked = this.listOfStudents.every((item) => {
          return item.checked;
        })
        this.studentList = new MatTableDataSource(res.studentMaster);
        this.getAllStudent = new StudentListModel();
      }
    });
  }

  getSearchResult(res) {
    if (res?.totalCount){
      this.searchCount = res.totalCount;
      this.totalCount = res.totalCount;
    }
    else{
      this.searchCount = 0;
      this.totalCount = 0;
    }
    this.pageNumber = res.pageNumber;
    this.pageSize = res.pageSize;
    if (res && res.studentMaster) {
      res?.studentMaster?.forEach((student) => {
        student.checked = false;
      });
      this.listOfStudents = res.studentMaster.map((item) => {
        this.selectedStudents.map((selectedUser) => {
          if (item.studentId == selectedUser.studentId) {
            item.checked = true;
            return item;
          }
        });
        return item;
      });

      this.masterCheckBox.checked = this.listOfStudents.every((item) => {
        return item.checked;
      })
    }
    this.studentList = new MatTableDataSource(res?.studentMaster);
    this.getAllStudent = new StudentListModel();
  }

  getToggleValues(event) {
    this.toggleValues = event;
    if (event.inactiveStudents === true) {
      this.columns[7].visible = true;
    }
    else if (event.inactiveStudents === false) {
      this.columns[7].visible = false;
    }
  }

  hideAdvanceSearch(event) {
    this.showAdvanceSearchPanel = false;
  }

  getSearchInput(event) {
    this.searchValue = event;
  }

  getAllGradeLevel() {
    this.gradeLevelService.getAllGradeLevels(this.getAllGradeLevels).pipe(takeUntil(this.destroySubject$)).subscribe((res) => {
      if (res) {
        if (res._failure) {
          if (res.tableGradelevelList === null) {
            this.getAllGradeLevels.tableGradelevelList = [];
            this.snackbar.open(res._message, '', {
              duration: 10000
            });
          } else {
            this.getAllGradeLevels.tableGradelevelList = res.tableGradelevelList;
          }
        }
        else {
          this.getAllGradeLevels.tableGradelevelList = res.tableGradelevelList;
        }
      } else {
        this.snackbar.open(sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
    })
  }

  someComplete(): boolean {
    let indetermine = false;
    for (let user of this.listOfStudents) {
      for (let selectedUser of this.selectedStudents) {
        if (user.studentId == selectedUser.studentId) {
          indetermine = true;
        }
      }
    }
    if (indetermine) {
      this.masterCheckBox.checked = this.listOfStudents.every((item) => {
        return item.checked;
      })
      if (this.masterCheckBox.checked) {
        return false;
      } else {
        return true;
      }
    }

  }

  setAll(event) {
    this.listOfStudents.forEach(user => { user.checked = event; });
    this.studentList = new MatTableDataSource(this.listOfStudents);
    this.decideCheckUncheck();
  }

  onChangeSelection(eventStatus: boolean, id) {
    for (let item of this.listOfStudents) {
      if (item.studentId == id) {
        item.checked = eventStatus;
        break;
      }
    }
    this.studentList = new MatTableDataSource(this.listOfStudents);
    this.masterCheckBox.checked = this.listOfStudents.every((item) => {
      return item.checked;
    });

    this.decideCheckUncheck();
  }

  decideCheckUncheck() {
    this.listOfStudents.map((item) => {
      let isIdIncludesInSelectedList = false;
      if (item.checked) {
        for (let selectedUser of this.selectedStudents) {
          if (item.studentId == selectedUser.studentId) {
            isIdIncludesInSelectedList = true;
            break;
          }
        }
        if (!isIdIncludesInSelectedList) {
          this.selectedStudents.push(item);
        }
      } else {
        for (let selectedUser of this.selectedStudents) {
          if (item.studentId == selectedUser.studentId) {
            this.selectedStudents = this.selectedStudents.filter((user) => user.studentId != item.studentId);
            break;
          }
        }
      }
      isIdIncludesInSelectedList = false;

    });
    this.selectedStudents = this.selectedStudents.filter((item) => item.checked);
  }

  generateTranscript() {
    if (!this.selectedGradeLevels?.length) {
      this.gradeLevelError = true;
      const invalidGradeLevel: HTMLElement = this.el.nativeElement.querySelector(
        '.custom-scroll'
      );
      invalidGradeLevel.scrollIntoView({ behavior: 'smooth',block: 'center' });
      return;
    } else {
      this.gradeLevelError = false;
    }
    if (!this.selectedStudents?.length) {
      this.snackbar.open('Select at least one student.', '', {
        duration: 3000
      });
      return
    }
    this.fillUpNecessaryValues();
    this.fetchTranscript();
  }

  fillUpNecessaryValues() {
    this.studentTranscipt.gradeLavels = this.selectedGradeLevels.toString();
    this.studentTranscipt.studentListForTranscript = [];
    this.selectedStudents?.map((item) => {
      this.studentTranscipt.studentListForTranscript.push({
        studentId: item.studentId,
        studentGuid: item.studentGuid,
        firstGivenName: item.firstGivenName,
        middleName: item.middleName,
        lastFamilyName: item.lastFamilyName,
      })
    });
  }

  fetchTranscript() {
    this.pdfGenerateLoader = true;
    this.transcriptService.addTranscriptForStudent(this.studentTranscipt).pipe(
      takeUntil(this.destroySubject$),
      switchMap((dataAfterAddingStudentRecords) => {
        let generateTranscriptObservable$
        if (!dataAfterAddingStudentRecords._failure) {
          generateTranscriptObservable$ = this.transcriptService.generateTranscriptForStudent(this.studentTranscipt)
        } else {
          this.snackbar.open(dataAfterAddingStudentRecords._message, '', {
            duration: 3000
          });
        }
        return forkJoin(generateTranscriptObservable$);
      })
    ).subscribe((res: any) => {
      this.pdfGenerateLoader = false;
      let response = res[0]
      if (response._failure) {
        this.snackbar.open(response._message, '', {
          duration: 3000
        });
      } else {
        this.pdfByteArrayForTranscript = response.transcriptPdf;
      }
    });
  }

  backToList() {
    this.pdfByteArrayForTranscript = null
  }

  ngOnDestroy() {
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }

}
