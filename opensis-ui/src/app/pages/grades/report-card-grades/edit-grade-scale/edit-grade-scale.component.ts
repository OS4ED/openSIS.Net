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
import { TranslateService } from '@ngx-translate/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { GradeScaleAddViewModel } from '../../../../models/grades.model';
import { GradesService } from 'src/app/services/grades.service';
import { ValidationService } from 'src/app/pages/shared/validation.service';

@Component({
  selector: 'vex-edit-grade-scale',
  templateUrl: './edit-grade-scale.component.html',
  styleUrls: ['./edit-grade-scale.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class EditGradeScaleComponent implements OnInit {
  icClose = icClose;
  form: FormGroup;
  gradeScaleTitle: string;
  buttonType: string;
  gradeScaleAddViewModel:GradeScaleAddViewModel=new GradeScaleAddViewModel();

  constructor(
    private dialogRef: MatDialogRef<EditGradeScaleComponent>,
    private fb: FormBuilder, 
    @Inject(MAT_DIALOG_DATA) public data:any,
    private snackbar:MatSnackBar,
    private gradesService:GradesService,
    public translateService: TranslateService
     ) {
       this.form=fb.group(
         {
          gradeScaleId:[0],
          gradeScaleName:['',[Validators.required,ValidationService.noWhitespaceValidator]],
          gradeScaleValue:['',[Validators.required,ValidationService.noWhitespaceValidator]],
          gradeScaleComment:[],
          calculateGpa:[false]
         }
       )
       if(data==null){
        this.gradeScaleTitle="addGradeScale";
        this.buttonType="submit";
      }
      else{
        this.buttonType="update";
        this.gradeScaleTitle="editGradeScale";
        this.form.controls.gradeScaleId.patchValue(this.data.gradeScaleId)
        this.form.controls.gradeScaleName.patchValue(this.data.gradeScaleName)
        this.form.controls.gradeScaleValue.patchValue(this.data.gradeScaleValue)
        this.form.controls.gradeScaleComment.patchValue(this.data.gradeScaleComment)
        this.form.controls.calculateGpa.patchValue(this.data.calculateGpa)
      }
  }

  ngOnInit(): void {
  }

  

  

  submit(){
    this.form.markAllAsTouched();
    if(this.form.valid){
      if(this.form.controls.gradeScaleId.value==0){
        this.gradeScaleAddViewModel.gradeScale.gradeScaleId=this.form.controls.gradeScaleId.value;
        this.gradeScaleAddViewModel.gradeScale.gradeScaleName=this.form.controls.gradeScaleName.value;
        this.gradeScaleAddViewModel.gradeScale.gradeScaleValue=this.form.controls?.gradeScaleValue?.value;
        this.gradeScaleAddViewModel.gradeScale.gradeScaleComment=this.form.controls.gradeScaleComment.value;
        this.gradeScaleAddViewModel.gradeScale.calculateGpa=this.form.controls.calculateGpa.value;
        this.gradeScaleAddViewModel.gradeScale.useAsStandardGradeScale=false;
        this.gradesService.addGradeScale(this.gradeScaleAddViewModel).subscribe(
          (res:GradeScaleAddViewModel)=>{
            if(typeof(res)=='undefined'){
              this.snackbar.open('' + sessionStorage.getItem("httpError"), '', {
                duration: 10000
              });
            }
            else{
              if (res._failure) {
                this.snackbar.open('' + res._message, '', {
                  duration: 10000
                });
              } 
              else { 
                this.snackbar.open(''+res._message, '', {
                  duration: 10000
                }); 
                this.dialogRef.close('submited');
              }
            }
          }
        );
      }
      else{
        
        this.gradeScaleAddViewModel.gradeScale.gradeScaleId=this.form.controls.gradeScaleId.value;
        this.gradeScaleAddViewModel.gradeScale.gradeScaleName=this.form.controls.gradeScaleName.value;
        this.gradeScaleAddViewModel.gradeScale.gradeScaleValue=this.form.controls?.gradeScaleValue?.value;
        this.gradeScaleAddViewModel.gradeScale.gradeScaleComment=this.form.controls.gradeScaleComment.value;
        this.gradeScaleAddViewModel.gradeScale.calculateGpa=this.form.controls.calculateGpa.value;
        this.gradeScaleAddViewModel.gradeScale.useAsStandardGradeScale=false;
        
        this.gradesService.updateGradeScale(this.gradeScaleAddViewModel).subscribe(
          (res:GradeScaleAddViewModel)=>{
            if(typeof(res)=='undefined'){
              this.snackbar.open('' + sessionStorage.getItem("httpError"), '', {
                duration: 10000
              });
            }  
            else{
              if (res._failure) {
                this.snackbar.open(''+res._message, '', {
                  duration: 10000
                });
              } 
              else { 
                this.snackbar.open(''+res._message, '', {
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
