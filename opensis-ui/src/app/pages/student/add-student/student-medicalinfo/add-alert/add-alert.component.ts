
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
import { TranslateService } from '@ngx-translate/core';
import icClose from '@iconify/icons-ic/twotone-close';
import { StudentService } from 'src/app/services/student.service';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ValidationService } from '../../../../shared/validation.service';
import { AddEditStudentMedicalAlertModel } from '../../../../../models/student.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'vex-add-alert',
  templateUrl: './add-alert.component.html',
  styleUrls: ['./add-alert.component.scss']
})
export class AddAlertComponent implements OnInit {

  icClose = icClose;
  form: FormGroup;
  addEditStudentMedicalAlertModel: AddEditStudentMedicalAlertModel = new AddEditStudentMedicalAlertModel();
  editMode;
  alertTitle;
  buttonType;
  constructor(
    private dialogRef: MatDialogRef<AddAlertComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public translateService: TranslateService,
    private fb: FormBuilder,
    private snackbar: MatSnackBar,
    private studentService: StudentService) {
    translateService.use('en');
    this.form = fb.group({
      id: [0],
      alertType: ['', [ValidationService.noWhitespaceValidator]],
      alertDescription: ['']
    });
    if (this.data == null){
      this.editMode = false;
      this.buttonType = 'submit';
      this.alertTitle = 'addAlertInformation';
    }
    else{
      this.editMode = true;
      this.buttonType = 'update';
      this.alertTitle = 'editAlertInformation';
      this.form.controls.id.patchValue(this.data.id);
      this.form.controls.alertType.patchValue(this.data.alertType);
      this.form.controls.alertDescription.patchValue(this.data.alertDescription);
    }
  }

  ngOnInit(): void {
  }
  submit(){
    this.form.markAsTouched();
    if (this.form.valid){
      if ( this.form.controls.id.value === 0){
        this.addEditStudentMedicalAlertModel.studentMedicalAlert.studentId = this.studentService.getStudentId();
        this.addEditStudentMedicalAlertModel.studentMedicalAlert.alertType = this.form.controls.alertType.value;
        this.addEditStudentMedicalAlertModel.studentMedicalAlert.alertDescription = this.form.controls.alertDescription.value;
        this.studentService.addStudentMedicalAlert(this.addEditStudentMedicalAlertModel).subscribe(
          (res: AddEditStudentMedicalAlertModel) => {
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
        this.addEditStudentMedicalAlertModel.studentMedicalAlert.studentId = this.studentService.getStudentId();
        this.addEditStudentMedicalAlertModel.studentMedicalAlert.id = this.form.controls.id.value;
        this.addEditStudentMedicalAlertModel.studentMedicalAlert.alertType = this.form.controls.alertType.value;
        this.addEditStudentMedicalAlertModel.studentMedicalAlert.alertDescription = this.form.controls.alertDescription.value;
        this.studentService.updateStudentMedicalAlert(this.addEditStudentMedicalAlertModel).subscribe(
          (res: AddEditStudentMedicalAlertModel) => {
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
