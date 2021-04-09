import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import icClose from '@iconify/icons-ic/twotone-close';
import { TranslateService } from '@ngx-translate/core';
import { GetAllProgramModel, GetAllSubjectModel, SearchCourseForScheduleModel } from '../../../../models/courseManagerModel';
import { CourseManagerService } from '../../../../services/course-manager.service';

@Component({
  selector: 'vex-add-course',
  templateUrl: './add-course.component.html',
  styleUrls: ['./add-course.component.scss']
})
export class AddCourseComponent implements OnInit {
  icClose = icClose;
  getAllProgramModel: GetAllProgramModel = new GetAllProgramModel();
  getAllSubjectModel: GetAllSubjectModel = new GetAllSubjectModel();
  searchCourseForScheduleModel: SearchCourseForScheduleModel = new SearchCourseForScheduleModel();
  searchedCourseDetails=[];
  constructor(private dialogRef: MatDialogRef<AddCourseComponent>, 
    public translateService:TranslateService,
    private courseManagerService:CourseManagerService,
    private fb: FormBuilder
    ) { 
    translateService.use('en');
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
                                                  
    this.courseManagerService.searchCourseForSchedule(this.searchCourseForScheduleModel).subscribe((res)=>{
      this.searchedCourseDetails=res.course;
    });
    
  }

  selectCourse(courseDetails){
    this.dialogRef.close(courseDetails)
  }

}
