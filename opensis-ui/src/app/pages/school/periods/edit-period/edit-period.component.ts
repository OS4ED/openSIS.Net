import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import icClose from '@iconify/icons-ic/twotone-close';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { TranslateService } from '@ngx-translate/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SchoolPeriodService } from '../../../../services/school-period.service';
import { ValidationService } from '../../../shared/validation.service';
import { BlockPeriodAddViewModel } from '../../../../models/schoolPeriodModel';
import { getTime } from 'date-fns';
import { SharedFunction } from '../../../shared/shared-function';

export const dateTimeCustomFormats = {
};

@Component({
  selector: 'vex-edit-period',
  templateUrl: './edit-period.component.html',
  styleUrls: ['./edit-period.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class EditPeriodComponent implements OnInit {
  icClose = icClose;
  form: FormGroup;
  buttonType: string;
  periodHeaderTitle: string;
  currentBlockId: number = null;
  blockPeriodAddViewModel: BlockPeriodAddViewModel = new BlockPeriodAddViewModel();

  constructor(private dialogRef: MatDialogRef<EditPeriodComponent>,
    public commonfunction:SharedFunction,
    public translateService: TranslateService,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fb: FormBuilder,
    private snackbar: MatSnackBar,
    private schoolPeriodService: SchoolPeriodService) {
    translateService.use('en');
    this.form = fb.group({
      periodId: [0],
      title: ['', [Validators.required, ValidationService.noWhitespaceValidator]],
      shortName: ['', [Validators.required,ValidationService.noWhitespaceValidator]],
      startTime: ['', [Validators.required]],
      endTime: [, [Validators.required]],
      calculateAttendance: [false]
    })
    if (data.periodData == null) {
      this.currentBlockId = data.blockId;
      this.periodHeaderTitle = "addNewPeriod";
      this.buttonType = "submit";
    }
    else {
      this.buttonType = "update";
      this.periodHeaderTitle = "editPeriod";
      this.blockPeriodAddViewModel.blockPeriod = data.periodData;
      this.currentBlockId = data.periodData.blockId;
      this.form.controls.periodId.patchValue(data.periodData.periodId);
      this.form.controls.title.patchValue(data.periodData.periodTitle);
      this.form.controls.shortName.patchValue(data.periodData.periodShortName);
      this.form.controls.startTime.patchValue(new Date(data.periodData.periodStartTime));
      this.form.controls.endTime.patchValue(new Date(data.periodData.periodEndTime));
      this.form.controls.calculateAttendance.patchValue(data.periodData.calculateAttendance);
    }

  }

  ngOnInit(): void {
  }

  submit() {
    this.form.markAllAsTouched();
    if (this.form.valid) {
      if (this.form.controls.periodId.value == 0) {
        this.blockPeriodAddViewModel.blockPeriod.blockId = this.currentBlockId;
        this.blockPeriodAddViewModel.blockPeriod.periodTitle = this.form.controls.title.value;
        this.blockPeriodAddViewModel.blockPeriod.calculateAttendance = this.form.controls.calculateAttendance.value;
        this.blockPeriodAddViewModel.blockPeriod.periodShortName = this.form.controls.shortName.value;
        this.blockPeriodAddViewModel.blockPeriod.periodStartTime = this.form.controls.startTime.value.toString().substr(16, 5);
        this.blockPeriodAddViewModel.blockPeriod.periodEndTime = this.form.controls.endTime.value.toString().substr(16, 5);
        this.blockPeriodAddViewModel.blockPeriod.createdBy = sessionStorage.getItem('email');
        this.schoolPeriodService.addBlockPeriod(this.blockPeriodAddViewModel).subscribe(
          (res: BlockPeriodAddViewModel) => {
            if (typeof (res) == 'undefined') {
              this.snackbar.open('Period Creation failed. ' + sessionStorage.getItem("httpError"), '', {
                duration: 10000
              });
            }
            else {
              if (res._failure) {
                this.snackbar.open('Period Creation failed. ' + res._message, '', {
                  duration: 10000
                });
              }
              else {
                this.snackbar.open('Period Successfully Created', '', {
                  duration: 10000
                });
                this.dialogRef.close('submited');
              }
            }
          }
        )

      }
      else {
        this.blockPeriodAddViewModel.blockPeriod.schoolId = +sessionStorage.getItem('selectedSchoolId');
        this.blockPeriodAddViewModel.blockPeriod.tenantId = sessionStorage.getItem('tenantId');
        this.blockPeriodAddViewModel.blockPeriod.periodId = this.form.controls.periodId.value;
        this.blockPeriodAddViewModel.blockPeriod.blockId = this.currentBlockId;
        this.blockPeriodAddViewModel.blockPeriod.periodTitle = this.form.controls.title.value;
        this.blockPeriodAddViewModel.blockPeriod.calculateAttendance = this.form.controls.calculateAttendance.value;
        this.blockPeriodAddViewModel.blockPeriod.periodShortName = this.form.controls.shortName.value;
        this.blockPeriodAddViewModel.blockPeriod.periodStartTime = this.form.controls.startTime.value.toString().substr(16, 5);
        this.blockPeriodAddViewModel.blockPeriod.periodEndTime = this.form.controls.endTime.value.toString().substr(16, 5);
        this.blockPeriodAddViewModel.blockPeriod.updatedBy = sessionStorage.getItem('email');
        this.schoolPeriodService.updateBlockPeriod(this.blockPeriodAddViewModel).subscribe(
          (res: BlockPeriodAddViewModel) => {
            if (typeof (res) == 'undefined') {
              this.snackbar.open('Period Updation failed. ' + sessionStorage.getItem("httpError"), '', {
                duration: 10000
              });
            }
            else {
              if (res._failure) {
                this.snackbar.open('Period Updation failed. ' + res._message, '', {
                  duration: 10000
                });
              }
              else {
                this.snackbar.open('' +res._message+ '', '', {
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
