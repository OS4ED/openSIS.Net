/***********************************************************************************
openSIS is a free student information system for public and non-public
schools from Open Solutions for Education, Inc.Website: www.os4ed.com.

Visit the openSIS product website at https://opensis.com to learn more.
If you have question regarding this software or the license, please contact
via the website.

The software is released under the terms of the GNU Affero General Public License as
published by the Free Software Foundation, version 3 of the License.
See https://www.gnu.org/licenses/agpl-3.0.en.html.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

Copyright (c) Open Solutions for Education, Inc.

All rights reserved.
***********************************************************************************/

import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { MatCheckbox } from '@angular/material/checkbox';
import icClose from '@iconify/icons-ic/twotone-close';
import { StudentDetails } from '../../../../models/student-details.model';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { TranslateService } from '@ngx-translate/core';
import { StudentService } from '../../../../services/student.service';
import { SectionService } from '../../../../services/section.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CommonService } from '../../../../services/common.service';
import { LoginService } from '../../../../services/login.service';
import { takeUntil } from 'rxjs/operators';
import { LanguageModel } from '../../../../models/language.model';
import { NgForm } from '@angular/forms';
import { Subject } from 'rxjs';
import { StudentListModel, StudentMasterModel, StudentMasterSearchModel } from '../../../../models/student.model';
import { SearchFilterAddViewModel } from '../../../../models/search-filter.model';
import { GetAllSectionModel } from '../../../../models/section.model';
import { GetAllGradeLevelsModel } from '../../../../models/grade-level.model';
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
  @ViewChild('masterCheckBox') private masterCheckBox: MatCheckbox;
  listOfStudent=[];
  selectedStudent=[]
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
    this.getAllLanguage();
    this.getAllSection();
    this.getAllGradeLevelList();
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


  getAllGradeLevelList(){   
    this.getAllGradeLevelsModel.schoolId = +sessionStorage.getItem("selectedSchoolId");
    this.getAllGradeLevelsModel._tenantName = sessionStorage.getItem("tenant");
    this.getAllGradeLevelsModel._token = sessionStorage.getItem("token");
    this.gradeLevelService.getAllGradeLevels(this.getAllGradeLevelsModel).subscribe(data => {          
      this.gradeLevelList=data.tableGradelevelList;      
    });
  }


  someComplete():boolean{
    let indetermine=false;
      for(let user of this.listOfStudent){
        for(let selectedUser of this.selectedStudent){
          if(user.StudentId==selectedUser.StudentId){
            indetermine=true;
          }
        }
      }
      if(indetermine){
        this.masterCheckBox.checked=this.listOfStudent.every((item)=>{
          return item.checked;
        })
        if(this.masterCheckBox.checked){
          return false;
        }else{
          return true;
        }
      }
      
  }
  
  setAll(event){
    this.listOfStudent.forEach(user => {user.checked = event;});
    this.studentDetails=new MatTableDataSource(this.listOfStudent);
    this.decideCheckUncheck();
  }
  
    onChangeSelection(eventStatus:boolean,id){
      for(let item of this.listOfStudent){
        if(item.studentId==id){
          item.checked=eventStatus;
          break;
        }
      }
      this.studentDetails=new MatTableDataSource(this.listOfStudent);
      this.masterCheckBox.checked=this.listOfStudent.every((item)=>{
        return item.checked;
      });
    
     this.decideCheckUncheck();
    }
  
  decideCheckUncheck(){
    this.listOfStudent.map((item)=>{
      let isIdIncludesInSelectedList=false;
      if(item.checked){
        for(let selectedUser of this.selectedStudent){
          if(item.studentId==selectedUser.studentId){
            isIdIncludesInSelectedList=true;
            break;
           }
        }
        if(!isIdIncludesInSelectedList){
          this.selectedStudent.push(item);
        }
      }else{
        for(let selectedUser of this.selectedStudent){
          if(item.studentId==selectedUser.studentId){
            this.selectedStudent=this.selectedStudent.filter((user)=>user.studentId!=item.studentId);
            break;
           }
        }
      }
      isIdIncludesInSelectedList=false;
      
    });
    this.selectedStudent=this.selectedStudent.filter((item)=>item.checked);
  }

  submit(){
    this.params = [];
    for (let key in this.studentMasterSearchModel) {
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

          for(const data of this.studentMasterList){
            this.languageList.map((val) => {
              let firstLanguageId = +data.firstLanguageId;
              if (val.langId === firstLanguageId) {
                data.firstLanguageName = val.locale;
              }
            });
            this.sectionList.map((val) => {
              let sectionId = +data.sectionId;
              if (val.sectionId === sectionId) {
                data.sectionName = val.name;
              }
            });
            }
        this.studentDetails = new MatTableDataSource(this.studentMasterList);


        this.studentMasterList.forEach(user => {
          user.checked=false
        });
        let response=this.studentMasterList.map((item)=>{
          this.selectedStudent.map((selectedUser)=>{
            if(item.studentId==selectedUser.studentId){
              item.checked=true;
              return item;
            }
          });
          return item;
        });
        this.listOfStudent=response;
        this.masterCheckBox.checked=response.every((item)=>{
          return item.checked;
        })


        this.getAllStudent=new StudentListModel();     
      }
    });
  }

  addStudent(){
    if(this.selectedStudent.length>0){
      this.dialogRef.close(this.selectedStudent);
    }else{
      this.snackbar.open('Please select at least 1 student', '', {
        duration: 2000
      });
    }
  }

  ngOnDestroy() {
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }
}
