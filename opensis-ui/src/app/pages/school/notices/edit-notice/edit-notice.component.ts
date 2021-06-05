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

import { Component, EventEmitter, Inject, OnInit, Output, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import icClose from '@iconify/icons-ic/twotone-close';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { EditorChangeContent, EditorChangeSelection } from 'ngx-quill';
import { GetAllMembersList } from '../../../../models/membership.model';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { NoticeService } from '../../../../../app/services/notice.service';
import { TranslateService } from '@ngx-translate/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NoticeAddViewModel } from '../../../../models/notice.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MomentDateAdapter } from '@angular/material-moment-adapter';
import { MY_FORMATS } from '../../../../pages/shared/format-datepicker';
import { MembershipService } from '../../../../../app/services/membership.service';
import * as moment from 'moment';
import { SharedFunction} from '../../../shared/shared-function';
import { ValidationService } from '../../../shared/validation.service';
import { LoaderService } from '../../../../services/loader.service';
import { MatCheckbox } from '@angular/material/checkbox';
import { DefaultValuesService } from '../../../../common/default-values.service';

@Component({
  selector: 'vex-edit-notice',
  templateUrl: './edit-notice.component.html',
  styleUrls: ['./edit-notice.component.scss',
    '../../../../../../node_modules/quill/dist/quill.snow.css',
    '../../../../../@vex/styles/partials/plugins/_quill.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ],
  providers: [
    { provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE] },
    { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
  ]
})
export class EditNoticeComponent implements OnInit {
  @ViewChild('checkBox' ) checkBox:MatCheckbox;
  checkAll:boolean;
  AddOrEditNotice: string;
  noticeModalActionTitle='submit';
  @Output() afterClosed = new EventEmitter<boolean>();
  getAllMembersList: GetAllMembersList = new GetAllMembersList();
  icClose = icClose;
  body: string = null;
  noticeAddViewModel = new NoticeAddViewModel();
  memberArray: number[] = [];
  form: FormGroup;
  membercount: number;
  loading: boolean = false;
  schoolVisibility = false;
  constructor(private dialogRef: MatDialogRef<EditNoticeComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any,
              private router: Router, private Activeroute: ActivatedRoute, private fb: FormBuilder,
              private noticeService: NoticeService, private membershipService: MembershipService,
              public translateService: TranslateService, private snackbar: MatSnackBar,
              private commonFunction: SharedFunction,
              private loaderService: LoaderService,
              private defaultValuesService: DefaultValuesService) {
    translateService.use('en');
    this.loaderService.isLoading.subscribe((v) => {
      this.loading = v;
    });
  }



  ngOnInit(): void {
    if(this.data==null){
      this.snackbar.open('Null vallue occur. ', '', {
        duration: 10000
      });
    }
    else{
      this.membercount = this.data.membercount;
      this.getAllMembersList=this.data.allMembers;
      if(this.data.notice==null){
        this.AddOrEditNotice = 'Add Notice';
      }
      else{
        this.AddOrEditNotice = 'editNotice';
        this.noticeModalActionTitle='update';
        this.noticeAddViewModel.notice = this.data.notice;
        this.schoolVisibility = this.data.notice.visibleToAllSchool;
        this.noticeService.viewNotice(this.noticeAddViewModel).subscribe(
          (res)=>{

            this.noticeAddViewModel.notice = res.notice;
            if(this.noticeAddViewModel.notice.targetMembershipIds !== null && this.noticeAddViewModel.notice.targetMembershipIds!=''){
              let membershipIds: string[] = this.noticeAddViewModel.notice.targetMembershipIds.split(',');
              this.memberArray = membershipIds.map(Number);
              if(this.memberArray.length === this.getAllMembersList.getAllMemberList.length){
            this.checkAll=true;
          }

            }
          }
        );
      }

    }
    this.form = this.fb.group({
      Title: ['', Validators.required],
      Body: [''],
      validFrom: ['', Validators.required],
      validTo: ['', Validators.required],
      TargetMembershipIds: ['']
    });
  }
  get f() {
    return this.form.controls;
  }
  schoolVisibilityCheck(event){
    if (event.checked){
      this.schoolVisibility = true;
    }
    else{
      this.schoolVisibility = false;
    }
  }
  submitNotice() {
    this.noticeAddViewModel.notice.body = this.form.value.Body;
    this.noticeAddViewModel.notice.title = this.form.value.Title;
    this.noticeAddViewModel.notice.validFrom = this.commonFunction.formatDateSaveWithoutTime(this.form.value.validFrom);
    this.noticeAddViewModel.notice.validTo = this.commonFunction.formatDateSaveWithoutTime(this.form.value.validTo);
    this.noticeAddViewModel.notice.visibleToAllSchool = this.schoolVisibility;
    this.noticeAddViewModel.notice.targetMembershipIds = this.memberArray.toString();

    if (this.form.valid) {
      if (this.noticeAddViewModel.notice.noticeId > 0) {

          this.noticeService.updateNotice(this.noticeAddViewModel).subscribe(data => {
            if (data._failure) {
              this.snackbar.open( data._message,'', {
                duration: 10000
              });
            } else {
              this.snackbar.open( data._message,'', {
                duration: 10000
              });
              this.noticeService.changeNotice(true)
              this.dialogRef.close();
            }

          });
      }
      else {

        this.noticeAddViewModel.notice.validFrom=this.commonFunction.formatDateSaveWithoutTime(this.noticeAddViewModel.notice.validFrom);
        this.noticeAddViewModel.notice.validTo=this.commonFunction.formatDateSaveWithoutTime(this.noticeAddViewModel.notice.validTo);
        this.noticeAddViewModel.notice.createdBy= this.defaultValuesService.getUserName();
        this.noticeService.addNotice(this.noticeAddViewModel).subscribe(data => {
            if (data._failure) {
              this.snackbar.open(data._message, '', {
                duration: 10000
              });
            } else {
              this.snackbar.open(data._message,'', {
                duration: 10000
              });
              this.dialogRef.close(true);
            }
          });
      }
    }
  }

  changedEditor(event: EditorChangeContent | EditorChangeSelection) {
    if (event.source == 'user') {
      this.body = document.querySelector('.ql-editor').innerHTML;
    }
  }

  updateCheck(event) {
    if (this.memberArray.length === this.getAllMembersList.getAllMemberList.length) {
      for (let member of  this.getAllMembersList.getAllMemberList) {
        let index = this.memberArray.indexOf(member.membershipId);
        if (index > -1) {
          this.memberArray.splice(index, 1);
        }
        else {
          this.memberArray.push(member.membershipId);
        }
      }
    }
    else if (this.memberArray.length === 0) {
      for (let member of  this.getAllMembersList.getAllMemberList) {
        const index = this.memberArray.indexOf(member.membershipId);
        if (index > -1) {
          this.memberArray.splice(index, 1);
        }
        else {
          this.memberArray.push(member.membershipId);
        }
      }
    }
    else {
      for (let member of  this.getAllMembersList.getAllMemberList) {
        const index = this.memberArray.indexOf(member.membershipId);
        if (index > -1) {
          continue;
        }
        else {
          this.memberArray.push(member.membershipId);
        }
      }
    }


  }
  selectChildren(event, id) {
    event.preventDefault();
    const index = this.memberArray.indexOf(id);
    if (index > -1) {
      this.memberArray.splice(index, 1);
    }
    else {
      this.memberArray.push(id);
    }
    if(this.memberArray.length === this.getAllMembersList.getAllMemberList.length){
      this.checkBox.checked=true;
      this.checkAll=true;
    }else{
      this.checkAll=false;
      this.checkBox.checked=false;
    }
  }

  dateCompare() {

    const openingDate = this.form.controls.validFrom.value
    const closingDate = this.form.controls.validTo.value

    if (ValidationService.compareValidation(openingDate, closingDate) === false) {
      this.form.controls.validTo.setErrors({ compareError: true });

    }

  }

}
