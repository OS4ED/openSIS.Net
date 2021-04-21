import { Component, OnInit, Input, ViewChild, OnDestroy } from '@angular/core';
import icMoreVert from '@iconify/icons-ic/twotone-more-vert';
import icAdd from '@iconify/icons-ic/baseline-add';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icDelete from '@iconify/icons-ic/twotone-delete';
import icSearch from '@iconify/icons-ic/search';
import icFilterList from '@iconify/icons-ic/filter-list';
import icImport from '@iconify/icons-ic/twotone-unarchive';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router} from '@angular/router';
import { fadeInUp400ms } from '../../../../@vex/animations/fade-in-up.animation';
import { stagger40ms } from '../../../../@vex/animations/stagger.animation';
import { TranslateService } from '@ngx-translate/core';
import { EditSchoolSpecificStandardComponent } from './edit-school-specific-standard/edit-school-specific-standard.component';
import { ViewDetailsComponent } from './view-details/view-details.component';
import { GradesService } from '../../../services/grades.service';
import { GetAllSchoolSpecificListModel, GradeStandardSubjectCourseListModel, SchoolSpecificStandarModel } from '../../../models/grades.model';
import { MatTableDataSource } from '@angular/material/table';
import { ConfirmDialogComponent } from '../../shared-module/confirm-dialog/confirm-dialog.component';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { debounceTime, distinctUntilChanged, takeUntil } from 'rxjs/operators';
import { ExcelService } from '../../../services/excel.service';
import { GetAllGradeLevelsModel } from '../../../models/grade-level.model';
import { GradeLevelService } from '../../../services/grade-level.service';
import { Subject } from 'rxjs';
import { LoaderService } from '../../../services/loader.service';
import { RolePermissionListViewModel, RolePermissionViewModel } from 'src/app/models/roll-based-access.model';
import { CryptoService } from 'src/app/services/Crypto.service';

@Component({
  selector: 'vex-school-specific-standards',
  templateUrl: './school-specific-standards.component.html',
  styleUrls: ['./school-specific-standards.component.scss'],
  animations: [
    fadeInUp400ms,
    stagger40ms
  ]
})
export class SchoolSpecificStandardsComponent implements OnInit,OnDestroy {
  
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort

  columns = [
    { label: 'Standard Ref No', property: 'standardRefNo', type: 'text', visible: true },
    { label: 'Subject', property: 'subject', type: 'text', visible: true },
    { label: 'Course', property: 'course', type: 'text', visible: true },
    { label: 'Grade', property: 'gradeLevel', type: 'text', visible: true },
    { label: 'Domain', property: 'domain', type: 'text', visible: true },
    { label: 'Topic', property: 'topic', type: 'text', visible: true },
    { label: 'Standard Details', property: 'standardDetails', type: 'text', visible: false },
    { label: 'Actions', property: 'actions', type: 'text', visible: true }
  ];

  schoolSpecificStandardsList:GetAllSchoolSpecificListModel=new GetAllSchoolSpecificListModel();
  icMoreVert = icMoreVert;
  icAdd = icAdd;
  icEdit = icEdit;
  icDelete = icDelete;
  icSearch = icSearch;
  icImport = icImport;
  icFilterList = icFilterList;

  gradeLevelList: GetAllGradeLevelsModel = new GetAllGradeLevelsModel();
  subjectList:GradeStandardSubjectCourseListModel=new GradeStandardSubjectCourseListModel();
  courseList:GradeStandardSubjectCourseListModel=new GradeStandardSubjectCourseListModel();
  form:FormGroup;
  loading:boolean;
  schoolSpecificList: MatTableDataSource<any>;
  totalCount;
  pageNumber;
  pageSize;
  searchCtrl: FormControl;
  updateExistingData=false;
  destroySubject$: Subject<void> = new Subject();
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();

  constructor(private router: Router,
    private dialog: MatDialog,
    public translateService:TranslateService,
    private gradesService:GradesService,
    private snackbar: MatSnackBar,
    private loaderService: LoaderService,
    private excelService:ExcelService,
    private fb: FormBuilder,
    private gradeLevelService: GradeLevelService,
    private cryptoService: CryptoService) {
    translateService.use('en');
    this.loaderService.isLoading.pipe(takeUntil(this.destroySubject$)).subscribe((val) => {
      this.loading = val;
    });
  }

