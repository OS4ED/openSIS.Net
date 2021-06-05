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

using opensis.core.helper;
using opensis.core.Student.Interfaces;
using opensis.data.Interface;
using opensis.data.Models;
using opensis.data.ViewModels.Student;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace opensis.core.Student.Services
{
    public class StudentService : IStudentService
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

        /// <summary>
        /// Add Student List
        /// </summary>
        /// <param name="studentListAddViewModel"></param>
        /// <returns></returns>
        public StudentListAddViewModel AddStudentList(StudentListAddViewModel studentListAddViewModel)
        {
            StudentListAddViewModel studentListAdd = new StudentListAddViewModel();
            if (TokenManager.CheckToken(studentListAddViewModel._tenantName + studentListAddViewModel._userName, studentListAddViewModel._token))
            {
                studentListAdd = this.studentRepository.AddStudentList(studentListAddViewModel);
            }
            else
            {
                studentListAdd._failure = true;
                studentListAdd._message = TOKENINVALID;
            }
            return studentListAdd;
        }

        /// <summary>
        /// Generate Transcript For Student
        /// </summary>
        /// <param name="transcriptViewModel"></param>
        /// <returns></returns>
        public TranscriptViewModel TranscriptViewForStudent(TranscriptViewModel transcriptViewModel)
        {
            TranscriptViewModel transcriptView = new TranscriptViewModel();
            if (TokenManager.CheckToken(transcriptViewModel._tenantName + transcriptViewModel._userName, transcriptViewModel._token))
            {
                transcriptView = this.studentRepository.TranscriptViewForStudent(transcriptViewModel);
            }
            else
            {
                transcriptView._failure = true;
                transcriptView._message = TOKENINVALID;
            }
            return transcriptView;
        }

        /// <summary>
        /// Add Transcript For Students
        /// </summary>
        /// <param name="transcriptAddViewModel"></param>
        /// <returns></returns>
        public TranscriptAddViewModel AddTranscriptForStudent(TranscriptAddViewModel transcriptAddViewModel)
        {
            TranscriptAddViewModel transcriptAdd = new TranscriptAddViewModel();
            if (TokenManager.CheckToken(transcriptAddViewModel._tenantName + transcriptAddViewModel._userName, transcriptAddViewModel._token))
            {
                transcriptAdd = this.studentRepository.AddTranscriptForStudent(transcriptAddViewModel);
            }
            else
            {
                transcriptAdd._failure = true;
                transcriptAdd._message = TOKENINVALID;
            }
            return transcriptAdd;
        }

        /// <summary>
        /// Generate Pdf Transcript For Student
        /// </summary>
        /// <param name="transcriptAddViewModel"></param>
        /// <returns></returns>
        public async Task<TranscriptAddViewModel> GenerateTranscriptForStudent(TranscriptAddViewModel transcriptAddViewModel)
        {
            TranscriptAddViewModel transcriptAdd = new TranscriptAddViewModel();
            if (TokenManager.CheckToken(transcriptAddViewModel._tenantName + transcriptAddViewModel._userName, transcriptAddViewModel._token))
            {
                transcriptAdd = await this.studentRepository.GenerateTranscriptForStudent(transcriptAddViewModel);
            }
            else
            {
                transcriptAdd._failure = true;
                transcriptAdd._message = TOKENINVALID;
            }
            return transcriptAdd;
        }

        /// <summary>
        /// Add Student Medical Alert
        /// </summary>
        /// <param name="studentMedicalAlertAddViewModel"></param>
        /// <returns></returns>
        public StudentMedicalAlertAddViewModel AddStudentMedicalAlert(StudentMedicalAlertAddViewModel studentMedicalAlertAddViewModel)
        {
            StudentMedicalAlertAddViewModel studentMedicalAlertAdd = new StudentMedicalAlertAddViewModel();
            if (TokenManager.CheckToken(studentMedicalAlertAddViewModel._tenantName + studentMedicalAlertAddViewModel._userName, studentMedicalAlertAddViewModel._token))
            {
                studentMedicalAlertAdd = this.studentRepository.AddStudentMedicalAlert(studentMedicalAlertAddViewModel);
            }
            else
            {
                studentMedicalAlertAdd._failure = true;
                studentMedicalAlertAdd._message = TOKENINVALID;
            }
            return studentMedicalAlertAdd;
        }

        /// <summary>
        /// Update Student Medical Alert
        /// </summary>
        /// <param name="studentMedicalAlertAddViewModel"></param>
        /// <returns></returns>
        public StudentMedicalAlertAddViewModel UpdateStudentMedicalAlert(StudentMedicalAlertAddViewModel studentMedicalAlertAddViewModel)
        {
            StudentMedicalAlertAddViewModel studentMedicalAlertUpdate = new StudentMedicalAlertAddViewModel();
            if (TokenManager.CheckToken(studentMedicalAlertAddViewModel._tenantName + studentMedicalAlertAddViewModel._userName, studentMedicalAlertAddViewModel._token))
            {
                studentMedicalAlertUpdate = this.studentRepository.UpdateStudentMedicalAlert(studentMedicalAlertAddViewModel);
            }
            else
            {
                studentMedicalAlertUpdate._failure = true;
                studentMedicalAlertUpdate._message = TOKENINVALID;
            }
            return studentMedicalAlertUpdate;
        }

        /// <summary>
        /// Delete Student Medical Alert
        /// </summary>
        /// <param name="studentMedicalAlertAddViewModel"></param>
        /// <returns></returns>
        public StudentMedicalAlertAddViewModel DeleteStudentMedicalAlert(StudentMedicalAlertAddViewModel studentMedicalAlertAddViewModel)
        {
            StudentMedicalAlertAddViewModel studentMedicalAlertdelete = new StudentMedicalAlertAddViewModel();
            if (TokenManager.CheckToken(studentMedicalAlertAddViewModel._tenantName + studentMedicalAlertAddViewModel._userName, studentMedicalAlertAddViewModel._token))
            {
                studentMedicalAlertdelete = this.studentRepository.DeleteStudentMedicalAlert(studentMedicalAlertAddViewModel);
            }
            else
            {
                studentMedicalAlertdelete._failure = true;
                studentMedicalAlertdelete._message = TOKENINVALID;
            }
            return studentMedicalAlertdelete;
        }

        /// <summary>
        /// Add Student Medical Note
        /// </summary>
        /// <param name="studentMedicalNoteAddViewModel"></param>
        /// <returns></returns>
        public StudentMedicalNoteAddViewModel AddStudentMedicalNote(StudentMedicalNoteAddViewModel studentMedicalNoteAddViewModel)
        {
            StudentMedicalNoteAddViewModel studentMedicalNoteAdd = new StudentMedicalNoteAddViewModel();
            if (TokenManager.CheckToken(studentMedicalNoteAddViewModel._tenantName + studentMedicalNoteAddViewModel._userName, studentMedicalNoteAddViewModel._token))
            {
                studentMedicalNoteAdd = this.studentRepository.AddStudentMedicalNote(studentMedicalNoteAddViewModel);
            }
            else
            {
                studentMedicalNoteAdd._failure = true;
                studentMedicalNoteAdd._message = TOKENINVALID;
            }
            return studentMedicalNoteAdd;
        }

        /// <summary>
        /// Update Student Medical Note
        /// </summary>
        /// <param name="studentMedicalNoteAddViewModel"></param>
        /// <returns></returns>
        public StudentMedicalNoteAddViewModel UpdateStudentMedicalNote(StudentMedicalNoteAddViewModel studentMedicalNoteAddViewModel)
        {
            StudentMedicalNoteAddViewModel studentMedicalNoteUpdate = new StudentMedicalNoteAddViewModel();
            if (TokenManager.CheckToken(studentMedicalNoteAddViewModel._tenantName + studentMedicalNoteAddViewModel._userName, studentMedicalNoteAddViewModel._token))
            {
                studentMedicalNoteUpdate = this.studentRepository.UpdateStudentMedicalNote(studentMedicalNoteAddViewModel);
            }
            else
            {
                studentMedicalNoteUpdate._failure = true;
                studentMedicalNoteUpdate._message = TOKENINVALID;
            }
            return studentMedicalNoteUpdate;
        }

        /// <summary>
        /// Delete Student Medical Note
        /// </summary>
        /// <param name="studentMedicalNoteAddViewModel"></param>
        /// <returns></returns>
        public StudentMedicalNoteAddViewModel DeleteStudentMedicalNote(StudentMedicalNoteAddViewModel studentMedicalNoteAddViewModel)
        {
            StudentMedicalNoteAddViewModel studentMedicalNoteDelete = new StudentMedicalNoteAddViewModel();
            if (TokenManager.CheckToken(studentMedicalNoteAddViewModel._tenantName + studentMedicalNoteAddViewModel._userName, studentMedicalNoteAddViewModel._token))
            {
                studentMedicalNoteDelete = this.studentRepository.DeleteStudentMedicalNote(studentMedicalNoteAddViewModel);
            }
            else
            {
                studentMedicalNoteDelete._failure = true;
                studentMedicalNoteDelete._message = TOKENINVALID;
            }
            return studentMedicalNoteDelete;
        }

        /// <summary>
        /// Add Student Medical Immunization
        /// </summary>
        /// <param name="studentMedicalImmunizationAddViewModel"></param>
        /// <returns></returns>
        public StudentMedicalImmunizationAddViewModel AddStudentMedicalImmunization(StudentMedicalImmunizationAddViewModel studentMedicalImmunizationAddViewModel)
        {
            StudentMedicalImmunizationAddViewModel studentMedicalImmunizationAdd = new StudentMedicalImmunizationAddViewModel();
            if (TokenManager.CheckToken(studentMedicalImmunizationAddViewModel._tenantName + studentMedicalImmunizationAddViewModel._userName, studentMedicalImmunizationAddViewModel._token))
            {
                studentMedicalImmunizationAdd = this.studentRepository.AddStudentMedicalImmunization(studentMedicalImmunizationAddViewModel);
            }
            else
            {
                studentMedicalImmunizationAdd._failure = true;
                studentMedicalImmunizationAdd._message = TOKENINVALID;
            }
            return studentMedicalImmunizationAdd;
        }

        /// <summary>
        /// Update Student Medical Immunization
        /// </summary>
        /// <param name="studentMedicalImmunizationAddViewModel"></param>
        /// <returns></returns>
        public StudentMedicalImmunizationAddViewModel UpdateStudentMedicalImmunization(StudentMedicalImmunizationAddViewModel studentMedicalImmunizationAddViewModel)
        {
            StudentMedicalImmunizationAddViewModel studentMedicalImmunizationUpdate = new StudentMedicalImmunizationAddViewModel();
            if (TokenManager.CheckToken(studentMedicalImmunizationAddViewModel._tenantName + studentMedicalImmunizationAddViewModel._userName, studentMedicalImmunizationAddViewModel._token))
            {
                studentMedicalImmunizationUpdate = this.studentRepository.UpdateStudentMedicalImmunization(studentMedicalImmunizationAddViewModel);
            }
            else
            {
                studentMedicalImmunizationUpdate._failure = true;
                studentMedicalImmunizationUpdate._message = TOKENINVALID;
            }
            return studentMedicalImmunizationUpdate;
        }

        /// <summary>
        /// Delete Student Medical Immunization
        /// </summary>
        /// <param name="studentMedicalImmunizationAddViewModel"></param>
        /// <returns></returns>
        public StudentMedicalImmunizationAddViewModel DeleteStudentMedicalImmunization(StudentMedicalImmunizationAddViewModel studentMedicalImmunizationAddViewModel)
        {
            StudentMedicalImmunizationAddViewModel studentMedicalImmunizationDelete = new StudentMedicalImmunizationAddViewModel();
            if (TokenManager.CheckToken(studentMedicalImmunizationAddViewModel._tenantName + studentMedicalImmunizationAddViewModel._userName, studentMedicalImmunizationAddViewModel._token))
            {
                studentMedicalImmunizationDelete = this.studentRepository.DeleteStudentMedicalImmunization(studentMedicalImmunizationAddViewModel);
            }
            else
            {
                studentMedicalImmunizationDelete._failure = true;
                studentMedicalImmunizationDelete._message = TOKENINVALID;
            }
            return studentMedicalImmunizationDelete;
        }

        /// <summary>
        /// Add Student Medical Nurse Visit
        /// </summary>
        /// <param name="studentMedicalNurseVisitAddViewModel"></param>
        /// <returns></returns>
        public StudentMedicalNurseVisitAddViewModel AddStudentMedicalNurseVisit(StudentMedicalNurseVisitAddViewModel studentMedicalNurseVisitAddViewModel)
        {
            StudentMedicalNurseVisitAddViewModel studentMedicalNurseVisitAdd = new StudentMedicalNurseVisitAddViewModel();
            if (TokenManager.CheckToken(studentMedicalNurseVisitAddViewModel._tenantName + studentMedicalNurseVisitAddViewModel._userName, studentMedicalNurseVisitAddViewModel._token))
            {
                studentMedicalNurseVisitAdd = this.studentRepository.AddStudentMedicalNurseVisit(studentMedicalNurseVisitAddViewModel);
            }
            else
            {
                studentMedicalNurseVisitAdd._failure = true;
                studentMedicalNurseVisitAdd._message = TOKENINVALID;
            }
            return studentMedicalNurseVisitAdd;
        }

        /// <summary>
        /// Update Student Medical Nurse Visit
        /// </summary>
        /// <param name="studentMedicalNurseVisitAddViewModel"></param>
        /// <returns></returns>
        public StudentMedicalNurseVisitAddViewModel UpdateStudentMedicalNurseVisit(StudentMedicalNurseVisitAddViewModel studentMedicalNurseVisitAddViewModel)
        {
            StudentMedicalNurseVisitAddViewModel studentMedicalNurseVisitUpdate = new StudentMedicalNurseVisitAddViewModel();
            if (TokenManager.CheckToken(studentMedicalNurseVisitAddViewModel._tenantName + studentMedicalNurseVisitAddViewModel._userName, studentMedicalNurseVisitAddViewModel._token))
            {
                studentMedicalNurseVisitUpdate = this.studentRepository.UpdateStudentMedicalNurseVisit(studentMedicalNurseVisitAddViewModel);
            }
            else
            {
                studentMedicalNurseVisitUpdate._failure = true;
                studentMedicalNurseVisitUpdate._message = TOKENINVALID;
            }
            return studentMedicalNurseVisitUpdate;
        }

        /// <summary>
        /// Delete Student Medical Nurse Visit
        /// </summary>
        /// <param name="studentMedicalNurseVisitAddViewModel"></param>
        /// <returns></returns>
        public StudentMedicalNurseVisitAddViewModel DeleteStudentMedicalNurseVisit(StudentMedicalNurseVisitAddViewModel studentMedicalNurseVisitAddViewModel)
        {
            StudentMedicalNurseVisitAddViewModel studentMedicalNurseVisitDelete = new StudentMedicalNurseVisitAddViewModel();
            if (TokenManager.CheckToken(studentMedicalNurseVisitAddViewModel._tenantName + studentMedicalNurseVisitAddViewModel._userName, studentMedicalNurseVisitAddViewModel._token))
            {
                studentMedicalNurseVisitDelete = this.studentRepository.DeleteStudentMedicalNurseVisit(studentMedicalNurseVisitAddViewModel);
            }
            else
            {
                studentMedicalNurseVisitDelete._failure = true;
                studentMedicalNurseVisitDelete._message = TOKENINVALID;
            }
            return studentMedicalNurseVisitDelete;
        }

        /// <summary>
        /// Add Student Medical Provider
        /// </summary>
        /// <param name="studentMedicalProviderAddViewModel"></param>
        /// <returns></returns>
        public StudentMedicalProviderAddViewModel AddStudentMedicalProvider(StudentMedicalProviderAddViewModel studentMedicalProviderAddViewModel)
        {
            StudentMedicalProviderAddViewModel studentMedicalProviderAdd = new StudentMedicalProviderAddViewModel();
            if (TokenManager.CheckToken(studentMedicalProviderAddViewModel._tenantName + studentMedicalProviderAddViewModel._userName, studentMedicalProviderAddViewModel._token))
            {
                studentMedicalProviderAdd = this.studentRepository.AddStudentMedicalProvider(studentMedicalProviderAddViewModel);
            }
            else
            {
                studentMedicalProviderAdd._failure = true;
                studentMedicalProviderAdd._message = TOKENINVALID;
            }
            return studentMedicalProviderAdd;
        }

        /// <summary>
        /// Update Student Medical Provider
        /// </summary>
        /// <param name="studentMedicalProviderAddViewModel"></param>
        /// <returns></returns>
        public StudentMedicalProviderAddViewModel UpdateStudentMedicalProvider(StudentMedicalProviderAddViewModel studentMedicalProviderAddViewModel)
        {
            StudentMedicalProviderAddViewModel studentMedicalProviderUpdate = new StudentMedicalProviderAddViewModel();
            if (TokenManager.CheckToken(studentMedicalProviderAddViewModel._tenantName + studentMedicalProviderAddViewModel._userName, studentMedicalProviderAddViewModel._token))
            {
                studentMedicalProviderUpdate = this.studentRepository.UpdateStudentMedicalProvider(studentMedicalProviderAddViewModel);
            }
            else
            {
                studentMedicalProviderUpdate._failure = true;
                studentMedicalProviderUpdate._message = TOKENINVALID;
            }
            return studentMedicalProviderUpdate;
        }

        /// <summary>
        /// Delete Student Medical Provider
        /// </summary>
        /// <param name="studentMedicalProviderAddViewModel"></param>
        /// <returns></returns>
        public StudentMedicalProviderAddViewModel DeleteStudentMedicalProvider(StudentMedicalProviderAddViewModel studentMedicalProviderAddViewModel)
        {
            StudentMedicalProviderAddViewModel studentMedicalProviderDelete = new StudentMedicalProviderAddViewModel();
            if (TokenManager.CheckToken(studentMedicalProviderAddViewModel._tenantName + studentMedicalProviderAddViewModel._userName, studentMedicalProviderAddViewModel._token))
            {
                studentMedicalProviderDelete = this.studentRepository.DeleteStudentMedicalProvider(studentMedicalProviderAddViewModel);
            }
            else
            {
                studentMedicalProviderDelete._failure = true;
                studentMedicalProviderDelete._message = TOKENINVALID;
            }
            return studentMedicalProviderDelete;
        }

        /// <summary>
        /// Get All Student Medical Info
        /// </summary>
        /// <param name="studentMedicalInfoViewModel"></param>
        /// <returns></returns>
        public StudentMedicalInfoViewModel GetAllStudentMedicalInfo(StudentMedicalInfoViewModel studentMedicalInfoViewModel)
        {
            StudentMedicalInfoViewModel studentMedicalInfoList = new StudentMedicalInfoViewModel();
            if (TokenManager.CheckToken(studentMedicalInfoViewModel._tenantName + studentMedicalInfoViewModel._userName, studentMedicalInfoViewModel._token))
            {
                studentMedicalInfoList = this.studentRepository.GetAllStudentMedicalInfo(studentMedicalInfoViewModel);
            }
            else
            {
                studentMedicalInfoList._failure = true;
                studentMedicalInfoList._message = TOKENINVALID;
            }
            return studentMedicalInfoList;
        }
    }
}
