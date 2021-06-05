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
import { fadeInUp400ms } from '../../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../../@vex/animations/stagger.animation';
import { TranslateService } from '@ngx-translate/core';
import { EditorChangeContent, EditorChangeSelection } from 'ngx-quill';
import { MatSnackBar } from '@angular/material/snack-bar';
import { StudentCommentsAddView, StudentCommentsModel } from '../../../../../models/student-comments.model';
import {StudentService} from '../../../../../services/student.service';
import { DefaultValuesService } from '../../../../../common/default-values.service';

@Component({
  selector: 'vex-edit-comment',
  templateUrl: './edit-comment.component.html',
  styleUrls: ['./edit-comment.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class EditCommentComponent implements OnInit {

  icClose = icClose;
  form: FormGroup;
  body: string = null;
  commentTitle: string;
  buttonType: string;
  currentCommnetID: any;
  studentCommentsAddView: StudentCommentsAddView = new StudentCommentsAddView();
  constructor(
    private dialogRef: MatDialogRef<EditCommentComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fb: FormBuilder,
    private snackbar: MatSnackBar,
    private studentService: StudentService,
    private defaultValuesService: DefaultValuesService,
    public translateService: TranslateService) {
      this.form = fb.group({
        commentId: [0],
        Body: ['', [Validators.required]]
      });
      if (data.information){
        this.studentCommentsAddView.studentComments = data.information;
        this.currentCommnetID = data.information.commentId;
        this.commentTitle = 'editComment';
        this.buttonType = 'update';
        this.form.controls.commentId.patchValue(data.information.commentId);
        this.form.controls.Body.patchValue(data.information.comment);
      }
      else{
        this.studentCommentsAddView.studentComments = new StudentCommentsModel();
        this.commentTitle = 'addComment';
        this.currentCommnetID = 0;
        this.buttonType = 'submit';
      }
     }

  ngOnInit(): void {
  }
  changedEditor(event: EditorChangeContent | EditorChangeSelection) {
    if (event.source === 'user') {
      this.body = document.querySelector('.ql-editor').innerHTML;
    }
  }
  submit(){
    if (this.form.controls.Body.hasError('required')){
      this.form.controls.Body.markAllAsTouched();
    }
    if (this.form.valid){
      if (this.form.controls.commentId.value === 0){
        this.studentCommentsAddView.studentComments.studentId = this.data.studentId;
        this.studentCommentsAddView.studentComments.commentId = this.currentCommnetID;
        this.studentCommentsAddView.studentComments.comment = this.form.controls.Body.value;

        this.studentService.addStudentComment(this.studentCommentsAddView).subscribe(
          (res: StudentCommentsAddView) => {
            if (res){
              if (res._failure) {
                this.snackbar.open(res._message, '', {
                  duration: 10000
                });
              }
              else {
                this.snackbar.open(res._message, '', {
                  duration: 10000
                });
                this.dialogRef.close('submited');
              }
            }
            else{
              this.snackbar.open(this.defaultValuesService.translateKey('commentFailed') + sessionStorage.getItem('httpError'), '', {
                duration: 10000
              });
            }
          }
        );
      }
      else{
        this.studentCommentsAddView.studentComments.studentId = this.data.studentId;
        this.studentCommentsAddView.studentComments.commentId = this.currentCommnetID;
        this.studentCommentsAddView.studentComments.comment = this.form.controls.Body.value;
        this.studentService.updateStudentComment(this.studentCommentsAddView).subscribe(
          (res: StudentCommentsAddView) => {
            if (res){
              if (res._failure) {
                this.snackbar.open( res._message, '', {
                  duration: 10000
                });
              }
              else {
                this.snackbar.open(res._message, '', {
                  duration: 10000
                });
                this.dialogRef.close('submited');
              }
            }
            else{
              this.snackbar.open(this.defaultValuesService.translateKey('commentFailed') + sessionStorage.getItem('httpError'), '', {
                duration: 10000
              });
            }
          }
        );
      }
    }
  }
}
