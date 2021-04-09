using opensis.core.helper;
using opensis.core.Student.Interfaces;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.Student;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.core.Student.Services
{
    public class StudentService: IStudentService
    {
        private static string SUCCESS = "success";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly string TOKENINVALID = "Token not Valid";

        public IStudentRepository studentRepository;
        public StudentService(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }
        //Required for Unit Testing
        public StudentService() { }

        /// <summary>
        /// Student Add
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public StudentAddViewModel SaveStudent(StudentAddViewModel student)
        {
            StudentAddViewModel studentAddViewModel = new StudentAddViewModel();
            if (TokenManager.CheckToken(student._tenantName + student._userName, student._token))
            {
                studentAddViewModel = this.studentRepository.AddStudent(student);
            }
            else
            {
                studentAddViewModel._failure = true;
                studentAddViewModel._message = TOKENINVALID;
            }
            return studentAddViewModel;
        }

        /// <summary>
        /// Student Update
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public StudentAddViewModel UpdateStudent(StudentAddViewModel student)
        {
            StudentAddViewModel studentUpdate = new StudentAddViewModel();
            if (TokenManager.CheckToken(student._tenantName + student._userName, student._token))
            {

                studentUpdate = this.studentRepository.UpdateStudent(student);
            }
            else
            {
                studentUpdate._failure = true;
                studentUpdate._message = TOKENINVALID;
            }
            return studentUpdate;
        }

        /// <summary>
        /// Get All Student With Pagination,sorting,searching
        /// </summary>
        /// <param name="pageResult"></param>
        /// <returns></returns>

        public StudentListModel GetAllStudentList(PageResult pageResult)
        {
            logger.Info("Method getAllStudentList called.");
            StudentListModel studentList = new StudentListModel();
            try
            {
                if (TokenManager.CheckToken(pageResult._tenantName + pageResult._userName, pageResult._token))
                {
                    studentList = this.studentRepository.GetAllStudentList(pageResult);
                    if (studentList.studentMaster.Count > 0)
                    {
                        studentList._message = SUCCESS;
                        studentList._failure = false;
                    }
                    else
                    {
                        studentList._message = "NO RECORD FOUND";
                        studentList._failure = true;
                    }
                    logger.Info("Method getAllStudentList end with success.");
                }

                else
                {
                    studentList._failure = true;
                    studentList._message = TOKENINVALID;
                    return studentList;
                }
            }
            catch (Exception ex)
            {
                studentList._message = ex.Message;
                studentList._failure = true;
                logger.Error("Method getAllStudent end with error :" + ex.Message);
            }

            return studentList;
        }

        /// <summary>
        /// Save StudentDocument
        /// </summary>
        /// <param name="studentDocumentAddViewModel"></param>
        /// <returns></returns>
        public StudentDocumentAddViewModel SaveStudentDocument(StudentDocumentAddViewModel studentDocumentAddViewModel)
        {
            StudentDocumentAddViewModel studentDocumentAdd = new StudentDocumentAddViewModel();
            if (TokenManager.CheckToken(studentDocumentAddViewModel._tenantName + studentDocumentAddViewModel._userName, studentDocumentAddViewModel._token))
            {
                studentDocumentAdd = this.studentRepository.AddStudentDocument(studentDocumentAddViewModel);
            }
            else
            {
                studentDocumentAdd._failure = true;
                studentDocumentAdd._message = TOKENINVALID;
            }
            return studentDocumentAdd;
        }
        /// <summary>
        /// Update StudentDocument
        /// </summary>
        /// <param name="studentDocumentAddViewModel"></param>
        /// <returns></returns>
        public StudentDocumentAddViewModel UpdateStudentDocument(StudentDocumentAddViewModel studentDocumentAddViewModel)
        {
            StudentDocumentAddViewModel studentDocumentUpdate = new StudentDocumentAddViewModel();
            if (TokenManager.CheckToken(studentDocumentAddViewModel._tenantName + studentDocumentAddViewModel._userName, studentDocumentAddViewModel._token))
            {
                studentDocumentUpdate = this.studentRepository.UpdateStudentDocument(studentDocumentAddViewModel);
            }
            else
            {
                studentDocumentUpdate._failure = true;
                studentDocumentUpdate._message = TOKENINVALID;
            }
            return studentDocumentUpdate;
        }
        /// <summary>
        /// Get All StudentDocuments List
        /// </summary>
        /// <param name="studentDocumentListViewModel"></param>
        /// <returns></returns>
        public StudentDocumentListViewModel GetAllStudentDocumentsList(StudentDocumentListViewModel studentDocumentListViewModel)
        {
            StudentDocumentListViewModel studentDocumentsList = new StudentDocumentListViewModel();
            if (TokenManager.CheckToken(studentDocumentListViewModel._tenantName + studentDocumentListViewModel._userName, studentDocumentListViewModel._token))
            {
                studentDocumentsList = this.studentRepository.GetAllStudentDocumentsList(studentDocumentListViewModel);
            }
            else
            {
                studentDocumentsList._failure = true;
                studentDocumentsList._message = TOKENINVALID;
            }
            return studentDocumentsList;
        }
        /// <summary>
        /// Delete StudentDocument
        /// </summary>
        /// <param name="studentDocumentAddViewModel"></param>
        /// <returns></returns>
        public StudentDocumentAddViewModel DeleteStudentDocument(StudentDocumentAddViewModel studentDocumentAddViewModel)
        {
            StudentDocumentAddViewModel studentDocumentdelete = new StudentDocumentAddViewModel();
            if (TokenManager.CheckToken(studentDocumentAddViewModel._tenantName + studentDocumentAddViewModel._userName, studentDocumentAddViewModel._token))
            {
                studentDocumentdelete = this.studentRepository.DeleteStudentDocument(studentDocumentAddViewModel);
            }
            else
            {
                studentDocumentdelete._failure = true;
                studentDocumentdelete._message = TOKENINVALID;
            }
            return studentDocumentdelete;
        }

        /// <summary>
        /// Add Student Login Info
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public LoginInfoAddModel AddStudentLoginInfo(LoginInfoAddModel login)
        {
            LoginInfoAddModel loginInfo = new LoginInfoAddModel();
            if (TokenManager.CheckToken(login._tenantName + login._userName, login._token))
            {

                loginInfo = this.studentRepository.AddStudentLoginInfo(login);
            }
            else
            {
                loginInfo._failure = true;
                loginInfo._message = TOKENINVALID;
            }
            return loginInfo;
        }

        /// <summary>
        /// Save StudentComment
        /// </summary>
        /// <param name="studentCommentAddViewModel"></param>
        /// <returns></returns>
        public StudentCommentAddViewModel SaveStudentComment(StudentCommentAddViewModel studentCommentAddViewModel)
        {
            StudentCommentAddViewModel studentCommentAdd = new StudentCommentAddViewModel();
            if (TokenManager.CheckToken(studentCommentAddViewModel._tenantName + studentCommentAddViewModel._userName, studentCommentAddViewModel._token))
            {
                studentCommentAdd = this.studentRepository.AddStudentComment(studentCommentAddViewModel);
            }
            else
            {
                studentCommentAdd._failure = true;
                studentCommentAdd._message = TOKENINVALID;
            }
            return studentCommentAdd;
        }
        /// <summary>
        /// Update StudentComment
        /// </summary>
        /// <param name="studentCommentAddViewModel"></param>
        /// <returns></returns>
        public StudentCommentAddViewModel UpdateStudentComment(StudentCommentAddViewModel studentCommentAddViewModel)
        {
            StudentCommentAddViewModel studentCommentUpdate = new StudentCommentAddViewModel();
            if (TokenManager.CheckToken(studentCommentAddViewModel._tenantName + studentCommentAddViewModel._userName, studentCommentAddViewModel._token))
            {
                studentCommentUpdate = this.studentRepository.UpdateStudentComment(studentCommentAddViewModel);
            }
            else
            {
                studentCommentUpdate._failure = true;
                studentCommentUpdate._message = TOKENINVALID;
            }
            return studentCommentUpdate;
        }
        /// <summary>
        /// GetAll StudentCommentsList
        /// </summary>
        /// <param name="studentCommentListViewModel"></param>
        /// <returns></returns>
        public StudentCommentListViewModel GetAllStudentCommentsList(StudentCommentListViewModel studentCommentListViewModel)
        {
            StudentCommentListViewModel studentCommentsList = new StudentCommentListViewModel();
            if (TokenManager.CheckToken(studentCommentListViewModel._tenantName + studentCommentListViewModel._userName, studentCommentListViewModel._token))
            {
                studentCommentsList = this.studentRepository.GetAllStudentCommentsList(studentCommentListViewModel);
            }
            else
            {
                studentCommentsList._failure = true;
                studentCommentsList._message = TOKENINVALID;
            }
            return studentCommentsList;
        }
        /// <summary>
        /// Delete StudentComment
        /// </summary>
        /// <param name="studentCommentAddViewModel"></param>
        /// <returns></returns>
        public StudentCommentAddViewModel DeleteStudentComment(StudentCommentAddViewModel studentCommentAddViewModel)
        {
            StudentCommentAddViewModel studentCommentDelete = new StudentCommentAddViewModel();
            if (TokenManager.CheckToken(studentCommentAddViewModel._tenantName + studentCommentAddViewModel._userName, studentCommentAddViewModel._token))
            {
                studentCommentDelete = this.studentRepository.DeleteStudentComment(studentCommentAddViewModel);
            }
            else
            {
                studentCommentDelete._failure = true;
                studentCommentDelete._message = TOKENINVALID;
            }
            return studentCommentDelete;
        }

        ///// <summary>
        ///// Student View By Id
        ///// </summary>
        ///// <param name="student"></param>
        ///// <returns></returns>
        //public StudentAddViewModel ViewStudent(StudentAddViewModel student)
        //{
        //    StudentAddViewModel studentView = new StudentAddViewModel();
        //    if (TokenManager.CheckToken(student._tenantName, student._token))
        //    {
        /// <summary>
        /// Student View By Id
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public StudentAddViewModel ViewStudent(StudentAddViewModel student)
        {
            StudentAddViewModel studentView = new StudentAddViewModel();
            if (TokenManager.CheckToken(student._tenantName + student._userName, student._token))
            {

                studentView = this.studentRepository.ViewStudent(student);
            }
            else
            {
                studentView._failure = true;
                studentView._message = TOKENINVALID;
            }
            return studentView;
        }

        //public StudentAddViewModel DeleteStudent(StudentAddViewModel student)
        //{
        //    StudentAddViewModel studentDelete = new StudentAddViewModel();
        //    try
        //    {
        //        if (TokenManager.CheckToken(student._tenantName, student._token))
        //        {
        //            studentDelete = this.studentRepository.DeleteStudent(student);
        //        }
        //        else
        //        {
        //            studentDelete._failure = true;
        //            studentDelete._message = TOKENINVALID;
        //        }
        //    }
        //    catch (Exception es)
        //    {
        //        studentDelete._failure = true;
        //        studentDelete._message = es.Message;
        //    }

        //    return studentDelete;
        //}

        /// <summary>
        /// Search Sibling For Student
        /// </summary>
        /// <param name="studentSiblingListViewModel"></param>
        /// <returns></returns>

        public SiblingSearchForStudentListModel SearchSiblingForStudent(SiblingSearchForStudentListModel studentSiblingListViewModel)
        {
            SiblingSearchForStudentListModel studentSiblingList = new SiblingSearchForStudentListModel();
            if (TokenManager.CheckToken(studentSiblingListViewModel._tenantName + studentSiblingListViewModel._userName, studentSiblingListViewModel._token))
            {
                studentSiblingList = this.studentRepository.SearchSiblingForStudent(studentSiblingListViewModel);
            }
            else
            {
                studentSiblingList._failure = true;
                studentSiblingList._message = TOKENINVALID;
            }
            return studentSiblingList;
        }

        /// <summary>
        /// Association Sibling
        /// </summary>
        /// <param name="siblingAddUpdateForStudentModel"></param>
        /// <returns></returns>
        public SiblingAddUpdateForStudentModel AssociationSibling(SiblingAddUpdateForStudentModel siblingAddUpdateForStudentModel)
        {
            SiblingAddUpdateForStudentModel siblingAddUpdateForStudent = new SiblingAddUpdateForStudentModel();
            try
            {
                if (TokenManager.CheckToken(siblingAddUpdateForStudentModel._tenantName + siblingAddUpdateForStudentModel._userName, siblingAddUpdateForStudentModel._token))
                {
                    siblingAddUpdateForStudent = this.studentRepository.AssociationSibling(siblingAddUpdateForStudentModel);
                }
                else
                {
                    siblingAddUpdateForStudent._failure = true;
                    siblingAddUpdateForStudent._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                siblingAddUpdateForStudent._failure = true;
                siblingAddUpdateForStudent._message = es.Message;
            }
            return siblingAddUpdateForStudent;
        }

        /// <summary>
        /// View All Sibling
        /// </summary>
        /// <param name="studentListModel"></param>
        /// <returns></returns>
        public StudentListModel ViewAllSibling(StudentListModel studentListModel)
        {
            logger.Info("Method ViewSibling called.");
            StudentListModel studentList = new StudentListModel();
            try
            {
                if (TokenManager.CheckToken(studentListModel._tenantName + studentListModel._userName, studentListModel._token))
                {
                    studentList = this.studentRepository.ViewAllSibling(studentListModel);
                    studentList._message = SUCCESS;
                    studentList._failure = false;
                    logger.Info("Method ViewSibling end with success.");
                }

                else
                {
                    studentList._failure = true;
                    studentList._message = TOKENINVALID;
                    return studentList;
                }
            }
            catch (Exception ex)
            {
                studentList._message = ex.Message;
                studentList._failure = true;
                logger.Error("Method getAllStudent end with error :" + ex.Message);
            }

            return studentList;
        }

        /// <summary>
        /// Remove Sibling
        /// </summary>
        /// <param name="siblingAddUpdateForStudentModel"></param>
        /// <returns></returns>
        public SiblingAddUpdateForStudentModel RemoveSibling(SiblingAddUpdateForStudentModel siblingAddUpdateForStudentModel)
        {
            SiblingAddUpdateForStudentModel associationshipDelete = new SiblingAddUpdateForStudentModel();
            try
            {
                if (TokenManager.CheckToken(siblingAddUpdateForStudentModel._tenantName + siblingAddUpdateForStudentModel._userName, siblingAddUpdateForStudentModel._token))
                {
                    associationshipDelete = this.studentRepository.RemoveSibling(siblingAddUpdateForStudentModel);
                }
                else
                {
                    associationshipDelete._failure = true;
                    associationshipDelete._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                associationshipDelete._failure = true;
                associationshipDelete._message = es.Message;
            }
            return associationshipDelete;
        }

        /// <summary>
        ///  Check Student InternalId Exist or Not
        /// </summary>
        /// <param name="checkStudentInternalIdViewModel"></param>
        /// <returns></returns>
        public CheckStudentInternalIdViewModel CheckStudentInternalId(CheckStudentInternalIdViewModel checkStudentInternalIdViewModel)
        {
            CheckStudentInternalIdViewModel checkInternalId = new CheckStudentInternalIdViewModel();
            if (TokenManager.CheckToken(checkStudentInternalIdViewModel._tenantName + checkStudentInternalIdViewModel._userName, checkStudentInternalIdViewModel._token))
            {
                checkInternalId = this.studentRepository.CheckStudentInternalId(checkStudentInternalIdViewModel);
            }
            else
            {
                checkInternalId._failure = true;
                checkInternalId._message = TOKENINVALID;
            }
            return checkInternalId;
        }
        /// <summary>
        /// Add Student Enrollment
        /// </summary>
        /// <param name="studentEnrollmentAddViewModel"></param>
        /// <returns></returns>
        public StudentEnrollmentListModel AddStudentEnrollment(StudentEnrollmentListModel studentEnrollmentListModel)
        {
            StudentEnrollmentListModel studentEnrollmentAddModel = new StudentEnrollmentListModel();
            if (TokenManager.CheckToken(studentEnrollmentListModel._tenantName + studentEnrollmentListModel._userName, studentEnrollmentListModel._token))
            {
                studentEnrollmentAddModel = this.studentRepository.AddStudentEnrollment(studentEnrollmentListModel);
            }
            else
            {
                studentEnrollmentAddModel._failure = true;
                studentEnrollmentAddModel._message = TOKENINVALID;
            }
            return studentEnrollmentAddModel;
        }
        /// <summary>
        /// Get All Student Enrollment
        /// </summary>
        /// <param name="studentEnrollmentListModel"></param>
        /// <returns></returns>
        public StudentEnrollmentListViewModel GetAllStudentEnrollment(StudentEnrollmentListViewModel studentEnrollmentListModel)
        {
            StudentEnrollmentListViewModel studentEnrollmentListView = new StudentEnrollmentListViewModel();
            try
            {
                if (TokenManager.CheckToken(studentEnrollmentListModel._tenantName + studentEnrollmentListModel._userName, studentEnrollmentListModel._token))
                {
                    studentEnrollmentListView = this.studentRepository.GetAllStudentEnrollment(studentEnrollmentListModel);
                }
                else
                {
                    studentEnrollmentListView._failure = true;
                    studentEnrollmentListView._message = TOKENINVALID;
                }
            }
            catch (Exception es)
            {
                studentEnrollmentListView._failure = true;
                studentEnrollmentListView._message = es.Message;
            }

            return studentEnrollmentListView;
        }
        /// <summary>
        /// Update Student Enrollment
        /// </summary>
        /// <param name="studentEnrollmentListModel"></param>
        /// <returns></returns>
        public StudentEnrollmentListModel UpdateStudentEnrollment(StudentEnrollmentListModel studentEnrollmentListModel)
        {
            StudentEnrollmentListModel studentEnrollmentUpdate = new StudentEnrollmentListModel();
            if (TokenManager.CheckToken(studentEnrollmentListModel._tenantName + studentEnrollmentListModel._userName, studentEnrollmentListModel._token))
            {

                studentEnrollmentUpdate = this.studentRepository.UpdateStudentEnrollment(studentEnrollmentListModel);
            }
            else
            {
                studentEnrollmentUpdate._failure = true;
                studentEnrollmentUpdate._message = TOKENINVALID;
            }
            return studentEnrollmentUpdate;
        }

        /// <summary>
        /// Add or Update Student Photo
        /// </summary>
        /// <param name="studentAddViewModel"></param>
        /// <returns></returns>
        public StudentAddViewModel AddUpdateStudentPhoto(StudentAddViewModel studentAddViewModel)
        {
            StudentAddViewModel studentPhotoUpdate = new StudentAddViewModel();
            if (TokenManager.CheckToken(studentAddViewModel._tenantName + studentAddViewModel._userName, studentAddViewModel._token))
            {
                studentPhotoUpdate = this.studentRepository.AddUpdateStudentPhoto(studentAddViewModel);
            }
            else
            {
                studentPhotoUpdate._failure = true;
                studentPhotoUpdate._message = TOKENINVALID;
            }
            return studentPhotoUpdate;
        }

        //public SearchStudentViewModel SearchStudentForSchedule(SearchStudentViewModel searchStudentViewModel)
        //{
        //    SearchStudentViewModel searchStudentView = new SearchStudentViewModel();
        //    if (TokenManager.CheckToken(searchStudentViewModel._tenantName, searchStudentViewModel._token))
        //    {
        //        searchStudentView = this.studentRepository.SearchStudentForSchedule(searchStudentViewModel);
        //    }
        //    else
        //    {
        //        searchStudentView._failure = true;
        //        searchStudentView._message = TOKENINVALID;
        //    }
        //    return searchStudentView;
        //}

        /// <summary>
        /// Search Student List For Reenroll
        /// </summary>
        /// <param name="pageResult"></param>
        /// <returns></returns>
        public StudentListModel SearchStudentListForReenroll(PageResult pageResult)
        {
            logger.Info("Method searchStudentForReenroll called.");
            StudentListModel studentList = new StudentListModel();
            try
            {
                if (TokenManager.CheckToken(pageResult._tenantName + pageResult._userName, pageResult._token))
                {
                    studentList = this.studentRepository.SearchStudentListForReenroll(pageResult);
                    if (studentList.studentMaster.Count > 0)
                    {
                        studentList._message = SUCCESS;
                        studentList._failure = false;
                    }
                    else
                    {
                        studentList._message = "NO RECORD FOUND";
                        studentList._failure = true;
                    }
                    logger.Info("Method searchStudentForReenroll end with success.");
                }
                else
                {
                    studentList._failure = true;
                    studentList._message = TOKENINVALID;
                    return studentList;
                }
            }
            catch (Exception ex)
            {
                studentList._message = ex.Message;
                studentList._failure = true;
                logger.Error("Method searchStudentForReenroll end with error :" + ex.Message);
            }
            return studentList;
        }

        /// <summary>
        /// Re enrollment For Student
        /// </summary>
        /// <param name="studentListModel"></param>
        /// <returns></returns>
        public StudentListModel ReenrollmentForStudent(StudentListModel studentListModel)
        {
            StudentListModel studentReenrollment = new StudentListModel();
            if (TokenManager.CheckToken(studentListModel._tenantName + studentListModel._userName, studentListModel._token))
            {

                studentReenrollment = this.studentRepository.ReenrollmentForStudent(studentListModel);
            }
            else
            {
                studentReenrollment._failure = true;
                studentReenrollment._message = TOKENINVALID;
            }
            return studentReenrollment;
        }
    }
}
