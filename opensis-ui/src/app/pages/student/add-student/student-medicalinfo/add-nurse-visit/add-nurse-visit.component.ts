import { Component, Inject, OnInit } from '@angular/core';
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

import { TranslateService } from '@ngx-translate/core';
import icClose from '@iconify/icons-ic/twotone-close';
import icSchedule from '@iconify/icons-ic/twotone-schedule';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { StudentService } from '../../../../../services/student.service';
import { AddEditStudentMedicalNurseVisitModel } from 'src/app/models/student.model';
import { ValidationService } from 'src/app/pages/shared/validation.service';

@Component({
  selector: 'vex-add-nurse-visit',
  templateUrl: './add-nurse-visit.component.html',
  styleUrls: ['./add-nurse-visit.component.scss']
})
export class AddNurseVisitComponent implements OnInit {

  icClose = icClose;
  icSchedule = icSchedule;
  form: FormGroup;
  addEditStudentMedicalNurseVisitModel: AddEditStudentMedicalNurseVisitModel = new AddEditStudentMedicalNurseVisitModel();
  editMode: boolean;
  buttonType: string;
  nurseVisitTitle: string;

  constructor(
    private dialogRef: MatDialogRef<AddNurseVisitComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private snackbar: MatSnackBar,
    private studentService: StudentService,
    private fb: FormBuilder,
    public translateService: TranslateService) {
    translateService.use('en');
  }

  ngOnInit(): void {
    this.form = this.fb.group({
      id: [0],
      nurseVisitDate: ['',[Validators.required]],
      timeIn: [],
      timeOut: [],
      reason: ['', [ValidationService.noWhitespaceValidator]],
      result: [],
      comment: []
    });
    if (this.data == null){
      this.editMode = false;
      this.buttonType = 'submit';
      this.nurseVisitTitle = 'addNurseVisitRecord';
    }
    else{
      this.editMode = true;
      this.buttonType = 'update';
      this.nurseVisitTitle = 'editNurseVisitRecord';
      this.form.controls.id.patchValue(this.data.id);
      this.form.controls.nurseVisitDate.patchValue(this.data.nurseVisitDate);
      this.form.controls.timeIn.patchValue(this.data.timeIn);
      this.form.controls.timeOut.patchValue(this.data.timeOut);
      this.form.controls.reason.patchValue(this.data.reason);
      this.form.controls.result.patchValue(this.data.result);
      this.form.controls.comment.patchValue(this.data.comment);
    }
  }
  submit(){
    this.form.markAsTouched();
    if (this.form.valid){
      if (this.form.controls.id.value === 0){
        this.addEditStudentMedicalNurseVisitModel.studentMedicalNurseVisit.nurseVisitDate =
        this.form.controls.nurseVisitDate.value;
        this.addEditStudentMedicalNurseVisitModel.studentMedicalNurseVisit.timeIn =
        this.form.controls.timeIn.value;
        this.addEditStudentMedicalNurseVisitModel.studentMedicalNurseVisit.timeOut =
        this.form.controls.timeOut.value;
        this.addEditStudentMedicalNurseVisitModel.studentMedicalNurseVisit.reason =
        this.form.controls.reason.value;
        this.addEditStudentMedicalNurseVisitModel.studentMedicalNurseVisit.result =
        this.form.controls.result.value;
        this.addEditStudentMedicalNurseVisitModel.studentMedicalNurseVisit.comment =
        this.form.controls.comment.value;
        this.addEditStudentMedicalNurseVisitModel.studentMedicalNurseVisit.studentId = this.studentService.getStudentId();
        this.studentService.addStudentMedicalNurseVisit(this.addEditStudentMedicalNurseVisitModel).subscribe(
          (res: AddEditStudentMedicalNurseVisitModel) => {
            if (res){
              if (res._failure){
                this.snackbar.open( res._message, '', {
                  duration: 10000
                });
              }
              else{
                this.snackbar.open( res._message, '', {
                  duration: 10000
                });
                this.dialogRef.close('submited');
              }
            }else{
              this.snackbar.open( sessionStorage.getItem('httpError'), '', {
                duration: 10000
              });
            }
          }
        );
      }
      else{
        this.addEditStudentMedicalNurseVisitModel.studentMedicalNurseVisit.id = this.form.controls.id.value;
        this.addEditStudentMedicalNurseVisitModel.studentMedicalNurseVisit.nurseVisitDate =
        this.form.controls.nurseVisitDate.value;
        this.addEditStudentMedicalNurseVisitModel.studentMedicalNurseVisit.timeIn =
        this.form.controls.timeIn.value;
        this.addEditStudentMedicalNurseVisitModel.studentMedicalNurseVisit.timeOut =
        this.form.controls.timeOut.value;
        this.addEditStudentMedicalNurseVisitModel.studentMedicalNurseVisit.reason =
        this.form.controls.reason.value;
        this.addEditStudentMedicalNurseVisitModel.studentMedicalNurseVisit.result =
        this.form.controls.result.value;
        this.addEditStudentMedicalNurseVisitModel.studentMedicalNurseVisit.comment =
        this.form.controls.comment.value;
        this.addEditStudentMedicalNurseVisitModel.studentMedicalNurseVisit.studentId = this.studentService.getStudentId();
        this.studentService.updateStudentMedicalNurseVisit(this.addEditStudentMedicalNurseVisitModel).subscribe(
          (res: AddEditStudentMedicalNurseVisitModel) => {
            if (res){
              if (res._failure){
                this.snackbar.open( res._message, '', {
                  duration: 10000
                });
              }
              else{
                this.snackbar.open( res._message, '', {
                  duration: 10000
                });
                this.dialogRef.close('submited');
              }
            }else{
              this.snackbar.open( sessionStorage.getItem('httpError'), '', {
                duration: 10000
              });
            }
          }
        );
      }
    }
  }
  cancel(){
    this.dialogRef.close();
  }
}
