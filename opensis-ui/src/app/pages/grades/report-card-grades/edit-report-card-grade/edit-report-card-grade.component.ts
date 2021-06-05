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

import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import icClose from '@iconify/icons-ic/twotone-close';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { GradeAddViewModel } from '../../../../models/grades.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { GradesService } from 'src/app/services/grades.service';
import { ValidationService } from 'src/app/pages/shared/validation.service';

@Component({
  selector: 'vex-edit-report-card-grade',
  templateUrl: './edit-report-card-grade.component.html',
  styleUrls: ['./edit-report-card-grade.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class EditReportCardGradeComponent implements OnInit {
  icClose = icClose;
  gradeAddViewModel: GradeAddViewModel = new GradeAddViewModel();
  form: FormGroup;
  gradeTitle: string;
  buttonType: string;
  gradeScaleId: number;

  constructor(
    private dialogRef: MatDialogRef<EditReportCardGradeComponent>,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private snackbar: MatSnackBar,
    private gradesService: GradesService
  ) {
    this.form = fb.group({
      gradeScaleId: [],
      gradeId: [0],
      title: ['', [ValidationService.noWhitespaceValidator]],
      breakoff: ['', [Validators.required, Validators.min(0)]],
      weightedGpValue: ['', [Validators.min(0)]],
      unweightedGpValue: ['', [Validators.min(0)]],
      comment: []
    });
    if (data.information == null) {
      this.gradeScaleId = data.gradeScaleId
      this.gradeTitle = "addGrade";
      this.buttonType = "submit";
    }
    else {
      this.buttonType = "update";
      this.gradeTitle = "editGrade";
      this.form.controls.gradeId.patchValue(data.information.gradeId)
      this.form.controls.gradeScaleId.patchValue(data.information.gradeScaleId)
      this.form.controls.title.patchValue(data.information.title)
      this.form.controls.breakoff.patchValue(data.information.breakoff)
      this.form.controls.weightedGpValue.patchValue(data.information.weightedGpValue)
      this.form.controls.unweightedGpValue.patchValue(data.information.unweightedGpValue)
      this.form.controls.comment.patchValue(data.information.comment)
    }
  }

  ngOnInit(): void {
  }
  submit() {
    this.form.markAllAsTouched();
    if (this.form.valid) {
      if (this.form.controls.gradeId.value == 0) {
        this.gradeAddViewModel.grade.gradeScaleId = this.gradeScaleId;
        this.gradeAddViewModel.grade.gradeId = this.form.controls.gradeId.value;
        this.gradeAddViewModel.grade.title = this.form.controls.title.value;
        this.gradeAddViewModel.grade.breakoff = this.form.controls.breakoff.value;
        this.gradeAddViewModel.grade.weightedGpValue = this.form.controls.weightedGpValue.value;
        this.gradeAddViewModel.grade.unweightedGpValue = this.form.controls.unweightedGpValue.value;
        this.gradeAddViewModel.grade.comment = this.form.controls.comment.value;

        this.gradesService.addGrade(this.gradeAddViewModel).subscribe(
          (res: GradeAddViewModel) => {
            if (typeof (res) == 'undefined') {
              this.snackbar.open('' + sessionStorage.getItem("httpError"), '', {
                duration: 10000
              });
            }
            else {
              if (res._failure) {
                this.snackbar.open('' + res._message, '', {
                  duration: 10000
                });
              }
              else {
                this.snackbar.open('' + res._message, '', {
                  duration: 10000
                });
                this.dialogRef.close('submited');
              }
            }
          }
        );
      }
      else {
        this.gradeAddViewModel.grade.gradeScaleId = this.form.controls.gradeScaleId.value;
        this.gradeAddViewModel.grade.gradeId = this.form.controls.gradeId.value;
        this.gradeAddViewModel.grade.title = this.form.controls.title.value;
        this.gradeAddViewModel.grade.breakoff = this.form.controls.breakoff.value;
        this.gradeAddViewModel.grade.weightedGpValue = this.form.controls.weightedGpValue.value;
        this.gradeAddViewModel.grade.unweightedGpValue = this.form.controls.unweightedGpValue.value;
        this.gradeAddViewModel.grade.comment = this.form.controls.comment.value;
        this.gradesService.updateGrade(this.gradeAddViewModel).subscribe(
          (res: GradeAddViewModel) => {
            if (typeof (res) == 'undefined') {
              this.snackbar.open('' + sessionStorage.getItem("httpError"), '', {
                duration: 10000
              });
            }
            else {
              if (res._failure) {
                this.snackbar.open('' + res._message, '', {
                  duration: 10000
                });
              }
              else {
                this.snackbar.open('' + res._message, '', {
                  duration: 10000
                });
                this.dialogRef.close('submited');
              }
            }
          }
        );
      }
    }
  }

}
