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

import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import icClose from '@iconify/icons-ic/twotone-close';
import icEdit from '@iconify/icons-ic/edit';
import icDelete from '@iconify/icons-ic/delete';
import icMoreVertical from '@iconify/icons-ic/more-vert';
import icDone from '@iconify/icons-ic/done';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { GetAllMembersList } from '../../../../models/membership.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CalendarEventAddViewModel, colors } from '../../../../models/calendar-event.model';
import { CalendarEventService } from '../../../../services/calendar-event.service';
import { CalendarService } from '../../../../services/calendar.service';
import { ValidationService } from '../../../shared/validation.service';
import * as moment from 'moment';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmDialogComponent } from '../../../shared-module/confirm-dialog/confirm-dialog.component';
import { SharedFunction} from '../../../shared/shared-function';
import { MatCheckbox } from '@angular/material/checkbox';
import { RollBasedAccessService } from '../../../../services/roll-based-access.service';
import { RolePermissionListViewModel, RolePermissionViewModel } from '../../../../models/roll-based-access.model';
import { CryptoService } from '../../../../services/Crypto.service';
import { DefaultValuesService } from '../../../../common/default-values.service';

@Component({
  selector: 'vex-add-event',
  templateUrl: './add-event.component.html',
  styleUrls: ['./add-event.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class AddEventComponent implements OnInit {
  @ViewChild('selectAllCheckBox') selectAllCheckBox:MatCheckbox;
  @ViewChild('systemWideCheck') systemWideCheck:MatCheckbox;
  isEditMode: boolean = false;
  checkAll:boolean;
  calendarEventTitle: string;
  calendarEventActionButtonTitle="submit";
  getAllMembersList: GetAllMembersList = new GetAllMembersList();
  calendarEventAddViewModel = new CalendarEventAddViewModel();
  weekArray: number[] = [];
  memberNames : string;
  membercount: number;
  memberArray: number[] = [];
  
  colors: colors[] =[
    {name: 'Red',value: '#f44336'},
    {name: 'Orange', value: '#ff9800'},
    {name: 'Amber', value: '#ffc107'},
    {name: 'Green', value: '#4caf50'},
    {name: 'Teal', value: '#009688'},
    {name: 'Cyan', value: '#00bcd4'},
    {name: 'Purple', value: '#9c27b0'},
    {name: 'Pink', value: '#e91e63'},
    {name: 'Blue', value: '#1763b3'}
  ];
    
  icClose = icClose;
  icEdit = icEdit;
  icDelete = icDelete;
  icMoreVertical = icMoreVertical;
  icDone = icDone;
  form: FormGroup;
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();
  constructor(private dialog: MatDialog,
              private commonFunction: SharedFunction,
              private dialogRef: MatDialogRef<AddEventComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any,
              public translate: TranslateService,
              private snackbar: MatSnackBar,
              public rollBasedAccessService: RollBasedAccessService,
              private calendarService: CalendarService,
              private cryptoService: CryptoService,
              private calendarEventService: CalendarEventService,
              private fb: FormBuilder,
              private defaultValuesService: DefaultValuesService) {
    this.translate.setDefaultLang('en');
    this.form = this.fb.group({
      title: ['', Validators.required],
      startDate: ['', Validators.required],
      endDate: ['', Validators.required],
      notes: [''],
      eventColor : [''],
      systemWideEvent : [false]
    });
  }

  ngOnInit(): void {
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 2);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 3);
    this.editPermission = permissionCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionCategory.rolePermission[0].canDelete;
    this.addPermission = permissionCategory.rolePermission[0].canAdd;
    if (this.data == null) {
      this.snackbar.open('Null value occur. ', '', {
        duration: 1000
      });

    }
    else {
      this.membercount = this.data.membercount;
      this.getAllMembersList = this.data.allMembers;
      if (this.data.calendarEvent == null) {
        this.calendarEventTitle = 'addEvent';
        this.isEditMode = true;
        this.form.patchValue({ startDate: this.data.day.date });
      }
      else {
        //show event value in form 
        this.calendarEventTitle = "editEvent";
        this.calendarEventActionButtonTitle="update";
        this.calendarEventAddViewModel.schoolCalendarEvent = this.data.calendarEvent.meta.calendar;
        this.form.patchValue({ title: this.data.calendarEvent.meta.calendar.title });
        this.form.patchValue({ startDate: this.data.calendarEvent.meta.calendar.startDate });
        this.form.patchValue({ endDate: this.data.calendarEvent.meta.calendar.endDate });
        this.form.patchValue({ notes: this.data.calendarEvent.meta.calendar.description });
        this.form.patchValue({ eventColor: this.data.calendarEvent.meta.calendar.eventColor });
        this.form.patchValue({ systemWideEvent: this.data.calendarEvent.meta.calendar.systemWideEvent });
        if (this.calendarEventAddViewModel.schoolCalendarEvent.visibleToMembershipId != null && this.calendarEventAddViewModel.schoolCalendarEvent.visibleToMembershipId !='') {
          let membershipIds: string[] = this.calendarEventAddViewModel.schoolCalendarEvent.visibleToMembershipId.split(',');
          this.memberArray = membershipIds.map(Number);
         
        let foundMembers = this.data.allMembers.getAllMemberList.filter(x => this.memberArray.indexOf(x.membershipId) !== -1);
        this.memberNames = foundMembers.map(a => a.profile).join();
        if(this.memberArray.length === this.getAllMembersList.getAllMemberList.length){
          this.checkAll=true;
        }
        }

      }

    }
  }

  dateCompare() {
    let openingDate = new Date(this.form.controls.startDate.value);
    let closingDate = new Date(this.form.controls.endDate.value);

    if (ValidationService.compareValidation(openingDate, closingDate) === false) {
      this.form.controls.endDate.setErrors({ compareError: true });

    }
  }

  // edit event
  editCalendarEvent() {
    this.isEditMode = true;
  }

  // confirm delete modal
  deleteCalendarConfirm() {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: {
        title: this.defaultValuesService.translateKey('areYouSure'),
        message: this.defaultValuesService.translateKey('youAreAboutToDelete') + this.data.calendarEvent.title
      }
    });
    // listen to response
    dialogRef.afterClosed().subscribe(dialogResult => {
      // if user pressed yes dialogResult will be true, 
      // if user pressed no - it will be false
      if (dialogResult) {
        this.deleteCalendarEvent();
      }
    });
  
  }
  // delete event
  deleteCalendarEvent() {
    this.dialogRef.close();
    let id = this.data.calendarEvent.id;
    if (id > 0) {
      this.calendarEventAddViewModel.schoolCalendarEvent.eventId = id;
      this.calendarEventService.deleteCalendarEvent(this.calendarEventAddViewModel).subscribe(
        (res) => {
          if (res._failure) {
            this.snackbar.open(res._message, '', {
              duration: 10000
            });
          } else {
            this.snackbar.open(res._message, '', {
              duration: 10000
            });
            this.calendarEventService.changeEvent(true);
          }
        });

    }

  }

