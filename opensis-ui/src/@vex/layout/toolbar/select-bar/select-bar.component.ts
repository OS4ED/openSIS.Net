import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ReplaySubject, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { SchoolService } from '../../../../app/services/school.service';
import { AllSchoolListModel, OnlySchoolListModel } from '../../../../app/models/getAllSchoolModel';
import { Router } from '@angular/router';
import { MarkingPeriodService } from '../../../../app/services/marking-period.service';
import { GetAcademicYearListModel, GetMarkingPeriodTitleListModel } from '../../../../app/models/markingPeriodModel';
import { DasboardService } from '../../../../app/services/dasboard.service';

@Component({
  selector: 'vex-select-bar',
  templateUrl: './select-bar.component.html',
  styleUrls: ['./select-bar.component.scss']
})
export class SelectBarComponent implements OnInit {
  getSchoolList: OnlySchoolListModel = new OnlySchoolListModel();
  schools=[];
  getAcademicYears: GetAcademicYearListModel = new GetAcademicYearListModel();
  markingPeriodTitleLists: GetMarkingPeriodTitleListModel = new GetMarkingPeriodTitleListModel();
  academicYears = [];
  periods = [];
  checkForAnyNewSchool: boolean = false;
  nullValueForDropdown="Please Select";
  schoolCtrl: FormControl;
  /** control for the selected Year */
  public academicYearsCtrl: FormControl = new FormControl();
  filteredSchoolsForDrop;
  /** control for the selected Part */
  public periodCtrl: FormControl = new FormControl();
  schoolFilterCtrl: FormControl;

  /** list of schools filtered by search keyword */
  public filteredSchools: ReplaySubject<AllSchoolListModel[]> = new ReplaySubject<AllSchoolListModel[]>(1);

  /** Subject that emits when the component has been destroyed. */
  protected onDestroy = new Subject<void>();

  constructor(private schoolService: SchoolService,
    private router: Router,
    private markingPeriodService: MarkingPeriodService,
    private dasboardService:DasboardService
  ) {
    this.schoolService.currentMessage.pipe(takeUntil(this.onDestroy)).subscribe((res) => {
      if (res) {
        this.checkForAnyNewSchool = res;
        this.callAllSchool();
        this.callAcademicYearsOnSchoolSelect();
      }
    })
  }

  ngOnInit() {
    this.callAllSchool();
    this.markingPeriodService.currentY.subscribe((res) => {
      if (res) {
        this.callAcademicYearsOnSchoolSelect();        
      }
    })
  }
  callAllSchool() {
    this.getSchoolList.tenantId = sessionStorage.getItem("tenantId");
    this.getSchoolList._tenantName = sessionStorage.getItem("tenant");
    this.getSchoolList._token = sessionStorage.getItem("token");

    this.schoolService.GetAllSchools(this.getSchoolList).subscribe((data) => {
      this.schools = data.schoolMaster;
    
      /** control for the selected School */
      this.schoolCtrl = new FormControl();
      this.schoolFilterCtrl = new FormControl();
      // set initial selection
      this.schoolCtrl.setValue(this.schools[0]);
      // load the initial School list
      this.filteredSchools.next(this.schools.slice());
      /** control for the MatSelect filter keyword */
      this.schoolFilterCtrl.valueChanges
        .pipe(takeUntil(this.onDestroy))
        .subscribe(() => {
          this.filterSchools();
        });
      if (this.checkForAnyNewSchool) {
        this.selectNewSchoolOnAddSchool();
        this.checkForAnyNewSchool = false;
      } else {
        this.selectSchoolOnLoad();
      }
      this.schoolService.changeSchoolListStatus(true);
    });
  }

  selectSchoolOnLoad() {
    if (!sessionStorage.getItem("selectedSchoolId")) {
      sessionStorage.setItem("selectedSchoolId", this.schools[0].schoolId);
      sessionStorage.setItem("schoolOpened", this.schools[0].schoolDetail[0].dateSchoolOpened);
      this.callAcademicYearsOnSchoolSelect();
    } else {
      this.setSchool();
    }
  }

  selectNewSchoolOnAddSchool() {
    this.setSchool();
  }

  setSchool() {
    let id = +sessionStorage.getItem("selectedSchoolId");
    let index = this.schools.findIndex((x) => {
      return x.schoolId === id
    });
    if (index != -1) {
      this.schoolCtrl.setValue(this.schools[index]);
      sessionStorage.setItem("schoolOpened", this.schools[index].schoolDetail[0].dateSchoolOpened);
    } else {
      this.schoolCtrl.setValue(this.schools[0]);
      sessionStorage.setItem("schoolOpened", this.schools[0].schoolDetail[0].dateSchoolOpened);
    }
    if(!this.checkForAnyNewSchool){
      this.callAcademicYearsOnSchoolSelect();
    }
  }

