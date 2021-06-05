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

import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { AddCommentsComponent } from './add-comments/add-comments.component';
import { AddTeacherCommentsComponent } from './add-teacher-comments/add-teacher-comments.component';
import icPrint from '@iconify/icons-ic/twotone-print';
import { AddReportCardPdf } from 'src/app/models/report-card.model';
import { GetMarkingPeriodTitleListModel } from 'src/app/models/marking-period.model';
import { MarkingPeriodService } from 'src/app/services/marking-period.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { StudentService } from 'src/app/services/student.service';
import { ReportCardService } from 'src/app/services/report-card.service';
import { Router } from '@angular/router';

@Component({
  selector: 'vex-student-report-card',
  templateUrl: './student-report-card.component.html',
  styleUrls: ['./student-report-card.component.scss']
})
export class StudentReportCardComponent implements OnInit {

  icPrint = icPrint;
  addReportCardPdf: AddReportCardPdf =  new AddReportCardPdf();
  getMarkingPeriodTitleListModel: GetMarkingPeriodTitleListModel = new GetMarkingPeriodTitleListModel();
  markingPeriodList = [];
  markingPeriodError: boolean;
  markingPeriods = [];
  studentDetailsForViewAndEdit;
  pdfData;
  studentCreateMode;

  constructor(
    private dialog: MatDialog,
    public translateService: TranslateService,
    private markingPeriodService: MarkingPeriodService,
    private snackbar: MatSnackBar,
    private studentService: StudentService,
    private reportCardService: ReportCardService,
    private router: Router,
    ) {
    translateService.use("en");
  }

  ngOnInit(): void {
    this.getAllMarkingPeriodList();
    this.studentService.studentDetailsForViewedAndEdited.subscribe((res)=>{
      this.addReportCardPdf.studentsReportCardViewModelList.push({studentId: res.studentMaster.studentId});
    });

    this.studentService.studentCreatedMode.subscribe((res)=>{
      this.studentCreateMode = res;
      if(!res) {
        this.router.navigate(['/school', 'students', 'student-generalinfo']);
      }
    })

  }
  addComments(){
    this.dialog.open(AddCommentsComponent, {
      width: '500px'
    })
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

  markingPeriodChecked(event, markingPeriod) {
    if(event.checked) {
      this.markingPeriods.push(markingPeriod.value);
    } else {
      this.markingPeriods.splice(this.markingPeriods.findIndex(x => x === markingPeriod.value), 1);
    }
    this.markingPeriodError = this.markingPeriods.length > 0 ? false : true;
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

  addTeacherComments(){
    this.dialog.open(AddTeacherCommentsComponent, {
      width: '500px'
    })
  }

}
