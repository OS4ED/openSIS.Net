import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import {
  StudentAddModel,
  StudentListModel,
  StudentResponseListModel,
  GetAllStudentDocumentsList,
  StudentDocumentAddModel,
  StudentSiblingSearch,
  StudentSiblingAssociation,
  StudentViewSibling,
  CheckStudentInternalIdViewModel,
  StudentEnrollmentModel,
  StudentEnrollmentSchoolListModel,
  StudentImportModel,
  StudentName
} from '../models/student.model';
import { StudentCommentsAddView, StudentCommentsListViewModel } from '../models/student-comments.model';
import { BehaviorSubject, Subject } from 'rxjs';
import { CryptoService } from './Crypto.service';
import { DefaultValuesService } from '../common/default-values.service';
import { SchoolCreate } from '../enums/school-create.enum';
@Injectable({
  providedIn: 'root'
})
export class StudentService {
  studentCreate = SchoolCreate;
  apiUrl: string = environment.apiURL;
  private currentYear = new BehaviorSubject(false);
  currentY = this.currentYear.asObservable();
  
  private studentCreateMode = new BehaviorSubject(this.studentCreate.ADD);
  studentCreatedMode = this.studentCreateMode.asObservable();

  private studentDetailsForViewAndEdit = new BehaviorSubject(false);
  studentDetailsForViewedAndEdited = this.studentDetailsForViewAndEdit.asObservable();
  
  private categoryId = new Subject();
  categoryIdSelected = this.categoryId.asObservable();

  private categoryTitle = new BehaviorSubject(null);
  selectedCatgoryTitle = this.categoryTitle.asObservable();

  private dataAfterSavingGeneralInfo = new BehaviorSubject(0);
  dataAfterSavingGeneralInfoChanged = this.dataAfterSavingGeneralInfo.asObservable();


  studentName: StudentName;
  private isFirstView: boolean = true;
  constructor(
    private http: HttpClient,
    private cryptoService: CryptoService,
    private defaultValuesService: DefaultValuesService) { }

