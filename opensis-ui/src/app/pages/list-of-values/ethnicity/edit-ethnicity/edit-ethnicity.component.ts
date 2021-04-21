import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import icClose from '@iconify/icons-ic/twotone-close';
import { LovAddView } from '../../../../models/lov.model';
import { CommonService } from '../../../../services/common.service';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'vex-edit-ethnicity',
  templateUrl: './edit-ethnicity.component.html',
  styleUrls: ['./edit-ethnicity.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class EditEthnicityComponent implements OnInit {
  icClose = icClose;
  ethnicityAddViewModel: LovAddView = new LovAddView();
  form: FormGroup;
  editMode = false;
  raceTitle: string;
  
  constructor(private dialogRef: MatDialogRef<EditEthnicityComponent>,
    public translateService: TranslateService,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fb: FormBuilder,
    private snackbar: MatSnackBar,
    private commonService: CommonService) {
      translateService.use('en');
    this.form = this.fb.group({
      id: [0],
      lovColumnValue: ['', [Validators.required]]
    })

    if (data == null) {
      this.raceTitle = "addEthnicity";
    }
    else {
      this.editMode = true;
      this.raceTitle = "editEthnicity";
      //this.ethnicityAddViewModel.dropdownValue = data;
      this.form.controls.id.patchValue(data.id)
      this.form.controls.lovColumnValue.patchValue(data.lovColumnValue)

    }

   }

  ngOnInit(): void {
  }

  submit() {
    if (this.form.valid) {
      if (this.form.controls.id.value == 0) {
        this.ethnicityAddViewModel.dropdownValue.lovColumnValue = this.form.controls.lovColumnValue.value;
        this.ethnicityAddViewModel.dropdownValue.lovName = "Ethnicity";
        this.ethnicityAddViewModel.dropdownValue.createdBy = sessionStorage.getItem("email");
        this.commonService.addDropdownValue(this.ethnicityAddViewModel).subscribe(
          (res) => {
            if (typeof (res) == 'undefined') {
              this.snackbar.open('Ethnicity Addition failed. ' + sessionStorage.getItem("httpError"), '', {
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
        this.ethnicityAddViewModel.dropdownValue.id = this.form.controls.id.value
        this.ethnicityAddViewModel.dropdownValue.lovColumnValue = this.form.controls.lovColumnValue.value;
        this.ethnicityAddViewModel.dropdownValue.lovName = "Ethnicity";
        this.ethnicityAddViewModel.dropdownValue.updatedBy = sessionStorage.getItem("email");
        this.commonService.updateDropdownValue(this.ethnicityAddViewModel).subscribe(
          (res) => {
            if (typeof (res) == 'undefined') {
              this.snackbar.open('Ethnicity Updation failed. ' + sessionStorage.getItem("httpError"), '', {
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
