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

import { Component, Inject, OnInit, Optional } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import icClose from '@iconify/icons-ic/twotone-close';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { EffortGradeScaleModel } from '../../../../models/grades.model';
import {GradesService} from '../../../../services/grades.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'vex-edit-effort-grade-scale',
  templateUrl: './edit-effort-grade-scale.component.html',
  styleUrls: ['./edit-effort-grade-scale.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class EditEffortGradeScaleComponent implements OnInit {
  icClose = icClose;
  form: FormGroup;
  editMode:boolean;
  effortGradeScaleModel:EffortGradeScaleModel=new EffortGradeScaleModel();
  modalDialogTitle="addNewEffortGradeScale";
  modalActionButton="submit";
  gradeScaleDetailsForEdit;
  constructor(private dialogRef: MatDialogRef<EditEffortGradeScaleComponent>,
    private fb: FormBuilder,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: any,
    private gradesService:GradesService,
    private snackbar: MatSnackBar
    ) {
      if(this.data.editMode){
        this.editMode = this.data.editMode;
        this.gradeScaleDetailsForEdit=this.data.gradeScaleDetails
        this.modalActionButton="update";
        this.modalDialogTitle="updateEffortGradeScale"
       }else{
        this.editMode = this.data.editMode;
       }
     }

  ngOnInit(): void {
    this.form = this.fb.group(
      {
        gradeScaleValue: ['',[Validators.required,Validators.min(1)]],
        gradeScaleComment: ['',Validators.required],
      });
    if(this.editMode){
      this.patchDataToForm();
    }
      
  }

  patchDataToForm(){
    this.form.patchValue({
      gradeScaleValue:this.gradeScaleDetailsForEdit.gradeScaleValue,
      gradeScaleComment:this.gradeScaleDetailsForEdit.gradeScaleComment,
    })
  }

  submit(){
    this.form.markAllAsTouched();
    if( this.form.invalid ){ return; }
     if(this.editMode){
       this.updateEffortGradeScale();
     }else{
       this.addEffortGradeScale();
     }
  }

  updateEffortGradeScale(){
    this.effortGradeScaleModel.effortGradeScale.effortGradeScaleId=this.gradeScaleDetailsForEdit.effortGradeScaleId;
    this.effortGradeScaleModel.effortGradeScale.gradeScaleValue=this.form.value.gradeScaleValue;
    this.effortGradeScaleModel.effortGradeScale.gradeScaleComment=this.form.value.gradeScaleComment;
    this.gradesService.updateEffortGradeScale(this.effortGradeScaleModel).subscribe((res)=>{
     if (typeof (res) == 'undefined') {
       this.snackbar.open('Failed to Update Effort Grade Scale ' + sessionStorage.getItem("httpError"), '', {
         duration: 10000
       });
     }else
     if (res._failure) {
       this.snackbar.open('Failed to Update Effort Grade Scale  ' + res._message, '', {
         duration: 10000
       });
     } else {
       this.snackbar.open('Effort Grade Scale Updated Successfully.', '', {
         duration: 10000
       });
       this.dialogRef.close(true);
     }
    })
  }

  addEffortGradeScale(){
    this.effortGradeScaleModel.effortGradeScale.gradeScaleValue=this.form.value.gradeScaleValue;
     this.effortGradeScaleModel.effortGradeScale.gradeScaleComment=this.form.value.gradeScaleComment;
     this.gradesService.addEffortGradeScale(this.effortGradeScaleModel).subscribe((res)=>{
      if (typeof (res) == 'undefined') {
        this.snackbar.open('Failed to Add Effort Grade Scale ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }else
      if (res._failure) {
        this.snackbar.open('Failed to Add Effort Grade Scale  ' + res._message, '', {
          duration: 10000
        });
      } else {
        this.snackbar.open('Effort Grade Scale Added Successfully.', '', {
          duration: 10000
        });
        this.dialogRef.close(true);
      }
     })
  }

}
