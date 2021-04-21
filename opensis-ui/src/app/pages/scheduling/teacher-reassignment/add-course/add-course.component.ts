import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import icClose from '@iconify/icons-ic/twotone-close';
import { TranslateService } from '@ngx-translate/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { GetAllProgramModel, GetAllSubjectModel, SearchCourseForScheduleModel } from '../../../../models/course-manager.model';
import { CourseManagerService } from '../../../../services/course-manager.service';
import { LoaderService } from '../../../../services/loader.service';

@Component({
  selector: 'vex-add-course',
  templateUrl: './add-course.component.html',
  styleUrls: ['./add-course.component.scss']
})
export class AddCourseComponent implements OnInit,OnDestroy {
  icClose = icClose;
  getAllProgramModel: GetAllProgramModel = new GetAllProgramModel();
  getAllSubjectModel: GetAllSubjectModel = new GetAllSubjectModel();
  searchCourseForScheduleModel: SearchCourseForScheduleModel = new SearchCourseForScheduleModel();
  searchedCourseDetails=[];
  isSearchRecordAvailable=false;
  loading=false;
  destroySubject$: Subject<void> = new Subject();
  constructor(private dialogRef: MatDialogRef<AddCourseComponent>, 
    public translateService:TranslateService,
    private courseManagerService:CourseManagerService,
    private loaderService:LoaderService,
    private fb: FormBuilder
    ) { 
    translateService.use('en');
    this.loaderService.isLoading.pipe(takeUntil(this.destroySubject$)).subscribe((val:boolean) => {
      this.loading = val;
    });
  }
  form:FormGroup;

  ngOnInit(): void {
    this.getAllProgramList();
    this.getAllSubjectList();
    
    this.form=this.fb.group({
      subject:[''],
      program:['']
    })
  }

  
  getAllProgramList(){   
    this.courseManagerService.GetAllProgramsList(this.getAllProgramModel).subscribe(data => {          
      this.getAllProgramModel.programList=data.programList;      
    });
  }
  getAllSubjectList(){   
    this.courseManagerService.GetAllSubjectList(this.getAllSubjectModel).subscribe(data => {          
      this.getAllSubjectModel.subjectList=data.subjectList;      
    });  
  }

  SearchCourseForSchedule(){
    if(this.form.value.subject){
      this.searchCourseForScheduleModel.courseSubject=this.form.value.subject;
    }
    if(this.form.value.program){
      this.searchCourseForScheduleModel.courseProgram=this.form.value.program;
    }
    this.isSearchRecordAvailable=true;                                  
    this.courseManagerService.searchCourseForSchedule(this.searchCourseForScheduleModel).subscribe((res)=>{
      this.searchedCourseDetails=res.course;
    });
    
  }

  selectCourse(courseDetails){
    this.dialogRef.close(courseDetails)
  }

  ngOnDestroy() {
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }

}
