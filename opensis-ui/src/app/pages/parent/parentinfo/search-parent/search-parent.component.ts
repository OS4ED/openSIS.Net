import { Component, OnInit, ViewChild, Output, Input, EventEmitter, OnDestroy } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatAccordion } from '@angular/material/expansion';
import { StudentService } from '../../../../services/student.service';
import { filterParams, StudentListModel, StudentMasterSearchModel } from '../../../../models/student.model';
import { GetAllSectionModel } from '../../../../models/section.model';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { CommonLOV } from '../../../shared-module/lov/common-lov';
import { SectionService } from '../../../../services/section.service';
import { CommonService } from '../../../../services/common.service';
import { LoginService } from '../../../../services/login.service';
import { CountryModel } from '../../../../models/country.model';
import { LanguageModel } from '../../../../models/language.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { userProfile} from '../../../../enums/studentAdd.enum';
import { ParentAdvanceSearchModel, ParentInfoModel } from 'src/app/models/parent-info.model';

@Component({
  selector: 'vex-search-parent',
  templateUrl: './search-parent.component.html',
  styleUrls: ['./search-parent.component.scss']
})
export class SearchParentComponent implements OnInit,OnDestroy {

  userProfileEnum = Object.keys(userProfile);
  @ViewChild(MatAccordion) accordion: MatAccordion;
  @Output() showHideAdvanceSearch = new EventEmitter<boolean>();
  @Output() searchList = new EventEmitter<any>();
  @Output() searchValue = new EventEmitter<any>();
  @Input() incomingSearchValue;
  countryModel: CountryModel = new CountryModel();
  @ViewChild('f') currentForm: NgForm;
  destroySubject$: Subject<void> = new Subject();
  parentSearchModel: ParentAdvanceSearchModel = new ParentAdvanceSearchModel();
  countryListArr = [];
  constructor(
    private snackbar: MatSnackBar,
    private commonService: CommonService,
  ) { }

  ngOnInit(): void {
    if (this.incomingSearchValue){
      this.parentSearchModel = this.incomingSearchValue;
    }
    this.getAllCountry();
  }


  getAllCountry() {
    this.commonService.GetAllCountry(this.countryModel).pipe(takeUntil(this.destroySubject$)).subscribe(data => {
      if (typeof (data) == 'undefined') {
        this.countryListArr = [];
      }
      else {
        if (data._failure) {
          this.countryListArr = [];
        } else {
          this.countryListArr = data.tableCountry?.sort((a, b) => { return a.name < b.name ? -1 : 1; })

        }
      }
    });
  }


  submit() {
    this.searchList.emit(this.parentSearchModel);
    this.searchValue.emit(this.currentForm.value);
  }

  resetData() {
    this.currentForm.reset();
    this.parentSearchModel = new ParentAdvanceSearchModel();
    this.submit();

  }

  hideAdvanceSearch() {
    this.showHideAdvanceSearch.emit(false);
  }

  onProfileChange(event){
    this.parentSearchModel.userProfile = event.toString();
  }

  ngOnDestroy() {
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }

}