  AddStudent(obj: StudentAddModel) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.studentMaster.tenantId = this.defaultValuesService.getTenantID();
    obj.studentMaster.schoolId = this.defaultValuesService.getSchoolID();
    obj.passwordHash = this.cryptoService.encrypt(obj.passwordHash);
    const apiurl = this.apiUrl + obj._tenantName + '/Student/addStudent';
    obj.studentMaster.studentPhoto = this.studentImage;
    return this.http.post<StudentAddModel>(apiurl, obj);
  }

  viewStudent(obj: StudentAddModel) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.studentMaster.tenantId = this.defaultValuesService.getTenantID();
    obj.studentMaster.schoolId = this.defaultValuesService.getSchoolID();
    const apiurl = this.apiUrl + obj._tenantName + '/Student/viewStudent';
    return this.http.post<StudentAddModel>(apiurl, obj);
  }

  UpdateStudent(obj: StudentAddModel) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    // obj.studentMaster.tenantId = this.defaultValuesService.getTenantID();
    // obj.studentMaster.schoolId = this.defaultValuesService.getSchoolID();
    obj.passwordHash = this.cryptoService.encrypt(obj.passwordHash);
    const apiurl = this.apiUrl + obj._tenantName + '/Student/updateStudent';
    obj.studentMaster.studentPhoto = this.studentImage;
    return this.http.put<StudentAddModel>(apiurl, obj);
  }

  GetAllStudentList(obj: StudentListModel) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    const apiurl = this.apiUrl + obj._tenantName + '/Student/getAllStudentList';
    return this.http.post<StudentResponseListModel>(apiurl, obj);
  }

  checkStudentInternalId(obj: CheckStudentInternalIdViewModel) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    const apiurl = this.apiUrl + obj._tenantName + '/Student/checkStudentInternalId';
    return this.http.post<CheckStudentInternalIdViewModel>(apiurl, obj);
  }



  private category = new Subject;
  categoryToSend = this.category.asObservable();

  changeCategory(category: number) {
    this.category.next(category);
  }

  private studentDetails;
  setStudentDetails(data) {
    this.studentDetails = data;
  }
  getStudentDetails() {
    return this.studentDetails;
  }

  private studentId: number;
  setStudentId(id: number) {
    this.studentId = id;
  }
  getStudentId() {
    return this.studentId;
  }

  private studentMultiselectValue: any;
  setStudentMultiselectValue(value: any) {
    this.studentMultiselectValue = value;
  }
  getStudentMultiselectValue() {
    return this.studentMultiselectValue;
  }

  private studentImage;
  setStudentImage(imageInBase64) {
    this.studentImage = imageInBase64;
  }

  // Update Mode in Student
  public pageMode = new Subject;
  modeToUpdate = this.pageMode.asObservable();

  changePageMode(mode: number) {
    this.pageMode.next(mode);
  }

  // for cancel after Student photo added
  public cloneStudentImage;
  setStudentCloneImage(image) {
    this.cloneStudentImage = image;
  }
  getStudentCloneImage() {
    return this.cloneStudentImage;
  }

  // Student comment
  addStudentComment(obj: StudentCommentsAddView) {
    obj.studentComments.tenantId = this.defaultValuesService.getTenantID();
    obj.studentComments.schoolId = this.defaultValuesService.getSchoolID();
    obj.studentComments.updatedBy = this.defaultValuesService.getUserName();
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    const apiurl = this.apiUrl + obj._tenantName + '/Student/addStudentComment';
    return this.http.post<StudentCommentsAddView>(apiurl, obj);
  }
  updateStudentComment(obj: StudentCommentsAddView) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.studentComments.tenantId = this.defaultValuesService.getTenantID();
    obj.studentComments.schoolId = this.defaultValuesService.getSchoolID();
    obj.studentComments.updatedBy = this.defaultValuesService.getUserName();
    const apiurl = this.apiUrl + obj._tenantName + '/Student/updateStudentComment';
    return this.http.put<StudentCommentsAddView>(apiurl, obj);
  }
  deleteStudentComment(obj: StudentCommentsAddView) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.studentComments.tenantId = this.defaultValuesService.getTenantID();
    obj.studentComments.schoolId = this.defaultValuesService.getSchoolID();
    obj.studentComments.updatedBy = this.defaultValuesService.getUserName();
    const apiurl = this.apiUrl + obj._tenantName + '/Student/deleteStudentComment';
    return this.http.post<StudentCommentsAddView>(apiurl, obj);
  }
  getAllStudentCommentsList(obj: StudentCommentsListViewModel) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    const apiurl = this.apiUrl + obj._tenantName + '/Student/getAllStudentCommentsList';
    return this.http.post<StudentCommentsListViewModel>(apiurl, obj);
  }

  // to Update student details in General for first time.
  private studentDetailsForGeneralInfo = new Subject();
  getStudentDetailsForGeneral = this.studentDetailsForGeneralInfo.asObservable();

  sendDetails(studentDetailsForGeneralInfo) {
    this.studentDetailsForGeneralInfo.next(studentDetailsForGeneralInfo);
  }

  GetAllStudentDocumentsList(obj: GetAllStudentDocumentsList) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    const apiurl = this.apiUrl + obj._tenantName + '/Student/getAllStudentDocumentsList';
    return this.http.post<GetAllStudentDocumentsList>(apiurl, obj);
  }
  AddStudentDocument(obj: StudentDocumentAddModel) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    const apiurl = this.apiUrl + obj._tenantName + '/Student/addStudentDocument';
    return this.http.post<StudentDocumentAddModel>(apiurl, obj);
  }
  DeleteStudentDocument(obj: StudentDocumentAddModel) {
    const apiurl = this.apiUrl + obj._tenantName + '/Student/deleteStudentDocument';
    return this.http.post<StudentDocumentAddModel>(apiurl, obj);
  }

  // Student Sibling
  siblingSearch(searchDetails: StudentSiblingSearch) {
    searchDetails.tenantId= this.defaultValuesService.getTenantID();
    searchDetails._tenantName= this.defaultValuesService.getTenent();
    searchDetails._userName= this.defaultValuesService.getUserName();
    searchDetails._token= this.defaultValuesService.getToken();
    const apiurl = this.apiUrl + searchDetails._tenantName + '/Student/siblingSearch';
    return this.http.post<StudentSiblingSearch>(apiurl, searchDetails);
  }
  associationSibling(studentDetails: StudentSiblingAssociation) {
    studentDetails = this.defaultValuesService.getAllMandatoryVariable(studentDetails);
    studentDetails.studentMaster.tenantId = this.defaultValuesService.getTenantID();
    const apiurl = this.apiUrl + studentDetails._tenantName + '/Student/associationSibling';
    return this.http.post<StudentSiblingAssociation>(apiurl, studentDetails);
  }
  viewSibling(studentDetails: StudentViewSibling) {
    studentDetails = this.defaultValuesService.getAllMandatoryVariable(studentDetails);
    const apiurl = this.apiUrl + studentDetails._tenantName + '/Student/viewSibling';
    return this.http.post<StudentViewSibling>(apiurl, studentDetails);
  }
  removeSibling(studentDetails: StudentSiblingAssociation) {
    studentDetails = this.defaultValuesService.getAllMandatoryVariable(studentDetails);
    studentDetails.studentMaster.tenantId = this.defaultValuesService.getTenantID();
    studentDetails.studentMaster.schoolId = this.defaultValuesService.getSchoolID();
    const apiurl = this.apiUrl + studentDetails._tenantName + '/Student/removeSibling';
    return this.http.post<StudentSiblingAssociation>(apiurl, studentDetails);
  }

  // Student Enrollment

  updateStudentEnrollment(studentDetails: StudentEnrollmentModel) {
    studentDetails = this.defaultValuesService.getAllMandatoryVariable(studentDetails);
    const apiurl = this.apiUrl + studentDetails._tenantName + '/Student/updateStudentEnrollment';
    return this.http.put<StudentEnrollmentModel>(apiurl, studentDetails);
  }

  getAllStudentEnrollment(studentDetails: StudentEnrollmentModel) {
    studentDetails = this.defaultValuesService.getAllMandatoryVariable(studentDetails);
    const apiurl = this.apiUrl + studentDetails._tenantName + '/Student/getAllStudentEnrollment';
    return this.http.post<StudentEnrollmentModel>(apiurl, studentDetails);
  }

  studentEnrollmentSchoolList(schoolDetails: StudentEnrollmentSchoolListModel) {
    schoolDetails = this.defaultValuesService.getAllMandatoryVariable(schoolDetails);
    const apiurl = this.apiUrl + schoolDetails._tenantName + '/School/studentEnrollmentSchoolList';
    return this.http.post<StudentEnrollmentSchoolListModel>(apiurl, schoolDetails);
  }

  addUpdateStudentPhoto(obj: StudentAddModel) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.studentMaster.studentId = this.getStudentId();
    obj.studentMaster.studentPhoto = this.studentImage;
    const apiurl = this.apiUrl + obj._tenantName + '/Student/addUpdateStudentPhoto';
    return this.http.put<StudentAddModel>(apiurl, obj);
  }

  searchStudentListForReenroll(obj: StudentListModel, schoolId) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    obj.schoolId = +schoolId;
    const apiurl = this.apiUrl + obj._tenantName + '/Student/searchStudentListForReenroll';
    return this.http.post<StudentResponseListModel>(apiurl, obj);
  }

  reenrollmentForStudent(obj: StudentListModel) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    const apiurl = this.apiUrl + obj._tenantName + '/Student/reenrollmentForStudent';
    return this.http.post<StudentListModel>(apiurl, obj);
  }
  addStudentList(obj: StudentImportModel) {
    obj = this.defaultValuesService.getAllMandatoryVariable(obj);
    const apiurl = this.apiUrl + obj._tenantName + '/Student/addStudentList';
    return this.http.post<StudentImportModel>(apiurl, obj);
  }

  setStudentName(studentName) {
    this.studentName = {
      firstGivenName: studentName.split('|')[0],
      middleName: studentName.split('|')[1],
      lastFamilyName: studentName.split('|')[2]
    }
  }
  getStudentName() {
    return this.studentName;
  }

  setStudentFirstView(status){
    this.isFirstView=status;
  }
  getStudentFirstView(){
    return this.isFirstView;
  }

  setStudentCreateMode(data) {
    this.studentCreateMode.next(data);
  }

  setStudentDetailsForViewAndEdit(data) {
    this.studentDetailsForViewAndEdit.next(data);
  }

  setCategoryId(data) {
    this.categoryId.next(data);
  }

  setCategoryTitle(title :string){
    this.categoryTitle.next(title);
  }
  
  setDataAfterSavingGeneralInfo(data) {
    this.dataAfterSavingGeneralInfo.next(data);
  }

}
