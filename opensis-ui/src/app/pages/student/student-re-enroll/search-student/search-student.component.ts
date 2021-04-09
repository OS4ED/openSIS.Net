import {
  Component,
  OnInit,
  ViewChild,
  Output,
  EventEmitter,
  OnDestroy,
  Input,
} from "@angular/core";
import { NgForm } from "@angular/forms";
import { MatAccordion } from "@angular/material/expansion";
import { StudentService } from "../../../../services/student.service";
import { CommonLOV } from "../../../shared-module/lov/common-lov";
import { SectionService } from "../../../../services/section.service";
import { CommonService } from "../../../../services/common.service";
import { LoginService } from "../../../../services/login.service";
import { MatSnackBar } from "@angular/material/snack-bar";
import icClose from "@iconify/icons-ic/twotone-close";
import { CountryModel } from "src/app/models/countryModel";
import { LanguageModel } from "src/app/models/languageModel";
import { Subject } from "rxjs";
import { StudentListModel, StudentMasterSearchModel } from "src/app/models/studentModel";
import { SearchFilterAddViewModel } from "src/app/models/searchFilterModel";
import { takeUntil } from "rxjs/operators";
import { SharedFunction } from "src/app/pages/shared/shared-function";
import { EnrollmentCodesService } from "src/app/services/enrollment-codes.service";
import { EnrollmentCodeListView } from "src/app/models/enrollmentCodeModel";
import { MatSlideToggle } from "@angular/material/slide-toggle";

@Component({
  selector: "vex-search-student",
  templateUrl: "./search-student.component.html",
  styleUrls: ["./search-student.component.scss"],
})
export class SearchStudentComponent implements OnInit {
  icClose = icClose;
  @ViewChild(MatAccordion) accordion: MatAccordion;
  @Output() showHideAdvanceSearch = new EventEmitter<any>();
  @Output() searchList = new EventEmitter<any>();
  @Input() filterJsonParams;
  searchSchoolId:number = +sessionStorage.getItem('selectedSchoolId');
  countryModel: CountryModel = new CountryModel();
  languages: LanguageModel = new LanguageModel();
  @ViewChild('f') currentForm: NgForm;
  destroySubject$: Subject<void> = new Subject();
  studentMasterSearchModel: StudentMasterSearchModel = new StudentMasterSearchModel();
  getAllStudent: StudentListModel = new StudentListModel();
  searchFilterAddViewModel: SearchFilterAddViewModel = new SearchFilterAddViewModel();
  enrollmentCodeListView: EnrollmentCodeListView= new EnrollmentCodeListView();
  dobEndDate: string;
  dobStartDate: string;
  showSaveFilter = true;
  params = [];
  searchTitle: string = 'search';
  updateFilter: boolean = false;
  countryListArr = [];
  ethnicityList = [];
  raceList = [];
  genderList = [];
  maritalStatusList = [];
  enrollmentList=[];
  languageList;
  constructor(
    private studentService: StudentService,
    private commonLOV: CommonLOV,
    private snackbar: MatSnackBar,
    private commonService: CommonService,
    private loginService: LoginService,
    private commonFunction: SharedFunction,
    private enrollmentCodeService: EnrollmentCodesService
  ) { }

  ngOnInit(): void {
    this.initializeDropdownsInAddMode();
  }

  initializeDropdownsInAddMode() {
    this.callLOVs();
    this.getAllCountry();
    this.GetAllLanguage();
    this.getAllStudentEnrollmentCode();
  }

  callLOVs() {
    this.commonLOV.getLovByName('Gender').pipe(takeUntil(this.destroySubject$)).subscribe((res) => {
      this.genderList = res;
    });
    this.commonLOV.getLovByName('Race').pipe(takeUntil(this.destroySubject$)).subscribe((res) => {
      this.raceList = res;
    });
    this.commonLOV.getLovByName('Ethnicity').pipe(takeUntil(this.destroySubject$)).subscribe((res) => {
      this.ethnicityList = res;
    });
    this.commonLOV.getLovByName('Marital Status').pipe(takeUntil(this.destroySubject$)).subscribe((res) => {
      this.maritalStatusList = res;
    });
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
    })
  }

  GetAllLanguage() {
    this.languages._tenantName = sessionStorage.getItem("tenant");
    this.loginService.getAllLanguage(this.languages).pipe(takeUntil(this.destroySubject$)).subscribe((res) => {
      if (typeof (res) == 'undefined') {
        this.languageList = [];
      }
      else {
        this.languageList = res.tableLanguage?.sort((a, b) => { return a.locale < b.locale ? -1 : 1; })

      }
    })
  }

  searchAllSchools(event: MatSlideToggle) {
    if (event.checked) {
      this.searchSchoolId = 0;
    }
    else {
      this.searchSchoolId = +sessionStorage.getItem('selectedSchoolId');
    }

  };

  submit() {
    this.getAllStudent= new StudentListModel();
    this.params = [];
    for (var key in this.studentMasterSearchModel) {
      if (this.studentMasterSearchModel.hasOwnProperty(key))
        if (this.studentMasterSearchModel[key] !== null && this.studentMasterSearchModel[key] !== '') {
          if (key === 'exitDate' || key === 'enrollmentDate' || key === 'dob') {
            this.params.push({ "columnName": key, "filterOption": 11, "filterValue": this.commonFunction.formatDateSaveWithoutTime(this.studentMasterSearchModel[key]) })
          }
          else {
            this.params.push({ "columnName": key, "filterOption": 11, "filterValue": this.studentMasterSearchModel[key] })
          }
        }
    }
    
    this.getAllStudent.filterParams = this.params;
    this.getAllStudent.sortingModel = null;
    this.getAllStudent.schoolId = this.searchSchoolId;
    this.studentService.searchStudentListForReenroll(this.getAllStudent).subscribe(data => {
      if (data._failure) {
        this.searchList.emit(data);
        this.snackbar.open('' + data._message, '', {
          duration: 10000
        });

      } else {
        this.searchList.emit(data);
        this.showHideAdvanceSearch.emit({ hide: false });
      }
    });

  }

  getAllStudentEnrollmentCode(){
    this.enrollmentCodeService.getAllStudentEnrollmentCode(this.enrollmentCodeListView).subscribe(
      (res:EnrollmentCodeListView)=>{
        if(typeof(res)=='undefined'){
          this.snackbar.open('Enrollment code list failed. ' + sessionStorage.getItem("httpError"), '', {
            duration: 10000
          });
        }
        else{
          if (res._failure) { 
          } 
          else { 
            this.enrollmentList= res.studentEnrollmentCodeList.filter(x=> x.type==='Rolled Over' || x.type==='Drop (Transfer)' || x.type==='Drop');
            }
        }
      }
    )
  }

  resetData() {
    this.currentForm.reset();
  }

  hideAdvanceSearch() {
    this.showHideAdvanceSearch.emit({ hide: false });
  }

  ngOnDestroy() {
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }
}
