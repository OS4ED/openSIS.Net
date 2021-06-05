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
import { TranslateService } from '@ngx-translate/core';
import icClose from '@iconify/icons-ic/twotone-close';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { GradeAddViewModel } from 'src/app/models/grades.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { GradesService } from 'src/app/services/grades.service';
import { ValidationService } from 'src/app/pages/shared/validation.service';
import { stagger60ms } from 'src/@vex/animations/stagger.animation';
import { fadeInUp400ms } from 'src/@vex/animations/fade-in-up.animation';

@Component({
  selector: 'vex-add-grade',
  templateUrl: './add-grade.component.html',
  styleUrls: ['./add-grade.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class AddGradeComponent implements OnInit {

  icClose = icClose;
  gradeAddViewModel: GradeAddViewModel = new GradeAddViewModel();
  form: FormGroup;
  gradeTitle: string;
  buttonType: string;
  gradeScaleId: number;

  constructor(private dialogRef: MatDialogRef<AddGradeComponent>,
    public translateService: TranslateService,
    private fb: FormBuilder,
    private snackbar: MatSnackBar,
    private gradesService: GradesService,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    translateService.use('en');

  }

  ngOnInit(): void {
    this.form = this.fb.group({
      gradeScaleId: [],
      gradeId: [0],
      title: ['', [ValidationService.noWhitespaceValidator]],
      description: ['', [Validators.required]]
    });
    if (this.data.information) {
      this.buttonType = "update";
      this.gradeTitle = "editGrade";
      this.form.patchValue({
        gradeId:this.data.information.gradeId,
        gradeScaleId:this.data.information.gradeScaleId,
        title:this.data.information.title,
        description:this.data.information.comment
      })
    }
    else {
      this.gradeScaleId = this.data.gradeScaleId
      this.gradeTitle = "addGrade";
      this.buttonType = "submit";
    }
  }

  submit() {
    this.form.markAllAsTouched();
    if (this.form.valid) {
      if (this.form.controls.gradeId.value == 0) {
        this.gradeAddViewModel.grade.gradeScaleId = this.gradeScaleId;
        this.gradeAddViewModel.grade.gradeId = this.form.controls.gradeId.value;
        this.gradeAddViewModel.grade.title = this.form.controls.title.value;
        this.gradeAddViewModel.grade.comment = this.form.controls.description.value;

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
                this.dialogRef.close(true);
              }
            }
          }
        );
      }
      else {
        this.gradeAddViewModel.grade.gradeScaleId = this.form.controls.gradeScaleId.value;
        this.gradeAddViewModel.grade.gradeId = this.form.controls.gradeId.value;
        this.gradeAddViewModel.grade.title = this.form.controls.title.value;
        this.gradeAddViewModel.grade.comment = this.form.controls.description.value;

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
                this.dialogRef.close(true);
              }
            }
          }
        );
      }
    }
  }

}
