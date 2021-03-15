import { AfterViewChecked, ChangeDetectionStrategy, ChangeDetectorRef, Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import icAdd from '@iconify/icons-ic/baseline-add';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icDelete from '@iconify/icons-ic/twotone-delete';
import icMoreVertical from '@iconify/icons-ic/baseline-more-vert';
import icRemoveCircle from '@iconify/icons-ic/remove-circle';
import icBack from '@iconify/icons-ic/baseline-arrow-back';
import icInfo from '@iconify/icons-ic/info';
import icClose from '@iconify/icons-ic/twotone-close';
import icCheckboxChecked from '@iconify/icons-ic/check-box';
import icCheckboxUnchecked from '@iconify/icons-ic/check-box-outline-blank';
import icSearch from '@iconify/icons-ic/search';
import icFilterList from '@iconify/icons-ic/filter-list';
import icEmail from '@iconify/icons-ic/twotone-email';
import { TranslateService } from '@ngx-translate/core';
import { MatDialog } from '@angular/material/dialog';
import { EditCourseSectionComponent } from '../edit-course-section/edit-course-section.component';
import { CourseSectionService } from '../../../../services/course-section.service';
import { CourseSectionAddViewModel, CourseSectionDataTransferModel, GetAllCourseSectionModel } from '../../../../models/courseSectionModel';
import { MatSnackBar } from '@angular/material/snack-bar';
import { LoaderService } from '../../../../services/loader.service';
import { takeUntil } from 'rxjs/operators';
import { CalendarEvent, CalendarMonthViewBeforeRenderEvent, CalendarView } from 'angular-calendar';
import { Subject } from 'rxjs';
import { SchoolPeriodService } from '../../../../services/school-period.service';
import { BlockListViewModel } from '../../../../models/schoolPeriodModel';
import { ConfirmDialogComponent } from '../../../shared-module/confirm-dialog/confirm-dialog.component';
import { CryptoService } from '../../../../services/Crypto.service';
import { RolePermissionListViewModel, RolePermissionViewModel } from '../../../../models/rollBasedAccessModel';
@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'vex-course-section',
  templateUrl: './course-section.component.html',
  styleUrls: ['./course-section.component.scss'],
  styles: [
    `
     .cal-month-view .bg-aqua,
      .cal-week-view .cal-day-columns .bg-aqua,
      .cal-day-view .bg-aqua {
        background-color: #ffdee4 !important;
      }
    `,
  ],
})
export class CourseSectionComponent implements OnInit,OnDestroy,AfterViewChecked{
  icAdd = icAdd;
  icEdit = icEdit;
  icDelete = icDelete;
  icEmail = icEmail;
  icMoreVertical = icMoreVertical;
  icRemoveCircle = icRemoveCircle;
  icInfo = icInfo;
  icBack = icBack;
  icClose = icClose;
  icCheckboxChecked = icCheckboxChecked;
  icCheckboxUnchecked = icCheckboxUnchecked;
  icFilterList = icFilterList;
  icSearch = icSearch;
  courseDetails = 0;
  selectedTab = 'overview';
  @Input() courseDetailsFromParent;
  @Output() backToCourseFromCourseSection = new EventEmitter<CourseSectionDataTransferModel>();
  getAllCourseSectionModel:GetAllCourseSectionModel=new GetAllCourseSectionModel();
  selectedSectionDetails:CourseSectionAddViewModel;
  selectedCourseSection:number=0;
  nameSearch:string=''
  classRoomName:string;
  classPeriodName:string;
  classtakeAttendance:string;
  classdate:string;
  color=['bg-deep-orange','bg-red','bg-green','bg-teal','bg-cyan','bg-deep-purple','bg-pink','bg-blue'];   
  calendarDayDetails=0;
  destroySubject$: Subject<void> = new Subject();
  loading:boolean;
  weekendDays: number[];
  filterDays = [];
  events: CalendarEvent[] = [];
  refresh: Subject<any> = new Subject();
  view: CalendarView = CalendarView.Month;
  viewDate: Date = new Date();
  cssClass: string;
  standardGradeTitle=[];
  courseSectionAddViewModel: CourseSectionAddViewModel = new CourseSectionAddViewModel();
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();

