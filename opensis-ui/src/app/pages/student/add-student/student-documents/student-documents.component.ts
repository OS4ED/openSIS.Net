import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { fadeInUp400ms } from '../../../../../@vex/animations/fade-in-up.animation';
import { stagger60ms } from '../../../../../@vex/animations/stagger.animation';
import { fadeInRight400ms } from '../../../../../@vex/animations/fade-in-right.animation';
import icEdit from '@iconify/icons-ic/twotone-edit';
import icDelete from '@iconify/icons-ic/twotone-delete';
import icSearch from '@iconify/icons-ic/twotone-search';
import icAdd from '@iconify/icons-ic/twotone-add';
import icFilterList from '@iconify/icons-ic/twotone-filter-list';
import { TranslateService } from '@ngx-translate/core';
import { MatTableDataSource } from '@angular/material/table';
import { LoaderService } from '../../../../services/loader.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import icMoreVert from '@iconify/icons-ic/twotone-more-vert';
import icComment from '@iconify/icons-ic/comment';
import icUpload from '@iconify/icons-ic/baseline-cloud-upload';
import {GetAllStudentDocumentsList} from '../../../../models/student.model';
import {StudentDocumentAddModel} from '../../../../models/student.model';
import {StudentService} from '../../../../services/student.service';
import {ConfirmDialogComponent } from '../../../shared-module/confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import * as _moment from 'moment';
import { default as _rollupMoment } from 'moment';
const moment =  _rollupMoment || _moment;
import { SchoolCreate } from '../../../../enums/school-create.enum';
import { RolePermissionListViewModel, RolePermissionViewModel } from '../../../../models/roll-based-access.model';
import { CryptoService } from '../../../../services/Crypto.service';
import { DefaultValuesService } from '../../../../common/default-values.service';

@Component({
  selector: 'vex-student-documents',
  templateUrl: './student-documents.component.html',
  styleUrls: ['./student-documents.component.scss'],
  animations: [
    stagger60ms,
    fadeInUp400ms,
    fadeInRight400ms
  ]
})
export class StudentDocumentsComponent implements OnInit {
  StudentCreate = SchoolCreate;
  studentCreateMode: SchoolCreate;
  studentDetailsForViewAndEdit;
  @Input()
  columns = [
    { label: 'File', property: 'fileUploaded', type: 'text', visible: true },
    { label: 'Uploaded By', property: 'uploadedBy', type: 'number', visible: true },
    { label: 'Uploaded On', property: 'uploadedOn', type: 'text', visible: true },
    { label: 'Action', property: 'action', type: 'text', visible: true }
  ];

  StudentDocumentsList;

  icEdit = icEdit;
  icDelete = icDelete;
  icAdd = icAdd;
  icComment = icComment;
  icMoreVert = icMoreVert;
  icSearch = icSearch;
  icFilterList = icFilterList;
  icUpload = icUpload;
  isShowDiv = true;
  loading: boolean;
  files: File[] = [];
  cloneFiles: File[] = [];
  base64;
  base64Arr = [];
  filesName = [];
  filesType = [];
  uploadSuccessfull = false;
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  studentDocument = [];
  getAllStudentDocumentsList: GetAllStudentDocumentsList = new GetAllStudentDocumentsList();
  StudentDocumentModelList: MatTableDataSource<any>;
  studentDocumentAddModel: StudentDocumentAddModel = new StudentDocumentAddModel();
  editPermission = false;
  deletePermission = false;
  addPermission = false;
  permissionListViewModel: RolePermissionListViewModel = new RolePermissionListViewModel();
  permissionGroup: RolePermissionViewModel = new RolePermissionViewModel();
  @ViewChild(MatSort) sort: MatSort;
  constructor(
    public translateService: TranslateService,
    private loaderService: LoaderService,
    private studentService: StudentService,
    private snackbar: MatSnackBar,
    private dialog: MatDialog,
    private defaultValuesService: DefaultValuesService,
    private cryptoService: CryptoService
  ) {
    translateService.use('en');
    this.loaderService.isLoading.subscribe((val) => {
       this.loading = val;
     });

  }
  ngOnInit(): void {
    this.studentService.studentCreatedMode.subscribe((res)=>{
      this.studentCreateMode = res;
    })
    this.studentService.studentDetailsForViewedAndEdited.subscribe((res)=>{
      this.studentDetailsForViewAndEdit = res;
    })

    this.permissionListViewModel = JSON.parse(this.cryptoService.dataDecrypt(localStorage.getItem('permissions')));
    this.permissionGroup = this.permissionListViewModel?.permissionList.find(x => x.permissionGroup.permissionGroupId === 3);
    const permissionCategory = this.permissionGroup.permissionGroup.permissionCategory.find(x => x.permissionCategoryId === 5);
    const permissionSubCategory = permissionCategory.permissionSubcategory.find( x => x.permissionSubcategoryId === 9);
    this.editPermission = permissionSubCategory.rolePermission[0].canEdit;
    this.deletePermission = permissionSubCategory.rolePermission[0].canDelete;
    this.addPermission = permissionSubCategory.rolePermission[0].canAdd;
    this.getAllDocumentsList();
  }
  // Split base 64 data from its datatype then push to base64 array
  HandleReaderLoaded(e) {
    const str = e.substr(e.indexOf(',') + 1);
    this.base64Arr.push(str);
  }
  onSelect(event) {
    this.files.push(...event.addedFiles);

    this.cloneFiles.push(...event.addedFiles)
    for(let i=0; i<this.cloneFiles.length;i++){
      this.filesName.push(this.cloneFiles[i].name);
      this.filesType.push(this.cloneFiles[i].type);
      const reader = new FileReader();
      reader.readAsDataURL(this.cloneFiles[i]);
      reader.onload = () => {
        this.HandleReaderLoaded(reader.result);
      };
    }
    this.cloneFiles = [];
  }

