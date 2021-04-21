import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import icClose from '@iconify/icons-ic/twotone-close';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { TranslateService } from '@ngx-translate/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CommonService } from '../../../../services/common.service';
import { LovAddView } from '../../../../models/lov.model';

@Component({
  selector: 'vex-edit-race',
  templateUrl: './edit-race.component.html',
  styleUrls: ['./edit-race.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class EditRaceComponent implements OnInit {
  icClose = icClose;
  raceAddViewModel: LovAddView = new LovAddView();
  form: FormGroup;
  editMode = false;
  raceTitle: string;

  constructor(
    private dialogRef: MatDialogRef<EditRaceComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public translateService: TranslateService,
    private fb: FormBuilder,
    private snackbar: MatSnackBar,
    private commonService: CommonService) {
    translateService.use('en');
    this.form = this.fb.group({
      id: [0],
      lovColumnValue: ['', [Validators.required]]
    })

    if (data == null) {
      this.raceTitle = "addRace";
    }
    else {
      this.editMode = true;
      this.raceTitle = "editRace";
    //  this.raceAddViewModel.dropdownValue = data;
      this.form.controls.id.patchValue(data.id)
      this.form.controls.lovColumnValue.patchValue(data.lovColumnValue)

    }

  }

  ngOnInit(): void {
  }
  submit() {
    if (this.form.valid) {
      if (this.form.controls.id.value == 0) {
        this.raceAddViewModel.dropdownValue.lovColumnValue = this.form.controls.lovColumnValue.value;
        this.raceAddViewModel.dropdownValue.lovName = "Race";
        this.raceAddViewModel.dropdownValue.createdBy=sessionStorage.getItem("email");
        this.commonService.addDropdownValue(this.raceAddViewModel).subscribe(
          (res) => {
            if (typeof (res) == 'undefined') {
              this.snackbar.open('Race Addition failed. ' + sessionStorage.getItem("httpError"), '', {
                duration: 10000
              });
            }
            else {
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
      else {
        this.raceAddViewModel.dropdownValue.id = this.form.controls.id.value
        this.raceAddViewModel.dropdownValue.lovColumnValue = this.form.controls.lovColumnValue.value;
        this.raceAddViewModel.dropdownValue.lovName = "Race";
        this.raceAddViewModel.dropdownValue.updatedBy=sessionStorage.getItem("email");
        this.commonService.updateDropdownValue(this.raceAddViewModel).subscribe(
          (res) => {
            if (typeof (res) == 'undefined') {
              this.snackbar.open('Race Updation failed. ' + sessionStorage.getItem("httpError"), '', {
                duration: 10000
              });
            }
            else {
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
        )
      }
    }
  }
  cancel() {
    this.dialogRef.close();
  }
}