  constructor(public translateService:TranslateService,
    private dialog: MatDialog,
    private courseSectionService:CourseSectionService,
    private snackbar: MatSnackBar,
    private cdr: ChangeDetectorRef,
    private loaderService:LoaderService,
    private schoolPeriodService: SchoolPeriodService,
    private cryptoService:CryptoService) {
    translateService.use('en');
    this.loaderService.isLoading.pipe(takeUntil(this.destroySubject$)).subscribe((val) => {
      this.loading = val;
    });
   }
 
   visibleColumns=''
  ngOnInit(): void {
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 6);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 12);
    this.editPermission = permissionCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionCategory.rolePermission[0].canDelete;
    this.addPermission = permissionCategory.rolePermission[0].canAdd;
    this.getAllCourseSection();    
  }
  ngAfterViewChecked(){
    this.cdr.detectChanges();
 }

  backToCourse() {
    this.backToCourseFromCourseSection.emit({courseSectionCount:this.getAllCourseSectionModel.getCourseSectionForView?.length,showCourse:true,courseId:this.courseDetailsFromParent.courseId});
  }
  
  addCourseSection() {
    this.dialog.open(EditCourseSectionComponent, {
      data: {
        courseDetails: this.courseDetailsFromParent,
        editMode: false
      },
      width: '900px'
    }).afterClosed().subscribe((res) => {
      if (res) {
        this.getAllCourseSection();
      }
    });
  }

  closeCourseDetails() {

  }

  initializeColor(){

  }

  changeTab(tab) {
    this.selectedTab = tab;
  }
  viewEvent(event){

    this.classPeriodName= event.title;
    this.classRoomName = event.meta.scheduleDetails.rooms.title;
    this.classdate= event.meta.scheduleDetails.date;
    this.classtakeAttendance= event.meta.scheduleDetails.takeAttendance ? 'Yes':'No';
    this.calendarDayDetails=1;
  }
  closeDetails(){
    this.calendarDayDetails=0;
  }
  
  getAllCourseSection(){  
    this.getAllCourseSectionModel.courseId=this.courseDetailsFromParent.courseId;
    this.getAllCourseSectionModel.schoolId=+sessionStorage.getItem("selectedSchoolId");
    this.getAllCourseSectionModel.tenantId=sessionStorage.getItem("tenantId");
    this.getAllCourseSectionModel.academicYear =+sessionStorage.getItem("academicyear");
    this.courseSectionService.getAllCourseSection(this.getAllCourseSectionModel).subscribe(res => {
      if (typeof (res) == 'undefined') {       
        this.snackbar.open('Course Section Failed ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (res._failure) {          
          this.getAllCourseSectionModel.getCourseSectionForView=res.getCourseSectionForView;
          this.snackbar.open(res._message, '', {
            duration: 10000
          });
        } else {
          this.getAllCourseSectionModel = res;
          this.findMarkingPeriodTitle()
          this.selectedSectionDetails = res.getCourseSectionForView[this.selectedCourseSection];
          let days = this.selectedSectionDetails.courseSection.schoolCalendars.days;
          if (days !== null && days !== undefined) {
            this.getDays(days);
          }
          this.renderCalendarPeriods(this.selectedSectionDetails);
        }
      }
    });
  }

  //render weekends
  getDays(days: string) {
    const calendarDays = days;
    var allDays = [0, 1, 2, 3, 4, 5, 6];
    var splitDays = calendarDays.split('').map(x => +x);
    this.filterDays = allDays.filter(f => !splitDays.includes(f));
    this.weekendDays = this.filterDays;
    this.cssClass = 'bg-aqua';
    this.refresh.next();
  }

  //render calendar periods
  renderCalendarPeriods(selectedSectionDetails) {
    this.events = [];
    for (let schedule of selectedSectionDetails.courseCalendarSchedule) {
     let random=Math.floor((Math.random() * 7) + 0);
      this.events.push({
        start: new Date(schedule.date),
        end: new Date(schedule.date),
        title: schedule.blockPeriod.periodTitle,
        color: null,
        actions: null,
        allDay: schedule.takeAttendance,
        resizable: {
          beforeStart: true,
          afterEnd: true,
        },
        draggable: false,
        meta: {scheduleDetails:schedule,randomColor:this.color[random]}
      });
      this.refresh.next();
    }
  }

  //for rendar weekends
  beforeMonthViewRender(renderEvent: CalendarMonthViewBeforeRenderEvent): void {
      renderEvent.body.forEach((day) => {
      const dayOfMonth = day.date.getDay();
      if (this.filterDays.includes(dayOfMonth)) {
        day.cssClass = this.cssClass;
      }
    });
        
  }
 
  findMarkingPeriodTitle(){
    this.getAllCourseSectionModel.getCourseSectionForView?.map((item)=>{
      if(item.courseSection.durationBasedOnPeriod){
        if(item.courseSection.quarters!=null){
          item.courseSection.mpTitle=item.courseSection.quarters.title;
          item.courseSection.mpStartDate=item.courseSection.quarters.startDate;
          item.courseSection.mpEndDate=item.courseSection.quarters.endDate;
        }else if(item.courseSection.schoolYears!=null){
          item.courseSection.mpTitle=item.courseSection.schoolYears.title;
          item.courseSection.mpStartDate=item.courseSection.schoolYears.startDate;
          item.courseSection.mpEndDate=item.courseSection.schoolYears.endDate;
        }else{
          item.courseSection.mpTitle=item.courseSection.semesters.title;
          item.courseSection.mpStartDate=item.courseSection.semesters.startDate;
          item.courseSection.mpEndDate=item.courseSection.semesters.endDate;
        }
      }
    })
  }

  changeRightSectionDetails(sectionDetails: CourseSectionAddViewModel, index) {
    this.selectedSectionDetails = sectionDetails;
    this.selectedCourseSection = index;
    let days = this.selectedSectionDetails.courseSection.schoolCalendars.days;
      if (days !== null && days !== undefined) {
        this.getDays(days);
      }
    this.renderCalendarPeriods(this.selectedSectionDetails);
  }

  editCourseSection(sectionDetails) {
    this.dialog.open(EditCourseSectionComponent, {
      data: {
        courseDetails: this.courseDetailsFromParent,
        editMode: true,
        courseSectionDetails: sectionDetails
      },
      width: '900px'
    }).afterClosed().subscribe((res) => {
      if (res) {
        this.getAllCourseSection();
      }
    });
  }

  confirmDelete(selectedCourseSection){
    // call our modal window
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: {
          title: "Are you sure?",
          message: "You are about to delete "+selectedCourseSection.courseSection.courseSectionName+"."}
    });
    // listen to response
    dialogRef.afterClosed().subscribe(dialogResult => {
      // if user pressed yes dialogResult will be true, 
      // if user pressed no - it will be false
      if(dialogResult){
        this.deleteCourseSection(selectedCourseSection);
      }
   });
  }

  deleteCourseSection(selectedCourseSection){
    this.courseSectionAddViewModel.courseSection.tenantId=sessionStorage.getItem("tenantId");
    this.courseSectionAddViewModel.courseSection.schoolId=+sessionStorage.getItem("selectedSchoolId");
    this.courseSectionAddViewModel.courseSection.courseId=selectedCourseSection.courseSection.courseId;
    this.courseSectionAddViewModel.courseSection.courseSectionId=selectedCourseSection.courseSection.courseSectionId;
    this.courseSectionService.deleteCourseSection(this.courseSectionAddViewModel).subscribe(data => {
      if (typeof (data) == 'undefined') {
        this.snackbar.open('Course Section Delete failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 5000
        });
      }
      else {
        if (data._failure) {
          this.snackbar.open(data._message, '', {
            duration: 5000
          });
        } else {
          this.snackbar.open(data._message, '', {
            duration: 5000
          });
          if(this.selectedSectionDetails==selectedCourseSection){
            this.selectedCourseSection=0;
            this.selectedSectionDetails=this.getAllCourseSectionModel.getCourseSectionForView[0];
          }
          
          this.getAllCourseSection();
        }
      }
    });
}
  ngOnDestroy() {
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }

}
