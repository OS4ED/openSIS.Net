import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import icClose from '@iconify/icons-ic/twotone-close';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { TranslateService } from '@ngx-translate/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CourseManagerService } from '../../../../services/course-manager.service';
import { AllCourseSectionView, GetAllCourseListModel, GetAllProgramModel, GetAllSubjectModel, SearchCourseSectionViewModel } from '../../../../models/courseManagerModel';
import { GetMarkingPeriodTitleListModel } from '../../../../models/markingPeriodModel';
import { MarkingPeriodService } from '../../../../services/marking-period.service';
import { CourseSectionService } from '../../../../services/course-section.service';
import { MatTableDataSource } from '@angular/material/table';
import { LoaderService } from '../../../../services/loader.service';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { SelectionModel } from '@angular/cdk/collections';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialogRef } from '@angular/material/dialog';
@Component({
  selector: 'vex-add-course-section',
  templateUrl: './add-course-section.component.html',
  styleUrls: ['./add-course-section.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class AddCourseSectionComponent implements OnInit,OnDestroy {
  getAllProgramModel: GetAllProgramModel = new GetAllProgramModel();
  getAllSubjectModel: GetAllSubjectModel = new GetAllSubjectModel();
  getAllCourseListModel: GetAllCourseListModel = new GetAllCourseListModel();
  getMarkingPeriodTitleListModel: GetMarkingPeriodTitleListModel = new GetMarkingPeriodTitleListModel();
  icClose = icClose;
  displayedColumns: string[] = ['staffSelected', 'course', 'courseSectionName', 'markingPeriod', 'startDate', 'endDate',  'gradeLevelTitle','scheduledTeacher'];
  courseSectionSearch:SearchCourseSectionViewModel = new SearchCourseSectionViewModel();
  courseSectionList: MatTableDataSource<any>;
  loading:boolean;
  destroySubject$: Subject<void> = new Subject();
  selection : SelectionModel<AllCourseSectionView> = new SelectionModel<AllCourseSectionView>(true, []);
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  isSearchRecordAvailable=false;
  constructor(public translateService: TranslateService,
    private courseManagerService: CourseManagerService,
    private snackbar: MatSnackBar,
    private markingPeriodService: MarkingPeriodService,
    private courseSectionService:CourseSectionService,
    private loaderService:LoaderService,
    private dialogRef: MatDialogRef<AddCourseSectionComponent>) {
    translateService.use('en');
    this.loaderService.isLoading.pipe(takeUntil(this.destroySubject$)).subscribe((val) => {
      this.loading = val;
    });
  }

  ngOnInit(): void {
    this.getAllCourse();
    this.getAllSubjectList();
    this.getAllProgramList();
    this.getAllMarkingPeriodList();
  }


  getAllProgramList() {
    this.courseManagerService.GetAllProgramsList(this.getAllProgramModel).subscribe(data => {
      this.getAllProgramModel.programList = data.programList;
    });
  }
  getAllSubjectList() {
    this.courseManagerService.GetAllSubjectList(this.getAllSubjectModel).subscribe(data => {
      this.getAllSubjectModel.subjectList = data.subjectList;
    });
  }

  getAllMarkingPeriodList() {
    this.getMarkingPeriodTitleListModel.schoolId = +sessionStorage.getItem("selectedSchoolId");
    this.getMarkingPeriodTitleListModel.academicYear = +sessionStorage.getItem("academicyear");
    this.markingPeriodService.getAllMarkingPeriodList(this.getMarkingPeriodTitleListModel).subscribe(data => {
      if (data._failure) {
        this.getMarkingPeriodTitleListModel.getMarkingPeriodView = [];
      } else {
        this.getMarkingPeriodTitleListModel.getMarkingPeriodView = data.getMarkingPeriodView;
      }
    });
  }

  getAllCourse() {
    this.courseManagerService.GetAllCourseList(this.getAllCourseListModel).subscribe(data => {
      if (data._failure) {
          this.getAllCourseListModel.courseViewModelList = [];
      } else {
        this.getAllCourseListModel.courseViewModelList = data.courseViewModelList;
      }
    })
  }

  searchCourseSection(){
    this.isSearchRecordAvailable=true;
    this.courseSectionService.searchCourseSectionForSchedule(this.courseSectionSearch).subscribe((res)=>{
      if(res._failure){
        if(res.allCourseSectionViewList===null){
            this.courseSectionList = new MatTableDataSource([]);   
            this.snackbar.open( res._message, '', {
              duration: 5000
            });
        } else{
          this.courseSectionList = new MatTableDataSource([]); 
        }
      }else{
        res.allCourseSectionViewList=this.findMarkingPeriodTitleById(res.allCourseSectionViewList)
        this.courseSectionList = new MatTableDataSource(res.allCourseSectionViewList);     
        this.courseSectionList.paginator = this.paginator;
      }
    })
  }

  findMarkingPeriodTitleById(courseSectionList){

    courseSectionList=courseSectionList.map((item)=>{
      if(item.yrMarkingPeriodId){
        item.yrMarkingPeriodId='0_'+item.yrMarkingPeriodId;
      }else if(item.smstrMarkingPeriodId){
        item.smstrMarkingPeriodId='1_'+item.smstrMarkingPeriodId;
      }else if(item.qtrMarkingPeriodId){
        item.qtrMarkingPeriodId='2_'+item.qtrMarkingPeriodId;
      }
      
      if(item.yrMarkingPeriodId || item.smstrMarkingPeriodId || item.qtrMarkingPeriodId){
        for(let markingPeriod of this.getMarkingPeriodTitleListModel.getMarkingPeriodView){
          if(markingPeriod.value==item.yrMarkingPeriodId){
            item.markingPeriodTitle=markingPeriod.text;
            break;
          }else if(markingPeriod.value==item.smstrMarkingPeriodId){
            item.markingPeriodTitle=markingPeriod.text;
            break;
          }else{
            item.markingPeriodTitle=markingPeriod.text;
            break;
          }
        }
      }else{
        item.markingPeriodTitle='Custom'
      }
      return item;
    });
    return courseSectionList;
  }

  /** Whether the number of selected elements matches the total number of rows. */
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    return numSelected === this.courseSectionList.paginator.pageSize;
  }

  checked(row: any){
    this.selection.select(row)
    var found = this.selection.selected.find(x => x.courseSectionId == row.courseSectionId);
    if (found){
      return true;
    }
    }
  
    unChecked(row: any){
      var found = this.selection.selected.find(x => x.courseSectionId == row.courseSectionId);
      // if (found) found.checked = false;
      this.selection.deselect(found); 
  
      if (found){
        return false;
      }
    }
  
    isChecked(row: any) {
      var found = this.selection.selected.find(x => x.courseSectionId == row.courseSectionId);
  
      if (found){
        return true;
      }
    }

     /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    this.isAllSelected() ?
        this.selection.clear() :
        this.selectRows();
    
  }

  selectRows() {
    for (let index = 0; index < this.courseSectionList.paginator.pageSize; index++) {
      this.selection.select(this.courseSectionList.data[index]);
      // this.selectionAmount = this.selection.selected.length;
    }
  }

  selectedCourseSection(){
    console.log(this.selection.selected);
    if(this.selection.selected.length>0){
      this.dialogRef.close(this.selection.selected);
    }else{
      this.snackbar.open('Please select at least 1 course section', '', {
        duration: 2000
      });
    }
  }

  ngOnDestroy() {
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }
}