// save event
  submitCalendarEvent() {
    this.calendarEventAddViewModel.schoolCalendarEvent.title = this.form.value.title;
    this.calendarEventAddViewModel.schoolCalendarEvent.academicYear = +sessionStorage.getItem("academicyear");
    this.calendarEventAddViewModel.schoolCalendarEvent.description = this.form.value.notes;
    this.calendarEventAddViewModel.schoolCalendarEvent.visibleToMembershipId = this.memberArray.toString();
    this.calendarEventAddViewModel.schoolCalendarEvent.startDate = this.commonFunction.formatDateSaveWithoutTime(this.form.value.startDate);
    this.calendarEventAddViewModel.schoolCalendarEvent.endDate = this.commonFunction.formatDateSaveWithoutTime(this.form.value.endDate);
    this.calendarEventAddViewModel.schoolCalendarEvent.calendarId = this.calendarService.getCalendarId();
    this.calendarEventAddViewModel.schoolCalendarEvent.eventColor = this.form.value.eventColor;
    this.calendarEventAddViewModel.schoolCalendarEvent.systemWideEvent = this.form.value.systemWideEvent;
    if (this.form.valid) {
      if (this.calendarEventAddViewModel.schoolCalendarEvent.eventId > 0) {
        this.calendarEventService.updateCalendarEvent(this.calendarEventAddViewModel).subscribe(data => {
          if (data._failure) {
            this.snackbar.open(data._message, '', {
              duration: 10000
            });
          } else {
            this.snackbar.open(data._message, '', {
              duration: 10000
            });
            this.dialogRef.close('submitedEvent');
          }

        });
      }
      else {
        this.calendarEventService.addCalendarEvent(this.calendarEventAddViewModel).subscribe(data => {
          if (data._failure) {
            this.snackbar.open(data._message, '', {
              duration: 10000
            });
          } else {
            this.snackbar.open(data._message, '', {
              duration: 10000
            });
            this.dialogRef.close('submitedEvent');
          }
        });
      }
    }
  }

  updateCheck(event) {
    if (this.memberArray.length === this.getAllMembersList.getAllMemberList.length) {
      for (let i = 0; i < this.getAllMembersList.getAllMemberList.length; i++) {
        let index = this.memberArray.indexOf(this.getAllMembersList.getAllMemberList[i].membershipId);
        if (index > -1) {
          this.memberArray.splice(index, 1);
        }
        else {
          this.memberArray.push(this.getAllMembersList.getAllMemberList[i].membershipId);
        }
      }
    }
    else if (this.memberArray.length === 0) {
      for (let i = 0; i < this.getAllMembersList.getAllMemberList.length; i++) {
        let index = this.memberArray.indexOf(this.getAllMembersList.getAllMemberList[i].membershipId);
        if (index > -1) {
          this.memberArray.splice(index, 1);
        }
        else {
          this.memberArray.push(this.getAllMembersList.getAllMemberList[i].membershipId);
        }
      }
    }
    else {
      for (let i = 0; i < this.getAllMembersList.getAllMemberList.length; i++) {
        let index = this.memberArray.indexOf(this.getAllMembersList.getAllMemberList[i].membershipId);
        if (index > -1) {
          continue;
        }
        else {
          this.memberArray.push(this.getAllMembersList.getAllMemberList[i].membershipId);
        }
      }
    }
  }
  
  selectChildren(event, id) {
    event.preventDefault();
    let index = this.memberArray.indexOf(id);
    if (index > -1) {
      this.memberArray.splice(index, 1);
    }
    else {
      this.memberArray.push(id);
    }
    if(this.memberArray.length == this.getAllMembersList.getAllMemberList.length){
      this.checkAll=true;
      this.selectAllCheckBox.checked=true;
    }else{
      this.checkAll=false;
      this.selectAllCheckBox.checked=false;
    }

  }

  showSystemWide(event){
    if(this.systemWideCheck.checked){
         this.selectAllCheckBox.checked=false;
      this.memberArray=[];
    }else{

         this.selectAllCheckBox.checked=true;
      this.memberArray = this.getAllMembersList.getAllMemberList.map(a => a.membershipId);
    }
  }

}
