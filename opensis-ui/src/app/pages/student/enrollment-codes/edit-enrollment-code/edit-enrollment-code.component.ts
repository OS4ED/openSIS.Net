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
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import icClose from '@iconify/icons-ic/twotone-close';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { MatSnackBar } from '@angular/material/snack-bar';
import { EnrollmentCodesService } from '../../../../services/enrollment-codes.service';
import {EnrollmentCodeAddView} from '../../../../models/enrollment-code.model';
import {EnrollmentCodeEnum } from '../../../../enums/enrollment_code.enum';
import { DefaultValuesService } from '../../../../common/default-values.service';

@Component({
  selector: 'vex-edit-enrollment-code',
  templateUrl: './edit-enrollment-code.component.html',
  styleUrls: ['./edit-enrollment-code.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class EditEnrollmentCodeComponent implements OnInit {
  icClose = icClose;
  form: FormGroup;
  enrollmentCodeTitle;
  buttonType;
  enrollmentCodeAddView: EnrollmentCodeAddView = new EnrollmentCodeAddView();
  enrollmentCodeEnum = Object.keys(EnrollmentCodeEnum);
  constructor(
    private dialogRef: MatDialogRef<EditEnrollmentCodeComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private defaultValuesService: DefaultValuesService,
    private snackbar: MatSnackBar,
    private fb: FormBuilder,
    private enrollmentCodeService: EnrollmentCodesService
     ) {
    this.form = fb.group({
      enrollmentCode: [0],
      title: ['', [Validators.required]],
      shortName: ['', [Validators.required]],
      sortOrder: ['', [Validators.required, Validators.min(1)]],
      type: ['', []]
    });
   }

  ngOnInit(): void {
    if (this.data == null){
      this.enrollmentCodeTitle = 'addEnrollmentCode';
      this.buttonType = 'SUBMIT';
    }
    else{
      this.enrollmentCodeTitle = 'editEnrollmentCode';
      this.buttonType = 'UPDATE';
      this.form.controls.enrollmentCode.patchValue(this.data.enrollmentCode);
      this.form.controls.title.patchValue(this.data.title);
      this.form.controls.shortName.patchValue(this.data.shortName);
      this.form.controls.sortOrder.patchValue(this.data.sortOrder);
      this.form.controls.type.patchValue(this.data.type);
    }
  }
  submit(){
    this.form.markAllAsTouched();
    if (this.form.valid) {
    if (this.form.controls.enrollmentCode.value === 0){
      this.enrollmentCodeAddView.studentEnrollmentCode.title = this.form.controls.title.value;
      this.enrollmentCodeAddView.studentEnrollmentCode.shortName = this.form.controls.shortName.value;
      this.enrollmentCodeAddView.studentEnrollmentCode.sortOrder = this.form.controls.sortOrder.value;
      this.enrollmentCodeAddView.studentEnrollmentCode.type = this.form.controls.type.value;
      this.enrollmentCodeService.addStudentEnrollmentCode(this.enrollmentCodeAddView).subscribe(
        (res: EnrollmentCodeAddView) => {
          if (res){
            if (res._failure) {
              this.snackbar.open( res._message, '', {
                duration: 10000
              });
            }
            else {
              this.snackbar.open( res._message, '', {
                duration: 10000
              });
              this.dialogRef.close('submited');
            }
          }else{
            this.snackbar.open( this.defaultValuesService.translateKey('enrollmentCodeFailed') + sessionStorage.getItem('httpError'), '', {
              duration: 10000
            });
          }
        }
      );
    }
    else{
      this.enrollmentCodeAddView.studentEnrollmentCode.enrollmentCode = this.form.controls.enrollmentCode.value;
      this.enrollmentCodeAddView.studentEnrollmentCode.title = this.form.controls.title.value;
      this.enrollmentCodeAddView.studentEnrollmentCode.shortName = this.form.controls.shortName.value;
      this.enrollmentCodeAddView.studentEnrollmentCode.sortOrder = this.form.controls.sortOrder.value;
      this.enrollmentCodeAddView.studentEnrollmentCode.type = this.form.controls.type.value;
      this.enrollmentCodeService.updateStudentEnrollmentCode(this.enrollmentCodeAddView).subscribe(
        (res: EnrollmentCodeAddView) => {
          if (res){
            if (res._failure) {
              this.snackbar.open( res._message, '', {
                duration: 10000
              });
            }
            else {
              this.snackbar.open( res._message, '', {
                duration: 10000
              });
              this.dialogRef.close('submited');
            }
          }else{
            this.snackbar.open(this.defaultValuesService.translateKey('enrollmentCodeFailed') + sessionStorage.getItem('httpError'), '', {
              duration: 10000
            });
          }
        }
      );
    }
  }
  }
}
