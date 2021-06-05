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

import { LovAddView } from '../../../../models/lov.model';
import { CommonService } from './../../../../services/common.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import icClose from '@iconify/icons-ic/twotone-close';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ValidationService } from 'src/app/pages/shared/validation.service';

@Component({
  selector: 'vex-edit-school-level',
  templateUrl: './edit-school-level.component.html',
  styleUrls: ['./edit-school-level.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class EditSchoolLevelComponent implements OnInit {
  icClose = icClose;
  form:FormGroup;
  schoolLevelTitle: string;
  lovAddView:LovAddView=new LovAddView()
  buttonType:string

  constructor(
    private dialogRef: MatDialogRef<EditSchoolLevelComponent>,
    @Inject(MAT_DIALOG_DATA) public data:any,
    private snackbar:MatSnackBar,
    private commonService:CommonService,
    fb:FormBuilder
    ) {
      this.form=fb.group({
        id:[0],
        lovName:["School Level"],
        lovColumnValue:['',[ValidationService.noWhitespaceValidator]],
      })
      if(data==null){
        this.schoolLevelTitle="addSchoolLevel";
        this.buttonType="submit";
      }
      else{
        
       // this.lovAddView.dropdownValue=data;
        this.schoolLevelTitle="editSchoolLevel";
        this.buttonType="update";
        this.form.controls.id.patchValue(data.id)
        this.form.controls.lovColumnValue.patchValue(data.lovColumnValue)
        this.form.controls.lovName.patchValue(data.lovName)
      }
     }

  ngOnInit(): void {
  }
  submit(){
    this.form.markAsTouched();
    if (this.form.valid) { 
      if(this.form.controls.id.value==0){
        this.lovAddView.dropdownValue.lovColumnValue=this.form.controls.lovColumnValue.value;
        this.lovAddView.dropdownValue.lovName=this.form.controls.lovName.value;
        this.lovAddView.dropdownValue.createdBy=sessionStorage.getItem("email");
        this.commonService.addDropdownValue(this.lovAddView).subscribe(
          (res:LovAddView)=>{
            if(typeof(res)=='undefined'){
              this.snackbar.open('School Level insertion failed. ' + sessionStorage.getItem("httpError"), '', {
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
                this.snackbar.open('' + res._message, '', {
                  duration: 10000
                });
                this.dialogRef.close('submited');
              }
            }
          }
        );
      }
      else{
        this.lovAddView.dropdownValue.id=this.form.controls.id.value;
        this.lovAddView.dropdownValue.lovName=this.form.controls.lovName.value;
        this.lovAddView.dropdownValue.lovColumnValue=this.form.controls.lovColumnValue.value;
        this.lovAddView.dropdownValue.updatedBy=sessionStorage.getItem("email");
        this.commonService.updateDropdownValue(this.lovAddView).subscribe(
          (res:LovAddView)=>{
            if(typeof(res)=='undefined'){
              this.snackbar.open('School Level Update failed. ' + sessionStorage.getItem("httpError"), '', {
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
               
                this.snackbar.open('' + res._message, '', {
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