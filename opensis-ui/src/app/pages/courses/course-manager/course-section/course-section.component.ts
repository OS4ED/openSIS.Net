import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
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
import { CourseSectionAddViewModel, GetAllCourseSectionModel } from '../../../../models/courseSectionModel';
import { MatSnackBar } from '@angular/material/snack-bar';
import { LoaderService } from '../../../../services/loader.service';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';

@Component({
  selector: 'vex-course-section',
  templateUrl: './course-section.component.html',
  styleUrls: ['./course-section.component.scss']
})
export class CourseSectionComponent implements OnInit,OnDestroy{
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
  @Output() backToCourseFromCourseSection = new EventEmitter<boolean>();
  getAllCourseSectionModel:GetAllCourseSectionModel=new GetAllCourseSectionModel();
  selectedSectionDetails:CourseSectionAddViewModel;
  selectedCourseSection:number=0;
  nameSearch:string=''
  destroySubject$: Subject<void> = new Subject();
  loading:boolean;
  constructor(public translateService:TranslateService,
    private dialog: MatDialog,
    private courseSectionService:CourseSectionService,
    private snackbar: MatSnackBar,
    private loaderService:LoaderService) {
    translateService.use('en');
    this.loaderService.isLoading.pipe(takeUntil(this.destroySubject$)).subscribe((val) => {
      this.loading = val;
    });
   }
   visibleColumns=''
  ngOnInit(): void {
    this.getAllCourseSection();
  }

  backToCourse(){
    this.backToCourseFromCourseSection.emit();
  }

  addCourseSection(){
    this.dialog.open(EditCourseSectionComponent, {
      data:{
        courseDetails:this.courseDetailsFromParent,
        editMode:false
      },
      width: '900px'
    }).afterClosed().subscribe((res)=>{
      if(res){
        this.getAllCourseSection();
      }
    });
  }

  closeCourseDetails(){

  }
  changeTab(tab){
    this.selectedTab=tab;
  }

  getAllCourseSection(){  
    this.getAllCourseSectionModel.courseId=this.courseDetailsFromParent.courseId;
    this.getAllCourseSectionModel.schoolId=+sessionStorage.getItem("selectedSchoolId");
    this.getAllCourseSectionModel.tenantId=sessionStorage.getItem("tenantId");
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
          this.getAllCourseSectionModel=res;
          this.findMarkingPeriodTitle()
          this.selectedSectionDetails=res.getCourseSectionForView[this.selectedCourseSection];
        }
      }
    });
  }

  findMarkingPeriodTitle(){
    this.getAllCourseSectionModel.getCourseSectionForView?.map((item)=>{
      if(item.courseSection.durationBasedOnPeriod){
        if(item.courseSection.quarters!=null){
          item.courseSection.mpTitle=item.courseSection.quarters.title;
        }else if(item.courseSection.schoolYears!=null){
          item.courseSection.mpTitle=item.courseSection.schoolYears.title;
        }else{
          item.courseSection.mpTitle=item.courseSection.semesters.title;
        }
      }
    })
  }

  changeRightSectionDetails(sectionDetails:CourseSectionAddViewModel,index){
    this.selectedSectionDetails=sectionDetails;
    this.selectedCourseSection=index;
  }

  editCourseSection(sectionDetails){
    this.dialog.open(EditCourseSectionComponent, {
      data:{
        courseDetails:this.courseDetailsFromParent,
        editMode:true,
        courseSectionDetails:sectionDetails
      },
      width: '900px'
    }).afterClosed().subscribe((res)=>{
      if(res){
        this.getAllCourseSection();
      }
    });
  }

  ngOnDestroy() {
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }

}
