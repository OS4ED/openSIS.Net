import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { MatCheckbox } from '@angular/material/checkbox';
import icClose from '@iconify/icons-ic/twotone-close';
import { StudentDetails } from '../../../../models/studentDetailsModel';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { TranslateService } from '@ngx-translate/core';
import { StudentService } from '../../../../services/student.service';
import { SectionService } from '../../../../services/section.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CommonService } from '../../../../services/common.service';
import { LoginService } from '../../../../services/login.service';
import { takeUntil } from 'rxjs/operators';
import { LanguageModel } from '../../../../models/languageModel';
import { NgForm } from '@angular/forms';
import { Subject } from 'rxjs';
import { StudentListModel, StudentMasterModel, StudentMasterSearchModel } from '../../../../models/studentModel';
import { SearchFilterAddViewModel } from '../../../../models/searchFilterModel';
import { GetAllSectionModel } from '../../../../models/sectionModel';
import { GetAllGradeLevelsModel } from '../../../../models/gradeLevelModel';
import { GradeLevelService } from '../../../../services/grade-level.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { LoaderService } from '../../../../services/loader.service';
import { SelectionModel } from '@angular/cdk/collections';
@Component({
  selector: 'vex-add-student',
  templateUrl: './add-student.component.html',
  styleUrls: ['./add-student.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms
  ]
})
export class AddStudentComponent implements OnInit,OnDestroy {
  languageList;
  sectionList = [];
  params=[];
  gradeLevelList= [];
  selectedList=[];
  studentName: string;
  totalCount:number=0;
  pageNumber:number;
  pageSize:number;
  searchRecord: boolean= false;
  loading:boolean;
  firstLanguageName: string;
  sectionName: string;
  gradeLavelName: string;
  studentMasterList: [StudentMasterModel]= [new StudentMasterModel()];
  languages: LanguageModel = new LanguageModel();
  @ViewChild('f') currentForm: NgForm;
  destroySubject$: Subject<void> = new Subject();
  studentMasterSearchModel: StudentMasterSearchModel = new StudentMasterSearchModel();
  getAllStudent: StudentListModel = new StudentListModel();
  searchFilterAddViewModel : SearchFilterAddViewModel= new SearchFilterAddViewModel();
  getAllGradeLevelsModel:GetAllGradeLevelsModel= new GetAllGradeLevelsModel();
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator; 
  @ViewChild('masterCheckbox') private masterCheckbox: MatCheckbox;
  studentDetails: MatTableDataSource<any>;
  selection : SelectionModel<StudentMasterModel> = new SelectionModel<StudentMasterModel>(true, []);
  icClose = icClose;
  displayedColumns: string[] = ['studentSelected', 'studentName', 'studentId', 'alternateId', 'gradeLevel', 'section', 'firstLanguage'];
  numRows:number;
  constructor(private dialogRef: MatDialogRef<AddStudentComponent>, public translateService:TranslateService,
    private studentService: StudentService,
    private snackbar: MatSnackBar,
    private sectionService: SectionService,
    private commonService: CommonService,
    private loginService: LoginService,
    private loaderService: LoaderService,
    private gradeLevelService: GradeLevelService) { 
    translateService.use('en');
    this.loaderService.isLoading.pipe(takeUntil(this.destroySubject$)).subscribe((val) => {
      this.loading = val;
    });
  }

  ngOnInit(): void {
    this.selection.isSelected = this.isChecked.bind(this);
    this.getAllLanguage();
    this.getAllSection();
    this.getAllGradeLevelList();
  }

//   isChecked(row: any): boolean {
//     const found = this.selection.selected.find((el) =>{
//        return el.studentId === row.studentId
//       });
//     if (found) {
//       return true;
//     }else{
//       console.log("Im in Unchecked")
//     }
//     return false;
//  }

 checked(row: any){
  this.selection.select(row)
  var found = this.selection.selected.find(x => x.studentId == row.studentId);
  if (found){
    return true;
  }
  }

