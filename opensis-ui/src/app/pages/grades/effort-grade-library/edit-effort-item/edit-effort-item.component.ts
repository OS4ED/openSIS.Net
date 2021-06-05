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
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import icClose from '@iconify/icons-ic/twotone-close';
import { EffortGradeLibraryCategoryItemAddViewModel } from '../../../../models/grades.model';
import { GradesService } from '../../../../services/grades.service';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { ValidationService } from 'src/app/pages/shared/validation.service';

@Component({
  selector: 'vex-edit-effort-item',
  templateUrl: './edit-effort-item.component.html',
  styleUrls: ['./edit-effort-item.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class EditEffortItemComponent implements OnInit {
  icClose = icClose;
  form: FormGroup;
  effortGradeLibraryCategoryItemAddViewModel:EffortGradeLibraryCategoryItemAddViewModel=new EffortGradeLibraryCategoryItemAddViewModel()
  effortCategoryId: number;
  effortCategoryItemTitle: string;
  buttonType: string;

  constructor(
    private dialogRef: MatDialogRef<EditEffortItemComponent>, 
    private fb: FormBuilder, 
    @Inject(MAT_DIALOG_DATA) public data:any,
    private snackbar:MatSnackBar,
    private gradesService:GradesService
    ) { 
      this.form=fb.group({
        effortItemId:[0],
        effortItemTitle:["",[ValidationService.noWhitespaceValidator]]
      })
      if(data.information==null){
        this.effortCategoryId=data.effortCategoryId
        this.effortCategoryItemTitle="addNewEffortItem";
        this.buttonType="submit";
      }
      else{
        this.buttonType="update";
        this.effortCategoryItemTitle="editEffortItem";
        this.effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.effortCategoryId=data.information.effortCategoryId
        this.form.controls.effortItemId.patchValue(data.information.effortItemId);
        this.form.controls.effortItemTitle.patchValue(data.information.effortItemTitle);
      }
    }

  ngOnInit(): void {
  }
  submit(){
    this.form.markAllAsTouched();
    if(this.form.valid){
      if(this.form.controls.effortItemId.value==0){
        this.effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.effortCategoryId=this.effortCategoryId;
        this.effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.effortItemTitle=this.form.controls.effortItemTitle.value
        this.gradesService.addEffortGradeLibraryCategoryItem(this.effortGradeLibraryCategoryItemAddViewModel).subscribe(
          (res:EffortGradeLibraryCategoryItemAddViewModel)=>{
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
        this.effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.effortItemId=this.form.controls.effortItemId.value
        this.effortGradeLibraryCategoryItemAddViewModel.effortGradeLibraryCategoryItem.effortItemTitle=this.form.controls.effortItemTitle.value
        this.gradesService.updateEffortGradeLibraryCategoryItem(this.effortGradeLibraryCategoryItemAddViewModel).subscribe(
          (res:EffortGradeLibraryCategoryItemAddViewModel)=>{
            if(typeof(res)=='undefined'){
              this.snackbar.open('' + sessionStorage.getItem("httpError"), '', {
                duration: 10000
              });
            }else{
              if (res._failure) {
                this.snackbar.open('' + res._message, '', {
                  duration: 10000
                });
              } 
              else{
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
