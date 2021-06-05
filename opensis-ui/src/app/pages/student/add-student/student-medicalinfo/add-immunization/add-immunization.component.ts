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
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AddEditStudentMedicalImmunizationModel } from 'src/app/models/student.model';
import { StudentService } from '../../../../../services/student.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'vex-add-immunization',
  templateUrl: './add-immunization.component.html',
  styleUrls: ['./add-immunization.component.scss']
})
export class AddImmunizationComponent implements OnInit {

  icClose = icClose;
  form: FormGroup;
  editMode;
  immunizationTitle;
  buttonType;
  addEditStudentMedicalImmunizationModel: AddEditStudentMedicalImmunizationModel = new AddEditStudentMedicalImmunizationModel();

  constructor(
    private dialogRef: MatDialogRef<AddImmunizationComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private snackbar: MatSnackBar,
    public translateService: TranslateService,
    private fb: FormBuilder,
    private studentService: StudentService) {
    translateService.use('en');
  }

  ngOnInit(): void {
    this.form = this.fb.group({
      id: [0],
      immunizationType: ['', [Validators.required]],
      immunizationDate: ['', [Validators.required]],
      comment: ['']
    });
    if (this.data == null){
      this.editMode = false;
      this.buttonType = 'submit';
      this.immunizationTitle = 'addImmunizationPhysicalRecord';
    }
    else{
      this.editMode = true;
      this.buttonType = 'update';
      this.immunizationTitle = 'editImmunizationPhysicalRecord';
      this.form.controls.id.patchValue(this.data.id);
      this.form.controls.immunizationType.patchValue(this.data.immunizationType);
      this.form.controls.immunizationDate.patchValue(this.data.immunizationDate);
      this.form.controls.comment.patchValue(this.data.comment);
    }
  }
  submit(){
    this.form.markAsTouched();
    if (this.form.valid){
      if (this.form.controls.id.value === 0){
        this.addEditStudentMedicalImmunizationModel.studentMedicalImmunization.immunizationType =
        this.form.controls.immunizationType.value;
        this.addEditStudentMedicalImmunizationModel.studentMedicalImmunization.immunizationDate =
        this.form.controls.immunizationDate.value;
        this.addEditStudentMedicalImmunizationModel.studentMedicalImmunization.comment =
        this.form.controls.comment.value;
        this.addEditStudentMedicalImmunizationModel.studentMedicalImmunization.studentId = this.studentService.getStudentId();

        this.studentService.addStudentMedicalImmunization(this.addEditStudentMedicalImmunizationModel).subscribe(
          (res: AddEditStudentMedicalImmunizationModel) => {
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
      }else{
        this.addEditStudentMedicalImmunizationModel.studentMedicalImmunization.id =
        this.form.controls.id.value;
        this.addEditStudentMedicalImmunizationModel.studentMedicalImmunization.immunizationType =
        this.form.controls.immunizationType.value;
        this.addEditStudentMedicalImmunizationModel.studentMedicalImmunization.immunizationDate =
        this.form.controls.immunizationDate.value;
        this.addEditStudentMedicalImmunizationModel.studentMedicalImmunization.comment =
        this.form.controls.comment.value;
        this.addEditStudentMedicalImmunizationModel.studentMedicalImmunization.studentId = this.studentService.getStudentId();
        this.studentService.updateStudentMedicalImmunization(this.addEditStudentMedicalImmunizationModel).subscribe(
          (res: AddEditStudentMedicalImmunizationModel) => {
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
