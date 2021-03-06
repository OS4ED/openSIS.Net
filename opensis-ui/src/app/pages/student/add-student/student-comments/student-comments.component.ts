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

import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { fadeInRight400ms } from '../../../../../@vex/animations/fade-in-right.animation';
import { TranslateService } from '@ngx-translate/core';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icDelete from '@iconify/icons-ic/twotone-delete';
import icAdd from '@iconify/icons-ic/baseline-add';
import icPrint from '@iconify/icons-ic/baseline-print';
import icComment from '@iconify/icons-ic/twotone-comment';
import { MatDialog } from '@angular/material/dialog';
import { EditCommentComponent } from './edit-comment/edit-comment.component';
import {StudentService} from '../../../../services/student.service';
import {ExcelService} from '../../../../services/excel.service';
import {StudentCommentsListViewModel, StudentCommentsAddView} from '../../../../models/student-comments.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ConfirmDialogComponent } from '../../../../pages/shared-module/confirm-dialog/confirm-dialog.component';
import { SchoolCreate } from '../../../../enums/school-create.enum';
import { SharedFunction } from '../../../../pages/shared/shared-function';
import { DatePipe } from '@angular/common';
import { RolePermissionListViewModel, RolePermissionViewModel } from '../../../../models/roll-based-access.model';
import { CryptoService } from '../../../../services/Crypto.service';
import { DefaultValuesService } from '../../../../common/default-values.service';


@Component({
  selector: 'vex-student-comments',
  templateUrl: './student-comments.component.html',
  styleUrls: ['./student-comments.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms,
    fadeInRight400ms
  ],
  providers: [DatePipe]
})
export class StudentCommentsComponent implements OnInit {

  icEdit = icEdit;
  icDelete = icDelete;
  icAdd = icAdd;
  icComment = icComment;
  icPrint = icPrint;
  listCount;
  StudentCreate = SchoolCreate;
  studentCreateMode: SchoolCreate;
  studentDetailsForViewAndEdit;
  studentCommentsListViewModel: StudentCommentsListViewModel = new StudentCommentsListViewModel();
  studentCommentsAddView: StudentCommentsAddView = new StudentCommentsAddView();
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();

  constructor(
    private fb: FormBuilder,
    private dialog: MatDialog,
    public translateService: TranslateService,
    private snackbar: MatSnackBar,
    private studentService: StudentService,
    private commonFunction: SharedFunction,
    private excelService: ExcelService,
    private datePipe: DatePipe,
    private defaultValuesService: DefaultValuesService,
    private cryptoService: CryptoService
    ) {
    translateService.use('en');
  }

  ngOnInit(): void {

    this.studentService.studentCreatedMode.subscribe((res)=>{
      this.studentCreateMode = res;
    })
    this.studentService.studentDetailsForViewedAndEdited.subscribe((res)=>{
      this.studentDetailsForViewAndEdit = res;
    })
 

    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 3);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 5);
    const permissionSubCategory = permissionCategory.permissionSubcategory.find( x => x.permissionSubcategoryId === 8);
    this.editPermission = permissionSubCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionSubCategory.rolePermission[0].canDelete;
    this.addPermission = permissionSubCategory.rolePermission[0].canAdd;
    this.getAllComments();
  }

  openAddNew() {
    this.dialog.open(EditCommentComponent, {
      data: {studentId: this.studentDetailsForViewAndEdit.studentMaster.studentId},
      width: '800px'
    }).afterClosed().subscribe((data) => {
      if (data === 'submited'){
        this.getAllComments();
      }
    });
  }
  getAllComments(){
    this.studentCommentsListViewModel.studentId = this.studentDetailsForViewAndEdit.studentMaster.studentId;
    this.studentService.getAllStudentCommentsList(this.studentCommentsListViewModel).subscribe(
      (res: StudentCommentsListViewModel) => {
        if (res){
          if (res._failure) {
            if (res.studentCommentsList === null){
              this.listCount = null;
              this.studentCommentsListViewModel.studentCommentsList = null ;
              this.snackbar.open( res._message, '', {
                duration: 10000
              });
            }else{
              this.listCount = null;
              this.studentCommentsListViewModel.studentCommentsList = null ;
            }
        }
        else {
          this.studentCommentsListViewModel.studentCommentsList = res.studentCommentsList;
          this.listCount = res.studentCommentsList.length;
          this.studentCommentsListViewModel.studentCommentsList.map( n => {
            n.lastUpdated = this.commonFunction.serverToLocalDateAndTime(n.lastUpdated);
          });
        }
        }else{
          this.snackbar.open(this.defaultValuesService.translateKey('studentCommentsNotFound') + sessionStorage.getItem('httpError'), '', {
            duration: 10000
          });
        }
      }
    );
  }

  exportCommentsToExcel(){
    if (this.studentCommentsListViewModel.studentCommentsList?.length > 0 || this.studentCommentsListViewModel.studentCommentsList!=null){
      const commentList = this.studentCommentsListViewModel.studentCommentsList?.map((item) => {
        return{
                   Comment: this.stripHtml(item.comment),
                   UpdatedBy: item.updatedBy,
                   LastUpdated: this.datePipe.transform(item.lastUpdated, 'MMM d, y, h:mm a')
        };
      });
      this.excelService.exportAsExcelFile(commentList, 'Comments_');
     }else{
       this.snackbar.open(this.defaultValuesService.translateKey('noRecordsFoundFailedtoExportComments'), '', {
         duration: 5000
       });
     }
  }

  stripHtml(html){
    // Create a new div element
    const temporalDivElement = document.createElement('div');
    // Set the HTML content with the providen
    temporalDivElement.innerHTML = html;
    // Retrieve the text property of the element (cross-browser support)
    return temporalDivElement.textContent || temporalDivElement.innerText || '';
}

  confirmDelete(element){
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: '400px',
      data: {
          title: this.defaultValuesService.translateKey('areYouSure'),
          message: this.defaultValuesService.translateKey('youAreAboutToDelete') + element.title + '.'}
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult){
        this.deleteStudentComment(element);
      }
   });
  }
  deleteStudentComment(element){
    this.studentCommentsAddView.studentComments = element;
    this.studentService.deleteStudentComment(this.studentCommentsAddView).subscribe(
      (res: StudentCommentsAddView) => {
        if (res){
          if (res._failure) {
            this.snackbar.open( res._message, '', {
              duration: 10000
            });
          }
          else {
            this.getAllComments();
          }
        }
        else{
          this.snackbar.open( this.defaultValuesService.translateKey('studentCommentsNotFound') + sessionStorage.getItem('httpError'), '', {
            duration: 10000
          });
        }
      }
    );
  }
  editComment(element){
    this.dialog.open(EditCommentComponent, {
      data: {studentId: this.studentDetailsForViewAndEdit.studentMaster.studentId, information: element},
      width: '800px'
    }).afterClosed().subscribe((data) => {
      if (data === 'submited'){
        this.getAllComments();
      }
    });
  }
}
