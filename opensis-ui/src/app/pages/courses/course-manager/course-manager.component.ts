import { Component, OnInit, Input } from '@angular/core';
import { fadeInUp400ms } from '../../../../@vex/animations/fade-in-up.animation';
import { stagger40ms } from '../../../../@vex/animations/stagger.animation';
import { TranslateService } from '@ngx-translate/core';
import icSettings from '@iconify/icons-ic/twotone-settings';
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
import { MatDialog } from '@angular/material/dialog';
import { ManageSubjectsComponent } from './manage-subjects/manage-subjects.component';
import { ManageProgramsComponent } from './manage-programs/manage-programs.component';
import { EditCourseComponent } from './edit-course/edit-course.component';
import { EditCourseSectionComponent } from './edit-course-section/edit-course-section.component';
import {GetAllCourseListModel,AddCourseModel,GetAllProgramModel,GetAllSubjectModel} from '../../../models/courseManagerModel';
import {CourseManagerService} from '../../../services/course-manager.service';
import {MatSnackBar} from  '@angular/material/snack-bar';
import { ConfirmDialogComponent } from '../../shared-module/confirm-dialog/confirm-dialog.component';
import {GradeLevelService} from '../../../services/grade-level.service';
import {GetAllGradeLevelsModel } from '../../../models/gradeLevelModel';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { LayoutService } from 'src/@vex/services/layout.service';
import { RolePermissionListViewModel, RolePermissionViewModel } from '../../../models/rollBasedAccessModel';
import { CryptoService } from '../../../services/Crypto.service';

@Component({
  selector: 'vex-course-manager',
  templateUrl: './course-manager.component.html',
  styleUrls: ['./course-manager.component.scss'],
  animations: [
    fadeInUp400ms,
    stagger40ms
  ]
})
export class CourseManagerComponent implements OnInit {
  @Input()
  columns = [
    { label: 'Student Name', property: 'student_name', type: 'text', visible: true },
    { label: 'Student ID', property: 'student_id', type: 'text', visible: true },
    { label: 'Grade Level', property: 'grade_level', type: 'text', visible: true },
    { label: 'Schedule Date', property: 'schedule_date', type: 'text', visible: true }
  ];
  selectedCourseDetails;
  EffortGradeScaleModelList;
  selectedOption = '1';
  icSettings = icSettings;
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
  showCourses = true;
  courseDetails = 0;
  icFilterList = icFilterList;
  icSearch = icSearch;
  loading:Boolean;
  selectedTab = 'scheduled_teachers';
  getAllCourseListModel: GetAllCourseListModel = new GetAllCourseListModel(); 
  addCourseModel:AddCourseModel= new AddCourseModel();
  getAllProgramModel: GetAllProgramModel = new GetAllProgramModel();
  getAllSubjectModel: GetAllSubjectModel = new GetAllSubjectModel();
  getAllGradeLevelsModel:GetAllGradeLevelsModel= new GetAllGradeLevelsModel();
  courseList=[];
  searchCtrl: FormControl;
  form:FormGroup;
  filterCourseList=[];
  filterCourseCount=0;
  globalFilterCourseList=[];
  selectedCourse={
    "tenantId": "",
    "schoolId": 0,
    "courseId": 0,
    "courseTitle": "",
    "courseShortName": "",
    "courseGradeLevel": "",
    "courseProgram": "",
    "courseSubject": "",
    "courseCategory": "",
    "creditHours": "",
    "courseDescription": "",
    "isCourseActive": false,
    "createdBy": "",
    "createdOn": "",
    "updatedBy": "",
    "updatedOn": ""
  };
  selectedCourseObj;
  nameSearch:string='';
  globalFilterCourseCount=0;
  courseIndex = 0;
  totalCourse= 0;
  programList=[];
  subjectList=[];
  gradeLevelList=[];
  noFilterMode=true; 
  courseListClone=[];
  standard;
  name;
  cloneFilterCourseList=[];
  selectedCourseFlag:boolean;
  showCourseSections:boolean=true;
  visibleColumns;
  selectedCourses:number=0;
  filterFlag:boolean=false;
  deletedCourse:number=0;
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();
  