  changeSchool(details) {
    sessionStorage.setItem("selectedSchoolId", details.schoolId);
    sessionStorage.setItem("schoolOpened", details.schoolDetail[0].dateSchoolOpened);
    this.callAcademicYearsOnSchoolSelect();
    this.router.navigate(['/school/dashboards']);
    this.dasboardService.sendPageLoadEvent(true);
  }

  callAcademicYearsOnSchoolSelect() {
    this.getAcademicYears.schoolId = +sessionStorage.getItem("selectedSchoolId");
    this.markingPeriodService.getAcademicYearList(this.getAcademicYears).subscribe((res) => {
      this.academicYears = res.academicYears;
      // set initial selection
      if (this.academicYears?.length > 0) {
        this.academicYearsCtrl.setValue(this.academicYears[this.academicYears.length - 1]);
        sessionStorage.setItem("academicyear", this.academicYearsCtrl.value.academyYear);
        sessionStorage.setItem("markingPeriod",this.academicYearsCtrl.value.startDate);
        
      } else {
       
        this.academicYearsCtrl.setValue(this.nullValueForDropdown);
        sessionStorage.setItem("academicyear","null");
        sessionStorage.setItem("markingPeriod","null");
      }
      if(this.academicYearsCtrl.value==this.nullValueForDropdown){
        sessionStorage.setItem("academicyear","null");
        sessionStorage.setItem("markingPeriod","null");
         this.periods=[]
        this.callMarkingPeriodTitleList();
        }else{         
          sessionStorage.setItem("academicyear", this.academicYearsCtrl.value.academyYear); 
          sessionStorage.setItem("markingPeriod",this.academicYearsCtrl.value.startDate);
          this.callMarkingPeriodTitleList();
        }
    })

  }

  changeYear(event) {
    if(event.value==this.nullValueForDropdown){
    sessionStorage.setItem("academicyear","null");
    sessionStorage.setItem("markingPeriod", "null");
    this.callMarkingPeriodTitleList();
    }else{
      sessionStorage.setItem("academicyear", event.value.academyYear);
      sessionStorage.setItem("markingPeriod", event.value.startDate);
      this.callMarkingPeriodTitleList();
    }
    this.router.navigate(['/school/dashboards']);
  }

  callMarkingPeriodTitleList() {
    if (sessionStorage.getItem("academicyear") !== "null") {
      this.markingPeriodTitleLists.schoolId = +sessionStorage.getItem("selectedSchoolId");
      this.markingPeriodTitleLists.academicYear = +sessionStorage.getItem("academicyear");
      this.markingPeriodService.getMarkingPeriodTitleList(this.markingPeriodTitleLists).subscribe((res) => {
        this.periods = res.period;
        if (this.periods?.length > 0) {
          for (let i = 0; i < this.periods.length; i++) {
            let today = new Date().setHours(0, 0, 0, 0);
            let startDate = new Date(this.periods[i]?.startDate).setHours(0, 0, 0, 0);
            let endDate = new Date(this.periods[i]?.endDate).setHours(0, 0, 0, 0);
            if (today <= endDate && today >= startDate) {
              this.periodCtrl.setValue(this.periods[i].periodTitle);
              sessionStorage.setItem("markingPeriodId",this.periods[i].markingPeriodId);
            } else {
              this.periodCtrl.setValue(this.periods[0].periodTitle);
              sessionStorage.setItem("markingPeriodId",this.periods[i].markingPeriodId);
            }
          }
        } else {
          this.periodCtrl.setValue(this.nullValueForDropdown);
          sessionStorage.setItem("markingPeriodId",null);
        }
      })
    } else {
      this.periodCtrl.setValue(this.nullValueForDropdown);
      sessionStorage.setItem("markingPeriodId",null);
    }
  }

  changePeriod(event) {
    this.router.navigate(['/school/dashboards']);
  }

  protected filterSchools() {
    if (!this.schools) {
      return;
    }
    // get the search keyword
    let search = this.schoolFilterCtrl.value;
    if (!search) {
      this.filteredSchools.next(this.schools.slice());
      return;
    } else {
      search = search.toLowerCase();
    }
    // filter the school
    this.filteredSchools.next(
      this.schools.filter(school => school.schoolName.toLowerCase().indexOf(search) > -1)
    );
  }

  ngOnDestroy() {
    this.onDestroy.next();
    this.onDestroy.complete();
    this.schoolService.changeMessage(false);
  }
}