  unChecked(row: any){
    var found = this.selection.selected.find(x => x.studentId == row.studentId);
    // if (found) found.checked = false;
    this.selection.deselect(found); 

    if (found){
      return false;
    }
  }

  isChecked(row: any) {
    var found = this.selection.selected.find(x => x.studentId == row.studentId);

    if (found){
      return true;
    }
  }
  getAllLanguage() {
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

  getAllSection() {
    let section: GetAllSectionModel = new GetAllSectionModel();
    this.sectionService.GetAllSection(section).pipe(takeUntil(this.destroySubject$)).subscribe(data => {
      if (data._failure) {
      }
      else {
        this.sectionList = data.tableSectionsList;
      }

    });
  }

  /** Whether the number of selected elements matches the total number of rows. */
  isAllSelected() {
    const numSelected = this.selection.selected.length;

    
    return numSelected === this.numRows;
    // const numSelected = this.selection.selected.length;
    // let count=0;
    // for(let i=0;i<this.studentDetails.data.length;i++){
    //   if(this.selection.selected.includes(this.studentDetails.data[i])){
    //     count=count+1
    //   }
    // }
    // const numRows = this.studentDetails.data.length;
    
    // return numSelected === numRows;
  }

  

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    this.isAllSelected() ?
        this.selection.clear() :
        this.studentDetails.data.forEach(row => this.selection.select(row));
    
  }



  getAllGradeLevelList(){   
    this.getAllGradeLevelsModel.schoolId = +sessionStorage.getItem("selectedSchoolId");
    this.getAllGradeLevelsModel._tenantName = sessionStorage.getItem("tenant");
    this.getAllGradeLevelsModel._token = sessionStorage.getItem("token");
    this.gradeLevelService.getAllGradeLevels(this.getAllGradeLevelsModel).subscribe(data => {          
      this.gradeLevelList=data.tableGradelevelList;      
    });
  }

  submit(){
    this.params = [];
    for (var key in this.studentMasterSearchModel) {
      if (this.studentMasterSearchModel.hasOwnProperty(key))
        if (this.studentMasterSearchModel[key] !== null) {
          this.params.push({ "columnName": key, "filterOption": 11, "filterValue": this.studentMasterSearchModel[key] })
        }
    }
    this.studentList();
  }

  getPageEvent(event){
   
    this.getAllStudent.pageNumber=event.pageIndex+1;
    this.getAllStudent.pageSize=event.pageSize;
    this.studentList();
    this.selection.isSelected = this.isChecked.bind(this);

  }

  studentList(){
    this.searchRecord= true;
    this.getAllStudent.filterParams = this.params;
    this.getAllStudent.sortingModel = null;
    this.getAllStudent.fullName = this.studentName;
    this.studentService.GetAllStudentList(this.getAllStudent).subscribe(data => {
      if(data._failure){
        if(data.studentMaster===null){
            this.studentDetails = new MatTableDataSource([]);   
            this.snackbar.open( data._message, '', {
              duration: 10000
            });
        } else{
          this.studentDetails = new MatTableDataSource([]); 
          this.totalCount= data.totalCount;  
        }
      }else{
        this.totalCount= data.totalCount;
        this.pageNumber = data.pageNumber;
        this.pageSize = data._pageSize;
        this.studentMasterList = data.studentMaster;

          for(let data of this.studentMasterList){
            this.languageList.map((val) => {
              var firstLanguageId = +data.firstLanguageId;
              if (val.langId === firstLanguageId) {
                data.firstLanguageName = val.locale;
              }
            });
            this.sectionList.map((val) => {
              var sectionId = +data.sectionId;
              if (val.sectionId === sectionId) {
                data.sectionName = val.name;
              }
            });
            }

        this.studentDetails = new MatTableDataSource(this.studentMasterList);
        this.numRows=this.totalCount;      
        this.getAllStudent=new StudentListModel();     
      }
    });
  }

  addStudent(){
    const numSelected = this.selection.selected;
    console.log(numSelected);
    this.dialogRef.close(numSelected);
  }

  ngOnDestroy() {
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }
}