  constructor(
    public translateService:TranslateService,
    private dialog: MatDialog,
    private gradeLevelService:GradeLevelService,
    private courseManager:CourseManagerService,
    private snackbar: MatSnackBar,
    private fb: FormBuilder,
    private layoutService: LayoutService,
    private cryptoService: CryptoService) {
      this.getAllCourse();
      translateService.use('en');
      if(localStorage.getItem("collapseValue") !== null){
        if( localStorage.getItem("collapseValue") === "false"){
          this.layoutService.expandSidenav();
        }else{
          this.layoutService.collapseSidenav();
        } 
      }else{
        this.layoutService.expandSidenav();
      }
    
    }

  ngOnInit(): void {
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 6);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 12);
    this.editPermission = permissionCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionCategory.rolePermission[0].canDelete;
    this.addPermission = permissionCategory.rolePermission[0].canAdd;

    this.searchCtrl = new FormControl();
    this.form = this.fb.group({
      subject:['all',[Validators.required]],
      program:['all',[Validators.required]],
      gradeLevel:['all',[Validators.required]],
    })
    this.getAllProgramList();
    this.getAllSubjectList();
    this.getAllGradeLevelList();
    
  }

  getAllCourse(){
    this.courseManager.GetAllCourseList(this.getAllCourseListModel).subscribe(data => {
      if(data._failure){
        if(data._message==="NO RECORD FOUND"){
          this.courseList=[];
          this.totalCourse = 0;
          this.courseListClone =[];
          for (const prop of Object.getOwnPropertyNames(this.selectedCourse)) {
            delete this.selectedCourse[prop];
          }
          this.snackbar.open('NO RECORD FOUND. ', '', {
            duration: 1000
          });        
        }
      }else{      
        this.courseList=data.courseViewModelList;
        this.courseListClone = data.courseViewModelList;           
        this.totalCourse = this.courseList.length;  
        var fl= false;
        if(this.totalCourse > 0){
          for(let i=0;i<this.totalCourse;i++){
            if(this.deletedCourse === this.selectedCourses){
              fl = true;
           }
          }       
          if(fl){
            this.selectedCourses = 0;
            this.selectedCourseObj = this.courseList[0].course;           
            this.standard = this.courseList[0].course.courseStandard;
            this.courseIndex = 0;
          }else{
            this.selectedCourse = data.courseViewModelList[this.selectedCourses];
            this.selectedCourseObj = this.courseList[this.selectedCourses].course;           
            this.standard = this.courseList[this.selectedCourses].course.courseStandard;
            this.courseIndex = this.selectedCourses;
          }       
        }      
      }
    });
  }

  getAllProgramList(){   
    this.courseManager.GetAllProgramsList(this.getAllProgramModel).subscribe(data => {          
      this.programList=data.programList;      
    });
  }
  getAllSubjectList(){   
    this.courseManager.GetAllSubjectList(this.getAllSubjectModel).subscribe(data => {          
      this.subjectList=data.subjectList;      
    });
  }
  getAllGradeLevelList(){   
    this.getAllGradeLevelsModel.schoolId = +sessionStorage.getItem("selectedSchoolId");
    this.getAllGradeLevelsModel._tenantName = sessionStorage.getItem("tenant");
    this.getAllGradeLevelsModel._token = sessionStorage.getItem("token");
    this.gradeLevelService.getAllGradeLevels(this.getAllGradeLevelsModel).subscribe(data => {          
      this.gradeLevelList=data.tableGradelevelList;      
    });
  }
  toggleColumnVisibility(column, event) {
    event.stopPropagation();
    event.stopImmediatePropagation();
    column.visible = !column.visible;
  }

 

  
  filterCourse(event,category){
  
    if(category === "Subject"){
      var subject  = event.value;
    }else{
      var subject  = this.form.value.subject;
    }
    if(category === "Program"){
      var program  = event.value; 
    }else{
      var program  = this.form.value.program; 
    }
    if(category === "Grade"){
      var gradeLevel  = event.value; 
    }else{
      var gradeLevel  = this.form.value.gradeLevel; 
    }
    
    if(subject !== "all" && program !== "all" && gradeLevel !== "all"){
      this.noFilterMode = false;
      var l_flag = "N";
      for(let i = 0;i< this.courseListClone.length ;i++){
        var obj1 = {};      
       
        if(subject == this.courseListClone[i].course.courseSubject && program == this.courseListClone[i].course.courseProgram && gradeLevel == this.courseListClone[i].course.courseGradeLevel ){
        
            var l_flag = "Y";             
            obj1["courseTitle"]= this.courseListClone[i].course.courseTitle,
            obj1["courseShortName"] = this.courseListClone[i].course.courseShortName,
            obj1["courseGradeLevel"] = this.courseListClone[i].course.courseGradeLevel,
            obj1["courseProgram"] = this.courseListClone[i].course.courseProgram,
            obj1["courseSubject"] = this.courseListClone[i].course.courseSubject,
            obj1["courseStandard"] = this.courseListClone[i].course.courseStandard,
            obj1["courseCategory"] = this.courseListClone[i].course.courseCategory,
            obj1["creditHours"] = this.courseListClone[i].course.creditHours,
            obj1["courseDescription"] = this.courseListClone[i].course.courseDescription,    
            this.filterCourseList.push(obj1);  
            this.cloneFilterCourseList.push(obj1);                  
          }
          
        }
        this.filterCourseCount = this.filterCourseList.length;   
        if(this.filterCourseList.length > 0){
          this.selectedCourseObj = this.filterCourseList[0]; 
          this.selectedCourse = this.filterCourseList[this.selectedCourses];          
          this.standard = this.filterCourseList[0].courseStandard;
          this.selectedCourse = this.filterCourseList[0]; 
          this.standard = this.filterCourseList[0].courseStandard;
          this.filterFlag = true;
        }     
        
        if(l_flag == "N"){
          this.totalCourse = 0;
          this.filterCourseList = [];
          this.cloneFilterCourseList = [];
          this.courseList=[];
          
        }
      }else{
        this.cloneFilterCourseList = [];
        this.filterCourseList = [];       
        this.getAllCourse();
      }
    }
  backToCourse(event) {
      this.showCourses = true;
      let courseIndex=this.courseList.findIndex((item)=>{
        return item.course.courseId==event.courseId;
      });
      if(courseIndex!=-1){
        this.courseList[courseIndex].courseSectionCount=event.courseSectionCount;
      }
  }

  courseSections(selectedCourseDetails) {
   this.selectedCourseDetails=selectedCourseDetails.course;
    this.showCourses = false;
  }

  closeCourseDetails() {
    this.courseDetails = 0;
  }

  showDetails(element,index) {
    this.courseIndex = index;    
    this.selectedCourseObj = element;    
    this.selectedCourses = index;
    this.standard = element.courseStandard;
  }
  editCourse(element){
  
    if(element.hasOwnProperty('courseSectionCount')){    
      Object.assign(element,element.course)
    }  
    
    this.dialog.open(EditCourseComponent, {
      width: '800px',
      data: {
        editDetails:element,
        mode:"EDIT"
      }
     }).afterClosed().subscribe(data => {
      if(data){
        this.getAllCourse();
      }     
    });
  }
  deleteCourse(element,index){   
    this.addCourseModel.course.courseId = element.courseId;   
    this.courseManager.DeleteCourse(this.addCourseModel).subscribe(data => {
      if (typeof (data) == 'undefined') {
        this.snackbar.open('Course Deletion failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (data._failure) {
          this.snackbar.open('Course Deletion failed. ' + data._message, 'LOL THANKS', {
            duration: 10000
          });
        } else {

          this.snackbar.open('Course Deletion Successful.', '', {
            duration: 10000
          }).afterOpened().subscribe(data => {
            this.deletedCourse = index;            
            this.getAllCourse();
          });
          
        }
      }
    });
  }
  openModalManageSubjects() {
    this.dialog.open(ManageSubjectsComponent, {
      width: '500px'
    }).afterClosed().subscribe((data) => {
      this.getAllSubjectList();
    });   
  }

  openModalManagePrograms() {
    this.dialog.open(ManageProgramsComponent, {
      width: '500px'
    }).afterClosed().subscribe((data) => {
      this.getAllProgramList();
    });   
  }
  confirmDelete(element,index){
    
    // call our modal window
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: {
          title: "Are you sure?",
          message: "You are about to delete "+element.courseTitle+"."}
    });
    // listen to response
    dialogRef.afterClosed().subscribe(dialogResult => {
      // if user pressed yes dialogResult will be true, 
      // if user pressed no - it will be false
      if(dialogResult){
        this.deleteCourse(element,index);
      }
   });
  }
  openModalEditCourse() {
    this.dialog.open(EditCourseComponent, {
      width: '800px',
      data: {
        editDetails:null,
        mode:"ADD"
      }
    }).afterClosed().subscribe(data => {
    if(data){
      this.getAllCourse();
    }
     
    });
  }


  changeTab(currentTab) {
    this.selectedTab = currentTab;
  }

}