  onRemove(event) {
    this.files.splice(this.files.indexOf(event), 1);
    this.filesName.splice(this.files.indexOf(event), 1);
    this.filesType.splice(this.files.indexOf(event), 1);
    this.base64Arr.splice(this.files.indexOf(event), 1);
  }

  confirmDelete(deleteDetails)
  {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: '400px',
      data: {
          title: this.defaultValuesService.translateKey('areYouSure'),
          message: this.defaultValuesService.translateKey('youAreAboutToDeleteFile') + deleteDetails.filename + '.'}
    });

    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult){
        this.deleteFile(deleteDetails);
      }
    });
  }
  deleteFile(deleteDetails){
    const studentDocument = [];
    let obj = {};
    obj = {
      tenantId: deleteDetails.tenantId,
      schoolId: deleteDetails.schoolId,
      studentId: deleteDetails.studentId,
      documentId: deleteDetails.documentId,
      fileUploaded: deleteDetails.fileUploaded,
      uploadedOn: deleteDetails.uploadedOn,
      uploadedBy: deleteDetails.uploadedBy,
      studentMaster: null
    };
    studentDocument.push(obj);
    this.studentDocumentAddModel.studentDocuments = studentDocument;
    this.studentService.DeleteStudentDocument(this.studentDocumentAddModel).subscribe(data => {
      if (data){
        if (data._failure) {
          this.snackbar.open(data._message, '', {
            duration: 10000
          });
        } else {
          this.snackbar.open(data._message, '', {
            duration: 10000
          }).afterOpened().subscribe(() => {
            this.getAllDocumentsList();
          });
        }
      }else{
        this.snackbar.open(this.defaultValuesService.translateKey('fileDeletionfailed') + sessionStorage.getItem('httpError'), '', {
          duration: 10000
        });
      }
    });
  }

  uploadFile(){
    this.base64Arr.forEach((value, index) => {
        let obj = {};
        obj = {
            tenantId: this.defaultValuesService.getTenantID(),
            schoolId: this.defaultValuesService.getSchoolID(),
            studentId: this.studentDetailsForViewAndEdit.studentMaster.studentId,
            documentId: 0,
            filename: this.filesName[index],
            filetype: this.filesType[index],
            fileUploaded: value,
            uploadedBy: this.defaultValuesService.getEmailId(),
            studentMaster: null
          };
        this.studentDocument.push(obj);
      });

    if (this.studentDocument.length > 0){

        this.studentDocumentAddModel.studentDocuments = this.studentDocument;
        this.studentService.AddStudentDocument(this.studentDocumentAddModel).subscribe(data => {
          if (data){
            if (data._failure) {
              this.snackbar.open( data._message, '', {
                duration: 10000
              });
            } else {
              this.snackbar.open(data._message, '', {
                duration: 10000
              }).afterOpened().subscribe(() => {
                this.base64Arr = [];
                this.studentDocument = [];
                this.filesName = [];
                this.filesType = [];
                this.uploadSuccessfull = true;
                this.isShowDiv = true;
                this.getAllDocumentsList();
                this.files = [];
              });
            }
          }else{
            this.snackbar.open(this.defaultValuesService.translateKey('studentDocumentUploadfailed') + sessionStorage.getItem('httpError'), '', {
              duration: 10000
            });
          }
        });
      }
      else{
        this.snackbar.open(this.defaultValuesService.translateKey('pleaseSelectFile'), '', {
          duration: 1000
        });
      }

  }

  downloadFile(name, type, content){
    const fileType = 'data:' + type + ';base64,' + content;
    const element = document.createElement('a');
    element.setAttribute('href', fileType);
    element.setAttribute('download', name);
    element.style.display = 'none';
    document.body.appendChild(element);
    element.click();
    document.body.removeChild(element);
}
  getAllDocumentsList(){
    this.getAllStudentDocumentsList.studentId = this.studentDetailsForViewAndEdit.studentMaster.studentId;
    this.studentService.GetAllStudentDocumentsList(this.getAllStudentDocumentsList).subscribe(data => {
      if (data){
        if (data._failure){

          if (data.studentDocumentsList === null){
            this.StudentDocumentModelList = new MatTableDataSource([]) ;
            this.StudentDocumentModelList.sort = this.sort;
            this.snackbar.open( data._message, '', {
              duration: 10000
            });
          }
          else{
            this.StudentDocumentModelList = new MatTableDataSource([]) ;
            this.StudentDocumentModelList.sort = this.sort;
          }
        }
        else{
          this.StudentDocumentModelList = new MatTableDataSource(data.studentDocumentsList);
          this.StudentDocumentModelList.sort = this.sort;
        }
      }
      else{
        this.snackbar.open(this.defaultValuesService.translateKey('studentDocumentListfailed') + sessionStorage.getItem('httpError'), '', {
          duration: 10000
        });
      }

    });
  }

  toggleColumnVisibility(column, event) {
    event.stopPropagation();
    event.stopImmediatePropagation();
    column.visible = !column.visible;
  }

  toggleDisplayDiv() {
    this.isShowDiv = !this.isShowDiv;
    this.uploadSuccessfull = false;
  }

  get visibleColumns() {
    return this.columns.filter(column => column.visible).map(column => column.property);
  }

}
