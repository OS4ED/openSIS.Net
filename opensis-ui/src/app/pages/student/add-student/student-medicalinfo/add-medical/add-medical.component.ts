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
import { StudentService } from '../../../../../services/student.service';
import { FormGroup, FormBuilder } from '@angular/forms';
import { AddEditStudentMedicalNoteModel } from '../../../../../models/student.model';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'vex-add-medical',
  templateUrl: './add-medical.component.html',
  styleUrls: ['./add-medical.component.scss']
})
export class AddMedicalComponent implements OnInit {

  icClose = icClose;
  form: FormGroup;
  addEditStudentMedicalNoteModel: AddEditStudentMedicalNoteModel = new AddEditStudentMedicalNoteModel();
  buttonType: string;
  editMode: boolean;
  medicalComponentTitle: string;

  constructor(
    private dialogRef: MatDialogRef<AddMedicalComponent>,
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
      noteDate: [],
      medicalNote: [],
    });
    if (this.data == null){
      this.editMode = false;
      this.buttonType = 'submit';
      this.medicalComponentTitle = 'addMedicalNotes';
    }
    else{
      this.editMode = true;
      this.buttonType = 'update';
      this.medicalComponentTitle = 'editMedicalNotes';
      this.form.controls.id.patchValue(this.data.id);
      this.form.controls.noteDate.patchValue(this.data.noteDate);
      this.form.controls.medicalNote.patchValue(this.data.medicalNote);
    }
  }
  submit(){
    this.form.markAsTouched();
    if (this.form.valid){
      if (this.form.controls.id.value === 0){
        this.addEditStudentMedicalNoteModel.studentMedicalNote.noteDate = this.form.controls.noteDate.value;
        this.addEditStudentMedicalNoteModel.studentMedicalNote.medicalNote = this.form.controls.medicalNote.value;
        this.addEditStudentMedicalNoteModel.studentMedicalNote.studentId = this.studentService.getStudentId();
        this.studentService.addStudentMedicalNote(this.addEditStudentMedicalNoteModel).subscribe(
          (res: AddEditStudentMedicalNoteModel) => {
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
        this.addEditStudentMedicalNoteModel.studentMedicalNote.id = this.form.controls.id.value;
        this.addEditStudentMedicalNoteModel.studentMedicalNote.noteDate = this.form.controls.noteDate.value;
        this.addEditStudentMedicalNoteModel.studentMedicalNote.medicalNote = this.form.controls.medicalNote.value;
        this.addEditStudentMedicalNoteModel.studentMedicalNote.studentId = this.studentService.getStudentId();
        this.studentService.updateStudentMedicalNote(this.addEditStudentMedicalNoteModel).subscribe(
          (res: AddEditStudentMedicalNoteModel) => {
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
