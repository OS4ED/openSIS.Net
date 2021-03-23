import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import icClose from '@iconify/icons-ic/twotone-close';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { TranslateService } from '@ngx-translate/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SchoolPeriodService } from '../../../../services/school-period.service';
import { BlockAddViewModel } from 'src/app/models/schoolPeriodModel';

@Component({
  selector: 'vex-edit-block',
  templateUrl: './edit-block.component.html',
  styleUrls: ['./edit-block.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class EditBlockComponent implements OnInit {
  icClose = icClose;
  form: FormGroup;
  blockHeaderTitle: string;
  buttonType: string;
  blockAddViewModel: BlockAddViewModel = new BlockAddViewModel();

  constructor(private dialogRef: MatDialogRef<EditBlockComponent>, public translateService: TranslateService,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private snackbar: MatSnackBar,
    private schoolPeriodService: SchoolPeriodService) {
    translateService.use('en');
    this.form = fb.group({
      blockId: [0],
      title: ['', [Validators.required]],
      sortOrder: ['', [Validators.required, Validators.min(1)]]
    });
    if (data == null) {
      this.blockHeaderTitle = "addBlockRotationDay";
      this.buttonType = "submit";
    }
    else {
      this.blockHeaderTitle = "editBlockRotationDay";
      this.buttonType = "update";
      this.blockAddViewModel.block = data
      this.form.controls.blockId.patchValue(data.blockId)
      this.form.controls.title.patchValue(data.blockTitle)
      this.form.controls.sortOrder.patchValue(data.blockSortOrder)
    }
  }

  ngOnInit(): void {
  }
  submit() {
    this.form.markAllAsTouched()
    if (this.form.valid) {
      if (this.form.controls.blockId.value == 0) {
        this.blockAddViewModel.block.blockTitle = this.form.controls.title.value;
        this.blockAddViewModel.block.blockSortOrder = this.form.controls.sortOrder.value;
        this.blockAddViewModel.block.blockSortOrder = this.form.controls.sortOrder.value;
        this.blockAddViewModel.block.createdBy = sessionStorage.getItem('email');
        this.schoolPeriodService.addBlock(this.blockAddViewModel).subscribe(
          (res: BlockAddViewModel) => {
            if (typeof (res) == 'undefined') {
              this.snackbar.open('Block/Rotation Days Creation failed. ' + sessionStorage.getItem("httpError"), '', {
                duration: 10000
              });
            }
            else {
              if (res._failure) {
                this.snackbar.open( res._message, '', {
                  duration: 10000
                });
              }
              else {
                this.snackbar.open( res._message, '', {
                  duration: 10000
                });
                this.dialogRef.close({mode:'submited', currentBlockId:res.block.blockId});
              }
            }
          }
        )

      }
      else {
        this.blockAddViewModel.block.blockId = this.form.controls.blockId.value;
        this.blockAddViewModel.block.blockTitle = this.form.controls.title.value;
        this.blockAddViewModel.block.blockSortOrder = this.form.controls.sortOrder.value;
        this.blockAddViewModel.block.updatedBy = sessionStorage.getItem('email');
        this.schoolPeriodService.updateBlock(this.blockAddViewModel).subscribe(
          (res: BlockAddViewModel) => {
            if (typeof (res) == 'undefined') {
              this.snackbar.open('Block/Rotation Days Updation failed. ' + sessionStorage.getItem("httpError"), '', {
                duration: 10000
              });
            }
            else {
              if (res._failure) {
                this.snackbar.open(res._message, '', {
                  duration: 10000
                });
              }
              else {
                this.snackbar.open(res._message, '', {
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
