import { AfterViewInit, Component, Input, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import icSearch from '@iconify/icons-ic/search';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { fadeInUp400ms } from '../../../../../../@vex/animations/fade-in-up.animation';
import { stagger40ms } from '../../../../../../@vex/animations/stagger.animation';
import { ScheduleStudentForView, ScheduleStudentListViewModel } from '../../../../../models/studentCourseSectionScheduleAddViewModel';
import { LoaderService } from '../../../../../services/loader.service';
import { StudentScheduleService } from '../../../../../services/student-schedule.service';

@Component({
  selector: 'vex-scheduled-students',
  templateUrl: './scheduled-students.component.html',
  styleUrls: ['./scheduled-students.component.scss'],
  animations: [
    fadeInUp400ms,
    stagger40ms
  ]
})
export class ScheduledStudentsComponent implements OnInit, AfterViewInit{
  icSearch = icSearch;
  @Input() courseSectionDetails;
  displayedColumns = ['studentName', 'studentId', 'gradeLevel', 'scheduleDate'];
  @ViewChild(MatPaginator) paginator: MatPaginator;
  studentDetails: MatTableDataSource<ScheduleStudentForView>;
  getAllStudent: ScheduleStudentListViewModel = new ScheduleStudentListViewModel();
  searchCtrl: FormControl;
  loading:boolean;
  constructor(private studentScheduleService: StudentScheduleService,
    private snackbar: MatSnackBar,
    private loaderService:LoaderService) {
    this.getAllStudent.filterParams = null;
    this.loaderService.isLoading.subscribe((val) => {
      this.loading = val;
    });
  }
  totalCount: number = 0;
  pageNumber: number;
  pageSize: number;

  ngOnChanges(changes: SimpleChanges) {
    this.searchScheduledStudentForGroupDrop();
  }

  ngOnInit(): void {
    this.searchCtrl = new FormControl();
  }

  ngAfterViewInit() {
    //  Searching
    this.searchCtrl.valueChanges.pipe(debounceTime(500),distinctUntilChanged()).subscribe((term)=>{
      if(term!=''){
     let filterParams=[
       {
        columnName:null,
        filterValue:term,
        filterOption:1
       }
     ]
     Object.assign(this.getAllStudent,{filterParams: filterParams});
     this.getAllStudent.pageNumber=1;
     this.paginator.pageIndex=0;
     this.getAllStudent.pageSize=this.pageSize;
     this.searchScheduledStudentForGroupDrop();
    }else{
      Object.assign(this.getAllStudent,{filterParams: null});
      this.getAllStudent.pageNumber=this.paginator.pageIndex+1;
     this.getAllStudent.pageSize=this.pageSize;
     this.searchScheduledStudentForGroupDrop();

    }
      })
  }

  searchScheduledStudentForGroupDrop() {
    this.getAllStudent.courseSectionId = this.courseSectionDetails.courseSection.courseSectionId;
    this.studentScheduleService.searchScheduledStudentForGroupDrop(this.getAllStudent).subscribe((res) => {
      if (res._failure) {
        if (res.scheduleStudentForView === null) {
          this.studentDetails = new MatTableDataSource([]);
          this.snackbar.open(res._message, '', {
            duration: 5000
          });
        } else {
          this.studentDetails = new MatTableDataSource([]);
        }
      } else {
        this.totalCount=res.totalCount?res.totalCount:0;
        this.pageNumber = res.pageNumber;
        this.pageSize = res._pageSize;

        this.studentDetails = new MatTableDataSource(res.scheduleStudentForView);
      }
    })
  }

  getPageEvent(event){
    if(this.searchCtrl.value!=null && this.searchCtrl.value!=""){
      let filterParams=[
        {
         columnName:null,
         filterValue:this.searchCtrl.value,
         filterOption:1
        }
      ]
     Object.assign(this.getAllStudent,{filterParams: filterParams});
    }
    this.getAllStudent.pageNumber=event.pageIndex+1;
    this.getAllStudent.pageSize=event.pageSize;
    this.searchScheduledStudentForGroupDrop();
  }

}
