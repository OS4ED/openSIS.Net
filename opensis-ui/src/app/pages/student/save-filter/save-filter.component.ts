import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import icClose from '@iconify/icons-ic/twotone-close';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { stagger60ms } from 'src/@vex/animations/stagger.animation';
import { fadeInUp400ms } from 'src/@vex/animations/fade-in-up.animation';
import { SearchFilterAddViewModel } from 'src/app/models/search-filter.model';
import { CommonService } from 'src/app/services/common.service';
import { ValidationService } from '../../shared/validation.service';

@Component({
  selector: 'vex-save-filter',
  templateUrl: './save-filter.component.html',
  styleUrls: ['./save-filter.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class SaveFilterComponent implements OnInit {
  icClose = icClose;
  form: FormGroup;
  searchFilterAddViewModel: SearchFilterAddViewModel = new SearchFilterAddViewModel();



  constructor(private dialogRef: MatDialogRef<SaveFilterComponent>,
    private commonService: CommonService,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fb: FormBuilder,
    private snackbar: MatSnackBar) {
    this.form = fb.group({
      filterId: [0],
      filterName: ['', [ValidationService.noWhitespaceValidator]],
      jsonList: []
    })

  }

  ngOnInit(): void {
  }


  submit() {
    this.form.markAllAsTouched();
    if (this.form.valid) {
      this.searchFilterAddViewModel.searchFilter.module = 'Student';
      this.searchFilterAddViewModel.searchFilter.jsonList = JSON.stringify(this.commonService.getSearchResult());
      this.searchFilterAddViewModel.searchFilter.filterName = this.form.value.filterName;
      this.searchFilterAddViewModel.searchFilter.createdBy = sessionStorage.getItem('email');
      this.commonService.addSearchFilter(this.searchFilterAddViewModel).subscribe((res) => {
        if (typeof (res) === 'undefined') {
          this.snackbar.open('Search filter added failed' + sessionStorage.getItem("httpError"), '', {
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
      });
    }
  }

}