  ngOnInit(): void {
    this.searchCtrl = new FormControl();
    this.form = this.fb.group({
      subject:['all',[Validators.required]],
      course:['all',[Validators.required]],
      gradeLevel:['all',[Validators.required]],
    })
    this.getAllSchoolSpecificList();
    this.getAllGradeLevel();
    this.getAllSubjectStandardList();
    this.getAllCourseStandardList();
    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 12);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 26);
    const permissionSubCategory = permissionCategory.permissionSubcategory.find( x => x.permissionSubcategoryId === 28);
    this.editPermission = permissionSubCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionSubCategory.rolePermission[0].canDelete;
    this.addPermission = permissionSubCategory.rolePermission[0].canAdd;
  }
  ngAfterViewInit() {
    //  Sorting
    this.schoolSpecificStandardsList = new GetAllSchoolSpecificListModel();
    this.sort.sortChange.subscribe((res) => {
      this.schoolSpecificStandardsList.pageNumber = this.pageNumber
      this.schoolSpecificStandardsList.pageSize = this.pageSize;
      this.schoolSpecificStandardsList.sortingModel.sortColumn = res.active;
      if (this.searchCtrl.value != null && this.searchCtrl.value != "") {
        let filterParams = [
          {
            columnName: null,
            filterValue: this.searchCtrl.value,
            filterOption: 3
          }
        ]
        Object.assign(this.schoolSpecificStandardsList, { filterParams: filterParams });
      }
      if (res.direction == "") {
        this.schoolSpecificStandardsList.sortingModel = null;
        this.getAllSchoolSpecificList();
        this.schoolSpecificStandardsList = new GetAllSchoolSpecificListModel();
        this.schoolSpecificStandardsList.sortingModel = null;
      } else {
        this.schoolSpecificStandardsList.sortingModel.sortDirection = res.direction;
        this.getAllSchoolSpecificList();
      }
    });

    //  Searching
    this.searchCtrl.valueChanges.pipe(debounceTime(500), distinctUntilChanged()).subscribe((term) => {
      if (term != '') {
        this.searchWithTerm(term)
      } else {
        this.searchWithoutTerm()
      }
    })
  }

  searchWithTerm(term) {
    let filterParams = [
      {
        columnName: null,
        filterValue: term,
        filterOption: 3
      }
    ]
    if (this.sort.active != undefined && this.sort.direction != "") {
      this.schoolSpecificStandardsList.sortingModel.sortColumn = this.sort.active;
      this.schoolSpecificStandardsList.sortingModel.sortDirection = this.sort.direction;
    }
    Object.assign(this.schoolSpecificStandardsList, { filterParams: filterParams });
    this.schoolSpecificStandardsList.pageNumber = 1;
    this.paginator.pageIndex = 0;
    this.schoolSpecificStandardsList.pageSize = this.pageSize;
    this.getAllSchoolSpecificList();
  }

  searchWithoutTerm() {
    Object.assign(this.schoolSpecificStandardsList, { filterParams: null });
    this.schoolSpecificStandardsList.pageNumber = this.paginator.pageIndex + 1;
    this.schoolSpecificStandardsList.pageSize = this.pageSize;
    if (this.sort.active != undefined && this.sort.direction != "") {
      this.schoolSpecificStandardsList.sortingModel.sortColumn = this.sort.active;
      this.schoolSpecificStandardsList.sortingModel.sortDirection = this.sort.direction;
    }
    this.getAllSchoolSpecificList();
  }

  getPageEvent(event) {
    if (this.sort.active != undefined && this.sort.direction != "") {
      this.schoolSpecificStandardsList.sortingModel.sortColumn = this.sort.active;
      this.schoolSpecificStandardsList.sortingModel.sortDirection = this.sort.direction;
    }
    if (this.searchCtrl.value != null && this.searchCtrl.value != "") {
      let filterParams = [
        {
          columnName: null,
          filterValue: this.searchCtrl.value,
          filterOption: 3
        }
      ]
      Object.assign(this.schoolSpecificStandardsList, { filterParams: filterParams });
    }
    this.schoolSpecificStandardsList.pageNumber = event.pageIndex + 1;
    this.schoolSpecificStandardsList.pageSize = event.pageSize;
    this.getAllSchoolSpecificList();
  }

  openViewDetails(viewDetails) {
    this.dialog.open(ViewDetailsComponent, {
      data: {
        details: viewDetails
      },
      width: '600px'
    });
  }

  openAdd(){
    this.dialog.open(EditSchoolSpecificStandardComponent, {
      data: {
        editMode: false,
      },
      width: '500px'
    }).afterClosed().subscribe((res) => {
      if (res) {
       this.getAllSchoolSpecificList();
       this.updateExistingData=true;
      }
    });
  }

  openEdit(schoolSpecificDetails){
    this.dialog.open(EditSchoolSpecificStandardComponent, {
      data: {
        editMode: true,
        schoolSpecificStandards: schoolSpecificDetails
      },
      width: '500px'
    }).afterClosed().subscribe((res) => {
      if (res) {
        this.getAllSchoolSpecificList();
        this.updateExistingData=true;
      }
    });
  }

  confirmDelete(deleteDetails) {
    // call our modal window
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: {
        title: "Are you sure?",
        message: "You are about to delete a standard."
      }
    });
    // listen to response
    dialogRef.afterClosed().subscribe(dialogResult => {
      // if user pressed yes dialogResult will be true, 
      // if user pressed no - it will be false
      if (dialogResult) {
        this.deleteSchoolSpecificStandard(deleteDetails);
      }
    });
  }

  deleteSchoolSpecificStandard(deleteDetails){
  let schoolSpecificStandard = new SchoolSpecificStandarModel(); 
  schoolSpecificStandard.gradeUsStandard.standardRefNo=deleteDetails.standardRefNo;
  schoolSpecificStandard.gradeUsStandard.gradeStandardId=deleteDetails.gradeStandardId;
    this.gradesService.deleteGradeUsStandard(schoolSpecificStandard).subscribe((res)=>{
      if (typeof (res) == 'undefined') {
        this.snackbar.open('Failed to Delete School Specific Standard ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }else
      if (res._failure) {
        this.snackbar.open('Failed to Delete School Specific Standard ' + res._message, 'LOL THANKS', {
          duration: 10000
        });
      } else {
        this.updateExistingData=true;
        this.getAllSchoolSpecificList();
        this.snackbar.open('School Specific Standard Deleted Successfully.', '', {
          duration: 10000
        });
      }
    })
  }

  getAllSchoolSpecificList(){
    if (this.schoolSpecificStandardsList.sortingModel?.sortColumn == "") {
      this.schoolSpecificStandardsList.sortingModel=null;
    }
    this.gradesService.getAllGradeUsStandardList(this.schoolSpecificStandardsList).subscribe(res => {
      if (res._failure) {
        this.snackbar.open('School Specific Standard List failed. ' + res._message, 'LOL THANKS', {
          duration: 10000
        });
      } else {
        this.totalCount = res.totalCount;
        this.pageNumber = res.pageNumber;
        this.pageSize = res._pageSize;
        this.schoolSpecificList = new MatTableDataSource(res.gradeUsStandardList);
        this.schoolSpecificStandardsList=new GetAllSchoolSpecificListModel();

        if(this.updateExistingData){
          this.getAllSubjectStandardList();
           this.getAllCourseStandardList();
        }
      }
    });
    
  }

  filterSchoolSpecificStandardsList(){
    this.form.markAllAsTouched();
    if(this.form.valid){
        let filterParams= [
          {
            columnName: "subject",
            filterValue: this.form.value.subject=="all"?null:this.form.value.subject,
            filterOption: 11
          },
          {
            columnName: "course",
            filterValue: this.form.value.course=="all"?null:this.form.value.course,
            filterOption: 11
          },
          {
            columnName: "gradeLevel",
            filterValue: this.form.value.gradeLevel=="all"?null:this.form.value.gradeLevel,
            filterOption: 11
          }
        ]
        Object.assign(this.schoolSpecificStandardsList, { filterParams: filterParams });
        this.getAllSchoolSpecificList();      
    }
  }

  translateKey(key) {
    let trnaslateKey;
    this.translateService.get(key).subscribe((res: string) => {
       trnaslateKey = res;
    });
    return trnaslateKey;
  }

  exportSchoolSpecificStandardsListToExcel() {
    let schoolSpecificStandardsList=new GetAllSchoolSpecificListModel();
    schoolSpecificStandardsList.pageNumber = 0;
    schoolSpecificStandardsList.pageSize = 0;
    schoolSpecificStandardsList.sortingModel=null;
    this.gradesService.getAllGradeUsStandardList(schoolSpecificStandardsList).subscribe(res => {
      if (res._failure) {
        this.snackbar.open('Failed to Export School Specific Standards List.' + res._message, 'LOL THANKS', {
          duration: 10000
        });
      } else {
        if (res.gradeUsStandardList?.length > 0) {
          let StandardsList = res.gradeUsStandardList?.map((item) => {
            return {
                [this.translateKey('standardRefNo')]: item.standardRefNo,
                [this.translateKey('subject')]: item.subject,
                [this.translateKey('course')]: item.course,
                [this.translateKey('gradeLevel')]: item.gradeLevel,
                [this.translateKey('domain')]: item.domain? item.domain:'-',
                [this.translateKey('topic')]: item.topic,
                [this.translateKey('standardDetails')]: item.standardDetails
            }
          });
          this.excelService.exportAsExcelFile(StandardsList, 'School_Specific_Standards_List_')
        } else {
          this.snackbar.open('No Records Found. Failed to Export School Specific Standards List', 'LOL THANKS', {
            duration: 5000
          });
        }
      }
    });

  }

  getAllGradeLevel() {
    this.gradeLevelService.getAllGradeLevels(this.gradeLevelList).subscribe((res) => {
      if (typeof (res) == 'undefined') {
        this.snackbar.open('Grade Level List failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (res._failure) {
            if (res.tableGradelevelList == null) {
              this.gradeLevelList.tableGradelevelList=[]
              this.snackbar.open( res._message, '', {
                duration: 10000
              });
            }
            else{
              this.gradeLevelList.tableGradelevelList=[]
            }

        }
        else {
          this.gradeLevelList=res;
        }
      }
    })
  }

  getAllSubjectStandardList(){
    this.subjectList=new GradeStandardSubjectCourseListModel();
    this.gradesService.getAllSubjectStandardList(this.subjectList).subscribe((res) => {
      if (typeof (res) == 'undefined') {
        this.snackbar.open('Standard Subject List failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (res._failure) {
            if (res.gradeUsStandardList == null) {
              this.subjectList.gradeUsStandardList = null
              this.snackbar.open(res._message, '', {
                duration: 10000
              });
            }
            else{
              this.subjectList.gradeUsStandardList = null
            }
        }
        else {
          this.subjectList=res;
        }
      }
    })
  }

  getAllCourseStandardList(){
    this.courseList=new GradeStandardSubjectCourseListModel();
    this.gradesService.getAllCourseStandardList(this.courseList).subscribe((res) => {
      if (typeof (res) == 'undefined') {
        this.snackbar.open('Standard Course List failed. ' + sessionStorage.getItem("httpError"), '', {
          duration: 10000
        });
      }
      else {
        if (res._failure) {
            if (res.gradeUsStandardList == null) {
              this.courseList.gradeUsStandardList=null
              this.snackbar.open( res._message, '', {
                duration: 10000
              });
            }
            else{
              this.courseList.gradeUsStandardList=null
            }
        }
        else {
          this.courseList=res;
        }
      }
    })
  }

  toggleColumnVisibility(column, event) {
    event.stopPropagation();
    event.stopImmediatePropagation();
    column.visible = !column.visible;
  }

  get visibleColumns() {
    return this.columns.filter(column => column.visible).map(column => column.property);
  }

  
  ngOnDestroy() {
    this.destroySubject$.next();
    this.destroySubject$.complete();
  }
}
