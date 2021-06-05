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
import { MatSnackBar } from "@angular/material/snack-bar";
import { MatTableDataSource } from "@angular/material/table";
import { FormControl } from "@angular/forms";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { debounceTime, distinctUntilChanged, switchMap, takeUntil } from "rxjs/operators";
import { forkJoin, Subject } from "rxjs";
import { MatCheckbox } from "@angular/material/checkbox";
import { fadeInUp400ms } from "src/@vex/animations/fade-in-up.animation";
import { stagger40ms } from "src/@vex/animations/stagger.animation";
import { fadeInRight400ms } from "src/@vex/animations/fade-in-right.animation";
import { GradeLevelService } from "src/app/services/grade-level.service";
import { StudentService } from "src/app/services/student.service";
import { LoaderService } from "src/app/services/loader.service";
import { StudentTranscriptService } from "src/app/services/student-transcript.service";
import { GetAllGradeLevelsModel } from "src/app/models/grade-level.model";
import { StudentListModel } from "src/app/models/student.model";
import { StudentTranscript } from "src/app/models/student-transcript.model";
import { Router } from "@angular/router";

@Component({
  selector: "vex-student-transcript",
  templateUrl: "./student-transcript.component.html",
  styleUrls: ["./student-transcript.component.scss"],
  animations: [
    fadeInUp400ms,
    stagger40ms,
    fadeInRight400ms
  ]
})
export class StudentTranscriptComponent implements OnInit {
  icSearch = icSearch;

  loading: boolean;
  destroySubject$: Subject<void> = new Subject();
  constructor(
    public translateService: TranslateService,
    private gradeLevelService: GradeLevelService,
    private snackbar: MatSnackBar,
    private studentService: StudentService,
    private loaderService: LoaderService,
    private transcriptService: StudentTranscriptService,
    private el: ElementRef,
    private router: Router
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
    this.getAllGradeLevel();
    this.studentService.studentDetailsForViewedAndEdited.subscribe((res)=>{
      this.selectedStudents.push(res.studentMaster);
    });

    this.studentService.studentCreatedMode.subscribe((res)=>{
      if(!res) {
        this.router.navigate(['/school', 'students', 'student-generalinfo']);
      }
    })
    this.selectedStudents
  }


  onGradeLevelChange(event, gradeId) {
    if (event.checked) {
      this.selectedGradeLevels.push(gradeId);
    } else {
      this.selectedGradeLevels = this.selectedGradeLevels.filter(item => item !== gradeId);
    }
    this.selectedGradeLevels?.length > 0 ? this.gradeLevelError = false : this.gradeLevelError = true;
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
