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
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import icClose from '@iconify/icons-ic/twotone-close';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { AttendanceCodeService } from '../../../../services/attendance-code.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AttendanceCodeCategoryModel } from '../../../../models/attendance-code.model';

@Component({
  selector: 'vex-attendance-category',
  templateUrl: './attendance-category.component.html',
  styleUrls: ['./attendance-category.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class AttendanceCategoryComponent implements OnInit {
  icClose = icClose;
  form: FormGroup;
  attendanceCategoryModel:AttendanceCodeCategoryModel=new AttendanceCodeCategoryModel();
  editMode:boolean=false;
  editDetails;
  attendanceCategoryModalTitle="addAttendanceCategory";
  attendanceCategoryModalActionButton="submit";
  constructor(
    private dialogRef: MatDialogRef<AttendanceCategoryComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: any,
    private attendanceCodeService:AttendanceCodeService,
    private fb: FormBuilder,
    private snackbar: MatSnackBar) {
      if(data!=null && data!=undefined){
        this.editMode=data.editMode;
        this.editDetails=data.categoryDetails;
      }else{
        this.editMode=false;
      }
    this.form = this.fb.group({
      title:[null,Validators.required]
    });
   }

   submit(){
     if(this.editMode){
      this.updateAttendanceCategory();
     }else{
      this.addAttendanceCategory();
     }
    
  }

  ngOnInit(): void {
    if(this.editMode){
      this.attendanceCategoryModalTitle="editAttendanceCategory";
      this.attendanceCategoryModalActionButton="update"
      this.form.patchValue({
        title:this.editDetails.title
      })
    }
  }

  // Add Attendance Category
  addAttendanceCategory() {
    if(this.form.valid && this.form.value.title!=''){
    this.attendanceCategoryModel.attendanceCodeCategories.title=this.form.value.title;
    this.attendanceCategoryModel.attendanceCodeCategories.academicYear=+sessionStorage.getItem("academicyear");
    this.attendanceCodeService.addAttendanceCodeCategories(this.attendanceCategoryModel).subscribe((res)=>{
      if (typeof (res) == 'undefined') {
        this.snackbar.open( sessionStorage.getItem('httpError'), '', {
          duration: 10000
        });
      }else if (res._failure) {
        this.snackbar.open(res._message, '', {
          duration: 10000
        });
      } else {
        this.snackbar.open(res._message, '', {
          duration: 10000
        });
        this.dialogRef.close(true);
      }
    });
    }
  }

    // Update Attendance Category
    updateAttendanceCategory() {
    if(this.form.valid && this.form.value.title!=''){
      this.attendanceCategoryModel.attendanceCodeCategories.attendanceCategoryId=this.editDetails.attendanceCategoryId;
      this.attendanceCategoryModel.attendanceCodeCategories.academicYear=this.editDetails.academicYear;
      this.attendanceCategoryModel.attendanceCodeCategories.title=this.form.value.title;
      this.attendanceCodeService.updateAttendanceCodeCategories(this.attendanceCategoryModel).subscribe((res)=>{
        if (typeof (res) == 'undefined') {
          this.snackbar.open('Attendance Category is Failed to Update!. ' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }else if (res._failure) {
          this.snackbar.open(res._message, '', {
            duration: 10000
          });
        } else {
          this.snackbar.open(res._message, '', {
            duration: 10000
          });
          this.dialogRef.close(true);
        }
      });
    }
  }

}
