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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using opensis.core.Student.Interfaces;
using opensis.data.Models;
using opensis.data.ViewModels.Student;

namespace opensisAPI.Controllers
{

    [EnableCors("AllowOrigin")]
    [Route("{tenant}/Student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost("addStudent")]
        public ActionResult<StudentAddViewModel> AddStudent(StudentAddViewModel student)
        {
            StudentAddViewModel studentAdd = new StudentAddViewModel();
            try
            {
                if (student.studentMaster.SchoolId > 0)
                {
                    studentAdd = _studentService.SaveStudent(student);
                }
                else
                {
                    studentAdd._token = student._token;
                    studentAdd._tenantName = student._tenantName;
                    studentAdd._failure = true;
                    studentAdd._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                studentAdd._failure = true;
                studentAdd._message = es.Message;
            }
            return studentAdd;
        }

        [HttpPut("updateStudent")]
        public ActionResult<StudentAddViewModel> UpdateStudent(StudentAddViewModel student)
        {
            StudentAddViewModel studentUpdate = new StudentAddViewModel();
            try
            {
                if (student.studentMaster.SchoolId > 0)
                {
                    studentUpdate = _studentService.UpdateStudent(student);
                }
                else
                {
                    studentUpdate._token = student._token;
                    studentUpdate._tenantName = student._tenantName;
                    studentUpdate._failure = true;
                    studentUpdate._message = "Please enter valid scholl id";
                }
            }
            catch (Exception es)
            {
                studentUpdate._failure = true;
                studentUpdate._message = es.Message;
            }
            return studentUpdate;
        }

        [HttpPost("getAllStudentList")]
        public ActionResult<StudentListModel> GetAllStudentList(PageResult pageResult)
        {
            StudentListModel studentList = new StudentListModel();
            try
            {
                studentList = _studentService.GetAllStudentList(pageResult);
            }
            catch (Exception es)
            {
                studentList._message = es.Message;
                studentList._failure = true;
            }
            return studentList;
        }

        [HttpPost("addStudentDocument")]
        public ActionResult<StudentDocumentAddViewModel> AddStudentDocument(StudentDocumentAddViewModel studentDocumentAddViewModel)
        {
            StudentDocumentAddViewModel studentDocoumentAdd = new StudentDocumentAddViewModel();
            try
            {
                if (studentDocumentAddViewModel.studentDocuments.FirstOrDefault().SchoolId > 0)
                {
                    studentDocoumentAdd = _studentService.SaveStudentDocument(studentDocumentAddViewModel);
                }
                else
                {
                    studentDocoumentAdd._token = studentDocumentAddViewModel._token;
                    studentDocoumentAdd._tenantName = studentDocumentAddViewModel._tenantName;
                    studentDocoumentAdd._failure = true;
                    studentDocoumentAdd._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                studentDocoumentAdd._failure = true;
                studentDocoumentAdd._message = es.Message;
            }
            return studentDocoumentAdd;
        }

        [HttpPut("updateStudentDocument")]

        public ActionResult<StudentDocumentAddViewModel> UpdateStudentDocument(StudentDocumentAddViewModel studentDocumentAddViewModel)
        {
            StudentDocumentAddViewModel studentDocumentUpdate = new StudentDocumentAddViewModel();
            try
            {
                if (studentDocumentAddViewModel.studentDocuments.FirstOrDefault().SchoolId > 0)
                {
                    studentDocumentUpdate = _studentService.UpdateStudentDocument(studentDocumentAddViewModel);
                }
                else
                {
                    studentDocumentUpdate._token = studentDocumentAddViewModel._token;
                    studentDocumentUpdate._tenantName = studentDocumentAddViewModel._tenantName;
                    studentDocumentUpdate._failure = true;
                    studentDocumentUpdate._message = "Please enter valid scholl id";
                }
            }
            catch (Exception es)
            {
                studentDocumentUpdate._failure = true;
                studentDocumentUpdate._message = es.Message;
            }
            return studentDocumentUpdate;
        }


        [HttpPost("getAllStudentDocumentsList")]

        public ActionResult<StudentDocumentListViewModel> GetAllStudentDocumentsList(StudentDocumentListViewModel studentDocumentsListViewModel)
        {
            StudentDocumentListViewModel studentDocumentsList = new StudentDocumentListViewModel();
            try
            {
                studentDocumentsList = _studentService.GetAllStudentDocumentsList(studentDocumentsListViewModel);
            }
            catch (Exception es)
            {
                studentDocumentsList._message = es.Message;
                studentDocumentsList._failure = true;
            }
            return studentDocumentsList;
        }


        [HttpPost("deleteStudentDocument")]
        public ActionResult<StudentDocumentAddViewModel> DeleteStudentDocument(StudentDocumentAddViewModel studentDocumentAddViewModel)
        {
            StudentDocumentAddViewModel studentDocoumentdelete = new StudentDocumentAddViewModel();
            try
            {
                if (studentDocumentAddViewModel.studentDocuments.FirstOrDefault().SchoolId > 0)
                {
                    studentDocoumentdelete = _studentService.DeleteStudentDocument(studentDocumentAddViewModel);
                }
                else
                {
                    studentDocoumentdelete._token = studentDocumentAddViewModel._token;
                    studentDocoumentdelete._tenantName = studentDocumentAddViewModel._tenantName;
                    studentDocoumentdelete._failure = true;
                    studentDocoumentdelete._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                studentDocoumentdelete._failure = true;
                studentDocoumentdelete._message = es.Message;
            }
            return studentDocoumentdelete;
        }


        [HttpPost("addStudentLoginInfo")]
        public ActionResult<LoginInfoAddModel> AddStudentLoginInfo(LoginInfoAddModel login)
        {
            LoginInfoAddModel loginInfo = new LoginInfoAddModel();
            try
            {
                if (login.userMaster.SchoolId > 0)
                {
                    loginInfo = _studentService.AddStudentLoginInfo(login);
                }
                else
                {
                    loginInfo._token = login._token;
                    loginInfo._tenantName = login._tenantName;
                    loginInfo._failure = true;
                    loginInfo._message = "Please enter valid scholl id";
                }
            }
            catch (Exception es)
            {
                loginInfo._failure = true;
                loginInfo._message = es.Message;
            }
            return loginInfo;
        }


        [HttpPost("addStudentComment")]
        public ActionResult<StudentCommentAddViewModel> AddStudentComment(StudentCommentAddViewModel studentCommentAddViewModel)
        {
            StudentCommentAddViewModel studentCommentAdd = new StudentCommentAddViewModel();
            try
            {
                if (studentCommentAddViewModel.studentComments.SchoolId > 0)
                {
                    studentCommentAdd = _studentService.SaveStudentComment(studentCommentAddViewModel);
                }
                else
                {
                    studentCommentAdd._token = studentCommentAddViewModel._token;
                    studentCommentAdd._tenantName = studentCommentAddViewModel._tenantName;
                    studentCommentAdd._failure = true;
                    studentCommentAdd._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                studentCommentAdd._failure = true;
                studentCommentAdd._message = es.Message;
            }
            return studentCommentAdd;
        }

        [HttpPut("updateStudentComment")]

        public ActionResult<StudentCommentAddViewModel> UpdateStudentComment(StudentCommentAddViewModel studentCommentAddViewModel)
        {
            StudentCommentAddViewModel studentCommentUpdate = new StudentCommentAddViewModel();
            try
            {
                if (studentCommentAddViewModel.studentComments.SchoolId > 0)
                {
                    studentCommentUpdate = _studentService.UpdateStudentComment(studentCommentAddViewModel);
                }
                else
                {
                    studentCommentUpdate._token = studentCommentAddViewModel._token;
                    studentCommentUpdate._tenantName = studentCommentAddViewModel._tenantName;
                    studentCommentUpdate._failure = true;
                    studentCommentUpdate._message = "Please enter valid scholl id";
                }
            }
            catch (Exception es)
            {
                studentCommentUpdate._failure = true;
                studentCommentUpdate._message = es.Message;
            }
            return studentCommentUpdate;
        }


        [HttpPost("getAllStudentCommentsList")]

        public ActionResult<StudentCommentListViewModel> GetAllStudentCommentsList(StudentCommentListViewModel studentCommentListViewModel)
        {
            StudentCommentListViewModel studentCommentsList = new StudentCommentListViewModel();
            try
            {
                studentCommentsList = _studentService.GetAllStudentCommentsList(studentCommentListViewModel);
            }
            catch (Exception es)
            {
                studentCommentsList._message = es.Message;
                studentCommentsList._failure = true;
            }
            return studentCommentsList;
        }


        [HttpPost("deleteStudentComment")]
        public ActionResult<StudentCommentAddViewModel> DeleteStudentComment(StudentCommentAddViewModel studentCommentAddViewModel)
        {
            StudentCommentAddViewModel studentCommentDelete = new StudentCommentAddViewModel();
            try
            {
                if (studentCommentAddViewModel.studentComments.SchoolId > 0)
                {
                    studentCommentDelete = _studentService.DeleteStudentComment(studentCommentAddViewModel);
                }
                else
                {
                    studentCommentDelete._token = studentCommentAddViewModel._token;
                    studentCommentDelete._tenantName = studentCommentAddViewModel._tenantName;
                    studentCommentDelete._failure = true;
                    studentCommentDelete._message = "Please enter valid school id";
                }
            }
            catch (Exception es)
            {
                studentCommentDelete._failure = true;
                studentCommentDelete._message = es.Message;
            }
            return studentCommentDelete;
        }

        //[HttpPost("viewStudent")]
        [HttpPost("viewStudent")]

        public ActionResult<StudentAddViewModel> ViewStudent(StudentAddViewModel student)
        {
            StudentAddViewModel studentView = new StudentAddViewModel();
            try
            {
                if (student.studentMaster.SchoolId > 0)
                {
                    studentView = _studentService.ViewStudent(student);
                }
                else
                {
                    studentView._token = student._token;
                    studentView._tenantName = student._tenantName;
                    studentView._failure = true;
                    studentView._message = "Please enter valid scholl id";
                }
            }
            catch (Exception es)
            {
                studentView._failure = true;
                studentView._message = es.Message;
            }
            return studentView;
        }



        //[HttpPost("deleteStudent")]

        //public ActionResult<StudentAddViewModel> DeleteStudent(StudentAddViewModel student)
        //{
        //    StudentAddViewModel studentDelete = new StudentAddViewModel();
        //    try
        //    {
        //        if (student.studentMaster.SchoolId > 0)
        //        {
        //            studentDelete = _studentService.DeleteStudent(student);
        //        }
        //        else
        //        {
        //            studentDelete._token = student._token;
        //            studentDelete._tenantName = student._tenantName;
        //            studentDelete._failure = true;
        //            studentDelete._message = "Please enter valid scholl id";
        //        }
        //    }
        //    catch (Exception es)
        //    {
        //        studentDelete._failure = true;
        //        studentDelete._message = es.Message;
        //    }
        //    return studentDelete;
        //}


        [HttpPost("siblingSearch")]

        public ActionResult<SiblingSearchForStudentListModel> ViewStudentSiblingList(SiblingSearchForStudentListModel studentSiblingListViewModel)
        {
            SiblingSearchForStudentListModel studentSiblingsList = new SiblingSearchForStudentListModel();
            try
            {
                studentSiblingsList = _studentService.SearchSiblingForStudent(studentSiblingListViewModel);
            }
            catch (Exception es)
            {
                studentSiblingsList._message = es.Message;
                studentSiblingsList._failure = true;
            }
            return studentSiblingsList;
        }

        [HttpPost("associationSibling")]
        public ActionResult<SiblingAddUpdateForStudentModel> AssociationSibling(SiblingAddUpdateForStudentModel siblingAddUpdateForStudentModel)
        {
            SiblingAddUpdateForStudentModel siblingAddUpdateForStudent = new SiblingAddUpdateForStudentModel();
            try
            {
                siblingAddUpdateForStudent = _studentService.AssociationSibling(siblingAddUpdateForStudentModel);
            }
            catch (Exception es)
            {
                siblingAddUpdateForStudent._failure = true;
                siblingAddUpdateForStudent._message = es.Message;
            }
            return siblingAddUpdateForStudent;
        }

        [HttpPost("viewSibling")]

        public ActionResult<StudentListModel> ViewSibling(StudentListModel studentListModel)
        {
            StudentListModel studentList = new StudentListModel();
            try
            {
                studentList = _studentService.ViewAllSibling(studentListModel);
            }
            catch (Exception es)
            {
                studentList._message = es.Message;
                studentList._failure = true;
            }
            return studentList;
        }

        [HttpPost("removeSibling")]
        public ActionResult<SiblingAddUpdateForStudentModel> RemoveSibling(SiblingAddUpdateForStudentModel siblingAddUpdateForStudentModel)
        {
            SiblingAddUpdateForStudentModel siblingRemove = new SiblingAddUpdateForStudentModel();
            try
            {
                siblingRemove = _studentService.RemoveSibling(siblingAddUpdateForStudentModel);
            }
            catch (Exception es)
            {
                siblingRemove._message = es.Message;
                siblingRemove._failure = true;
            }
            return siblingRemove;
        }

        [HttpPost("checkStudentInternalId")]
        public ActionResult<CheckStudentInternalIdViewModel> CheckStudentInternalId(CheckStudentInternalIdViewModel checkStudentInternalIdViewModel)
        {
            CheckStudentInternalIdViewModel checkInternalId = new CheckStudentInternalIdViewModel();
            try
            {
                checkInternalId = _studentService.CheckStudentInternalId(checkStudentInternalIdViewModel);
            }
            catch (Exception es)
            {
                checkInternalId._message = es.Message;
                checkInternalId._failure = true;
            }
            return checkInternalId;
        }

        [HttpPost("addStudentEnrollment")]
        public ActionResult<StudentEnrollmentListModel> AddStudentEnrollment(StudentEnrollmentListModel studentEnrollmentListModel)
        {
            StudentEnrollmentListModel studentEnrollmentAdd = new StudentEnrollmentListModel();
            try
            {

                if (studentEnrollmentListModel.studentEnrollments.Count > 0)
                {
                    studentEnrollmentAdd = _studentService.AddStudentEnrollment(studentEnrollmentListModel);
                }
                else
                {
                    studentEnrollmentAdd._token = studentEnrollmentListModel._token;
                    studentEnrollmentAdd._tenantName = studentEnrollmentListModel._tenantName;
                    studentEnrollmentAdd._failure = true;                    
                }
            }
            catch (Exception es)
            {

                studentEnrollmentAdd._failure = true;
                studentEnrollmentAdd._message = es.Message;
            }
            return studentEnrollmentAdd;
        }
        [HttpPost("getAllStudentEnrollment")]

        public ActionResult<StudentEnrollmentListViewModel> GetAllStudentEnrollment(StudentEnrollmentListViewModel studentEnrollmentListModel)
        {
            StudentEnrollmentListViewModel studentEnrollmentList = new StudentEnrollmentListViewModel();
            try
            {
                    studentEnrollmentList = _studentService.GetAllStudentEnrollment(studentEnrollmentListModel);
            }
            catch (Exception es)
            {
                studentEnrollmentList._message = es.Message;
                studentEnrollmentList._failure = true;
            }
            return studentEnrollmentList;
        }

        [HttpPut("updateStudentEnrollment")]

        public ActionResult<StudentEnrollmentListModel> UpdateStudentEnrollment(StudentEnrollmentListModel studentEnrollmentListModel)
        {
            StudentEnrollmentListModel studentEnrollmentUpdate = new StudentEnrollmentListModel();
            try
            {
                studentEnrollmentUpdate = _studentService.UpdateStudentEnrollment(studentEnrollmentListModel);            
            }
            catch (Exception es)
            {
                studentEnrollmentUpdate._failure = true;
                studentEnrollmentUpdate._message = es.Message;
            }
            return studentEnrollmentUpdate;
        }

        [HttpPut("addUpdateStudentPhoto")]
        public ActionResult<StudentAddViewModel> AddUpdateStudentPhoto(StudentAddViewModel studentAddViewModel)
        {
            StudentAddViewModel studentPhotoUpdate = new StudentAddViewModel();
            try
            {
                studentPhotoUpdate = _studentService.AddUpdateStudentPhoto(studentAddViewModel);
            }
            catch (Exception es)
            {
                studentPhotoUpdate._failure = true;
                studentPhotoUpdate._message = es.Message;
            }
            return studentPhotoUpdate;
        }

        //[HttpPost("searchStudentForSchedule")]

        //public ActionResult<SearchStudentViewModel> SearchStudentForSchedule(SearchStudentViewModel searchStudentViewModel)
        //{
        //    SearchStudentViewModel searchStudentView = new SearchStudentViewModel();
        //    try
        //    {
        //        searchStudentView = _studentService.SearchStudentForSchedule(searchStudentViewModel);
        //    }
        //    catch (Exception es)
        //    {
        //        searchStudentView._message = es.Message;
        //        searchStudentView._failure = true;
        //    }
        //    return searchStudentView;
        //}

        [HttpPost("searchStudentListForReenroll")]
        public ActionResult<StudentListModel> SearchStudentListForReenroll(PageResult pageResult)
        {
            StudentListModel studentList = new StudentListModel();
            try
            {
                studentList = _studentService.SearchStudentListForReenroll(pageResult);
            }
            catch (Exception es)
            {
                studentList._message = es.Message;
                studentList._failure = true;
            }
            return studentList;
        }

        [HttpPost("reenrollmentForStudent")]
        public ActionResult<StudentListModel> ReenrollmentForStudent(StudentListModel studentListModel)
        {
            StudentListModel studentReenrollment = new StudentListModel();
            try
            {
                studentReenrollment = _studentService.ReenrollmentForStudent(studentListModel);
            }
            catch (Exception es)
            {
                studentReenrollment._message = es.Message;
                studentReenrollment._failure = true;
            }
            return studentReenrollment;
        }

        [HttpPost("addStudentList")]
        public ActionResult<StudentListAddViewModel> AddStudentList(StudentListAddViewModel studentListAddViewModel)
        {
            StudentListAddViewModel studentListAdd = new StudentListAddViewModel();
            try
            {
                studentListAdd = _studentService.AddStudentList(studentListAddViewModel);
            }
            catch (Exception es)
            {
                studentListAdd._failure = true;
                studentListAdd._message = es.Message;
            }
            return studentListAdd;
        }

        [HttpPost("transcriptViewForStudent")]
        public ActionResult<TranscriptViewModel> TranscriptViewForStudent(TranscriptViewModel transcriptViewModel)
        {
            TranscriptViewModel transcriptView = new TranscriptViewModel();
            try
            {
                transcriptView = _studentService.TranscriptViewForStudent(transcriptViewModel);
            }
            catch (Exception es)
            {
                transcriptView._failure = true;
                transcriptView._message = es.Message;
            }
            return transcriptView;
        }

        [HttpPost("addTranscriptForStudent")]
        public ActionResult<TranscriptAddViewModel> AddTranscriptForStudent(TranscriptAddViewModel transcriptAddViewModel)
        {
            TranscriptAddViewModel transcriptAdd = new TranscriptAddViewModel();
            try
            {
                transcriptAdd = _studentService.AddTranscriptForStudent(transcriptAddViewModel);
            }
            catch (Exception es)
            {
                transcriptAdd._failure = true;
                transcriptAdd._message = es.Message;
            }
            return transcriptAdd;
        }

        [HttpPost("generateTranscriptForStudent")]
        public async Task<TranscriptAddViewModel> GenerateTranscriptForStudent(TranscriptAddViewModel transcriptAddViewModel)
        {
            TranscriptAddViewModel transcriptAdd = new TranscriptAddViewModel();
            try
            {
                transcriptAdd =await _studentService.GenerateTranscriptForStudent(transcriptAddViewModel);
            }
            catch (Exception es)
            {
                transcriptAdd._failure = true;
                transcriptAdd._message = es.Message;
            }
            return transcriptAdd;
        }

        [HttpPost("addStudentMedicalAlert")]
        public ActionResult<StudentMedicalAlertAddViewModel> AddStudentMedicalAlert(StudentMedicalAlertAddViewModel studentMedicalAlertAddViewModel)
        {
            StudentMedicalAlertAddViewModel studentMedicalAlertAdd = new StudentMedicalAlertAddViewModel();
            try
            {
                studentMedicalAlertAdd =  _studentService.AddStudentMedicalAlert(studentMedicalAlertAddViewModel);
            }
            catch (Exception es)
            {
                studentMedicalAlertAdd._failure = true;
                studentMedicalAlertAdd._message = es.Message;
            }
            return studentMedicalAlertAdd;
        }

        [HttpPut("updateStudentMedicalAlert")]
        public ActionResult<StudentMedicalAlertAddViewModel> UpdateStudentMedicalAlert(StudentMedicalAlertAddViewModel studentMedicalAlertAddViewModel)
        {
            StudentMedicalAlertAddViewModel studentMedicalAlertUpdate = new StudentMedicalAlertAddViewModel();
            try
            {
                studentMedicalAlertUpdate = _studentService.UpdateStudentMedicalAlert(studentMedicalAlertAddViewModel);
            }
            catch (Exception es)
            {
                studentMedicalAlertUpdate._failure = true;
                studentMedicalAlertUpdate._message = es.Message;
            }
            return studentMedicalAlertUpdate;
        }

        [HttpPost("deleteStudentMedicalAlert")]
        public ActionResult<StudentMedicalAlertAddViewModel> DeleteStudentMedicalAlert(StudentMedicalAlertAddViewModel studentMedicalAlertAddViewModel)
        {
            StudentMedicalAlertAddViewModel studentMedicalAlertdelete = new StudentMedicalAlertAddViewModel();
            try
            {
                studentMedicalAlertdelete = _studentService.DeleteStudentMedicalAlert(studentMedicalAlertAddViewModel);
            }
            catch (Exception es)
            {
                studentMedicalAlertdelete._failure = true;
                studentMedicalAlertdelete._message = es.Message;
            }
            return studentMedicalAlertdelete;
        }

        [HttpPost("addStudentMedicalNote")]
        public ActionResult<StudentMedicalNoteAddViewModel> AddStudentMedicalNote(StudentMedicalNoteAddViewModel studentMedicalNoteAddViewModel)
        {
            StudentMedicalNoteAddViewModel studentMedicalNoteAdd = new StudentMedicalNoteAddViewModel();
            try
            {
                studentMedicalNoteAdd = _studentService.AddStudentMedicalNote(studentMedicalNoteAddViewModel);
            }
            catch (Exception es)
            {
                studentMedicalNoteAdd._failure = true;
                studentMedicalNoteAdd._message = es.Message;
            }
            return studentMedicalNoteAdd;
        }

        [HttpPut("updateStudentMedicalNote")]
        public ActionResult<StudentMedicalNoteAddViewModel> UpdateStudentMedicalNote(StudentMedicalNoteAddViewModel studentMedicalNoteAddViewModel)
        {
            StudentMedicalNoteAddViewModel studentMedicalNoteUpdate = new StudentMedicalNoteAddViewModel();
            try
            {
                studentMedicalNoteUpdate = _studentService.UpdateStudentMedicalNote(studentMedicalNoteAddViewModel);
            }
            catch (Exception es)
            {
                studentMedicalNoteUpdate._failure = true;
                studentMedicalNoteUpdate._message = es.Message;
            }
            return studentMedicalNoteUpdate;
        }

        [HttpPost("deleteStudentMedicalNote")]
        public ActionResult<StudentMedicalNoteAddViewModel> DeleteStudentMedicalNote(StudentMedicalNoteAddViewModel studentMedicalNoteAddViewModel)
        {
            StudentMedicalNoteAddViewModel studentMedicalNoteDelete = new StudentMedicalNoteAddViewModel();
            try
            {
                studentMedicalNoteDelete = _studentService.DeleteStudentMedicalNote(studentMedicalNoteAddViewModel);
            }
            catch (Exception es)
            {
                studentMedicalNoteDelete._failure = true;
                studentMedicalNoteDelete._message = es.Message;
            }
            return studentMedicalNoteDelete;
        }

        [HttpPost("addStudentMedicalImmunization")]
        public ActionResult<StudentMedicalImmunizationAddViewModel> AddStudentMedicalImmunization(StudentMedicalImmunizationAddViewModel studentMedicalImmunizationAddViewModel)
        {
            StudentMedicalImmunizationAddViewModel studentMedicalImmunizationAdd = new StudentMedicalImmunizationAddViewModel();
            try
            {
                studentMedicalImmunizationAdd = _studentService.AddStudentMedicalImmunization(studentMedicalImmunizationAddViewModel);
            }
            catch (Exception es)
            {
                studentMedicalImmunizationAdd._failure = true;
                studentMedicalImmunizationAdd._message = es.Message;
            }
            return studentMedicalImmunizationAdd;
        }

        [HttpPut("updateStudentMedicalImmunization")]
        public ActionResult<StudentMedicalImmunizationAddViewModel> UpdateStudentMedicalImmunization(StudentMedicalImmunizationAddViewModel studentMedicalImmunizationAddViewModel)
        {
            StudentMedicalImmunizationAddViewModel studentMedicalImmunizationUpdate = new StudentMedicalImmunizationAddViewModel();
            try
            {
                studentMedicalImmunizationUpdate = _studentService.UpdateStudentMedicalImmunization(studentMedicalImmunizationAddViewModel);
            }
            catch (Exception es)
            {
                studentMedicalImmunizationUpdate._failure = true;
                studentMedicalImmunizationUpdate._message = es.Message;
            }
            return studentMedicalImmunizationUpdate;
        }

        [HttpPost("deleteStudentMedicalImmunization")]
        public ActionResult<StudentMedicalImmunizationAddViewModel> DeleteStudentMedicalImmunization(StudentMedicalImmunizationAddViewModel studentMedicalImmunizationAddViewModel)
        {
            StudentMedicalImmunizationAddViewModel studentMedicalImmunizationDelete = new StudentMedicalImmunizationAddViewModel();
            try
            {
                studentMedicalImmunizationDelete = _studentService.DeleteStudentMedicalImmunization(studentMedicalImmunizationAddViewModel);
            }
            catch (Exception es)
            {
                studentMedicalImmunizationDelete._failure = true;
                studentMedicalImmunizationDelete._message = es.Message;
            }
            return studentMedicalImmunizationDelete;
        }

        [HttpPost("addStudentMedicalNurseVisit")]
        public ActionResult<StudentMedicalNurseVisitAddViewModel> AddStudentMedicalNurseVisit(StudentMedicalNurseVisitAddViewModel studentMedicalNurseVisitAddViewModel)
        {
            StudentMedicalNurseVisitAddViewModel studentMedicalNurseVisitAdd = new StudentMedicalNurseVisitAddViewModel();
            try
            {
                studentMedicalNurseVisitAdd = _studentService.AddStudentMedicalNurseVisit(studentMedicalNurseVisitAddViewModel);
            }
            catch (Exception es)
            {
                studentMedicalNurseVisitAdd._failure = true;
                studentMedicalNurseVisitAdd._message = es.Message;
            }
            return studentMedicalNurseVisitAdd;
        }

        [HttpPut("updateStudentMedicalNurseVisit")]
        public ActionResult<StudentMedicalNurseVisitAddViewModel> UpdateStudentMedicalNurseVisit(StudentMedicalNurseVisitAddViewModel studentMedicalNurseVisitAddViewModel)
        {
            StudentMedicalNurseVisitAddViewModel studentMedicalNurseVisitUpdate = new StudentMedicalNurseVisitAddViewModel();
            try
            {
                studentMedicalNurseVisitUpdate = _studentService.UpdateStudentMedicalNurseVisit(studentMedicalNurseVisitAddViewModel);
            }
            catch (Exception es)
            {
                studentMedicalNurseVisitUpdate._failure = true;
                studentMedicalNurseVisitUpdate._message = es.Message;
            }
            return studentMedicalNurseVisitUpdate;

        }

        [HttpPost("deleteStudentMedicalNurseVisit")]
        public ActionResult<StudentMedicalNurseVisitAddViewModel> DeleteStudentMedicalNurseVisit(StudentMedicalNurseVisitAddViewModel studentMedicalNurseVisitAddViewModel)
        {
            StudentMedicalNurseVisitAddViewModel studentMedicalNurseVisitDelete = new StudentMedicalNurseVisitAddViewModel();
            try
            {
                studentMedicalNurseVisitDelete = _studentService.DeleteStudentMedicalNurseVisit(studentMedicalNurseVisitAddViewModel);
            }
            catch (Exception es)
            {
                studentMedicalNurseVisitDelete._failure = true;
                studentMedicalNurseVisitDelete._message = es.Message;
            }
            return studentMedicalNurseVisitDelete;
        }

        [HttpPost("addStudentMedicalProvider")]
        public ActionResult<StudentMedicalProviderAddViewModel> AddStudentMedicalProvider(StudentMedicalProviderAddViewModel studentMedicalProviderAddViewModel)
        {
            StudentMedicalProviderAddViewModel studentMedicalProviderAdd = new StudentMedicalProviderAddViewModel();
            try
            {
                studentMedicalProviderAdd = _studentService.AddStudentMedicalProvider(studentMedicalProviderAddViewModel);
            }
            catch (Exception es)
            {
                studentMedicalProviderAdd._failure = true;
                studentMedicalProviderAdd._message = es.Message;
            }
            return studentMedicalProviderAdd;
        }

        [HttpPut("updateStudentMedicalProvider")]
        public ActionResult<StudentMedicalProviderAddViewModel> UpdateStudentMedicalProvider(StudentMedicalProviderAddViewModel studentMedicalProviderAddViewModel)
        {
            StudentMedicalProviderAddViewModel studentMedicalProviderUpdate = new StudentMedicalProviderAddViewModel();
            try
            {
                studentMedicalProviderUpdate = _studentService.UpdateStudentMedicalProvider(studentMedicalProviderAddViewModel);
            }
            catch (Exception es)
            {
                studentMedicalProviderUpdate._failure = true;
                studentMedicalProviderUpdate._message = es.Message;
            }
            return studentMedicalProviderUpdate;
        }

        [HttpPost("deleteStudentMedicalProvider")]
        public ActionResult<StudentMedicalProviderAddViewModel> DeleteStudentMedicalProvider(StudentMedicalProviderAddViewModel studentMedicalProviderAddViewModel)
        {
            StudentMedicalProviderAddViewModel studentMedicalProviderDelete = new StudentMedicalProviderAddViewModel();
            try
            {
                studentMedicalProviderDelete = _studentService.DeleteStudentMedicalProvider(studentMedicalProviderAddViewModel);
            }
            catch (Exception es)
            {
                studentMedicalProviderDelete._failure = true;
                studentMedicalProviderDelete._message = es.Message;
            }
            return studentMedicalProviderDelete;
        }

        [HttpPost("getAllStudentMedicalInfo")]
        public ActionResult<StudentMedicalInfoViewModel> GetAllStudentMedicalInfo(StudentMedicalInfoViewModel studentMedicalInfoViewModel)
        {
            StudentMedicalInfoViewModel studentMedicalInfoList = new StudentMedicalInfoViewModel();
            try
            {
                studentMedicalInfoList = _studentService.GetAllStudentMedicalInfo(studentMedicalInfoViewModel);
            }
            catch (Exception es)
            {
                studentMedicalInfoList._failure = true;
                studentMedicalInfoList._message = es.Message;
            }
            return studentMedicalInfoList;
        }
    }
}
